/*
 * opts.c
 *
 *  Created on: Jun 3, 2013
 *      Author: Francis Giraldeau <francis.giraldeau@polymtl.ca>
 */

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <getopt.h>
#include "opts.h"

#define DEFAULT_CHUNK "byte"

__attribute__((noreturn))
void usage(void) {
    fprintf(stderr, "Usage: %s [OPTIONS] [COMMAND]\n", "drmem");
    fprintf(stderr, "\nOptions:\n\n");
    fprintf(stderr, "  --block-size       set size of blocs [ byte | word | page | huge ]\n");
    fprintf(stderr, "  --max-mem          maximal memory to allocate in bytes\n");
    fprintf(stderr, "  --where            where to allocate [ heap | stack ]\n");
    fprintf(stderr, "  --fill             fill allocated data\n");
    fprintf(stderr, "  --trim             set trim threshold\n");
    fprintf(stderr, "  --repeat           number of cycles to execute\n");
    fprintf(stderr, "  --delay            add nano second sleep after each operation\n");
    fprintf(stderr, "  --verbose          display more information\n");
    fprintf(stderr, "  --help             this help\n");
    exit(EXIT_FAILURE);
}

void parse_opts(int argc, char **argv, memutil_opts_t *memutil) {
    int opt;

    struct option options[] = {
            { "block-size", 1, 0, 'b' },
            { "max-mem",    1, 0, 'm' },
            { "where",      1, 0, 'w' },
            { "fill",       0, 0, 'f' },
            { "trim",       1, 0, 't' },
            { "repeat",     1, 0, 'r' },
            { "delay",      1, 0, 'd' },
            { "verbose",    0, 0, 'v' },
            { "help",       0, 0, 'h' },
            { 0, 0, 0, 0}
    };
    int idx;

    // defaults
    memutil->chunk_type = memutil_chunk_from_name(DEFAULT_CHUNK);
    memutil->max = 4096;
    memutil->on_heap = 1;
    memutil->trim = 128*1024;
    memutil->fill = 0;
    memutil->delay.tv_sec = 0;
    memutil->delay.tv_nsec = 0;
    memutil->repeat = 2;
    memutil->verbose = 0;
    while ((opt = getopt_long(argc, argv, "b:m:w:ft:r:d:vh", options, &idx)) != -1) {
        switch(opt) {
        case 'b':
            memutil->chunk_type = memutil_chunk_from_name(optarg);
            break;
        case 'm':
            memutil->max = atoi(optarg);
            break;
        case 'w':
            if (strcmp(optarg, "heap") == 0)
                memutil->on_heap = 1;
            else if (strcmp(optarg, "stack") == 0)
                memutil->on_heap = 0;
            else usage();
            break;
        case 'f':
            memutil->fill = 1;
            break;
        case 't':
            memutil->trim = atoi(optarg);
            break;
        case 'r':
            memutil->repeat = atoi(optarg);
            break;
        case 'd':
            memutil->delay.tv_nsec = atoi(optarg);
            break;
        case 'v':
            memutil->verbose = 1;
            break;
        case 'h':
            usage();
            break;
        default:
            usage();
            break;
        }
    }
}
