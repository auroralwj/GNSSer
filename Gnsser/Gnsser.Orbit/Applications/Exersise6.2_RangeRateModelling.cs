
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing, Exersise6.2_RangeRateModelling


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
    //   Exersise6.2_RangeRateModelling
    class Exersise6_2_RangeRateModelling
    {
        static void Main0(string[] args)
        {


            // Ground station

            const double lon_Sta = 11.0 * OrbitConsts.RadPerDeg;             // [rad]
            const double lat_Sta = 48.0 * OrbitConsts.RadPerDeg;             // [rad]
            const double alt_h = 0.0e3;               // [m]

            GeoCoord Sta = new GeoCoord(lon_Sta, lat_Sta, alt_h);     // Geodetic coordinates

            // Earth rotation

            Vector omega_vec = new Vector(0.0, 0.0, OrbitConsts.RotationSpeedOfEarth_Rad); // Earth rotation vector

            // Spacecraft orbit

            double Mjd_Epoch = DateUtil.DateToMjd(1997, 01, 01);    // Epoch

            const double a = 960.0e3 + OrbitConsts.RadiusOfEarth;      // Semimajor axis [m]
            const double e = 0.0;                  // Eccentricity
            const double i = 97.0 * OrbitConsts.RadPerDeg;              // Inclination [rad]
            const double Omega = 130.7 * OrbitConsts.RadPerDeg;              // RA ascend. node [rad]
            const double omega = 0.0 * OrbitConsts.RadPerDeg;              // Argument of latitude [rad]
            const double M0 = 0.0 * OrbitConsts.RadPerDeg;              // Mean anomaly at epoch [rad]

            Vector Kep = new Vector(a, e, i, Omega, omega, M0);      // Keplerian elements


            // Radar Modelling

            const int I_max = 3;                      // Maximum light time iterations
            const double Count = 1.0;                    // Doppler count time [s]


            // Variables

            int Iteration, Step;                     // Loop counters
            double Mjd_UTC, t;                          // Time
            double rho;                                 // Range 1-way
            double range1, range0;                       // Range 2-way at end, begin of count
            double range_rate;                          // Range rate
            double Doppler;                             // Instantaneous Doppler
            double tau_up, tau_down;                     // Upleg/downleg light time
            double rho_up = 0, rho_down = 0;                    // Upleg/downleg range
            Vector R_Sta = new Vector(3);                            // Earth-fixed station position
            Vector r_Sta = new Vector(3);                            // Inertial station position
            Vector r = new Vector(3);                                // Inertial satellite position
            Vector x = new Vector(3), v = new Vector(3);                           // Earth-fixed satellite position, velocity 
            Vector u = new Vector(3);                                // Unit vector satellite station
            Matrix U = new Matrix(3, 3);                              // Earth rotation matrix


            // Station

            R_Sta = Sta.ToXyzVector(OrbitConsts.RadiusOfEarth, OrbitConsts.FlatteningOfEarth);         // Geocentric position vector


            // Header
            var endl = "\r\n";
            var info = "Exercise 6-2: Range Rate Modelling" + endl + endl
                 + "   Date         UTC       Range Rate     Doppler  Difference" + endl
                 + "yyyy/mm/dd  hh:mm:ss.sss      [m/s]       [m/s]       [m/s] " + endl;
            Console.WriteLine(info);
            // Orbit 

            for (Step = 0; Step <= 6; Step++)
            {

                // Ground-received time

                t = 360.0 + 180.0 * Step;                      // Time since epoch [s]

                Mjd_UTC = Mjd_Epoch + t / 86400.0;             // Modified Julian Date [UTC]

                U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC));                      // Earth rotation matrix

                r_Sta = U.Transpose() * R_Sta;                     // Inertial station position

                // Light time iteration at count interval end for downleg satellite -> station

                tau_down = 0.0;
                for (Iteration = 0; Iteration <= I_max; Iteration++)
                {
                    r = Kepler.State(OrbitConsts.GM_Earth, Kep, t - tau_down).Slice(0, 2);  // Spacecraft position
                    rho = (r - r_Sta).Norm();                            // Downleg range
                    tau_down = rho / OrbitConsts.SpeedOfLight;                         // Downleg light time
                    rho_down = rho;
                };

                // Light time iteration at count interval end for upleg station -> satellite 

                tau_up = 0.0;
                for (Iteration = 0; Iteration <= I_max; Iteration++)
                {
                    U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC - (tau_down + tau_up) / 86400.0));
                    r_Sta = U.Transpose() * R_Sta;                        // Inertial station pos.
                    rho = (r - r_Sta).Norm();                            // at ground transmit time
                    tau_up = rho / OrbitConsts.SpeedOfLight;                           // Upleg light time
                    rho_up = rho;
                };

                // Two-way range at end of count interval 

                range1 = 0.5 * (rho_down + rho_up);

                // Station position at begin of count interval

                U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC - Count / 86400.0));        // Earth rotation matrix

                r_Sta = U.Transpose() * R_Sta;                     // Inertial station position

                // Light time iteration at count interval begin for downleg satellite -> station

                tau_down = 0.0;
                for (Iteration = 0; Iteration <= I_max; Iteration++)
                {
                    r = Kepler.State(OrbitConsts.GM_Earth, Kep, t - tau_down - Count).Slice(0, 2);  // Spacecraft position
                    rho = (r - r_Sta).Norm();                            // Downleg range
                    tau_down = rho / OrbitConsts.SpeedOfLight;                         // Downleg light time
                    rho_down = rho;
                };

                // Light time iteration at count interval begin for upleg station -> satellite 

                tau_up = 0.0;
                for (Iteration = 0; Iteration <= I_max; Iteration++)
                {
                    U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC - (tau_down + tau_up + Count) / 86400.0));
                    r_Sta = U.Transpose() * R_Sta;                        // Inertial station pos.
                    rho = (r - r_Sta).Norm();                            // at ground transmit time
                    tau_up = rho / OrbitConsts.SpeedOfLight;                           // Upleg light time
                    rho_up = rho;
                };

                // Two-way range at begin of count interval 

                range0 = 0.5 * (rho_down + rho_up);

                // Two-way average range rate

                range_rate = (range1 - range0) / Count;


                // Instantaneous Doppler modelling at mid of count interval 

                U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC - (Count / 2.0) / 86400));            // Earth rotation matrix
                x = U * Kepler.State(OrbitConsts.GM_Earth, Kep, t - (Count / 2.0)).Slice(0, 2);  // Spacecraft position
                v = U * Kepler.State(OrbitConsts.GM_Earth, Kep, t - (Count / 2.0)).Slice(3, 5)   // Spacecraft velocity
                    - omega_vec.Cross3D(x);
                u = (x - R_Sta) / (x - R_Sta).Norm();                         // Unit vector s/c-station 

                Doppler = v.Dot(u);                                  // Instantaneous Doppler


                // Output

                info = DateUtil.MjdToDateTimeString(Mjd_UTC) + String.Format("{0, 12:F3}{1, 12:F3}{2, 12:F3}", range_rate, Doppler, range_rate - Doppler);

                Console.WriteLine(info);


            }
            Console.ReadKey();
        }
    }
}
