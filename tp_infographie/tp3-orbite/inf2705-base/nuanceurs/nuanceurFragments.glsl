// les étudiants peuvent utiliser l'exemple du cours pour démarrer:
//    http://www.cours.polymtl.ca/inf2705/nuanceurs/exempleSimple/

// couleur du pôle sans réchauffement
vec4 couleurPole = vec4( 1.0, 1.0, 1.0, 1.0 );

varying float latitude;
varying vec4 color;
varying float rayon;
uniform float facteurRechauffement;

void main( void )
{
   //gl_FragColor = vec4( 0.4, 0.5, 0.6, 1.0 ); // un quelconque gris bleuté

   gl_FragColor = color;

   if (rayon < 3)
   {
   		//gl_FragColor = (1-latitude)*(color*(1-facteurRechauffement))+(latitude)*((couleurPole)*(facteurRechauffement));

   		gl_FragColor += (latitude*(1-facteurRechauffement)*couleurPole);
   }

  
}
