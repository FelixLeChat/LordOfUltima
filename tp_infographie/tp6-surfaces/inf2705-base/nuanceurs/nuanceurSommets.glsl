uniform int indiceFonction;
uniform int localViewer;
uniform int utiliseTexture;

uniform float facteurZ;
uniform sampler2D displacementMap;
varying vec3 normal, lightDir, eyeVec;

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
   float x = gl_Vertex[0];
   float y = gl_Vertex[1];
   
   vec4 position = vec4(gl_Vertex.x, gl_Vertex.y, Fonc(gl_Vertex.x, gl_Vertex.y), 1.0);
  // position.xyz = gl_Vertex.xyz;

   //position[2]= Fonc(x, y);

   // Calculer normale
    float de = 0.0001;
    float dx = (Fonc(x+de, y)- Fonc(x-de,y))/(2.0*de);
    float dy = (Fonc(x, y+de)- Fonc(x,y-de))/(2.0*de);
    normal[0] = dx;
    normal[1] = dy;
    normal[2] = -1.0;
   
    normal = gl_NormalMatrix*normalize(normal);

   vec3 ecPosition = vec3( gl_ModelViewMatrix * position );
   // vecteur de la direction de la lumière
   lightDir = vec3( gl_LightSource[0].position.xyz - ecPosition );
   eyeVec = -ecPosition; // vecteur qui pointe vers le (0,0,0), c'est-à-dire vers l'oeil


   gl_Position = gl_ModelViewProjectionMatrix * position;

   // calculer de la position du sommet dans l'espace de l'oeil afin que la carte fasse le découpage demandée par glClipPlane()
   // (On doit initialiser la variable gl_ClipVertex pour que le découpage soit fait par OpenGL.)
   gl_ClipVertex = gl_ModelViewMatrix * position;
    // calculer la position du sommet dans l'espace de la caméra ("eye-coordinate position")

}
