//2016.08.09, czs, create in fujianyongan, WM星间差分FCB计算器
//2016.08.19, czs, 安徽 黄山 屯溪, 宽项调通，重构
//2016.10.17.13.09, czs, 宽项计算，直接处产品
//2016.10.19, czs, 宽项计算，更名为 MultiSiteMwWideLaneSolver

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
using System.Collections.Concurrent;

namespace Gnsser
{
    /// <summary>
    /// 多站MW宽项计算器
    /// </summary>
    public class MultiSiteMwWideLaneSolver : BaseDictionary<string, EpochMwWideLaneSolver>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn"></param>
        /// <param name="SetWeightWithSat"></param>
        /// <param name="maxError"></param>
        /// <param name="IsOutputDetails"></param>
        public MultiSiteMwWideLaneSolver(SatelliteNumber BasePrn, bool SetWeightWithSat = false, double maxError = 2, bool IsOutputDetails = true)
        {
            this.BasePrn = BasePrn;
            this.MaxError = maxError;
            this.IsOutputDetails = IsOutputDetails;
            this.IsSetWeightWithSat = SetWeightWithSat;
        }
        /// <summary>
        /// 是否输出详细计算信息
        /// </summary>
        public bool IsOutputDetails { get; set; }
        /// <summary>
        /// 基准卫星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 按照卫星（高度角和距离）定权。否为等权处理。
        /// </summary>
        public bool IsSetWeightWithSat { get; set; }
        /// <summary>
        /// 最大误差
        /// </summary>
        public double MaxError { get; set; }


        public override EpochMwWideLaneSolver Create(string key)
        {
            return new EpochMwWideLaneSolver(BasePrn, key, IsSetWeightWithSat,MaxError ,IsOutputDetails);
        }
    }
 
}