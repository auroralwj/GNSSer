//2018.03.24, czs, extract in HMX, 平差结果矩阵。  
//2018.06.07, czs, edit in hmx, 残差计算增加常量D
//2018.10.20, czs, create in hmx, 单独形成一个类，用于模糊度固定

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using System.Threading.Tasks;
using Geo.IO;


namespace Geo.Algorithm.Adjust
{ 
    /// <summary>
    /// 固定参数（模糊度）计算。
    /// </summary>
    public class FixedParamSolver
    {
        /// <summary>
        /// 模糊度固定默认构造函数
        /// </summary>
        /// <param name="IsFixParamByConditionOrHugeWeight"></param>
        public FixedParamSolver(bool IsFixParamByConditionOrHugeWeight)
        {
            this.IsFixParamByConditionOrHugeWeight = IsFixParamByConditionOrHugeWeight;

        }
        /// <summary>
        /// 固定模糊度算法，是：条件平差，否：大权参数平差。
        /// </summary>
        public bool IsFixParamByConditionOrHugeWeight { get; set; }
        /// <summary>
        /// 固定结果转换到浮点解（原始解）的系数阵。比较复杂的需要外部输入，默认为点对点。
        /// </summary>
        public Matrix CoeefOfFixedToFloat { get; set; }
        /// <summary>
        /// 外部输入系数阵
        /// </summary>
        public event Func<WeightedVector, WeightedVector, Matrix> CoeefOfFixedToFloatBuildEventHandler;

        /// <summary>
        /// 获取固定模糊度后的结果。默认参数直接固定，并通过名称关联。
        /// </summary>
        /// <param name="fixedAmbiguities"></param>
        /// <param name="originalVector"></param> 
        /// <returns></returns>
        public WeightedVector GetParamFixedResult(WeightedVector originalVector,  WeightedVector fixedAmbiguities)
        {
            WeightedVector result = null;
            if (IsFixParamByConditionOrHugeWeight)
            {
                result = GetResultByConditionAdjust(originalVector, fixedAmbiguities);
            }
            else
            {
                result = GetResultByWeighedParamAdjust(originalVector, fixedAmbiguities);
            }
            return result;
        }

        #region  大权参数平差
        /// <summary>
        /// 具有约束条件的参数平差的无限权解法解算模糊度固定。
        /// </summary>
        /// <param name="totalFloat"></param>
        /// <param name="fixedAmbiguities"></param>
        /// <returns></returns>
        public  WeightedVector GetResultByWeighedParamAdjust(WeightedVector totalFloat, WeightedVector fixedAmbiguities)
        {
            if (fixedAmbiguities.Count == 0) { return totalFloat; }

            int totalParamCount = totalFloat.Count; //待估参数个数  
            int fixedAmbiCount = fixedAmbiguities.Count;

            //平差系数阵
            var PA = new Matrix(totalFloat.Weights);
            var LA = new Matrix((IVector)totalFloat, true) { RowNames = totalFloat.ParamNames };
            var A = DiagonalMatrix.GetIdentity(totalParamCount);//A x = l
            var LB = new Matrix((IVector)fixedAmbiguities, true) { RowNames = fixedAmbiguities.ParamNames };
            var PB = new Matrix(fixedAmbiguities.Weights);

            Matrix B = BuildCoeefOfFixedToFloat(totalFloat, fixedAmbiguities);

            var BTBP = B.Trans * PB;

            //A 为单位阵，不需要计算
            var NA = PA; //AT * P * A
            var UA = PA * LA;//AT *　Ｐ　*　Ｌ
            UA.ColNames = new List<string>() { "Names" };
            NA.ColNames = totalFloat.ParamNames;
            UA.RowNames = totalFloat.ParamNames;
            NA.RowNames = totalFloat.ParamNames;


            var NB = BTBP * B;
            var UB = BTBP * LB;
            UB.ColNames = new List<string>() { "Names" };
            NB.ColNames = totalFloat.ParamNames;
            UB.RowNames = totalFloat.ParamNames;
            NB.RowNames = totalFloat.ParamNames;

            MatrixEquation neA = new MatrixEquation(NA, UA);
            MatrixEquation neB = new MatrixEquation(NB, UB);
            var eq = neA + neB;

            var result = eq.GetEstimated();

            //整体矩阵验证, 2018.10.20, czs, in hmx, 已经验证与整体平差一致，但是如果权太大如1e40，则可能出现舍入误差，而失真！！
            if (false)
            {
                Matrix AB = new Matrix(A.RowCount + B.RowCount, A.ColCount);
                AB.SetSub(A);
                AB.SetSub(B, A.ColCount);

                Matrix PAB = new Matrix(PA.RowCount + PB.RowCount);
                PAB.SetSub(PA);
                PAB.SetSub(PB, PA.RowCount, PA.ColCount);

                Vector LAB = new Vector(totalFloat);
                LAB.AddRange(fixedAmbiguities);

                ParamAdjuster paramAdjuster = new ParamAdjuster();
                var result2 = paramAdjuster.Run(new AdjustObsMatrix(new WeightedVector(LAB, PAB.Inversion), AB));
            }
            return result;
        }

