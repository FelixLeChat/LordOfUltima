#ifndef _matrice_HEADER_
#define _matrice_HEADER_

#include <GL/glut.h>
#include <math.h>
#include <string.h>

class Matrice
{
public:

   static GLdouble* copy( GLdouble* B, const GLdouble* A )
   { return( (GLdouble *) memcpy( B, A, 16*sizeof(GLdouble) ) ); }

   // inverser une matrice OpenGL (normalisée, c'est-à-dire sans glScale):
   static GLdouble* inverse( GLdouble* B, const GLdouble* A )
   {
      GLdouble C[16] = { 1, 0, 0, 0,  0, 1, 0, 0,  0, 0, 1, 0,  0, 0, 0, 1 };
      // i) la diagonale ne change pas
      C[ 0] = A[ 0];  C[ 5] = A[ 5];  C[10] = A[10];
      // ii) la partie 3x3 est simplement transposée
      C[ 1] = A[ 4];  C[ 4] = A[ 1];
      C[ 2] = A[ 8];  C[ 8] = A[ 2];
      C[ 9] = A[ 6];  C[ 6] = A[ 9];
      // iii) la partie 3x1 de la translation est recalculée dans le nouveau repère
      C[12] = -( A[ 0] * A[12] + A[ 1] * A[13] + A[ 2] * A[14] );
      C[13] = -( A[ 4] * A[12] + A[ 5] * A[13] + A[ 6] * A[14] );
      C[14] = -( A[ 8] * A[12] + A[ 9] * A[13] + A[10] * A[14] );
      // iv) et les autres éléments restent à '0'
      return( copy( B, C ) );
   }
};

#endif
