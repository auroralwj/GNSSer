
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing,Exersise4.2_Gauss-Jackson4th-orderPredictor
//2017.06.26, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    // 
    // Purpose: 
    //
    //   Satellite Orbits - Models, Methods, and Applications
    //   Exersise4.2_Gauss-Jackson4th-orderPredictor
    class Exersise4_2_GaussJackson4thOrderPredictor
    { 
        /// <summary>
        ///  Computes the second time derivative of the position vector for the 
        ///   normalized (GM=1) Kepler's problem in three dimensions.
        ///   pAux is expected to point to an integer variable that will be incremented
        ///   by one on each call of f_Kep3D  
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="r"></param>
        /// <param name="v"></param>
        /// <param name="a"></param>
        /// <param name="pAux"></param>
        static void f_Kep3D(double t, Geo.Algorithm.Vector r, Geo.Algorithm.Vector v, ref  Geo.Algorithm.Vector a, Action pAux)
        {
            // Pointer to auxiliary integer variable used as function call counter
            //int* pCalls = static_cast<int*>(pAux);

            // 2nd order derivative d^2(r)/dt^2 of the position vector
            a = new Geo.Algorithm.Vector(  -r / (Math.Pow(r.Norm(), 3)));

            // Increment function call count
            //(*pCalls)++;
            pAux();
        }

        static int nCalls;

        static void nCallsPlus() { nCalls++; }

        static void Main0(string[] args)
        {
            // Constants

            const double GM = 1.0;                   // Gravitational coefficient
            const double e = 0.1;                   // Eccentricity
            const double t_end = 20.0;                  // End time
            Geo.Algorithm.Vector Kep = new Geo.Algorithm.Vector(1.0, e, 0.0, 0.0, 0.0, 0.0);    // (a,e,i,Omega,omega,M)
            Geo.Algorithm.Vector y_ref = Kepler.State(GM, Kep, t_end);   // Reference solution

            int[] Steps = { 100, 300, 600, 1000, 1500, 2000, 3000, 4000 };

            // Variables
            // Function call count
            int iCase;
            double t, h;                                 // Time and step aboutSize 

            GaussJackson4OrderPredictor Orbit = new GaussJackson4OrderPredictor(f_Kep3D, 3, nCallsPlus);            // Object for integrating the
            // 2nd order diff. equation
            // defined by f_Kep3D using the 4th-order GJ predictor

            // Header 

            var info = "Exercise 4-2: Gauss-Jackson 4th-order predictor" + "\r\n";
            info += "  Problem D1 (e=0.1)" + "\r\n";
            info += "  N_fnc   Accuracy   Digits " + "\r\n";
            Console.WriteLine(info);
            // Loop over test cases

            for (iCase = 0; iCase < 8; iCase++)
            {
                // Step aboutSize
                var step = Steps[iCase];
                h = t_end / step;
                // Initial values
                nCalls = 0;
                t = 0.0;
                var y = Orbit.GetState(e, t, h, step);

                info = String.Format("{0, 6:D}{1, 14:E4}{2, 9:F}", nCalls, (y - y_ref).Norm(), -Math.Log10((y - y_ref).Norm()));
                Console.WriteLine(info);
            }
            Console.ReadKey();
        }


    }
}