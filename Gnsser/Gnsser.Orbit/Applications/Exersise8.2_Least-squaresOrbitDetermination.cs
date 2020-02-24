//2017.06.22, czs, funcKeyToDouble from c++ in hongqing, Exersise8.2_Least-squaresOrbitDetermination

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
    //  Exersise8.2_Least-squaresOrbitDetermination
    public class Exersise8_2_LeastSquaresOrbitDetermination
    {
        public struct ObsType
        {
            public double Mjd_UTC;
            public double Azim, Elev, Dist;
        };


        static void Main0(string[] args)
        {
            // Ground station
            const int N_obs = 6;
            const double Step = 1200.0;

            Vector Null3D = new Vector(0.0, 0.0, 0.0);

            const double sigma_range = 10.0;        // [m]
            const double sigma_angle = 0.01 * OrbitConsts.RadPerDeg;    // [rad] (=36")

            string[] Label = { "x  [m]  ", "y  [m]  ", "z  [m]  ",
                             "vx [m/s]", "vy [m/s]", "vz [m/s]"  };

            // Variables

            int i, iterat;
            double Mjd0, t, MjdUTC, Theta;
            double Azim=0, Elev=0, Dist=0;
            Vector Y0_ref = new Vector(6), Y0_apr = new Vector(6), Y0 = new Vector(6), Y = new Vector(6), r = new Vector(3), R = new Vector(3), s = new Vector(3);
            Vector dAds = new Vector(3), dEds = new Vector(3), dDds = new Vector(3);
            Vector dAdY0 = new Vector(6), dEdY0 = new Vector(6), dDdY0 = new Vector(6);
            Matrix dYdY0 = new Matrix(6, 6), U = new Matrix(3, 3), E = new Matrix(3, 3);
            LsqEstimater OrbEst = new LsqEstimater(6);
            Vector dY0 = new Vector(6), SigY0 = new Vector(6);


            ObsType[] Obs = new ObsType[N_obs];


            // Ground station

            R = new Vector(+1344.0e3, +6069.0e3, 1429.0e3);    // [m]
            E = CoordTransformer.XyzToGeoCoord(new XYZ(R.OneDimArray)).ToLocalNez_Matrix();

            // Header 
            var endl = "\r\n";
            var info = "Exercise 8-2: Least-squares orbit determination" + endl + endl;
            Console.Write(info);


            // Generation of artificial observations from given epoch state 

            Mjd0 = DateUtil.DateToMjd(1995, 03, 30, 00, 00, 00.0);       // Epoch (UTC)

            Y0_ref = new Vector(-6345.000e3, -3723.000e3, -580.000e3,     // [m]
                            +2.169000e3, -9.266000e3, -1.079000e3);   // [m/s]
            Y0 = Y0_ref;

            info = "Measurements" + endl + endl
                 + "     Date          UTC       Az[deg]   El[deg]   Range[km]" + endl;
            Console.Write(info);

            for (i = 0; i < N_obs; i++)
            {

                // Time increment and propagation
                t = (i + 1) * Step;                    // Time since epoch [s]
                MjdUTC = Mjd0 + t / 86400.0;              // Modified Julian Date
                Kepler.TwoBody(OrbitConsts.GM_Earth, Y0_ref, t, ref Y, ref dYdY0); // State vector

                // Topocentric coordinates
                Theta = IERS.GetGmstRad(MjdUTC);                  // Earth rotation
                U = Matrix.RotateZ3D(Theta);
                r = Y.Slice(0, 2);
                s = E * (U * r - R);                          // Topocentric position [m]
                GeoCoord.LocalEnzToPolar(s, out Azim, out Elev, out Dist);   // Azimuth, Elevation, Range    

                // Observation record
                Obs[i].Mjd_UTC = MjdUTC;
                Obs[i].Azim = Azim;
                Obs[i].Elev = Elev;
                Obs[i].Dist = Dist;

                // Output
                info = "  " + DateUtil.MjdToDateTimeString(MjdUTC) + String.Format("{0, 10:F3}{1, 10:F3}{2, 12:F3}", +OrbitConsts.DegPerRad * Azim, OrbitConsts.DegPerRad * Elev, Dist / 1000.0) + endl;
                Console.Write(info);
            };
            Console.WriteLine();


            //
            // Orbit determination
            //

            Mjd0 = DateUtil.DateToMjd(1995, 03, 30, 00, 00, 00.0);       // Epoch (UTC)

            Y0_apr = Y0_ref + new Vector(+10.0e3, -5.0e3, +1.0e3, -1.0, +3.0, -0.5);
            Y0 = Y0_apr;

            // Iteration

            for (iterat = 1; iterat <= 3; iterat++)
            {

                OrbEst.Init();

                info = "Iteration Nr. " + iterat + endl + endl
                   + "  Residuals:" + endl + endl
                   + "     Date          UTC       Az[deg]   El[deg]  Range[m]" + endl;
                Console.Write(info);
                for (i = 0; i < N_obs; i++)
                {

                    // Time increment and propagation
                    MjdUTC = Obs[i].Mjd_UTC;                // Modified Julian Date
                    t = (MjdUTC - Mjd0) * 86400.0;         // Time since epoch [s]
                    Kepler.TwoBody(OrbitConsts.GM_Earth, Y0, t, ref Y, ref dYdY0);     // State vector

                    // Topocentric coordinates
                    Theta = IERS.GetGmstRad(MjdUTC);                  // Earth rotation
                    U = Matrix.RotateZ3D(Theta);
                    r = Y.Slice(0, 2);
                    s = E * (U * r - R);                          // Topocentric position [m]

                    // Observations and partials
                    GeoCoord.LocalEnzToPolar(s, out Azim, out Elev, out dAds,out dEds);            // Azimuth, Elevation
                    Dist = s.Norm(); dDds = s / Dist;              // Range

                    dAdY0 = (dAds * E * U).Stack(Null3D) * dYdY0;
                    dEdY0 = (dEds * E * U).Stack(Null3D) * dYdY0;
                    dDdY0 = (dDds * E * U).Stack(Null3D) * dYdY0;

                    // Accumulate least-squares system

                    OrbEst.Accumulate(dAdY0, (Obs[i].Azim - Azim), sigma_angle / Math.Cos(Elev));
                    OrbEst.Accumulate(dEdY0, (Obs[i].Elev - Elev), sigma_angle);
                    OrbEst.Accumulate(dDdY0, (Obs[i].Dist - Dist), sigma_range);

                    // Output
                    info = "  " + DateUtil.MjdToDateTimeString(MjdUTC) + String.Format("{0, 10:F3}{1, 10:F3}{2, 10:F3}", OrbitConsts.DegPerRad * (Obs[i].Azim - Azim), OrbitConsts.DegPerRad * (Obs[i].Elev - Elev)
                         , Obs[i].Dist - Dist) + endl;
                    Console.Write(info);
                };

                // Solve least-squares system

                OrbEst.Solve(dY0);
                SigY0 = OrbEst.StdDev();

                info = endl + "  Correction:" + endl + endl
                     + "  Pos" + dY0.Slice(0, 2)
                     + "  m  " + endl
                     + "  Vel" + dY0.Slice(3, 5)
                     + "  m/s" + endl + endl;
                Console.Write(info);
                // Correct epoch state

                Y0 = Y0 + dY0;

            };

            // Summary

            info = "Summary:" + endl
                 + "             a priori   correction      final        sigma"
                 + endl;
            Console.Write(info);
            for (i = 0; i < 6; i++)
            {
                info = "  " + String.Format("{0, 10:S}", Label[i])
                + String.Format("{0, 12:F3}{1, 11:F3}{2, 14:F3}{3, 11:F3}", Y0_apr[i], Y0[i] - Y0_apr[i], Y0[i], SigY0[i])
                     + endl;
                Console.Write(info);
            }
             

            Console.ReadKey();
        }
    }
}
