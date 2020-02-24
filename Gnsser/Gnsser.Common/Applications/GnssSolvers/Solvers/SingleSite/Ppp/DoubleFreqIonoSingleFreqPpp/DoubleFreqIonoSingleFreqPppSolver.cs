//2018.07.05, czs, create in HMX, 双频电离层延迟改正，单频PPP

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

namespace Gnsser.Service
{
    /// <summary>
    /// 双频电离层延迟改正，单频PPP
    /// </summary>
    public class DoubleFreqIonoSingleFreqPppSolver : SingleSiteGnssSolver
    {
        #region 构造函数 
        
        /// <summary>
        ///  通用单站单频计算，高度可配置 
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public DoubleFreqIonoSingleFreqPppSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "单站单频计算";
             this.MatrixBuilder = new DoubleFreqIonoSingleFreqPppMatrixBuilder(PositionOption); 
        }

        #endregion

        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
             return new DoubleFreqIonoSingleFreqPppGnssResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }
}
