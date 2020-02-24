//2014.09.09, czs, edit, 继承自 Geo.Algorithm.Adjust.Adjustment 
//2016.10.01, double, edit in hongqing, 修正增益矩阵和均方差的计算
//2018.03.24, czs, edit in hmx, 按照新架构重构 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo;

namespace Geo.Algorithm.Adjust
{
    //卡尔曼全名Rudolf Emil Kalman，匈牙利数学家，1930年出生于匈牙利首都布达佩斯。
    //1953，1954年于麻省理工学院分别获得电机工程学士及硕士学位。1957年于哥伦比亚大学获得博士学位。
    //卡尔曼滤波器，正是源博士论文和1960年发表的论文《A New Approach to Linear Filtering and Prediction Problems》
    //（线性滤波与预测问题的新方法）
    //http://www.cs.unc.edu/~welch/kalman/media/pdf/Kalman1960.pdf, 2013.5.05 链接可用

    ///卡尔曼滤波的基本思想是：假设系统的动态噪声和观测噪声的统计特性为已知的零均值或高斯白噪声序列，
    ///通过建立动态系统的状态方程和测量方程，以状态估计的均方差达到最小为指标，来获得系统状态的估值。 

    ///状态估计首先要建立系统的状态方程和观测方程，也可以称为动态系统的函数模型。
    ///状态方程：X'(t) = B(t)X(t) + F(t)Ω(t)
    ///观测方程：L(t) = A(t)X(t) + Δ(t)
    ///式中 X(t) 为状态向量，X'(t)为X(t)对t的一阶微分，A、B、F都是随时间变化的系数矩阵，
    ///L为观测向量，Ω为动态噪声，Δ为系统观测噪声。

    ///求解以上微分方程并离散化，的线性状态方程和观测方程,括号内为下标：


    /// <summary>
    /// 卡尔曼滤波。
    /// 需要建立状态方程和观测方程：
    /// X(k) = Φ(k,k-1) X(k-1) + W(k);
    /// L(k) = A(k)X(k) + Δ(k)
    /// 其中，X(k),X(k-1)分别为t(k)、t(k-1)时刻的状态向量，Φ(k,k-1)为m*m阶状态转移矩阵，
    /// W(k)为动力模型噪声向量，L(k)为t(k)时刻的观测向量，A(k)为n*m阶设计矩阵，也称观测矩阵，
    /// Δ(k)为观测噪声向量。
    /// 
    ///卡尔曼滤波就是利用观测向量L1、L2、...、Lk，由相应的状态方程及随机模型求tk时刻的状态向量的最佳估值。
    ///
    /// 设X(k-1)为t(k-1)时刻的状态向量，D_X(k-1)为其协方差矩阵，L(k)为t(k)时刻的观测量，
    /// 观测噪声向量 Δ(k)和动态噪声向量W(k)为高斯白噪声误差向量，即：
    /// D_W(k)W(j) = δkjDw,
    /// D_Δ(k)Δ(j) =δkj DΔ 
    /// 
    /// 1.由观测数据组成误差方程
    /// 2.输入上历元的状态向量及其权逆阵
    /// 3.计算状态向量的预报值及其权逆阵
    /// 4.计算预报残差及其权逆阵
    /// 5.计算增益矩阵
    /// 6.计算参数估值及其精度估计
    /// </summary>
    public class KalmanFilter : Geo.Algorithm.Adjust.MatrixAdjuster
    {
        //参数命名规则：下标 0 表示上一个，1 表示预测，无数字表示当次 
        #region 构造函数 
            /// <summary>
            /// 构造函数
            /// </summary>
        public KalmanFilter()
        {

        }
        /// <summary>
        /// 观测量的叠加
        /// </summary>
        public int SumOfObsCount { get; set; }
        #endregion
        /// <summary>
        /// 数据处理。全部转化为计算偏移量，即，各种参数采用近似值，此处需要考虑初值情况。
        /// </summary>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            //参数命名规则：下标 0 表示上一个，1 表示预测，无数字表示当次 
            #region 参数预测  
            WeightedVector apriori = input.Apriori;
            if (apriori == null)
                throw new ArgumentException("必须具有先验参数值。");
            //先验值赋值
            ArrayMatrix X0 = new ArrayMatrix(apriori);                              //上一次参数估值
            ArrayMatrix Qx0 = new ArrayMatrix(apriori.InverseWeight.Array);                      //上一次估计误差方差权逆阵

