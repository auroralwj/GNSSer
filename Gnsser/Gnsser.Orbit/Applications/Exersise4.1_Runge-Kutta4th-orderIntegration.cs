//2017.06.21, czs, funcKeyToDouble from c++ in hongqing,Exersise4.1_Runge-Kutta4th-orderIntegration
//2017.06.27, czs, edit in hongqing, format and refactor codes

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
    //   Exersise4.1_Runge-Kutta4th-orderIntegration
    class Exersise4_1_RungeKutta4thOrderIntegration
    { 
        /// <summary>
        ///  Computes the derivative of the state vector for the normalized (GM=1)
        ///   Kepler's problem in three dimensions   
        ///    Note:
        ///
        ///   pAux is expected to point to an integer variable that will be incremented
        //   by one on each call of Deriv
        /// </summary>
        /// <param name="t"></param>
        /// <param name="y"></param>
        /// <param name="pAux"></param>
        /// <returns></returns>
        static Geo.Algorithm.Vector  f_Kep6D(double t, Geo.Algorithm.Vector y,  Object pAux)
        {
            // Pointer to auxiliary integer variable used as function call counter        

            // State vector derivative
            Geo.Algorithm.Vector r = y.Slice(0, 2);
            Geo.Algorithm.Vector v = y.Slice(3, 5);
            Geo.Algorithm.Vector yp = v.Stack(-r / (Math.Pow(r.Norm(), 3)));

            // Increment function call count
             ((Action)pAux)();

             return yp;
        }

        static void nCallsPlus()
        {
            nCalls++;
        }
       static  int nCalls;             
          

        static void Main0(string[] args)
        {
            // Constants
            const double GM = 1.0;                   // Gravitational coefficient
            const double e = 0.1;                   // Eccentricity
            const double t_end = 20.0;                  // End time
            Geo.Algorithm.Vector Kep = new Geo.Algorithm.Vector(1.0, e, 0.0, 0.0, 0.0, 0.0);    // (a,e,i,Omega,omega,M)
            Geo.Algorithm.Vector y_ref = Kepler.State(GM, Kep, t_end);   // Reference solution

            int[] Steps = { 50, 100, 250, 500, 750, 1000, 1500, 2000 };

            // Variables

            // Function call count
            int iCase;
            double t, h;                                 // Time and step aboutSize
            Geo.Algorithm.Vector y = new Geo.Algorithm.Vector(6);                                // State vector

            RungeKutta4StepIntegrator Orbit = new RungeKutta4StepIntegrator(f_Kep6D, 6,  (Action)nCallsPlus);            // Object for integrating the
            // differential equation
            // defined by f_Kep6D using the
            // 4th-order Runge-Kutta method
            // Header 

            var info = "Exercise 4-1: Runge-Kutta 4th-order integration" + "\r\n" + "\r\n"
              + "  Problem D1 (e=0.1)" + "\r\n" + "\r\n"
              + "  N_fnc   Accuracy   Digits " + "\r\n";
            Console.WriteLine(info);

            // Loop over test cases
            for (iCase = 0; iCase < 8; iCase++)
            {
                // Step aboutSize
                h = t_end / Steps[iCase];

                // Initial values
                t = 0.0;
                y = new Geo.Algorithm.Vector(1.0 - e, 0.0, 0.0, 0.0, Math.Sqrt((1 + e) / (1 - e)), 0.0);
                nCalls = 0;

                // Integration from t=t to t=t_end
                for (int i = 1; i <= Steps[iCase]; i++)
                    Orbit.Step(ref t, ref y, h);

                // Output
                info = String.Format("{0, 6:D}{1, 14:E4}{2, 9:F}", nCalls, (y - y_ref).Norm(), -Math.Log10((y - y_ref).Norm()));
                Console.WriteLine(info);
            };

            Console.ReadKey();
        }
    }
}