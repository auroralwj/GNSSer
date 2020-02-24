//2014.10.14, czs, edit, 去掉了多余的钟差相对论改正，该改正应该放在钟差中，在星历计算之时或之前。
//2018.05.18, czs, edit in HMX, 直接 改正到站星对象上

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
    /// 卫星钟改正， 伪距改正数
    /// </summary>
    public class SatClockBiasCorrector :  AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public SatClockBiasCorrector(   )
        {
            this.Name = "卫星钟差距离改正";
            this.CorrectionType = CorrectionType.ClockBiasCorrector;
        }

        public override void Correct(EpochSatellite epochSatellite)
        {
           if(epochSatellite.Ephemeris == null)
            {
                return;
            }
            
            //卫星钟差δts所引起距离误差cδts，在这里变成了观测近似值的钟差距离，改正到观测值的近似值上，其符号为负，见公式推导。
            double correction = -1.0 * GetClockBiasCorectValue(epochSatellite.Ephemeris.ClockBias);


            epochSatellite.SetCommonCorrection( CorrectionNames.SatClockBiasDistance.ToString(), correction);


            this.Correction = 0;// (correction);
        }

        /// <summary>
        /// 钟差距离改正。 LIGHT_SPEED * svClock_sec。
        /// 改正后的伪距 = 伪距测量值 + 伪距改正值
        ///钟差是指同一时刻两台钟的钟面时之差。
        ///系统时间 = 钟面时 + 卫星钟差
        ///定义：dT(r) = Time(r) - Time (sys)
        ///
        /// </summary>
        /// <param name="svClock_sec"></param>
        /// <returns></returns>
        public static double GetClockBiasCorectValue(double svClock_sec)
        {  
            double range1SvDistance = GnssConst.LIGHT_SPEED * svClock_sec;
            return range1SvDistance;
        }
    }
}