            ArrayMatrix Trans = new ArrayMatrix(input.Transfer.Array);           //状态转移矩阵
            ArrayMatrix TransT = Trans.Transpose();
            ArrayMatrix Q_m = new ArrayMatrix(input.InverseWeightOfTransfer.Array);    //状态转移模型噪声

            //计算参数预测值，可以看做序贯平差中的第一组数据
            ArrayMatrix X1 = Trans * X0;
            ArrayMatrix Qx1 = Trans * Qx0 * TransT + Q_m;

            var Predicted = new WeightedVector(X1, Qx1) { ParamNames = input.ParamNames };//结果为残差 
            #endregion

            //System.IO.File.WriteAllText(saveDir + @"\Predicted.txt", Predicted.ToFormatedText());
            //  System.IO.File.WriteAllText(saveDir + @"\Apriori.txt", Apriori.ToFormatedText()); 

            #region 参数估计
            ArrayMatrix A = new ArrayMatrix(input.Coefficient.Array);                 //误差方程系数阵
            ArrayMatrix AT = A.Transpose();                           //A 的转置 

            //估计值才需要观测值，而预测值不需要
            //观测值赋值 
            WeightedVector obs = input.Observation - input.FreeVector;
            if (obs == null)
                throw new ArgumentException("必须具有观测向量。");

            ArrayMatrix L = new ArrayMatrix(obs); //观测值，或 观测值 - 估计值，！！
            ArrayMatrix Q_o = new ArrayMatrix(obs.InverseWeight.Array);//观测噪声权逆阵
            ArrayMatrix P_o = Q_o.Inverse;


            //计算预测的观测残差 自由项
            //由 V = A X - L, 得 V = A x - l, l = L - A X0, X = X0 + x
            ArrayMatrix dL = L - A * X1;//此处注意符号
            ArrayMatrix QdL = Q_o + A * Qx1 * AT;
            //计算平差值的权阵
            ArrayMatrix PXk = AT * P_o * A + Qx1.Inverse;
            //计算平差值的权逆阵
            ArrayMatrix Qx = PXk.Inverse;
            //计算增益矩阵
            ArrayMatrix J = Qx * AT * P_o;
            //计算参数改正值和估值
            ArrayMatrix deltaX = J * dL;
            ArrayMatrix X = X1 + deltaX;//改 X0 为 X1

            //精度估计
            ArrayMatrix UnitMatrix = ArrayMatrix.EyeMatrix(J.RowCount, 1.0);
            ArrayMatrix B = UnitMatrix - J * A;
            Qx = B * Qx1 * B.Transposition + J * Q_o * J.Transposition; //参数权逆阵  

            //   Matrix Qx = (AT * P_o * A + Qx1.Inverse).Inverse;

            var Estimated = new WeightedVector(X, Qx) { ParamNames = input.ParamNames };
            #endregion

            #region 验后观测残差
            // Matrix Px = Qx.Inverse;
            ArrayMatrix V = L - A * X;

            this.PostfitObservation = new Vector(MatrixUtil.GetColVector(V.Array)) { ParamNames = input.ParamNames };
            #endregion

            #region 精度估计
            this.SumOfObsCount += input.ObsCount;
            var Freedom = SumOfObsCount - input.ParamCount;// input.Freedom;

            //观测噪声权阵
            ArrayMatrix V1TPV1 = deltaX.Transpose() * Qx1.Inverse * deltaX;
            ArrayMatrix VTPV = V.Transpose() * P_o * V;
            var vtpv = VTPV[0, 0];
            double upper = (V1TPV1 + VTPV)[0, 0];


            if (!DoubleUtil.IsValid(upper) || upper > 1e10 || upper < 0)
                log.Debug("方差值无效！" + upper);

