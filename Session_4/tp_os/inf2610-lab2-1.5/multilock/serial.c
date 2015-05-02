/*
 * serial.c
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdio.h>
#include <stdlib.h>

#include "statistics.h"
#include "multilock.h"

void *serial_worker(void *ptr) {
    unsigned long i, j;
    struct experiment *exp = ptr;

    for (i = 0; i < exp->outer; i++) {
        for (j = 0; j < exp->inner; j++) {
            unsigned long idx = (i * exp->inner) + j;
            statistics_add_sample(exp->data, (double) idx);
        }
    }
    return NULL;
}

void serial_init(struct experiment *exp) {
    exp->data = make_statistics();
    exp->lock = NULL;
}

void serial_done(struct experiment *exp) {
    statistics_copy(exp->stats, exp->data);
    free_statistics(exp->data);
    exp->data = NULL;
}
