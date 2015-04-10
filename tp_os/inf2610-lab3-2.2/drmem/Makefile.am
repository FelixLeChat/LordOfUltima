bin_PROGRAMS = drmem

#noinst_LIBRARIES = libmemutil.a libstack.a
noinst_LTLIBRARIES = libmemutil.la libstack.la
#lib_LTLIBRARIES = libstack.la

libstack_la_SOURCES = stack.c stack_tp.c stack_tp_provider.h
libstack_la_LDFLAGS = -rdynamic
libstack_la_LIBADD = -llttng-ust

libmemutil_la_SOURCES = memutil.c memutil.h
libmemutil_la_CFLAGS = -finstrument-functions
libmemutil_la_LIBADD = libstack.la

drmem_SOURCES = drmem.c opts.c opts.h
drmem_LDFLAGS = -rdynamic
drmem_LDADD = libmemutil.la libstack.la -ldl -llttng-ust
