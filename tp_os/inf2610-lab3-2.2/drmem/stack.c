/*
 * stack.c
 *
 *  Created on: 2013-09-16
 *      Author: francis
 */

#include <execinfo.h>
#include <stdio.h>

#define TRACEPOINT_DEFINE
#include <stack_tp_provider.h>

__attribute__((constructor))
static void initialize_ust_regs_rsp() {
    printf("libstack.so loaded\n");
}

void __cyg_profile_func_enter(void *this_fn, void *call_site)
        __attribute__((no_instrument_function));

void __cyg_profile_func_exit(void *this_fn, void *call_site)
        __attribute__((no_instrument_function));

void __cyg_profile_func_enter(void *this_fn, void *call_site) {
    register unsigned long rsp asm("rsp");
    tracepoint(stack, entry, rsp);
    //printf("size of stack %ld\n", top_of_stack - rsp);
}

void __cyg_profile_func_exit(void *this_fn, void *call_site) {
}
