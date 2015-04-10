#ifndef FLOAT3_H
#define FLOAT3_H
#define _USE_MATH_DEFINES
#include <math.h>

class float3
{
public:
   float val[3];

   float3(float v0=0, float v1=0, float v2=0)
   {
      val[0] = v0;
      val[1] = v1;
      val[2] = v2;
   }

   float3 operator+(float3 v)
   {
      float3 r;
      for ( int i=0 ; i<3 ; i++ )
      {
         r[i] = val[i] + v[i];
      }
      return r;
   }

   float3 operator-(float3 v)
   {
      float3 r;
      for ( int i=0 ; i<3 ; i++ )
      {
         r[i] = val[i] - v[i];
      }
      return r;
   }

   friend float dot(float3 u, float3 v)
   {
      float r=0;
      for ( int i=0 ; i<3 ; i++ )
      {
         r += u[i] * v[i];
      }
      return r;
   }

   float& operator[](int i)
   {
      return val[i];
   }

   float norm()
   {
      return sqrt( val[0]*val[0] + val[1]*val[1] + val[2]*val[2] );
   }

   float3& operator/=(float f)
   {
      for ( int i=0 ; i<3 ; i++ )
      {
         val[i] /= f;
      }
      return *this;
   }

   float3& operator+=(float3 f)
   {
      for ( int i=0 ; i<3 ; i++ )
      {
         val[i] += f[i];
      }
      return *this;
   }

   void normalize()
   {
      (*this) /= norm();
   }
};

#endif
