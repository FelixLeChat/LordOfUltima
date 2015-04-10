/*
 * multiwait.c
 *
 *  Created on: 2013-08-19
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdio.h>
#include <stdlib.h>
#include <getopt.h>
#include <math.h>

#include "multilock.h"
#include "serial.h"
#include "mutex.h"
#include "semrelay.h"
#include "spinlock.h"

#define PROGNAME "multilock"
#define VAL_VERSION "1.0"
#define DEFAULT_INNER 1
#define DEFAULT_OUTER 1000
#define DEFAULT_NR_THREAD 4
#define DEFAULT_LIB LIB_SERIAL
static const char *const progname = PROGNAME;

/*
 * Définition des librairies de traitement
 * Voir struct lib_def dans multilock.h
 */
static struct lib_def libs[] = {
	{
	  .id = LIB_SERIAL,
	  .name = "serial",
	  .worker = serial_worker,
	  .init = serial_init,
	  .done = serial_done,
	  .spawn = spawn_serial,
	  .equals = 1,
	},
	{
	  .id = LIB_MUTEX,
	  .name = "mutex",
	  .worker = mutex_worker,
	  .init = mutex_init,
	  .done = mutex_done,
	  .spawn = spawn_parallel,
	  .equals = 1,
	},
    {
        .id = LIB_SEMRELAY,
        .name = "semrelay",
        .worker = semrelay_worker,
        .init = semrelay_init,
        .done = semrelay_done,
        .spawn = spawn_parallel,
        .equals = 1,
    },
	{
	  .id = LIB_SPINLOCK,
	  .name = "spinlock",
	  .worker = spinlock_worker,
	  .init = spinlock_init,
	  .done = spinlock_done,
	  .spawn = spawn_parallel,
	  .equals = 1,
	},
};

void *serial_wrapper(void *ptr) {
    int i;
    struct experiment *exp = ptr;
    for (i = 0; i < exp->nr_thread; i++) {
        exp->lib->worker(exp);
    }
    return NULL;
}

/*
 * Démarrage en série de l'expérience
 */
void spawn_serial(struct experiment *exp) {
    int i;
    pthread_t thread;
    exp->lib->init(exp);
    pthread_create(&thread, NULL, serial_wrapper, exp);
    pthread_join(thread, NULL);
    exp->lib->done(exp);
}

/*
 * Démarrage en parallèle des expériences
 */
void spawn_parallel(struct experiment *exp) {
    int i;
    pthread_t *threads = calloc(exp->nr_thread, sizeof(pthread_t));
    struct experiment *expa = calloc(exp->nr_thread, sizeof(struct experiment));
    exp->lib->init(exp);
    for (i = 0; i < exp->nr_thread; i++) {
        expa[i] = *exp;         // copie tout le contenu de l'expérience
        expa[i].rank = i;       // définir un rang unique
        pthread_create(&threads[i], NULL, exp->lib->worker, &expa[i]);
    }
    for (i = 0; i < exp->nr_thread; i++) {
        pthread_join(threads[i], NULL);
    }
    exp->lib->done(exp);
    free(threads);
    free(expa);
}

/*
 * Retourne un pointeur de la librairie de traitement selon son nom
 */
struct lib_def *find_lib(const char *name) {
    int i;
    for (i = 0; i < LIB_LAST; i++) {
        if (strcmp(libs[i].name, name) == 0)
            return &libs[i];
    }
    return NULL;
}

/*
 * Calcul le résultat attendu
 */
void compute_expected(struct experiment *exp) {
    long i, j;
    struct statistics *stats = make_statistics();
    unsigned long repeat = exp->outer * exp->inner;
    stats->N = repeat * exp->nr_thread;
    stats->sum = ((repeat * (repeat - 1)) / 2) * exp->nr_thread;
    stats->mean = ((double) stats->sum) / ((double)stats->N);
    for (i = 0; i < exp->nr_thread; i++)
    for (j = 0; j < repeat; j++)
        stats->variance += ((j - stats->mean) * (j - stats->mean));
    stats->variance /= stats->N;
    stats->stdev = sqrt(stats->variance);
    exp->expected = stats;
}

