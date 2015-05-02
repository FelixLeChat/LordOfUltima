/*
 * whoami.h
 *
 *  Created on: 2013-08-15
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <libgen.h>
#include <string.h>

static int rank;

#define PATH_MAX 1024

/**
 * Affiche le pid et la variable le rank
 */
void whoami(const char *name) {
	printf("%-10s rank=%-5d pid=%-5d\n", name, rank, getpid());
}

/**
 * Incrémente la variable static rank
 */
void increment_rank(void) {
	rank++;
}

/**
 * add_pwd_to_path - Ajoute le repertoire parent de FILE a la variable
 * d'environnement PATH
 * @file: fichier de reference
 *
 * Retourne 0 si OK, -1 en cas d'erreur
 */
int add_pwd_to_path(const char *file) {
        int ret, len;
        char *abspath, *curdir, *path, *oldpath;

        oldpath = getenv("PATH");
        abspath = realpath(file, NULL);
        if (!abspath)
                goto error;
        curdir = strdup(abspath);
        if (!curdir)
                goto error;
        curdir = dirname(curdir);
        len = strlen(curdir) + strlen(oldpath) + 2;
        path = calloc(sizeof(char), len);
        if (!path)
                goto error;
        sprintf(path, "%s:%s", curdir, oldpath);
        setenv("PATH", path, 1);
        ret = 0;
done:
        free(path);
        free(curdir);
        free(abspath);
        return ret;
error:
        ret = -1;
        goto done;
}
