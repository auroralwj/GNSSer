//2014.09.26, czs, create, 简单伪距单点定位。只需要导航文件和观测文件，不需要测站初始值，的快速伪距定位计算。
//2016.01.26, czs, edit in hongqing, 修复无初始坐标快速定位，原因为历元信息中增加了EstmatedXyz替代了ApproxXyz，伪距定位在10米内，已经足够了，  
//2017.09.05, czs, edit in hongqing, 整理代码，本类将不再扩展，制作纯粹的简单伪距定位，其它功能转移到 DynamicRangeMatrixBuilder 和 MultiSysRangeMatrixBuilder

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Correction;

namespace Gnsser.Service
{
    /// <summary>
    /// 简单伪距单点定位。只需要星历文件和观测文件，不需要测站初始值的快速伪距定位计算。
    /// </summary>
    public class SimpleRangePositioner : SingleSiteGnssSolver
    {
        #region 构造函数
        /// <summary>
        /// 简单伪距单点定位
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public SimpleRangePositioner(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "简单伪距定位";

            this.BaseParamCount = 4;
            this.MatrixBuilder = new SimpleRangeMatrixBuilder(this.Option);

            //这里是测试
            SingleFreqSelfIonoRangeReviser=(new SingleFreqSelfIonoRangeReviser(Option));


        }
        #endregion

        SingleFreqSelfIonoRangeReviser SingleFreqSelfIonoRangeReviser { get; set; }

        #region 核心计算方法
        /// <summary>
        ///  迭代求解     
        ///  误差方程：
        ///  V = coeffOfParams\hat X + D - ObsMinusApriori
        ///  令
        ///  freeTerm = coeffOfParams{X_0} + D - ObsMinusApriori
        ///  则
        ///  V = coeffOfParams\delta \hat X + freeTerm
        ///  V = coeffOfParams\delta \hat X + coeffOfParams{X_0} + D - ObsMinusApriori
        ///  
        /// 伪距单点定位误差方程：
        /// v = ex 站deltaX + ey 站deltaY + ez 站deltaZ - b + 伪距 - F
        /// F = ex 站星deltaX0 + ey 站星deltaY0 + ez 站星deltaZ0 - 光速 * 卫星钟差
        /// 其中：
        /// 伪距为观测值 ObsMinusApriori
        /// 站星delta 为估计值
        ///  光速 * 卫星钟差 为常量 D
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="lastResult"></param>
        /// <returns></returns>
        //public override SingleSiteGnssResult CaculateKalmanFilter(
        //    EpochInformation epochInfo,
        //    SingleSiteGnssResult lastResult = null)
        //{
        //    return Caculate(epochInfo);
        //}

        public override void Complete()
        {
            base.Complete();
            SingleFreqSelfIonoRangeReviser.Complete();
        }

        /// <summary>
        /// 由于滤波收敛较慢，通常初始坐标不准的情况下适合采用历元独立计算。
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public override SingleSiteGnssResult CaculateIndependent(EpochInformation epochInfo)
        {
            return Caculate(epochInfo);
        }

        private SingleSiteGnssResult Caculate(EpochInformation epochInfo)
        {
            if (epochInfo.EnabledSatCount < Option.MinSatCount) { log.Error("不足4颗可用卫星：" + epochInfo.EnabledSatCount); return null; }

           SingleFreqSelfIonoRangeReviser.Revise(ref epochInfo);

            RangePointPositionResult result = null;
            var prev = this.CurrentProduct;
            double differ = double.MaxValue;
            int index = 0;

            do
            {
                if (index > 0)
                {
                    EpochEphemerisSetter.Revise(ref epochInfo);   //采用修正后的钟差重新计算卫星坐标。
                    BuildAdjustMatrix();//重新生成矩阵                  
                }

                var obsMatrix = BuildAdjustObsMatrix(epochInfo);// new AdjustObsMatrix(this.MatrixBuilder);

               // var text = obsMatrix.ToReadableText();

                this.Adjustment = this.RunAdjuster(obsMatrix);

                result = new RangePointPositionResult(epochInfo, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
                if (prev != null)
                {
                    differ = result.EstimatedXyz.Distance(prev.EstimatedXyz);
                }

                prev = result;

                //实时更新测站坐标
                if ( true && this.IsUpdateEstimatePostition)
                {
                    epochInfo.SiteInfo.EstimatedXyz = result.EstimatedXyz;
                }

                 SingleFreqSelfIonoRangeReviser.Revise(ref epochInfo);

                // recInfo.ApproxXyz = result.EstimatedXyz;
                //log.Info(index + ", " + result.EstimatedXyz + ", " + result.EstimatedXyzRms);
                index++;
            } while (  false && index < this.Option.MaxLoopCount && differ > 1e-6);//result.EstimatedXyzRms.Length > 1 && 
            return result;
        }
        

        /// <summary>
        /// 结果生成
        /// </summary> 
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            return new RangePointPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);// { PreviousResult = this.CurrentProduct };
        }
        #endregion

