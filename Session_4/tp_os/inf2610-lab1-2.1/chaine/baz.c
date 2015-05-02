/*
 * baz.c
 *
 *  Created on: 2013-08-15
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#define _GNU_SOURCE
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <sys/types.h>
#include "whoami.h"

int main(int argc, char **argv) {
	increment_rank();
	whoami("baz");
	int n = atoi(argv[1]);
	--n;
	char* m;
	asprintf(&m,"%d", n);
	execlp("chaine", "chaine", m, NULL);
	return 0;
}
