//2014.10.14， czs, edit, 在改正数中去掉了多余的钟差相对论改正，该改正应该放在钟差中，在星历计算之时或之前。

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;

namespace Gnsser.Correction
{    
    /// <summary>
    /// 地球自转效应改正
    /// sagnac effect correction
    /// </summary>
    public class SagnacEffectCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SagnacEffectCorrector()
        {
            this.Name = "sagnac效应（地球自转效应）距离改正";
            this.CorrectionType = CorrectionType.ClockRelative;
        }

     
        public override void Correct(EpochSatellite epochSatellite)
        {
            IEphemeris sat = epochSatellite.Ephemeris;
            XYZ reXyz = epochSatellite.SiteInfo.EstimatedXyz;
            double correction = GetRelativeCorrection(sat, reXyz);

            this.Correction = (correction);
         } 

        /// <summary>
        /// 地球自转效应改正。
        /// 
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public static double GetRelativeCorrection(IEphemeris sat, XYZ rr)
        {
            XYZ rs = sat.XYZ;
            //double OMGE = 7.2921151467E-5;   /* earth angular velocity (IS-GPS) (rad/s) */     

            double sagnacCorrect = GnssConst.EARTH_ROTATE_SPEED * (rs.X * rr.Y - rs.Y * rr.X) / GnssConst.LIGHT_SPEED;

            return sagnacCorrect;
        }
                
    }
}