            //赋值
            var VarianceOfUnitWeight = Math.Abs(upper) / Freedom;

            //System.IO.File.WriteAllText(saveDir + @"\Estimated.txt", Estimated.ToFormatedText());
            //System.IO.File.WriteAllText(saveDir + @"\Observation.txt", Observation.ToFormatedText());

            #endregion

            AdjustResultMatrix result = new AdjustResultMatrix()
             .SetEstimated(Estimated)
             .SetFreedom(Freedom)
             .SetObsMatrix(input)
             .SetVarianceFactor(VarianceOfUnitWeight)
             .SetVtpv(vtpv);

            return result;
        }
         

        #region 其它算法
        /// <summary>
        /// 计算的简化版本
        /// </summary>
        /// <param name="coeff"></param>
        /// <param name="obs"></param>
        /// <param name="inverseWeight_obs"></param>
        /// <param name="lastInverseWeight"></param>
        /// <param name="inverseWeight_model"></param>
        /// <param name="trans"></param>
        /// <param name="lastParams"></param>
        public void Init1(
            double[][] coeff,//A
            double[][] obs, //L
            double[][] trans, //状态转移矩阵
            double[][] lastParams,//X0上历元的状态向量
            double[][] inverseWeight_obs,//Q 本次观测噪声权逆阵
            double[][] lastInverseWeight,//Q0上历元的权逆阵
            double[][] inverseWeight_model //动力模型噪声权逆阵
            )
        {
            //0表示上一个，1表示预测，无数字表示当次， kk0表示转移逆阵
            ArrayMatrix A, AT, D_o, D_m, L, D0, X0, Trans;//
            //Matrix N, U;//middle
            A = new ArrayMatrix(coeff);//误差方程系数阵
            AT = A.Transpose();//A 的转置
            L = new ArrayMatrix(obs);//观测值，或 观测值 - 估计值
            D_o = new ArrayMatrix(inverseWeight_obs);//观测噪声权逆阵
            D_m = new ArrayMatrix(inverseWeight_model);//动态噪声权逆阵
            D0 = new ArrayMatrix(lastInverseWeight);//上一次估计误差方差矩阵
            X0 = new ArrayMatrix(lastParams); //上一次估值
            Trans = new ArrayMatrix(trans);//状态转移矩阵


            //状态一步预测
            ArrayMatrix X1 = Trans * X0;
            //一步预测误差方差矩阵
            ArrayMatrix D1 = Trans * D0 * Trans.Transpose() + D_m;
            //计算增益矩阵J
            ArrayMatrix J = X1 * AT * (A * D1 * AT + D_o).Inverse;
            //状态估计
            ArrayMatrix X = X1 + J * (L - A * X1);
            //状态估计误差方差矩阵
            ArrayMatrix D = D1 - J * A * D1;
        } 
        #endregion

        #region 属性
        /// <summary>
        /// 验后观测残差  V = L - A * X;
        /// </summary>
        public Vector PostfitObservation { get; protected set; }
  
      
        #endregion

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //if (Transfer != null)
            //{
            //    sb.AppendLine("TransferMatrix");
            //    sb.AppendLine(MatrixUtil.GetFormatedText(Transfer.Array));
            //}
            //if (InverseWeightOfTransfer != null)
            //{
            //    sb.AppendLine("InverseWeightOfDynamics");
            //    sb.AppendLine(MatrixUtil.GetFormatedText(InverseWeightOfTransfer.Array));
            //}

