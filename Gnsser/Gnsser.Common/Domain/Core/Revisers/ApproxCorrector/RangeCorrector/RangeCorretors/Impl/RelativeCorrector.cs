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
    /// 钟差的相对论改正，与光速相差转换为伪距改正。
    /// </summary>
    public class RelativeCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RelativeCorrector()
        {
            this.Name = "卫星钟差相对论距离改正";
            this.CorrectionType = CorrectionType.ClockRelative;
        }

     
        public override void Correct(EpochSatellite epochSatellite)
        {
            IEphemeris sat = epochSatellite.Ephemeris;
            double correction = GetRelativeCorrection(sat) * GnssConst.LIGHT_SPEED;

            this.Correction = (correction);
         } 

        /// <summary>
        /// 相对论效应改正。
        /// 由于卫星速度快，产生相对论效应，使卫星钟变慢。
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public static double GetRelativeCorrection(IEphemeris sat)
        {
            XYZ pos = sat.XYZ;
            XYZ speed = sat.XyzDot;
            return EphemerisUtil.GetRelativeCorrection(pos, speed);
        }
                
    }
}
