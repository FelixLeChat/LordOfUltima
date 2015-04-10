#include <iostream>
#include <stdio.h>
#include "chargertex.h"
#include "bitmap.h"

bool ChargerTexture( std::string fichier, GLuint &texture )
{
   // vérifier la présence du fichier BMP en essayant de l'ouvrir
   FILE *img = fopen( fichier.c_str(), "r" );
   if ( !img )
   {
      std::cerr << "Fichier de texture manquant: " << fichier << std::endl;
      return false;
   }
   fclose( img );

   // créer un objet Bitmap et obtenir la taille de l'image et les pixels RGBA
   CBitmap bitmap( fichier.c_str() );
   unsigned int size = bitmap.GetWidth() * bitmap.GetHeight() * 4; // w * h * 4 composantes
   unsigned char *pixels = new unsigned char[size];
   bitmap.GetBits( (void*) pixels, size, 0x000000FF, 0x0000FF00, 0x00FF0000, 0xFF000000 ); // obtenir les composantes RGBA

   // création de la texture OpenGL
   glGenTextures( 1, &texture );
   glBindTexture( GL_TEXTURE_2D, texture );
   glTexImage2D( GL_TEXTURE_2D, 0, 3, bitmap.GetWidth(), bitmap.GetHeight(), 0, GL_RGBA, GL_UNSIGNED_BYTE, pixels );
   glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR );
   glTexParameteri( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR );
   glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT );
   glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT );
   glBindTexture( GL_TEXTURE_2D, 0 );

   // faire le ménage
   delete[] pixels;

   return true;
}
