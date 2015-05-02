/*
 * libdummy.c
 *
 *  Created on: 2013-08-15
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#define _GNU_SOURCE
#include <stdio.h>
#include <stdlib.h>
#include <execinfo.h>
#include <inttypes.h>
#include <stdint.h>
#include <dlfcn.h>

#include "libdummy.h"

#define DEPTH 10

static inline char const * symname(void *addr) {
    Dl_info sym;
    int ret = dladdr(addr, &sym);
    if (ret != 0)
        return sym.dli_sname;
    return NULL;
}

void c(void) {
    int ret, i;
    char **syms;
    void **ptrs = calloc(sizeof(void *), DEPTH);

    printf("\nPile d'appel:\n");
    ret = backtrace(ptrs, DEPTH);
    syms = backtrace_symbols(ptrs, ret);
    for (i = 0; i < ret; i++) {
        printf("0x%016" PRIxPTR " %-20s %s\n", (uintptr_t) ptrs[i],
                symname(ptrs[i]), syms[i]);
    }
    free(syms);
}

void b(void) {
    c();
}

void a(void) {
    b();
}
