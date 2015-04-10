/*
 * statistics.h
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#ifndef CRITICAL_H_
#define CRITICAL_H_

#include <stdio.h>

struct statistics {
    unsigned long N;
    double sum;
    double mean;
    double variance;
    double stdev;
    double prev_m;
    double next_m;
    double prev_s;
    double next_s;
};

struct statistics *make_statistics();
void free_statistics(struct statistics *s);
void statistics_add_sample(struct statistics *s, double sample);
void statistics_print_header(FILE *fd);
void statistics_print(FILE *fd, char *label, struct statistics *s);
int statistics_equals(struct statistics *s1, struct statistics *s2, double precision);
void statistics_copy(struct statistics *dst, struct statistics *src);

#endif /* CRITICAL_H_ */