        #region 快速计算一个坐标
        static object locker = new object();
        /// <summary>
        /// 快速计算一个坐标。自动重置数据流。
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <returns></returns>
        public static SingleSiteGnssResult GetApproxPosition(DataSourceContext DataSourceContext)
        {
            GnssProcessOption PositionOption = GnssProcessOption.GetDefaultSimplePseudoRangePositioningOption();
            var p = new SimpleRangePositioner(DataSourceContext, PositionOption)
            {
                IsUpdateEstimatePostition = true,
                IsPostCheckEnabled = false,//取消检查，否则报错，2019.05.16, czs
            };

            var Reviser = EpochInfoReviseManager.GetDefaultEpochInfoReviser(DataSourceContext, PositionOption, null);
            var checker = EpochCheckingManager.GetDefaultCheckers(DataSourceContext, PositionOption);
            var EpochEphemerisSetter = new EpochEphemerisSetter(DataSourceContext, PositionOption);
            lock (locker)
            {
                var initMaxRms = PositionOption.MaxStdDev;
                var initType = PositionOption.CaculateType;
                var initUpdate = PositionOption.IsUpdateEstimatePostition;
                var initPostCheck = PositionOption.IsResidualCheckEnabled;
                var initResultCheck = PositionOption.IsResultCheckEnabled;

                PositionOption.CaculateType = CaculateType.Independent;
                PositionOption.MaxStdDev = 9999999;
                PositionOption.IsResidualCheckEnabled = false;
                PositionOption.IsResultCheckEnabled = false;
                PositionOption.IsUpdateEstimatePostition = initUpdate;

                int i = 0;
                SingleSiteGnssResult GnssResult = null;
                List<SingleSiteGnssResult> results = new List<SingleSiteGnssResult>();
                foreach (var item in DataSourceContext.ObservationDataSource)
                {
                    i++;

                   if (!checker.Check(item)) { continue; }
                    var info = item;
                   // if (!Reviser.Revise(ref info)) { continue; }
                    Reviser.Revise(ref info);
                    if (info.EnabledSatCount < 4) { continue; }
                    GnssResult = p.Get(item);
                    if (GnssResult != null)
                    {
                        item.SiteInfo.SetApproxXyz(item.SiteInfo.EstimatedXyz);
                        results.Add(GnssResult);
                    }
                    if (i > 5 && results.Count > 0)
                    {
                        results.Sort(new Comparison<SingleSiteGnssResult>(delegate(SingleSiteGnssResult a, SingleSiteGnssResult b) { return (int)(1000 * (a.EstimatedXyzRms.Length - b.EstimatedXyzRms.Length)); }));
                        GnssResult = results[0];
                        break;
                    }
                }
                PositionOption.IsResidualCheckEnabled = initPostCheck;
                PositionOption.IsUpdateEstimatePostition = initUpdate;
                PositionOption.IsResultCheckEnabled = initResultCheck;
                PositionOption.CaculateType = initType;
                DataSourceContext.ObservationDataSource.Reset();
                PositionOption.MaxStdDev = initMaxRms;
                return GnssResult;
            }
        }
        #endregion
    }
}
