//2018.05.15, czs, create, 基础的单站单星单历元GNSS矩阵生成器


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils; 
using Geo.Coordinates;
using Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
    /// <summary>
    /// 基础的单站单星单历元GNSS矩阵生成器
    /// </summary>
    public abstract class SingleSiteSingleSatGnssMatrixBuilder : SimpleBaseGnssMatrixBuilder<BaseSingleSatGnssResult, EpochSatellite>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleSiteSingleSatGnssMatrixBuilder(GnssProcessOption GnssOption)
            : base(GnssOption)
        {
        } 
    }
}