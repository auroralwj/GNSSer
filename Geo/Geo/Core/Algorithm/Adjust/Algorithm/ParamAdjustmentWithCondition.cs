// 2013.05.02.02.18, czs, Creating

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
    ///
    /// </summary>
    public class ParamAdjustmentWithCondition
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="coeff_error">参数（误差方程）系数阵</param>
        /// <param name="obsMinusApriori_error">误差方程右手边</param>
        /// <param name="inverseWeight">观测方程权逆阵</param>
        /// <param name="coeff_condition">条件方程系数阵</param>
        /// <param name="obsMinusApriori_condition">条件方程常数项</param>
        public ParamAdjustmentWithCondition(double[][] coeff_error, double[] obsMinusApriori_error, double[][] coeff_condition, double[] obsMinusApriori_condition, double[][] inverseWeight = null)
        {
            Init(coeff_error, MatrixUtil.Create(obsMinusApriori_error), inverseWeight, coeff_condition, MatrixUtil.Create(obsMinusApriori_condition));
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="coeffOfParams">参数（误差方程）系数阵</param>
        /// <param name="obsMinusApriori">误差方程右手边</param>
        /// <param name="inverseWeight">观测方程权逆阵</param>
        /// <param name="coeff_condition">条件方程系数阵</param>
        /// <param name="obsMinusApriori_condition">条件方程常数项</param>
        public ParamAdjustmentWithCondition(double[][] coeffOfParams, double[][] obsMinusApriori, double[][] coeff_condition, double[][] obsMinusApriori_condition, double[][] inverseWeight = null)
        {
            Init(coeffOfParams, obsMinusApriori, inverseWeight, coeff_condition, obsMinusApriori_condition);
        }


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="coeffOfParams">参数（误差方程）系数阵</param>
        /// <param name="obsMinusApriori">误差方程右手边</param>
        /// <param name="inverseWeight">观测方程权逆阵</param>
        /// <param name="coeff_condition">条件方程系数阵</param>
        /// <param name="obsMinusApriori_condition">条件方程常数项</param>
        public void Init(double[][] coeffOfParams, double[][] obsMinusApriori, double[][] inverseWeight, double[][] coeff_condition, double[][] obsMinusApriori_condition)
        {
            ArrayMatrix Q, P;

            if (inverseWeight == null)//若为空，则为单位阵
            {
                Q = new ArrayMatrix(MatrixUtil.CreateIdentity(coeffOfParams.Length));
                P = Q;
            }
            else
            {
                Q = new ArrayMatrix(inverseWeight);
                P = Q.Inverse;
            }

            this.Coeff_condition = coeff_condition;
            this.ObsMinusApriori_condition = obsMinusApriori_condition;

            this.A = new ArrayMatrix(coeffOfParams);
            this.L = new ArrayMatrix(obsMinusApriori);
            ArrayMatrix AT = A.Transpose();

            this.Weight = P.Array;
            this.ObsCount = coeffOfParams.Length;
            this.Normal_error = (AT * P * A).Array;
            this.RightHandSide_error = (AT * P * L).Array;
            this.Normal_condition = (new ArrayMatrix(coeff_condition) * new ArrayMatrix(Normal_error) * new ArrayMatrix(coeff_condition).Transpose()).Array;
        }


        /// <summary>
        ///  误差方程的法方程右手边
        /// </summary>
        public double[][] RightHandSide_error { get; set; }
        /// <summary>
        /// 误差方程的法方程系数阵
        /// </summary>
        public double[][] Normal_error { get; set; }
        /// <summary>
        /// 条件方程的法方程系数阵
        /// </summary>
        public double[][] Normal_condition { get; set; }
        /// <summary>
        /// 条件方程的系数阵
        /// </summary>
        public double[][] Coeff_condition { get; set; }
        /// <summary>
        /// 条件方程的常数项
        /// </summary>
        public double[][] ObsMinusApriori_condition { get; set; }

        /// <summary>
        ///  最小二乘解算。返回参数向量平差值。 
        /// </summary>
        /// <returns></returns>
        public double[][] Solve()
        {
            //组建大法矩阵
            ArrayMatrix InverN_error = new ArrayMatrix(Normal_error).Inverse;
            int row = Normal_error.Length + Coeff_condition.Length;
            double[][] normalArray = MatrixUtil.Create(row);
            MatrixUtil.SetSubMatrix(normalArray, Normal_error);
            MatrixUtil.SetSubMatrix(normalArray, Coeff_condition, Normal_error.Length);
            MatrixUtil.SetSubMatrix(normalArray, MatrixUtil.Transpose(Coeff_condition), 0, Normal_error.Length);
            //法方程大右手边
            double[][] rightHandSide = MatrixUtil.Create(row, 1);
            MatrixUtil.SetSubMatrix(rightHandSide, RightHandSide_error);
            MatrixUtil.SetSubMatrix(rightHandSide, ObsMinusApriori_condition, RightHandSide_error.Length);
            //解
            ArrayMatrix XK = new ArrayMatrix(normalArray).Inverse * new ArrayMatrix(rightHandSide);
            ArrayMatrix X = new ArrayMatrix(MatrixUtil.GetSubMatrix(XK.Array, 6, 1, 0, 0));

            //精度评定
            if (A != null && L != null)
            {
                ArrayMatrix V = A * X - L;
                this.VarianceOfUnitWeight = (V.Transpose() * V)[0, 0] / (this.Freedom + ConditionCount);
                //结果转化
                this.CovaOfParams = (InverN_error.Inverse * VarianceOfUnitWeight).Array;//(N.Inverse ).Array; //
            }
            this.Param = X.Array;
            return Param;
        }

        /// <summary>
        ///  最小二乘解算。返回参数向量平差值。
        ///  Delta X^ =  inv（N） U
        /// </summary>
        /// <returns></returns>
        public double[][] Solve_ToBeCheck()
        {
            ArrayMatrix InverNe = new ArrayMatrix(Normal_error).Inverse;
            ArrayMatrix Ue = new ArrayMatrix(RightHandSide_error);
            ArrayMatrix Coe_c = new ArrayMatrix(Coeff_condition);
            ArrayMatrix TransCoe_c = Coe_c.Transpose();
            ArrayMatrix InverNc = new ArrayMatrix(Normal_condition).Inverse;
            ArrayMatrix Vc = new ArrayMatrix(ObsMinusApriori_condition);

            ArrayMatrix K = InverNc * (Coe_c * InverNe * Ue - Vc);
            ArrayMatrix X = -InverNe * (TransCoe_c * K - Ue);


            //Matrix X2 = (InverNe - InverNe * TransCoe_c * InverNc * Coe_c * InverNe) * Ue - InverNe * TransCoe_c * InverNc * Vc;

            //精度评定
            if (A != null && L != null)
            {
                ArrayMatrix V = A * X - L;
                 this.ObsError = V.Array;
                this.VarianceOfUnitWeight = (V.Transpose() * V)[0, 0] / (this.Freedom + ConditionCount);
                //结果转化
                this.CovaOfParams = (InverNe.Inverse * VarianceOfUnitWeight).Array;//(N.Inverse ).Array; //
            }
            this.Param = X.Array;

            return Param; 

        }
        ArrayMatrix A, L;


        #region 属性

        /// <summary>
        /// 观测值误差 V
        /// </summary>
        public double[][] ObsError { get; set; }
        /// <summary>
        /// 权阵.Weight=InverseWeight^(-1)
        /// </summary>
        public double[][] Weight { get; set; }
        /// <summary>
        /// 参数的权逆阵（协因数阵） Inverse Weight Matrix（Cofactor Matrix ）of Some Vector。
        /// 协因数阵。InverseWeight=Weight^(-1)
        /// 法方程系数阵的逆阵为未知参数向量的权逆阵。
        /// </summary>
        public double[][] InverseWeight { get; set; }
        /// <summary>
        /// 参数阵的值.为计算结果。矩阵。
        /// </summary>
        public double[][] Param { get; private set; }
        /// <summary>
        /// 以一维数据形式返回解算的参数
        /// </summary>
        public double[] ParamVector { get { return MatrixUtil.GetColVector(Param); } }
        /// <summary>
        ///  法方程系数阵，正定阵。
        /// （1）法方程的个数等于未知参数的个数 
        /// （2）法方程系数阵 N 对称、正定。
        ///  正定阵的定义：若存在一非零向量 Y，使得 Y^T N Y > 0  ，则   正定。
        /// （3）X^ 满足无偏性、一致性、有效性。      
        /// （4）X^ 满足 V^T Weight V = min。 
        /// </summary>
        //public double[][] Normal { get; set; }
        ///// <summary>
        ///// A'PL
        ///// </summary>
        //public double[][] RightHandSide { get; set; }
        /// <summary>
        /// 参数（未知数）的协方差阵。
        /// </summary>
        public double[][] CovaOfParams { get; set; }

        /// <summary>
        /// Aposteriori variance factor.验后方差因子（单位权方差）。
        /// 方差：随机变量与其数学期望之差的平方的数学期望，称为方差。
        /// </summary>
        public double VarianceOfUnitWeight { get; set; }

        /// <summary>
        ///  均方差(Standard deviation )估值， 单位权中误差。
        ///  方差不可求而中误差可求. 
        ///  中误差：root mean square error; RMSE,也可称为 标准差 或 均方根差？
        ///  单位权中误差： unit weight mean square error.
        /// </summary>
        public double StdDev { get { return Math.Sqrt(VarianceOfUnitWeight); } }

        /// <summary>
        /// 观测值数量
        /// </summary>
        public int ObsCount { get; set; }
        /// <summary>
        /// 条件数
        /// </summary>
        public int ConditionCount { get; set; }
        /// <summary>
        /// 未知参数数量
        /// </summary>
        public int ParamsCount { get { return Normal_error.Length; } }
        /// <summary>
        /// 自由度，样本中独立或能自由变化的变量个数,通常为：样本个数 - 被限制的变量个数或条件数，或多余观测数。
        /// </summary>
        public int Freedom { get { return ObsCount - ParamsCount; } }

        #endregion


    }
}
