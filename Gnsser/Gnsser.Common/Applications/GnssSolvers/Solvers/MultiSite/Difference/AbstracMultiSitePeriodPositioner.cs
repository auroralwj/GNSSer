//2012.05.29, czs, creare, 单差,载波差分定位。许其凤,164-167 
//2014.12.10, czs, edit in jinxinliaomao shaungliao, 短基线载波相位差分，单差
//2014.12.14, czs, edit in namu shaungliao, 单基线差分定位顶层类
//2016.04.25, czs, edit in hongqing, 重构，多站，

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
    /// 双站多历元GNSS计算器。
    /// 基线载波相位差分，单基线差分定位顶层类
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站) 
    /// </summary>
    public abstract class AbstracMultiSitePeriodPositioner : MultiSitePeriodSolver
    {
        /// <summary>
        /// 差分
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public AbstracMultiSitePeriodPositioner(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
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
        //public override BaseGnssResult CaculateKalmanFilter(MultiSitePeriodInfo MultiSitePeriodInfo, BaseGnssResult lastResult = null)
        //{ 
        //    if (!this.MatrixBuilder.IsAdjustable)
        //    {
        //        log.Debug("不适合平差！" + MatrixBuilder.Message);
        //        return null;
        //    }

        //    // this.Adjustment = new KalmanFilter(MatrixBuilder); 
        //    this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));

        //    //固定解 
        //    return BuildResult();
        //}
        /// <summary>
        /// 结果生成
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            return new TwoSitePeriodDifferPositionResult(this.CurrentMaterial, Adjustment, this.CurrentBasePrn, this.MatrixBuilder.GnssParamNameBuilder);
        }

    
        #endregion

    }
}
