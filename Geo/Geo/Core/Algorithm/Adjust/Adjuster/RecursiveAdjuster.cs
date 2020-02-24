//2018.04.16, czs,   created in hmx, 递归最小二乘法 
//2018.10.20, czs,  edit in hmx, 重新进行了设计，分阶段计算

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
    /// 递归最小二乘所处的阶段或类型
    /// </summary>
    public enum StepOfRecursive
    {
        /// <summary>
        /// 不变参数法方程叠加
        /// </summary>
        SuperposOfConstNeq = 0,
        /// <summary>
        /// 不变参数的序贯解法
        /// </summary>
        SequentialConst,
        /// <summary>
        ///计算易变参数
        /// </summary>
        //ComputeMutableParam,
        /// <summary>
        /// 实时估计和计算
        /// </summary>
        RealTime,
        /// <summary>
        /// 简化为普通参数平差
        /// </summary>
        ParamAdjust,

    }
    /// <summary>
    /// 递归最小二乘法，适合于导航计算。 
    /// 需要一定的积累数据，计算固定参数，然后再一边修正固定参数，一边输出易变（状态）参数。
    /// 为了保证能用，第一个也输出，只不过精度有限。
    /// </summary>
    public class RecursiveAdjuster : MatrixAdjuster
    {
        #region 构造函数         
        /// <summary>
        /// 构造函数
        /// </summary>
        public RecursiveAdjuster() {
            NormalEquationSuperposer = new NormalEquationSuperposer();
            MatrixAdjuster = new KalmanFilter();
            this.StepOfRecursive = StepOfRecursive.SuperposOfConstNeq;
        }
        #endregion

        #region 核心计算方法
        /// <summary>
        /// 迭代阶段
        /// </summary>
        public StepOfRecursive StepOfRecursive { get; set; }
        /// <summary>
        /// 法方程叠加器
        /// </summary>
        public NormalEquationSuperposer NormalEquationSuperposer { get; set; }
        /// <summary>
        /// 设置递归最小二乘法的阶段或类型。
        /// </summary>
        /// <param name="StepOfRecursive"></param>
        /// <returns></returns>
        public RecursiveAdjuster SetStepOfRecursive(StepOfRecursive StepOfRecursive)
        {
            this.StepOfRecursive = StepOfRecursive; return this;
        }
        /// <summary>
        /// 矩阵算法
        /// </summary>
        MatrixAdjuster MatrixAdjuster { get; set; }
        /// <summary>
        /// 最后一个结果
        /// </summary>
        public AdjustResultMatrix LastResult { get; set; }
        /// <summary>
        /// 上一个固定结果
        /// </summary>
        public AdjustResultMatrix LastConstResult { get; set; }

        /// <summary>
        /// 批量总体技术。参数不要改变，否则达不到预期效果。
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public List<AdjustResultMatrix> Run(List<AdjustObsMatrix> inputs)
        {
            AdjustObsMatrix firstMatrix = inputs[0];
            Matrix Y0all = firstMatrix.HasSecondApprox ? new Matrix(firstMatrix.SecondApproxVector, true) : null;
            WeightedVector estY = GetConstY(inputs);

            //Matrix Y = Y0all + constY;
            //step 3：求易变参数
            List<AdjustResultMatrix> results = new List<AdjustResultMatrix>();
            foreach (var obsMatrix in inputs)
            {
                this.ObsMatrix = obsMatrix;

                AdjustResultMatrix result = Step3GetMutableX(estY, obsMatrix);

                results.Add(result);
            }
            return results;
        }
        /// <summary>
        /// 实时计算，数据计算
        /// </summary>
        /// <param name="input">观测矩阵</param>
        /// <returns></returns>
        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {
            if (input == this.ObsMatrix) { return this.LastResult; }
            this.ObsMatrix = input;

            switch (StepOfRecursive)
            {
                case StepOfRecursive.SuperposOfConstNeq:
                    return GetConstParamResult(input, NormalEquationSuperposer);
                //case StepOfRecursive.ComputeMutableParam:  break;
                case StepOfRecursive.SequentialConst:
                    AdjustResultMatrix res = GetSequentialConst(input);
                    this.LastConstResult = res;
                    return res;
                case StepOfRecursive.RealTime://实时计算，参数变化可能带来错误
                    AdjustResultMatrix result = GetRealTimeResult(input);
                    this.LastResult = result;
                    return result;
                case StepOfRecursive.ParamAdjust://参数逐历元平差 
                    return GetSimpleParamAdjustResult(input);//参数平差验证,2018.10.15, czs, hmx, 验证第一个结果是一样的。
                default:
                    break;
            }
            return null;
        }

        private AdjustResultMatrix GetRealTimeResult(AdjustObsMatrix input)
        {
            //构建不变参数的法方程
            var newConstParamNe = input.BuildConstParamNormalEquation();
            NormalEquationSuperposer.Add(newConstParamNe);//添加到法方程迭加器中
            WeightedVector estY = NormalEquationSuperposer.GetEstimated();
            AdjustResultMatrix result = Step3GetMutableX(estY, ObsMatrix);//求异变参数
            return result;
        }

        /// <summary>
        /// Kalman滤波计算不变参数。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private AdjustResultMatrix GetSequentialConst(AdjustObsMatrix input)
        {
            WeightedVector appri = null;
            Matrix B2 = input.BuildCoeefOfConstParam();
            if (LastConstResult != null)
            {
                var IsEqual = Geo.Utils.ListUtil.IsEqual(LastConstResult.ParamNames, input.SecondParamNames);
                if (IsEqual)
                {
                    appri = LastConstResult.Estimated;
                }
                else
                {
                    appri = SimpleAdjustMatrixBuilder.GetNewWeighedVectorInOrder(input.SecondParamNames, LastConstResult.Estimated);
                }
            }
            if (appri == null)//第一次，使用参数平差结果
            {
                ParamAdjuster paramAdjuster = new ParamAdjuster();
                var paramResult = paramAdjuster.Run(new AdjustObsMatrix(input.Observation, B2, null, input.SecondParamNames));
                appri = paramResult.Estimated;
            }

            AdjustObsMatrix obsMatrix1 = new AdjustObsMatrix(appri, input.Observation, B2, input.SecondTransfer);
            obsMatrix1.ParamNames = input.SecondParamNames;

            //var kalmanFilter = new SimpleKalmanFilter();

            var res = MatrixAdjuster.Run(obsMatrix1);
            return res;
        }

        /// <summary>
        /// 恢复参数平差。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static AdjustResultMatrix GetSimpleParamAdjustResult(AdjustObsMatrix input)
        {
            #region 合并系数阵
            Matrix A = new Matrix(input.Coefficient.RowCount, input.Coefficient.ColCount + input.SecondCoefficient.ColCount);
            A.SetSub(input.Coefficient);
            A.SetSub(input.SecondCoefficient, 0, input.Coefficient.ColCount);
            var names = new List<string>();
            names.AddRange(input.ParamNames);
            names.AddRange(input.SecondParamNames);
            #endregion

            ParamAdjuster paramAdjuster = new ParamAdjuster();
            AdjustObsMatrix adjustObs = new AdjustObsMatrix(input.Observation, A, null, names);
            var resultMatrix = paramAdjuster.Run(adjustObs);
            return resultMatrix;
        }

        #endregion


        #region 计算细节
        /// <summary>
        /// 第三步：计算易变参数
        /// </summary>
        /// <param name="estY"></param>
        /// <param name="obsMatrix"></param>
        /// <returns></returns>
        private AdjustResultMatrix Step3GetMutableX(WeightedVector estY, AdjustObsMatrix obsMatrix)
        {
            Matrix constY = estY.GetVectorMatrix();
            Matrix constQy = estY.InverseWeight;

            //观测值权阵设置,对已知量赋值 
            Matrix L = new Matrix((IMatrix)obsMatrix.Observation);
            Matrix QL = new Matrix(obsMatrix.Observation.InverseWeight);
            Matrix PL = new Matrix(QL.GetInverse());
            Matrix A = new Matrix(obsMatrix.Coefficient);
            Matrix AT = A.Trans;
            Matrix B = new Matrix(obsMatrix.SecondCoefficient);
            Matrix BT = B.Trans;
            Matrix X0 = obsMatrix.HasApprox ? new Matrix(obsMatrix.ApproxVector, true) : null;
            Matrix Y0 = obsMatrix.HasSecondApprox ? new Matrix(obsMatrix.SecondApproxVector, true) : null;
            Matrix D = obsMatrix.HasFreeVector ? new Matrix(obsMatrix.FreeVector, true) : null;
            int obsCount = L.RowCount;
            int fixedParamCount = B.ColCount;
            int mutableParamCount = A.ColCount;
            int paramCount = fixedParamCount + mutableParamCount;
            int freedom = obsCount - paramCount;

            Matrix lxy = L - (A * X0 + B * Y0 + D); //采用估值计算的观测值小量
            Matrix ATPL = AT * PL;
            //法方程
            Matrix Na = ATPL * A;
            Matrix Nab = AT * PL * B;
            Matrix InverNa = Na.Inversion;

            //求x
            //观测值更新,采用估值进行计算
            Matrix lx = lxy - B * constY;// = L - (A * X0 + B * Y + D);
                                         /*  Matrix x = InverNa * ATPL * lx;*/
            Matrix x = InverNa * (ATPL * lxy - Nab * constY);//这两个计算是等价的
            Matrix X = X0 + x;

            Matrix Ntmp = Na.Inversion * Nab;
            Matrix Qx = InverNa + Ntmp * constQy * Ntmp.Trans;

            //Matrix Qxy = AT * PL * B;
            //Matrix Qtemp = Qx * Qxy; 
            //Matrix Dx = Qx + Qtemp * Dy * Qtemp.Trans;

            //精度评定
            Matrix V = A * x - lx;
            Matrix Qv = QL - A * Qx * AT - B * constQy * BT;

            // Matrix lT = l.Trans;

            double vtpv = (V.Trans * PL * V).FirstValue;//(lT * PL * l - lT * PL * Ac * y).FirstValue;// 

            double s0 = vtpv / (freedom == 0 ? 0.1 : freedom);//单位权方差  

            WeightedVector estX = new WeightedVector(x, Qx) { ParamNames = obsMatrix.ParamNames };
            WeightedVector CorrectedEstimate = new WeightedVector(X, Qx) { ParamNames = obsMatrix.ParamNames };
            WeightedVector estV = new WeightedVector(V, Qv) { ParamNames = obsMatrix.Observation.ParamNames };

            Matrix Lhat = L + V;
            Matrix QLhat = A * Qx * AT;
            var correctedObs = new WeightedVector(Lhat, QLhat) { ParamNames = this.ObsMatrix.Observation.ParamNames };


            if (!DoubleUtil.IsValid(s0))
            {
                log.Error("方差值无效！" + s0);
            }

            AdjustResultMatrix result = new AdjustResultMatrix()
                .SetAdjustmentType(AdjustmentType.递归最小二乘)
                .SetEstimated(estX)
                .SetSecondEstimated(estY)
                .SetCorrection(estV)
                .SetCorrectedObs(correctedObs)
                .SetCorrectedEstimate(CorrectedEstimate)
                .SetObsMatrix(obsMatrix)
                .SetFreedom(freedom)
                .SetVarianceFactor(s0)
                .SetVtpv(vtpv);
            return result;
        }

        private static WeightedVector GetConstY(List<AdjustObsMatrix> inputs)
        {
           var normals = Setp1ComposeConstNormaEqualtion(inputs);

            WeightedVector estY = Step2GetConstY(normals);
            return estY;
        }

        private static WeightedVector Step2GetConstY(List<MatrixEquation> normals)
        {
            NormalEquationSuperposer NormalEquationSuperPoser = new NormalEquationSuperposer();

            //step 2： 求固定参数  
            foreach (var normal in normals)
            {
                NormalEquationSuperPoser.Add(normal); 
            }

            return NormalEquationSuperPoser.GetEstimated();
        }

        /// <summary>
        /// 第一步：组固定参数的方法方程
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private static List<MatrixEquation> Setp1ComposeConstNormaEqualtion(List<AdjustObsMatrix> inputs)
        {
            List<MatrixEquation> normals = new List<MatrixEquation>();
            //step 1： 分离易变参数
            foreach (var obsMatrix in inputs)
            {
                MatrixEquation ne = obsMatrix.BuildConstParamNormalEquation();
                normals.Add(ne);
            }
            return normals;
        }
         

        /// <summary>
        /// 构建不变参数的计算结果。
        /// 注意：需要控制参数的增减问题。
        /// 基本思路：增加则插入，减少则删除，通过参数名称来控制。
        /// </summary>
        /// <param name="obsMatrix"></param>
        /// <param name="NormalEquationSuperposer"></param>
        /// <returns></returns>
        public static AdjustResultMatrix GetConstParamResult (AdjustObsMatrix obsMatrix, NormalEquationSuperposer NormalEquationSuperposer)
        {
            //观测值权阵设置,对已知量赋值 
            Matrix L = new Matrix((IMatrix)obsMatrix.Observation);
            Matrix QL = new Matrix(obsMatrix.Observation.InverseWeight);
            Matrix PL = new Matrix(QL.GetInverse());
            Matrix A = new Matrix(obsMatrix.Coefficient);
            Matrix AT = A.Trans;
            Matrix B = new Matrix(obsMatrix.SecondCoefficient);
            Matrix BT = B.Trans;
            Matrix X0 = obsMatrix.HasApprox ? new Matrix(obsMatrix.ApproxVector, true) : null;
            Matrix Y0 = obsMatrix.HasSecondApprox ? new Matrix(obsMatrix.SecondApproxVector, true) : null;
            Matrix D = obsMatrix.HasFreeVector ? new Matrix(obsMatrix.FreeVector, true) : null;
            int obsCount = L.RowCount;
            int fixedParamCount = obsMatrix.SecondParamNames.Count;// B.ColCount;
            int freedom = obsCount - fixedParamCount;

            //观测值更新
            Matrix lxy = L - (A * X0 + B * Y0 + D); //采用估值计算的观测值小量

            Matrix ATPL = AT * PL;
            //法方程
            Matrix Na = ATPL * A;
            Matrix Nab = AT * PL * B;
            Matrix InverNa = Na.Inversion;
            Matrix J = A * InverNa * AT * PL;
            Matrix I = Matrix.CreateIdentity(J.RowCount);
            Matrix B2 = (I - J) * B; //新的系数阵 Ac, 原文中为 B波浪~
            Matrix B2T = B2.Trans;
            Matrix B2TPL = B2T * PL;
            Matrix NofB2 = B2TPL * B2;
            Matrix UofB2 = B2TPL * lxy;
            NofB2.ColNames = obsMatrix.SecondParamNames;
            NofB2.RowNames = obsMatrix.SecondParamNames;

            UofB2.RowNames = obsMatrix.SecondParamNames;
            UofB2.ColNames = new List<string>() { "ConstParam" };

            //生成法方程
            var ne = new MatrixEquation(NofB2, UofB2);
            //叠加法方程
            NormalEquationSuperposer.Add(ne);//添加到法方程迭加器中

            var acNe = NormalEquationSuperposer.CurrentAccumulated;

            Matrix inverN = acNe.N.Inversion;
            Matrix y = inverN * acNe.U;
            y.RowNames = acNe.ParamNames;
            Matrix Qy = inverN;
            Qy.ColNames = acNe.ParamNames;
            Qy.RowNames = acNe.ParamNames;
            var estY = new WeightedVector(y, Qy) { ParamNames = acNe.ParamNames };

            var V = B2 * y - lxy;
            Matrix Qv = QL - B2 * Qy * B2T;
            Matrix Y = Y0 + y;
            var vtpv = (V.Trans * PL * V).FirstValue; 
            double s0 = vtpv / (freedom == 0 ? 0.1 : freedom);//单位权方差  

            WeightedVector CorrectedEstimate = new WeightedVector(Y, Qy) ;

            WeightedVector estV = new WeightedVector(V, Qv) { ParamNames = obsMatrix.Observation.ParamNames };
            Matrix Lhat = L + V;
            Matrix QLhat = B2 * Qy * B2T;
            var correctedObs = new WeightedVector(Lhat, QLhat) { ParamNames = obsMatrix.Observation.ParamNames };
            
            AdjustResultMatrix result = new AdjustResultMatrix()
           .SetAdjustmentType(AdjustmentType.递归最小二乘)
           .SetEstimated(estY)
           .SetCorrection(estV)
           .SetCorrectedObs(correctedObs)
           .SetCorrectedEstimate(CorrectedEstimate)
           .SetObsMatrix(obsMatrix)
           .SetFreedom(freedom)
           .SetVarianceFactor(s0)
           .SetVtpv(vtpv);
            return result; 
        }
        #endregion

    }
}
