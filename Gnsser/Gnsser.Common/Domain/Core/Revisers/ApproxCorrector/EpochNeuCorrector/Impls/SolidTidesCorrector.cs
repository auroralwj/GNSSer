//2014.05.22, Cui Yang, created
//2014.06.30, Cui Yang, 极潮改正的参数更新为IERS2010
//2014.08.18, czs, 将结果采用NEU坐标表示，分别对应北东天方向。
//2014.09.15, cy, 重构固体潮


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
    public class SolidTidesCorrector : AbstractEpochNeuReviser
    {
        /** This class computes the effect of solid Earth tides at a given
    * position and epoch.
    *
    * The model used is the simple quadrupole response model described
    * by J.G. Williams (1970).
    *
    * Given the limitations of the algorithms used to compute the Sun 
    * and Moon positions, this class is limited to the period between 
    * March 1st, 1900 and February 28th, 2100.
    *
    */

        /// <summary>
        /// 固体潮改正，单位NEU
        /// </summary>
        public SolidTidesCorrector(DataSourceContext DataSouceProvider)
        {
            this.Name = "固体潮改正";
            this.CorrectionType = CorrectionType.SolidTides;
        }
        DataSourceContext DataSouceProvider;
        //Love numbers
        // private  const double H_LOVE = 0.609, L_LOVE = 0.0852;
        private const double H_LOVE = 0.6078, L_LOVE = 0.0847; //IERS 2010 

        // Phase lag. Assumed as zero here because no phase lag has been 
        // detected so far.
        private const double PH_LAG = 0.0;

        public override void Correct(EpochInformation epochInformation)
        {
            Time gpsTime = epochInformation.ReceiverTime;//.CorrectedTime;
            XYZ reciverPosition = epochInformation.SiteInfo.EstimatedXyz;

            //查找地球自转信息
            //gpst2utc
            Time tutc = gpsTime.GpstToUtc();
            Gnsser.Data.ErpItem erpv = null;
            if (DataSouceProvider.ErpDataService != null)
            {
                erpv = DataSouceProvider.ErpDataService.Get(tutc);
            }
            if (erpv == null) erpv = ErpItem.Zero;
             
            var pos = DataSouceProvider.UniverseObjectProvider.GetOrCreate(gpsTime, erpv); 

            //Gnsser.Data.ErpItem erpv = epochInformation.DataSouceProvider.ErpDataService.Gete);
            //XYZ sunPos = new XYZ(); XYZ moonPos = new XYZ(); double gmst = 0.0;
            //epochInformation.DataSouceProvider.UniverseObjectProvider.GetSunPosition(gpsTime, erpv, ref sunPos, ref moonPos,ref gmst);

            NEU correction = GetSolidTidesCorrectValue(gpsTime, reciverPosition, pos.SunXyz, pos.MoonXyz);

            this.Correction = (correction);
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
        /// 固体潮改正
        /// Returns the effect of solid Earth tides (meters) at the given position and epoch, in the Up-East-North (UEN) reference frame.
        /// 此处采用NEU表示，其中的各个分量相对应。
        /// </summary>
        /// <param name="gpsTime">Epoch to look up</param>
        /// <param name="position">Position of interest</param>
        /// <returns></returns>
        public static NEU GetSolidTidesCorrectValue(Time gpsTime, XYZ position, XYZ sunPos, XYZ moonPos)
        {

            NEU solidTides = new NEU();

            ////Objects to compute Sun and Moon positions
            //SunPosition sunPosition = new SunPosition();
            //MoonPosition moonPosition = new MoonPosition();

            //// Variables to hold Sun and Moon positions
            //XYZ sunPos = (sunPosition.GetPosition(gpsTime));
            //XYZ moonPos = (moonPosition.GetPosition(gpsTime));


            // Compute the factors for the Sun
            double rpRs = (position.X * sunPos[0] +
                         position.Y * sunPos[1] +
                         position.Z * sunPos[2]);

            double Rs2 = sunPos[0] * sunPos[0] +
                      sunPos[1] * sunPos[1] +
                      sunPos[2] * sunPos[2];
            double rp2 = (position.X * position.X + position.Y * position.Y + position.Z * position.Z);

            double xy2p = (position.X * position.X + position.Y * position.Y);
            double sqxy2p = (Math.Sqrt(xy2p));

            double sqRs2 = (Math.Sqrt(Rs2));

            double fac_s = (3.0 * SunMoonPosition.MU_SUN * rp2 / (sqRs2 * sqRs2 * sqRs2 * sqRs2 * sqRs2));

            double g1sun = (fac_s * (rpRs * rpRs / 2.0 - rp2 * Rs2 / 6.0));

            double g2sun = (fac_s * rpRs * (sunPos[1] * position.X -
                          sunPos[0] * position.Y) * Math.Sqrt(rp2) / sqxy2p);

            double g3sun = (fac_s * rpRs * (sqxy2p * sunPos[2] -
                          position.Z / sqxy2p * (position.X * sunPos[0] +
                          position.Y * sunPos[1])));

            // Compute the factors for the Moon
            double rpRm = (position.X * moonPos[0] +
                         position.Y * moonPos[1] +
                         position.Z * moonPos[2]);

            double Rm2 = (moonPos[0] * moonPos[0] +
                       moonPos[1] * moonPos[1] +
                       moonPos[2] * moonPos[2]);

            double sqRm2 = (Math.Sqrt(Rm2));

            double fac_m = (3.0 * SunMoonPosition.MU_MOON * rp2 / (sqRm2 * sqRm2 * sqRm2 * sqRm2 * sqRm2));

            double g1moon = (fac_m * (rpRm * rpRm / 2.0 - rp2 * Rm2 / 6.0));

            double g2moon = (fac_m * rpRm * (moonPos[1] * position.X -
                           moonPos[0] * position.Y) * Math.Sqrt(rp2) / sqxy2p);

            double g3moon = (fac_m * rpRm * (sqxy2p * moonPos[2] -
                           position.Z / sqxy2p * (position.X * moonPos[0] +
                           position.Y * moonPos[1])));



            // Effects due to the Sun
            double delta_sun1 = (H_LOVE * g1sun);
            double delta_sun2 = (L_LOVE * g2sun);
            double delta_sun3 = (L_LOVE * g3sun);

            // Effects due to the Moon
            double delta_moon1 = (H_LOVE * g1moon);
            double delta_moon2 = (L_LOVE * g2moon);
            double delta_moon3 = (L_LOVE * g3moon);

            // Combined effect
            //Up-East-North
            double up = delta_sun1 + delta_moon1;
            double east = delta_sun2 + delta_moon2;
            double north = delta_sun3 + delta_moon3;

            solidTides = new NEU(north, east, up);

            return solidTides;
        }
    }
}
