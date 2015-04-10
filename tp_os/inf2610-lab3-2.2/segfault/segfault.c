/*
 * segfault.c
 *
 *  Created on: 2013-10-22
 *      Author: francis
 */

#include <stdio.h>
#include <stdlib.h>
#include <signal.h>
#include <fcntl.h>
#include <unistd.h>
#include <execinfo.h>

#define WIDTH 8

static volatile char *addr;
static volatile int offset;

#define BUFLEN 128

void save_maps(void) {
    int ret;
    char buf[BUFLEN];
    sprintf(buf, "/proc/%d/maps", getpid());
    int maps_fd = open(buf, O_RDONLY);
    int dest_fd = open("maps.snapshot", O_WRONLY | O_CREAT, S_IRUSR | S_IWUSR);
    while((ret = read(maps_fd, buf, BUFLEN)) > 0) {
        ret = write(dest_fd, buf, ret);
        if (ret < 0)
            break;
    }
    close(maps_fd);
    close(dest_fd);
}

/*
 * La fonction crash_handler intercepte le signal SIGSEGV. De cette manière,
 */
void crash_handler(int signal) {
    printf("\nHouston, we have a problem:\n");
    printf("segfault at addr=%p offset=0x%x (%f kio)\n", addr, offset, offset / 1024.0);
    exit(1);
}

void scan_memory(void *data, int direction) {
    int i; (void) i;
    volatile char bidon = 0; (void) bidon;
    char *start = (char *) data; (void) start;

    if (direction != -1 && direction != 1) {
        printf("error: direction must be 1 or -1\n");
        return;
    }
    printf("direction=%d\n", direction);

    /* TODO: Lire, octet par octet, jusqu'à la réception du signal SIGSEGV.
     *
     * Dans une boucle:
     * 1 - mettre à jour la variable offset
     * 2 - calculer l'adresse à accéder dans addr
     * 3 - appel à __sync_synchronize(); (empêche le compilateur de réordonner les instructions)
     * 4 - lire la valeur à l'adresse addr dans la variable bidon (force l'accès à cette adresse)
     *
     * Lecture seulement de la mémoire. Si le balayage écrase de la mémoire
     * utile au programme, alors il se peut que son comportement soit modifié.
     */
    printf("D'oh! No segfault!\n");
    return;
}

int main(int argc, char **argv) {
    int dir = 0; (void) dir;
    long *bidon = malloc(sizeof(long));
    *bidon = 0xCAFEBABECAFEBABE;

    if (argc < 2) {
        printf("Specifier la direction de balayage: 1 ou -1\n");
        return -1;
    }
    dir = atoi(argv[1]);

    // TODO: enregister fonction crash_handler au signal SIGSEGV

    save_maps();

    // TODO: appel à scan_memory()

    printf("done\n");
    return 0;
}
