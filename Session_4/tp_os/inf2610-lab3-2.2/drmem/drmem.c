/*
 * drmem.c
 *
 *  Created on: 2013-05-31
 *      Author: Francis Giraldeau <francis.giraldeau@polymtl.ca>
 */

#define _GNU_SOURCE
#include <stdlib.h>
#include <stdio.h>
#include <time.h>
#include <math.h>
#include <inttypes.h>
#include <string.h>
#include <malloc.h>
#include "memutil.h"
#include "opts.h"

void run_one(memutil_opts_t *opts) {
    if (opts->on_heap)
        memutil_grow_heap(opts);
    else
        memutil_grow_stack(opts);
}

void run_experiment(memutil_opts_t *opts) {
    int i;
    for (i = 0; i < opts->repeat; i++) {
        run_one(opts);
    }
}

int main(int argc, char **argv) {
    memutil_opts_t opts;

    parse_opts(argc, argv, &opts);
    memutil_init(&opts);
    run_experiment(&opts);
    memutil_destroy(&opts);
    return 0;
} 
