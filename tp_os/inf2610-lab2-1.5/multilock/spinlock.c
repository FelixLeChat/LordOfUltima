/*
 * spinlock.c
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdlib.h>
#include <stdio.h>
#include "minispinlock.h"
#include "statistics.h"
#include "multilock.h"

/* Spinlock: verrou actif
 *
 * Voir l'implémentation du verrou dans le fichier minispinlock.asm
 * et l'interface dans minispinlock.h
 */
void *spinlock_worker(void *ptr) {
    unsigned long i, j;
    struct experiment *exp = ptr;
    for (i = 0; i < exp->outer; i++) {
        mini_spin_lock(exp->lock);
        for (j = 0; j < exp->inner; j++) {
            unsigned long idx = (i * exp->inner) + j;
            statistics_add_sample(exp->data, (double) idx);
        }
        mini_spin_unlock(exp->lock);
    }
    return NULL;
}

void spinlock_init(struct experiment *exp) {
    exp->data = make_statistics();
    exp->lock = calloc(1, sizeof(long));
}

void spinlock_done(struct experiment *exp) {
    statistics_copy(exp->stats, exp->data);
    free(exp->data);
    free(exp->lock);
}

