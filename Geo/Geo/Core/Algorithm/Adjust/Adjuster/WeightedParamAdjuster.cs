//2013.06.01, czs, Creating.
//2017.07.20, czs, edit in hongqing, 实现 Adjustment
//2018.03.24, czs, edit in hmx, 按照新架构重构 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    //参见宋力杰课件
    /// <summary>
    /// 参数加权平差。参数分为两部分加权和未加权的。
    /// </summary>
    public class WeightedParamAdjuster : MatrixAdjuster// AdjustResultMatrix
    {   
        
        /// <summary>
        /// 对应的方法为 Process
        /// </summary>
        public WeightedParamAdjuster()
        {
        }

        /// <summary>
        /// 参数加权平差
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        { 
            //下标 o 表示观测值，x表示估计值，xa0表示具有先验信息的随机参数, 
            //xa为包含先验信息xa0的矩阵，是在整个误差方程中计算的矩阵 
            var Freedom = input.ObsCount - (input.ParamCount - input.Apriori.RowCount); // n -tb

            Matrix A = new Matrix(input.Coefficient);
            Matrix AT = A.Trans;
            Matrix L = new Matrix((IMatrix)input.Observation);
            Matrix Qo = new Matrix(input.Observation.InverseWeight );
            Matrix Po = Qo.Inversion;
            int obsCount = L.RowCount;
            int paramCount = A.ColCount;

            //具有先验信息的随机参数
            Matrix Xa0 = new Matrix((IMatrix)input.Apriori);
            Matrix Qxa0 = new Matrix(input.Apriori.InverseWeight);
            Matrix Pxa0 = Qxa0.Inversion;

            //计算先验信息的平差矩阵部分
            Matrix Nxa0 = new Matrix(input.ParamCount);
            Nxa0.SetSub(Pxa0); 

            Matrix Uxa0 = Pxa0 * Xa0; 
            Matrix Uxa = new Matrix(input.ParamCount, 1);
            Uxa.SetSub(Uxa0);
            
            //法方程系数阵
            Matrix N = AT * Po * A + Nxa0;
            Matrix InverN = N.Inversion;

            //法方程右手边
            Matrix U = AT * Po * L + Uxa;

            //计算估值
            Matrix X = InverN * U;

            //精度估计
            Matrix Xa = X.GetSub(0,0, Xa0.RowCount);
            Matrix dXa = Xa - Xa0;

            Matrix V = A * X - L;
            Matrix VT = V.Trans;
            var vtpv = (VT * Po * V + dXa.Trans * Pxa0 * dXa).FirstValue;
            var VarianceOfUnitWeight = vtpv / Freedom;
 
            var Estimated = new WeightedVector(X, InverN) { ParamNames = input.ParamNames };
            AdjustResultMatrix result = new AdjustResultMatrix()
               .SetEstimated(Estimated)
               .SetObsMatrix(input)
               .SetFreedom(Freedom)
               .SetVarianceFactor(VarianceOfUnitWeight)
               .SetVtpv(vtpv);
            return result;
        }

         

        /// <summary>
        /// 测试参数加权平差
        /// </summary>
        public static void Test()
        {
            Random random = new Random();//观测噪声
            double[][] A = MatrixUtil.CreateIdentity(6);
            double[][] L = MatrixUtil.Create(6, 1);
            for (int i = 0; i < L.Length; i++) L[i][0] = i + 1 + random.NextDouble() - 0.5;
            double[][] QL = MatrixUtil.CreateDiagonal(6, 1);
            double[][] Xa0 = MatrixUtil.Create(3, 1);
            for (int i = 0; i < Xa0.Length; i++) Xa0[i][0] = i + 1;
            double[][] Qx0 = MatrixUtil.CreateDiagonal(3, 0.001);

         //   WeightedParamAdjustment adjust = new WeightedParamAdjustment(A, L, QL, Xa0, Qx0);
        }
         
    }
}
