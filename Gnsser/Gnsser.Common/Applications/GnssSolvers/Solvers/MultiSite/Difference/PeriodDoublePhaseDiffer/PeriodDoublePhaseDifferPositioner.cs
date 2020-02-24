//2012.05.29, czs, creare, 单差,载波差分定位。许其凤,164-167 
//2014.12.10, czs, edit in jinxinliaomao shaungliao, 基线载波相位差分，双差
//2016.04.26, czs, edit in hongqing, 重构
//2018.07.31, czs, edit in hmx, 名称前冠名Period

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
using Geo.IO;
using Geo.Algorithm;

namespace Gnsser.Service
{
    /// <summary>
    ///  载波相位差分，双差，包含 2 类参数：流动站坐标改正数（x,y,z）和模糊度差分。
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站)
    /// </summary>
    public class PeriodDoublePhaseDifferPositioner : AbstracMultiSitePeriodPositioner
    {
        #region 构造函数
        /// <summary>
        /// 载波相位差分，双差，包含 2 类参数：流动站坐标改正数（x,y,z）和模糊度差分。
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public PeriodDoublePhaseDifferPositioner(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
            : base(DataSourceContext, GnssOption)
        {
            //InitDetect(); 
            this.IsBaseSatelliteRequried = true;//强制启用基准星
            this.AmbiguityManager = new AmbiguityManager(GnssOption); //每个历元固定一次
            this.Name = "载波相位双差";
            this.BaseParamCount = 3;
            this.MatrixBuilder = new PeriodDoubleDifferMatrixBuilder(GnssOption);

        }
         
        #endregion  

        /// <summary>
        /// 模糊度管理器
        /// </summary>
        public AmbiguityManager AmbiguityManager { get; set; }

        #region 平差计算

        public override BaseGnssResult CaculateIndependent(MultiSitePeriodInfo epochInfos)
        {
            if (!this.MatrixBuilder.IsAdjustable) { log.Warn("不适合平差！" + MatrixBuilder.Message); return null; }

            this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));
            AdjustResultMatrix lastFloat = this.Adjustment;
            return BuildResult();
        }


        public override void BuildAdjustMatrix()
        {
            //--------------------计算前-模糊度处理------------------------- 
            //标记新出现的卫星具有周跳，要使多历元模糊度一致。
            if (this.CurrentProduct != null)
            {
                var differ = SatelliteNumberUtils.GetNews(this.CurrentMaterial.EnabledPrns, this.CurrentProduct.EnabledPrns);
                foreach (var item in differ)
                {
                    if (this.CurrentMaterial.EnabledPrns.Contains(item) && this.CurrentMaterial.Last.Last.EnabledPrns.Contains(item))
                    {
                        this.CurrentMaterial.Last.Last[item].SetCycleSlip(true);
                    }
                }
            }
            base.BuildAdjustMatrix();
        }
        /// <summary>
        /// 滤波算法。
        /// </summary>
        /// <param name="epochInfos"></param>
        /// <param name="lastResult"></param>
        /// <returns></returns>
        public override BaseGnssResult CaculateKalmanFilter(MultiSitePeriodInfo epochInfos, BaseGnssResult lastResult = null)
        {
            var result = base.CaculateKalmanFilter(epochInfos, lastResult) as PeriodDoubleDifferPositionResult;


            //模糊度固定解
            //if (Option.IsFixingAmbiguity && result.EstimatedXyzRms.Norm < 0.01)
            //{
            //    Vector fixedIntAmbiCycles = null;
            //    if (!TryFixeAmbiguites(result, out fixedIntAmbiCycles))
            //    {
            //        return result;
            //    }

            //    //固定成功，则将浮点解作为虚拟观测值，整数解作为约束进行条件平差
            //    //保存到结果，用于输出
            //    result.FixedIntAmbiguities = fixedIntAmbiCycles;
            //    var lastDdResult = lastResult as PeriodDoubleDifferPositionResult;
            //    if (lastDdResult == null)
            //    {
            //        return result;
            //    }
            //    //恢复模糊度为米,计算固定解
            //    var fixedAmbiMeters = ConvertCycleAmbiguityToMeter(fixedIntAmbiCycles);
            //    var prevFixedAmbiMeters = ConvertCycleAmbiguityToMeter(lastDdResult.FixedIntAmbiguities);
            //    WeightedVector NewEstimated = Adjustment.SolveAmbiFixedResult(fixedAmbiMeters, prevFixedAmbiMeters);

            //    result.ResultMatrix.Estimated = NewEstimated;
            //}

            //this.CurrentProduct = result;

            return result;
        }
        /// <summary>
        /// Kalmam滤波。
        /// </summary>
        /// <param name="epochInfos">接收信息</param> 
        /// <param name="lastResult">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public  BaseGnssResult CaculateKalmanFilter2(MultiSitePeriodInfo epochInfos, BaseGnssResult lastResult = null)
        {
            var result = base.CaculateKalmanFilter(epochInfos, lastResult) as PeriodDoubleDifferPositionResult;
            
            //模糊度固定解
            if (this.IsFixingAmbiguity)
            {
                AmbiguityManager.Regist(result); //用于存储和输出模糊度。

                var floatSolMetter = this.Adjustment.Estimated;
                //尝试固定模糊度
                var fixedIntAmbiCycles = FixePhaseAmbiguity(result); 
                
                //模糊度固定失败，直接返回浮点数结果。
                if (fixedIntAmbiCycles.Count == 0)
                {
                    return result;
                }

                //固定成功，则将浮点解作为虚拟观测值，整数解作为约束进行条件平差

                //保存到结果，用于输出
                result.FixedParams = fixedIntAmbiCycles;
                //恢复模糊度为米
                var fixedAmbiMeters = ConvertCycleAmbiguityToMeter(fixedIntAmbiCycles);

                //组建条件数的系数阵， 
                var coeffOfConstrant = BuildCoeffiientMatrix(floatSolMetter.ParamNames, fixedAmbiMeters.ParamNames);

                //条件平差，观测值为浮点解，上一观测结果作为虚拟观测值，
                var obsVector = floatSolMetter;
                var ca = new ConditionalAdjustment(obsVector, coeffOfConstrant, (Vector)(fixedAmbiMeters * (-1.0))); //此处B0为加，所以要乘以-1
                ca.Process();

                var fixedDiffer = ca.CorrectedObservation;
                //避免方差太小，但是好像没有什么用处。
                for (int j = 0; j < floatSolMetter.Count; j++)
                {
                    if (fixedDiffer.InverseWeight[j, j] < 1e-6)
                    {
                        fixedDiffer.InverseWeight[j, j] = 1e-6;
                    }
                }


                //更新结果
                //只更新坐标
                //floatSolMetter[Gnsser.ParamNames.DeltaX] = fixedDiffer[Gnsser.ParamNames.DeltaX];
                //floatSolMetter[Gnsser.ParamNames.DeltaY] = fixedDiffer[Gnsser.ParamNames.DeltaY];
                //floatSolMetter[Gnsser.ParamNames.DeltaZ] = fixedDiffer[Gnsser.ParamNames.DeltaZ];
                result.ResultMatrix.Estimated = fixedDiffer;

                //result.Adjustment.Estimated = estimated;

                //from cuiyang to check
                IMatrix B = coeffOfConstrant;
                IMatrix BT = B.Transposition;
                IMatrix L = floatSolMetter;
                IMatrix QL = floatSolMetter.InverseWeight;
                IMatrix B0 = new VectorMatrix(fixedAmbiMeters * (-1));//条件方程常数项
                IMatrix W = B.Multiply(L).Plus(B0); // W = BL - B0
                IMatrix BQBT = B.Multiply(QL).Multiply(BT);
                IMatrix Nadd = (QL.Multiply(BT)).Multiply(BQBT.GetInverse());
                IMatrix X_new = L.Minus(Nadd.Multiply(W));
                IMatrix QX_new = QL.Minus(Nadd.Multiply(B).Multiply(QL));
                var check = new WeightedVector(X_new, QX_new);
                int ii = 0;
                //result.Adjustment.Estimated = new WeightedVector(X_new, QX_new); 
            }

            this.SetProduct( result);
            
            return result;
        }

        /// <summary>
        /// 构建条件方程矩阵
        /// </summary>
        /// <param name="obsParamNames"></param>
        /// <param name="fixedParamNames"></param>
        /// <returns></returns>
        private  ArrayMatrix BuildCoeffiientMatrix(List<string> obsParamNames, List<string> fixedParamNames)
        {
            var paramCount = obsParamNames.Count;
            var conditionCount = fixedParamNames.Count;
            var coeffOfConstrant = new ArrayMatrix(conditionCount, paramCount);

            int i = 0;
            foreach (var name in fixedParamNames)
            {
                //行为条件数,即参数个数，列应该从第三列后开始（忽略3个坐标观测值），此处与浮点解下标相同
                int index = obsParamNames.IndexOf(name);
                coeffOfConstrant[i, index] = 1;
                i++;
            }
            return coeffOfConstrant;
        } 

        /// <summary>
        /// 结果,固定解
        /// </summary>
        /// <returns></returns>
        public override  BaseGnssResult BuildResult()
        {            
            return new PeriodDoubleDifferPositionResult(this.CurrentMaterial, Adjustment, this.CurrentBasePrn, Option, this.MatrixBuilder.GnssParamNameBuilder);
        }

       
        #endregion
    }
}
