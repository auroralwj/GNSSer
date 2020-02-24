//2016.08.09, czs, create in fujianyongan, WM星间差分FCB计算器
//2016.08.19, czs, 安徽 黄山 屯溪, 宽项调通，重构
//2016.10.17.13.09, czs, 宽项计算，直接处产品

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
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
using Geo.Times;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 卫星差分数据计算值管理器
    /// </summary>
    public class EpochSatDifferValueManager : BaseDictionary<SatelliteNumber, EpochSatDifferValue> { }
    /// <summary>
    /// 卫星数值管理器。
    /// </summary>
    public class EpochSatValueManager : BaseDictionary<SatelliteNumber, EpochSatValue> { }

    /// <summary>
    /// 历元卫星信息。具有原始值，平滑值，可以用于存储和表达星间单差窄巷。
    /// </summary>
    public class EpochSatValue
    {  /// <summary>
        /// 构造函数
        /// </summary>
        public EpochSatValue()
        {

         }

        /// <summary>
        /// 历元标记
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 原始值
        /// </summary>
        public double RawValue { get; set; }
        /// <summary>
        /// 平滑值
        /// </summary>
        public double SmoothValue { get; set; }
        /// <summary>
        /// 绑定数据
        /// </summary>
        public Object Tag { get; set; }

    }

    /// <summary>
    /// 卫星值,具有差分值三个数值属性。
    /// 可以用于存储星间单差宽项，模糊度浮点解等。
    /// </summary>
    public class EpochSatDifferValue : EpochSatValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
         public  EpochSatDifferValue(){

         }
        /// <summary>
        /// 平滑数据和基准卫星的差分值
        /// </summary>
        public double DifferValue { get; set; }
    }

}