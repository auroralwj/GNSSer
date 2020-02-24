
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing,Exersise4.2_Gauss-Jackson4th-orderPredictor

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
    //   Exersise5.1_TransformationFromCelestialToTerrestrialReferenceSystem
    class Exersise5_1_TransformationFromCelestialToTerrestrialReferenceSystem
    { 

        static void Main0(string[] args)
        { 
            // Variables

            double MJD_UTC;    // Modified Julian Date (UTC)
            double MJD_UT1;    // Modified Julian Date (UTC)
            double MJD_TT;     // Modified Julian Date (TT)
            Matrix P = new Matrix(3, 3);     // Precession matrix (ICRS -> mean-of-date)
            Matrix N = new Matrix(3, 3);     // Nutation matrix (mean-of-date -> true-of-date)
            Matrix Theta = new Matrix(3, 3); // Sidereal Time matrix (tod -> pseudo-Earth-fixed)
            Matrix Pi = new Matrix(3, 3);    // Polar motion matrix (pseudo-Earth-fixed -> ITRS)


            // Header 

            var info = "Exercise 5-1: Transformation from celestial "
                 + "to terrestrial reference system" + "\r\n";
            Console.WriteLine(info);


            // Earth Orientation Parameters (UT1-UTC[s],UTC-TAI[s], x["], y["])
            // (from IERS Bulletin B #135 and C #16; valid for 1999/03/04 0:00 UTC)

            IERS IERS = new IERS(0.6492332, -32.0, 0.06740, 0.24173);


            // Date

            MJD_UTC = DateUtil.DateToMjd(1999, 03, 04, 0, 0, 0.0);
            MJD_UT1 = MJD_UTC + IERS.GetUT1_UTC(MJD_UTC) / 86400.0;
            MJD_TT = MJD_UTC + IERS.GetTT_UTC(MJD_UTC) / 86400.0;

            // IAU 1976 Precession
            // (ICRF to mean equator and equinox of date)

            P = IERS.PrecessionMatrix(OrbitConsts.MJD_J2000, MJD_TT);

            // IAU 1980 Nutation
            // (Transformation to the true equator and equinox)

            N = IERS.NutMatrix(MJD_TT);

            // Apparent Sidereal Time
            // Rotation about the Celestial Ephemeris Pole

            Theta = IERS.GreenwichHourAngleMatrix(MJD_UT1);   // Note: here we evaluate the equation of the
            // equinoxes with the MJD_UT1 time argument 
            // (instead of MJD_TT)

            // Polar motion
            // (Transformation from the CEP to the IRP of the ITRS)

            Pi = IERS.PoleMatrix(MJD_UTC);     // Note: the time argument of polar motion series
            // is not rigorously defined, but any differences
            // are negligible

            // Output
            var endl = "\r\n";
            info = "Date" + "\r\n"
                 + " " + DateUtil.MjdToDateTimeString(MJD_UTC) + " UTC" + endl
                 + " " + DateUtil.MjdToDateTimeString(MJD_UT1) + " UT1" + endl
                 + " " + DateUtil.MjdToDateTimeString(MJD_TT) + " TT " + endl + endl + endl;
            Console.WriteLine(info);
            info = "IAU 1976 Precession matrix (ICRS to tod)" + endl
                 +   P.ToString();
            Console.WriteLine(info);
            info = "IAU 1980 Nutation matrix (tod to mod)" + endl
                 +  N.ToString();

            Console.WriteLine(info);
            info = "Earth Rotation matrix" + endl
                 + Theta.ToString();

            Console.WriteLine(info);
            info = "Polar motion matrix" + endl
                 + Pi.ToString();
            Console.WriteLine(info);
            info = "ICRS-ITRS transformation" + endl
                 + (Pi * Theta * N * P).ToString();
            Console.WriteLine(info);

            Console.ReadKey();
        }
    }
}