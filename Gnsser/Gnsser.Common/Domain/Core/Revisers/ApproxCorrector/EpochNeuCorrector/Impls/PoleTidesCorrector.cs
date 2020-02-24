//2014.05.22, Cui Yang, created
//2014.06.30, Cui Yang, 极潮改正的参数更新为IERS2010
//2014.08.18, czs, 将结果采用NEU坐标表示，分别对应北东天方向。
//2014.09.15, cy, 重构


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Gnsser.Domain;
using Geo.Times;
using Gnsser.Data;

namespace Gnsser.Correction
{
    public class PoleTidesCorrector : AbstractEpochNeuReviser
    {
        /** This class computes the effect of pole tides, or more properly
       *  called "rotational deformations due to polar motion", at a given
       *  position and epoch.
       *
       *  The model used is the one proposed by the "International Earth
       *  Rotation and Reference Systems Service" (IERS) in its upcomming
       *  "IERS Conventions" document (Chapter 7), available at:
       *
       *  http://tai.bipm.org/iers/convupdt/convupdt.html
       *
       *  The pole movement parameters x, y for a given epoch may be
       *  found at:
       *
       *  ftp://hpiers.obspm.fr/iers/eop/eop.others
       *
       *  Maximum displacements because of this effect are:
       *
       *  \ltime Vertical:    2.5 cm
       *  \ltime Horizontal:  0.7 cm
       *
       *  For additional information you may consult: Wahr, J.M., 1985,
       *  "Deformation Induced by Polar Motion", Journal of Geophysical
       *  Research, Vol. 90, No B11, info. 9363-9368.
       *
       *  \warning Please take into account that pole tide equations in
       *  IERS document use co-latitude instead of latitude.
       */
        /// <summary>
        /// 极潮改正，单位NEU
        /// </summary>
        public PoleTidesCorrector(DataSourceContext DataSouceProvider)
        {
            this.Name = "极潮改正";
            this.CorrectionType = CorrectionType.PoleTides;
            this.DataSouceProvider = DataSouceProvider;
        }
        DataSourceContext DataSouceProvider;
        public override void Correct(EpochInformation epochInformation)
        {
            Time gpsTime = epochInformation.ReceiverTime;//.CorrectedTime;

            XYZ reciverPosition = epochInformation.SiteInfo.EstimatedXyz;

            GeoCoord geoCoord = epochInformation.SiteInfo.ApproxGeoCoord;

            if (XYZ.IsZeroOrEmpty( reciverPosition))
            {
                this.Correction = NEU.Zero;
                return;
            }

            //GPS时转UTC时
            Time tutc = gpsTime.GpstToUtc();

            //查找ERP信息
            //查找地球自转信息
            Gnsser.Data.ErpItem erpv = null;
            if (DataSouceProvider.ErpDataService != null)
            {
                erpv = DataSouceProvider.ErpDataService.Get(tutc);
            }
            if (erpv == null) erpv = ErpItem.Zero; 

            //EarthPoleDisplacement
            double xp = erpv.Xpole;//arcsec
            double yp = erpv.Ypole;

            //RTKLIB的极潮改正模块
            // NEU correction = GetPoleTidesCorrectValue2(reciverPosition, xp, yp);

            //GPSTK的极潮改正模块
            NEU correction = GetPoleTidesCorrectValue(gpsTime, reciverPosition, xp, yp, geoCoord);

            this.Correction = (correction);
        }

        /// <summary>
        /// 极潮改正，参考RTKLIB模块
        /// </summary>
        /// <param name="reciverPosition"></param>
        /// <param name="xDisplacement"></param>
        /// <param name="yDisplacement"></param>
        /// <returns></returns>
        private static NEU GetPoleTidesCorrectValue2(XYZ reciverPosition, double xDisplacement, double yDisplacement)
        {
            double[] pos = new double[2];
            pos[0] = Math.Asin(reciverPosition.Z / reciverPosition.Length);
            pos[1] = Math.Atan2(reciverPosition.Y, reciverPosition.X);

            double cosl = Math.Cos(pos[1]); double sinl = Math.Sin(pos[1]);

            double e = 9E-3 * Math.Sin(pos[0]) * (xDisplacement * sinl + yDisplacement * cosl);
            double n = -9E-3 * Math.Cos(2.0 * pos[0]) * (xDisplacement * cosl - yDisplacement * sinl);
            double u = -32E-3 * Math.Sin(2.0 * pos[0]) * (xDisplacement * cosl - yDisplacement * sinl);

            NEU correction = new NEU(n, e, u);
            return correction;
        }

