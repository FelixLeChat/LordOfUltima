// Prénoms, noms et matricule des membres de l'équipe:
// - Felix La Rocque Carrier(1621348)
// - Mathieu Gamache (1626377)
//#error "Écrire les Prénoms, noms et matricule des membres de l'équipe dans le fichier et commenter cette ligne"

#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <GL/glew.h>
#include <GL/glut.h>
#include "varglob.h"
#include "float3.h"
#include "matrice.h"
#include "fctsUtiles.h"

class CorpsCeleste
{
public:
   float Rayon; // le rayon
   float Distance; // la distance au soleil
   float Rotation; // l'angle actuel de rotation en degrés
   float Revolution; // l'angle actuel de révolution
   float IncrRotation; // l'incrément à ajouter à chaque appel de la fonction animer
   float IncrRevolution; // l'incrément à ajouter à chaque appel de la fonction animer
   CorpsCeleste( float rayon, float distance, float rotation, float revolution, float incrRotation, float incrRevolution ) :
      Rayon(rayon), Distance(distance),
      Rotation(rotation), Revolution(revolution),
      IncrRotation(incrRotation), IncrRevolution(incrRevolution)
   { }
};

CorpsCeleste Soleil( 4.0, 0.0, 5.0, 0.0, 0.01, 0.0 );
CorpsCeleste Terre( 0.5, 8.0, 20.0, 45.0, 0.5, 0.005 );
CorpsCeleste Lune( 0.2, 1.0, 20.0, 30.0, 0.5, 0.07 );
CorpsCeleste Jupiter( 1.2, 14.0, 10.0, 30.0, 0.03, 0.003 );
CorpsCeleste Europa( 0.2, 1.5, 5.0, 15.0, 0.7, 0.09 );
CorpsCeleste Callisto( 0.3, 2.0, 10.0, 2.0, 0.8, 0.7 );

int modele = 1; // le modèle à afficher (1-sphère, 2-cube, 3-théière)

GLdouble equation[] = { 0, 0, 1, 0 }; // équation du plan de coupe

const float MAXPHI = M_PI - 0.001, MINPHI = 0.001;

GLUquadric *sphere = NULL;

int vueCourante = 0; // 0 pour la vue exoplanétaire; 1 pour la vue à partir du pôle Nord
float facteurRechauffement = 0.2; // la facteur qui servira à calculer la couleur des pôles (0.0=froid, 1.0=chaud)

static void animer( int tempsPrec )
{
   // obtenir le temps depuis le début du programme, en millisecondes
   int tempsCour = glutGet( GLUT_ELAPSED_TIME );
   if ( tempsPrec == 0 ) tempsPrec = tempsCour;

   // temps d'attente en secondes avant le prochain affichage
   const int FPS = 60;  // en "images/seconde"
   const int delai = 1000/FPS;  // en "millisecondes/image" (= 1000 millisecondes/seconde  /  images/seconde)
   if ( enmouvement ) glutTimerFunc( delai, animer, tempsCour );

   // incrémenter Rotation[] et Revolution[] pour faire tourner les planètes
   // modifs ici ...
   Soleil.Rotation += Soleil.IncrRotation;
   Soleil.Revolution += Soleil.IncrRevolution;
   
   Terre.Rotation += Terre.IncrRotation;
   Terre.Revolution += Terre.IncrRevolution;
   
   Lune.Rotation += Lune.IncrRotation;
   Lune.Revolution += Lune.IncrRevolution;
   
   Jupiter.Rotation += Jupiter.IncrRotation;
   Jupiter.Revolution += Jupiter.IncrRevolution;

   Europa.Rotation += Europa.IncrRotation;
   Europa.Revolution += Europa.IncrRevolution;
   
   Callisto.Rotation += Callisto.IncrRotation;
   Callisto.Revolution += Callisto.IncrRevolution;
   // indiquer qu'il faut afficher à nouveau
   glutPostRedisplay();
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
   enmouvement = true;

   phi = DEG2RAD(75.0), theta = DEG2RAD(60.0);
   dist = 26.0;
   utiliseNuanceurs = false;
   sphere = gluNewQuadric();

   // donner la couleur de fond
   glClearColor( 0.2, 0.2, 0.2, 1.0 );

   // activer les etats openGL
   glEnable( GL_DEPTH_TEST );

   // charger les nuanceurs
   chargerNuanceurs();
}

