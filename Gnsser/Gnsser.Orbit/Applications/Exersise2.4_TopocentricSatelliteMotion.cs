
//2017.06.19, czs, funcKeyToDouble from c++ in hongqing, Exersise2_2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;
using Geo.Algorithm; using Geo.Utils;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    class Exersise2_4_TopocentricSatelliteMotion
    {
        static void Main0(string[] args)
        {
            // Ground station

            const double lon_Sta = 11.0 * OrbitConsts.RadPerDeg;           // [rad]
            const double lat_Sta = 48.0 * OrbitConsts.RadPerDeg;           // [rad]
            const double alt_h = 0.0e3;             // [m]

            GeoCoord StaGeoCoord = new GeoCoord(lon_Sta, lat_Sta, alt_h);   // Geodetic coordinates


            // Spacecraft orbit

            double Mjd_Epoch = DateUtil.DateToMjd(1997, 01, 01, 0, 0, 0);  // Epoch

            const double a = 960.0e3 + OrbitConsts.RadiusOfEarth;    // Semimajor axis [m]
            const double e = 0.0;                // Eccentricity
            const double i = 97.0 * OrbitConsts.RadPerDeg;            // Inclination [rad]
            const double Omega = 130.7 * OrbitConsts.RadPerDeg;            // RA ascend. node [rad]
            const double omega = 0.0 * OrbitConsts.RadPerDeg;            // Argument of latitude [rad]
            const double M0 = 0.0 * OrbitConsts.RadPerDeg;            // Mean anomaly at epoch [rad]

            Vector kepElements = new Geo.Algorithm.Vector(a, e, i, Omega, omega, M0);    // Keplerian elements

            // Variables
            double Mjd_UTC, dt;

            // Station
            var R_Sta = StaGeoCoord.ToXyzVector(OrbitConsts.RadiusOfEarth, OrbitConsts.FlatteningOfEarth);        // Geocentric position vector
            Matrix E = StaGeoCoord.ToLocalNez_Matrix();                     // Transformation to 
            // local tangent coordinates
            // Header

            var info = "Exercise 2-4: Topocentric satellite motion" + "\r\n"
               + "   Date         UTC           Az         El      Dist" + "\r\n"
               + "yyyy/mm/dd  hh:mm:ss.sss     [deg]     [deg]     [km]";
            Console.WriteLine(info);

            // Orbit
            for (int Minute = 6; Minute <= 24; Minute++)
            {
                Mjd_UTC = Mjd_Epoch + Minute / 1440.0;        // Time
                dt = (Mjd_UTC - Mjd_Epoch) * 86400.0;           // Time since epoch [s] 
                Geo.Algorithm.Vector r = Kepler.State(OrbitConsts.GM_Earth, kepElements, dt).Slice(0, 2);      // Inertial position vector
                Matrix U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UTC));                     // Earth rotation 
                var enz = E * (U * r - R_Sta);                    // Topocentric position vector
                double Azim = 0, Elev = 0, Dist;
                GeoCoord.LocalEnzToPolar(enz, out Azim, out Elev, out Dist);                     // Azimuth, Elevation

                info = DateUtil.MjdToDateTimeString(Mjd_UTC) + "  " + String.Format("{0, 9:F3}", (Azim * OrbitConsts.DegPerRad)) + "  " + String.Format("{0, 9:F3}", (Elev * OrbitConsts.DegPerRad))
                     + "  " + String.Format("{0, 9:F3}", (Dist / 1000.0));
                Console.WriteLine(info);
            };

            Console.ReadKey();
        }
    }
}
