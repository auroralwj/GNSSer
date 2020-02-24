
//2017.06.22, czs, funcKeyToDouble from c++ in hongqing, Exersise8.1_Least-squaresFitUsingGivensRotations
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    /// <summary>
    /// Exersise8_1_LeastSquaresFitUsingGivensRotations
    /// </summary>
    class Exersise8_1_LeastSquaresFitUsingGivensRotations
    {
        static void Main0(string[] args)
        {
            // Constants
            double[] t = { 0.04, 0.32, 0.51, 0.73, 1.03, 1.42, 1.60 };
            double[] z = { 2.63, 1.18, 1.16, 1.54, 2.65, 5.41, 7.67 };

            // Variables
            int i;
            double b;
            Geo.Algorithm.Vector a = new Geo.Algorithm.Vector(3), c = new Geo.Algorithm.Vector(3), d = new Geo.Algorithm.Vector(3);
            Matrix R = new Matrix(3, 3);
            LsqEstimater PolyFit = new LsqEstimater(3);


            // Header 
            var endl = "\r\n";
            var info = "Exercise 8-1: Least-squares fit using Givens rotation"
                 + endl + endl;

            Console.Write(info);

            // Accumulation of satData equations
            for (i = 0; i < 7; i++)
            {
                // Data equation
                a[0] = 1.0;
                a[1] = t[i];
                a[2] = t[i] * t[i];
                b = z[i];

                info = "Observation " + i + endl + endl
                     + String.Format("  a = {0, 8:F4}", a) + "    "
                     + String.Format("  b = {0, 8:F4}", b) + endl + endl;
                Console.Write(info);
                // Process satData equation

                PolyFit.Accumulate(a, b);

                // Square-root information matrix and transformed satData 

                R = PolyFit.SRIM();
                d = PolyFit.Data();

                info = "      " + R.Row(0) + "    "
                     + "      " + String.Format("{0, 8:F3}", d[0]) + endl;
                Console.Write(info);
                info = "  R = " + R.Row(1) + "    "
                     + "  d = " + String.Format("{0, 8:F3}", d[1]) + endl;
                Console.Write(info);
                info = "      " + R.Row(2) + "    "
                     + "      " + String.Format("{0, 8:F3}", d[2]) + endl + endl;
                Console.Write(info);

            }

            // Solution of least squares system
            PolyFit.Solve(c);

            info = endl+ "Adjusted polynomial coefficients" + endl + endl;
            for (i = 0; i < 3; i++)
                info += "  c(" + i + ") = "
                     + String.Format("{0, 8:F3}", c[i]) + endl;

            Console.WriteLine(info);
            Console.ReadKey();
        }
    }
}
