//2013.05.03.10.41, czs, in Pengzhou, Creating 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

//平差：在有多余观测的基础上，根据一组含有误差的观测值，
//依一定的数学模型，按某种平差准则，求出未知量的最优估值，并进行精度评定。
namespace Geo.Algorithm.Adjust
{
    //起源于根据最小二乘推估来内插和外推重力异常的课题。
    //1969年，克拉鲁普把推估重力异常的方法发展为用不同类型的数据，例如重力异常、垂线偏差等，去估计重力异常场中的任一元素，
    //例如扰动位、大地水准面差距等，提出了最小二乘配置法。莫里兹对最小二乘配置进行了系统深入的研究，提出了带系统参数的最小二乘配置，
    //并概述了这种方法在大地测量其它方面的应用。最小二乘配置还在航空摄影测量、坐标系统变换、GPS水准拟合、形变测量等方面得到应用。
    //
    //与一般最小二乘法不同的是：最小二乘法将全部待估参数作为非随机量，或不考虑参数的随机特性。
    //而最小二乘配置包含随机参数。
 
    /// <summary>
    ///最小二乘配置(least square collocation)
    /// V = A X + B Y - l, l = L - A X0 - B Y0, Y = [S s]'
    /// 最小二乘配置的主要目的是估计未测点的完全信号   
    /// </summary>
    public class LsCollocation
    {

        /// <summary>
        /// 最小二乘配置(least square collocation)
        /// </summary>
        /// <param name="coeff_param">参数的系数阵</param>
        /// <param name="obsMinusApriori">观测值减去估值</param>
        /// <param name="inverseWeight_obs">观测值的权逆阵</param>
        /// <param name="inverseWeight_signal">含下面最后三个变量</param>
        public LsCollocation(
            double[][] coeff_param,//参数的系数阵
            double[][] obsMinusApriori,//观测值减去估值
            double[][] inverseWeight_obs,//观测值的权逆阵
            double[][] inverseWeight_signal// 含下面最后三个变量
            )
        {
            Init(
            coeff_param,//参数的系数阵
            obsMinusApriori,//观测值减去估值
            inverseWeight_obs,//观测值的权逆阵
            inverseWeight_signal// 含下面最后三个变量
            );
        }
        /// 初始化并计算。
        /// </summary>
        /// <param name="coeff_param">参数的系数阵</param>
        /// <param name="obsMinusApriori">观测值减去估值</param>
        /// <param name="inverseWeight_obs">观测值的权逆阵</param>
        /// <param name="inverseWeight_signal">含下面最后三个变量</param>
        public void Init(
            double[][] coeff_param,//参数的系数阵
            double[][] obsMinusApriori,//观测值减去估值
            double[][] inverseWeight_obs,//观测值的权逆阵
            double[][] inverseWeight_signal// 含下面最后三个变量
            )
        {
            int paramCount = coeff_param[0].Length;
            int allSignalCount = inverseWeight_signal[0].Length;
            int measuredSignalCount = paramCount;
            int notMeasuredSignalCount = allSignalCount - measuredSignalCount;

            //生成信号的系数阵
            double[][] coeff_signal = MatrixUtil.Create(paramCount, allSignalCount);//是否应该自己生成
            MatrixUtil.SetSubMatrix(coeff_signal, MatrixUtil.CreateIdentity(paramCount));

            //提取方差、协方差阵
            double[][] inverseWeight_mesuredSignal = MatrixUtil.GetSubMatrix(inverseWeight_signal, measuredSignalCount, measuredSignalCount); //inverseWeight_signal的左上角
            double[][] inverseWeight_notMesuredSignal = MatrixUtil.GetSubMatrix(inverseWeight_signal, notMeasuredSignalCount, notMeasuredSignalCount, measuredSignalCount, measuredSignalCount); //inverseWeight_signal的右下角
            double[][] covaSignals_Ss = MatrixUtil.GetSubMatrix(inverseWeight_signal, 0, notMeasuredSignalCount, measuredSignalCount, measuredSignalCount);   //inverseWeight_signal的右上角

            Init(
            coeff_param,//参数的系数阵
            obsMinusApriori,//观测值减去估值
            inverseWeight_obs,//观测值的权逆阵
            inverseWeight_signal,// 含下面最后三个变量
            coeff_signal,//是否应该自己生成
            inverseWeight_mesuredSignal,//inverseWeight_signal的左上角
            inverseWeight_notMesuredSignal,//inverseWeight_signal的右下角
            covaSignals_Ss//inverseWeight_signal的右上角
            );
        }
        /// <summary>
        /// 初始化并计算。
        /// </summary>
        /// <param name="coeff_param">参数的系数阵</param>
        /// <param name="obsMinusApriori">观测值减去估值</param>
        /// <param name="inverseWeight_obs">观测值的权逆阵</param>
        /// <param name="inverseWeight_signal">含下面最后三个变量</param>
        /// <param name="coeff_signal">是否应该自己生成</param>
        /// <param name="inverseWeight_mesuredSignal">inverseWeight_signal的左上角</param>
        /// <param name="inverseWeight_notMesuredSignal">inverseWeight_signal的右下角</param>
        /// <param name="covaSignals_Ss">inverseWeight_signal的右上角</param>
        public void Init(
            double[][] coeff_param,//参数的系数阵
            double[][] obsMinusApriori,//观测值减去估值
            double[][] inverseWeight_obs,//观测值的权逆阵
            double[][] inverseWeight_signal,// 含下面最后三个变量
            double[][] coeff_signal,//是否应该自己生成
            double[][] inverseWeight_mesuredSignal,//inverseWeight_signal的左上角
            double[][] inverseWeight_notMesuredSignal,//inverseWeight_signal的右下角
            double[][] covaSignals_Ss//inverseWeight_signal的右上角
            )
        {
            ArrayMatrix
                D_L,//总模型方差
                P_L,//总模型权阵
                B,//信号系数阵
                BT,//信号系数阵的转置
                D_Y,//信号方差
                D_S,//已测信号方差
                P_S,//已测信号权阵
                D_s,//未测信号方差
                D_Ss,//已测和未测信号的协方差
                D_obs,//观测量的方差
                P_obs//观测量的权阵
                ;

            B = new ArrayMatrix(coeff_signal);
            BT = B.Transpose();
            D_Y = new ArrayMatrix(inverseWeight_signal);
            D_S = new ArrayMatrix(inverseWeight_mesuredSignal);
            P_S = D_S.Inverse;
            D_s = new ArrayMatrix(inverseWeight_notMesuredSignal);
            D_Ss = new ArrayMatrix(covaSignals_Ss);

            D_obs = new ArrayMatrix(inverseWeight_obs);
            P_obs = D_obs.Inverse;

            //第1步 求P_L，D_L 
            D_L = B * D_Y * BT + D_obs;// = D_S  + D_obs
            P_L = D_L.Inverse;

            this.Weight_obs = P_L.Array;

            this.A = new ArrayMatrix(coeff_param);
            this.l = new ArrayMatrix(obsMinusApriori);
            ArrayMatrix AT = A.Transpose();

            this.Weight_obs = P_L.Array;
            this.ObsCount = coeff_param.Length;
            this.Normal = (AT * P_L * A).Array;
            this.RightHandSide = (AT * P_L * l).Array;

            //第2步，参数 X 解
            ArrayMatrix N = new ArrayMatrix(Normal);
            ArrayMatrix U = new ArrayMatrix(RightHandSide);
            ArrayMatrix X = N.Inverse * U;
            this.Param = X.Array;

            ArrayMatrix LminusAX = l - A * X;
            //第3步，求已测信号 S
            ArrayMatrix S = D_S * P_L * LminusAX;
            MeasuredSignal = S.Array;

            //第4步，求未测信号 s
            ArrayMatrix s = D_Ss * P_L * LminusAX;
            NotMeasuredSignal = s.Array;

            //第5步，求未测信号 s
            ArrayMatrix V = -D_obs * D_L * LminusAX;

            //第6步，精度评定 
            this.VarianceOfUnitWeight = ((V.Transpose() * P_obs * V)[0, 0] - (S.Transpose() * P_S * S)[0, 0]) / (this.Freedom);
            //结果转化
            this.CovaOfParams = (N.Inverse * VarianceOfUnitWeight).Array;//(N.Inverse ).Array; //
        }

