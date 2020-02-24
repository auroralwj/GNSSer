//2014.09.24, czs, create, 卫星可用性

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Data
{
     
    /// <summary>
    /// 卫星可用性类，记录不可用卫星。
    /// </summary>
    public class SatExcludeItem
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public SatExcludeItem(Time GpsTime,List<SatelliteNumber> prns )
        {
            this.Date = GpsTime;
            this.Prns = prns;
        }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public List<SatelliteNumber> Prns { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public Time Date { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Date.ToDateString());
            foreach (var item in Prns)
            {
                sb.Append(" ");
                sb.Append(item);
            }
            sb.AppendLine();

            return sb.ToString();
        }
    }
}