// Prénoms, noms et matricule des membres de l'équipe:
// - Prénom1 NOM1 (matricule1)
// - Prénom2 NOM2 (matricule2)
#warning "Écrire les prénoms, noms et matricule des membres de l'équipe dans le fichier et commenter cette ligne"

#include <iostream>
#define GL_GLEXT_PROTOTYPES 1
#include <GL/glew.h>
#include <GL/glut.h>
#include "varglob.h"
#include "float3.h"
#include "fctsUtiles.h"
#include "chargertex.h"

float MAXPHI = M_PI - 0.001, MAXTHETA = M_PI - 0.001;
float MINPHI = 0.001, MINTHETA = 0.001;

float posX = 0.25, posY = 0.25, posZ = -1.75;
bool positionnelle = true;
bool localViewer = false;        // la valeur de GL_LIGHT_MODEL_LOCAL_VIEWER

int affichageStereo = 0;         // l'affichage est en mono

GLuint textures[9];              // les textures chargées
int indiceTexture = 0;           // indice de la texture à utiliser pour le déplacement
int indiceCouleur = 0;           // indice de la texture à utiliser pour la couleur
int indiceFonction = 0;          // indice de la fonction à afficher
float facteurZ = 1.0;            // facteur de déplacement initial en Z

struct VariablesGlobales
{
   GLint w, h;
   GLdouble dip;       // la distance interpupillaire
   GLdouble factzoom;  // le facteur de zoom
   GLdouble zavant;    // la position du plan avant du volume de visualisation
   GLdouble zarriere;  // la position du plan arrière du volume de visualisation
   GLdouble zecran;    // la position du plan de l'écran: les objets affichés en avant de ce plan «sortiront» de l'écran
   GLint modele;       // le modèle à afficher
   GLdouble rotationY;       // l'angle courant de rotation
   GLdouble rotationIncrement; // l'incrément
};
struct VariablesGlobales glob;

const int NSOMMETS = 100;
GLfloat sommets[8*2*NSOMMETS*NSOMMETS];
GLfloat texcoords[8*2*NSOMMETS*NSOMMETS];

void chargerToutesLesTextures()
{
   ChargerTexture( "textures/texture1.bmp", textures[0] );
   ChargerTexture( "textures/texture2.bmp", textures[1] );
   ChargerTexture( "textures/texture3.bmp", textures[2] );
   ChargerTexture( "textures/texture4.bmp", textures[3] );
   ChargerTexture( "textures/texture5.bmp", textures[4] );
   ChargerTexture( "textures/texture6.bmp", textures[5] );
   ChargerTexture( "textures/texture7.bmp", textures[6] );
   ChargerTexture( "textures/texture8.bmp", textures[7] );
   ChargerTexture( "textures/texture9.bmp", textures[8] );
}

void chargerNuanceurs()
{
   // charger les nuanceurs
   const char *ns = "nuanceurs/nuanceurSommets.glsl";
   const char *nf = "nuanceurs/nuanceurFragments.glsl";
   progNuanceur = initialiserNuanceurs( ns, nf );
}

void initialisation()
{
   phi = M_PI/2, theta = M_PI/2;
   dist = 10;

   utiliseNuanceurs = true;

   // donner la couleur de fond
   glClearColor( 0.2, 0.2, 0.2, 1.0 );

   // activer les etats openGL
   glEnable( GL_NORMALIZE );
   glEnable( GL_DEPTH_TEST );
   glDepthFunc( GL_LEQUAL );

   glob.dip = 0.80;
   glob.factzoom = 1.0;
   glob.zavant = 4.0;
   glob.zarriere = 20.0;
   glob.zecran = 10.0;
   glob.modele = 1;
   glob.rotationY = 0.0;
   glob.rotationIncrement = 2.0;

   //glLightModeli( GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE );     // two-side mode en openGL
   //glEnable( GL_VERTEX_PROGRAM_TWO_SIDE );                // two-side mode en GLSL
   glEnable( GL_LIGHTING );

   // charger les nuanceurs
   chargerNuanceurs();

   // charger les textures
   chargerToutesLesTextures();

   // initialiser le tableau de sommets
   int decalage = 0;
   for ( int i = 0; i < NSOMMETS-1; i++ )
   {
      for ( int j = 0; j < NSOMMETS-1; j++ )
      {
         const int larg = 4.0;  // intervalle [-2.0,2.0]
         const float dl = 1.0 / ( NSOMMETS-1 );
         sommets[decalage+0] = larg * (  i   *dl - 0.5 );  sommets[decalage+1] = larg * (  j   *dl - 0.5 );
         sommets[decalage+2] = larg * (  i   *dl - 0.5 );  sommets[decalage+3] = larg * ( (j+1)*dl - 0.5 );
         sommets[decalage+4] = larg * ( (i+1)*dl - 0.5 );  sommets[decalage+5] = larg * ( (j+1)*dl - 0.5 );
         sommets[decalage+6] = larg * ( (i+1)*dl - 0.5 );  sommets[decalage+7] = larg * (  j   *dl - 0.5 );
         decalage += 8;
      }
   }
}

