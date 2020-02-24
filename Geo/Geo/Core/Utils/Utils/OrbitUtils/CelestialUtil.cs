//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Force
//2017.06.26, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Geo.Algorithm;


namespace Geo.Utils
{
    

    /// <summary>
    /// 力
    /// </summary>
    public static  class CelestialUtil
    {
        public const double MJD_J2000 = OrbitConsts.MJD_J2000;// 51544.5;             // Modif. Julian Date of J2000.0
        public const double AU = OrbitConsts.AU;// 149597870000.0;      // Astronomical unit [m]; IAU 1976  

       
        // Local funtions 
        // Fractional part of a number (y=x-[x])
        private static double Frac(double x) { return MathUtil.Fraction(x); }

        /// <summary>
        /// 计算低精度太阳的地心坐标。 Computes the Sun's geocentric position using a low precision analytical series
        /// </summary>
        /// <param name="Mjd_TT">Terrestrial Time (Modified Julian Date)</param>
        /// <returns>Solar position vector [m] with respect to the  
        /// mean equator and equinox of J2000 (EME2000, ICRF)</returns>
        public static Geo.Algorithm.Vector Sun(double Mjd_TT)
        {
            // Constants
            double eps = 23.43929111 * OrbitConsts.RadPerDeg;             // Obliquity of J2000 ecliptic
            double T = (Mjd_TT - MJD_J2000) / 36525.0;  // Julian cent. since J2000

            // Variables
            double L, M, r;
            Geo.Algorithm.Vector r_Sun = new Geo.Algorithm.Vector(3);

            // Mean anomaly, ecliptic longitude and radius
            M = OrbitConsts.TwoPI * Frac(0.9931267 + 99.9973583 * T);                    // [rad]
            L = OrbitConsts.TwoPI * Frac(0.7859444 + M / OrbitConsts.TwoPI +
                              (6892.0 * Math.Sin(M) + 72.0 * Math.Sin(2.0 * M)) / 1296.0e3); // [rad]
            r = 149.619e9 - 2.499e9 * cos(M) - 0.021e9 * cos(2 * M);             // [m]

            // Equatorial position vector
            r_Sun = Matrix.RotateX3D(-eps) * new Geo.Algorithm.Vector(r * cos(L), r * Math.Sin(L), 0.0);

            return r_Sun;
        }


        /// <summary>
        /// 计算低精度月亮的地心坐标。Computes the Moon's geocentric position using a low precision
        ///   analytical series
        /// </summary>
        /// <param name="Mjd_TT">Terrestrial Time (Modified Julian Date)</param>
        /// <returns>Lunar position vector [m] with respect to the 
        ///mean equator and equinox of J2000 (EME2000, ICRF)</returns>
        public static Geo.Algorithm.Vector Moon(double Mjd_TT)
        {
            // Constants
            double eps = 23.43929111 * OrbitConsts.RadPerDeg;             // Obliquity of J2000 ecliptic
            double T = (Mjd_TT - MJD_J2000) / 36525.0;  // Julian cent. since J2000

            // Variables
            double L_0, l, lp, F, D, dL, S, h, N;
            double L, B, R, cosB;
            Geo.Algorithm.Vector r_Moon = new Geo.Algorithm.Vector(3);

            // Mean elements of lunar orbit
            L_0 = Frac(0.606433 + 1336.851344 * T);     // Mean longitude [rev]
            // w.r.t. J2000 equinox
            l = OrbitConsts.TwoPI * Frac(0.374897 + 1325.552410 * T);     // Moon's mean anomaly [rad]
            lp = OrbitConsts.TwoPI * Frac(0.993133 + 99.997361 * T);     // Sun's mean anomaly [rad]
            D = OrbitConsts.TwoPI * Frac(0.827361 + 1236.853086 * T);     // Diff. long. Moon-Sun [rad]
            F = OrbitConsts.TwoPI * Frac(0.259086 + 1342.227825 * T);     // Argument of latitude 

            // Ecliptic longitude (w.r.t. equinox of J2000)
            dL = +22640 * sin(l) - 4586 * sin(l - 2 * D) + 2370 * sin(2 * D) + 769 * sin(2 * l)
                 - 668 * sin(lp) - 412 * sin(2 * F) - 212 * sin(2 * l - 2 * D) - 206 * sin(l + lp - 2 * D)
                 + 192 * sin(l + 2 * D) - 165 * sin(lp - 2 * D) - 125 * sin(D) - 110 * sin(l + lp)
                 + 148 * sin(l - lp) - 55 * sin(2 * F - 2 * D);

            L = OrbitConsts.TwoPI * Frac(L_0 + dL / 1296.0e3);  // [rad]

            // Ecliptic latitude
            S = F + (dL + 412 * sin(2 * F) + 541 * sin(lp)) / OrbitConsts.ArcSecondsPerRad;
            h = F - 2 * D;
            N = -526 * sin(h) + 44 * sin(l + h) - 31 * sin(-l + h) - 23 * sin(lp + h)
                 + 11 * sin(-lp + h) - 25 * sin(-2 * l + F) + 21 * sin(-l + F);

            B = (18520.0 * sin(S) + N) / OrbitConsts.ArcSecondsPerRad;   // [rad]

            cosB = cos(B);

            // Distance [m]
            R = 385000e3 - 20905e3 * cos(l) - 3699e3 * cos(2 * D - l) - 2956e3 * cos(2 * D)
                - 570e3 * cos(2 * l) + 246e3 * cos(2 * l - 2 * D) - 205e3 * cos(lp - 2 * D)
                - 171e3 * cos(l + 2 * D) - 152e3 * cos(l + lp - 2 * D);

            // Equatorial coordinates
            r_Moon = Matrix.RotateX3D(-eps) * new Geo.Algorithm.Vector(R * cos(L) * cosB, R * sin(L) * cosB, R * sin(B));
            return r_Moon;
        }

        private  static double sin(double val) { return Math.Sin(val); }
        private static double cos(double val) { return Math.Cos(val); }
        
        /// <summary>
        ///计算航天器是否在太阳光照中。1 为是， 0 为否。 可用于计算太阳光压。
        /// Computes the fractional illumination of a spacecraft in the 
        ///   vicinity of the Earth assuming a cylindrical shadow model
        /// </summary>
        /// <param name="satXyz_m">Spacecraft position vector [m]</param>
        /// <param name="sunXyz_m">   Sun position vector [m]</param>
        /// <returns>  Illumination factor:
        ///                     nu=0   Spacecraft in Earth shadow 
        ///                     nu=1   Spacecraft fully illuminated by the Sun</returns>
        public static double Illumination(Geo.Algorithm.Vector satXyz_m, Geo.Algorithm.Vector sunXyz_m)
        {
            Geo.Algorithm.Vector e_Sun = sunXyz_m.DirectionUnit();   // Sun direction unit vector
            double s = satXyz_m.Dot(e_Sun);      // Projection of s/c position 
            return ((s > 0 || (satXyz_m - s * e_Sun).Norm() > OrbitConsts.RadiusOfEarth) ? 1.0 : 0.0);
        }
         
    }
}