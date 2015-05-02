/*
 * pagedump.c
 *
 *  Created on: 2013-10-23
 *      Author: francis
 */


#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>

/*
 * Savegarder la page contenant ptr dans le fichier fname
 */
void save_page(char *fname, void *ptr) {
    (void) fname;
    (void) ptr;
    /*
     * TODO:
     * 1 - calculer l'adresse du début de la page
     * 2 - ouvrir le fichier de destination
     * 3 - écrire la page dans le fichier
     * 4 - fermer le fichier
     */

     unsigned long pageStart = (long)ptr - ((long)ptr%(4*1024));
     printf("Page Start : %x\n", pageStart);

     FILE* fd = fopen(fname, "w");
     if(fd == NULL)
     {
     	printf("ERROR OPENING FILE\n");
     }
     else
     {
     	int error = fwrite(pageStart, sizeof(char), 4*1024, fd);
     }

    fclose(fd);
    return;
}

int main(int argc, char **argv) {
    /*
     * L'utilisation de volatile empêche le compilateur
     * d'optimiser (i.e. supprimer) les variables.
     *
     * (void) var; évite l'avertissement concernant une variable non-utilisée
     *
     */

    /*
     * Variables sur la pile (stack)
     */
    volatile int cafe1 = 0x1111CAFE; (void) cafe1;
    volatile int cafe2 = 0x2222CAFE; (void) cafe2;
    volatile int cafe3[2] = { 0x3333CAFE, 0x4444CAFE }; (void) cafe3;

    /*
     * Variables sur le monceau (heap)
     */
    volatile int *beef1 = malloc(sizeof(int)); (void) beef1;
    volatile int *beef2 = malloc(sizeof(int)); (void) beef2;
    volatile int *beef3 = malloc(2 * sizeof(int)); (void) beef3;
    *beef1 = 0x1111BEEF;
    *beef2 = 0x2222BEEF;
    beef3[0] = 0x3333BEEF;
    beef3[1] = 0x4444BEEF;

    save_page("beef.page", (void *)beef1);
    save_page("cafe.page", (void *)&cafe1);
    return 0;
}
