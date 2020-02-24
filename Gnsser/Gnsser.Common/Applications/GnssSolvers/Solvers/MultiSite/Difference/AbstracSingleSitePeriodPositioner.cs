//2017.09.18, czs, edit in hongqing, 单站多历元GNSS计算

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Filter;
using Gnsser.Checkers;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Geo.Algorithm;

namespace Gnsser.Service
{

    /// <summary>
    /// 单站多历元GNSS计算
    /// </summary>
    public abstract class AbstracSingleSitePeriodPositioner : SingleSitePeriodSolver
    {
        /// <summary>
        /// 单站多历元GNSS计算
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public AbstracSingleSitePeriodPositioner(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
            : base(DataSourceContext, GnssOption)
        {
           
        }

        #region 平差计算


        /// <summary>
        /// Kalmam滤波。
        /// </summary>
        /// <param name="MultiSitePeriodInfo">接收信息</param> 
        /// <param name="lastResult">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public override BaseGnssResult CaculateKalmanFilter(PeriodInformation MultiSitePeriodInfo, BaseGnssResult lastResult = null)
        { 
            if (!this.MatrixBuilder.IsAdjustable)
            {
                log.Debug("不适合平差！" + MatrixBuilder.Message);
                return null;
            }

            // this.Adjustment = new KalmanFilter(MatrixBuilder);
            this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));

            //固定解 
            return BuildResult();
        }
        /// <summary>
        /// 结果生成
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            return new SingleSitePeriodInfoGnssResult(this.CurrentMaterial, Adjustment,  this.MatrixBuilder.GnssParamNameBuilder);
        }

    
        #endregion

    }
}
