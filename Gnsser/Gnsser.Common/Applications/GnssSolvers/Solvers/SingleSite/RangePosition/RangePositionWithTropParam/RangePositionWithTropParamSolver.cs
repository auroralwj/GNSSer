//2017.10.20, czs, create in hongqing, 对流层模型改正伪距定位

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
    //包含4类参数，测站位置（x,y,z），钟差（Cdt），对流程天顶距延迟(zpd)和非整的整周模糊度（N）。

    /// <summary>
    /// 精密单点定位。电离层模型改正单频PPP
    /// 此处采用观测值残差向量计算。
    /// 条件：必须是双频观测，且观测卫星数量大于5个。
    /// 参考：Jan Kouba and Pierre Héroux. GPS Precise Point Positioning Using IGS Orbit Products[J].2000,sep
    /// </summary>
    public class RangePositionWithTropParamSolver : SingleSiteGnssSolver
    {
        #region 构造函数 
        
        /// <summary>
        /// 参数化对流层伪距定位
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public RangePositionWithTropParamSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "参数化对流层伪距定位";
            this.MatrixBuilder = new RangePositionWithTropParamMatrixBuilder(PositionOption);
        }

        #endregion


        public override SingleSiteGnssResult CaculateIndependent(EpochInformation material)
        {
            return Caculate(material);
        }


        public override SingleSiteGnssResult CaculateKalmanFilter(EpochInformation epochInfo, SingleSiteGnssResult lastResult)
        {


        //    return Caculate(epochInfo);

            return base.CaculateKalmanFilter(epochInfo, lastResult);

        }




        private SingleSiteGnssResult Caculate(EpochInformation epochInfo)
        {
            if (epochInfo.EnabledSatCount < Option.MinSatCount) { log.Error("不足5颗可用卫星：" + epochInfo.EnabledSatCount); return null; }

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
                if (this.IsUpdateEstimatePostition)
                {
                    epochInfo.SiteInfo.EstimatedXyz = result.EstimatedXyz;
                }

                // recInfo.ApproxXyz = result.EstimatedXyz;
                //log.Info(index + ", " + result.EstimatedXyz + ", " + result.EstimatedXyzRms);
                index++;
            } while (index < this.Option.MaxLoopCount && differ > 1e-6);//result.EstimatedXyzRms.Length > 1 && 

            CheckOrUpdateEstimatedCoord(epochInfo, result);

            return result;
        }



    }
}
