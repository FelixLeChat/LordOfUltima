uniform int indiceFonction;
uniform int localViewer;
uniform int indiceTexture;
uniform int indiceCouleur;

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
   vec4 position;
   float de = 0.0001;
   gl_TexCoord[0] = gl_MultiTexCoord0;
   vec4 textureColor = texture2D( displacementMap, gl_TexCoord[0].st);

   
   if(indiceTexture == 0)
   {
   	   position = vec4(gl_Vertex.x, gl_Vertex.y, Fonc(gl_Vertex.x, gl_Vertex.y), 1.0);
   	    // Calculer normale
	    
	    float dx = (Fonc(x+de, y)- Fonc(x-de,y))/(2.0*de);
	    float dy = (Fonc(x, y+de)- Fonc(x,y-de))/(2.0*de);
	    normal[0] = dx;
	    normal[1] = dy;
	    normal[2] = -1.0;
   }
   else
   {
 	   position = vec4(gl_Vertex.x, gl_Vertex.y,  textureColor[0]*facteurZ , 1.0);
 	   float eps = 0.01;

 	   vec2 deltaX1 = vec2(gl_TexCoord[0].s +eps, gl_TexCoord[0].t);
	   vec2 deltaX2 = vec2(gl_TexCoord[0].s -eps, gl_TexCoord[0].t);
 	   vec4 textureColorDelta1 = texture2D( displacementMap, deltaX1);
 	   vec4 textureColorDelta2 = texture2D( displacementMap, deltaX2);
 	   float dx = (textureColorDelta1[0] - textureColorDelta2[0])/(2.0*de);

 	   vec2 deltaY1 = vec2(gl_TexCoord[0].s, gl_TexCoord[0].t + eps);
	   vec2 deltaY2 = vec2(gl_TexCoord[0].s, gl_TexCoord[0].t - eps);
 	   textureColorDelta1 = texture2D( displacementMap, deltaY1);
 	   textureColorDelta2 = texture2D( displacementMap, deltaY2);
 	   float dy = (textureColorDelta1[0] - textureColorDelta2[0])/(2.0*de);


	   //textureColor = texture2D(avec coor modif)
	   normal[0] = dx;
	   normal[1] = dy;
	   normal[2] = -1.0;
   }


   
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
