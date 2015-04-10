// les étudiants peuvent utiliser l'exemple du cours pour démarrer:
//    http://www.cours.polymtl.ca/inf2705/nuanceurs/exempleIllumination/phong.glsl

// Les paramètres de la source de lumière sont définis ainsi:
// struct gl_LightSourceParameters
// {
//    vec4 ambient;
//    vec4 diffuse;
//    vec4 specular;
//    vec4 position;
//    vec4 halfVector;
//    vec3 spotDirection;
//    float spotExponent;
//    float spotCutoff;          // ( [0.0,90.0], 180.0 )
//    float spotCosCutoff;       // == cos(spotCutoff) ( [1.0,0.0], -1.0 )
//    float constantAttenuation;
//    float linearAttenuation;
//    float quadraticAttenuation;
// };

 uniform gl_LightSourceParameters gl_LightSource[gl_MaxLights];
 //bool gl_FrontFacing  // on est en train de tracer la face avant?

varying vec3 normal, lightDir, eyeVec;
uniform bool utiliseBlinn, utiliseDirect, noirTransparent;
uniform int texnumero;

// Pour Texture
uniform sampler2D laTexture;

void main( void )
{
   // calcul de la composante ambiante
   vec4 couleur = gl_FrontLightModelProduct.sceneColor +
                  gl_LightSource[0].ambient * gl_FrontMaterial.ambient;

   // vecteur normal
   vec3 N = normalize( normal );

   // direction de la lumière
   // calcul du vecteur de la surface vers la source lumineuse
   // normaliser le vecteur de la surface vers la source lumineuse
   vec3 L = normalize( lightDir );

   vec3 D = normalize(gl_LightSource[0].spotDirection);

   // produit scalaire pour le calcul de la réflexion diffuse
   // normale . direction de la lumière
   float NdotL = dot( N, L );

   float intensiteSpeculaire = 0;
   float attenuationSpot = 0;

	if(utiliseDirect)
	{
		float exposant = ((gl_LightSource[0].spotExponent)/2)+1;

		attenuationSpot = (dot(-L,D) - pow(gl_LightSource[0].spotCosCutoff, exposant)) /(gl_LightSource[0].spotCosCutoff-pow(gl_LightSource[0].spotCosCutoff, exposant));
	    attenuationSpot = max(attenuationSpot,0);
	}
	else
	{
	  	attenuationSpot = max(dot(-L,D),0);
	  	if(attenuationSpot > gl_LightSource[0].spotCosCutoff)
	  	{
	  		attenuationSpot = pow (attenuationSpot, gl_LightSource[0].spotExponent);
	  	}
		else
		{
			attenuationSpot = 0;
		}
	}


	// calcul de l'éclairage seulement si le produit scalaire est positif
	if ( NdotL > 0.0)
	{
		// calcul de la composante diffuse
		couleur += (gl_LightSource[0].diffuse * gl_FrontMaterial.diffuse)*attenuationSpot;

		if(utiliseBlinn)
		{
			// Parametre pour Blinn cos(phi)
			float BdotN = dot( normalize(lightDir+ eyeVec),N);

			intensiteSpeculaire = max( BdotN, 0.0);
		}
		else
		{
			// calcul de la composante spéculaire
			vec3 E = normalize( eyeVec );

			// Phong
			vec3 R = -reflect( L, N ); // réflexion de L par rapport à N
			// produit scalaire pour la réflexion spéculaire (N dot HV)
			intensiteSpeculaire = max( dot( R, E ), 0.0 );
		}
		couleur += (gl_LightSource[0].specular * gl_FrontMaterial.specular * pow( intensiteSpeculaire, gl_FrontMaterial.shininess ))*attenuationSpot;
	}

	if(texnumero > 0)
	{
		vec4 textureColor = texture2D( laTexture, gl_TexCoord[0].st);



		if(textureColor[0] == 0.0 && textureColor[1] == 0.0 && textureColor[2] == 0.0)
		{
			if(noirTransparent)
			{
				discard;
			}

			// fragment == noir
			gl_FragColor = textureColor;
		}
		else if(texnumero == 1)
		{
			gl_FragColor = clamp( couleur, 0.0, 1.0 );
		}
		else if(texnumero == 2)
		{
			gl_FragColor += textureColor;
		}
	}
	else
	{
		gl_FragColor = clamp( couleur, 0.0, 1.0 );
	}
}