void observerDeLaTerre( )
{
   glMatrixMode( GL_MODELVIEW );
   glPushMatrix();
	glRotatef(Terre.Revolution, 0.0, 0.0, 1.0);
	glTranslatef(Terre.Distance, 0, 0 );
	glRotatef(Terre.Rotation, 0.0, 0.0, 1.0);


   GLdouble matrice[16];
   GLdouble matrice_copy[16];

   // Optient une copie de la matrice MODELVIEW
   glGetDoublev(GL_MODELVIEW_MATRIX, matrice);
   Matrice::inverse(matrice_copy, matrice);
   glPopMatrix();

   // On met une nouvelle matrice MODELVIEW

   
   glRotatef(90, -1, 0, 0);
   glTranslatef(0, 0,-Terre.Rayon-0.05);
   glMultMatrixd(matrice_copy);

}

void definirCamera()
{
   if ( vueCourante == 0 )
   {
      // La souris influence le point de vue
      gluLookAt( dist*cos(theta)*sin(phi), dist*sin(theta)*sin(phi), dist*cos(phi),
                 0, 0, 0,
                 0, 0, 1 );
   }
   else
   {
      // La caméra est sur la Terre et voir passer les autres objets célestes en utilisant l'inverse de la matrice mm
      observerDeLaTerre( );
   }
}

void afficherCorpsCeleste( GLfloat rayon )
{
   switch ( modele )
   {
   case 1:
      gluSphere( sphere, rayon, 16, 16 );
      break;
   case 2:
      glutSolidCube( 2*rayon );
      break;
   case 3:
   default:
      glPushMatrix(); {
         glFrontFace( GL_CW ); // Les polygones de la theiere ont leur face avant vers l'interieur
         glRotatef( 90, 1, 0, 0 ); // révolution terre autour soleil
         glutSolidTeapot( rayon );
         glFrontFace( GL_CCW );
      } glPopMatrix();
      break;
   }
}

void afficherDisque( GLfloat distance )
{
   glutSolidTorus( 0.1, distance, 4, 64 );
}

