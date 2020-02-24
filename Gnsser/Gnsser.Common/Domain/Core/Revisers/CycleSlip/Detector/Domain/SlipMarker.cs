// 2014.09.10, czs, create, 周跳探测初步

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Times;
using System.Text;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Times; 

namespace Gnsser.Checkers
{
    /// <summary>
    /// 周跳标记。
    /// </summary>
    public class SlipMarker
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Prn"></param>
        /// <param name="Time"></param>
        /// <param name="ObservationNames"></param>
        public SlipMarker(SatelliteNumber Prn, Time GpsTime, FrequenceType PhaseName)
        {
            this.Prn = Prn;
            this.GpsTime = GpsTime;
            this.PhaseName = PhaseName;
        }
        /// <summary>
        /// 观测量类型
        /// </summary>
        public FrequenceType PhaseName { get; set; }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time GpsTime { get; set; }

        public override string ToString()
        {
            return Prn.ToString() + PhaseName + GpsTime;
        }


    }
}
