//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Exersise3.2_LunarEphemerides


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    class Exersise2_1
    {
        static void Main0(string[] args)
        {
            // Constants

            const double h_1 = 750.0e3;     // Initial altitude [m]
            const double h_2 = 775.0e3;     // Final   altitude [m]

            const double r_1 = OrbitConsts.RadiusOfEarth + h_1; // Initial radius [m]
            const double r_2 = OrbitConsts.RadiusOfEarth + h_2; // Final   radius [m]

            // Variables

            double v_1, v_2, a_t, v_p, v_a;

            // Circular velocities

            v_1 = Math.Sqrt(OrbitConsts.GM_Earth / r_1);  // [m/s]
            v_2 = Math.Sqrt(OrbitConsts.GM_Earth / r_2);  // [m/s]

            // Transfer orbit 

            a_t = 0.5 * (r_1 + r_2);                // [m]
            v_p = Math.Sqrt(OrbitConsts.GM_Earth * r_2 / (a_t * r_1)); // [m/s]
            v_a = Math.Sqrt(OrbitConsts.GM_Earth * r_1 / (a_t * r_2)); // [m/s]

            // Header
            Console.WriteLine("Exercise 2-1: Orbit raising using Hohmann transfer");

            // Results

            Console.WriteLine(" Initial altitude     h_1    " + h_1 / 1000 + " km" + "\r\n"
                + " Final   altitude     h_2    " + h_2 / 1000 + " km" + "\r\n"
                + " Circular velocity    v_1    " + v_1 + " m/s" + "\r\n"
                + " Circular velocity    v_2    " + v_2 + " m/s" + "\r\n"
                + " Difference                  " + (v_1 - v_2) + " m/s");
            Console.WriteLine();
            Console.WriteLine(" Transfer orbit sma   a_t    " + a_t / 1000 + " km" + "\r\n"
                + " Pericenter velocity  v_p    " + v_p + " m/s" + "\r\n"
                + " Apocenter  velocity  v_a    " + v_a + " m/s" + "\r\n"
                + " Difference           v_p-v_1 " + (v_p - v_1) + " m/s" + "\r\n"
                + " Difference           v_2-v_a " + (v_2 - v_a) + " m/s" + "\r\n"
                + " Total velocity diff.        " + (v_2 - v_a + v_p - v_1) + " m/s")
                 ;
            //return 0;
            Console.ReadKey();
        }
    }
}