void afficherModele()
{
   // s'il y a lieu, activer les nuanceurs
   if ( utiliseNuanceurs )
   {
      glUseProgram( progNuanceur );
      glUniform1f( glGetUniformLocation( progNuanceur, "facteurRechauffement" ), facteurRechauffement );
   }
   else
      glUseProgram( 0 );

   // afficher le système solaire
   glPushMatrix( ); {

	// Revolution Terre et Lune
	 glPushMatrix();
	   glRotatef(Terre.Revolution, 0.0, 0.0, 1.0);
	   glTranslatef(Terre.Distance, 0, 0 );
	 
	   // Rotation Terre
	   glPushMatrix();
	     glRotatef(Terre.Rotation, 0.0, 0.0, 1.0);
             glColor3f( 0.5, 0.5, 1.0 );
             afficherCorpsCeleste( Terre.Rayon );
	   glPopMatrix();
	 
	  // Revolution Lune
	  glPushMatrix();
	   glRotatef(Lune.Revolution, 0.0, 0.0, 1.0);
           glTranslatef(Lune.Distance, 0, 0 );
	   // Revolution Lune
      	     glPushMatrix();
	       glRotatef(Lune.Rotation, 0.0, 0.0, 1.0);
               glColor3f( 0.6, 0.6, 0.6 );
               afficherCorpsCeleste( Lune.Rayon );
	     glPopMatrix();
	   glPopMatrix();
	 
	 glPopMatrix();

	  glColor3f(0.0,0.0,1.0);
	  afficherDisque( Terre.Distance );
	  glColor3f(1.0,0.0,0.0);
	  afficherDisque( Jupiter.Distance );

	// Revolution Jupiter
	 glPushMatrix();
	   glRotatef(Jupiter.Revolution, 0.0, 0.0, 1.0);
	   glTranslatef(Jupiter.Distance, 0, 0 );
	 
	   // Rotation Jupiter
	   glPushMatrix();
	     glRotatef(Jupiter.Rotation, 0.0, 0.0, 1.0);
             glColor3f( 1.0, 0.5, 0.5 );
             afficherCorpsCeleste( Jupiter.Rayon );
	   glPopMatrix();
	 
	  // Revolution Europa
	  glPushMatrix();
	   glRotatef(Europa.Revolution, 0.0, 0.0, 1.0);
           glTranslatef(Europa.Distance, 0, 0 );
	   // Revolution Europa
      	     glPushMatrix();
	       glRotatef(Europa.Rotation, 0.0, 0.0, 1.0);
               glColor3f( 0.4, 0.4, 0.8 );
               afficherCorpsCeleste( Europa.Rayon );
	     glPopMatrix();
	   glPopMatrix();
	   
	  // Revolution Callisto
	  glPushMatrix();
	   glRotatef(Callisto.Revolution, 0.0, 0.0, 1.0);
           glTranslatef(Callisto.Distance, 0, 0 );
	   // Revolution Callisto
      	     glPushMatrix();
	       glRotatef(Callisto.Rotation, 0.0, 0.0, 1.0);
               glColor3f( 0.5, 0.5, 0.1 );
               afficherCorpsCeleste( Callisto.Rayon );
	     glPopMatrix();
	   glPopMatrix();
	 
	 glPopMatrix();
	 
	          // Rotation et revolution du soleil!
	 glPushMatrix();
	   glTranslatef(Soleil.Revolution, 0.0, 0.0);
	 
	   glPushMatrix();
	     glRotatef(Soleil.Rotation, 0.0, 0.0, 1.0);
	     
	     glEnable(GL_BLEND);
	     glBlendFunc(GL_ONE, GL_ONE);
             glColor4f( 1.0, 1.0, 0.0, 0.5 );
             afficherCorpsCeleste( Soleil.Rayon );
	     glDisable(GL_BLEND);
	   glPopMatrix();
	 glPopMatrix();
	 

	
   } glPopMatrix( );
}

void afficherScene( )
{
   glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT );

   glMatrixMode( GL_PROJECTION );
   glLoadIdentity( );
   gluPerspective( 70.0, (GLdouble) g_largeur / (GLdouble) g_hauteur, 0.1, 100.0 );

   glMatrixMode( GL_MODELVIEW );
   glLoadIdentity( );

   definirCamera( );

   // dessiner la scène
   afficherAxes();
   
   // Plan de coupe et lineloop
   glEnable( GL_CLIP_PLANE0 );
   glClipPlane(GL_CLIP_PLANE0, equation);
   glColor3f(1.0, 1.0, 1.0);
   glBegin(GL_LINE_LOOP);
   glVertex3f(15,15,-equation[3]+0.001);
   glVertex3f(-15,15,-equation[3]+0.001);
   glVertex3f(-15,-15,-equation[3]+0.001);
   glVertex3f(15,-15,-equation[3]+0.001);
   glEnd();
   
   
   glEnable(GL_STENCIL_TEST);
   
   
   // On met a 1 dès qu'on voit le modèle
   glStencilFunc(GL_NEVER, 1, 1);
   glStencilOp(GL_REPLACE, GL_KEEP, GL_KEEP);
   afficherModele();
   
   // On met à zéro si on voit la face extérieur
   glEnable(GL_CULL_FACE);
   glStencilFunc(GL_NEVER, 0, 1);
   glStencilOp(GL_REPLACE, GL_KEEP, GL_KEEP);
   afficherModele();
   glDisable(GL_CULL_FACE);
   
   
  
   // On affiche la base verte si la valeur est 1
   glStencilFunc(GL_EQUAL, 1, 1);
   glStencilOp(GL_KEEP, GL_KEEP, GL_KEEP);

   glBegin( GL_QUADS );
   glColor3f(0.0, 1.0, 0.0);
   glVertex3f( -15, 15, -equation[3]+0.001); 
   glVertex3f( -15, -15, -equation[3]+0.001);
   glVertex3f( 15, -15, -equation[3]+0.001); 
   glVertex3f( 15, 15, -equation[3]+0.001);
   glEnd();
   

   glDisable(GL_STENCIL_TEST);
   

   afficherModele();
   glutSwapBuffers( );
}

