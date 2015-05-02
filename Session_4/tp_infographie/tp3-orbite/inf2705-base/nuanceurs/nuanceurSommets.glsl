// les étudiants peuvent utiliser l'exemple du cours pour démarrer:
//    http://www.cours.polymtl.ca/inf2705/nuanceurs/exempleSimple/

#define M_PI_2  (1.57079632679489661923)  // PI/2

varying float latitude;
varying vec4 color;
varying float rayon;
uniform float facteurRechauffement;


void main( void )
{
   // transformation standard du sommet (ModelView et Projection)
   //gl_Position = gl_ProjectionMatrix * gl_ModelViewMatrix * gl_Vertex;
   gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

   // position
   vec4 ecPos = gl_Vertex;

   gl_FrontColor = gl_Color;
   gl_BackColor = 1.0 - gl_Color;

   color = gl_FrontColor;

   rayon = sqrt(pow(ecPos.x,2) + pow(ecPos.y,2) + pow(ecPos.z,2));

   latitude = abs(ecPos.z/rayon);

}
