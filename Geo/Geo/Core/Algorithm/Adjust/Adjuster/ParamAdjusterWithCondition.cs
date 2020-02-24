//2012, czs, Create, 具有约束条件的参数平差
// 2013.05.02.02.18, czs, Creating
//2018.04.11, czs, edit in hmx, 修改具有约束条件的参数平差

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

//平差：在有多余观测的基础上，根据一组含有误差的观测值，
//依一定的数学模型，按某种平差准则，求出未知量的最优估值，并进行精度评定。
namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 具有约束条件的参数平差（The Adjustment of Parameter with Coditions）
    /// n 个方程，s个限制条件。
    /// 误差方程：V = B x - l .
    /// 条件方程：c x - w  = 0.
    /// </summary>
    public class ParamAdjusterWithCondition : MatrixAdjuster
    {
        #region 构造函数
        public ParamAdjusterWithCondition()
        {
        }

        #endregion


        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            //u(0<u<t)个独立量为参数,多余观测数r=n-t
            //原始输入 m=r+u
            Matrix A = input.Coefficient; //m×t
            Matrix B = input.SecondCoefficient;//m×n
            Matrix L = new Matrix((IMatrix)input.Observation);//n×1
            Matrix QL = new Matrix((IMatrix)input.Observation.InverseWeight);//m×m
            Matrix X0 = input.HasApprox ? new Matrix(input.ApproxVector, true) : null;//u×1
            Matrix D = input.HasFreeVector ? new Matrix(input.FreeVector, true) : null;//B0//m×1
            Matrix B0 = input.HasSecondFreeVector ? new Matrix(input.SecondFreeVector, true) : null;//B0//m×1

            Matrix AT = A.Trans;
            Matrix PL = QL.Inversion;
            Matrix BT = B.Trans;

            //多余观测数为r=m-u=n-t
            int obsCount = L.RowCount;
            int totalConditionCount = B.RowCount;
            int paramCount = A.ColCount;//u 
            int freedom = obsCount - paramCount;

            //观测值更新
            Matrix l = L - (A * X0 + D); //如果null，则是本身
            Matrix W = -(B * X0 - B0); 
            Matrix N = AT * PL * A;
            Matrix U = AT * PL * l;
            Matrix inverN = N.Inversion;
           // ************************
            //采用分块矩阵直接计算
            int dimOfX = N.ColCount; //X的维度
            int dimOfK = B.RowCount; //K的维度
            int dimOfBigN = dimOfK + dimOfX;
            Matrix BigN = new Matrix(dimOfBigN, dimOfBigN);
            BigN.SetSub(N);
            BigN.SetSub(BT, 0, dimOfX);
            BigN.SetSub(B, dimOfX, 0);
            
            Matrix inverBigN = BigN.Inversion;
            Matrix bigW = new Matrix(dimOfBigN, 1);
            bigW.SetSub(U);
            bigW.SetSub(W, U.RowCount);
            Matrix bigX = inverBigN * bigW;

            Matrix x = bigX.GetSub(0, 0, dimOfX);
            Matrix K = bigX.GetSub(dimOfX, 0);

            Matrix X = X0 + x;

            Matrix V = A * x - l;
            Matrix EstL = L + V;

            //**************精度估计---------------
            Matrix Q11 = inverBigN.GetSub(0, 0, dimOfX, dimOfX);
            Matrix Q12 = inverBigN.GetSub(0, dimOfX, dimOfK);
            Matrix Q21 = inverBigN.GetSub(dimOfX, 0, dimOfK, dimOfK);
            Matrix Q22 = inverBigN.GetSub(dimOfX, dimOfX);

            Matrix Qx = Q11;
            Matrix QestL =  A * Q11 * AT;

            var coWeightedX = new WeightedVector(X, Qx);
            var weightedX = new WeightedVector(x, Qx);
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
                .SetCorrectedEstimate(coWeightedX)
                .SetCorrectedObs(weightedL)
                .SetObsMatrix(input)
                .SetFreedom(freedom)
                .SetVarianceFactor(s0)
                .SetVtpv(vtpv);

            return result;
        }
    }
}