            return base.ToString() + sb.ToString();
        }


        #region 测试

        /// <summary>
        /// 测试卡尔曼滤波。
        /// </summary>
        public static void Test()
        {
            int len = 100;
            Random random = new Random();

            double[] x = new double[len];//状态向量，非随机参数
            double[] y = new double[len];//随机参数
            double[] L = new double[len];//观测向量
            double[] modelNoise = new double[len];//模型噪声
            double[] obsNoise = new double[len];//观测噪声
            for (int i = 0; i < len; i++)
            {
                x[i] = i;
                y[i] = x[i];
                modelNoise[i] = 10.0 * (random.NextDouble() - 0.5);
                obsNoise[i] = 10.0 * (random.NextDouble() - 0.5);
                L[i] = y[i] + obsNoise[i];
            }
            double lastParam = 0.5;//初始化上次参数
            double lastCova = 1;//上次协方差
            double cova_obs = 2;//观测值方差
            double cova_model = 2;//模型方差


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("X \t Y \t Obs \t Pre \t Est");
            for (int i = 1; i < len; i++)
            {
                double[] l = new double[1];
                l[0] = L[i];
                double trans = (i + 1.0) / i;
                //SimplKalmanFilter kf = new KalmanFilter( L[i],1, trans, lastParam, lastCova, cova_obs, cova_model);
                //kf.Process();
                //lastParam = kf.Corrected[0];
                //lastCova = kf.Corrected.InverseWeight[0,0];

                //sb.AppendLine(x[i] + "\t" + y[i] + "\t" + L[i].ToString("0.0000") + "\t" + kf.Predicted[0].ToString("0.0000") + "\t" + kf.Corrected[0].ToString("0.0000"));
            }

            System.IO.File.WriteAllText("d:\\KalmanResult.txt", sb.ToString());
        }

        /// <summary>
        /// 在 GNSS 动态导航中的应用
        /// </summary>
        protected void Motion()
        {
            List<string> paramNames = new List<string>() { "X", "Y", "Z", "dX", "dY", "dZ" };
            ///参数
            List<Vector> satXyzs = new List<Vector>();
            Vector siteXyz = new Vector(3);
            Vector L = new Vector(6); //观测值
            double interval = 30;//时间间隔（单位：秒）


            //将瞬时加速度作为随机干扰，
            //状态向量， 位置和速度 X = trans([x, y, z, vx, vy, vz])
            //噪声向量， 为加速度 Noise = trans(ax, ay, az])
            //纯量形式， y = y0 + t * v0 + 0.5 * a0 * a0 * t
            //           v =          v0 +            a0 * t
            /// 状态方程
            ///         ┌I Δt┐          ┌0.5*Δt*Δt┐
            ///  X(k) = │     │X(k-1) +  │           │Ω(k-1)
            ///         └0  I ┘          └    Δt    ┘
            ///  观测方程
            ///  L(k) = A(k) X(k) + Ω(k)
            ///  其中，L(k)为 k 时刻系统的 C/A 码观测向量

            //常用量
            double[][] identity3x3 = MatrixUtil.CreateIdentity(3);
            int satCount = satXyzs.Count;

            //状态方程状态转移矩阵 Φ（k,k-1）
            //  double[][] trans = MatrixUtil.CreateIdentity(6);
            ArrayMatrix trans = new ArrayMatrix(6, 6);
            MatrixUtil.SetSubMatrix(trans.Array, MatrixUtil.GetMultiply(identity3x3, interval), 0, 3);
            //状态方程噪声向量Ω(k-1)的系数阵
            double[][] coeef_noise = MatrixUtil.Create(6, 3);
            MatrixUtil.SetSubMatrix(coeef_noise, MatrixUtil.GetMultiply(identity3x3, 0.5 * interval * interval));
            MatrixUtil.SetSubMatrix(coeef_noise, MatrixUtil.GetMultiply(identity3x3, interval), 3);

            //观测方程系数矩阵 A,每个时刻不同
            ArrayMatrix coeef_obs = new ArrayMatrix(satCount, 6);
            for (int i = 0; i < satCount; i++)
            {
                IVector differXyz = (satXyzs[i] - siteXyz);
                coeef_obs[i, 0] = differXyz.GetCos(0);//.CosX;
                coeef_obs[i, 1] = differXyz.GetCos(1);
                coeef_obs[i, 2] = differXyz.GetCos(2);
            }
            KalmanFilter lastKf = null;

            //Vector lastParams = lastKf.Corrected ?? null;
            //IMatrix lastParamCova = lastKf.Corrected.InverseWeight ?? null;

            //KalmanFilter kf = new KalmanFilter(coeef_obs, L, trans, lastParams, lastParamCova);
            //kf.Process();
        }

        #endregion

    }
}
