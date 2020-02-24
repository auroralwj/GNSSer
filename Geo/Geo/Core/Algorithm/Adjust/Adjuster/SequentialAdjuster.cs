//2013.06.01, czs, creating
//2017.04.22,, czs, edit in hongqing, 重构， 实现Adjustment
//2017.07.19, czs, edit in hongqing, 采用通用矩阵类整理代码
//2018.03.24, czs, edit in hmx, 按照新架构重构

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
    public class SequentialAdjuster0 : MatrixAdjuster
    {
        /// <summary>
        /// 序贯平差
        /// </summary>
        public SequentialAdjuster0()
        {
        }
        
        /// <summary>
        /// 历次VTPV值
        /// </summary>
        public double SumOfVptv { get; set; }
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
            //if (input.Transfer != null && input.InverseWeightOfTransfer!=null)
            //{
            //    Matrix Trans = new Matrix(input.Transfer.Array);           //状态转移矩阵
            //    Matrix TransT = Trans.Transpose();
            //    Matrix Q_m = new Matrix(input.InverseWeightOfTransfer.Array);    //状态转移模型噪声

            //    //计算参数预测值，可以看做序贯平差中的第一组数据
            //    //ArrayMatrix X1 = Trans * X0;
            //    //ArrayMatrix Qx1 = Trans * Qx0 * TransT + Q_m;
            //    X0 = Trans * X0;
            //    Qx0 = Trans * Qx0 * TransT + Q_m;
            //    Px0 = Qx0.Inversion;
            //}




            //1.预测残差
            //计算预测残差
            var V1 = L - A * X0;//观测值 - 估计近似值
            var Qv1 = Qo + A * Qx0 * AT; //预测残差方差
            //2.计算增益矩阵
            var J = Qx0 * AT * Qv1.Inversion;// 增益矩阵 
            //3.平差结果
            var dX = J * V1;//参数改正
            var X = X0 + dX;        

            //4.精度评定
            var Qx = Qx0 - J * A * Qx0;
            var Estimated = new WeightedVector(X, Qx) { ParamNames = input.ParamNames };

            SumOfObsCount += input.ObsCount;
            var Freedom = SumOfObsCount - input.ParamCount;// AprioriObsCount;     
          //  var V = A * dX - V1;//估值-观测值 V = A * X - L = A * (X0 + deltaX) - (l + A * X0) =  A * deltaX - l.
          //  this.VarianceOfUnitWeight = (V.Trans * Po * V).FirstValue / Freedom;//单位权方差

            Matrix V = A * X - L;
            Matrix Vx = X - X0;
            Matrix VTPV = null;
            if (Po.IsDiagonal) { VTPV = new Matrix(AdjustmentUtil.ATPA(V, Po)) + (Vx.Trans * Px0 * Vx); }
            else { VTPV = V.Trans * Po * V + (Vx.Trans * Px0 * Vx); }

            var vtpv = VTPV[0, 0];
            this.SumOfVptv += vtpv;
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