//2018.03.24, czs, extract in HMX, 平差结果矩阵。  
//2018.06.07, czs, edit in hmx, 残差计算增加常量D
//2018.10.20, czs, edit in hmx, 抽取模糊度固定为单独的类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using System.Threading.Tasks;
using Geo.IO;


namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 计算结果类型
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Unknown,
        /// <summary>
        /// 浮点解
        /// </summary>
        Float,
        /// <summary>
        /// 固定解
        /// </summary>
        Fixed
    }

    /// <summary>
    /// 平差结果矩阵。 下标 0 代表先验值， 无代表估计值，1 代表预报值。
    /// </summary>
    public class AdjustResultMatrix : IDisposable, IReadable
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log = Log.GetLog(typeof(AdjustResultMatrix));

        /// <summary>
        /// 平差。构造函数。
        /// </summary>
        public AdjustResultMatrix() { }

        /// <summary>
        /// 平差类型
        /// </summary>
        public AdjustmentType AdjustmentType { get; set; }

        /// <summary>
        ///观测数量
        /// </summary>
        public int ObsCount { get => this.ObsMatrix.ObsCount; }
        
        /// <summary>
        /// 观测矩阵
        /// </summary>
        public AdjustObsMatrix ObsMatrix { get; set; }
        /// <summary>
        /// 参数（未知数）的协方差阵。D = Inverse(Normal) * VarianceOfUnitWeight.
        /// </summary>
        public IMatrix CovaOfSecondEstimatedParam { get { if (SecondEstimated == null) { return null; } return SecondEstimated.GetCovariance(VarianceOfUnitWeight); } }
        /// <summary>
        /// 参数（未知数）的协方差阵。D = Inverse(Normal) * VarianceOfUnitWeight.
        /// </summary>
        public IMatrix CovaOfEstimatedParam { get { if (Estimated == null) { return null; } return Estimated.GetCovariance(VarianceOfUnitWeight); } }
        /// <summary>
        /// 估值向量标准差
        /// </summary>
        public IVector StdOfEstimatedParam
        {
            get
            {
                if (CovaOfEstimatedParam == null) return null;
                Vector vars = new Vector(ParamCount) { ParamNames = ParamNames };
                IMatrix cova = CovaOfEstimatedParam;
                for (int i = 0; i < ParamCount; i++)
                {
                    vars[i] = Math.Sqrt(cova[i, i]);
                }
                return vars;
            }
        }      
        /// <summary>
        /// 估值向量方差
        /// </summary>
        public IVector StdOfSecondEstimatedParam
        {
            get
            {
                if (CovaOfSecondEstimatedParam == null) return null;
                Vector vars = new Vector(SecondParamCount) { ParamNames = SecondParamNames };
                IMatrix cova = CovaOfSecondEstimatedParam;
                for (int i = 0; i < SecondParamCount; i++)
                {
                    vars[i] = Math.Sqrt(cova[i, i]);
                }
                return vars;
            }
        }
        /// <summary>
        /// 参数数量
        /// </summary>
        public int ParamCount { get => ParamNames.Count; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public List<String> ParamNames { get => this.Estimated.ParamNames; }
        /// <summary>
        /// 第二参数数量
        /// </summary>
        public int SecondParamCount { get => this.SecondEstimated==null ? 0 : this.SecondParamNames.Count; }
        /// <summary>
        /// 第二参数名称
        /// </summary>
        public List<String> SecondParamNames { get => this.SecondEstimated == null ?  new List<string>() : this.SecondEstimated.ParamNames; }
        /// <summary>
        /// 改正后的观测值, LHat
        /// </summary>
        public WeightedVector CorrectedObs { get; set; }
        /// <summary>
        /// 验后残差 V = L - A * X + D 
        /// </summary>
        public WeightedVector PostfitResidual
        {
            get
            {
                WeightedVector L = (this.ObsMatrix.Observation);
                IMatrix X = (this.Estimated);
                IMatrix A = (this.ObsMatrix.Coefficient);
                Vector D = (this.ObsMatrix.FreeVector);
                IMatrix Qx = this.Estimated.InverseWeight;

                // L - A * X;
                // IMatrix dL = L.Minus(A.Multiply(X));

                IMatrix dL = A.Multiply(X).Minus(L);
                if(D != null)
                {
                    dL = dL.Plus(new Matrix(D, isColOrRowVector: true));
                }
                dL.RowNames = L.ParamNames;

                IMatrix p = A.Multiply(Qx).Multiply(A.Transposition);

                return new WeightedVector(dL, p) { ParamNames = this.ObsMatrix.Observation.ParamNames };
            }
        }
        #region 属性
        /// <summary>
        /// 计算结果类型
        /// </summary>
        public ResultType ResultType { get; set; }
        /// <summary>
        /// 存储当前的VTPV值
        /// </summary>
        public double Vtpv { get;  set; }
        /// <summary>
        /// 改正数估值 V，参数平差是 V=AX-L，条件平差是 V=P(-1)B(T)K
        /// </summary>
        public WeightedVector Correction { get; set; }
        /// <summary>
        /// 参数预测值及其权倒数、协方差
        /// </summary>
        public WeightedVector Predicted { get;  set; }

        /// <summary>
        /// 估计值，如果具有残差，则直接存储值为先验值或近似值，通过调用 CorrectedVector 调用改正后的值。
        /// 参数估计值及其权倒数、协方差。规定：向量为先验值，改正数为计算偏移量。
        /// </summary>
        public WeightedVector Estimated { get; set; }
        /// <summary>
        /// 第二估计值，如果具有残差，则直接存储值为先验值或近似值，通过调用 CorrectedVector 调用改正后的值。
        /// 参数估计值及其权倒数、协方差。规定：向量为先验值，改正数为计算偏移量。
        /// </summary>
        public WeightedVector SecondEstimated { get; set; }

        /// <summary>
        /// 单位权方差 Aposteriori variance factor.验后方差因子。
        /// variance of unit weight
        /// 方差：随机变量与其数学期望之差的平方的数学期望，称为方差。
        /// </summary>
        public double VarianceOfUnitWeight { get;  set; }

        /// <summary>
        ///   单位权中误差,均方差(Standard deviation )估值。
        ///  方差不可求而中误差可求. 
        ///  中误差：root mean square error; RMSE,也可称为 标准差 或 均方根差？
        ///  单位权中误差： unit weight mean square error.
        /// </summary>
        public double StdDev { get { return Math.Sqrt(VarianceOfUnitWeight); } }
        /// <summary>
        /// 自由度
        /// </summary>
        public int Freedom { get;  set; }
        /// <summary>
        /// 该正后的估值，如果具有初始值，则会由初始值加上估值，是完整的最终结果。
        /// </summary>
        public WeightedVector CorrectedEstimate { get; set; }
        /// <summary>
        /// 改正后的数值.如果具有近似值，则 = 估计值 + 近似值，
        /// 否则直接返回估计值。 权逆阵直接为估计值的权逆阵。
        /// </summary>
        public CorrectableWeightedVector Corrected
        {
            get
            {
                if (ObsMatrix.HasApprox)
                {
                    //log.Debug("取消了 Corrected ，直接为 Estimated");
                    return new CorrectableWeightedVector(this.Estimated, ObsMatrix.ApproxVector);
                    // { ParamNames = this.Estimated.ParamNames };
                }
                return new CorrectableWeightedVector(this.Estimated);
            }
        }

      

        #region 设置
        /// <summary>
        /// 设置自由度
        /// </summary>
        /// <param name="obsMatrix"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetObsMatrix(AdjustObsMatrix obsMatrix) { this.ObsMatrix = obsMatrix; return this; }

        /// <summary>
        /// 设置平差类型
        /// </summary>
        /// <param name="AdjustmentType"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetAdjustmentType(AdjustmentType AdjustmentType) { this.AdjustmentType = AdjustmentType; return this; }
        /// <summary>
        /// 设置自由度
        /// </summary>
        /// <param name="freedom"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetFreedom(int freedom) { this.Freedom = freedom; return this; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetVarianceFactor(double val) { this.VarianceOfUnitWeight = val; return this; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetEstimated(WeightedVector val) { this.Estimated = val; return this; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetSecondEstimated(WeightedVector val) { this.SecondEstimated = val; return this; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetPredicted(WeightedVector val) { this.Predicted = val; return this; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetCorrection(WeightedVector val) { this.Correction = val; return this; }
        public AdjustResultMatrix SetCorrectedEstimate(WeightedVector  val) { this.CorrectedEstimate = val; return this; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetCorrectedObs(WeightedVector val) { this.CorrectedObs = val; return this; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public AdjustResultMatrix SetVtpv(double val) { this.Vtpv = val; return this; }

        #endregion

        #endregion

        /// <summary>
        /// 参数的编号，从 0 开始。失败则返回 -1.
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <returns>失败则返回 -1</returns>
        public int GetIndexOf(string paramName) { return ParamNames.IndexOf(paramName); }

        #region IO
        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        { 
            return "" + Estimated;
        }

       

        public void Dispose()
        {
           // this.ApproxVector = null;
        }


        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <returns></returns>
        public virtual string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            int nameLen = 10;
            sb.AppendLine(Geo.Utils.StringUtil.FillSpaceRight("ObsCount", nameLen) + "\t" + ObsCount);
            if(AdjustmentType != AdjustmentType.条件平差)
            {
                sb.AppendLine(Geo.Utils.StringUtil.FillSpaceRight("ParamCount", nameLen) + "\t" + ParamCount);
            }           

            sb.AppendLine(Geo.Utils.StringUtil.FillSpaceRight("Freedom", nameLen) + "\t" + Freedom);
            sb.AppendLine(Geo.Utils.StringUtil.FillSpaceRight("StdDev", nameLen) + "\t" + StdDev); 
            sb.AppendLine();

            //if (this.HasApprox)
            //{
            //    sb.AppendLine(ObsMatrixType.Observation);
            //    sb.AppendLine(this.ApproxVector.GetTabValues());
            //}
            if (this.Estimated != null)
            {
                sb.AppendLine("Estimated");
                sb.AppendLine(Estimated.ToReadableText(splitter));
            }
            if (this.Correction != null)
            {
                sb.AppendLine("Correction");
                sb.AppendLine(Correction.ToReadableText(splitter));
            }
            if (this.SecondEstimated != null)
            {
                sb.AppendLine("SecondEstimated");
                sb.AppendLine(SecondEstimated.ToReadableText(splitter));
            }
            if (this.CorrectedObs != null)
            {
                sb.AppendLine("CorrectedObs");
                sb.AppendLine(CorrectedObs.ToReadableText(splitter));
            }

            if (this.ObsMatrix.HasApprox && this.CorrectedEstimate != null)
            {
                sb.AppendLine("CorrectedEstimate");
                sb.AppendLine(CorrectedEstimate.ToReadableText(splitter));
            }

            if (this.ObsMatrix.HasApprox && this.Corrected != null)
            {
                sb.AppendLine("Corrected");
                sb.AppendLine(Corrected.ToReadableText(splitter));
            }
            return sb.ToString();
        }


        /// <summary>
        /// 用于打印输出和分析。
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="FullParamNames"></param>
        /// <returns></returns>
        public TableRowData GetTableRow(Vector vector, List<string> FullParamNames)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            int i = 0;
            foreach (var item in vector)
            {
                dic.Add(ParamNames[i], item);
                i++;
            }
            TableRowData row = new TableRowData(dic, FullParamNames);
            return row;
        }

        /// <summary>
        /// 用于具有多列的对象
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="FullParamNames">列的全部名称</param>
        /// <param name="keyNames">列关键字</param>
        /// <returns></returns>
        public TableRowData<RmsedNumeral> GetRmsedTableRow(WeightedVector vector, List<string> FullParamNames, List<string> keyNames)
        {
            Dictionary<string, RmsedNumeral> dic = new Dictionary<string, RmsedNumeral>();
            int i = 0;
            foreach (var item in vector)
            {
                RmsedNumeral num = vector.GetInverseWeightedValue(i);
                dic.Add(ParamNames[i], num);
                i++;
            }
            TableRowData<RmsedNumeral> row = new TableRowData<RmsedNumeral>(dic, FullParamNames, keyNames, RmsedNumeral.TabPlaceHolder);
            return row;
        }

        #endregion
        /// <summary>
        /// 返回不同类型的参数加权向量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public WeightedVector Get(AdjustParamVectorType type)
        {
            switch (type)
            {
                //case AdjustVectorType.观测向量: return Observation;
                case AdjustParamVectorType.参数先验值: return ObsMatrix. Apriori;
                case AdjustParamVectorType.参数改正后向量: return Corrected.CorrectedValue;
                case AdjustParamVectorType.参数估计值: return Estimated;
                case AdjustParamVectorType.参数预测值: return Predicted;
                //case AdjustVectorType.观测向量验后残差: return PostfitResidual; 
                default: return Estimated;
            }
        }
        

        #region  模糊度固定 2017.03.27, czs, add, last edit 2018.10.20, hmx
        /// <summary>
        /// 固定模糊度
        /// </summary>
        /// <param name="fixedAmbiguities"></param>
        /// <param name="prevFixedAmbiguities"></param>
        /// <param name="isConditionOrBigWeightParam"></param>
        /// <returns></returns>
        public WeightedVector SolveAmbiFixedResult(WeightedVector fixedAmbiguities, WeightedVector prevFixedAmbiguities, bool  isConditionOrBigWeightParam = true) 
        {
            FixedParamSolver fixedParamSolver = new FixedParamSolver(isConditionOrBigWeightParam);
            return fixedParamSolver.GetParamFixedResult(this.Estimated, fixedAmbiguities);
        }
 
        #endregion

        #region LLy模糊度固定
        /// <summary>
        /// PPP模糊度固定后的估计值,给不同的权,对应不同的估计值
        /// </summary>
        public WeightedVector Estimated_PPPAR1 { get; set; }
        /// <summary>
        /// PPP模糊度固定后的估计值,给不同的权,对应不同的估计值
        /// </summary>
        public WeightedVector Estimated_PPPAR2 { get; set; }
        /// <summary>
        /// PPP模糊度固定
        /// </summary>
        public IMatrix PppArmbiResolve(Dictionary<int, double> MWs, List<int> clipSats)
        {
            #region 宽巷FCB值，暂时手动输入 2013001当天
            double[] WLFCBs = new double[32];
            WLFCBs[0] = 0.000; WLFCBs[1] = 0.384; WLFCBs[2] = -0.250; WLFCBs[3] = 0.409; WLFCBs[4] = -0.281; WLFCBs[5] = -0.010;
            WLFCBs[6] = 0.130; WLFCBs[7] = -0.385; WLFCBs[8] = 0.400; WLFCBs[9] = -0.080; WLFCBs[10] = 0.463; WLFCBs[11] = 0.480;
            WLFCBs[12] = -0.010; WLFCBs[13] = 0.200; WLFCBs[14] = 0.285; WLFCBs[15] = 0.280; WLFCBs[16] = 0.420; WLFCBs[17] = 0.470;
            WLFCBs[18] = 0.310; WLFCBs[19] = -0.200; WLFCBs[20] = -0.274; WLFCBs[21] = 0.150; WLFCBs[22] = 0.050; WLFCBs[23] = -0.030;
            WLFCBs[24] = -0.060; WLFCBs[25] = 0.196; WLFCBs[26] = 0.000; WLFCBs[27] = -0.435; WLFCBs[28] = 0.170; WLFCBs[29] = -0.080;
            WLFCBs[30] = 0.240; WLFCBs[31] = 0.210;
            #endregion
            #region 窄巷FCB值，暂时手动输入 2013001当天
            double[] NLFCBs = new double[32];
            NLFCBs[0] = 0.000; NLFCBs[1] = 0.000; NLFCBs[2] = 0.290; NLFCBs[3] = 0.419; NLFCBs[4] = 0.000; NLFCBs[5] = 0.440;
            NLFCBs[6] = 0.115; NLFCBs[7] = 0.347; NLFCBs[8] = 0.230; NLFCBs[9] = 0.000; NLFCBs[10] = -0.451; NLFCBs[11] = 0.000;
            NLFCBs[12] = 0.279; NLFCBs[13] = -0.261; NLFCBs[14] = -0.131; NLFCBs[15] = -0.058; NLFCBs[16] = 0.249; NLFCBs[17] = 0.100;
            NLFCBs[18] = -0.472; NLFCBs[19] = -0.332; NLFCBs[20] = 0.000; NLFCBs[21] = 0.248; NLFCBs[22] = -0.061; NLFCBs[23] = -0.122;
            NLFCBs[24] = 0.366; NLFCBs[25] = 0.000; NLFCBs[26] = 0.000; NLFCBs[27] = 0.068; NLFCBs[28] = 0.000; NLFCBs[29] = 0.265;
            NLFCBs[30] = -0.114; NLFCBs[31] = 0.016;
            #endregion
            return PppArmbiResolve(MWs, clipSats, WLFCBs, NLFCBs);

        }

        private IMatrix PppArmbiResolve(
            Dictionary<int, double> MWs,
            List<int> clipSats,
            double[] fractionCodeBiasOfWideBand,
            double[] fractionCodeBiasOfNarrowBand)
        {

            #region 对MW值（宽巷模糊度）进行宽巷FCB纠正，注意是-
            Dictionary<int, double> new_MWs = new Dictionary<int, double>();
            foreach (var item in MWs)
            {
                int i = item.Key;//卫星编号
                new_MWs.Add(i, MWs[i] - MWs[1] - fractionCodeBiasOfWideBand[i - 1]); //星间单差MW值，并改正WLFCB
            }
            #endregion

            #region 对窄巷模糊度进行NLFCB改正,注意是-
            Dictionary<int, double> floatAmb = new Dictionary<int, double>();//存储浮点模糊度,有参考星  
            List<int> satnum = new List<int>(new_MWs.Keys);//所有卫星
            int refsat = satnum.First();//参考星 
            int jj = 5;
            for (int i = 5; i < ParamNames.Count; i++)
            {
                int sat = int.Parse(ParamNames[i].Substring(1, 2));
                if (clipSats.Contains(sat)) continue;
                floatAmb.Add(satnum[jj - 5], Estimated[i, 0]);//估值   
                jj++;
            }
            Dictionary<int, double> floatSDAmb = new Dictionary<int, double>();//存储星间单差浮点模糊度,没有参考星,单位：周
            foreach (var sat in satnum)
            {
                if (sat == refsat) continue;
                floatSDAmb.Add(sat, (floatAmb[sat] - floatAmb[refsat]) / 0.190293672798);//
            }
            //(vector[j] - vector[5]) * (1575.42 + 1227.6) / (1575.42 * 0.190293672798) - (wl) * 1227.6 / (1575.42 - 1227.6);
            Dictionary<int, double> floatNLAmb = new Dictionary<int, double>();//存储浮点窄巷模糊度,没有参考星，注意是-    
            foreach (var sat in floatSDAmb.Keys)
            {
                if ((Math.Abs(new_MWs[sat] - Math.Round(new_MWs[sat])) <= 0.25) && fractionCodeBiasOfWideBand[sat - 1] != 0 && fractionCodeBiasOfNarrowBand[sat - 1] != 0)  //NLFCBs=0代表无效
                {
                    new_MWs[sat] = Math.Round(new_MWs[sat]);
                    double tmpnl = floatSDAmb[sat] * (1575.42 + 1227.6) / 1575.42 - new_MWs[sat] * 1227.6 / (1575.42 - 1227.6) - fractionCodeBiasOfNarrowBand[sat - 1];
                    floatNLAmb.Add(sat, tmpnl);
                }
            }
            #endregion
            IMatrix new_Estimated = new ArrayMatrix(floatNLAmb.Count, 1, 0);//可以固定的窄巷模糊度对应的估值
            #region LAMBDA固定窄巷模糊度
            if (floatNLAmb.Count >= 4) //模糊度维度>4的情况才进行固定
            {
                #region 星间单差窄巷模糊度协方差矩阵
                IMatrix B = new ArrayMatrix(ParamNames.Count - 6, ParamNames.Count - 5, 0.0); //将非差模糊度转换为星间单差模糊度的旋转矩阵
                for (int i = 0; i < ParamNames.Count - 6; i++)
                {
                    B[i, 0] = -1; B[i, i + 1] = 1;
                }
                IMatrix BT = B.Transposition;
                IMatrix covaAbm = new ArrayMatrix(ParamNames.Count - 5, ParamNames.Count - 5, 0); //非差模糊度的协方差矩阵
                for (int i = 0; i < ParamNames.Count - 5; i++)
                {
                    for (int j = 0; j < ParamNames.Count - 5; j++)
                    {
                        covaAbm[i, j] = CovaOfEstimatedParam[i + 5, j + 5];
                    }
                }
                IMatrix covaSDAmb = B.Multiply(covaAbm).Multiply(BT); //星间单差模糊度的协方差矩阵
                IMatrix covaSDNLAbm = new ArrayMatrix(ParamNames.Count - 6, ParamNames.Count - 6, 0.0); //星间单差窄巷模糊度的协方差矩阵
                for (int i = 0; i < ParamNames.Count - 6; i++)
                {
                    for (int j = 0; j < ParamNames.Count - 6; j++)
                    {
                        covaSDNLAbm[i, j] = covaSDAmb[i, j] * (1575.42 + 1227.6) / (1575.42 * 0.190293672798);//系数
                    }
                }
                #endregion

                #region 部分模糊度固定，先将可以固定的星间单差窄巷模糊度 及其 协方差矩阵挑出来
                List<string> oldamb = new List<string>();
                for (int i = 6; i < ParamNames.Count; i++)
                {
                    string sat = ParamNames[i].Substring(1, 2);
                    oldamb.Add(sat);//所有的模糊度（除了参考星）
                }
                List<string> newamb = new List<string>();
                foreach (var item in floatNLAmb.Keys)
                {
                    newamb.Add(item.ToString());//可以固定的模糊度（宽巷 < 0.25 && NLFCB != 0）
                }
                IMatrix newtrix = NamedMatrix.GetNewMatrix(newamb, oldamb, covaSDNLAbm.Array); //将可以固定的模糊度的协方差矩阵挑出来
                #endregion

                #region LAMBDA星间单差窄巷模糊度固定
                //LAMBDA,依据协方差大小进行升序排列，将协方差最大的模糊度放在最后
                IMatrix narrowLaneAmbiguity = new ArrayMatrix(floatNLAmb.Count, 1, 0);
                int ii = 0;
                foreach (var item in floatNLAmb.Values)
                {
                    narrowLaneAmbiguity[ii, 0] = item;
                    ii++;
                }
                #region 降维，但是存在相等就麻烦了
                //IMatrix TransNarrowLaneMatrix = new Matrix(newtrix.RowCount, newtrix.RowCount);
                //double[] arr = new double[newtrix.RowCount]; for (int i = 0; i < newtrix.RowCount; i++) arr[i] = newtrix[i, i];
                //sort(ref arr); //降序排列
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    if (i < arr.Length - 1)
                //        if (arr[i] == arr[i + 1])
                //            //throw new Exception("没有考虑存在相等的数值情况！！！");
                //            arr[i + 1] += Math.Pow(10, -16);
                //    for (int j = 0; j < arr.Length; j++)
                //    {
                //        if (newtrix[j, j] == arr[i])
                //        {
                //            TransNarrowLaneMatrix[i, j] = 1;
                //        }
                //    }
                //}                

                //int ii = 0;
                //foreach(var key in floatNLAmb.Values)
                //{
                //    narrowLaneAmbiguity[ii, 0] = key;
                //    ii++;
                //}
                //IMatrix satsnum = new Matrix(floatNLAmb.Count, 1, 0);//卫星编号，重新进行了排序
                //ii = 0;
                //foreach(var key in floatNLAmb.Keys)
                //{
                //    satsnum[ii, 0] = key;
                //    ii++;
                //}
                //IMatrix orderNarrowLaneAmbiguity = TransNarrowLaneMatrix.Multiply(narrowLaneAmbiguity);
                //IMatrix orderSatsNum = TransNarrowLaneMatrix.Multiply(satsnum);
                //IMatrix orderNarrowLaneAmbiguityCovariance = TransNarrowLaneMatrix.Multiply(newtrix).Multiply(TransNarrowLaneMatrix.Transposition);
                #endregion
                //double[] a = orderNarrowLaneAmbiguity.GetCol(0).OneDimArray; //一维的星间单差窄巷模糊度
                //double[][] Q = orderNarrowLaneAmbiguityCovariance.Array; //按协方差对角线从小到大排列的协方差矩阵
                double[] a = narrowLaneAmbiguity.GetCol(0).OneDimArray;
                double[][] Q = newtrix.Array;
                LlyLambda lambda = new LlyLambda(newamb.Count, 2, Q, a);//LAMBDA算法
                double[] N1 = new double[a.Length * 2]; for (int i = 0; i < a.Length; i++) N1[i] = Math.Floor(a[i] + 0.5);
                double[] s = new double[2];
                int info = 0;
                info = lambda.getLambda(ref N1, ref s);
                double ratio = s[1] / s[0];
                #region
                //if (Q.Length > 4)
                //{
                //    while (info == -1 || ratio <= 3) //检验没有通过
                //    {
                //        Q = MatrixUtil.GetSubMatrix(Q, Q.Length - 1, Q.Length - 1);
                //        if (Q.Length < 4)
                //            break;
                //        a = new double[Q.Length]; for (int i = 0; i < a.Length; i++) a[i] = orderNarrowLaneAmbiguity.GetCol(0).OneDimArray[i];

                //        N1 = new double[a.Length * 2]; for (int i = 0; i < a.Length; i++) N1[i] = Math.Floor(a[i] + 0.5);
                //        s = new double[2];
                //        lambda = new LLYLAMBDA(a.Length, 2, Q, a);
                //        info = lambda.getLambda(ref N1, ref s);
                //        ratio = s[1] / s[0];
                //    }
                //}
                #endregion
                #endregion

                Dictionary<int, double> fixedIonoAmb = new Dictionary<int, double>();
                #region 求固定后的无电离层模糊度

                if (ratio > 3)
                {
                    if (N1.Length == newtrix.RowCount * 2)//没有出现降维
                    {
                        List<int> validsats = new List<int>(floatNLAmb.Keys);
                        for (int i = 0; i < newtrix.RowCount; i++)
                        {
                            int sat = validsats[i];
                            double currentWL = new_MWs[sat];//该卫星的宽巷
                            double currentNL = N1[i];//该卫星的窄巷
                            double aa = 1575.42 * 1227.60 / (1575.42 * 1575.42 - 1227.6 * 1227.60);
                            double bb = 1575.42 / (1575.42 + 1227.60);
                            double currentIonoAmb = aa * currentWL + bb * currentNL + bb * fractionCodeBiasOfNarrowBand[sat - 1];
                            fixedIonoAmb.Add(sat, currentIonoAmb);
                        }
                    }
                    IMatrix restrictMatrix = new ArrayMatrix(fixedIonoAmb.Count, ParamCount, 0); //约束矩阵
                    IMatrix W = new ArrayMatrix(fixedIonoAmb.Count, 1, 0);//W矩阵
                    ii = 0;
                    foreach (var sat in fixedIonoAmb.Keys)
                    {
                        int currentIndex = satnum.IndexOf(sat);//获取索引值                        
                        restrictMatrix[ii, 5] = -1;
                        restrictMatrix[ii, 5 + currentIndex] = 1;
                        W[ii, 0] = (floatSDAmb[sat] - fixedIonoAmb[sat]) * 0.190293672798;
                        ii++;
                    }
                    #region 具有约束条件的参数平差
                    IMatrix A = restrictMatrix;
                    IMatrix AT = A.Transposition;
                    IMatrix AQAT = A.Multiply(CovaOfEstimatedParam).Multiply(AT);
                    IMatrix Inv_AQAT = AQAT.GetInverse();
                    IMatrix tmp1 = CovaOfEstimatedParam.Multiply(AT).Multiply(Inv_AQAT);
                    IMatrix tmp2 = tmp1.Multiply(W);
                    IMatrix X1 = new ArrayMatrix(ParamNames.Count, 1, 0);
                    for (int i = 0; i < ParamNames.Count; i++)
                    {
                        X1[i, 0] = Estimated[i, 0];
                    }

                    new_Estimated = X1.Minus(tmp2);//估值

                    IMatrix tmp3 = tmp1.Multiply(A).Multiply(CovaOfEstimatedParam);
                    IMatrix new_CovaOfEstimatedParam = CovaOfEstimatedParam.Minus(tmp3);//协方差矩阵

                    IMatrix P2 = new ArrayMatrix(fixedIonoAmb.Count, fixedIonoAmb.Count, 0);
                    for (int kk = 0; kk < fixedIonoAmb.Count; kk++)
                    {
                        P2[kk, kk] = 5 * Math.Pow(10, 6);
                    }

                    IMatrix m1 = P2.GetInverse();
                    IMatrix m2 = m1.Plus(AQAT);
                    IMatrix m3 = m2.GetInverse();
                    IMatrix J = CovaOfEstimatedParam.Multiply(AT).Multiply(m3);
                    IMatrix M4 = J.Multiply(W);
                    IMatrix new2_Estimated = Estimated;
                    IMatrix new2_CovaOfEstimatedParam = CovaOfEstimatedParam;
                    if (Math.Sqrt(M4[0, 0] * M4[0, 0] + M4[1, 0] * M4[1, 0] + M4[2, 0] * M4[2, 0]) < 0.001)
                    {
                        new2_Estimated = Estimated;
                        new2_CovaOfEstimatedParam = CovaOfEstimatedParam;
                    }
                    else
                    {
                        new2_Estimated = X1.Minus(M4);
                        //new2_Estimated = X1.Plus(M4);
                        IMatrix m5 = J.Multiply(A).Multiply(CovaOfEstimatedParam);
                        new2_CovaOfEstimatedParam = CovaOfEstimatedParam.Minus(m5);
                    }

                    //this.Estimated = new WeightedVector(new_Estimated, new2_CovaOfEstimatedParam);
                    #endregion
                    #region
                    //string SavePath = "C:\\Users\\lilinyang\\Desktop\\Q_ALGO" + ".txt";
                    //FileInfo aFile = new FileInfo(SavePath);
                    //StreamWriter SW = aFile.CreateText();
                    //System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
                    //GN.NumberDecimalDigits = 6;
                    //for (int i = 0; i < new2_CovaOfEstimatedParam.RowCount; i++)
                    //{
                    //    for (int j = 0; j < new2_CovaOfEstimatedParam.ColCount; j++)
                    //    {
                    //        SW.Write(new_CovaOfEstimatedParam[i, j].ToString());
                    //        SW.Write(" ");
                    //    }
                    //    SW.Write("\n");
                    //}                       
                    //SW.Close();
                    #endregion
                    IMatrix Trans = new ArrayMatrix(ParamCount - 1, ParamCount, 0);
                    for (int i = 0; i < ParamCount - 1; i++)
                    {
                        if (i <= 4)
                        {
                            Trans[i, i] = 1;
                        }
                        else
                        {
                            Trans[i, 5] = -1; Trans[i, i + 1] = 1;
                        }
                    }
                    IMatrix Trans_T = Trans.Transposition;
                    IMatrix CovaofXSDN = Trans.Multiply(CovaOfEstimatedParam).Multiply(Trans_T); //坐标、对流层、钟差和单差模糊度的协方差矩阵
                    IMatrix Q_XN = new ArrayMatrix(5, ParamCount - 6);
                    IMatrix Q_N = new ArrayMatrix(ParamCount - 6, ParamCount - 6);
                    IMatrix Q_X = new ArrayMatrix(5, 5, 0);
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Q_X[i, j] = CovaofXSDN[i, j];
                        }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < ParamCount - 6; j++)
                        {
                            Q_XN[i, j] = CovaofXSDN[i, j + 5];
                        }
                    }
                    for (int i = 0; i < ParamCount - 6; i++)
                    {
                        for (int j = 0; j < ParamCount - 6; j++)
                        {
                            Q_N[i, j] = CovaofXSDN[i + 5, j + 5];
                        }
                    }
                    IMatrix Q_NX = Q_XN.Transposition;
                    IMatrix Inv_QN = Q_N.GetInverse();
                    IMatrix common1 = Q_XN.Multiply(Inv_QN);
                    IMatrix dealt_N = new ArrayMatrix(ParamCount - 6, 1, 0);
                    ii = 0;
                    foreach (var sat in fixedIonoAmb.Keys)
                    {
                        int currentIndex = satnum.IndexOf(sat);//获取索引值                        
                        dealt_N[currentIndex - 1, 0] = (floatSDAmb[sat] - fixedIonoAmb[sat]) * 0.190293672798;
                        ii++;
                    }
                    IMatrix common2 = common1.Multiply(dealt_N);
                    IMatrix old_X = new ArrayMatrix(5, 1, 0);
                    for (int i = 0; i < 5; i++)
                    {
                        old_X[i, 0] = Estimated[i, 0];
                    }
                    IMatrix new_X = old_X.Minus(common2);
                    IMatrix common3 = Q_XN.Multiply(Inv_QN).Multiply(Q_NX);
                    IMatrix new_QX = Q_X.Minus(common3);
                    IMatrix new3_CovaOfEstimatedParam = CovaOfEstimatedParam;
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            new3_CovaOfEstimatedParam[i, j] = new_QX[i, j];
                        }
                    }
                    IMatrix new3_Estimated = X1;
                    for (int i = 0; i < 5; i++)
                    {
                        new3_Estimated[i, 0] = new_X[i, 0];
                    }
                    this.Estimated = new WeightedVector(new2_Estimated, new2_CovaOfEstimatedParam) { ParamNames = ParamNames };
                }
                #endregion
            }
            #endregion
            return new_Estimated;
        }

        #endregion
    }
     

}