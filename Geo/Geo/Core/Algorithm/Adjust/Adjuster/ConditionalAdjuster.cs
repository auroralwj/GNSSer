//2012, czs, Create, 条件平差
//2016.10.10, czs, refactor in hongqing, 重构
//2016.10.25, czs, edit in hongqing 实验室509机房， 自由项为 B0
//2018.04.07, czs, edit in hmx, 按照平差器重新设计

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;


namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 条件平差。B L - B0 = 0
    /// 条件平差的参数就是观测值，即直接对参数进行观测。
    /// 取全部观测量的最或然值为未知数，建立这些未知数之间应满足的几何条件（条件方程），
    /// 然后依原则求满足条件方程的最或然值，并估计精度。 
    /// 1.条件方程式的个数等于多余观测数 r ：r = n - t
    /// 2.个条件方程要求相互独立，即其中的任一条件方程都不能由其余的条件方程推出。
    /// 函数模型：B L - B0 = 0
    /// 
    ///条件方程：B V - W = 0
    ///  W = -（BL - B0）
    ///  
    /// 
    /// 平差方法的选择
    /// 1．手算时代：当 t>r 时，用条件平差，当 r>t 时，用参数平差。
    /// 2．电算时代：对于大规模网的平差一般采用参数平差法。
    /// 对于小的测边网，工程网多采用条件平差法。
    /// </summary>
    public class ConditionalAdjuster : MatrixAdjuster
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConditionalAdjuster()
        {
        }

        #endregion

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            //原始输入
            Matrix B = new Matrix(input.Coefficient);
            Matrix L = new Matrix((IMatrix)input.Observation);
            Matrix QL = new Matrix((IMatrix)input.Observation.InverseWeight);
            Matrix B0 = input.HasFreeVector ? new Matrix(input.FreeVector, true) : null;//B0

            Matrix PL = QL.Inversion;
            int freedom = B.RowCount;
            Matrix BT = B.Trans;

            int obsCount = L.RowCount;
            int paramCount =0; 


            Matrix W = -(B * L - B0);
            Matrix N = B * QL * BT;
            Matrix inverN = N.Inversion;
            Matrix K = inverN * W;
            Matrix Vhat = (QL * BT * K);
            Matrix Qvhat = QL * BT * inverN * B * QL;
            WeightedVector estLW = new WeightedVector(Vhat, Qvhat) { ParamNames = input.Observation.ParamNames };

            Matrix Lhat = L + Vhat;
            Matrix QhatL = QL - Qvhat;
            WeightedVector correctedObs = new WeightedVector(Lhat, QhatL) { ParamNames = input.Observation.ParamNames };

            double vtpv = (Vhat.Trans * PL * Vhat).FirstValue;
            double s0 = vtpv / freedom;//单位权中误差估值

            if (!DoubleUtil.IsValid(s0))
            {
                log.Error("方差值无效！" + s0);
            }

            AdjustResultMatrix result = new AdjustResultMatrix()
                .SetAdjustmentType(AdjustmentType.条件平差)
                .SetEstimated(estLW)
                .SetCorrectedObs(correctedObs)
                .SetObsMatrix(input)
                .SetFreedom(freedom)
                .SetVarianceFactor(s0)
                .SetVtpv(vtpv);

            return result;
        }
    }
}