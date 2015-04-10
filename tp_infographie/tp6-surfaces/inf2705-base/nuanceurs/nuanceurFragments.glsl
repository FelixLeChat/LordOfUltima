uniform sampler2D colorMap;
uniform int utiliseTexture, indiceTexture;
uniform int indiceCouleur;

varying vec3 normal, lightDir, eyeVec;

void main( void )
{
 // calcul de la composante ambiante
   vec4 couleur = gl_FrontLightModelProduct.sceneColor +
                  gl_LightSource[0].ambient * gl_FrontMaterial.ambient;

   // vecteur normal
   vec3 N = normalize( gl_FrontFacing ? normal : -normal );

   // direction de la lumières
   // calcul du vecteur de la surface vers la source lumineuse
   // normaliser le vecteur de la surface vers la source lumineuse
   vec3 L = normalize( lightDir );

   vec3 D = normalize(gl_LightSource[0].spotDirection);

   // produit scalaire pour le calcul de la réflexion diffuse
   // normale . direction de la lumière
   float NdotL = dot( N, L );

   float intensiteSpeculaire = 0.0;

	// calcul de l'éclairage seulement si le produit scalaire est positif
	if ( NdotL > 0.0)
	{
		// calcul de la composante diffuse
		couleur += (gl_LightSource[0].diffuse * gl_FrontMaterial.diffuse * NdotL);

		// Parametre pour Blinn cos(phi)
		float BdotN = dot( normalize(L+ normalize(eyeVec)),N);

		intensiteSpeculaire = max( BdotN, 0.0);


		couleur += (gl_LightSource[0].specular * gl_FrontMaterial.specular * pow( intensiteSpeculaire, gl_FrontMaterial.shininess ));
	}


	vec4 textureColor = texture2D( colorMap, gl_TexCoord[0].st);
	couleur += textureColor;

	
   gl_FragColor = clamp( couleur, 0.0, 1.0 );

   
}
