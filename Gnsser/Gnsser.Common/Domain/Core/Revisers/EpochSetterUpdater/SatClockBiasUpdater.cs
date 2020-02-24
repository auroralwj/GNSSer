// 2014.09.15, czs, create, 

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Data;

namespace Gnsser
{
    /// <summary>
    /// 钟差改正更新器
    /// </summary>
    public class SatClockBiasUpdater : EpochInfoReviser
    {
        /// <summary>
        /// 钟差改正更新器构造函数
        /// </summary>
        public SatClockBiasUpdater(ClockService ClockDataSource)
        {
            this.ClockDataSource = ClockDataSource;
        }
        /// <summary>
        /// 钟差数据源。
        /// </summary>
        public  ClockService ClockDataSource { get; set; }


        public override bool Revise(ref EpochInformation info)
        {
            foreach (var item in info)
            {
                #region 相对论改正  不加算的很差 
                XYZ SatXyz = item.Ephemeris.XYZ;
                XYZ SatSpeed = item.Ephemeris.XyzDot;
                double relativeTime = GetRelativeCorrection(SatXyz, SatSpeed); 
               #endregion
                
                AtomicClock clock =  ClockDataSource.Get(item.Prn, item.EmissionTime);
                item.Ephemeris.ClockBias = clock.ClockBias + relativeTime;
                //如果钟漂不为 0 ，则赋值。
                if (clock.ClockDrift != 0) item.Ephemeris.ClockDrift = clock.ClockDrift; 
            }

            return info.EnabledPrns.Count > 0;
        }

        /// <summary>
        /// 相对论改正。
        /// </summary>
        /// <param name="SatXyz">卫星位置</param>
        /// <param name="SatSpeed">卫星速度</param>
        /// <returns></returns>
        private static double GetRelativeCorrection(XYZ SatXyz, XYZ SatSpeed)
        {
            double relativity = -2 * (SatXyz.X / GnssConst.LIGHT_SPEED) * (SatSpeed.X / GnssConst.LIGHT_SPEED)
                - 2 * (SatXyz.Y / GnssConst.LIGHT_SPEED) * (SatSpeed.Y / GnssConst.LIGHT_SPEED)
                - 2 * (SatXyz.Z / GnssConst.LIGHT_SPEED) * (SatSpeed.Z / GnssConst.LIGHT_SPEED);

            return relativity;
        }

    }
}
