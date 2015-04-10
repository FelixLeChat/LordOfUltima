/*
 * statistics.c
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#include "statistics.h"

struct statistics *make_statistics(void) {
    struct statistics *s = calloc(1, sizeof(struct statistics));
    return s;
}

void free_statistics(struct statistics *s) {
    free(s);
}

// source: http://www.johndcook.com/standard_deviation.html
void statistics_add_sample(struct statistics *s, double sample) {
    if (s == NULL)
        return;
    s->N++;
    s->sum += sample;
    if (s->N == 1) {
        s->prev_m = s->next_m = sample;
    } else {
        s->next_m = s->prev_m + (sample - s->prev_m) / s->N;
        s->next_s = s->prev_s + (sample - s->prev_m) * (sample - s->next_m);
        s->prev_m = s->next_m;
        s->prev_s = s->next_s;
    }
    s->mean = ((s->N > 0) ? s->next_m : 0.0);
    s->variance = ((s->N > 1) ? s->next_s / (s->N - 1) : 0.0);
    s->stdev = sqrt(s->variance);
}

void statistics_print_header(FILE *fd) {
    fprintf(fd, "%-10s %10s %18s %18s\n", "label", "N", "sum", "mean");
}

void statistics_print(FILE *fd, char *label, struct statistics *s) {
    if (s == NULL)
        return;
    fprintf(fd, "%-10s %10lu %#18.3f %#18.3f\n", label, s->N, s->sum, s->mean);
}

int statistics_equals(struct statistics *s1, struct statistics *s2, double precision) {
    if (s1->N == s2->N &&
            fabs(s1->sum - s2->sum) < precision &&
            fabs(s1->mean - s2->mean) < precision)
        return 1;
    return 0;

}

void statistics_copy(struct statistics *dst, struct statistics *src) {
    if (dst == NULL || src == NULL)
        return;
    *dst = *src;
}
