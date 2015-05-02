import os
from os.path import join, dirname, abspath
import sys
import string
import subprocess
from optparse import OptionParser

orig_pwd = abspath(os.path.curdir)
top_srcdir = dirname(dirname(abspath(sys.argv[0])))
trace_home = os.environ.get("LTTNG_TRACE_HOME", orig_pwd)

chunk = {
    "byte": 1,
    "word": 8,
    "page": 1 << 12,
    "huge": 1 << 20,
}

experiences = {
    "heap_nofill_huge": {
        "where": "heap",
        "block-size": "huge",
        "block-count": 100,
        "fill": False },
    "heap_fill_huge": {
        "where": "heap",
        "block-size": "huge",
        "block-count": 100,
        "fill": True },
}

# let's fix max-mem to be always 1000 * chunk size
def build_drmem_cmd(options, name, exp):
    cmd =  [ "lttng-simple", "-c", "-u", "-k" ]
    cmd += [ "-e", "mm" ]
    cmd += [ "--enable-libc-wrapper" ]
    cmd += [ "--stateless", "--name", name, "--" ]
    cmd += [ join(options.drmem_dir, "drmem") ]
    cmd += [ "--where", exp.get("where", "heap") ]
    bs = exp.get("block-size", "page")
    cmd += [ "--block-size", bs ]
    bc = exp.get("block-count", 100)
    # allocate at least 1MB
    max_mem = bc * chunk[bs]
    if (max_mem < (1 << 20)):
        max_mem = 1 << 20

    cmd += [ "--max-mem", str(max_mem) ]
    cmd += [ "--delay", "5" ]
    fill = exp.get("fill", True)
    if (fill): cmd += [ "--fill" ]
    trim = exp.get("trim", None)
    if (trim):
        cmd += [ "--trim", str(trim) ]
    return cmd

def build_kmem_cmd(name, output_dir):
    trace_dir = join(trace_home, name + "-k-u")
    jar_file = join(top_srcdir, "scripts", "lttng-kmem.jar")
    cmd = [ "java", "-jar", jar_file ]
    cmd += [ "--output", output_dir, trace_dir ]
    return cmd

def build_gnuplot_cmd(name, outputdir):
    cmd = [ "gnuplot", "data.gnuplot" ]
    return cmd;

def run_cmd(cmd):
    ret = subprocess.call(cmd)
    if ret != 0:
        raise RuntimeError("Command failed: " + string.join(cmd, " "))

def do_one(options, name, exp):
    output_dir = join(top_srcdir, "results", name)
    cmd1 = build_drmem_cmd(options, name, exp)
    cmd2 = build_kmem_cmd(name, output_dir)
    cmd3 = build_gnuplot_cmd(name, output_dir)
    
    print(cmd1)
    run_cmd(cmd1)
    
    print(cmd2)
    run_cmd(cmd2)
    
    print(cmd3)
    os.chdir(output_dir)
    run_cmd(cmd3)
    os.chdir(orig_pwd)

def which(name, flags=os.X_OK):
    result = []
    exts = filter(None, os.environ.get('PATHEXT', '').split(os.pathsep))
    path = os.environ.get('PATH', None)
    if path is None:
        return []
    for p in os.environ.get('PATH', '').split(os.pathsep):
        p = os.path.join(p, name)
        if os.access(p, flags):
            result.append(p)
        for e in exts:
            pext = p + e
            if os.access(pext, flags):
                result.append(pext)
    return result


usage = """usage: %prog [options] [experiment1, experiment2, ...]

Execute drmem experiments.
"""

if __name__=="__main__":
    parser = OptionParser(usage=usage)
    parser.add_option("--dry-run", dest="dry_run", default=False, action="store_true", help="display commands and do not execute them")
    parser.add_option("--list", dest="list", default=False, action="store_true", help="display available experiments")
    parser.add_option("--drmem-dir", dest="drmem_dir", default=join(top_srcdir, "drmem"), help="drmem directory")
    
    (options, args) = parser.parse_args()
    if (options.list):
        print("available experiences:")
        for name, opts in experiences.items():
            print(name)
        sys.exit(0)
    
    if (options.dry_run):
        for name, opts in experiences.items():
            output_dir = join(top_srcdir, "results", name)
            print(name)
            cmd = build_drmem_cmd(options, name, opts)
            print("tracing:  " + string.join(cmd, " "))
            cmd = build_kmem_cmd(name, output_dir)
            print("analysis: " + string.join(cmd, " "))
            cmd = build_gnuplot_cmd(name, output_dir)
            print("ploting:  " + string.join(cmd, " "))
        sys.exit(0)
    
    print(options)
    print(args)
    # validate experiences
    for arg in args:
        if not experiences.has_key(arg):
            raise Exception("unknown experience %s" % (arg))
    
    # check for required tools
    ok = True
    exe_list = [ "java", "lttng-simple", "lttng", "gnuplot" ]
    for exe in exe_list:
        res = which(exe)
        if len(res) == 0:
            ok = False
            print("Not found in path: " + exe)
    if not os.path.exists(options.drmem_dir):
        print("drmem not found")
        ok = False
    if not os.path.exists(join(top_srcdir, "scripts", "lttng-kmem.jar")):
        print("lttng-kmem.jar not found in scripts directory")
        ok = False
    if not ok:
        print("Can't run analysis, verify the setup")
        sys.exit(1)
            
    
    try:
        if len(args) == 0:
            # run all experiences
            for name, opts in experiences.items():
                do_one(options, name, opts)
        else:
            for arg in args:
                do_one(options, arg, experiences[arg])
    except (KeyboardInterrupt, RuntimeError) as e:
        os.chdir(orig_pwd)
        print("destroy sessions...")
        run_cmd(["lttng", "destroy", "-a"])
        #print(str(e))
        
