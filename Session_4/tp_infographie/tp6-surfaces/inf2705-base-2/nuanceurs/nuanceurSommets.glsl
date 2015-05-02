uniform int indiceFonction;
uniform int localViewer;
uniform int indiceTexture;

uniform float facteurZ;
uniform sampler2D displacementMap;

float Fonc( float x, float y )
{
   float z = 0.0;
   if ( indiceFonction == 0 )
      z = ( y*y - x*x );
   else if ( indiceFonction == 1 )
      z = 2.0 * x*y;
   else if ( indiceFonction == 2 )
      z = ( y*sin(2.0*x) * x*cos(2.0*y) );
   else if ( indiceFonction == 3 )
      z = 5.0 * (x*y) / exp(x*x + y*y);
   return facteurZ * z;
}

void main( void )
{
   // transformation standard du sommet (ModelView et Projection)
   gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

   // calculer de la position du sommet dans l'espace de l'oeil afin que la carte fasse le découpage demandée par glClipPlane()
   // (On doit initialiser la variable gl_ClipVertex pour que le découpage soit fait par OpenGL.)
   gl_ClipVertex = gl_ModelViewMatrix * gl_Vertex;
}
