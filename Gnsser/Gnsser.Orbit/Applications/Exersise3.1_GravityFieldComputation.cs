
//2017.06.19, czs, funcKeyToDouble from c++ in hongqing, Exersise2_6_InitialOrbitDetermination
//2017.06.27, czs, edit in hongqing, format and refactor codes


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;
using Geo.Algorithm; using Geo.Utils;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    // 
    // Purpose: 
    //
    //   Satellite Orbits - Models, Methods, and Applications
    //   Exersise3.1_GravityFieldComputation
    class Exersise3_1_GravityFieldComputation
    {
        static void Main0(string[] args)
        {
            string endl = "\r\n";
            // Constants

            const int N_Step = 10000;  // Recommended for 0.01 sec timer (Linux)
            const int n_max = 20;

            Vector r = new Vector(6525.919e3, 1710.416e3, 2508.886e3);  // Position [m]

            // Variables

            int i, n;                 // Loop counters
            DateTime start, end;           // Processor time at start and end 
            double duration;
            Vector a = new Vector(3);

            // Header 

            var info = "Exercise 3-1: Gravity Field Computation " + "\r\n";
            info += " Order   CPU Time [s]";
            Console.WriteLine(info);// + endl;
            // Outer loop [2,4,...,n_max]

            for (n = 2; n <= n_max; n += 2)
            {
                // Start timing
                start = DateTime.Now;

                // Evaluate gravitational acceleration N_Step times
                for (i = 0; i <= N_Step; i++)
                    a = Force.AccelerOfHarmonicGraviFiled(r, Matrix.CreateIdentity(3), Force.Grav.GM, Force.Grav.R_ref, Force.Grav.CS, n, n);

                // Stop CPU time measurement
                end = DateTime.Now;


                duration = (end - start).TotalSeconds;

                info = String.Format("{0,4:D}", n) + String.Format("{0,8:F}", duration) + "  " ;
                foreach (var item in a.Data)
                {
                    info += String.Format("{0, 12:F6}", item);
                }
                Console.WriteLine(info);// + endl;

            };

            Console.ReadKey();
        }
    }
}