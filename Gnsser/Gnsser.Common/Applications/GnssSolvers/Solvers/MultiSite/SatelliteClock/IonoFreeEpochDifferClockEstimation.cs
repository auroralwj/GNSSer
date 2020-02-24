//2016.10.06 double creates on the train of quxian-xi'an
// 2016.10.09.22 double edits in hongqing

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
    /// 基于历元间差分的钟差估计。
    ///  </summary>
    public class IonoFreeEpochDifferClockEstimationer : MultiSitePeriodSolver
    {
        #region 构造函数
        public IonoFreeEpochDifferClockEstimationer(DataSourceContext DataSourceContext, GnssProcessOption model)
            : base(DataSourceContext, model)
        {
            this.MatrixBuilder = new IonoFreeEpochDifferClockEstimationMatrixBuilder(model);
        }


        #endregion


        #region 卡尔曼滤波
        
        public override BaseGnssResult CaculateKalmanFilter(MultiSitePeriodInfo recInfo, BaseGnssResult lastClockEstimationResult = null)
        {

            DifferClockEstimationResult last = null;
            if (lastClockEstimationResult != null) last = (DifferClockEstimationResult)lastClockEstimationResult;
            //  ISatWeightProvider SatWeightProvider = new SatElevateAndRangeWeightProvider();

            var matrixBuilder = (IonoFreeEpochDifferClockEstimationMatrixBuilder)MatrixBuilder;
            matrixBuilder.CurrentMaterial = recInfo;
            matrixBuilder.SetPreviousProduct(last);
            matrixBuilder.Build();


            //  this.Adjustment = new KalmanFilter( this.MatrixBuilder);
            this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));

            if (Adjustment.Estimated == null) return null;

            ////尝试固定模糊度  cuiyang 2015.07
            //int fixFlag = Ppp_AR.Process(recInfo, Adjustment);

            DifferClockEstimationResult result = (DifferClockEstimationResult)BuildResult();

            return result;
        }

        public override BaseGnssResult BuildResult()
        {
            DifferClockEstimationResult result = new DifferClockEstimationResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
            // result.PreviousResult = this.CurrentProduct;
            result.BasePrn = this.CurrentBasePrn;
            return result;
        }

        #endregion

    }
}