        /** Returns the effect of pole tides (meters) at the given
          *  position and epoch, in the Up-East-North (UEN) reference frame.
          *
          * @param[in]  t Epoch to look up
          * @param[in]  info Position of interest
          *
          * @return a Triple with the pole tide effect, in meters and in
          *    the UEN reference frame.
          *
          * @throw InvalidRequest If the request can not be completed for any
          *    reason, this is thrown. The text may have additional
          *    information about the reason the request failed.
          *
          * @warning In order to use this method, you must have previously
          *    set the current pole displacement parameters.
          *
          */
        /// <summary>
        /// 极潮改正，需要通过时间读取相应文件，找到极点位置 x和y ，参考GPSTK模块
        /// </summary>
        /// <param name="gpsTime">Epoch to look up</param>
        /// <param name="position">Position of interest</param>
        /// <returns></returns>
        public static NEU GetPoleTidesCorrectValue(Time gpsTime, XYZ position, double xDisplacement, double yDisplacement, GeoCoord geoCoord)
        {

            //store the results
            NEU res = new NEU(0.0, 0.0, 0.0);

            //Declare J2000 reference time: January 1st, 2000, at noon
            Time j2000 = new Time(2000, 1, 1, 12, 0, 0.0);

            //Get current position's latitude and longitude, in radians           
            GeoCoord geoCoord0 = Geo.Coordinates.CoordTransformer.XyzToGeoCoord_Rad(position.X, position.Y, position.Z);



            double latitude = geoCoord.Lat * SunMoonPosition.DegToRad;
            double longitude = geoCoord.Lon * SunMoonPosition.DegToRad;

            // Compute appropriate running averages
            // Get time difference between current epoch and
            // J2000.0, in years
            double timedif = (double)((gpsTime.MJulianDays - j2000.MJulianDays) / 365.25M);

            //IERS mean pole model
            //double xpbar = (0.054 + timedif * 0.00083);
            //double ypbar = (0.357 + timedif * 0.00395);
            double xpbar = 0.0;
            double ypbar = 0.0;

            #region IERS Conventions 2010 p115

            if (gpsTime.Year < 2010)
            {
                xpbar = 55.974 * 1.0e-3 + timedif * 1.8243 * 1.0e-3 + timedif * timedif * 0.18413 * 1.0e-3 + timedif * timedif * timedif * 0.007024 * 1.0e-3;
                ypbar = 346.346 * 1.0e-3 + timedif * 1.7896 * 1.0e-3 + timedif * timedif * (-0.10729) * 1.0e-3 + timedif * timedif * timedif * (-0.000908) * 1.0e-3;
            }
            else
            {
                xpbar = 23.513 * 1.0e-3 + timedif * 7.6141 * 1.0e-3;
                ypbar = 358.891 * 1.0e-3 + timedif * (-0.6287) * 1.0e-3;
            }


            #endregion
            // Now, compute m1 and m2 parameters
            double m1 = (xDisplacement - xpbar);
            double m2 = (ypbar - yDisplacement);

            // Now, compute some useful values
            double sin2lat = (Math.Sin(2.0 * latitude));
            double cos2lat = (Math.Cos(2.0 * latitude));
            double sinlat = (Math.Sin(latitude));
            double sinlon = (Math.Sin(longitude));
            double coslon = (Math.Cos(longitude));

            // Finally, get the pole tide values, in UEN reference
            // frame and meters
            double u = -0.033 * sin2lat * (m1 * coslon + m2 * sinlon);
            double e = +0.009 * sinlat * (m1 * sinlon - m2 * coslon);
            double n = -0.009 * cos2lat * (m1 * coslon + m2 * sinlon);

            res = new NEU(n, e, u);

            // Please be aware that the former equations take into account
            // that the IERS pole tide equations use CO-LATITUDE instead
            // of LATITUDE. See Wahr, 1985.

            return res;
        }





    }
}
