
//2015.01.08, czs, create in namu, 一颗卫星的时段

using System;
using System.Collections.Generic;
using System.Text;
using System.IO; 
using Gnsser.Times;
using Geo; 
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm; 
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Geo.Times; 
using Gnsser.Correction;

namespace Gnsser
{
    /// <summary>
    /// 一颗卫星的时段。
    /// 用于排序的，排序为有效日期长度。
    /// </summary>
    public class SatPeriod : IComparable<SatPeriod>
    {
        /// <summary>
        /// 构造函数。一颗卫星的时段。
        /// </summary>
        /// <param name="Prn">卫星编号</param>
        /// <param name="TimePeriod">时段</param>
        public SatPeriod(SatelliteNumber Prn, BufferedTimePeriod TimePeriod)
        {
            this.Prn = Prn;
            this.TimePeriod = TimePeriod;
        }
        /// <summary>
        /// 卫星编号。
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 时段
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; }

        public int CompareTo(SatPeriod other)
        {
            return this.TimePeriod.Span.CompareTo(other.TimePeriod.Span);
        }

        public override string ToString()
        {
            return Prn + " : " + TimePeriod;
        }
    }
}
