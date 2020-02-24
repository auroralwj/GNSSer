//2012, czs, Create, 条件平差
//2016.10.10, czs, refactor in hongqing, 重构
//2016.10.25, czs, edit in hongqing 实验室509机房， 自由项为 B0
//2018.04.07, czs, edit in hmx, 按照平差器重新设计
//2018.04.09, czs, edit in hmx, 修改为具有参数的条件平差

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;


namespace Geo.Algorithm.Adjust
{
    /// <summary>
    ///具有参数的条件平差
    /// </summary>
    public class ConditionalAdjusterWithParam : MatrixAdjuster
    {
        #region 构造函数
        public ConditionalAdjusterWithParam()
        {
        }

        #endregion


        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            //u(0<u<t)个独立量为参数,多余观测数r=n-t
            //原始输入 m=r+u
            Matrix B = input.Coefficient;//m×n
            Matrix A = input.SecondCoefficient; //m×t
            Matrix L = new Matrix((IMatrix)input.Observation);//n×1
            Matrix QL = new Matrix((IMatrix)input.Observation.InverseWeight);//m×m
            Matrix X0 = input.HasApprox ? new Matrix(input.ApproxVector, true) : null;//u×1
            Matrix B0 = input.HasFreeVector ? new Matrix(input.FreeVector, true) : null;//B0//m×1

            Matrix AT = A.Trans;
            Matrix PL = QL.Inversion;
            Matrix BT = B.Trans;

            //多余观测数为r=m-u=n-t
            int obsCount = L.RowCount;
            int totalConditionCount = B.RowCount;
            int paramCount = A.ColCount;//u 
            int freedom = totalConditionCount - paramCount;

            Matrix W = -(B * L - A * X0 - B0);
            Matrix N = B * QL * BT;
            Matrix inverN = N.Inversion;

            //采用分块矩阵直接计算
            int dimOfK = N.ColCount; //K的维度
            int dimOfX = A.ColCount; //X的维度
            int dimOfBigN = dimOfK + dimOfX;
            Matrix BigN = new Matrix(dimOfBigN, dimOfBigN);
            BigN.SetSub(N);
            BigN.SetSub(A, 0, dimOfK);
            BigN.SetSub(AT, dimOfK, 0);
            
            Matrix inverBigN = BigN.Inversion;
            Matrix bigW = new Matrix(dimOfBigN, 1);
            bigW.SetSub(W);
            Matrix bigX = inverBigN * bigW;

            Matrix K = bigX.GetSub(0, 0, dimOfK);
            Matrix X = bigX.GetSub(dimOfK, 0);

            Matrix V = QL * BT * K;
            Matrix EstL = L + V;

            //**************精度估计---------------
            Matrix Q11 = inverBigN.GetSub(0, 0, dimOfK, dimOfK);
            Matrix Q12 = inverBigN.GetSub(0, dimOfK, dimOfX);
            Matrix Q21 = inverBigN.GetSub(dimOfK, 0, dimOfX, dimOfX);
            Matrix Q22 = inverBigN.GetSub(dimOfK, dimOfK);

            Matrix Qx = -Q22;
            Matrix QestL = QL - QL * BT * Q11 * B * QL;

            var weightedX = new WeightedVector(X, Qx);
            var weightedL = new WeightedVector(EstL, QestL);


            double vtpv = (V.Trans * PL * V).FirstValue;
            double s0 = vtpv / freedom;//单位权中误差估值

            if (!DoubleUtil.IsValid(s0))
            {
                log.Error("方差值无效！" + s0);
            }

            AdjustResultMatrix result = new AdjustResultMatrix()
                .SetAdjustmentType(AdjustmentType.具有参数的条件平差)
                .SetEstimated(weightedX)
                .SetCorrectedObs(weightedL)
                .SetObsMatrix(input)
                .SetFreedom(freedom)
                .SetVarianceFactor(s0)
                .SetVtpv(vtpv);

            return result;
        }
    }
}