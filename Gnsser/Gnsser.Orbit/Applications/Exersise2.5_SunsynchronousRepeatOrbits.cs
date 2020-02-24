
//2017.06.19, czs, funcKeyToDouble from c++ in hongqing, Exersise2_2

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
    //   Exercise 2-5: Sunsynchronous repeat orbits
    class Exersise2_5_SunsynchronousRepeatOrbits
    {
         // Flattening; WGS-84   

        
                      // 2pi
                  // Radians per degree
                  // Degrees per radian
             // Arcseconds per radian
              // Radius Earth [m]; WGS-84
        

        static void Main0(string[] args)
        {             
            const int K = 3;                              // Number of cycles
            const int N = 43;                              // Number of orbits
            const double T_N = (double)K / (double)N;         // Draconic period [d]
            const double J_2 = 1.08263e-3;                      // Oblateness coefficient

            const double Omega_dot = +0.985647240 * OrbitConsts.RadPerDeg;          // Nodal rate [rad/d]

            double omega_dot = 0.0;                             // Arg. of latitude rate
            double Delta_n = 0.0;                             // Perturb. of mean motion

            double n_0 = 0, a = 0, h, i = 0;

            // Header 

            var info = "Exercise 2-5: Sunsynchronous repeat orbits" + "\r\n"
              + "  Draconic period          T_N = "
              + String.Format("{0, 9:F2}", T_N )+ " d" + "\r\n"
              + "  Rate                 2pi/T_N = "
              +  String.Format("{0, 9:F2}",360.0 / T_N )+ " deg/d" + "\r\n"
                  + "  Node offset  Delta lam_Omega = "
                  + String.Format("{0, 9:F2}", -K * 360.0 / N) + " deg/Orbit" + "\r\n";
            // Iteration
            Console.WriteLine(info);
            for (int iterat = 0; iterat <= 2; iterat++)
            {

                // Secular rates of argument of perigee and mean anomaly [rad/d]
                if (iterat == 0)
                {
                    omega_dot = 0.0;
                    Delta_n = 0.0;
                }
                else
                {
                    omega_dot = -0.75 * n_0 * J_2 * Math.Pow(OrbitConsts.RadiusOfEarth / a, 2) * (1.0 - 5 * Math.Pow(Math.Cos(i), 2));
                    Delta_n = -0.75 * n_0 * J_2 * Math.Pow(OrbitConsts.RadiusOfEarth / a, 2) * (1.0 - 3 * Math.Pow(Math.Cos(i), 2));
                };

                // Mean motion, semimajor axis and altitude
                n_0 = OrbitConsts.TwoPI / T_N - Delta_n - omega_dot;                // [rad/d]
                a = Math.Pow(OrbitConsts.GM_Earth / Math.Pow(n_0 / 86400.0, 2), 1.0 / 3.0);  // [m]
                h = a - OrbitConsts.RadiusOfEarth;                                  // [m]

                // Inclination [rad}
                i = Math.Acos(-2.0 * Omega_dot / (3.0 * n_0 * J_2) * Math.Pow(a / OrbitConsts.RadiusOfEarth, 2));

                info = "  Iteration " + iterat + "\r\n"
                + "  Arg. perigee rate  omega_dot = "
                    +  String.Format("{0, 9:F2}", OrbitConsts.DegPerRad * omega_dot) + " deg/d" + "\r\n"
                    + "  Perturb. mean motion   n-n_0 = "
                    + String.Format("{0, 9:F2}", OrbitConsts.DegPerRad * Delta_n )+ " deg/d" + "\r\n"
                    + "  Mean motion              n_0 = "
                    +  String.Format("{0, 9:F2}",OrbitConsts.DegPerRad * n_0 )+ " deg/d" + "\r\n"
                    + "  Semimajor axis             a = "
                    +  String.Format("{0, 9:F2}",a / 1000.0 )+ " km" + "\r\n"
                    + "  Altitude                   h = "
                    + String.Format("{0, 9:F2}", h / 1000.0 )+ " km" + "\r\n"
                    + "  Inclination                i = "
                    + String.Format("{0, 9:F2}", OrbitConsts.DegPerRad * i) + " deg";
                Console.WriteLine(info);
            };

            // Ascending nodes

            info = "  Greenwich longitude of ascending node" + "\r\n"
            + "      Day 1          Day 2          Day 3    " + "\r\n"
            + "  Orbit   [deg]  Orbit   [deg]  Orbit   [deg] ";
            Console.WriteLine(info);


            for (int I = 0; I < 15; I++)
            {
                info = "";
                for (int J = 0; J < 3; J++)
                {
                    var val = (MathUtil.Modulo(-(15 * J + I) * K * 360.0 / N + 180.0, 360.0) - 180.0);
                    info += String.Format("{0, 5:D}",(15 * J + I ))+ " " + String.Format("{0, 9:F2}", val);
                }; 
                Console.WriteLine(info);
            };
            Console.ReadKey();
        }
    }
}
