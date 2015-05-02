// Prénoms, noms et matricule des membres de l'équipe:
// - Prénom1 NOM1 (matricule1)
// - Prénom2 NOM2 (matricule2)

#include <iostream>
#include <GL/glew.h>
#include <GL/glut.h>
#include "varglob.h"
#include "float3.h"
#include "teapot_data.h"

// partie 1:
double thetaBras = 0.0;   // angle de rotation du bras
double phiBras = 0.0;     // angle de rotation du bras
double phiTea = 0.0;     // angle de rotation du bras
double phiCamera = -18.0;
double thetaCamera = 0.0;

// partie 2:
GLdouble MAXPHI = M_PI - 0.001, MAXTHETA = M_PI - 0.001;
GLdouble MINPHI = 0.001, MINTHETA = 0.001;

GLuint g_VBOsommets = 0;
GLuint g_VBOconnec = 0;

bool modeLookAt = true;

GLuint vboSommetId;
GLuint vboConnecId;
void creerVBO()
{
   // créer le VBO pour les sommets
	/*
   glEnableClientState(GL_VERTEX_ARRAY);
   glVertexPointer(3, GL_FLOAT, 0, gTeapotSommets);
   glDrawArrays(GL_QUADS, 0, 530*3);
  

   // créer le VBO la connectivité
   glDrawElements(GL_QUADS, sizeof(gTeapotConnec)/sizeof(GLuint), GL_UNSIGNED_INT, gTeapotConnec);
   glDisableClientState(GL_VERTEX_ARRAY);
   */
	glGenBuffers(1, &vboSommetId);
	glBindBuffer(GL_ARRAY_BUFFER, vboSommetId);
	glBufferData(GL_ARRAY_BUFFER, sizeof(gTeapotSommets), gTeapotSommets, GL_STATIC_DRAW);
	//glDeleteBuffers(1, &vboSommetId);
	 glBindBuffer(GL_ARRAY_BUFFER, 0);
	
	glGenBuffers(1, &vboConnecId);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, vboConnecId);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(gTeapotConnec), gTeapotConnec, GL_STATIC_DRAW);
	//glDeleteBuffers(1, &vboConnecId);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	
}

void initialisation()
{
   theta = 0.;
   phi = 1.;
   dist = 10.;

   // donner la couleur de fond
   glClearColor( 0.0, 0.0, 0.0, 1.0 );

   // activer les etats openGL
   glEnable( GL_DEPTH_TEST );

   glewInit();

   creerVBO();
}

// (partie 1) Vous devez vous servir de ces fonctions pour tracer les quadriques.
void afficherCylindre( )
{
   // affiche un cylindre de rayon 1 et de hauteur 1
   static GLUquadric* q = 0;
   if ( !q ) q = gluNewQuadric();
   const GLint slices = 16, stack = 2;
   glColor3f( 0, 0, 1 );
   gluCylinder( q, 1.0, 1.0, 1.0, slices, stack );
}
void afficherSphere( )
{
   // affiche une sphere de rayon 1
   static GLUquadric* q = 0;
   if ( !q ) q = gluNewQuadric();
   const GLint slices = 16, stack = 32;
   glColor3f( 1, 0, 0 );
   gluSphere( q, 1.0, slices, stack );
}

// (partie 2) Vous modifierez cette fonction pour utiliser les VBOs
void afficherTheiere()
{
   glColor3f( 0.0, 1.0, 0.0 );
   
   glBindBuffer(GL_ARRAY_BUFFER, vboSommetId);
   glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, vboConnecId);
   
   glEnableClientState(GL_VERTEX_ARRAY);
   
   glVertexPointer(3, GL_FLOAT, 0, 0);
   glDrawElements(GL_TRIANGLES,sizeof(gTeapotConnec)/sizeof(GLuint), GL_UNSIGNED_INT, 0);
   
   glDisableClientState(GL_VERTEX_ARRAY);
   
   glBindBuffer(GL_ARRAY_BUFFER, 0);
   glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
   
      //	std::cout << "FUCK YOU JE DESSINE PAS" << "\r\n";
}

void afficherBras()
{
   const GLfloat cylhauteur = 2.0;
   const GLfloat sphererayon = 0.25;
   
   glPushMatrix();

   // Premier cylindre"
   glScalef(sphererayon,sphererayon,cylhauteur);
   afficherCylindre();
   
   // Première sphère
   glPopMatrix();
   glTranslatef(0.0, 0.0, cylhauteur + sphererayon/3 );
   glPushMatrix();
   glScalef(sphererayon,sphererayon,sphererayon);
   afficherSphere();
   
   // Premier cylindre couché avec rotation theta
   glPopMatrix();
   glRotatef(90,1,0,0);
   glRotatef(thetaBras,0,1,0);
   glPushMatrix();
   glScalef(sphererayon,sphererayon,cylhauteur);
   glTranslatef(0.0, 0.0, sphererayon/3 );
   afficherCylindre();
   
   // Sphere de jointur entre les 2 bras
   glPopMatrix();
   glTranslatef(0.0, 0.0, cylhauteur + sphererayon);
   glPushMatrix();
   glScalef(sphererayon,sphererayon,sphererayon);
   afficherSphere();
   
   // 2e bras articulé (phi)
   glPopMatrix();
   glTranslatef(0.0, 0.0, sphererayon/3);
   glRotatef(phiBras,0,1,0);
   glPushMatrix();
   glScalef(sphererayon,sphererayon,cylhauteur);
   afficherCylindre();
   
   // Articulation avec le teapot
    glPopMatrix();
    glTranslatef(0.0, 0.0, cylhauteur + sphererayon/3);
    glPushMatrix();
    glScalef(sphererayon,sphererayon,sphererayon);
    afficherSphere();
    
   // afficher la théière
    glPopMatrix();
    glRotatef(-90,0,1,0);
    glRotatef(phiTea,0,0,1);
    glTranslatef(sphererayon*3, -0.3, 0.0);
    glScalef(0.15,0.15,0.15);
    afficherTheiere();

}



