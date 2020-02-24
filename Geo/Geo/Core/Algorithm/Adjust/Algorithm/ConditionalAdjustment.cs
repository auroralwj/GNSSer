//2012, czs, Create, 条件平差
//2016.10.10, czs, refactor in hongqing, 重构
//2016.10.25,czs, edit in hongqing 实验室509机房， 自由项为 B0

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;


namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 条件平差。B L + B0 = 0
    /// 条件平差的参数就是观测值，即直接对参数进行观测。
    /// 取全部观测量的最或然值为未知数，建立这些未知数之间应满足的几何条件（条件方程），
    /// 然后依原则求满足条件方程的最或然值，并估计精度。 
    /// 1.条件方程式的个数等于多余观测数 r ：r = n - t
    /// 2.个条件方程要求相互独立，即其中的任一条件方程都不能由其余的条件方程推出。
    /// 函数模型：B L + B0 = 0
    /// 
    ///条件方程：B V + W = 0
    ///  W = BL + B0
    ///  
    /// 
    /// 平差方法的选择
    /// 1．手算时代：当 t>r 时，用条件平差，当 r>t 时，用参数平差。
    /// 2．电算时代：对于大规模网的平差一般采用参数平差法。
    /// 对于小的测边网，工程网多采用条件平差法。
    /// </summary>
    public class ConditionalAdjustment
    {
        #region 构造函数
        /// <summary>
        /// 条件平差
        /// </summary>
        /// <param name="Observation">观测值初值 X 及其权逆阵 QX </param>
        /// <param name="CoeffOfCondition">条件系数阵 B</param>
        /// <param name="ConstVectorOfCondition">条件自由项 B0，注意：非 W</param>
        public ConditionalAdjustment(WeightedVector Observation, IMatrix CoeffOfCondition, Vector ConstVectorOfCondition)
            : this(Observation, CoeffOfCondition, (IMatrix)new VectorMatrix(ConstVectorOfCondition))
        {
            
        }

        /// <summary>
        /// 条件平差
        /// </summary>
        /// <param name="Observation">观测值初值 X 及其权逆阵 QX </param>
        /// <param name="CoeffOfCondition">条件系数阵 B</param>
        /// <param name="ConstVectorOfCondition">条件自由项 B0，注意：非 W</param>
        public ConditionalAdjustment(WeightedVector Observation, IMatrix CoeffOfCondition, IMatrix ConstVectorOfCondition)
        {
            this.Observation = Observation;
            this.CoeffOfCondition = CoeffOfCondition;
            this.ConstVectorOfCondition = ConstVectorOfCondition;
        }

        #endregion


        #region 属性
        /// <summary>
        /// 观测值
        /// </summary>
        public WeightedVector Observation { get; set; }
        /// <summary>
        /// 改正后的观测值。
        /// </summary>
        public WeightedVector CorrectedObservation { get; set; }
        /// <summary>
        /// 条件数量
        /// </summary>
        public int ConditionCount { get { return CoeffOfCondition.RowCount; } }
        /// <summary>
        /// 观测值数量。
        /// </summary>
        public int ObsCount { get { return Observation.Count; } }
        /// <summary>
        /// 条件方程的系数阵
        /// </summary>
        public IMatrix CoeffOfCondition { get; set; }

        /// <summary>
        /// 条件方程自由项向量，单列矩阵,B0
        /// </summary>
        public IMatrix ConstVectorOfCondition { get; set; } 
        /// <summary>
        /// 改正数向量dL，解算结果。
        /// </summary>
        public WeightedVector Correction { get; set; }
        /// <summary>
        /// 单位权方差，单位权中误差的平方
        /// </summary>
        public double VarianceOfUnitWeight{ get; set; }
        #endregion

        /// <summary>
        /// 计算，返回观测量的该证数。
        /// </summary>
        /// <returns></returns>
        public void Process()
        { 
            //观测值和系数阵
            IMatrix B = CoeffOfCondition;  //系数阵
            IMatrix BT = B.Transposition;
            IMatrix B0 = ConstVectorOfCondition; //条件常数项
            IMatrix L = Observation;//观测值

            //权阵和权逆阵
            var QL = this.Observation.InverseWeight; 
            var PL = QL.GetInverse() ;

            var W = B.Multiply(L).Plus(B0); //W = BL + B0
            //解算
            var BQBT = B.Multiply(QL).Multiply(BT); //Normal
            var InverBQBT = BQBT.GetInverse();
           //求V和Q的中间量 - Q*BT*InverN
            var Temp = QL.Multiply(BT).Multiply(InverBQBT).Multiply(-1);
            //结果 DL = V，即观测值的改正数
            IMatrix V = Temp.Multiply(W);
            var Qv = Temp.Multiply(B).Multiply(QL);//Temp * B * QL
            //结果转化
            this.Correction = new WeightedVector(V.GetCol(0), Qv) { ParamNames = this.Observation.ParamNames };

            var VT = V.Transposition;
            var VTPV = VT.Multiply(PL).Multiply(V);
            this.VarianceOfUnitWeight = VTPV[0, 0] / CoeffOfCondition.RowCount;     
            var QLhat = QL.Minus(Qv);
            this.CorrectedObservation = new WeightedVector(Observation.Plus(V), QLhat) { ParamNames = Observation.ParamNames };
        }

  
    }
}