//2014.05.22, Cui Yang, created
//2014.06.30, Cui Yang, 极潮改正的参数更新为IERS2010
//2014.08.18, czs, 将结果采用NEU坐标表示，分别对应北东天方向。
//2014.09.15, cy, 重构固体潮
//2015.04.12, cy, 增加RTKLIB的海潮代码


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser;
using Gnsser.Domain;
using Geo.Times;
using Gnsser.Data;

namespace Gnsser.Correction
{
    public class OceanTidesCorrector : AbstractEpochNeuReviser
    {
        /// <summary>
        /// 海洋潮汐改正，单位NEU
        /// This class computes the effect of ocean tides at a given position and epoch.
        /// This model neglects minor tides and nodal modulations, which may
        /// lead to errors up to 5 mm (RMS) at high latitutes. For more details,
        /// please see: http://tai.bipm.org/iers/convupdt/convupdt_c7.html
        public OceanTidesCorrector(DataSourceContext DataSouceProvider)
        {
            this.Name = "海洋潮汐改正";
            this.DataSouceProvider = DataSouceProvider;
            this.CorrectionType = CorrectionType.OceanTides;
        }

        DataSourceContext DataSouceProvider;
        public override void Correct(EpochInformation epochInformation)
        {
            string markerName = epochInformation.SiteInfo.SiteName;

            //   Time gpsTime = epochInformation.CorrectedTime;
            Time gpsTime = epochInformation.ReceiverTime;


            //gpst2utc
            Time tutc = gpsTime.GpstToUtc();

            //查找地球自转信息
            Gnsser.Data.ErpItem erpv = null;
            if (DataSouceProvider.ErpDataService != null)
            {
               erpv = DataSouceProvider.ErpDataService.Get(tutc);
            }
            if (erpv == null) erpv = ErpItem.Zero;

            //GPS时转UTC时，再加上ERP改正
            gpsTime = tutc + erpv.Ut12Utc;


            // Geo.Algorithm.Matrix harmonics = new Geo.Algorithm.Matrix(6, 11, 0.0);


            // harmonics = epochInformation.AssistantInfo.Harmonics; 
            if(markerName.Length > 4)
            {
                markerName = markerName.Substring(0, 4);
            }
            Geo.Algorithm.IMatrix harmonics = DataSouceProvider.OceanLoadingDataSource.Get(markerName);


            //GPSTK模块
            // NEU correction = GetOceanTidesCorrectValue(gpsTime,  harmonics);

            //RTKLIB模块
            NEU correction = GetOceanTidesCorrectValue1(gpsTime, harmonics);

            this.Correction = (correction);
        }

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
        /// <summary>
        /// 海洋负荷改正,采用GPSTK模块
        /// 此处采用NEU表示，其中的各个分量相对应。
        /// </summary>
        /// <param name="gpsTime">Epoch to look up</param>
        /// <param name="position">Position of interest</param>
        /// <returns></returns>
        public static NEU GetOceanTidesCorrectValue(Time gpsTime, Geo.Algorithm.IMatrix harmonics)
        {



            const int NUM_COMPONENTS = 3;
            const int NUM_HARMONICS = 11;

            double[] arguments = new double[11];
            // List<double> arguments = new List<double>(11);
            //Compute arguments
            arguments = GetArg(gpsTime);


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

            NEU oceanTides = new NEU(north, east, up);

            return oceanTides;
        }

        /** Compute the value of the corresponding astronomical arguments,
           * in radians. This routine is based on IERS routine ARG.f.
           *
           * @param time      Epoch of interest
           *
           * @return A Vector<double> of 11 elements with the corresponding
           * astronomical arguments to be used in ocean loading model.
           */
        private static double[] GetArg(Time time)
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

        /// <summary>
        /// 海潮改正模型，采用RTKLIB的模块
        /// 2015.04.12
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="harmonics"></param>
        /// <returns></returns>
        public static NEU GetOceanTidesCorrectValue1(Time gpsTime, Geo.Algorithm.IMatrix harmonics)
        {
            double[][] args = {
                                  new double[]  {1.40519E-4, 2.0,-2.0, 0.0, 0.00},  /* M2 */
                                  new double[]  {1.45444E-4, 0.0, 0.0, 0.0, 0.00},  /* S2 */
                                  new double[]{1.37880E-4, 2.0,-3.0, 1.0, 0.00},  /* N2 */
                                  new double[] {1.45842E-4, 2.0, 0.0, 0.0, 0.00},  /* K2 */
                                  new double[] {0.72921E-4, 1.0, 0.0, 0.0, 0.25},  /* K1 */
                                  new double[]{0.67598E-4, 1.0,-2.0, 0.0,-0.25},  /* O1 */
                                  new double[]{0.72523E-4,-1.0, 0.0, 0.0,-0.25},  /* P1 */
                                  new double[]{0.64959E-4, 1.0,-3.0, 1.0,-0.25},  /* Q1 */
                                  new double[]{0.53234E-5, 0.0, 2.0, 0.0, 0.00},  /* Mf */
                                  new double[]{0.26392E-5, 0.0, 1.0,-1.0, 0.00},  /* Mm */
                                  new double[]   {0.03982E-5, 2.0, 0.0, 0.0, 0.00}   /* Ssa */
                              }; //11 * 5 
            Time ep1975 = new Time(1975, 1, 1);
            //angular argument
            // double fday = gpsTime.SecondsOfDay;// gpsTime.Hour * 3600 + gpsTime.Minute * 60 + gpsTime.Fraction;

            double fday = gpsTime.Hour * 3600 + gpsTime.Minute * 60 + gpsTime.Seconds;




            Time gpsTime0 = gpsTime.Date;

            double days = (double)(gpsTime0 - ep1975) / 86400.0;
            double t, t2, t3, ang;
            double[] a = new double[5]; double[] dp = new double[3];

            t = (27392.500528 + 1.000000035 * days) / 36525.0;
            t2 = t * t; t3 = t2 * t;

            a[0] = fday;
            a[1] = (279.69668 + 36000.768930485 * t + 3.03E-4 * t2) * SunMoonPosition.DegToRad; /* H0 */
            a[2] = (270.434358 + 481267.88314137 * t - 0.001133 * t2 + 1.9E-6 * t3) * SunMoonPosition.DegToRad; /* S0 */
            a[3] = (334.329653 + 4069.0340329577 * t + 0.010325 * t2 - 1.2E-5 * t3) * SunMoonPosition.DegToRad; /* P0 */
            a[4] = 2.0 * SunMoonPosition.PI;

            //displancements by 11 constituents
            for (int i = 0; i < 11; i++)
            {
                ang = 0.0;
                for (int j = 0; j < 5; j++) ang += a[j] * args[i][j];
                for (int j = 0; j < 3; j++) dp[j] += harmonics[j, i] * Math.Cos(ang - harmonics[j + 3, i] * SunMoonPosition.DegToRad);
            }

            double e = -dp[1];
            double n = -dp[2];
            double u = dp[0];

            NEU res = new NEU(n, e, u);

            return res;

        }
    }
}
