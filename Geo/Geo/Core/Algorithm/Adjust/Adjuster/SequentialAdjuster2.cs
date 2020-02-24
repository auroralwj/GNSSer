//2013.06.01, czs, creating
//2017.04.22,, czs, edit in hongqing, 重构， 实现Adjustment
//2017.07.19, czs, edit in hongqing, 采用通用矩阵类整理代码
//2018.03.24, czs, edit in hmx, 按照新架构重构
//2018.05.14, czs, edit in hmx, 加入状态转移信息，计算结果与Kalman滤波基本相同了

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 序贯平差 
    /// </summary>
    public class SequentialAdjuster : MatrixAdjuster
    {
        /// <summary>
        /// 序贯平差
        /// </summary>
        public SequentialAdjuster()
        {
        }
         
        /// <summary>
        /// 历次观测总数
        /// </summary>
        public int SumOfObsCount { get; set; }

        /// <summary>
        /// 计算
        /// </summary>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            this.ObsMatrix = input;

            //命名规则：0表示上一个（先验信息），1表示预测，无数字表示当次
            //上次次观测设置 
            var Qx0 = new Matrix(input.Apriori.InverseWeight);
            var Px0 =new Matrix( Qx0.GetInverse());
            var X0 = new Matrix((IMatrix)input.Apriori);

            //本次观测设置 
            var Qo = new Matrix(input.Observation.InverseWeight);
            var Po = new Matrix(Qo.GetInverse());
            var A = new Matrix(input.Coefficient);
            var AT = A.Trans;
            var L = new Matrix((IMatrix)input.Observation);
            int obsCount = L.RowCount;
            int paramCount = A.ColCount;

            //具有状态转移的序贯平差
            if (input.Transfer != null && input.InverseWeightOfTransfer != null)
            {
                Matrix Trans = new Matrix(input.Transfer.Array);           //状态转移矩阵
                Matrix TransT = Trans.Transpose();
                Matrix Q_m = new Matrix(input.InverseWeightOfTransfer.Array);    //状态转移模型噪声

                //计算参数预测值，可以看做序贯平差中的第一组数据
                //ArrayMatrix X1 = Trans * X0;
                //ArrayMatrix Qx1 = Trans * Qx0 * TransT + Q_m;
                //更新先验值
                X0 = Trans * X0;
                Qx0 = Trans * Qx0 * TransT + Q_m;
                Px0 = Qx0.Inversion;
            }
            var ATP = AT * Po;
            var N = ATP * A;
            var U = ATP * L;
            var Px = (N + Px0);
            var Qx = Px.Inversion;
            var X = Qx * (U + Px0 * X0);

             
            var Estimated = new WeightedVector(X, Qx) { ParamNames = input.ParamNames };

            SumOfObsCount += input.ObsCount;
            var Freedom = SumOfObsCount - input.ParamCount;// AprioriObsCount;     

            Matrix V = A * X - L;
            Matrix Vx = X - X0;
            Matrix VTPV = null;
            if (Po.IsDiagonal) { VTPV = new Matrix(AdjustmentUtil.ATPA(V, Po)) + (Vx.Trans * Px0 * Vx); }
            else { VTPV = V.Trans * Po * V + (Vx.Trans * Px0 * Vx); }

            var vtpv = VTPV[0, 0]; 
            this.SumOfObsCount += A.RowCount;
            var VarianceOfUnitWeight = Math.Abs(vtpv / (Freedom));

            AdjustResultMatrix result = new AdjustResultMatrix()
                .SetEstimated(Estimated)
                .SetFreedom(Freedom)
                .SetObsMatrix(input)
                .SetVarianceFactor(VarianceOfUnitWeight)
                .SetVtpv(vtpv);

            return result;
        }
         
    }
}