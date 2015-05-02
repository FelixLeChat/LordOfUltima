/*
 * utils.c
 *
 *  Created on: 2014-02-05
 *      Author: francis
 */

#include "utils.h"

#define NANOSEC 1000000000

/*
 * Retourne t1 - t2
 */
void time_sub(struct timespec *res, struct timespec *x, struct timespec *y)
{
    if (res == NULL || x == NULL || y == NULL)
        return;
    res->tv_sec = x->tv_sec - y->tv_sec;
    res->tv_nsec = x->tv_nsec - y->tv_nsec;
    if (x->tv_nsec < y->tv_nsec) {
        res->tv_sec--;
        res->tv_nsec += NANOSEC;
    }
    return;
}
