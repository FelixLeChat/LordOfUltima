/*
 * memutil.h
 *
 *  Created on: Jun 3, 2013
 *      Author: Francis Giraldeau <francis.giraldeau@polymtl.ca>
 */

#ifndef MEMUTIL_H_
#define MEMUTIL_H_

#include <time.h>

// Assume 4 KiB pages (1 << 12)
#define PAGE_SIZE 4096

typedef enum chunk_size {
    CHUNK_BYTE = 0,
    CHUNK_WORD,
    CHUNK_PAGE,
    CHUNK_HUGE,
    CHUNK_LAST
} chunk_size_t;

typedef struct chunk {
    enum chunk_size type;
    char *name;
    int bytes;
} memutil_chunk_t;

typedef struct settings {
    memutil_chunk_t *chunk_type;
    char **data;
    int data_len;
    int max;
    int on_heap;
    int trim;
    int fill;
    struct timespec delay;
    int repeat;
    int verbose;
} memutil_opts_t;

extern char *units_iec[];

#define DEBUG(msg, args ...)                            \
    do {                                                \
        fprintf(stderr, "%s:%-4d debug: ",                \
                __FILE__, __LINE__);                    \
      fprintf(stderr, msg, ## args);                    \
      fputc('\n', stderr);                              \
    } while(0)

/*
 * API for memutils
 */

void memutil_grow_heap(memutil_opts_t *opts);
void memutil_grow_stack(memutil_opts_t *opts);
void memutil_init(memutil_opts_t *opts);
void memutil_destroy(memutil_opts_t *opts);
memutil_chunk_t *memutil_chunk_from_name(char *name);

#endif /* MEMUTIL_H_ */
