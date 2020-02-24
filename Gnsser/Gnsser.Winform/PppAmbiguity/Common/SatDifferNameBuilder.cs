//2016.04.02, czs, create in hongqing, PPP 模糊度
//2016.04.08, czs, edit in hongqing, 更名为 IonoFreePppFcbSolver
//2016.06.21, czs, edit in hongqing, 基准卫星变换

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.IO;
using Geo.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;

namespace Gnsser
{
   
    /// <summary>
    /// 名称构造器。
    /// </summary>
    public class SatDifferNameBuilder
    {
        /// <summary>
        /// 名称构造器
        /// </summary>
        /// <param name="BasePrn"></param>
        public SatDifferNameBuilder(SatelliteNumber BasePrn)
        {
            this.BasePrn = BasePrn;
        }
        /// <summary>
        /// 基准卫星编号
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 当前卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 生成名称。
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            return Prn + "-" + BasePrn;
        }
    }
}