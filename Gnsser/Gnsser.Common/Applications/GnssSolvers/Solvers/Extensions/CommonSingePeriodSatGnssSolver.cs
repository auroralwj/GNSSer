//2018.06.04, czs, create in hmx, 通用单站单星多历元GNSS计算

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Gnsser.Checkers; 
using Geo.Times;
using Gnsser.Filter;

namespace Gnsser.Service
{
    /// <summary>
    /// 通用单站单星多历元GNSS计算
    /// </summary>
    public class CommonSingePeriodSatGnssSolver : BaseAbstractGnssSolver<BaseSinglePeriodSatGnssResult, PeriodSatellite>
    {
        #region 构造函数

        /// <summary>
        /// 通用单站单星多历元GNSS计算
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        /// <param name="gnssMatrixBuilder"></param>
        public CommonSingePeriodSatGnssSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption, SingleSiteSinglePeriodSatGnssMatrixBuilder gnssMatrixBuilder)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "通用单站单星多历元GNSS计算";
            this.MatrixBuilder = gnssMatrixBuilder; 
        }
        #endregion  
          
        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override BaseSinglePeriodSatGnssResult BuildResult()
        {
            return new BaseSinglePeriodSatGnssResult(Adjustment,this.CurrentMaterial);//, this.MatrixBuilder.GnssParamNameBuilder);
        }
    } 
}