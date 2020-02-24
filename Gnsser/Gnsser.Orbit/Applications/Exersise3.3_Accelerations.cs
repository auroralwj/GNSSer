
//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Exersise3.2_LunarEphemerides
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
    //   Exersise3.2_LunarEphemerides
    class Exersise3_3_Accelerations
    {
        /// <summary>
        ///  A_Diff: Function computes the difference of acceleration due to SRP and DRG
        /// </summary>
        /// <param name="h"> h         Height [m]</param>
        /// <returns>Difference of acceleration</returns>
        public static double A_Diff(double h)
        {
            double CD = 2.3;      // Spacecraft parameters
            double CR = 1.3;
            double Mjd_TT = 51269.0;      // State epoch
             
            Vector r = new Vector(1.0, 0.0, 0.0) * (Force.Grav.R_ref + h);
            double dens = TerrestrialUtil.AtmosDensity_HP(Mjd_TT, r);

            return (0.5 * CD * dens * Force.Grav.GM / (Force.Grav.R_ref + h)) - (OrbitConsts.PressureOfSolarRadiationPerAU * CR);
        }

 
        /// <summary>
        /// 功能不明。2017.06.27 ？？？ czs
        /// 
        ///  飞马座 Pegasus: Root finder using the Pegasus method
        /// 
        ///  Output:
        ///
        ///   Root          Root found (valid only if Success is true)
        ///   Success       Flag indicating success of the routine
        ///
        /// References:                                                               
        ///
        ///   Dowell M., Jarratt P., 'A modified Regula Falsi Method for Computing    
        ///     the root of an equation', BIT 11, p.168-174 (1971).                   
        ///   Dowell M., Jarratt P., 'The "PEGASUS Method for Computing the root      
        ///     of an equation', BIT 12, p.503-508 (1972).                            
        ///   Engeln-Muellges G., Reutter F., 'Formelsammlung zur Numerischen           
        ///     Mathematik mit FORTRAN77-Programmen', Bibliogr. Institut,             
        ///     Zuerich (1986).                                                       
        ///
        /// Notes:
        ///
        ///   Pegasus assumes that the root to be found is bracketed in the interval
        ///   [LowerBound, UpperBound]. Ordinates for these abscissae must therefore
        ///   have different signs.
        /// </summary>
        /// <param name="f"> PegasusFunct  Pointer to the function to be examined</param>
        /// <param name="LowerBound">LowerBound    Lower bound of search interval</param>
        /// <param name="UpperBound">UpperBound    Upper bound of search interval</param>
        /// <param name="Accuracy">Accuracy      Desired accuracy for the root</param>
        /// <param name="Root"></param>
        /// <param name="Success"></param>
        public static void Pegasus(Func<double, double> f,
                       double LowerBound, double UpperBound, double Accuracy,
                       ref double Root, ref bool Success)
        {
            //
            // Constants
            //
            const int MaxIterat = 30;

            //
            // Variables
            //
            double x1 = LowerBound; double f1 = f(x1);
            double x2 = UpperBound; double f2 = f(x2);
            double x3 = 0.0; double f3 = 0.0;

            int Iterat = 0;

            // Initialization
            Success = false;
            Root = x1;

            // Iteration
            if (f1 * f2 < 0.0)
                do
                {
                    // Approximation of the root by interpolation
                    x3 = x2 - f2 / ((f2 - f1) / (x2 - x1)); f3 = f(x3);

                    // Replace (x1,f2) and (x2,f2) by new values, such that
                    // the root is again within the interval [x1,x2]
                    if (f3 * f2 <= 0.0)
                    {
                        // Root in [x2,x3]
                        x1 = x2; f1 = f2; // Replace (x1,f1) by (x2,f2)
                        x2 = x3; f2 = f3; // Replace (x2,f2) by (x3,f3)
                    }
                    else
                    {
                        // Root in [x1,x3]
                        f1 = f1 * f2 / (f2 + f3); // Replace (x1,f1) by (x1,f1')
                        x2 = x3; f2 = f3;     // Replace (x2,f2) by (x3,f3)
                    }

                    if (Math.Abs(f1) < Math.Abs(f2))
                        Root = x1;
                    else
                        Root = x2;

                    Success = (Math.Abs(x2 - x1) <= Accuracy);
                    Iterat++;
                }
                while (!Success && (Iterat < MaxIterat));
        }


        static void Main0(string[] args)
        {
            const double r_Moon = 384400.0e+03; // Geocentric Moon distance [m]
            const double J20_norm = 4.841e-04; // Normalized geopotential coefficients
            const double J22_norm = 2.812e-06;

            const double h1 = 150.0e+3;       // Start of interval [m]
            const double h2 = 2000.0e+3;       // Stop of interval [m]
            const double eps = 100.0;          // Accuracy [m]  

            bool Success = false;
            double r5, r, h = 0;

            var info = "Exercise 3-3: Accelerations ";
            Console.WriteLine(info);
            // Balance of accelerations due to drag and solar radiation pressure

            Pegasus(A_Diff, h1, h2, eps, ref h, ref Success);

            info = " a_DRG > a_SRP  for " + Format(h / 1000.0) + " km";
            Console.WriteLine(info);
            // Balance of accelerations due to J22 and Moon

            r5 = 3.0 / 2.0 * Force.Grav.GM / OrbitConsts.GM_Moon * pow(Force.Grav.R_ref, 2) * pow(r_Moon, 3) * J22_norm;
            r = pow(r5, 0.2);

            info = " a_J22 > a_Moon for " + Format((r - Force.Grav.R_ref) / 1000.0) + " km";
            Console.WriteLine(info);
            // Balance of accelerations due to J22 and Sun

            r5 = 3.0 / 2.0 * Force.Grav.GM / OrbitConsts.GM_Sun * pow(Force.Grav.R_ref, 2) * pow(OrbitConsts.AU, 3) * J22_norm;
            r = pow(r5, 0.2);

            info = " a_J22 > a_Sun  for " + Format((r - Force.Grav.R_ref) / 1000.0) + " km";
            Console.WriteLine(info);
            // Balance of accelerations due to J20 and Moon

            r5 = 3.0 / 2.0 * Force.Grav.GM / OrbitConsts.GM_Moon * pow(Force.Grav.R_ref, 2) * pow(r_Moon, 3) * J20_norm;
            r = pow(r5, 0.2);

            info = " a_J20 > a_Moon for " + Format((r - Force.Grav.R_ref) / 1000.0) + " km";
            Console.WriteLine(info);
            // Balance of accelerations due to J20 and Sun

            r5 = 3.0 / 2.0 * Force.Grav.GM / OrbitConsts.GM_Sun * pow(Force.Grav.R_ref, 2) * pow(OrbitConsts.AU, 3) * J20_norm;
            r = pow(r5, 0.2);

            info = " a_J20 > a_Sun  for " + Format((r - Force.Grav.R_ref) / 1000.0) + " km";
            Console.WriteLine(info);
            Console.ReadKey();
        }

        static string Format(double val)
        {
            return String.Format("{0, 12:F6}", val);
        }

        static double pow(double val, double p)
        {
            return Math.Pow(val, p);
        }
    }

}