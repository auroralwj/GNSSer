//2014.09.04, czs, edit, 基本理清思路
//2014.12.11, czs, edit in jinxinliangmao shuangliao, 差分定位矩阵生成器
//2016.03.10, czs, edit in hongqing, 重构，准备重构定位器
//2016.04.30, czs, edit in hongqing, 重命名为 BaseGnssMatrixBuilder
//2016.04.30, czs, refactor in hongqing,  基于泛型的GNSS矩阵生成器
//2017.09.02, czs, refactor in hongqing, 重构状态转移模型

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
    /// <summary>
    /// 基础的单站单历元GNSS矩阵生成器
    /// </summary>
    public abstract class SingleSiteGnssMatrixBuilder : BaseGnssMatrixBuilder<SingleSiteGnssResult, EpochInformation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleSiteGnssMatrixBuilder(GnssProcessOption GnssOption)
            : base(GnssOption)
        {
        }
    }
}