
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing, Exersise5.3_GeodeticCoordinates
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
    //   Exersise6.1_LightTimeIteration
    class Exersise6_1_LightTimeIteration
    {
        
        static void Main0(string[] args)
        {


            // Ground station

            const double lon_Sta = 11.0 * OrbitConsts.RadPerDeg;             // [rad]
            const double lat_Sta = 48.0 * OrbitConsts.RadPerDeg;             // [rad]
            const double alt_h = 0.0e3;               // [m]

            GeoCoord Sta = new GeoCoord(lon_Sta, lat_Sta, alt_h);     // Geodetic coordinates


            // Spacecraft orbit

            double Mjd_Epoch = DateUtil.DateToMjd(1997, 01, 01);    // Epoch

            const double a = 960.0e3 + OrbitConsts.RadiusOfEarth;      // Semimajor axis [m]
            const double e = 0.0;                  // Eccentricity
            const double i = 97.0 * OrbitConsts.RadPerDeg;              // Inclination [rad]
            const double Omega = 130.7 * OrbitConsts.RadPerDeg;              // RA ascend. node [rad]
            const double omega = 0.0 * OrbitConsts.RadPerDeg;              // Argument of latitude [rad]
            const double M0 = 0.0 * OrbitConsts.RadPerDeg;              // Mean anomaly at epoch [rad]

            Vector Kep = new Vector(a, e, i, Omega, omega, M0);      // Keplerian elements

            // Light time iteration 
            const int I_max = 2;                      // Maxim. number of iterations
            
            // Variables
            int Iteration, Step;                     // Loop counters
            double Mjd_UTC, t;                          // Time
            double rho, range;                           // Range 1-way/2-way
            double tau_up, tau_down;                     // Upleg/downleg light time
            Vector R_Sta = new Vector(3);                            // Earth-fixed station position
            Vector r_Sta = new Vector(3);                            // Inertial station position
            Vector r = new Vector(3);                                // Inertial satellite position 
            Vector rho_up = new Vector(I_max + 1), rho_down = new Vector(I_max + 1);  // Upleg/downleg range
            Matrix U = new Matrix(3, 3);                             // Earth rotation matrix
            
            // Station
            R_Sta = Sta.ToXyzVector(OrbitConsts.RadiusOfEarth, OrbitConsts.FlatteningOfEarth);         // Geocentric position vector


            // Header
            var endl = "\r\n";
            var info = "Exercise 6-1: Light time iteration" + endl + endl
                 + "   Date         UTC        Distance   " +
                    "Down It 1   It 2   Up It 1    Range" + endl
                 + "yyyy/mm/dd  hh:mm:ss.sss      [m]     " +
                    "    [m]     [mm]      [m]      [m] " + endl;
            Console.WriteLine(info);

            // Orbit 

            for (Step = 0; Step <= 6; Step++)
            {

                // Ground-received time

                t = 360.0 + 180.0 * Step;                      // Time since epoch [s]

                Mjd_UTC = Mjd_Epoch + t / 86400.0;             // Modified Julian Date [UTC]

                U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC));                      // Earth rotation matrix

                r_Sta = U.Transpose() * R_Sta;                     // Inertial station position

                // Light time iteration for downleg satellite -> station

                tau_down = 0.0;
                for (Iteration = 0; Iteration <= I_max; Iteration++)
                {
                    r = Kepler.State(OrbitConsts.GM_Earth, Kep, t - tau_down).Slice(0, 2);  // Spacecraft position
                    rho = (r - r_Sta).Norm();                            // Downleg range
                    tau_down = rho / OrbitConsts.SpeedOfLight;                         // Downleg light time
                    rho_down[Iteration] = rho;
                };

                // Light time iteration for upleg station -> satellite 

                tau_up = 0.0;
                for (Iteration = 0; Iteration <= I_max; Iteration++)
                {
                    U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC - (tau_down + tau_up) / 86400.0));
                    r_Sta = U.Transpose() * R_Sta;                        // Inertial station pos.
                    rho = (r - r_Sta).Norm();                            // at ground transmit time
                    tau_up = rho / OrbitConsts.SpeedOfLight;                           // Upleg light time
                    rho_up[Iteration] = rho;
                };

                // Two-way range 

                range = 0.5 * (rho_down[I_max] + rho_up[I_max]);

                info = DateUtil.MjdToDateTimeString(Mjd_UTC)
                   + String.Format("{0,15:F3}{1,9:F3}{2,9:F3}{3,8:F3}{4,12:F3}", rho_down[0], rho_down[1] - rho_down[0], (rho_down[2] - rho_down[1]) * 1000.0, rho_up[1] - rho_up[0], range);
                Console.WriteLine(info);
            }
          

            Console.ReadKey();
        }
    }
}
