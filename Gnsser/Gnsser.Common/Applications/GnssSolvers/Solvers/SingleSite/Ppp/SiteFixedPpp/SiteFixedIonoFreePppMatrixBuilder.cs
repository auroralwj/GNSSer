// 2014.09.03, czs, 独立于Ppp.cs
// 2014.09.05, czs, edit, 实现 IAdjustMatrixBuilder 接口
// 2016.03.11, czs, edit in hongqing, 提取基础的精密单点定位平差矩阵构建器
//2016.10.23, czs, edit in zhengzhou xinhua street, 修复错误（系数阵取消频率参数，观测值只取启用Enabled卫星），精度达到正常，
//2019.01.01, czs, edit in hmx, 提取继承自 IonoFreePppMatrixBuilder

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Gnsser.Models;
using Geo.Times; 
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 精密单点定位矩阵生成类。
    /// </summary>
    public class SiteFixedIonoFreePppMatrixBuilder : IonoFreePppMatrixBuilder
    {
        /// <summary>
        /// 先构造，再设置历元值。
        /// </summary>
        /// <param name="option">解算选项</param> 
        public SiteFixedIonoFreePppMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            option.IsFixingCoord = true;
            ParamNameBuilder = new SiteFixedIonoFreePppParamNameBuilder(option); 
        }
     
        

    }//end class
}