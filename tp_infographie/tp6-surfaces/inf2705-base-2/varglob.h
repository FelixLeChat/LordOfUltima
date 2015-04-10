#ifndef VARIABLES_GLOBALES_H
#define VARIABLES_GLOBALES_H

//
// déclaration des macros et defines globaux
//

#define _USE_MATH_DEFINES
#include <math.h>

#define DEG2RAD(a) ((a) * M_PI / 180.0)
#define RAD2DEG(a) ((a) * 180.0 / M_PI)
#define CLIP(a,min,max) ( (a < min) ? min : ((a > max) ? max : a) )

//
// déclaration des variables globales utilisées dans la plupart des TPs
//

// la fenêtre graphique
int g_feneID = 0;                // le ID de la fenêtre graphique GLUT
GLsizei g_largeur = 900;         // la largeur de la fenêtre
GLsizei g_hauteur = 600;         // la hauteur de la fenêtre

// variables d'état
bool enPerspective = false;      // indique si on est en mode Perspective (true) ou Ortho (false)
bool enmouvement = false;        // le modèle est en mouvement/rotation automatique ou non

// variables pour définir le point de vue
double theta = 0.0;              // angle de rotation de la caméra (coord. sphériques)
double phi = 0.0;                // angle de rotation de la caméra (coord. sphériques)
double dist = 0.0;               // distance (coord. sphériques)

float angleRotX = 0.0;           // angle courant de rotation en X du modèle
float angleRotY = 0.0;           // angle courant de rotation en Y du modèle
float angleRotZ = 0.0;           // angle courant de rotation en Z du modèle

// variables pour l'utilisation des nuanceurs
bool utiliseNuanceurs = false;   // indique si on utilise les nuanceurs
GLuint progNuanceur = 0;         // votre programme de nuanceurs

#endif
