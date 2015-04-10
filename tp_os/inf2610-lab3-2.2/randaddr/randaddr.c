/*
 * randaddr.c
 *
 *  Created on: 2013-08-15
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <getopt.h>
#include <string.h>
#include <sys/wait.h>
#include <sys/personality.h>

#define PROGNAME "randaddr"
#define VAL_VERSION "1.0"

/* Global variables */

static const char * const progname = PROGNAME;

struct vars {
    char *prog;
    char **args;
    int nr_args;
    int dry_run;
    int verbose;
};

__attribute__((noreturn))
static void
usage(void) {
    fprintf(stderr, "Usage: %s [OPTIONS] [COMMAND]\n", progname);
    fprintf(stderr, "Execute un programme a un emplacement fixe en memoire\n");
    fprintf(stderr, "\nOptions:\n\n");
    fprintf(stderr, "  --dry-run  execute sans changer les parametres\n");
    fprintf(stderr, "  --help     cet aide\n");
    exit(EXIT_FAILURE);
}

static void
parse_opts(int argc, char **argv, struct vars *vars) {
    int opt;
    int i, j;

    struct option options[] = { { "help", 0, 0, 'h' }, { "dry-run", 0, 0, 'n' },
            { "verbose", 0, 0, 'v' }, { 0, 0, 0, 0 } };
    int idx;

    while ((opt = getopt_long(argc, argv, "hnv", options, &idx)) != -1) {
        switch (opt) {
        case 'n':
            vars->dry_run = 1;
            break;
        case 'v':
            vars->verbose = 1;
            break;
        case 'h':
            usage();
            break;
        default:
            usage();
            break;
        }
    }

    if (optind < argc) {
        vars->nr_args = argc - optind;
        vars->args = calloc(vars->nr_args, sizeof(char *));
        vars->prog = strdup(argv[optind]);
        for (i = 0, j = optind; j < argc; i++, j++) {
            vars->args[i] = strdup(argv[j]);
        }
    }

    if (vars->prog == NULL)
        usage();

    if (vars->verbose) {
        printf("vars->prog %s\n", vars->prog);
        for (i = 0; i < vars->nr_args; i++) {
            printf("arg %d %s\n", i, vars->args[i]);
        }
    }
}

void
free_vars(struct vars *vars) {
    int i;
    if (vars == NULL)
        return;
    for (i = 0; i < vars->nr_args; i++) {
        free(vars->args[i]);
    }
    free(vars->args);
    free(vars->prog);
    free(vars);
}

int
main(int argc, char **argv) {
    int ret;
    struct vars *vars = calloc(1, sizeof(struct vars));
    parse_opts(argc, argv, vars);

    ret = fork();
    switch (ret) {
    case -1:
        printf("Fork error\n");
        break;
    case 0:
        /*
         * TODO: faire un appel à personality(),
         * puis exécuter la commande passée en argument (voir vars->prog et vars->args).
         * ATTENTION: bien s'assurer de traiter l'argument vars->dry_run
         */
        break;
    default:
        wait(NULL);
        break;
    }

    free_vars(vars);
    printf("Done\n");
    return 0;
}
