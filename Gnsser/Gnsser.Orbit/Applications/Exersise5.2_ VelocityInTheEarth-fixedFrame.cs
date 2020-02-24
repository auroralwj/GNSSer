
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing,Exersise5.2_ VelocityInTheEarth-fixedFrame

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;
using Geo.Algorithm; using Geo.Utils;
using Geo.Algorithm; using Geo.Utils;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{


    // 
    // Purpose: 
    //
    //   Satellite Orbits - Models, Methods, and Applications
    //   Exersise5.2_ VelocityInTheEarth-fixedFrame
    class Exersise5_2_VelocityInTheEarthFixedFrame
    {
        static void Main0(string[] args)
        {

            // Variables

            int i;                  // Loop counter
            double MJD_GPS, MJD_TT;     // Modified Julian Date (GPS,TT)
            double MJD_UTC, MJD_UT1;    // Modified Julian Date (UTC,UT1)
            Matrix P = new Matrix(3, 3), N = new Matrix(3, 3);      // Precession/nutation matrix 
            Matrix Theta = new Matrix(3, 3);         // Sidereal Time matrix 
            Matrix S = new Matrix(3, 3), dTheta = new Matrix(3, 3); // and derivative
            Matrix Pi = new Matrix(3, 3);            // Polar motion matrix 
            Matrix U = new Matrix(3, 3), dU = new Matrix(3, 3);     // ICRS to ITRS transformation and derivative
            Vector r_WGS = new Vector(3), v_WGS = new Vector(3);  // Position/velocity in the Earth-fixed frame
            Vector r = new Vector(3), v = new Vector(3);          // Position/velocity in the ICRS 
            Vector y = new Vector(6), Kep = new Vector(6);        // Satte vector and Keplerian elements


            // Header 
            var endl = "\r\n";
            var info = "Exercise 5-2: Velocity in the Earth-fixed frame"
                   + endl + endl;
            Console.WriteLine(info);

            // Earth Orientation Parameters (UT1-UTC[s],UTC-TAI[s], x["], y["])
            // (from IERS Bulletin B #135 and C #16; valid for 1999/03/04 0:00 UTC)

            IERS IERS = new IERS(0.6492332, -32.0, 0.06740, 0.24173);

            // Date

            MJD_GPS = DateUtil.DateToMjd(1999, 03, 04, 0, 0, 0.0);

            MJD_UTC = MJD_GPS - IERS.GetGPS_UTC(MJD_GPS) / 86400.0;
            MJD_UT1 = MJD_UTC + IERS.GetUT1_UTC(MJD_UTC) / 86400.0;
            MJD_TT = MJD_UTC + IERS.GetTT_UTC(MJD_UTC) / 86400.0;

            // Earth-fixed state vector of GPS satellite #PRN15
            // (from NIMA ephemeris nim09994.eph; WGS84(G873) system)

            r_WGS = new Vector(19440.953805e+3, 16881.609273e+3, -6777.115092e+3); // [m]
            v_WGS = new Vector(-8111.827456e-1, -2573.799137e-1, -30689.508125e-1); // [m/s]


            // ICRS to ITRS transformation matrix and derivative

            P = IERS.PrecessionMatrix(OrbitConsts.MJD_J2000, MJD_TT);    // IAU 1976 Precession
            N = IERS.NutMatrix(MJD_TT);               // IAU 1980 Nutation
            Theta = IERS.GreenwichHourAngleMatrix(MJD_UT1);              // Earth rotation
            Pi = IERS.PoleMatrix(MJD_UTC);             // Polar motion

            S[0, 1] = 1.0; S[1, 0] = -1.0;              // Derivative of Earth rotation 
            dTheta = OrbitConsts.RotationSpeedOfEarth_Rad * S * Theta;             // matrix [1/s]

            U = Pi * Theta * N * P;                    // ICRS to ITRS transformation
            dU = Pi * dTheta * N * P;                   // Derivative [1/s]

            // Transformation from WGS to ICRS

            r = U.Transpose() * r_WGS;
            v = U.Transpose() * v_WGS + dU.Transpose() * r_WGS;

            // Orbital elements

            y = r.Stack(v);
            Kep = Kepler.Elements(OrbitConsts.GM_Earth, y);


            // Output

            info = "Date" + endl + endl
                + " " + DateUtil.MjdToDateTimeString(MJD_GPS) + " GPS" + endl
                + " " + DateUtil.MjdToDateTimeString(MJD_UTC) + " UTC" + endl
                + " " + DateUtil.MjdToDateTimeString(MJD_UT1) + " UT1" + endl
                + " " + DateUtil.MjdToDateTimeString(MJD_TT) + " TT " + endl + endl;

            Console.WriteLine(info);
            info = "WGS84 (G873) State vector:" + endl + endl;
            info += "  Position       ";
            for (i = 0; i < 3; i++) { info += String.Format("{0, 10:F6}", r_WGS[i] / 1000.0); };
            info += "  [km]";
            Console.WriteLine(info);
            info = "  Velocity       ";
            for (i = 0; i < 3; i++) { info += String.Format("{0, 10:F6}", v_WGS[i] / 1000.0); };
            info += "  [km/s]" + endl + endl;
            Console.WriteLine(info);
            info = "ICRS-ITRS transformation" + endl + endl
                + String.Format("{0, 10:F6}", U) + endl;

            info += "Derivative of ICRS-ITRS transformation [10^(-4)/s]" + endl + endl
                + String.Format("{0, 10:F6}", dU * 1.0e4) + endl;

            info += "ICRS State vector:" + endl;
            Console.WriteLine(info);
            info = "  Position       ";
            for (i = 0; i < 3; i++) { info += String.Format("{0, 14:F6}", r[i] / 1000.0); };
            info += "  [km]";
            Console.WriteLine(info);
            info = "  Velocity       ";
            for (i = 0; i < 3; i++) { info += String.Format("{0, 14:F6}", v[i] / 1000.0); };
            info += "  [km/s]" + endl + endl;
            Console.WriteLine(info);
            info = "Orbital elements:" + endl + endl
                + "  Semimajor axis   " + String.Format("{0, 10:F3}", Kep[0] / 1000.0) + " km" + endl
                + "  Eccentricity     " + String.Format("{0, 10:F7}", Kep[1]) + endl
                + "  Inclination      " + String.Format("{0, 10:F3}", Kep[2] * OrbitConsts.DegPerRad) + " deg" + endl
                + "  RA ascend. node  " + String.Format("{0, 10:F3}", Kep[3] * OrbitConsts.DegPerRad) + " deg" + endl
                + "  Arg. of perigee  " + String.Format("{0, 10:F3}", Kep[4] * OrbitConsts.DegPerRad) + " deg" + endl
                + "  Mean anomaly     " + String.Format("{0, 10:F3}", Kep[5] * OrbitConsts.DegPerRad) + " deg" + endl;

            Console.WriteLine(info);

            Console.ReadKey();
        }
    }
}