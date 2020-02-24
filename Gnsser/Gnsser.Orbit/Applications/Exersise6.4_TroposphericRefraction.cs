
//2017.06.22, czs, funcKeyToDouble from c++ in hongqing, Exersise6.4_TroposphericRefraction
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
    //  Exersise6.4_TroposphericRefraction
    class Exersise6_4_TroposphericRefraction
    {
        static void Main0(string[] args)
        {
            // Ground station

            const double lon_Sta = 11.0 * OrbitConsts.RadPerDeg;             // [rad]
            const double lat_Sta = 48.0 * OrbitConsts.RadPerDeg;             // [rad]
            const double alt_h = 0.0e3;               // [m]

            GeoCoord Sta = new GeoCoord(lon_Sta, lat_Sta, alt_h);     // Geodetic coordinates


            // Fixed media satData at ground site

            const double T0 = 273.2;                    // Temperature at 0 deg C [K]
            const double pa = 1024.0;                    // Partial pressure of dry air [mb]
            const double fh = 0.7;                    // Relative humidity 

            // Spacecraft orbit

            double Mjd_Epoch = DateUtil.DateToMjd(1997, 01, 01);    // Epoch

            const double a = 42164.0e3;              // Semimajor axis [m]
            const double e = 0.000296;             // Eccentricity
            const double i = 0.05 * OrbitConsts.RadPerDeg;              // Inclination [rad]
            const double Omega = 150.7 * OrbitConsts.RadPerDeg;              // RA ascend. node [rad]
            const double omega = 0.0 * OrbitConsts.RadPerDeg;              // Argument of latitude [rad]
            const double M0 = 0.0 * OrbitConsts.RadPerDeg;              // Mean anomaly at epoch [rad]

            Vector Kep = new Vector(a, e, i, Omega, omega, M0);      // Keplerian elements


            // Variables

            int Hour;
            double Mjd_UTC, dt;
            double Azim = 0, Elev = 0, Elev0 = 0, Dist;
            double Ns, eh, T, TC;
            Vector dElev = new Vector(2);
            Vector R_Sta = new Vector(3);
            Vector r = new Vector(3), s = new Vector(3);
            Matrix U = new Matrix(3, 3), E = new Matrix(3, 3);

            double[] Tv = { 303.0, 283.0 };          // Temperature [K] 

            // Station

            R_Sta = Sta.ToXyzVector(OrbitConsts.RadiusOfEarth, OrbitConsts.FlatteningOfEarth);         // Geocentric position vector
            E = Sta.ToLocalNez_Matrix();                      // Transformation to 
            // local tangent coordinates


            // Header
            var endl = "\r\n";
            var info = "Exercise 6-4: Tropospheric Refraction" + endl + endl;
            Console.Write(info);

            // Orbit 

            for (Hour = 0; Hour <= 8; Hour++)
            {

                Mjd_UTC = Mjd_Epoch + 3.0 * Hour / 24.0;         // Modified Julian Date [UTC]

                dt = (Mjd_UTC - Mjd_Epoch) * 86400.0;            // Time since epoch [s] 

                r = Kepler.State(OrbitConsts.GM_Earth, Kep, dt).Slice(0, 2);       // Inertial position vector

                U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC));                      // Earth rotation 
                s = E * (U * r - R_Sta);                     // Topocentric position vector
                 
                GeoCoord.LocalEnzToPolar(s, out Azim, out Elev, out Dist);                      // Azimuth, Elevation

                if (Hour == 0)
                {
                    Elev0 = Elev;                              // Store initial elevation
                    info = "E0 [deg] " + String.Format("{0, 10:F3}", Elev0 * OrbitConsts.DegPerRad) + endl + endl;
                    Console.Write(info);
                    info = "   Date         UTC          E-E0      dE_t1     dE_t2 " + endl
                    + "yyyy/mm/dd  hh:mm:ss.sss     [deg]     [deg]     [deg]" + endl;
                    Console.Write(info);
                };

                for (int Ti = 0; Ti <= 1; Ti++)
                {                // Evaluate at 2 temperatures
                    T = Tv[Ti];                               // Map to scalar 
                    TC = T - T0;                                 // Temperature [C]
                    eh = 6.10 * fh * Math.Exp(17.15 * TC / (234.7 + TC));     // Partial water pressure
                    Ns = 77.64 * pa / T + 3.734e5 * eh / (T * T);        // Refractivity
                    dElev[Ti] = Ns * 1.0e-6 / Math.Tan(Elev);           // Tropospheric refraction
                };


                info = DateUtil.MjdToDateTimeString(Mjd_UTC)
                   + String.Format("{0, 10:F3}", (Elev - Elev0) * OrbitConsts.DegPerRad)
                   + String.Format("{0, 10:F3}", dElev[0] * OrbitConsts.DegPerRad) + String.Format("{0, 10:F3}", dElev[1] * OrbitConsts.DegPerRad) + endl;
                Console.Write(info);

            }; 
            Console.ReadKey();
        }
    }
}
