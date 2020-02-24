//2015.05.10, czs, edit in namu, 增加注释

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 钟差解算站。
    /// 出现在钟差头部文件中。
    /// </summary>
    public class ClockSolnStation
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 空间直角坐标
        /// </summary>
        public XYZ XYZ { get; set; }
        /// <summary>
        /// 大地坐标
        /// </summary>
        public GeoCoord GeoCoord { get; set; }
 
    }

}
