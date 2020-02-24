// 2016.10.09.22 double creates in hongqing

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
using Geo.Algorithm;
using Geo.Times;
using Gnsser.Service;

namespace Gnsser
{
   
    /// <summary>
    /// 基于3频的钟差估计。
    ///  </summary>
    public class TriFreqIonoFreeClockEstimationer : MultiSiteEpochSolver
    {
        #region 构造函数
        public TriFreqIonoFreeClockEstimationer(DataSourceContext DataSourceContext, GnssProcessOption model)
            : base(DataSourceContext, model)
        {
            this.MatrixBuilder = new TriFreqIonoFreeClockEstimationMatrixBuilder(model);
        }


        #endregion
 
         
        public override BaseGnssResult BuildResult()
        {
            ClockEstimationResult result = new ClockEstimationResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
           // result.PreviousResult = this.CurrentProduct;
            result.BasePrn = this.CurrentBasePrn;
            return result;
        } 

    }
}
