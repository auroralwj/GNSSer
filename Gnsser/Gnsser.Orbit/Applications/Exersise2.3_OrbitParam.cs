
//2017.06.19, czs, funcKeyToDouble from c++ in hongqing, Exersise2_2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    class Exersise2_3_OrbitParam
    {

        // Degrees per radian
        static void Main0(string[] args)
        {
            // Position and velocity 
            Geo.Algorithm.Vector r = new Geo.Algorithm.Vector(+10000.0e3, +40000.0e3, -5000.0e3);  // [m]
            Geo.Algorithm.Vector v = new Geo.Algorithm.Vector(-1.500e3, +1.000e3, -0.100e3);  // [m/s]

            // Variables
            int i;
            Geo.Algorithm.Vector y = new Geo.Algorithm.Vector(6), Kep = new Geo.Algorithm.Vector(6);

            // Orbital elements
            y = r.Stack(v);
            
            Kep = Kepler.Elements(OrbitConsts.GM_Earth, y);

            // Output
            var info = "Exercise 2-3: Osculating elements";
            Console.WriteLine(info);

            info = "State vector:";
            Console.WriteLine(info);
            info = "  Position       ";
            Console.Write(info);
            for (i = 0; i < 3; i++)
            {
                Console.Write(r[i] / 1000.0 + ", ");
                //Console.WriteLine(info);
            };

            info = "  [km]";
            Console.WriteLine(info);

            info = "  Velocity       ";
            Console.Write(info);
            for (i = 0; i < 3; i++)
            {
                Console.Write(v[i] / 1000.0 + ", ");
            }
            Console.WriteLine("  [km/s]");

            Console.WriteLine();
            Console.WriteLine("Orbital elements:");
            Console.WriteLine("  Semimajor axis   " + String.Format("{0:f3}", (Kep[0] / 1000.0)) + " km");
            Console.WriteLine("  Eccentricity     " + String.Format("{0:f3}", Kep[1]));
            Console.WriteLine("  Inclination      " + String.Format("{0:f3}", Kep[2] * OrbitConsts.DegPerRad) + " deg");
            Console.WriteLine("  RA ascend. node  " + String.Format("{0:f3}", Kep[3] * OrbitConsts.DegPerRad) + " deg");
            Console.WriteLine("  Arg. of perigee  " + String.Format("{0:f3}", Kep[4] * OrbitConsts.DegPerRad) + " deg");
            Console.WriteLine("  Mean anomaly     " + String.Format("{0:f3}", Kep[5] * OrbitConsts.DegPerRad) + " deg");

            Console.ReadKey();
        }
    }
}