        ArrayMatrix A, l;

        /// <summary>
        /// 非随机参数向量。NonrandomParamVector
        /// </summary>
        public double[] ParamVector { get; set; }
        /// <summary>
        /// 随机参数向量。
        /// </summary>
        public double[] RandomParamVector { get; set; }
        /// <summary>
        /// 已测信号向量
        /// </summary>
        public double[] MeasuredSignalVector { get; set; }
        /// <summary>
        /// 未测信号向量
        /// </summary>
        public double[] NotMeasuredSignalVector { get; set; }

        /// <summary>
        /// 已测信号
        /// </summary>
        public double[][] MeasuredSignal { get; set; }
        /// <summary>
        /// 未测信号
        /// </summary>
        public double[][] NotMeasuredSignal { get; set; }
        /// <summary>
        /// 参数。
        /// </summary>
        public double[][] Param { get; set; }
        /// <summary>
        /// 自由度，样本中独立或能自由变化的变量个数,通常为：样本个数 - 被限制的变量个数或条件数，或多余观测数。
        /// </summary>
        public int Freedom { get; set; }
        /// <summary>
        /// 观测数量。
        /// </summary>
        public int ObsCount { get; set; }
        /// <summary>
        /// 单位权方差
        /// </summary>
        public double VarianceOfUnitWeight { get; set; }
        /// <summary>
        /// 参数协方差
        /// </summary>
        public double[][] CovaOfParams { get; set; }
        /// <summary>
        /// 法方程右手边
        /// </summary>
        public double[][] RightHandSide { get; set; }
        /// <summary>
        /// 法方程
        /// </summary>
        public double[][] Normal { get; set; }
        public double[][] Weight_obs { get; set; }
    }
}
