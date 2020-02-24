//201X, cy, translate from gpstk in zz
//2014.09.09, czs, edit, 继承自 Geo.Algorithm.Adjust.Adjustment 
//2016.10.01, double, edit in hongqing, 代码梳理
//2016.10.20-21, czs & double, edit in hongqing, 所有的权逆阵都改为对称阵，以减少存储和加快计算速度,代码整理。
//2017.06.15, cy & czs, edit in chongqing & hongqing, 修改Kalman滤波实现方式 
//2017.07.19, czs, edit in hongqing, 采用Matrix矩阵，简化编写，提高可视化效果
//2018.03.24, czs, edit in hmx, 按照新架构重构 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Geo.Utils;
using Geo.Algorithm.Adjust;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// Kalman 滤波计算器，包含预报和估计两部分。
    /// 参见介绍 by Welch, G. and G. Bishop.
    /// "An Introduction to the Kalman IsSatisfied", at:
    /// http://www.cs.unc.edu/~welch/kalman/kalmanIntro.html.
    /// 此版本修改自 G. J. Bierman. "Factorization Methods for
    /// Discrete Sequential Estimation". Mathematics in Science and
    /// Engineering, Vol. 128. Academic Press, New York, 1977. 
    /// 其具有更好的稳定性。
    /// </summary>
    public class SimpleKalmanFilter : Geo.Algorithm.Adjust.MatrixAdjuster
    {
        /// <summary>
        /// 对应的方法为 Run
        /// </summary>
        public SimpleKalmanFilter()
        {
        }


        #region 属性

        double VarianceOfUnitWeight { get; set; }
        double vtpv { get; set; }
        /// <summary>
        /// 观测量的叠加
        /// </summary>
        public int SumOfObsCount { get; set; }
        int Freedom;
        
        #region 计算属性
        /// <summary>
        /// 先验值。  A priori state estimation.
        /// </summary>
        public IMatrix PredictParam { get { return (Predicted); } }
        /// <summary>
        ///先验值方差。  A priori error covariance.
        /// </summary>
        public IMatrix CovaOfPredictParam { get { return (Predicted.InverseWeight); } }
        #endregion

        #region 需设置
        /// <summary>
        /// 先验参数矩阵
        /// </summary>
        public IMatrix AprioriParam { get { return (this.ObsMatrix.Apriori); } }
        /// <summary>
        /// 先验值的协方差矩阵
        /// </summary>
        public IMatrix CovaOfAprioriParam { get { return (ObsMatrix.Apriori.InverseWeight); } }
        /// <summary>
        /// 参数状态转移的权逆阵
        /// </summary>
        public IMatrix InverseWeightOfTransfer { get { return ObsMatrix.Transfer.InverseWeight; } }

        #endregion
        #endregion

        /// <summary>
        /// 计算
        /// </summary>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            this.SumOfObsCount += input.ObsCount;
            int paramCount = input.Coefficient.ColCount;
       //   this.Freedom = input.ObsCount - paramCount;// input.Freedom;
           this.Freedom = SumOfObsCount - paramCount;// input.Freedom;
            //try
            //{
            this.ObsMatrix = input;
            Predict(input.Transfer, InverseWeightOfTransfer);

            var est1 = CorrectSimple(input.Observation-input.FreeVector, input.Coefficient, input.Observation.InverseWeight);
            // var est2 = CorrectNormal(this.Observation, CoeffOfParam, Observation.InverseWeight);
            // var est3 = NewCorrect(this.Observation, CoeffOfParam, Observation.InverseWeight);

            //var differ12 = est1 - est2;
            //var differ23 = est2 - est3;
            //var differ13 = est1 - est3;

            var Estimated = est1;

            //}
            //catch (Exception ex)
            //{
            //    log.Error("SimpleKalmanFilter 滤波出错了", ex);
            //} 


            AdjustResultMatrix result = new AdjustResultMatrix()
                .SetEstimated(Estimated)
                .SetObsMatrix(input)
                .SetFreedom(Freedom)
                .SetVarianceFactor(VarianceOfUnitWeight)
               .SetVtpv(vtpv)
                ;
            return result;
        }

        #region 私有定义
        /// <summary>
        /// 预报
        /// </summary>
        /// <param name="TransferMatrix"></param>
        /// <param name="InverseWeightOfTransfer"></param>
        private void Predict(IMatrix TransferMatrix, IMatrix InverseWeightOfTransfer)
        {
            //  int stateRow = AprioriParam.RowCount;
            //这两个矩阵的作用是什么？？？？ 2014.12.14,czs, namu shaungliao,
            //IMatrix controlMatrix = new Matrix(stateRow, 1, 0.0);
            //IMatrix controlInputVector = new Matrix(1, 1, 0.0);

            // Compute the a priori state vector
            IMatrix PredictParam = TransferMatrix.Multiply(AprioriParam);//.Plus(controlMatrix.Multiply(controlInputVector));

            //IMatrix tranposOfTranfer = TransferMatrix.Transposition;
            // Compute the a priori estimate error covariance matrix
            //  IMatrix CovaOfPredictParam1 = TransferMatrix.Multiply(CovaOfAprioriParam).Multiply(tranposOfTranfer).Plus(InverseWeightOfTransfer);
            //此处解算结果完全一致，但是 CovaOfPredictParam1 计算的是 对角阵，BQBT 计算的是三角阵。BQBT该判断是否对角阵后计算，更能体现优势。
            //应该进一步测试比较内存消耗和执行效率，最后决定采用方法。??? 2016.10.21, czs noted.       
            IMatrix CovaOfPredictParam1 = AdjustmentUtil.BQBT(TransferMatrix, CovaOfAprioriParam).Plus(InverseWeightOfTransfer);

            //IMatrix q = CovaOfPredictParam1.Minus(CovaOfPredictParam);
            this.Predicted = new Geo.Algorithm.Adjust.WeightedVector(PredictParam, CovaOfPredictParam1) { ParamNames = ObsMatrix.ParamNames };
        }
        WeightedVector Predicted;
        /// <summary>
        ///  估计，改正。通常采用的方法。From 崔阳、2017.06.22
        /// </summary>
        /// <param name="observation"></param>
        /// <param name="control"></param>
        /// <param name="covaOfObs"></param>
        /// <returns></returns>
        private WeightedVector NewCorrect(IMatrix observation, IMatrix control, IMatrix covaOfObs)
        {

            Matrix Q1 = new Matrix(CovaOfPredictParam);
            Matrix P1 = new Matrix(CovaOfPredictParam.GetInverse());
            Matrix X1 = new Matrix(PredictParam);
            Matrix L = new Matrix(observation);
            Matrix A = new Matrix(control);
            Matrix AT = new Matrix(A.Transposition);
            Matrix Po = new Matrix(covaOfObs.GetInverse());
            Matrix Qo = new Matrix(covaOfObs);

            //计算新息向量
            Matrix Vk1 = A * X1 - L;
            Matrix PXk = null;

            if (Q1.IsDiagonal)
            {
                var atpa = new Matrix(AdjustmentUtil.ATPA(AT, Q1));
                //IMatrix at = AT.Multiply(P_o).Multiply(control);
                PXk = new Matrix(atpa + Qo);
            }
            else
            {
                PXk = A * Q1 * AT + Qo;//平差值Xk的权阵
            }

            //计算平差值的权逆阵
            Matrix CovaOfP = PXk.Inversion;//.GetInverse();

            //计算增益矩阵
            //IMatrix J = Q1.Multiply(AT).Multiply(CovaOfP);
            Matrix J = Q1 * AT * CovaOfP;
            //计算平差值
            //IMatrix X = PredictParam.Minus(J.Multiply(Vk1));
            Matrix X = X1 - J * Vk1;


            Matrix I = Matrix.CreateIdentity(Q1.ColCount);

            #region 理论公式
            Matrix t2 = J * A;// (J.Multiply(A));
            Matrix t3 = I - t2;// I.Minus(t2);
            Matrix Qx = t3 * Q1;// (t3).Multiply(Q1);
            #endregion

            #region 阮论文公式
            //IMatrix t21 = I.Minus(J.Multiply(control));
            //IMatrix t22 = I.Minus(controlT.Multiply(J.Transposition));
            //IMatrix t3 = J.Multiply(covaOfObs.Multiply(J.Transposition));
            //IMatrix CovaOfEstParam = ((t21).Multiply(CovaOfPredictParam).Multiply(t22)).Plus(t3);
            #endregion

            var Estimated = new WeightedVector(X, Qx) { ParamNames = ObsMatrix.ParamNames };

            BuildCovaFactor(L, A, Po, P1, X1, X);

            return Estimated;
        }

        /// <summary>
        /// 估计，改正。通常采用的方法。
        /// </summary>
        /// </summary>
        /// <param name="observation">观测值信息</param>
        /// <param name="control">控制矩阵，有时为非对称阵，如PPP</param>
        /// <param name="covaOfObs">观测值协方差</param>
        private WeightedVector CorrectNormal(IMatrix observation, IMatrix control, IMatrix covaOfObs)
        {
            //简化字母表示
            Matrix Q1 = new Matrix(CovaOfPredictParam);
            Matrix P1 = new Matrix(CovaOfPredictParam.GetInverse());
            Matrix X1 = new Matrix(PredictParam);
            Matrix L = new Matrix(observation);
            Matrix A = new Matrix(control);
            Matrix AT = new Matrix(A.Transposition);
            Matrix Po = new Matrix(covaOfObs.GetInverse());
            Matrix Qo = new Matrix(covaOfObs);

            /*******  Normal method Start ********/
            //计算增益矩阵         
            var temp = (Qo + (A * Q1 * AT)).Inversion;// (Qo.Plus(A.Multiply(Q1).Multiply(AT))).GetInverse();
            var J = Q1 * AT * temp; //Q1.Multiply(AT).Multiply(temp);

            //计算估计值
            Matrix Vk1 = A * X1 - L; //计算新息向量A.Multiply(X1).Minus(L);
            Matrix X = X1 - J * Vk1;//X1.Minus(J.Multiply(Vk1));
            //计算平差值的权逆阵
            var JA = J * A;// J.Multiply(A);
            var temp2 = JA * Q1;//JA.Multiply(Q1);
            var Qx = Q1 - temp2;//Q1.Minus(temp2);
            //var maxtrix = new Matrix(Q1.Array) - new Matrix(temp2.Array);
            //var differ2 = maxtrix.Minus(Qx);
            //var I = DiagonalMatrix.GetIdentity(Q1.ColCount);
            //var Qx2 = I.Minus(JA).Multiply(Q1);            
            //var differ = Qx2 .Minus(Qx);
            /*******  Normal method End ********/
            var Estimated = new WeightedVector(X, Qx) { ParamNames = ObsMatrix.ParamNames };

            //观测残差
            BuildCovaFactor(L, A, Po, P1, X1, X);
            return Estimated;
        }


        /// <summary>
        /// 估计，改正。一个更健壮的方法。
        /// </summary>
        /// <param name="observation">观测值信息</param>
        /// <param name="control">控制矩阵，有时为非对称阵，如PPP</param>
        /// <param name="covaOfObs">观测值协方差</param>
        private WeightedVector CorrectSimple(IMatrix observation, IMatrix control, IMatrix covaOfObs)
        {
            //简化字母表示
            Matrix Q1 = new Matrix(CovaOfPredictParam);
            Matrix P1 = new Matrix(CovaOfPredictParam.GetInverse());
            Matrix X1 = new Matrix(PredictParam);
            Matrix L = new Matrix(observation);
            Matrix A = new Matrix(control);
            Matrix AT = new Matrix(A.Transposition);
            Matrix Po = new Matrix(covaOfObs.GetInverse());
            Matrix Qo = new Matrix(covaOfObs);

            //平差值Xk的权阵
            Matrix PXk = null;
            Matrix Atpa = null;
            if (Po.IsDiagonal) { Atpa = ATPA(A, Po); }
            else { Atpa = AT * Po * A; }

            PXk = new Matrix(SymmetricMatrix.Parse(Atpa)) + P1;
                        
            //计算平差值的权逆阵
            Matrix Qx = PXk.Inversion;
            Matrix J = Qx * AT * Po;

            //计算平差值
            Matrix Vk1 = A * X1 - L;//计算新息向量
            Matrix X = X1 - J * Vk1;
            X.RowNames = ObsMatrix.ParamNames;


            BuildCovaFactor(L, A, Po, P1, X1, X);

            var Estimated = new WeightedVector(X, Qx) { ParamNames = ObsMatrix.ParamNames };
            
            return Estimated;
        }

        private void BuildCovaFactor(Matrix L, Matrix A, Matrix Po, Matrix P1, Matrix X1, Matrix X)
        {
            //Matrix V = A * X - L;
            //Matrix Vx = X - X1;
            //Matrix VTPV = null;
            //if (Po.IsDiagonal) { VTPV = ATPA(V, Po) + (Vx.Trans * P1 * Vx); }
            //else { VTPV = V.Trans * Po * V + (Vx.Trans * P1 * Vx); }

            //this.VarianceOfUnitWeight = Math.Abs(VTPV[0, 0] / A.RowCount);

            Matrix V = A * X - L;
            Matrix Vx = X - X1;
            Matrix VTPV = null;
            if (Po.IsDiagonal) { VTPV = ATPA(V, Po) + (Vx.Trans * P1 * Vx); }
            else { VTPV = V.Trans * Po * V + (Vx.Trans * P1 * Vx); }

            vtpv = VTPV[0, 0];


            VarianceOfUnitWeight = Math.Abs(vtpv / this.Freedom);
        }



        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (ObsMatrix.Transfer != null)
            {
                sb.AppendLine("TransferMatrix");
                sb.AppendLine(MatrixUtil.GetFormatedText(ObsMatrix.Transfer.Array));
            }
            if (InverseWeightOfTransfer != null)
            {
                sb.AppendLine("InverseWeightOfDynamics");
                sb.AppendLine(MatrixUtil.GetFormatedText(InverseWeightOfTransfer.Array));
            }

            return base.ToString() + sb.ToString();
        }

        /// <summary>
        /// 将状态转移模型数值填充。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Geo.RmsedNumeral> GetTransferModelDic()
        {
            Dictionary<string, Geo.RmsedNumeral> dic = new Dictionary<string, RmsedNumeral>();
            int i = 0;
            foreach (var item in ObsMatrix.ParamNames)
            {
                dic.Add(item, new RmsedNumeral(ObsMatrix.Transfer[i, i], Math.Sqrt(InverseWeightOfTransfer[i, i])));
                i++;
            }
            return dic;
        }


    }
}
