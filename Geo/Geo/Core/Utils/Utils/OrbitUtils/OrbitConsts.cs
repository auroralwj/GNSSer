//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Exersise3.2_LunarEphemerides
//2017.06.24, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Geo.Algorithm;


namespace Geo.Utils
{
    /// <summary>
    /// 常用常量
    /// </summary>
    public class OrbitConsts
    {

        //
        // Mathematical constants
        //
        public const double PI = 3.1415926535897932384626433832795;
        public const double TwoPI = 2.0 * PI;           // 2pi
        public const double RadPerDeg = PI / 180.0;          // Radians per degree
        public const double DegPerRad = 180.0 / PI;           // Degrees per radian
        /// <summary>
        ///  1.0e-15;  smallest such that 1.0+DBL_EPSILON != 1.0 1.0e-15;
        /// </summary>
        public const double MinDigitalResolution = 1e-15;
        public const double ArcsecondsPerRadian = 3600.0 * 180.0 / PI;     // Arcseconds per radian
             /// <summary>
        /// 1弧度有多少角秒。  Arcseconds per radian
             /// </summary>
        public const double ArcSecondsPerRad = 3600.0 * 180.0 / PI;
    

        // Time const
     

        /// <summary>
        /// 51544.5;   Modif. Julian Date of J2000.0
        /// </summary>
        public const double MJD_J2000 = 51544.5;  
        /// <summary>
        /// 86400.0
        /// </summary>
        public const double SecondsPerDay = 86400.0;
        /// <summary>
        /// TT-TAI time difference [s]
        /// </summary>
        public const double TT_TAI = +32.184; 
        /// <summary>
        /// GPS-TAI time difference [s]
        /// </summary>
        public const double GPS_TAI = -19.0;   

        /// <summary>
        /// 149597870000.0, Astronomical unit [m]; IAU 1976
        /// </summary>
        public const double AU = 149597870000.0; 
        /// <summary>
        /// Speed of light  [m/s]; IAU 1976
        /// </summary>
        public const double SpeedOfLight = 299792458.0;

        //
        // Physical parameters of the Earth, Sun and Moon
        // 
        // Equatorial radius and flattening 
        /// <summary>
        /// semimajor axis of earth，Radius Earth [m]; WGS-84
        /// </summary>
        public const double RadiusOfEarth = 6378.137e3;      // 
        public const double FlatteningOfEarth = 1.0 / 298.257223563; // Flattening; WGS-84
        public const double RadiusOfSun = 696000.0e3;        // Radius Sun [m]; Seidelmann 1992
        public const double RadiusOfMoon = 1738.0e3;        // Radius Moon [m]
        public const double RotationSpeedOfEarth_Rad = 7.2921158553e-5;   // [rad/s]; Aoki 1982, NIMA 1997
        // Earth rotation (derivative of GMST at J2000; differs from inertial period by precession) 

        // Gravitational coefficient
        public const double GM_Earth = 398600.4415e+9;    // [m^3/s^2]; JGM3         
        public const double GM_Sun = 1.32712438e+20;    // [m^3/s^2]; IAU 1976 


        // Solar radiation pressure at 1 AU 
        public const double PressureOfSolarRadiationPerAU = 4.560E-6;          // [N/m^2] (~1367 W/m^2); IERS 96



        #region Constants
        //public const double PI            = 3.141592653589793;
        public const double TwoPi = 2.0 * PI;
        public const double RadsPerDegree = PI / 180.0;
        public const double DegreesPerRad = 180.0 / PI;

        /// <summary>
        /// 恒星日，秒
        /// </summary>
        public const double DaySidereal = (23 * 3600) + (56 * 60) + 4.09;  // sec
        /// <summary>
        /// 真太阳连续两次过同一子午圈的时间间隔。 
        /// </summary>
        public const double DaySolar = (24 * 3600);   // sec
        /// <summary>
        /// ????
        /// </summary>
        public const double Ae = 1.0;
        /// <summary>
        ///  Earth equatorial radius - kilometers (WGS '72)
        /// </summary>
        public const double RadiusOfEquator = 6378.135;     // Earth equatorial radius - kilometers (WGS '72) 
        public const double Ge = 398600.8;     // Earth gravitational constant (WGS '72)
        public const double J2 = 1.0826158E-3; // J2 harmonic (WGS '72)
        public const double J3 = -2.53881E-6;  // J3 harmonic (WGS '72)
        public const double J4 = -1.65597E-6;  // J4 harmonic (WGS '72)
        public const double Ck2 = J2 / 2.0;
        public const double Ck4 = -3.0 * J4 / 8.0;
        public const double Xj3 = J3;
        public const double Qo = OrbitConsts.Ae + 120.0 / OrbitConsts.RadiusOfEquator;
        public const double S = OrbitConsts.Ae + 78.0 / OrbitConsts.RadiusOfEquator;
        public const double MinPerDay = 1440.0;        // Minutes per secondOfWeek (solar)
        public const double SecPerDay = 86400.0;       // Fraction per secondOfWeek (solar)
        public const double EarthRotationPerSiderealDay = 1.00273790934; // Earth rotation per sidereal secondOfWeek
        public static double Xke = Math.Sqrt(3600.0 * Ge /
                                             (OrbitConsts.RadiusOfEquator * OrbitConsts.RadiusOfEquator * OrbitConsts.RadiusOfEquator)); // sqrt(ge) ER^3/min^2
        public static double Qoms2t = Math.Pow((Qo - OrbitConsts.S), 4); //(QO - S)^4 ER^4
    

        #endregion


        public const double GM_Moon = OrbitConsts.GM_Earth / 81.300587;// [m^3/s^2]; DE200
         


    }
}
