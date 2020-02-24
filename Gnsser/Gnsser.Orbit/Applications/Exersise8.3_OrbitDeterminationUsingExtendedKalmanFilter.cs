
//2017.06.22, czs, funcKeyToDouble from c++ in hongqing, ExersisExersise8.3_OrbitDeterminationUsingExtendedKalmanFilter
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
    //  EExersise8.3_OrbitDeterminationUsingExtendedKalmanFilter
    public class Exersise8_3_OrbitDeterminationUsingExtendedKalmanFilter
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
            const double sigma_angle = 0.01 * OrbitConsts.RadPerDeg;    // [rad] (0.01 deg = 36")

            string[] Label = { "x  [m]  ", "y  [m]  ", "z  [m]  ",
                             "vx [m/s]", "vy [m/s]", "vz [m/s]"  };

            // Variables

            int i;
            double Mjd0, t, t_old, MjdUTC, Theta;
            double Azim=0, Elev=0, Dist=0;
            Vector Y0_true = new Vector(6), Y_true = new Vector(6), Y = new Vector(6), Y_old = new Vector(6);
            Vector ErrY = new Vector(6), SigY = new Vector(6);
            Vector r = new Vector(3), R = new Vector(3), s = new Vector(3);
            Vector dAds = new Vector(3), dEds = new Vector(3), dDds = new Vector(3);
            Vector dAdY = new Vector(6), dEdY = new Vector(6), dDdY = new Vector(6);
            Matrix U = new Matrix(3, 3), E = new Matrix(3, 3);
            Matrix Phi = new Matrix(6, 6), Phi_true = new Matrix(6, 6), P = new Matrix(6, 6);
            ExtendedKalmanFilter Filter = new ExtendedKalmanFilter(6);

            ObsType[] Obs = new ObsType[N_obs];


            // Ground station

            R = new Vector(+1344.0e3, +6069.0e3, 1429.0e3);    // [m] Bangalore
            E = CoordTransformer.XyzToGeoCoord(new XYZ(R.OneDimArray)).ToLocalNez_Matrix();

            // Header 
            var endl = "\r\n";
            var info = "Exercise 8-3: Sequential orbit determination" + endl + endl;
            Console.Write(info);

            // Generation of artificial observations from given epoch state 

            Mjd0 = DateUtil.DateToMjd(1995, 03, 30, 00, 00, 00.0);       // Epoch (UTC)

            Y0_true = new Vector(-6345.000e3, -3723.000e3, -580.000e3,     // [m]
                             +2.169000e3, -9.266000e3, -1.079000e3);   // [m/s]

            info = "Measurements" + endl + endl
                 + "     Date          UTC       Az[deg]   El[deg]   Range[km]" + endl;
            Console.Write(info);
            for (i = 0; i < N_obs; i++)
            {

                // Time increment and propagation
                t = (i + 1) * Step;                    // Time since epoch [s]
                MjdUTC = Mjd0 + t / 86400.0;              // Modified Julian Date
                Kepler.TwoBody(OrbitConsts.GM_Earth, Y0_true, t, ref Y, ref Phi);  // State vector

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
                info = "  " + DateUtil.MjdToDateTimeString(MjdUTC) + String.Format(" {0, 10:F3}{1, 10:F3}{2, 10:F3}", +OrbitConsts.DegPerRad * Azim, OrbitConsts.DegPerRad * Elev, Dist / 1000.0) + endl;
                Console.Write(info);

            };
             

            //
            // Orbit determination
            //

            info = "State errors" + endl + endl
                 + "                                     Pos[m]             Vel[m/s]    "
                 + endl
                 + "     Date          UTC    Upd.   Error     Sigma     Error     Sigma"
                 + endl;
            Console.Write(info);

            // Initialization

            Mjd0 = DateUtil.DateToMjd(1995, 03, 30, 00, 00, 00.0);        // Epoch (UTC)

            t = 0.0;

            Y = Y0_true + new Vector(+10.0e3, -5.0e3, +1.0e3, -1.0, +3.0, -0.5);

            //P = 0.0;
            for (i = 0; i < 3; i++) P[i, i] = 1.0e8;
            for (i = 3; i < 6; i++) P[i, i] = 1.0e2;

            Filter.Init(t, Y, P);

            // Measurement loop

            for (i = 0; i < N_obs; i++)
            {

                // Previous step
                t_old = Filter.Time();
                Y_old = Filter.State();

                // Propagation to measurement epoch
                MjdUTC = Obs[i].Mjd_UTC;                  // Modified Julian Date
                t = (MjdUTC - Mjd0) * 86400.0;           // Time since epoch [s]


                Kepler.TwoBody(OrbitConsts.GM_Earth, Y_old, t - t_old, ref Y, ref Phi); // State vector
                Theta = IERS.GetGmstRad(MjdUTC);                    // Earth rotation
                U = Matrix.RotateZ3D(Theta);

                // Time update
                Filter.TimeUpdate(t, Y, Phi);

                // Truth orbit
                Kepler.TwoBody(OrbitConsts.GM_Earth, Y0_true, t, ref Y_true, ref Phi_true);

                // State error and standard deviation
                ErrY = Filter.State() - Y_true;
                SigY = Filter.StdDev();
                info = DateUtil.MjdToDateTimeString(MjdUTC) + "  t  "
                    + String.Format("{0, 10:F3}{1, 10:F3}{2, 10:F3}{3, 10:F3}", ErrY.Slice(0, 2).Norm(), SigY.Slice(0, 2).Norm(), ErrY.Slice(3, 5).Norm(), SigY.Slice(3, 5).Norm()) + endl;
                   
                Console.Write(info);
                // Azimuth and partials
                r = Filter.State().Slice(0, 2);
                s = E * (U * r - R);                            // Topocentric position [m]
                GeoCoord.LocalEnzToPolar(s, out Azim, out Elev, out dAds, out dEds);              // Azimuth, Elevation
                dAdY = (dAds * E * U).Stack(Null3D);

                // Measurement update
                Filter.MeasUpdate(Obs[i].Azim, Azim, sigma_angle / Math.Cos(Elev), dAdY);
                ErrY = Filter.State() - Y_true;
                SigY = Filter.StdDev();
                info = "                         Az "
                    + String.Format("{0, 10:F3}{1, 10:F3}{2, 10:F3}{3, 10:F3}", ErrY.Slice(0, 2).Norm(), SigY.Slice(0, 2).Norm(), ErrY.Slice(3, 5).Norm(), SigY.Slice(3, 5).Norm()) + endl;
                Console.Write(info);

                // Elevation and partials
                r = Filter.State().Slice(0, 2);
                s = E * (U * r - R);                            // Topocentric position [m]
                GeoCoord.LocalEnzToPolar(s, out Azim, out Elev, out dAds, out dEds);              // Azimuth, Elevation
                dEdY = (dEds * E * U).Stack(Null3D);

                // Measurement update
                Filter.MeasUpdate(Obs[i].Elev, Elev, sigma_angle, dEdY);
                ErrY = Filter.State() - Y_true;
                SigY = Filter.StdDev();
                info = "                         El "
                    + String.Format("{0, 10:F3}{1, 10:F3}{2, 10:F3}{3, 10:F3}", ErrY.Slice(0, 2).Norm(), SigY.Slice(0, 2).Norm(), ErrY.Slice(3, 5).Norm(), SigY.Slice(3, 5).Norm()) + endl;
                Console.Write(info);

                // Range and partials
                r = Filter.State().Slice(0, 2);
                s = E * (U * r - R);                            // Topocentric position [m]
                Dist = s.Norm(); dDds = s / Dist;                // Range
                dDdY = (dDds * E * U).Stack(Null3D);

                // Measurement update
                Filter.MeasUpdate(Obs[i].Dist, Dist, sigma_range, dDdY);
                ErrY = Filter.State() - Y_true;
                SigY = Filter.StdDev();
                info = "                         rho"
                    + String.Format("{0, 10:F3}{1, 10:F3}{2, 10:F3}{3, 10:F3}", ErrY.Slice(0, 2).Norm(), SigY.Slice(0, 2).Norm(), ErrY.Slice(3, 5).Norm(), SigY.Slice(3, 5).Norm()) + endl;
                Console.Write(info);
            }             

            Console.ReadKey();
        }
    }
}
