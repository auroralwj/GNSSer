//2018.11.11, czs, create in hmx, 移除指定的卫星

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Correction;
using Gnsser.Filter;

namespace Gnsser
{
    /// <summary>
    /// 移除指定的卫星
    /// </summary>
    public class SatelliteRemover : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PositionOption"></param>
        public SatelliteRemover(GnssProcessOption PositionOption)
            : this()
        { 
            this.SatPrns = PositionOption.SatsToBeRemoved;
            log.Info("将移除卫星 " + String.Format(new EnumerableFormatProvider(), "{0} 的观测值", PositionOption.SatsToBeRemoved));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prns"></param>
        public SatelliteRemover(List<SatelliteNumber> prns):this()
        {
            this.SatPrns = prns;
            log.Info("将移除非 " + String.Format(new EnumerableFormatProvider(), "{0} 系统的观测值", prns));
        }
        public SatelliteRemover()
        {
            this.Name = "GNSS 卫星移除器";
        }
        /// <summary>
        /// 移除卫星编号
        /// </summary> 
        public List<SatelliteNumber> SatPrns { get; set; }
        /// <summary>
        /// 修正
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        {
            obj.Remove(SatPrns);

            return true;
        }
    }
}
