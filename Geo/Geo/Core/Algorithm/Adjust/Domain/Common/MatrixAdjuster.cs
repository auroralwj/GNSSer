//2018.03.24, czs, create in HMX, 平差器。  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common; 

using System.Threading.Tasks;
using Geo.IO;
using Geo;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    ///平差器。 平差结果矩阵。 下标 0 代表先验值， 无代表估计值，1 代表预报值。
    /// </summary>
    public abstract class MatrixAdjuster<TObsMatrix, TResultMatrix> : AbstractProcess<TObsMatrix, TResultMatrix>
        where TObsMatrix : AdjustObsMatrix
        where TResultMatrix : AdjustResultMatrix
    {
        protected new ILog log = Log.GetLog(typeof(MatrixAdjuster<TObsMatrix, TResultMatrix>));

        /// <summary>
        /// 平差。构造函数。
        /// </summary>
        public MatrixAdjuster() { }
        
        /// <summary>
        /// 输入参数的引用
        /// </summary>
        public TObsMatrix ObsMatrix {get;set;}


        #region BQBT 计算
        /// <summary>
        /// 实用方法，Q为对称阵,速度较慢？摘抄自 宋力杰测量平差程序设计 P11
        /// </summary>
        /// <param name="B"></param>
        /// <param name="Q">可能非对称</param>
        /// <returns></returns>
        public static IMatrix BQBT(IMatrix B, IMatrix Q) { return AdjustmentUtil.BQBT(B, Q); }

        #endregion BQBT

        #region ATPA 计算
        /// <summary>
        /// 快捷计算方法，返回 SymmetricMatrix
        /// </summary>
        /// <param name="A">系数阵</param>
        /// <param name="P">对角阵</param>
        /// <returns></returns>
        public static Matrix ATPA(IMatrix A, IMatrix P) { return new Matrix(AdjustmentUtil.ATPA(A, P)); }

        #endregion
    }
    /// <summary>
    ///平差器。 平差结果矩阵。 下标 0 代表先验值， 无代表估计值，1 代表预报值。
    /// </summary>
    public abstract class MatrixAdjuster : MatrixAdjuster<AdjustObsMatrix, AdjustResultMatrix> 
    {
        protected new ILog log = Log.GetLog(typeof(MatrixAdjuster));

        /// <summary>
        /// 平差。构造函数。
        /// </summary>
        public MatrixAdjuster() { }


        /// <summary>
        /// 直接输入构造器
        /// </summary>
        /// <param name="matrixBuilder"></param>
        /// <returns></returns>
        public AdjustResultMatrix Run(BaseAdjustMatrixBuilder matrixBuilder)
        {
           this.ObsMatrix = new AdjustObsMatrix(matrixBuilder);
           return  Run(ObsMatrix);
        }

    }
}