void definirEclairage()
{
   GLfloat LumiAmbiant[] = { 0.5, 0.5, 0.5, 1.0 };
   GLfloat LumiDiffuse[] = { 0.7, 0.7, 0.7, 1.0 };
   GLfloat LumiSpeculaire[] = { 0.5, 0.5, 0.5, 1.0 };
   glLightfv( GL_LIGHT0, GL_AMBIENT, LumiAmbiant );
   glLightfv( GL_LIGHT0, GL_DIFFUSE, LumiDiffuse );
   glLightfv( GL_LIGHT0, GL_SPECULAR, LumiSpeculaire );
   // partie 1: définir la lumière
   //GLfloat LumiPosition[] = ...
   //glLightfv( GL_LIGHT0, GL_POSITION, LumiPosition );

   glEnable( GL_LIGHT0 );
}

void definirProjection( int OeilMult ) // 0: mono, -1: oeil gauche, +1: oeil droit
{
   glMatrixMode( GL_PROJECTION );
   glLoadIdentity();
   // changer pour glFrustum()
   gluPerspective( 50.0, (GLdouble) g_largeur / (GLdouble) g_hauteur, 1, 30.0 );
   glMatrixMode( GL_MODELVIEW );
}

void afficherModele()
{
   GLfloat mat_ambiant[]  = { 0.2, 0.2, 0.2, 1.0 };
   GLfloat mat_diffuse[]  = { 1.0, 1.0, 1.0, 1.0 };
   GLfloat mat_specular[] = { 1.0, 1.0, 1.0, 1.0 };
   GLfloat mat_emission[] = { 0.0, 0.0, 0.0, 1.0 };
   GLfloat mat_shininess[] = { 100.0 };
   glMaterialfv( GL_FRONT_AND_BACK, GL_AMBIENT, mat_ambiant );
   glMaterialfv( GL_FRONT_AND_BACK, GL_DIFFUSE, mat_diffuse );
   glMaterialfv( GL_FRONT_AND_BACK, GL_SPECULAR, mat_specular );
   glMaterialfv( GL_FRONT_AND_BACK, GL_EMISSION, mat_emission );
   glMaterialfv( GL_FRONT_AND_BACK, GL_SHININESS, mat_shininess );

   definirEclairage();

   // dessiner la géométrie
   if ( utiliseNuanceurs )
   {
      glUseProgram( progNuanceur );
      glUniform1i( glGetUniformLocation( progNuanceur, "indiceFonction" ), indiceFonction );
      glUniform1i( glGetUniformLocation( progNuanceur, "localViewer" ), localViewer );
      //glUniform3f( glGetUniformLocation( progNuanceur, "vecteurQuelconque" ), comp1, comp2, comp3 );
      glUniform1i( glGetUniformLocation( progNuanceur, "indiceTexture" ), indiceTexture );
      glUniform1i( glGetUniformLocation( progNuanceur, "indiceCouleur" ), indiceCouleur );
      glUniform1f( glGetUniformLocation( progNuanceur, "facteurZ" ), facteurZ );
      glUniform1i( glGetUniformLocation( progNuanceur, "displacementMap" ), 0 ); // '0' => utilisation de GL_TEXTURE0
      glUniform1i( glGetUniformLocation( progNuanceur, "colorMap" ), 1 );        // '1' => utilisation de GL_TEXTURE1
   }
   else
      glUseProgram( 0 );

   // partie 2: s'il y a lieu, assigner la texture à utiliser
   //glBindTexture( GL_TEXTURE_2D, textures[indiceTexture-1] );
   
   // partie 1: définir les glClipPane limitant le z vers le haut et vers le bas
   // ...

   // afficher la surface (plane)
   glEnableClientState( GL_VERTEX_ARRAY );
   glVertexPointer( 2, GL_FLOAT, 0, sommets );
   glDrawArrays( GL_QUADS, 0, 8*(NSOMMETS-1)*(NSOMMETS-1) );
   glDisableClientState( GL_VERTEX_ARRAY );

   // dessiner le cube englobant
   glPushAttrib( GL_ENABLE_BIT | GL_CURRENT_BIT );
   glDisable( GL_LIGHTING );
   glUseProgram( 0 );

   // dessiner le cube englobant
   glColor3f( 1.0, 1.0, 1.0 );
   glutWireCube( 4.0 );

   // dessiner une sphère à la position de la lumière
   glColor3f( 1.0, 1.0, 1.0 );
   if ( !positionnelle )
   {
      glBegin( GL_LINES );
      glVertex3f( 0, 0, 0 );
      glVertex3f( 2*posX, 2*posY, 2*posZ );
      glEnd( );
   }
   glPushMatrix();{
      glTranslatef( posX, posY, posZ );
      glutSolidSphere( 0.1, 10, 10 );
   }glPopMatrix();

   glPopAttrib( );
}

