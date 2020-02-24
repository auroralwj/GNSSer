//2016.05.11, czs, create in hongqing, GnssSysRemover

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
    /// 移除未指定的GNSS系统观测数据
    /// </summary>
    public class GnssSysRemover : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PositionOption"></param>
        public GnssSysRemover(GnssProcessOption PositionOption)
            : this()
        { 
            this.SatelliteTypes = PositionOption.SatelliteTypes;
            log.Info("将移除非 " + String.Format(new EnumerableFormatProvider(), "{0} 系统的观测值", PositionOption.SatelliteTypes));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SatelliteTypes"></param>
        public GnssSysRemover(List<SatelliteType> SatelliteTypes):this()
        {
            this.SatelliteTypes = SatelliteTypes;
            log.Info("将移除非 " + String.Format(new EnumerableFormatProvider(), "{0} 系统的观测值", SatelliteTypes));
        }
        public GnssSysRemover()
        {
            this.Name = "GNSS 类型移除器";
        }
        /// <summary>
        /// 定位选项
        /// </summary> 
        public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 修正
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        {
            obj.RemoveOtherGnssSystem(SatelliteTypes);

            return true;
        }
    }
}
