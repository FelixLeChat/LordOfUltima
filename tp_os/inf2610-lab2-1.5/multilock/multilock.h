/*
 * multilock.h
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#ifndef MULTILOCK_H_
#define MULTILOCK_H_

#include "statistics.h"

struct experiment;
struct lib_def;
struct opts;

typedef void *(*worker_t)(void *);
typedef void (*init_t)(struct experiment *);
typedef void (*done_t)(struct experiment *);
typedef void (*spawn_t)(struct experiment *);

enum lib_type {
    LIB_SERIAL = 0,
    LIB_MUTEX,
    LIB_SEMRELAY,
    LIB_SPINLOCK,
    LIB_LAST,
};

struct lib_def {
    const int id;       // identifiant
    const char *name;   // nom
    init_t init;        // fonction d'initialisation
    worker_t worker;    // fonction principale de traitement
    done_t done;        // fonction de terminaison
    spawn_t spawn;      // fonction de démarrage (spawn_parallel ou spawn_serial)
    const int equals;   // 1 -> il est attentu que les résultats soient égaux, 0 sinon
};

struct experiment {
    struct lib_def *lib;// librairie de traitement à utiliser
    int nr_thread;      // nombre de fils d'exécution à démarrer
    int rank;           // rang du fil d'exécution (0 à n - 1)
    void *data;         // pointeur de données polyvalent
    void *lock;         // pointeur de verrous
    struct statistics *stats;   // statistiques calculées
    struct statistics *expected;// statistiques attendues
    unsigned long outer;        // répétition de la boucle interne (nombre de verrouillage/déverouillage)
    unsigned long inner;        // répétition de la boucle interne (influence la durée du verrou)
    int verbose;        // affiche plus de détail
    int check;          // vérifie tous les résultats (mode test)
};

void spawn_serial(struct experiment *exp);
void spawn_parallel(struct experiment *exp);

#endif /* MULTILOCK_H_ */
