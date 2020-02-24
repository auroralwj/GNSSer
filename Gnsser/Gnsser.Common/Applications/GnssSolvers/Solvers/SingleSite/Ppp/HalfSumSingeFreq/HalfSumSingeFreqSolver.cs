//2017.11.10, czs, create in hongqing, 单频半和模型

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
    /// 单频半和模型
    /// </summary>
    public class HalfSumSingeFreqSolver : SingleSiteGnssSolver
    {
        #region 构造函数 
        
        /// <summary>
        ///  单频半和模型 
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public HalfSumSingeFreqSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "单频半和模型";
            this.MatrixBuilder = new HalfSumSingeFreqMatrixBuilder(PositionOption); 
        }

        #endregion

        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
             return new CommonSingeFreqGnssResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }
}
