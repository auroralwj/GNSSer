//2017.8.1, cy, create in chongqing, 双频相位载波双差计算器


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
using Geo.Algorithm;

namespace Gnsser.Service
{ 
    /// <summary>
    ///  单历元相位双差
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站)
    /// </summary>
    public class EpochDouFreDoubleDifferPositioner : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public EpochDouFreDoubleDifferPositioner(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        :base( DataSourceContext,   GnssOption)
        { 
            //双差模糊度固定
            this.DoubleIonFreeAmReslution = new DoubleIonFreeAmbiguityReslution(); 
            this.ResidualsAnalysis = new ResidualsAnalysis();
            this.DualBandCycleSlipDetector = new DualBandCycleSlipDetector();
            this.Name = "单历元双频相位双差";


            double BaselineLength = (this.DataSourceContext.ObservationDataSources.BaseDataSource.SiteInfo.ApproxXyz
                                  - this.DataSourceContext.ObservationDataSources.OtherDataSource.SiteInfo.ApproxXyz).Length;
           
            //默认双差基础参数为5个（3个坐标坐标+基准站的对流层参数+流动站的对流层参数）
            this.BaseParamCount = 5;
            if (BaselineLength <= 8000)
            { 
                this.BaseParamCount = 3;
            
            }
            else if (BaselineLength <= 8000 && BaselineLength > 800)
            {
                this.BaseParamCount = 4;

            }
           

            this.IsBaseSatelliteRequried = true;//强制启用基准星 
            this.MatrixBuilder = new EpochDouFreDoubleDifferMatrixBuilder(Option,this.BaseParamCount);
            AmbiguityManager = new AmbiguityManager(Option);
        }   

          
         
        #endregion         

        #region 属性  
         
        /// <summary>
        /// 双差模糊度固定
        /// </summary>
        public DoubleIonFreeAmbiguityReslution DoubleIonFreeAmReslution { get; set; }

        /// <summary>
        /// 残差分析
        /// </summary>
        public ResidualsAnalysis ResidualsAnalysis { get; set; }

        /// <summary>
        /// 双频周跳探测
        /// </summary>
        public DualBandCycleSlipDetector DualBandCycleSlipDetector { get; set; }
        /// <summary>
        /// 模糊度管理器
        /// </summary>
        public AmbiguityManager AmbiguityManager { get; set; }

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
            var result = base.CaculateKalmanFilter(epochInfos, lastResult) as EpochDouFreDoubleDifferPositionResult;

            //模糊度固定解
            if (this.IsFixingAmbiguity)// && result.EstimatedXyzRms.Norm < 0.001)
            {
                WeightedVector fixedIntAmbiCycles = null;
                if (!TryFixeAmbiguites(result, out fixedIntAmbiCycles))
                {
                    return result;
                }

                //固定成功，则将浮点解作为虚拟观测值，整数解作为约束进行条件平差
                //保存到结果，用于输出
                result.FixedIntAmbiguities = fixedIntAmbiCycles;

                var lastDdResult = lastResult as EpochDouFreDoubleDifferPositionResult;
                if (lastDdResult == null)
                {
                    return result;
                }
                ////恢复模糊度为米,计算固定解
                //var fixedAmbiMeters = ConvertCycleAmbiguityToMeter(fixedIntAmbiCycles);
                //var prevfixedAmbiMeters = ConvertCycleAmbiguityToMeter( lastDdResult.FixedIntAmbiguities);
                //WeightedVector NewEstimated = Adjustment.SolveAmbiFixedResult(fixedAmbiMeters, prevfixedAmbiMeters);

                WeightedVector NewEstimated = Adjustment.SolveAmbiFixedResult(fixedIntAmbiCycles, lastDdResult.FixedIntAmbiguities, Option.IsFixParamByConditionOrHugeWeight);

                //

                int nx = result.ResultMatrix.Estimated.Count; //参数个数  n*1
                // int nx = 5; //参数个数  n*1
                //set solution
                for (int i = 0; i < nx; i++)
                {
                    result.ResultMatrix.Estimated[i] = NewEstimated[i];
                    //for (j = 0; j < nx; j++)
                    //{
                    //    if (i == j)
                    //    {
                    //        if (Estimated.InverseWeight[i, j] == 0.0 && i >= 5)  //如果取0，后续无法直接求逆
                    //        {
                    //            adjustment.Estimated.InverseWeight[i, i] = 1e-14;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        adjustment.Estimated.InverseWeight[i, j] = Estimated.InverseWeight[i, j];
                    //    }
                    //}
                }
                //for (int i = 0; i < fixedIntAmbiCycles.Count; i++)
                //{
                //    //对所有的参数
                //    var name = fixedIntAmbiCycles.ParamNames[i];
                //    int j = result.Adjustment.ParamNames.IndexOf(name);

                //    //此处应该判断，如果是第一次出现，
                //    //第二次出现且和以往的模糊度不冲突的情况下再固定模糊度。 
                //    //增加高权
                //    //result.Adjustment.Estimated.InverseWeight.SetColValue(j, 0);
                //    //result.Adjustment.Estimated.InverseWeight.SetRowValue(j, 0);
                //    result.Adjustment.Estimated.InverseWeight[j, j] = 1e-20;
                //}

                // result.Adjustment.Estimated = NewEstimated;
            }

            this.SetProduct(result);

            return result;
        }

       

        #endregion

        
        #endregion 

        
        public override BaseGnssResult BuildResult()
        {
            var result = new EpochDouFreDoubleDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn, BaseParamCount) { DifferPositionOption = this.Option };
            return result;
        }
    }
}
