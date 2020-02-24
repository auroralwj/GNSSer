
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing,Exersise4.2_Gauss-Jackson4th-orderPredictor

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{

    // Record for passing global satData between f_Kep6D_ and the calling program 

    struct AuxDataRecord
    {
        public int n_step;
        public double t;
    }

    // 
    // Purpose: 
    //
    //   Satellite Orbits - Models, Methods, and Applications
    //   Exersise4.3_StepSizeControlOfDeMultistepMethod
    class Exersise4_3_StepSizeControlOfDeMultistepMethod
    {

        //------------------------------------------------------------------------------
        //
        // f_Kep6D_
        //
        // Purpose:
        // 
        //   Computes the derivative of the state vector for the normalized (GM=1)
        //   Kepler's problem in three dimensions
        //
        // Note:
        //
        //   pAux is expected to point to a variable of type AuxDataRecord, which is
        //   used to communicate with the other program sections and to hold satData 
        //   between subsequent calls of this function
        //
        //------------------------------------------------------------------------------

        static Geo.Algorithm.Vector   f_Kep6D_(double t, Geo.Algorithm.Vector y, Object pAux)
        {
            // State vector derivative
            Geo.Algorithm.Vector r = y.Slice(0, 2);
            Geo.Algorithm.Vector v = y.Slice(3, 5);
            Geo.Algorithm.Vector yp = v.Stack(-r / (Math.Pow(r.Norm(), 3)));

            // Pointer to auxiliary satData record
            AuxDataRecord p = (AuxDataRecord)pAux;// static_cast<AuxDataRecord*>(pAux);

            // Write current time, step aboutSize and radius; store time for next step
            if (t - p.t > 1.0e-10)
            {
                p.n_step++;
                var info = String.Format("{0, 5:D}{1,12:F6}{2,12:F6}{3,12:F3}", p.n_step, t, t - p.t, r.Norm());
                Console.WriteLine(info);
                p.t = t;
            };
            pAux = p;

            return yp;
        } 

        static void Main0(string[] args)
        { 
            // Constants

            const double GM = 1.0;                   // Gravitational coefficient
            const double e = 0.9;                   // Eccentricity
            const double t_end = 20.0;                  // End time
            Geo.Algorithm.Vector Kep = new Geo.Algorithm.Vector(1.0, e, 0.0, 0.0, 0.0, 0.0);    // (a,e,i,Omega,omega,M)
            Geo.Algorithm.Vector y_ref = Kepler.State(GM, Kep, t_end);   // Reference solution

            // Variables
            
            double t;                            // Time
            double relerr, abserr;                // Accuracy requirements
            Geo.Algorithm.Vector y = new Geo.Algorithm.Vector(6);                         // State vector
            AuxDataRecord Aux = new AuxDataRecord();                          // Auxiliary satData
            DeIntegrator Orbit = new DeIntegrator(f_Kep6D_, 6, Aux);       // Object for integrating the
            // differential equation
            // defined by f_Kep6D_
            // Header 

            var info = "Exercise 4-3: Step size control of DE multistep method" + "\r\n";

            info += "  Step       t           h          r " + "\r\n";
            Console.WriteLine(info);
            // Initial values
            t = 0.0;
            y = new Geo.Algorithm.Vector(1.0 - e, 0.0, 0.0, 0.0, Math.Sqrt((1 + e) / (1 - e)), 0.0);

            Aux.n_step = 0;
            Aux.t = 0;

            relerr = 0.0;
            abserr = 1.0e-8;

            // Integration from t=t to t=t_end
            Orbit.Init(t, relerr, abserr);
            Orbit.Integ(t_end, y); 

            Console.ReadKey();
        }
    }
}