void print_libs() {
    int i;
    for (i = 0; i < LIB_LAST; i++) {
        printf("%s ", libs[i].name);
    }
    printf("\n");
}

__attribute__((noreturn))
static void usage(void) {
    fprintf(stderr, "Usage: %s [OPTIONS] [COMMAND]\n", progname);
    fprintf(stderr, "Experiences sur les types de verrous\n");
    fprintf(stderr, "\nOptions:\n\n");
    fprintf(stderr, "--lib LIB        execute la librairie LIB\n");
    fprintf(stderr, "--thread N       lance N fils d'execution parallels\n");
    fprintf(stderr, "--outer NR       nombre de repetition de la boucle externe\n");
    fprintf(stderr, "--inner NR       nombre de repetition de la boucle interne\n");
    fprintf(stderr, "--check          execute toutes les librairies\n");
    fprintf(stderr, "--verbose        affiche plus de message\n");
    fprintf(stderr, "--help           ce message d'aide\n");
    fprintf(stderr, "Choix de librairie:\n");
    print_libs();
    exit(EXIT_FAILURE);
}

static void parse_opts(int argc, char **argv, struct experiment *exp) {
    int opt;

    struct option options[] = {
            { "help",       0, 0, 'h' },
            { "outer",      1, 0, 'o' },
            { "inner",      1, 0, 'i' },
            { "thread",     1, 0, 't' },
            { "check",      0, 0, 'c' },
            { "verbose",    0, 0, 'v' },
            { "lib",        1, 0, 'l' },
            { 0, 0, 0, 0}
    };
    int idx;

    exp->lib = &libs[DEFAULT_LIB];
    exp->outer = DEFAULT_OUTER;
    exp->inner = DEFAULT_INNER;
    exp->nr_thread = DEFAULT_NR_THREAD;

    while ((opt = getopt_long(argc, argv, "hco:i:l:t:", options, &idx)) != -1) {
        switch (opt) {
        case 'c':
            exp->check = 1;
            break;
        case 'o':
            exp->outer = atol(optarg);
            break;
        case 'i':
            exp->inner = atol(optarg);
            break;
        case 't':
            exp->nr_thread = atoi(optarg);
            break;
        case 'v':
            exp->verbose = 1;
            break;
        case 'l':
            exp->lib = find_lib(optarg);
            if (!exp->lib) {
                printf("invalid library %s", optarg);
                usage();
            }
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

/*
 * Exécute une expérience
 */
int do_one(struct experiment *exp) {
    int ret, equals;
    char *msg;
    exp->stats = make_statistics();
    exp->lib->spawn(exp);       // lance le traitement de la librairie
    compute_expected(exp);      // calcul analytique

    if (exp->verbose) {
        statistics_print_header(stdout);
        statistics_print(stdout, "actual", exp->stats);
        statistics_print(stdout, "expected", exp->expected);
    }

    equals = statistics_equals(exp->stats, exp->expected, 0.001);
    if (equals == exp->lib->equals) {
        msg = "PASS";
        ret = 0;
    } else {
        msg = "FAIL";
        ret = 1;
    }
    printf("%s %s\n", msg, exp->lib->name);

    free_statistics(exp->stats);
    free_statistics(exp->expected);
    return ret;
}

/*
 * Lance l'expérience pour chaque librairies disponibles
 */
int do_all(struct experiment *exp) {
    int ret;
    int i;

    ret = 0;
    for (i = 0; i < LIB_LAST; i++) {
        exp->lib = &libs[i];
        ret |= do_one(exp);
    }

    return ret;
}

int main(int argc, char **argv) {
    int ret;
    struct experiment *exp = calloc(1, sizeof(struct experiment));

    parse_opts(argc, argv, exp);
    if (exp->check) {
        ret = do_all(exp);
    } else {
        ret = do_one(exp);
    }

    free(exp);
    return ret;
}