        #endregion

        #region 条件平差法解算固定解
        /// <summary>
        /// 条件平差法解算固定解
        /// </summary>
        /// <param name="fixedAmbiguities">已经固定的模糊度</param>
        /// <param name="totalFloat">浮点解平差信息</param>
        /// <returns></returns>
        public  WeightedVector GetResultByConditionAdjust(WeightedVector totalFloat, WeightedVector fixedAmbiguities)
        {
            if (fixedAmbiguities.Count == 0) { return totalFloat; }
            
            //新增加约束条件的系数矩阵和权矩阵、观测值  
            Matrix coeefOfCondition = BuildCoeefOfFixedToFloat(totalFloat, fixedAmbiguities);

            WeightedVector NewEstimated = SolveAmbiFixedResultByConditionAdjust(totalFloat, fixedAmbiguities, coeefOfCondition);

            return NewEstimated;
        }

        /// <summary>
        /// 条件平差法解算固定解，将固定解当成虚拟观测量,对原浮点解进行约束，条件平差。
        /// </summary>
        /// <param name="coeffOfParam">系数阵,条件方程构造</param>
        /// <param name="totalFloat">原浮点解</param>
        /// <param name="fixedObs">已经固定的参数，固定解当成虚拟观测量</param>
        /// <returns></returns>
        public static WeightedVector SolveAmbiFixedResultByConditionAdjust(WeightedVector totalFloat, Vector fixedObs, IMatrix coeffOfParam)
        {
            //以下算法已经验证等价！！2018.09.02， czs, hmx
            bool isSong = false;
            WeightedVector NewEstimated = null;
            if (isSong)
            {
                #region 求固定解 宋力杰方法
                IMatrix X_old = totalFloat;
                IMatrix QX_old = totalFloat.InverseWeight;
                IMatrix coeffOfParamT = coeffOfParam.Transposition;

                IMatrix W = coeffOfParam.Multiply(X_old).Minus(new VectorMatrix(fixedObs));

                IMatrix tmp = coeffOfParam.Multiply(QX_old).Multiply(coeffOfParamT);

                IMatrix Nadd = (QX_old.Multiply(coeffOfParamT)).Multiply(tmp.GetInverse());

                IMatrix X_new = X_old.Minus(Nadd.Multiply(W));

                IMatrix tmp2 = Nadd.Multiply(coeffOfParam);
                IMatrix QX_new = QX_old.Minus(tmp2.Multiply(QX_old));

                NewEstimated = new WeightedVector(X_new, QX_new) { ParamNames = coeffOfParam.ColNames };
                #endregion
            }
            else
            {
                //条件平差
                AdjustObsMatrix obsMatrix = new AdjustObsMatrix();
                obsMatrix.SetCoefficient(coeffOfParam).SetObservation(totalFloat).SetFreeVector(fixedObs);
                ConditionalAdjuster adjuster = new ConditionalAdjuster();
                var resultMatrix = adjuster.Run(obsMatrix);

                NewEstimated = resultMatrix.CorrectedObs;
            }

            return NewEstimated;
        }
        #endregion

        /// <summary>
        /// 建立固定参数与浮点解转换的系数阵.
        /// 这是直接对等的做法。
        /// </summary>
        /// <param name="totalFloat"></param>
        /// <param name="fixedAmbiguities"></param>
        /// <returns></returns>
        private  Matrix BuildCoeefOfFixedToFloat(WeightedVector totalFloat, WeightedVector fixedAmbiguities)
        {
            //首先外部优先
            if(CoeefOfFixedToFloatBuildEventHandler != null)
            {
                return CoeefOfFixedToFloatBuildEventHandler(totalFloat, fixedAmbiguities);
            }

            //外部没有则，直接点对点
            int totalParamCount = totalFloat.Count; //待估参数个数  
            int fixedAmbiCount = fixedAmbiguities.Count;
            var coeefOfFixedToFloat = new Matrix(fixedAmbiCount, totalParamCount) { RowNames = fixedAmbiguities.ParamNames };
            //constraints to fixed ambiguities  
            for (int i = 0; i < fixedAmbiCount; i++)
            {
                //对所有的参数
                var name = fixedAmbiguities.ParamNames[i];
                int j = totalFloat.ParamNames.IndexOf(name);

                coeefOfFixedToFloat[i, j] = 1;
            }

            return coeefOfFixedToFloat;
        }
    }
}