//2018.08.12, czs, create in hmx, 多频率伪距定位

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
    /// 多频率伪距定位 
    /// </summary>
    public class MultiFreqRangePositioner : SingleSiteGnssSolver
    {
        #region 构造函数 
        /// <summary>
        /// 多频率伪距定位
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public MultiFreqRangePositioner(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "多频率伪距定位";
          
            this.BaseParamCount = 4;
            this.MatrixBuilder = new MultiFreqRangeMatrixBuilder(this.Option); 
            
         }
        #endregion 
         
        #region 核心计算方法
        /// <summary>
        ///  迭代求解      
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="lastResult"></param>
        /// <returns></returns>
        public override SingleSiteGnssResult CaculateKalmanFilter(
            EpochInformation epochInfo,
            SingleSiteGnssResult lastResult = null)
        {
            return base.CaculateKalmanFilter(epochInfo, lastResult);
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
                this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));

                result = new RangePointPositionResult(epochInfo, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
                if (prev != null)
                {
                    differ = result.EstimatedXyz.Distance(prev.EstimatedXyz);
                }

                prev = result;

                //实时更新测站坐标
                if (this.IsUpdateEstimatePostition)
                {
                    epochInfo.SiteInfo.EstimatedXyz = result.EstimatedXyz;
                }

                // recInfo.ApproxXyz = result.EstimatedXyz;
                //log.Info(index + ", " + result.EstimatedXyz + ", " + result.EstimatedXyzRms);
                index++;
            } while (  index < this.Option.MaxLoopCount && differ > 1e-6);//result.EstimatedXyzRms.Length > 1 && 
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
         
    }
}
