//2018.04.13, czs,   created in hmx, 递归最小二乘法 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Algorithm.Adjust;

//平差：在有多余观测的基础上，根据一组含有误差的观测值，
//依一定的数学模型，按某种平差准则，求出未知量的最优估值，并进行精度评定。
namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 递归最小二乘法。
    /// 最小二乘法方程之参数平差,或间接平差。
    ///    函数模型： L = A X + delta
    ///    随机模型： E(delta) = 0; 
    ///    误差方程： v  = A x - l , 其中 l = L - A X0, 即 l = 观测值 - 先验值的函数值。
    /// </summary>
    public class SingleRecursiveAdjuster : MatrixAdjuster
    {
        #region 构造函数         
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleRecursiveAdjuster() { }
        #endregion

        #region 核心计算方法
        /// <summary>
        /// 数据计算
        /// </summary>
        /// <param name="input">观测矩阵</param>
        /// <returns></returns>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            this.ObsMatrix = input;

            //观测值权阵设置,对已知量赋值 
            Matrix L = new Matrix((IMatrix)input.Observation);
            Matrix QL = new Matrix(input.Observation.InverseWeight);
            Matrix PL = new Matrix(QL.GetInverse());
            Matrix A = new Matrix(input.Coefficient);
            Matrix AT = A.Trans;
            Matrix B = new Matrix(input.SecondCoefficient);
            Matrix BT = B.Trans;
            Matrix X0 = input.HasApprox ? new Matrix(input.ApproxVector, true) : null;
            Matrix Y0 = input.HasSecondApprox ? new Matrix(input.SecondApproxVector, true) : null;
            Matrix D = input.HasFreeVector ? new Matrix(input.FreeVector, true) : null;
            int obsCount = L.RowCount;
            int fixedParamCount = B.ColCount;

            //观测值更新
            Matrix l=L-(A * X0 + B* Y0 + D); //如果null，则是本身

            Matrix ATPL = AT * PL;
            //法方程
            Matrix Na = ATPL * A;
            Matrix Nab = AT * PL * B;
            Matrix InverNa = Na.Inversion;
            Matrix J = A * InverNa * AT * PL;
            Matrix B2 = (Matrix.CreateIdentity(J.RowCount) - J) * B;
            Matrix AcT = B2.Trans;
            Matrix Nc = AcT * PL * B2; ;
            Matrix InverNc = Nc.Inversion;
            //平差结果 
            Matrix y = InverNc * AcT * PL * l;
            Matrix Y = Y0 + y;
            Matrix Qy = InverNc;

            //只针对y的精度评定
            Matrix W = B2 * y - l;
            double ys0 = (W.Trans * PL * W).FirstValue / (obsCount - fixedParamCount);
            Matrix Dy = ys0 * Qy;


            //求x
            Matrix lx = L - (A * X0 + B * Y + D); //如果null，则是本身
            Matrix x = InverNa * ATPL * lx;//这两个计算是等价的
           // Matrix x = InverNa * (ATPL * l - Nab * y);//这两个计算是等价的
            Matrix X = X0 + x;

            Matrix Ntmp = Na.Inversion * Nab;
            Matrix Qx = InverNa + Ntmp * Qy * Ntmp.Trans;

            //Matrix Qxy = AT * PL * B;
            //Matrix Qtemp = Qx * Qxy;
            //Matrix QtempT = Qtemp.Trans;
            //Matrix Dx = Qx + Qtemp * Dy * QtempT;

            //精度评定
            Matrix V =  A * x  + B * y - l;
            Matrix Qv = QL - A * Qx * AT - B * Qy * BT;

           // Matrix lT = l.Trans;

            double vtpv = (V.Trans * PL * V)[0, 0];//(lT * PL * l - lT * PL * Ac * y).FirstValue;// 
            int paramCount = A.ColCount + B.ColCount;
            int freedom = A.RowCount - paramCount;
            double s0 = vtpv / (freedom == 0 ? 0.1 : freedom);//单位权方差  

            WeightedVector estX = new WeightedVector(x, Qx) { ParamNames = input.ParamNames };
            WeightedVector CorrectedEstimate = new WeightedVector(X, Qx) { ParamNames = input.ParamNames };
            WeightedVector estY = new WeightedVector(y, Qy) { ParamNames = input.SecondParamNames };
            WeightedVector estV  = new WeightedVector(V, Qv) ;
             
            Matrix Lhat = L + V;
            Matrix QLhat = A * Qx * AT;
            var correctedObs = new WeightedVector(Lhat, QLhat) { ParamNames = this.ObsMatrix.Observation.ParamNames };


            if (!DoubleUtil.IsValid(s0))
            {
                log.Error("方差值无效！" + s0);
            }

            AdjustResultMatrix result = new AdjustResultMatrix()
                .SetAdjustmentType(AdjustmentType.单期递归最小二乘)
                .SetEstimated(estX)
                .SetSecondEstimated(estY)
                .SetCorrection(estV)
                .SetCorrectedObs(correctedObs)
                .SetCorrectedEstimate(CorrectedEstimate)
                .SetObsMatrix(input)
                .SetFreedom(freedom)
                .SetVarianceFactor(s0)
                .SetVtpv(vtpv);

            return result;
        }

        #endregion

    }
}