void redimensionnement( GLsizei w, GLsizei h )
{
   g_largeur = w;
   g_hauteur = h;
   glViewport( 0, 0, w, h );
   glutPostRedisplay();
}

void verifierAngles()
{
   if ( theta > 2*M_PI )
      theta -= 2*M_PI;
   else if ( theta < -2*M_PI )
      theta += 2*M_PI;
   if ( phi > MAXPHI )
      phi = MAXPHI;
   else if ( phi < MINPHI )
      phi = MINPHI;
}

void clavier( unsigned char touche, int x, int y )
{
   switch ( touche )
   {
   case '\e': // escape
   case 'q':
      glutDestroyWindow( g_feneID );
      exit( 0 );
      break;
   case ' ':
      enmouvement = !enmouvement;
      if ( enmouvement ) animer( 0 );
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

   case 'c':
      facteurRechauffement += 0.05; if ( facteurRechauffement > 1.0 ) facteurRechauffement = 1.0;
      std::cout << " facteurRechauffement=" << facteurRechauffement << " " << std::endl;
      break;
   case 'f':
      facteurRechauffement -= 0.05; if ( facteurRechauffement < 0.0 ) facteurRechauffement = 0.0;
      std::cout << " facteurRechauffement=" << facteurRechauffement << " " << std::endl;
      break;

   case '0': // point de vue globale/externe
   case '1': // point de vue situé au pôle Nord
   case '2': // ... autres point de vue
   case '3':
   case '4':
   case '5':
   case '6':
   case '7':
   case '8':
   case '9':
      vueCourante = touche - '0';
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
      break;

   case '+':
   case '=':
      dist--;
      std::cout << " dist=" << dist << std::endl;
      break;

   case '_':
   case '-':
      dist++;
      std::cout << " dist=" << dist << std::endl;
      break;

   case '>':
      equation[3] += 0.1;
      std::cout << " equation[3]=" << equation[3] << std::endl;
      glutPostRedisplay();
      break;
   case '<':
      equation[3] -= 0.1;
      std::cout << " equation[3]=" << equation[3] << std::endl;
      glutPostRedisplay();
      break;

   case ';':
      if ( ++modele > 3 ) modele = 1;
      std::cout << " modele=" << modele << std::endl;
      glutPostRedisplay();
      break;
   }
}

void clavierSpecial( int touche, int x, int y )
{
}

bool sourisMouvementActif = false;
int dernierX = 0, dernierY = 0;
void sourisClic( int button, int state, int x, int y )
{
   if ( button == GLUT_LEFT_BUTTON && state == GLUT_DOWN )
   {
      dernierX = x;
      dernierY = y;
      sourisMouvementActif = true;
   }
   else
      sourisMouvementActif = false;
}

void sourisMouvement( int x, int y )
{
   if ( sourisMouvementActif )
   {
      theta += (x-dernierX) / 50.0;
      phi += (y-dernierY) / 50.0;
      dernierX = x;
      dernierY = y;
      verifierAngles();
   }
}

int main( int argc, char *argv[] )
{
   // initialisation de GLUT
   glutInit( &argc, argv );
   // paramétrage de l'affichage
   glutInitDisplayMode( GLUT_RGBA | GLUT_DEPTH | GLUT_DOUBLE | GLUT_STENCIL );
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

   initialisation();
   if ( enmouvement ) animer( 0 );

   // boucle principale de gestion des evenements
   glutMainLoop();

   // le programme n'arrivera jamais jusqu'ici
   return EXIT_SUCCESS;
}
