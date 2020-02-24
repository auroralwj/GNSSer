
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing, Exersise6.3_UserClockErrorFromGpsPseudorange
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
    //   Exersise6.3_UserClockErrorFromGpsPseudorange
    class Exersise6_3_UserClockErrorFromGpsPseudorange
    {
       

        //------------------------------------------------------------------------------
        //
        // Intpol
        // 
        // Purpose:
        //
        //   Interpolation using Nevilles's algorithm
        //
        // Input/output:
        //
        //   n         Number of points
        //   x         Abscissa values x_i
        //   y         Ordinate values y_i=y(x_i)
        //   x0        Interpolation point
        //   <return>  Interpolated value y(x0)
        //
        // References:
        //
        //   Schwarz H.R.; Numerische Mathematik; B. G. Teubner, Stuttgart (1988).
        // 
        //------------------------------------------------------------------------------
        static double Intpol(int n,   double[] x,   double[] y, double x0)
        {
            int i, k;                    // Loop counters
            double[] p = new double[n];     // Interpolation tableau
            double y0;
            // Neville interpolation
            for (i = 0; i < n; i++) p[i] = y[i];    // Copy of ordinate values
            for (k = 1; k < n; k++)
                for (i = n - 1; i >= k; i--)
                    p[i] += (x0 - x[i]) * (p[i] - p[i - 1]) / (x[i] - x[i - k]);
            y0 = p[n - 1];
            // delete p;         // Clean-up 
            return y0;        // Result
        }

        static void Main0(string[] args)
        {
            // Constants
            const int n_eph = 13;                       // Number of ephemeris points
            const int n_obs = 6;                       // Number of observations
            const int n_it = 3;                       // Light time iterations steps

            const double L1 = 154.0;                     // Frequency multiplier L1
            const double L2 = 120.0;                     // Frequency multiplier L2
            const double f_rel = 1.0 / (1.0 - (L2 * L2) / (L1 * L1)); // Ionosphere multiplier

            // Goldstone station coordinates WGS-84 [m]

            Vector r_Sta = new Vector(-2353614.1280, -4641385.4470, 3676976.5010);

            // Variables

            int i, i_it;                                 // Loop counters
            double tau;                                     // Light time [s]
            double t_snd, t_rcv;                             // Send and receive time [h]
            double dt_User;                                 // User clock offset [s]
            double dt_Sat;                                  // Satellite clock offset [s]
            double mean = 0.0;                              // Mean clock error [m]
            double range;                                   // Observed range [m]
            double rho;                                     // Computed range [m]
            Vector r_GPS;                                   // GPS pos., Earth-fixed [m]
            double[] res = new double[n_obs];                // Pseudorange residuals [m]

            // SP3 PRN1 position coordinates (x,y,z) [m] and clock error [s]
            // 1998/02/19 08:00:00.0 - 11:00:00.0, at 15 m intervals [GPS time]
            double[] t = {  8.00, 8.25, 8.50, 8.75, 9.00, 9.25, 9.50, 9.75, 
                         10.00, 10.25, 10.50, 10.75, 11.00 };

            double[] x = { -15504291.797,-15284290.679,-14871711.829,-14242843.546,
                        -13380818.523,-12276418.004,-10928585.710, -9344633.744, 
                         -7540134.384, -5538503.062, -3370289.205, -1072201.838,
                          1314093.678 };

            double[] y = { -21530763.883,-21684703.684,-21600510.259,-21306712.708,
                        -20837175.663,-20229688.085,-19524421.024,-18762314.034,
                        -17983451.817,-17225491.970,-16522202.377,-15902162.018,
                        -15387672.739 };

            double[] z = {  -1271498.273,  1573435.406,  4391350.089,  7133948.741,  
                          9754366.309, 12207953.668, 14453015.617, 16451492.281, 
                         18169574.686, 19578246.580, 20653745.961, 21377940.941, 
                         21738615.794 };

            double[] clk = {  40.018233e-6, 40.097295e-6, 40.028697e-6, 40.154941e-6, 
                         40.193626e-6, 40.039288e-6, 40.012677e-6, 39.883106e-6, 
                         40.181357e-6, 40.328261e-6, 40.039533e-6, 40.052642e-6, 
                         40.025493e-6  };


            // Goldstone pseudo range observations P2, C1 [m] from PRN1
            // 1998/02/19 08:30:00.0 - 11:00:00.0, at 30 m intervals [GPS time]

            double[] t_obs = { 8.5, 9.0, 9.5, 10.0, 10.5, 11.0 };

            double[] range_P2 = { 21096577.475, 20519964.850, 20282706.954, 
                                20375838.496, 20751678.769, 21340055.129 };

            double[] range_C1 = { 21096579.501, 20519966.875, 20282709.233,
                                20375840.613, 20751680.997, 21340057.362 };


            // Process pseudorange measurements

            for (i = 0; i <= n_obs - 1; i++)
            {
                dt_Sat = Intpol(n_eph,   t,   clk, t_obs[i]);        // Satellite clock offset [s]

                var p2 = range_P2[i];
                var c1 = range_C1[i];
                var ionoCorrected = (p2 - c1) * f_rel;
                var clkOffset = OrbitConsts.SpeedOfLight * dt_Sat;
                range = p2 - ionoCorrected   //  // Pseudrorange corrected for - ionosphere
                         + clkOffset;                    // - satellite clock offset

                // Light time iteration for downleg GPS -> station

                dt_User = 0.0;
                tau = 0.0;
                for (i_it = 1; i_it <= n_it; i_it++)
                {
                    t_rcv = t_obs[i] - dt_User / 3600.0;          // Receive time (GPS)
                    t_snd = t_rcv - tau / 3600.0;                 // Transmit time (GPS) [h]
                    r_GPS = new Vector(Intpol(n_eph,   t,   x, t_snd),   // Position of GPS satellite
                                     Intpol(n_eph,   t,   y, t_snd),   // at t_snd in Earth-fixed
                                     Intpol(n_eph,   t,    z, t_snd)); // system at t_snd and in 
                    r_GPS = Matrix.RotateZ3D(OrbitConsts.RotationSpeedOfEarth_Rad * tau) * r_GPS;       // Earth-fixed frame at t_obs
                    rho = (r_GPS - r_Sta).Norm();                  // Range
                    tau = rho / OrbitConsts.SpeedOfLight;                        // Light time
                    res[i] = range - rho;                      // Residual
                    dt_User = res[i] / OrbitConsts.SpeedOfLight;                   // Approx. user clock error 
                }

                mean += res[i];
            }

            mean = mean / n_obs;                              // Scaling of mean value
            // Output
            var endl = "\r\n";
            var info = "Exercise 6-3: User Clock Error from GPS Pseudorange" + endl
                  + endl
                  + "                               Error    Err-Mean" + endl
                  + "                                [m]        [m]  ";
            Console.WriteLine(info);
            for (i = 0; i <= n_obs - 1; i++)
            {
                info = " " + DateUtil.MjdToDateTimeString(DateUtil.DateToMjd(1998, 2, 19) + t_obs[i] / 24.0)
                      + String.Format("{0, 12:F3}{1, 12:F3}", res[i], res[i] - mean);
                Console.WriteLine(info);
            }
            info = endl + " Mean value " + String.Format("{0, 25:F}", mean) + endl;
            Console.WriteLine(info);
            Console.ReadKey();
        }
    }
}
