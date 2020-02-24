//2017.09.12， cy, create in 重庆， 首写平方根信息滤波算法 
//2018.03.24, czs, edit in hmx, 按照新架构重构 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 平方根信息滤波SRIF
    /// Square Root Information Filter最早由JPL提出，是卡尔曼滤波的一个演化版本，具有数值精度高、稳定性强等特点。
    /// SRIF以Householder正交变换为基础实现滤波递推计算的观测更新和时间更新。
    /// 目前Panda、GIPSY等软件均采用SRIF算法。
    /// </summary>
    public class SquareRootInformationFilter : Geo.Algorithm.Adjust.MatrixAdjuster
    {
        /// <summary>
        /// 对应的方法为 Process
        /// </summary>
        public SquareRootInformationFilter()
        {
        }

        int Freedom;
        double vtpv;
        #region 属性
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
        public IMatrix AprioriParam { get { return (ObsMatrix.Apriori); } }
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
        /// <param name="input"></param>
        /// <returns></returns>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            //try
            //{
            this.ObsMatrix = input;
            int paramCount = input.Coefficient.ColCount;
            Predict(input.Transfer, InverseWeightOfTransfer);

            var est1 = CorrectSimple(input.Observation- input.FreeVector, input.Coefficient, input.Observation.InverseWeight);
            // var est2 = CorrectNormal(this.Observation, CoeffOfParam, Observation.InverseWeight);
            // var est3 = NewCorrect(this.Observation, CoeffOfParam, Observation.InverseWeight);

            //var differ12 = est1 - est2;
            //var differ23 = est2 - est3;
            //var differ13 = est1 - est3;

            var Estimated = est1;
            AdjustResultMatrix result = new AdjustResultMatrix()
            .SetEstimated(Estimated)
             .SetObsMatrix(input)
            .SetFreedom(Freedom)
            .SetVarianceFactor(VarianceOfUnitWeight)
            .SetVtpv(vtpv)
            ;

            return result;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("SimpleKalmanFilter 滤波出错了", ex);
            //}
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
            IMatrix CovaOfPredictParam1 = BQBT(TransferMatrix, CovaOfAprioriParam).Plus(InverseWeightOfTransfer);

            //IMatrix q = CovaOfPredictParam1.Minus(CovaOfPredictParam);
            this.Predicted = new Geo.Algorithm.Adjust.WeightedVector(PredictParam, CovaOfPredictParam1) { ParamNames = ObsMatrix.ParamNames };
        }WeightedVector Predicted;
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
                var atpa = ATPA(AT, Q1);
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
            var Estimated = new WeightedVector(X, Qx) { ParamNames = ObsMatrix. ParamNames };

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
            //先验信息
            Matrix Q1 = new Matrix(CovaOfPredictParam);          
            Matrix X1 = new Matrix(PredictParam);

            //观测信息
            Matrix L = new Matrix(observation);
            Matrix B = new Matrix(control);     
            Matrix Qo = new Matrix(covaOfObs);

            Matrix P1 = new Matrix(CovaOfPredictParam.GetInverse());
            Matrix Po = new Matrix(covaOfObs.GetInverse());

            //通过Cholesky分解，单位化
            CholeskyDecomposition Cholesky1 = new CholeskyDecomposition(P1);
            Matrix R0 = new Matrix((Cholesky1.LeftTriangularFactor.Transpose()));//
            Matrix z0 = R0 * X1;

            CholeskyDecomposition Cholesky2 = new CholeskyDecomposition(Po);
            Matrix R = new Matrix((Cholesky2.LeftTriangularFactor.Transpose()));//
            Matrix z = R * L; 
            Matrix A = R * B;


            if(R0.ColCount!=A.ColCount)
            {
                //
                throw new Exception("What is wrong in SquareRootInformationFilter!");
            }
            //合并矩阵
            Matrix ConbRA = ConbineMatrixByCol(R0, A);
            Matrix ConbZ0Z = ConbineMatrixByCol(z0, z);

            //正交化
            HouseholderTransform HouseholderTransform = new HouseholderTransform(ConbRA);
            Matrix T = HouseholderTransform.T;

            //更新
            Matrix newConbRA = T * ConbRA;
            Matrix newConbZ0Z = T * ConbZ0Z;
            //
            //提取
            Matrix newR0 = newConbRA.GetSub(0, 0, R0.RowCount, R0.ColCount);
            Matrix newZ0 = newConbZ0Z.GetSub(0, 0, z0.RowCount, z0.ColCount);

            //解算
            Matrix newR0_Cov = new Matrix(newR0.GetInverse());
            Matrix X = newR0_Cov * newZ0;
            Matrix QX = newR0_Cov * newR0_Cov.Transpose();

            BuildCovaFactor(L, B, Po, P1, X1, X);


            if (false)
            {
                var III = newR0_Cov * newR0_Cov;
                int i = 0; 
                i = 0;
            }

            var Estimated = new WeightedVector(X, QX) { ParamNames = ObsMatrix.ParamNames }; 
            return Estimated;
        }

        /// <summary>
        /// 合并矩阵,将两个矩阵块合并成列块形式，ConbRA（(m1+m2)*n)=[R(m1*n);A(m2*n)]
        /// </summary>
        /// <param name="R"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        private static Matrix ConbineMatrixByCol(Matrix R, Matrix A)
        {
            Matrix ConbRA = new Matrix(R.RowCount + A.RowCount, R.ColCount);
            for (int i = 0; i < R.RowCount; i++)
            {
                for (int j = 0; j < R.ColCount; j++)
                {
                    ConbRA[i, j] = R[i, j];
                }
            }
            for (int i = 0; i < A.RowCount; i++)
            {
                for (int j = 0; j < A.ColCount; j++)
                {
                    ConbRA[i + R.RowCount, j] = A[i, j];
                }
            }
            return ConbRA;
        }

        private void BuildCovaFactor(Matrix L, Matrix A, Matrix Po, Matrix P1, Matrix X1, Matrix X)
        {
            Matrix V = A * X - L;
            Matrix Vx = X - X1;
            Matrix VTPV = null;
            if (Po.IsDiagonal) { VTPV = ATPA(V, Po) + (Vx.Trans * P1 * Vx); }
            else { VTPV = V.Trans * Po * V + (Vx.Trans * P1 * Vx); }
            vtpv = VTPV[0, 0];
            Freedom = A.RowCount - A.ColCount;
            this.VarianceOfUnitWeight = Math.Abs(vtpv / Freedom);
        }
        double VarianceOfUnitWeight;
        #endregion
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //if (input.Transfer != null)
            //{
            //    sb.AppendLine("TransferMatrix");
            //    sb.AppendLine(MatrixUtil.GetFormatedText(input.Transfer.Array));
            //}
            if (InverseWeightOfTransfer != null)
            {
                sb.AppendLine("InverseWeightOfDynamics");
                sb.AppendLine(MatrixUtil.GetFormatedText(InverseWeightOfTransfer.Array));
            }

            return base.ToString() + sb.ToString();
        }

    }
}
