//2014.10.06, czs, create in hailutu, 差分定位
//2014.12.07, czs, edit in jinxinliaomao shaungliao, 名称改为 PppResidualDifferPositioner
//2015.05.17, cy,  修复bug，添加两个"历元信息构建器",分别为基准站和流动站的。
//2016.04.24, czs, edit in hongqing, 继承类重构，自 MultiSiteGnssSolver
//2018.12.10, cy, edit in chongqing, 调通无电离层组合
//2018.12.29, czs, edit in hmx, 重新设计实现模糊度固定

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
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    ///  无电离层双差，单历元解算。
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站)
    /// </summary>
    public class IonFreeDoubleDifferPositioner : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public IonFreeDoubleDifferPositioner(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        : base(DataSourceContext, GnssOption)
        {
            //双差模糊度固定
            this.DoubleIonFreeAmReslution = new DoubleIonFreeAmbiguityReslution();
            this.ResidualsAnalysis = new ResidualsAnalysis();
            this.DualBandCycleSlipDetector = new DualBandCycleSlipDetector();
            this.Name = "无电离层组合双差";
            this.BaseParamCount = 5;

            this.IsBaseSatelliteRequried = true;//强制启用基准星

            var distanceOfBaseline = (DataSourceContext.ObservationDataSources.BaseDataSource.SiteInfo.ApproxXyz - DataSourceContext.ObservationDataSources.OtherDataSource.SiteInfo.ApproxXyz).Length;
            if (distanceOfBaseline <= GnssOption.MaxDistanceOfShortBaseLine)
            {
                this.BaseParamCount = 3;
            }
            else if (GnssOption.MaxDistanceOfShortBaseLine < distanceOfBaseline && distanceOfBaseline < GnssOption.MinDistanceOfLongBaseLine)
            {
                this.BaseParamCount = 4;
            }
            else
            {
                this.BaseParamCount = 5;
            }

            this.Option.IsBaseSatelliteRequried = true;//强制启用基准星 
            this.MatrixBuilder = new IonFreeDoubleDifferMatrixBuilder(Option, this.BaseParamCount);

            //双差模糊度固定
            this.DoubleIonFreeAmReslution = new DoubleIonFreeAmbiguityReslution(this.BaseParamCount);
            this.ResidualsAnalysis = new ResidualsAnalysis();
        }

        #endregion

        #region 属性  

        /// <summary>
        /// 双差模糊度固定
        /// </summary>
        public DoubleIonFreeAmbiguityReslution DoubleIonFreeAmReslution { get; set; }

        /// <summary>
        /// 残差分析,判断是否有未发现的周跳或粗差
        /// </summary>
        public ResidualsAnalysis ResidualsAnalysis { get; set; }

        /// <summary>
        /// 双频周跳探测
        /// </summary>
        public DualBandCycleSlipDetector DualBandCycleSlipDetector { get; set; }


        static object locker = new object();

        #endregion         

        #region 核心计算方法
        /// <summary>
        /// 构建矩阵
        /// </summary>
        public override void BuildAdjustMatrix()
        {
            var builder = (IonFreeDoubleDifferMatrixBuilder)MatrixBuilder;
            builder.SetMaterial(this.CurrentMaterial).SetPreviousProduct(this.CurrentProduct);
            builder.BaseParamCount = BaseParamCount;
            builder.SetBasePrn(this.CurrentBasePrn);
            builder.Build();
        }


        #region 卡尔曼滤波
        /// <summary>
        /// PPP 计算核心方法。 Kalmam滤波。
        /// 观测量的顺序是先伪距观测量，后载波观测量，观测量的总数为卫星数量的两倍。
        /// 参数数量为卫星数量加5,卫星数量对应各个模糊度，5为3个位置量xyz，1个接收机钟差量，1个对流程湿分量。
        /// </summary>
        /// <param name="mSiteEpochInfo">接收信息</param> 
        /// <param name="lastResult">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public override BaseGnssResult CaculateKalmanFilter(MultiSiteEpochInfo mSiteEpochInfo, BaseGnssResult lastResult = null)
        {
            var refEpoch = mSiteEpochInfo.BaseEpochInfo;
            var rovEpoch = mSiteEpochInfo.OtherEpochInfo;

            IonFreeDoubleDifferPositionResult last = null;
            if (lastResult != null) last = (IonFreeDoubleDifferPositionResult)lastResult;

            //双差必须观测到2颗以上卫星，否则返回空
            if (refEpoch.EnabledPrns.Count < 2 && rovEpoch.EnabledPrns.Count < 2)
            {
                log.Warn("双差必须观测到2颗以上卫星，返回空");
                return null;
            }

            this.Adjustment = this.RunAdjuster(  BuildAdjustObsMatrix(mSiteEpochInfo));

            if (Adjustment.Estimated == null)
            {
                return null;
            }
            this.Adjustment.ResultType = ResultType.Float;


            //检查并尝试固定模糊度

            if (true)//默认算法
            {
                var theResult = BuildResult();
                theResult.ResultMatrix.ResultType = ResultType.Float;

                //检查并尝试固定模糊度
                theResult = this.CheckOrTryToGetAmbiguityFixedResult(theResult);

                return theResult;
            }

            //-----cuiyang 算法 --2018.12.27-------------模糊度固定-----------------------------
            if (false && Option.IsFixingAmbiguity)
            {
                //尝试固定模糊度
                lock (locker)
                {
                    //尝试固定模糊度
                    var fixIonFreeAmbiCycles = DoubleIonFreeAmReslution.Process(rovEpoch, refEpoch, Adjustment, CurrentBasePrn);
                    //是否保存固定的双差LC模糊度信息，继承已固定的？ 建议不用， 始终动态更新      
                    // 对过去已固定，但当前历元却没有固定，是否继承下来？根据是否发生基准星变化、周跳等信息判断
                    WeightedVector preFixedVec = new WeightedVector(); //上一次固定结果，这里好像没有用，//2018.07.31， czs

                    //作为约束条件，重新进行平差计算
                    if (fixIonFreeAmbiCycles != null && fixIonFreeAmbiCycles.Count > 0)//
                    {
                        fixIonFreeAmbiCycles.InverseWeight = new Matrix(new DiagonalMatrix(fixIonFreeAmbiCycles.Count, 1e-10));

                        WeightedVector NewEstimated = Adjustment.SolveAmbiFixedResult(fixIonFreeAmbiCycles, preFixedVec, Option.IsFixParamByConditionOrHugeWeight);

                        XYZ newXYZ = new XYZ(NewEstimated[0], NewEstimated[1], NewEstimated[2]);
                        XYZ oldXYZ = new XYZ(Adjustment.Estimated[0], Adjustment.Estimated[1], Adjustment.Estimated[2]);
                        //模糊度固定错误的判断准则（参考李盼论文）
                        if ((newXYZ - oldXYZ).Length > 0.05 && newXYZ.Length / oldXYZ.Length > 1.5)
                        {
                            //模糊度固定失败,则不用
                        }
                        else
                        {
                            int nx = Adjustment.Estimated.Count;
                            //nx = 3;
                            for (int i = 0; i < nx; i++)
                            {
                                Adjustment.Estimated[i] = NewEstimated[i];
                                // for (int j = 0; j < nx ; j++) adjustment.Estimated.InverseWeight[i, j] = Estimated.InverseWeight[i, j];
                            }
                            this.Adjustment.ResultType = ResultType.Fixed;
                        }
                    }
                }
            }

            if (Adjustment.Estimated != null)
            {
                var result = BuildResult();
                this.SetProduct(result);
                //是否更新测站估值
                this.CheckOrUpdateEstimatedCoord(mSiteEpochInfo, result);
                return result;
            }
            else
                return null;
        }
        #endregion
         

        #endregion 

        /// <summary>
        /// 默认采用Lambda算法直接固定。
        /// 如果是无电离层组合，则需要分别对待，不能直接固定，需要子类进行实现，//2018.11.06，czs， hmx
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <returns></returns>
        protected override WeightedVector DoFixAmbiguity(WeightedVector rawFloatAmbiCycles)
        {
           return DoFixIonoFreeDoubleDifferAmbiguity(rawFloatAmbiCycles, false); 
        }   
        /// <summary>
        /// 结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new IonFreeDoubleDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn, BaseParamCount);
            result.Option = this.Option; 
            return result;
        }
         
  
    }     
}
