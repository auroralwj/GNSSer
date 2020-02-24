


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using System.Threading.Tasks;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 分区平差的一个分区。
    /// 有两个步骤：第一次计算区内Ni和Ui；第二次由公共参数值计算区内参数值。
    /// </summary>
    public class BlockAdjustItem
    {
        /// <summary>
        /// 分区平差的一个分区
        /// </summary>
        /// <param name="coeffOfParams">区内参数观测方程系数阵</param>
        /// <param name="obs">观测值</param>
        /// <param name="inverseWeightOfObs">观测值权逆阵</param>
        /// <param name="coeffOfCommonParams">公共参数观测方程系数阵</param> 
        public BlockAdjustItem(
           double[][] coeffOfParams,
           double[][] obs,
           double[][] inverseWeightOfObs,
           double[][] coeffOfCommonParams)
        {
            this.CoeffOfParams = coeffOfParams;
            this.Obs = obs;
            this.InverseWeightOfObs = inverseWeightOfObs;
            this.CoeffOfCommonParams = coeffOfCommonParams;

            this.ObsCount = obs.Length;
            this.BlockParamCount = CoeffOfParams[0].Length;
            this.ObsCount = CoeffOfParams.Length;
            this.CommonParamCount = coeffOfCommonParams[0].Length;
            this.Freedom = this.ObsCount - this.BlockParamCount;
            //Solve();
        }
        /// <summary>
        /// 第一步计算。如，在多核上执行。
        /// 求 RightHand，NormalAB， etc. 
        /// </summary>
        public void Solve_step1()
        {
            //输入参数
            ArrayMatrix A = new ArrayMatrix(CoeffOfParams);
            ArrayMatrix Q = new ArrayMatrix(InverseWeightOfObs);
            ArrayMatrix P = Q.Inverse;
            ArrayMatrix L = new ArrayMatrix(Obs);
            ArrayMatrix B = new ArrayMatrix(CoeffOfCommonParams);
            //间接变量
            ArrayMatrix AT = A.Transpose();
            ArrayMatrix BT = B.Transpose();
            ArrayMatrix Na = AT * P * A;
            ArrayMatrix InverNa = Na.Inverse;
            ArrayMatrix Ua = AT * P * L;
            ArrayMatrix Nb = AT * P * B;//不一定为法矩阵Normal
            ArrayMatrix NbT = Nb.Transpose();
            ArrayMatrix Ub = BT * P * L;
            ArrayMatrix Nbb = BT * P * B;

            //结果
            ArrayMatrix N = Nbb - NbT * InverNa * Nb;
            ArrayMatrix U = Ub - NbT * InverNa * Ua;

            this.InverseNormal = InverNa.Array;
            this.Normal = N.Array;
            this.RightHand = U.Array;
            this.RightHandA = Ua.Array;
            this.NormalAB = Nb.Array;
            this.InverseWeightOfParam = N.Array;
        }

        /// <summary>
        /// 第二步，由公共参数计算分区参数。
        /// 求 ObsError， Params。
        /// </summary>
        public void Solve_step2()
        {
            ArrayMatrix A = new ArrayMatrix(CoeffOfParams);
            ArrayMatrix Q = new ArrayMatrix(InverseWeightOfObs);
            ArrayMatrix P = Q.Inverse;
            ArrayMatrix L = new ArrayMatrix(Obs);
            ArrayMatrix B = new ArrayMatrix(CoeffOfCommonParams);


            ArrayMatrix Xb = new ArrayMatrix(CommonParams);
            ArrayMatrix InverNa = new ArrayMatrix(InverseNormal);
            ArrayMatrix Ua = new ArrayMatrix(RightHandA);
            ArrayMatrix Nb = new ArrayMatrix(NormalAB);

            ArrayMatrix X = InverNa * (Ua - Nb * Xb);
            ArrayMatrix V = A * X + B * Xb - L;
            ArrayMatrix vtpv = V.Transpose() * P * V;

            this.ObsError = V.Array;
            this.EstimatedParam = X.Array;
            this.VTPV = vtpv[0,0];
            this.VarianceOfUnitWeight = this.VTPV / Freedom;
            this.StdDev = Math.Sqrt(VarianceOfUnitWeight);//标准差

            ArrayMatrix Dx = InverNa * VarianceOfUnitWeight;//协方差阵

            double[] ParamCovaVector = MatrixUtil.GetDiagonal(Dx.Array);
            this.ParamRmsVector = MatrixUtil.GetPow(ParamCovaVector, 0.5);

            //结果转化 
            this.WeightOfParam = InverNa.Array;  
            this.CovaOfParams = Dx.Array;
            this.ObsError = V.Array;
        }

        #region 结果属性
        /// <summary>
        /// V'PV 用于精度股估计。
        /// </summary>
        public double VTPV { get; private set; }
        /// <summary>
        /// 公共参数
        /// </summary>
        public double[][] CommonParams { set; private get; }
        /// <summary>
        /// 观测残差 V 
        /// </summary>
        public double[][] ObsError { get; private set; }
        /// <summary>
        /// 区内参数值.需要调用GetParams方法后才赋值。
        /// </summary>
        public double[][] EstimatedParam { get; private set; }
        /// <summary>
        /// 分区内参数权逆阵。
        /// </summary>
        public double[][] InverseNormal { get; private set; }
        /// <summary>
        /// 发方程系数阵组成部分。
        /// </summary>
        public double[][] Normal { get; private set; }
        /// <summary>
        /// 区内发方程右手边组成部分。
        /// </summary>
        public double[][] RightHandA { get; private set; }
        /// <summary>
        /// 发方程右手边组成部分。
        /// </summary>
        public double[][] RightHand { get; private set; }

        /// <summary>
        /// AT * P * Ab;
        /// </summary>
        public double[][] NormalAB { get; private set; }
        #endregion 

        #region 输入参数属性
        /// <summary>
        /// 观测系数阵
        /// </summary>
        public double[][] CoeffOfParams { get; private set; }
        /// <summary>
        /// 观测值
        /// </summary>
        public double[][] Obs { get; private set; }
        /// <summary>
        /// 观测值权逆阵
        /// </summary>
        public double[][] InverseWeightOfObs { get; private set; }
        /// <summary>
        /// 公共参数系数阵
        /// </summary>
        public double[][] CoeffOfCommonParams { get; private set; }
        #endregion

        /// <summary>
        /// 参数的均方根差。
        /// </summary>
        public double[] ParamRmsVector { get; private set; }
        /// <summary>
        ///  参数的权逆阵。 法方程系数阵，正定阵。
        /// （1）法方程的个数等于未知参数的个数 
        /// （2）法方程系数阵 N 对称、正定。
        ///  正定阵的定义：若存在一非零向量 Y，使得 Y^T N Y > 0  ，则   正定。
        /// （3）X^ 满足无偏性、一致性、有效性。      
        /// （4）X^ 满足 V^T Weight V = min。 
        /// </summary>
        public double[][] InverseWeightOfParam { get; private set; }
        /// <summary>
        /// 参数的权阵
        /// </summary>
        public double[][] WeightOfParam { get; private set; }
        /// <summary>
        /// 参数（未知数）的协方差阵。D = Inverse(Normal) * VarianceOfUnitWeight.
        /// </summary>
        public double[][] CovaOfParams { get; private set; }

        /// <summary>
        /// 单位权方差 Aposteriori variance factor.验后方差因子。
        /// 方差：随机变量与其数学期望之差的平方的数学期望，称为方差。
        /// </summary>
        public double VarianceOfUnitWeight { get; private set; }

        /// <summary>
        ///   单位权中误差,均方差(Standard deviation )估值。
        ///  方差不可求而中误差可求. 
        /// </summary>
        public double StdDev { get; private set; }
        /// <summary>
        /// 本区观测数量
        /// </summary>
        public int ObsCount { get; private set; }
        /// <summary>
        /// 区内参数数量
        /// </summary>
        public int BlockParamCount { get; private set; }
        /// <summary>
        /// 公共参数数量
        /// </summary>
        public int CommonParamCount { get; private set; }
        /// <summary>
        /// 自由度（多余观测数=ObsCount - BlockParamCount）,此处公共参数作为已知，不知对不对？？？2013.06.23
        /// </summary>
        public int Freedom { get; private set; }
    }

}