void definirCamera()
{
	if(modeLookAt)
	{
		gluLookAt( cos(DEG2RAD(thetaCamera))*cos(DEG2RAD(phiCamera))*dist, sin(DEG2RAD(thetaCamera))*cos(DEG2RAD(phiCamera))*dist, sin(DEG2RAD(phiCamera))*dist,  0, 0, 0,  0, 0, 1 );
	}
	else
	{	
		glTranslatef(0, 0, -dist);
		glRotatef(90, 0.0, 1.0, 0.0 );
		glRotatef(-90, 1.0, 0.0, 0.0 );
		glRotatef( thetaCamera, 0.0, 0.0, 1.0 );
		glRotatef( phiCamera, 0.0, 1.0, 0.0 );
		
	}
}

void afficherScene()
{
   glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );

   glMatrixMode( GL_PROJECTION );
   glLoadIdentity();
   gluPerspective( 45.0, (GLdouble) g_largeur / (GLdouble) g_hauteur, 0.1, 300.0 );

   glMatrixMode( GL_MODELVIEW );
   glLoadIdentity();

   definirCamera();

   glColor3f( 1., .5, .5 );
   glBegin( GL_QUADS );
   glVertex3f( -4.0,  4.0, 0.0 );
   glVertex3f(  4.0,  4.0, 0.0 );
   glVertex3f(  4.0, -4.0, 0.0 );
   glVertex3f( -4.0, -4.0, 0.0 );
   glEnd();

   afficherBras();

   glutSwapBuffers();
}

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
   case '-':
   case '_':
		if(dist < 14)
		{
			dist += 0.1;
		}
	  break;
   case '+':
   case '=':
      if ( dist > 1.0 )
         dist -= 0.1;
      break;
   case 'r':
      break;
   case 'l':
      modeLookAt = !modeLookAt;
      std::cout << " modeLookAt=" << modeLookAt << std::endl;
      break;
   case 'g':
      {
         static bool wireframe = true;
	if(wireframe){
	glPolygonMode( GL_FRONT_AND_BACK, GL_FILL );
	wireframe = false;
	}
	else 
	{
	glPolygonMode( GL_FRONT_AND_BACK, GL_LINE );
	wireframe = true;
	}
      }
      break;
   case ']':
     phiTea += 2.0;
     break;
  case '[':
     phiTea -= 2.0;
     break;
   }
   phiTea = CLIP(phiTea,-90,90);
   
   glutPostRedisplay();
}

void clavierSpecial( int touche, int x, int y )
{
   switch ( touche )
   {
   case GLUT_KEY_LEFT:
      phiBras -= 2.0;
      break;
   case GLUT_KEY_RIGHT:
      phiBras += 2.0;
      break;
   case GLUT_KEY_DOWN:
      thetaBras -= 2.0;
      break;
   case GLUT_KEY_UP:
      thetaBras += 2.0;
      break;
   }
   phiBras = CLIP(phiBras,-RAD2DEG(MAXPHI)/2,RAD2DEG(MAXPHI)/2);
  // thetaBras = CLIP(thetaBras,RAD2DEG(MINTHETA),RAD2DEG(MAXTHETA));
   glutPostRedisplay();
}



int dernierX, dernierY;
void sourisClic( int button, int state, int x, int y )
{
   // button est un parmi { GLUT_LEFT_BUTTON, GLUT_MIDDLE_BUTTON, GLUT_RIGHT_BUTTON }
   // state  est un parmi { GLUT_DOWN, GLUT_UP }
   // Pour référence et quelques exemples, voir http://www.lighthouse3d.com/opengl/glut/index.php?9
   if ( state == GLUT_DOWN )
   {
      dernierX = x;
      dernierY = y;
   }
}

int interpolate(int value, int min_value, int max_value, int min, int max)
{
	double proportion =(double)( (value-min_value)) / max_value;
	proportion = CLIP(proportion, 0, 1);
	return (max-min)*proportion + min;
}

void sourisMouvement( int x, int y )
{
   dernierX = x;
   dernierY = y;
   phiCamera = interpolate(y, 0, g_hauteur, -90, 90) ;
   thetaCamera = interpolate(x, 0, g_largeur, -180, 180) ;
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

   initialisation();

   // boucle principale de gestion des evenements
   glutMainLoop();

   // le programme n'arrivera jamais jusqu'ici
   return EXIT_SUCCESS;
}