void afficherScene()
{
   glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );

   glMatrixMode( GL_MODELVIEW );
   glLoadIdentity();
   gluLookAt( dist*cos(phi)*sin(theta), dist*sin(theta)*sin(phi), dist*cos(theta), 0, 0, 0, 0, 0, 1 );
   afficherAxes( );

   // partie 1: afficher la surface en mono ou en stéréo
   definirProjection( 0 );
   afficherModele();

   glutSwapBuffers();
}

// fonction de redimensionnement de la fenêtre graphique
void redimensionnement( GLsizei w, GLsizei h )
{
   g_largeur = w;
   g_hauteur = h;
   glViewport( 0, 0, w, h );
   glutPostRedisplay();
}

void clavier( unsigned char touche, int x, int y )
{
   switch ( touche )
   {
   case '\e': // escape
      glutDestroyWindow( g_feneID );
      exit( 0 );
      break;

   case 'g':
      {
         static bool modePlein = true;
         modePlein = !modePlein;
         if ( modePlein )
            glPolygonMode( GL_FRONT_AND_BACK, GL_FILL );
         else
            glPolygonMode( GL_FRONT_AND_BACK, GL_LINE );
      }
      glutPostRedisplay();
      break;

   case 'x': // permutation de l'activation des nuanceurs
      utiliseNuanceurs = !utiliseNuanceurs;
      std::cout << "// Utilisation des nuanceurs ? " << ( utiliseNuanceurs ? "OUI" : "NON" ) << std::endl;
      glutPostRedisplay();
      break;

   case 'v': // Recharger les nuanceurs
      chargerNuanceurs();
      std::cout << "// Recharger nuanceurs" << std::endl;
      glutPostRedisplay();
      break;

   case 'l': // local viewer
      localViewer = !localViewer;
      std::cout << " localViewer=" << localViewer << std::endl;
      glLightModeli( GL_LIGHT_MODEL_LOCAL_VIEWER, localViewer );
      glutPostRedisplay();
      break;

   case 's': // affichage stereo
      affichageStereo++;
      if ( affichageStereo > 2 ) affichageStereo = 0;
      std::cout << " affichageStereo=" << affichageStereo << std::endl;
      glutPostRedisplay();
      break;

   case '.': // fonction à afficher
      indiceFonction++;
      if ( indiceFonction > 3 ) indiceFonction = 0;
      std::cout << " indiceFonction=" << indiceFonction << std::endl;
      glutPostRedisplay();
      break;

   case 'c': // indice de la texture à utiliser pour la couleur
      indiceCouleur++;
      if ( indiceCouleur > 9 ) indiceCouleur = 0;
      std::cout << " indiceCouleur=" << indiceCouleur << std::endl;
      glutPostRedisplay();
      break;

   case 'd': // indice de la texture à utiliser pour le déplacement (remplace les touches 1 à 9)
      indiceTexture++;
      if ( indiceTexture > 9 ) indiceTexture = 0;
      std::cout << " indiceTexture=" << indiceTexture << std::endl;
      glutPostRedisplay();
      break;

   case '0':
   case '1':
   case '2':
   case '3':
   case '4':
   case '5':
   case '6':
   case '7':
   case '8':
   case '9': // indice de la texture à utiliser pour le déplacement
      indiceTexture = touche - '0';
      std::cout << " indiceTexture=" << indiceTexture << std::endl;
      glutPostRedisplay();
      break;

   case '-':
      facteurZ -= 0.01;
      std::cout << " facteurZ=" << facteurZ << std::endl;
      glutPostRedisplay();
      break;

   case '+':
   case '=':
      facteurZ += 0.01;
      std::cout << " facteurZ=" << facteurZ << std::endl;
      glutPostRedisplay();
      break;

   case 'p':
      positionnelle = !positionnelle;
      std::cout << " positionnelle=" << positionnelle << std::endl;
      glutPostRedisplay();
      break;
   }
}

