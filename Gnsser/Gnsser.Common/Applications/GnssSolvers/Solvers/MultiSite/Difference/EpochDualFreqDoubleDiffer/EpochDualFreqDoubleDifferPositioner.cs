//2018.07.26, czs, create in HMX, 简易近距离单历元载波相位双差


using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser;

namespace Gnsser.Service
{
    /// <summary>
    ///  简易近距离单历元载波相位双差
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站)
    /// </summary>
    public class EpochDualFreqDoubleDifferPositioner : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        public EpochDualFreqDoubleDifferPositioner(DataSourceContext Context, GnssProcessOption Option)
        :base( Context,   Option)
        { 
            this.ResidualsAnalysis = new ResidualsAnalysis();
            this.DualBandCycleSlipDetector = new DualBandCycleSlipDetector();
            this.Name = "双频单历元载波相位双差";
            
            //默认双差基础参数为3个坐标坐标
            var distanceOfBaseline = (Context.ObservationDataSources.BaseDataSource.SiteInfo.ApproxXyz - Context.ObservationDataSources.OtherDataSource.SiteInfo.ApproxXyz).Length;
            if (distanceOfBaseline <= Option.MaxDistanceOfShortBaseLine)
            {
                this.BaseParamCount = 3;
            }
            else if (distanceOfBaseline > Option.MaxDistanceOfShortBaseLine  && distanceOfBaseline < Option.MinDistanceOfLongBaseLine)
            {
                this.BaseParamCount = 4;
            }

            if (distanceOfBaseline > Option.MaxDistanceOfShortBaseLine)
            {
                this.BaseParamCount = 4;
                log.Warn("基线距离有点长： " + distanceOfBaseline.ToString("0.0000") +"m, 我们默认为  " + Option.MaxDistanceOfShortBaseLine + "m 内， 电离层可能影响精度，请使用无电离层计算。");
            }

            if (this.Option.IsEstimateTropWetZpd)
            {
                this.BaseParamCount = 4;
                log.Warn("指定了对流层湿延迟估计");
            }

            this.IsBaseSatelliteRequried = true;//强制启用基准星 
            this.MatrixBuilder = new EpochDualFreqDoubleDifferMatrixBuilder(base.Option, BaseParamCount);
        }

        #endregion

        #region 属性  
        
        /// <summary>
        /// 残差分析
        /// </summary>
        public ResidualsAnalysis ResidualsAnalysis { get; set; }

        /// <summary>
        /// 双频周跳探测
        /// </summary>
        public DualBandCycleSlipDetector DualBandCycleSlipDetector { get; set; }

        #endregion         

        #region 核心计算方法

        #region 卡尔曼滤波
        /// <summary>
        /// Kalmam滤波。
        /// </summary>
        /// <param name="epochInfos">接收信息</param> 
        /// <param name="lastResult">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public override BaseGnssResult CaculateKalmanFilter(MultiSiteEpochInfo epochInfos, BaseGnssResult lastResult = null)
        {
            var result = base.CaculateKalmanFilter(epochInfos, lastResult) as EpochDualFreqDoubleDifferPositionResult;
            //这里其实不用了，顶层已经做了
            if (Option.PositionType == PositionType.动态定位 && Option.IsUpdateEstimatePostition)
            {
                epochInfos.OtherEpochInfo.SiteInfo.EstimatedXyz = result.GetEstimatedBaseline().EstimatedXyzOfRov;
            }

            this.SetProduct( result);

            return result;
        }
        #endregion

        
        #endregion 
        /// <summary>
        ///结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new EpochDualFreqDoubleDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn);
            return result;
        }
    }
}
