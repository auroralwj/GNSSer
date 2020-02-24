//2014.05.22, Cui Yang, created
//2014.08.18, czs, edit, 计算结果 UEN -> NEU，分量相对应

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Coordinates;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// 海洋潮汐。
    /// This class computes the effect of ocean tides at a given position and epoch.
    /// This model neglects minor tides and nodal modulations, which may
    /// lead to errors up to 5 mm (RMS) at high latitutes. For more details,
    /// please see: http://tai.bipm.org/iers/convupdt/convupdt_c7.html
    /// </summary>
    public class OceanLoading
    {

        /** Common constructor
           *
           * @param filename  Name of BLQ file containing ocean tide
           *                  harmonics satData.
           *
           * @warning If filename is not given, this class will look for
           * a file named "oceanloading.blq" in the current absDirectory.
           */
        public OceanLoading(string filename = "oceanloading.blq")
        {
            this.BLQDataReader = new BLQDataReader(filename); 
        }

        /// <summary>
        /// object to read BLQ ocean tides harmonics satData file
        /// </summary>
        public BLQDataReader BLQDataReader { get; set; }

        /** Returns the effect of ocean tides loading (meters) at the given
          *  station and epoch, in the Up-East-North (UEN) reference frame.
          *
          * @param name  Station name (case is NOT relevant).
          * @param time  Epoch to look up
          *
          * @return a Triple with the ocean tidas loading effect, in meters
          * and in the UEN reference frame.
          *
          * @throw InvalidRequest If the request can not be completed for any
          * reason, this is thrown. The text may have additional information
          * about the reason the request failed.
          */
        //b本程序统一采用NEU表示。
        public NEU GetOceanLoading(string name, Time t)
        {
            const int NUM_COMPONENTS = 3;
            const int NUM_HARMONICS = 11;

            Geo.Algorithm.ArrayMatrix harmonics = new Geo.Algorithm.ArrayMatrix(6, 11, 0.0);

            //Get harmonics satData from file
            harmonics = BLQDataReader.GetTideHarmonics(name);

            double[] arguments = new double[11];
           // List<double> arguments = new List<double>(11);
            //Compute arguments
            arguments = getArg(t);

       
            double[] triple = new double[NUM_COMPONENTS];
            for (int i = 0; i < NUM_COMPONENTS; i++)
            {
                double temp = 0.0;
                for (int k = 0; k < NUM_HARMONICS; k++)
                {
                    double tttt = harmonics[i, k];
                    double ttt = arguments[k];
                    double tt = harmonics[i + 3, k];
                    temp += harmonics[i, k] * Math.Cos(arguments[k] - harmonics[i + 3, k] * CoordConsts.DegToRadMultiplier);

                }
                triple[i] = temp;
            }
            // This Triple is in Up, West, South reference frame
            double up = triple[0];
            double west = triple[1];
            double sourth = triple[2];

            double north = -sourth;
            double east = -west;

            return new NEU(north, east, up); 
        }

        /** Compute the value of the corresponding astronomical arguments,
         * in radians. This routine is based on IERS routine ARG.f.
         *
         * @param time      Epoch of interest
         *
         * @return A Vector<double> of 11 elements with the corresponding
         * astronomical arguments to be used in ocean loading model.
         */
        private double[] getArg(Time time)
        {
            const int NUM_HARMONICS = 11;

            // Store some important values
            double[] sig = new double[(NUM_HARMONICS)];
            sig[0] = 1.40519e-4;
            sig[1] = 1.45444e-4;
            sig[2] = 1.37880e-4;
            sig[3] = 1.45842e-4;
            sig[4] = 0.72921e-4;
            sig[5] = 0.67598e-4;
            sig[6] = 0.72523e-4;
            sig[7] = 0.64959e-4;
            sig[8] = 0.053234e-4;
            sig[9] = 0.026392e-4;
            sig[10] = 0.003982e-4;

            Geo.Algorithm.ArrayMatrix angfac = new Geo.Algorithm.ArrayMatrix(4, NUM_HARMONICS, 0.0);
            angfac[0, 0] = 2.0; angfac[1, 0] = -2.0; angfac[2, 0] = 0.0; angfac[3, 0] = 0.0;
            angfac[0, 1] = 0.0; angfac[1, 1] = 0.0; angfac[2, 1] = 0.0; angfac[3, 1] = 0.0;
            angfac[0, 2] = 2.0; angfac[1, 2] = -3.0; angfac[2, 2] = 1.0; angfac[3, 2] = 0.0;
            angfac[0, 3] = 2.0; angfac[1, 3] = 0.0; angfac[2, 3] = 0.0; angfac[3, 3] = 0.0;
            angfac[0, 4] = 1.0; angfac[1, 4] = 0.0; angfac[2, 4] = 0.0; angfac[3, 4] = 0.25;
            angfac[0, 5] = 1.0; angfac[1, 5] = -2.0; angfac[2, 5] = 0.0; angfac[3, 5] = -0.25;
            angfac[0, 6] = -1.0; angfac[1, 6] = 0.0; angfac[2, 6] = 0.0; angfac[3, 6] = -0.25;
            angfac[0, 7] = 1.0; angfac[1, 7] = -3.0; angfac[2, 7] = 1.0; angfac[3, 7] = -0.25;
            angfac[0, 8] = 0.0; angfac[1, 8] = 2.0; angfac[2, 8] = 0.0; angfac[3, 8] = 0.0;
            angfac[0, 9] = 0.0; angfac[1, 9] = 1.0; angfac[2, 9] = -1.0; angfac[3, 9] = 0.0;
            angfac[0, 10] = 2.0; angfac[1, 10] = 0.0; angfac[2, 10] = 0.0; angfac[3, 10] = 0.0;

            double[] arguments = new double[NUM_HARMONICS];

            //Get secondOfWeek of year
            short year = Convert.ToInt16(time.Year);
            // Fractional part of secondOfWeek, in fraction
            double fday = time.SecondsOfDay;

            //Compute time
            double d = time.DayOfYear + 365.0 * (year - 1975.0) + Math.Floor(Convert.ToDouble(year - 1973.0) / 4.0);
            double t = (27392.500528 + 1.000000035 * d) / 36525.0;

            //Mean longitude of Sun at beginning of secondOfWeek
            double H0 = ((279.69668 + (36000.768930485 + 3.03e-4 * t) * t) * CoordConsts.DegToRadMultiplier);
            // Mean longitude of Moon at beginning of secondOfWeek
            double S0 = ((((1.9e-6 * t - 0.001133) * t +
                          481267.88314137) * t + 270.434358) * CoordConsts.DegToRadMultiplier);

            // Mean longitude of lunar perigee at beginning of secondOfWeek
            double P0 = ((((-1.2e-5 * t - 0.010325) * t +
                          4069.0340329577) * t + 334.329653) * CoordConsts.DegToRadMultiplier);

            for (int k = 0; k < NUM_HARMONICS; k++)
            {
                double temp = (sig[k] * fday + angfac[0, k] * H0 +
                        angfac[1, k] * S0 + angfac[2, k] * P0 + angfac[3, k] * CoordConsts.PI * 2);

                arguments[k] = Math.IEEERemainder(temp, CoordConsts.PI * 2);


                if (arguments[k] < 0.0)
                {
                    arguments[k] = arguments[k] + CoordConsts.PI * 2;
                }
            }
            return arguments;

        } 

    }
}