void clavierSpecial( int touche, int x, int y )
{
   switch ( touche )
   {
   case GLUT_KEY_LEFT:
      posX += 0.1;
      glutPostRedisplay();
      break;
   case GLUT_KEY_RIGHT:
      posX -= 0.1;
      glutPostRedisplay();
      break;
   case GLUT_KEY_UP:
      posY += 0.1;
      glutPostRedisplay();
      break;
   case GLUT_KEY_DOWN:
      posY -= 0.1;
      glutPostRedisplay();
      break;
   case GLUT_KEY_PAGE_UP:
      posZ += 0.1;
      glutPostRedisplay();
      break;
   case GLUT_KEY_PAGE_DOWN:
      posZ -= 0.1;
      glutPostRedisplay();
      break;
   }
}

// fonction callback pour un clic de souris
int dernierX = 0; // la dernière valeur en X de position de la souris
int dernierY = 0; // la derniere valeur en Y de position de la souris
static enum { deplaceCam, deplaceLumHauteur, deplaceLum } deplace = deplaceCam;
void sourisClic( int button, int state, int x, int y )
{
   if ( state == GLUT_DOWN )
   {
      // on vient de presser la souris
      dernierX = x;
      dernierY = y;
      switch ( button )
      {
      case GLUT_LEFT_BUTTON:
         deplace = deplaceCam;
         break;
      case GLUT_MIDDLE_BUTTON:
         deplace = deplaceLumHauteur;
         break;
      case GLUT_RIGHT_BUTTON:
         deplace = deplaceLum;
         break;

      case 3: // roulette de la souris vers le bas
         facteurZ -= 0.01;
         std::cout << " facteurZ=" << facteurZ << std::endl;
         glutPostRedisplay();
         break;
      case 4: // roulette de la souris vers le haut
         facteurZ += 0.01;
         std::cout << " facteurZ=" << facteurZ << std::endl;
         glutPostRedisplay();
         break;
      }
   }
   else
   {
      // on vient de relacher la souris
   }
}

// fonction de mouvement de la souris
void sourisMouvement( int x, int y )
{
   int dx = x - dernierX;
   int dy = y - dernierY;
   switch ( deplace )
   {
   case deplaceCam:
      phi   += dx / 80.0;
      theta += dy / 50.0;
      break;
   case deplaceLumHauteur:
      posZ -= dy * 0.01;
      break;
   case deplaceLum:
      posX += dx * 0.01;
      posY += dy * 0.01;
      break;
   }

   dernierX = x;
   dernierY = y;

   if ( theta > MAXTHETA )
      theta = MAXTHETA;
   if ( theta < MINTHETA )
      theta = MINTHETA;
   if ( phi > 2*M_PI )
      phi = 0;
   if ( phi < -2*M_PI)
      phi = 0;

   glutPostRedisplay();
}

int main( int argc, char *argv[] )
{
   // initialisation de GLUT
   glutInit( &argc, argv );
   // paramétrage de l'affichage
   glutInitDisplayMode( GLUT_RGBA | GLUT_DEPTH | GLUT_DOUBLE );
   // taille de la fenetre
   glutInitWindowSize( g_largeur, g_hauteur );
   // création de la fenêtre
   g_feneID = glutCreateWindow( "INF2705" );

   // référencement de la fonction de rappel pour l'affichage
   glutDisplayFunc( afficherScene );
   // référencement de la fonction de rappel pour le redimensionnement
   glutReshapeFunc( redimensionnement );
   // référencement de la fonction de gestion des touches
   glutKeyboardFunc( clavier );
   // référencement de la fonction de gestion des touches spéciales
   glutSpecialFunc( clavierSpecial );
   // référencement de la fonction de rappel pour le mouvement de la souris
   glutMotionFunc( sourisMouvement );
   // référencement de la fonction de rappel pour le clic de la souris
   glutMouseFunc( sourisClic );

   glewInit();
   printGLStrings( );

   initialisation();

   // boucle principale de gestion des evenements
   glutMainLoop();

   // le programme n'arrivera jamais jusqu'ici
   return EXIT_SUCCESS;
}
