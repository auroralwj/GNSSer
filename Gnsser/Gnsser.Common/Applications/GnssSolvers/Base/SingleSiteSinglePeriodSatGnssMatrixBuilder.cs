//2018.06.04, czs, create, 基础的单站单星多历元GNSS矩阵生成器


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
    /// 基础的单站单星多历元GNSS矩阵生成器
    /// </summary>
    public abstract class SingleSiteSinglePeriodSatGnssMatrixBuilder : SimpleBaseGnssMatrixBuilder<BaseSinglePeriodSatGnssResult, PeriodSatellite>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleSiteSinglePeriodSatGnssMatrixBuilder(GnssProcessOption GnssOption)
            : base(GnssOption)
        {
        } 
    }
}