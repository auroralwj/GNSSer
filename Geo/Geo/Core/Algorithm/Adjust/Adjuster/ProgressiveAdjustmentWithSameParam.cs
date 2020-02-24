//2017.07.20, czs, create in hongqing, 逐次分组平差 (阶段平差) 
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
    /// 参数固定的逐次分组平差 (阶段平差) 
    /// </summary>
    public class ProgressiveAdjustmentWithSameParam : MatrixAdjuster
    { 
        /// <summary>
        /// 对应的方法为 Process
        /// </summary> 
        public ProgressiveAdjustmentWithSameParam()
        { 
        }

        /// <summary>
        /// 分组数量从0开始。
        /// </summary>
        public int GroupIndex { get; set; }

        /// <summary>
        /// 计算
        /// </summary>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {  
            //命名规则：0表示上一个，1表示预测，无数字表示当次
            //上次次观测设置 
            var X0 = new Matrix((IMatrix)input.Apriori);
            var Qx0 = new Matrix(input.Apriori.InverseWeight);
            var Px0 =new Matrix( Qx0.GetInverse());
            //本次观测设置 
            var Qo = new Matrix(input.Observation.InverseWeight);
            var Po = new Matrix(Qo.GetInverse());
            var A = new Matrix(input.Coefficient);
            var AT = A.Trans;
            var L = new Matrix((IMatrix)input.Observation);

            int paramCount = A.ColCount;
            int obsCount = A.RowCount;

            //1.预测残差
            //计算预测残差
            var V1 = L - A * X0;//观测值 - 估计近似值
            var Qv1 = Qo + A * Qx0 * AT; 



            //2.计算增益矩阵
            var J = Qx0 * AT * Qv1.Inversion;// 增益矩阵 
            //3.平差结果
            var dX = J * V1;
            var X = X0 + dX;        

            //4.精度评定
            var Qx = Qx0 - J * A * Qx0;
            var Freedom = input.Observation.Count - input.ParamCount + input.Apriori.Count;
          
            var V = A * dX - V1;//估值-观测值 V = A * X - L = A * (X0 + deltaX) - (l + A * X0) =  A * deltaX - l.
            var vtpv = (V.Trans * Po * V)[0, 0];
            var VarianceOfUnitWeight = vtpv / Freedom;//单位权方差
            var Estimated = new WeightedVector(X, Qx) { ParamNames = input.ParamNames };


            AdjustResultMatrix result = new AdjustResultMatrix()
                .SetEstimated(Estimated)
                .SetFreedom(Freedom)
                .SetObsMatrix(input)
                .SetVarianceFactor(VarianceOfUnitWeight)
                .SetVtpv(vtpv)
                ;

            return result;
        }

    }
}