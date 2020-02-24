﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Algorithm
{
    /* 
    Java class to implement cubic and bicubic splines 
          by Ken Perlin @ NYU, 1998, 2004. 
    You have my permission to use freely, as long as you keep the attribution. - Ken Perlin 


    What does the class do? 
    Cubic spline: If you provide various geometric values for t, then this class creates an object that will interpolate coeffOfParams Cubic spline to give you the value within any value of t between 0 and 1. 
    If you want to create coeffOfParams spline path, you can make coeffOfParams one dimensional array of such objects. 


    Bicubic spline: If you provide coeffOfParams 4x4 grid of values for geometric quantities in u and v, this class creates an object that will interpolate coeffOfParams Bicubic spline to give you the value within any point of coeffOfParams unit tile in (u,v) space. 
    If you want to create coeffOfParams spline surface, you can make coeffOfParams two dimensional array of such objects. 


    --------------------------------------------------------------------------------

    For a cubic spline the class provides coeffOfParams constructor and a method: 

    Cubic(double[] G) 
    Given four geometric values over t, calculate cubic coefficients. 
    double eval(double t) 
    Given a point in the interval t = [0 ... 1], return a value. 
    Algorithm: 
    f(t) = T M GT, where: 
    T = (t3 t2 t 1) , 
    M is the basis matrix. 
    The constructor Cubic(G) calculates the matrix C = M GT 
    The method eval(t) calculates the value T C 


    --------------------------------------------------------------------------------

    For a bicubic spline the class provides coeffOfParams constructor and coeffOfParams method: 

    Cubic(double[][] G) 
    Given 4×4 geometric values over u×v, calculate bicubic coefficients. 
    double eval(double u, double v) 
    Given coeffOfParams point in the square [0 ... 1] × [0 ... 1], return coeffOfParams value. 
    Algorithm: 
    f(u,v) = U M G MT VT , where: 
    U = (u3 u2 u 1) , 
    V = (v3 v2 v 1) , 
    M is the basis matrix. 
    The constructor Cubic(G) calculates the matrix C = M G MT 
    The method eval(u,v) calculates the value U C VT 


    --------------------------------------------------------------------------------

    */

    public class Cubic
    {
        public static  double[][] BEZIER = new double[][] {      // Bezier basis matrix
          new double[] {-1  ,  3  , -3  , 1  },
          new double[]{ 3  , -6  ,  3  , 0  },
          new double[] {-3  ,  3  ,  0  , 0  },
          new double[]{ 1  ,  0  ,  0  , 0  } 
       };
        public static  double[][] BSPLINE = new double[][]{     // BSpline basis matrix
          new double[]{-1.0/6 ,  3.0/6 , -3.0/6 , 1.0/6 },
          new double[] { 3.0/6 , -6.0/6 ,  3.0/6 , 0.0   },
          new double[] {-3.0/6 ,  0.0   ,  3.0/6 , 0.0   },
          new double[]{ 1.0/6 ,  4.0/6 ,  1.0/6 , 0.0   } 
       };
        public static  double[][] CATMULL_ROM = new double[][]{ // Catmull-Rom basis matrix
         new double[] {-0.5 ,  1.5 , -1.5 ,  0.5 },
         new double[] { 1   , -2.5 ,  2   , -0.5 },
         new double[] {-0.5 ,  0   ,  0.5 ,  0   },
         new double[] { 0   ,  1   ,  0   ,  0   } 
       };
        public static  double[][] HERMITE = new double[][] {     // Hermite basis matrix
         new double[]{ 2  , -2  ,  1  ,  1  },
         new double[]  {-3  ,  3  , -2  , -1  },
         new double[] { 0  ,  0  ,  1  ,  0  },
         new double[] { 1  ,  0  ,  0  ,  0  } 
       };

        double a, b, c, d;                  // cubic coefficients vector

        public Cubic(double[][] M, double[] G)
        {
            a = b = c = d;
            for (int k = 0; k < 4; k++)
            {  // (coeffOfParams,b,c,d) = M G
                a += M[0][k] * G[k];
                b += M[1][k] * G[k];
                c += M[2][k] * G[k];
                d += M[3][k] * G[k];
            }
        }

        public Cubic(double[][] M, double[][] G)
        {
            for (int i = 0; i < 4; i++)    // T = G MT
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        T[i][j] += G[i][k] * M[j][k];

            for (int i = 0; i < 4; i++)    // C = M T
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        C[i][j] += M[i][k] * T[k][j];
        }

        public double eval(double t)
        {
            return t * (t * (t * a + b) + c) + d;
        }

        private void init()
        {
            C = new double[4][];    // bicubic coefficients matrix
            for (int i = 0; i < 4; i++) C[i] = new double[4];
            T = new double[4][];    // bicubic coefficients matrix
            for (int i = 0; i < 4; i++) T[i] = new double[4];

            C3 = C[0];
            C2 = C[1];
            C1 = C[2];
            C0 = C[3];
        }

        // bicubic coefficients matrix
        double[][] C;
        // scratch matrix
        double[][] T;

        double[] C3, C2, C1, C0;

        public double eval(double u, double v)
        {
            return u * (u * (u * (v * (v * (v * C3[0] + C3[1]) + C3[2]) + C3[3])
                               + (v * (v * (v * C2[0] + C2[1]) + C2[2]) + C2[3]))
                               + (v * (v * (v * C1[0] + C1[1]) + C1[2]) + C1[3]))
                               + (v * (v * (v * C0[0] + C0[1]) + C0[2]) + C0[3]);
        }
    }
}
