
//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Exersise2_2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    class Exersise2_2
    {
        static void Main0(string[] args)
        {

            // Variables

            double[] MeanAnom = { 4.0, 50.0 };   // [deg]
            int i;
            double M, E, E_ref, e;


            for (int iCase = 0; iCase <= 1; iCase++)
            { 
                // Test Case 
                M = MeanAnom[iCase] * OrbitConsts.RadPerDeg;
                e = 0.72;
                E_ref = Kepler.EccAnom(M, e);

                // Header

                String str = "Exercise 2-2: Solution of Kepler's equation" + "\r\n" + "\r\n";
                str += "  M =" + M.ToString("F11") + "\r\n";
                str += "  e =" + e.ToString("F11") + "\r\n";
                str += "  E =" + E_ref.ToString("F11") + "\r\n"; ;
                Console.WriteLine(str);
                // Newton's iteration 
                string info = "  a) Newton's iteration" + "\r\n" + "\r\n"
                        + "  i         E         Accuracy  sin/cos" + "\r\n";
                Console.WriteLine(info);
                E = M;
                i = 0;
                do
                {
                    i++;
                    E = E - (E - e * Math.Sin(E) - M) / (1.0 - e * Math.Cos(E));
                    info = " " + i + "   " + E.ToString("F11") + "  " + Math.Abs(E - E_ref).ToString("E2") + "  " + 2 * i;
                    Console.WriteLine(info);
                } while (Math.Abs(E - E_ref) > 1.0e-10);

                // Fixed point iteration
                Console.WriteLine();
                info = "  b) Fixed point iteration" + "\r\n" + "  i         E         Accuracy  sin/cos";
                Console.WriteLine(info);
                E = M;
                i = 0;
                do
                {
                    i++;
                    E = M + e * Math.Sin(E);
                    info = " " + i + "   " + E.ToString("F11") + "  " + Math.Abs(E - E_ref).ToString("E2") + "  " + i;
                    Console.WriteLine(info);
                } while (Math.Abs(E - E_ref) > 1.0e-10); 
            };
            Console.ReadKey();
        }
    }
}
