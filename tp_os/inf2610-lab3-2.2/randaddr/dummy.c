/*
 * fixup.c
 *
 *  Created on: 2013-08-15
 *      Author: Francis Giraldeau <francis.giraldeau@gmail.com>
 */

#include <stdlib.h>
#include <stdio.h>
#include <inttypes.h>
#include <stdint.h>
#include "libdummy.h"

volatile int global = 0;

int main(int argc, char **argv) {
	volatile int stack = 0;
	int *heap = malloc(sizeof(int));
	printf("Variables:\n");
	printf("GLOBAL 0x%016" PRIxPTR "\n", (uintptr_t) &global);
	printf("STACK  0x%016" PRIxPTR "\n", (uintptr_t) &stack);
	printf("HEAP   0x%016" PRIxPTR "\n", (uintptr_t) heap);
	a();
	return 0;
}
