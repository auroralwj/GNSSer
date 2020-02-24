//2016.08.18, czs, create in 黄山市休宁县, 一维滤波矩阵构造器
//2016.10.17, czs, edit in hongqing, 增加观测值权值

using System;
using System.Collections.Generic; 
using System.Text; 
using Geo.Algorithm;
using Geo.Algorithm.Adjust; 
using Geo.Utils;
using Geo;
using Gnsser;

namespace Geo.Algorithm.Adjust
{

    /// <summary>
    /// 一维滤波矩阵构造器。
    /// </summary>
    public class OneDimAdjustMatrixBuilder : BaseAdjustMatrixBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="PrevAdjustment"></param>
        public OneDimAdjustMatrixBuilder(RmsedNumeral obs, AdjustResultMatrix PrevAdjustment = null)
        {
            this.PrevAdjustment = PrevAdjustment;
            this.ObsValue = obs;
            this.ParamNames = new List<string>() { "Value" };
        }

        /// <summary>
        /// 一维滤波矩阵构造器
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="PrevAdjustment"></param>
        public OneDimAdjustMatrixBuilder(double obs, AdjustResultMatrix PrevAdjustment = null, double rmsOrStdDev = 1)
            : this(new RmsedNumeral(obs, rmsOrStdDev))
        {
        }

        /// <summary>
        /// 观测值
        /// </summary>
        public RmsedNumeral ObsValue { get; set; }
        /// <summary>
        /// 上一个平差器
        /// </summary>
        public AdjustResultMatrix PrevAdjustment { get; set; }

        /// <summary>
        /// 先验值
        /// </summary>
        public override WeightedVector AprioriParam
        {
            get
            {
                if (PrevAdjustment != null) return PrevAdjustment.Estimated;
                else return BuildObservation();
            }
        }

        /// <summary>
        /// 一维变量，永不改变
        /// </summary>
        public override bool IsParamsChanged
        {
            get { return false; }
        }
        /// <summary>
        /// 协方差阵
        /// </summary>
        public override Matrix Coefficient
        {
            get { return new Matrix( ArrayMatrix.Diagonal(1, 1, 1)); }
        }
        /// <summary>
        /// 观测残差
        /// </summary>
        public override WeightedVector Observation
        {
            get { return BuildObservation(); }
        }

        private WeightedVector BuildObservation()
        {
            var cova = MatrixUtil.CreateDiagonal(1, ObsValue.Variance);
            return new WeightedVector(new double[] { ObsValue.Value }, cova);
        }

        /// <summary>
        /// 创建状态转移矩阵
        /// </summary>  
        public override WeightedMatrix Transfer
        {
            get
            {
                var transMatrix = DiagonalMatrix.GetIdentity(ParamCount);
                transMatrix.ColNames = this.ParamNames;
                transMatrix.RowNames = this.ParamNames;
                return new WeightedMatrix(transMatrix, new ZeroMatrix(ParamCount));
            }
        }
    }

}