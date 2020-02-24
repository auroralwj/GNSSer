//2018.03.24, czs, create in HMX, 平差输入矩阵，观测值

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common; 
using System.Threading.Tasks;
using Geo.IO;


namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 观测矩阵类型
    /// </summary>
    public enum ObsMatrixType
    {
        Apriori,
        Observation,
        Coefficient,
        Transfer,
        /// <summary>
        /// 误差方程自由项，参数平差的D，条件平差的B0
        /// </summary>
        FreeVector,
        /// <summary>
        /// 近似值，可以是参数近似值
        /// </summary>
        ApproxVector,
        SecondCoefficient,
        SecondFreeVector,
        SecondTransfer,
        SecondApproxVector,
    }

    /// <summary>
    /// 平差输入矩阵，观测值
    /// </summary>
    public class AdjustObsMatrix : IDisposable, IReadable
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log = Log.GetLog(typeof(AdjustObsMatrix));

        /// <summary>
        /// 平差。构造函数。
        /// </summary>
        public AdjustObsMatrix() { }

        #region 构造函数

        /// <summary>
        /// 参数平差构造函数。
        /// </summary>
        /// <param name="coeffOfParams">参数系数阵</param>
        /// <param name="obsMinusApriori">观测值 - 估值，观测方程常数项</param>
        /// <param name="inervseWeight">权逆阵</param>
        public AdjustObsMatrix(double coeffOfParams, double obsMinusApriori, double inervseWeight = 1)
            : this(MatrixUtil.Create(coeffOfParams), MatrixUtil.Create(obsMinusApriori), MatrixUtil.Create(inervseWeight)) { }

        /// <summary>
        /// 参数平差构造函数。
        /// </summary>
        /// <param name="coeffOfParams">参数系数阵</param>
        /// <param name="obsMinusApriori">观测值 - 估值，观测方程常数项</param>
        /// <param name="inervseWeight">权逆阵</param>
        public AdjustObsMatrix(double[][] coeffOfParams, double[] obsMinusApriori, double[][] inervseWeight = null, Vector approx = null)
            : this(coeffOfParams, MatrixUtil.Create(obsMinusApriori), inervseWeight, approx) { }

        /// <summary>
        /// 参数平差构造函数。
        /// 误差方程为 V = A dX - obsMinusApriori,
        /// obsMinusApriori = 观测值 - 先验函数值,
        /// 自定义权阵。
        /// </summary>
        /// <param name="coeffOfParams"></param>
        /// <param name="obsMinusApriori"></param>
        /// <param name="inverseWeightOfObs"></param>
        /// <param name="approx"></param>
        public AdjustObsMatrix(double[][] coeffOfParams, double[][] obsMinusApriori, double[][] inverseWeightOfObs = null, Vector approx = null)
            : this(new WeightedVector(new ArrayMatrix(obsMinusApriori),
                inverseWeightOfObs == null ? null : new ArrayMatrix(inverseWeightOfObs)), 
                  new Matrix(coeffOfParams), approx) { }

        /// <summary>
        /// 参数平差构造函数。
        /// </summary>
        /// <param name="observation">观测值和协方差</param>
        /// <param name="coeffOfParams">系数阵</param>
        /// <param name="approx">是否使用参数的偏移量</param>
        /// <param name="paramNames">参数名称</param>
        public AdjustObsMatrix(WeightedVector observation, Matrix coeffOfParams, Vector approx = null, List<string> paramNames = null)
        {
            this.Coefficient = (coeffOfParams);
            this.Observation = observation;
            this.ApproxVector = approx;
            if (paramNames != null) { this.ParamNames = new List<string>(paramNames); }

        }

        /// <summary>
        /// 构造函数。 
        /// </summary>
        /// <param name="builder">矩阵生成器</param>
        public AdjustObsMatrix(BaseAdjustMatrixBuilder builder)
           : this(
           builder.AprioriParam,
           builder.Observation,
           builder.Coefficient,
           builder.Transfer,
           builder.ApproxParam,
           builder.FreeVector
           )
        {
            this.SecondApproxVector = builder.SecondApproxVector;
            this.SecondCoefficient = builder.SecondCoefficient;
            this.SecondFreeVector = builder.SecondFreeVector;
            this.SecondTransfer = builder.SecondTransfer;
            this.CoeffIncrementOfNormalEquation = builder.CoeffIncrementOfNormalEquation;
            this.ParamNames = new List<string>(builder.ParamNames);
            this.SecondParamNames = builder.SecondParamNames;
        }

        #endregion

        /// <summary>
        /// kalman滤波构造函数。 
        /// </summary>
        /// <param name="coeff">A n*m阶设计矩阵，也称观测矩阵，系数阵</param> 
        /// <param name="apriori">先验值</param>
        /// <param name="observation">观测值 L,满足 l = L - (AX0 + D), 若X0与D为null， 则l=L </param>  
        /// <param name="trans">Φ 状态转移矩阵和Q_m 动力模型噪声向量 W 的方差</param>
        /// <param name="approx">参数近似向量X0,满足 l = L - (AX0 + D) </param> 
        /// <param name="freeVector">常数项D，满足 l = L - (AX0 + D) </param> 
        public AdjustObsMatrix(
            WeightedVector apriori,
            WeightedVector observation,
            Matrix coeff,
            WeightedMatrix trans,
            Vector approx = null,
            Vector freeVector = null
            )
        {
            this.Coefficient = coeff;
            this.Apriori = apriori;
            this.Observation = observation;
            this.Transfer = trans;
            this.ApproxVector = approx;
            this.FreeVector = freeVector;

        }

        #region 常用方法
        /// <summary>
        /// 可选，独立设置。
        /// </summary>
        /// <param name="approx"></param>
        public void SetApprox(double[] approx)
        {
            this.ApproxVector = new Vector(approx);
        }
        /// <summary>
        /// 参数的编号，从 0 开始。失败则返回 -1.
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <returns>失败则返回 -1</returns>
        public int GetIndexOf(string paramName) { return ParamNames.IndexOf(paramName); }
        #endregion

        #region 属性
        /// <summary>
        /// 存储信息
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 参数顺序和名称。必须设置！！！
        /// </summary>
        public List<string> ParamNames { get;  set; }
        /// <summary>
        ///第二参数
        /// </summary>
        public List<string> SecondParamNames { get; protected set; }
        #region  属性判断
        /// <summary>
        /// 是否具有先验参数
        /// </summary>
        public bool HasApriori { get { return Apriori != null && Apriori.Count > 0; } }
        /// <summary>
        /// 是否具有状态转移矩阵
        /// </summary>
        public bool HasTransfer { get { return Transfer != null && Transfer.RowCount > 0; } }
        /// <summary>
        /// 是否具有近似值。
        /// </summary>
        public bool HasApprox { get { return ApproxVector != null && ApproxVector.Count > 0; } }
        /// <summary>
        /// 是否具有第二近似值。
        /// </summary>
        public bool HasSecondApprox { get { return SecondApproxVector != null && SecondApproxVector.Count > 0; } }
        /// <summary>
        /// 是否具有预测值
        /// </summary>
        public bool HasPredict { get { return Predicted != null && Predicted.Count > 0; } }
        /// <summary>
        /// 是否具有自由向量
        /// </summary>
        public bool HasFreeVector { get { return FreeVector != null && FreeVector.Count > 0; } }
        /// <summary>
        /// 是否具有第二自由向量
        /// </summary>
        public bool HasSecondFreeVector { get { return SecondFreeVector != null && SecondFreeVector.Count > 0; } }
        #endregion

        /// <summary>
        /// 观测值，通常为改正数，即观测值减去近似值。
        /// </summary>
        public WeightedVector Observation { get; protected set; }
        /// <summary>
        /// 误差方程的常数项，自由项，维度与系数阵行数相同，只有数值没有权。
        /// 是参数平差里面的D，条件平差里面的B0，程序中，通常使用V0，因为其总是与V同维度。
        /// </summary>
        public Vector FreeVector { get; set; }
        /// <summary>
        /// 扩展平差误差方程的常数项，自由项，维度与系数阵行数相同，只有数值没有权。
        /// 如果 FreeVector 不够用，则用这个
        /// </summary>
        public Vector SecondFreeVector { get; set; }
        /// <summary>
        /// 观测值数量
        /// </summary>
        public int ObsCount { get { return Coefficient.RowCount; } }
        /// <summary>
        /// 近似值，即所求最终参数的近似值，当先验值不是残差时，近似值可以直接作为先验值。
        /// 近似值为参数近似数值，没有权信息，其估计残差才有权值信息。
        /// 如果不设置近似值，则近似值为 0 。
        /// </summary>
        public Vector ApproxVector { get; protected set; }
        /// <summary>
        /// 第二参数近似向量
        /// </summary>
        public Vector SecondApproxVector { get; protected set; }
        /// <summary>
        /// 先验值，与参数估计值同质，如果采用参数的改正量进行计算时，先验值通常是值为 0 的向量。
        /// </summary>
        public WeightedVector Apriori { get; protected set; }
        /// <summary>
        /// 参数估计值及其权倒数、协方差
        /// </summary>
        public WeightedVector Predicted { get; protected set; }

        /// <summary>
        /// 观测方程中的默认系数阵
        /// </summary>
        public Matrix Coefficient { get; protected set; }
        /// <summary>
        /// 法方程系数阵增量
        /// </summary>
        public Matrix CoeffIncrementOfNormalEquation { get; protected set; }
        /// <summary>
        /// 观测方程中第二系数阵，如在具有参数的条件平差
        /// </summary>
        public Matrix SecondCoefficient { get; protected set; }

        /// <summary>
        /// 参数状态转移的权逆阵
        /// </summary>
        public IMatrix InverseWeightOfTransfer { get { return Transfer.InverseWeight; } }
        /// <summary>
        /// 状态转移矩阵
        /// </summary>
        public WeightedMatrix Transfer { get; protected set; } 
        /// <summary>
        /// 第二状态转移
        /// </summary>
        public WeightedMatrix SecondTransfer { get; protected set; } 

        /// <summary>
        ///自由度，样本中独立或能自由变化的变量个数,通常为：样本个数 - 被限制的变量个数或条件数，或多余观测数。
        /// </summary>
        public virtual int Freedom { get { return Coefficient.RowCount - Coefficient.ColCount; } }
        /// <summary>
        /// 未知参数数量
        /// </summary>
        public int ParamCount { get { if (Coefficient != null) return Coefficient.ColCount; return 0; } }
        /// <summary>
        /// 设置自由向量
        /// </summary>
        /// <param name="FreeVector"></param>
        /// <returns></returns>
        public AdjustObsMatrix SetFreeVector(Vector FreeVector) { this.FreeVector = FreeVector; return this; }
        /// <summary>
        /// 设置第一系数阵
        /// </summary>
        /// <param name="Coefficient"></param>
        /// <returns></returns>
        public AdjustObsMatrix SetCoefficient(IMatrix Coefficient) { this.Coefficient = new Matrix(Coefficient); return this; }
        /// <summary>
        /// 设置第一系数阵
        /// </summary>
        /// <param name="Coefficient"></param>
        /// <returns></returns>
        public AdjustObsMatrix SetCoefficient(Matrix Coefficient) { this.Coefficient = Coefficient; return this; }
        /// <summary>
        /// 设置观测值
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public AdjustObsMatrix SetObservation(WeightedVector observation) { this.Observation = observation; return this; }
        /// <summary>
        /// 设置观测值,等权视之。
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public AdjustObsMatrix SetObservation(Vector observation) { this.Observation = new WeightedVector( observation, new DiagonalMatrix(observation.Count, 1.0)); return this; }

        #endregion
        /// <summary>
        /// 参数平差的矩阵方程
        /// </summary>
        /// <returns></returns>
        public MatrixEquation GetObsMatrixEquation(string name)
        {
            MatrixEquation eq = new MatrixEquation(this.Coefficient, this.Observation, name);

            return eq;
        }
      

        /// <summary>
        /// 构建不变参数的法方程。不变参数由第二参数构成
        /// 注意：需要控制不变参数的增减问题。
        /// 基本思路：增加则插入，减少则删除，通过参数名称来控制。
        /// </summary>
        /// <returns></returns>
        public MatrixEquation BuildConstParamNormalEquation()
        {
            //观测值权阵设置,对已知量赋值 
            Matrix L = new Matrix((IMatrix)this.Observation);
            Matrix QL = new Matrix(this.Observation.InverseWeight);
            Matrix PL = new Matrix(QL.GetInverse());
            Matrix A = new Matrix(this.Coefficient);
            Matrix AT = A.Trans;
            Matrix B = new Matrix(this.SecondCoefficient);
            Matrix BT = B.Trans;
            Matrix X0 = this.HasApprox ? new Matrix(this.ApproxVector, true) : null;
            Matrix Y0 = this.HasSecondApprox ? new Matrix(this.SecondApproxVector, true) : null;
            Matrix D = this.HasFreeVector ? new Matrix(this.FreeVector, true) : null;
            int obsCount = L.RowCount;
            int fixedParamCount = B.ColCount;
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
            NofB2.ColNames = this.SecondParamNames;
            NofB2.RowNames = this.SecondParamNames;

            UofB2.RowNames = this.SecondParamNames;
            UofB2.ColNames = new List<string>() { "ConstParam" };


            var ne = new MatrixEquation(NofB2, UofB2);
            return ne;
        }

        /// <summary>
        /// 构建易变参数无关的不变参数系数阵。原易变参数由第二参数系数阵划分。
        /// </summary>
        /// <returns></returns>
        public Matrix BuildCoeefOfConstParam()
        {
            //观测值权阵设置,对已知量赋值 
            Matrix L = new Matrix((IMatrix)this.Observation);
            Matrix QL = new Matrix(this.Observation.InverseWeight);
            Matrix PL = new Matrix(QL.GetInverse());
            Matrix A = new Matrix(this.Coefficient);
            Matrix AT = A.Trans;
            Matrix B = new Matrix(this.SecondCoefficient);
            Matrix X0 = this.HasApprox ? new Matrix(this.ApproxVector, true) : null;
            Matrix Y0 = this.HasSecondApprox ? new Matrix(this.SecondApproxVector, true) : null;
            Matrix D = this.HasFreeVector ? new Matrix(this.FreeVector, true) : null;

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
            return B2;
        }


        #region IO
        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToFormatedText();
        }

        /// <summary>
        /// 格式化输出。
        /// </summary>
        /// <returns></returns>
        private string ToFormatedText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Adjustment Report,  By Geo                " + DateTime.Now);
            sb.AppendLine(String.Format("      ObsCount:{0}", this.Observation.Count));
            sb.AppendLine(String.Format("   ParamsCount:{0}", ParamCount));

            sb.AppendLine("------------------------------------------------------------------");

            if (this.HasApprox)
            {
                sb.AppendLine("参数近似值 X0");
                sb.AppendLine(this.ApproxVector.ToReadableText());
            }
            if (this.FreeVector != null)
            {
                sb.AppendLine("自由向量 D"); 
                sb.AppendLine(this.FreeVector.ToReadableText());
            }
            if (this.Observation != null)
            {
                sb.AppendLine("观测残差 L");
                sb.AppendLine(Observation.ToReadableText());
            } 
            if (this.HasApriori)
            {
                sb.AppendLine("参数先验值 X1");
                sb.AppendLine(this.Apriori.ToReadableText());
            }
            if (this.Coefficient != null)
            {
                sb.AppendLine("系数（控制）矩阵 A");
                sb.AppendLine(this.Coefficient.ToReadableText()); 
            }

            if (this.Transfer != null)
            {
                sb.AppendLine("状态转移矩阵 T");
                sb.AppendLine(this.Transfer.ToReadableText());
            }

            if (this.SecondCoefficient != null)
            {
                sb.AppendLine("第二系数（控制）矩阵 B");
                sb.AppendLine(this.SecondCoefficient.ToReadableText());
            }

            if (this.SecondFreeVector != null)
            {
                sb.AppendLine("第二自由向量 D2");
                sb.AppendLine(  this.SecondFreeVector.ToReadableText());
            }

            if (this.SecondTransfer != null)
            {
                sb.AppendLine("状态转移矩阵 T2");
                sb.AppendLine(this.SecondTransfer.ToReadableText());
            }

            return sb.ToString();
        }

        public void Dispose()
        {
            this.ApproxVector = null;
        }

        #region 可读性好的文本表达


        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <returns></returns>
        public virtual string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.GetType().Name);

            //首先是属性
            Geo.Utils.StringUtil.AppendListLine(sb, ParamNames, "ParamNames:");
            Geo.Utils.StringUtil.AppendListLine(sb, SecondParamNames, "SecondParamNames:"); 
                        
            if (this.Observation != null)
            {
                sb.AppendLine(ObsMatrixType.Observation.ToString());
                sb.AppendLine(Observation.ToReadableText(splitter));
            }
            if (this.HasFreeVector)
            {
                sb.AppendLine(ObsMatrixType.FreeVector.ToString());
                sb.AppendLine(FreeVector.ToReadableText());
            }
            if (this.HasSecondFreeVector)
            {
                sb.AppendLine(ObsMatrixType.SecondFreeVector.ToString());
                sb.AppendLine(SecondFreeVector.ToReadableText());
            }
            if (this.HasApprox)
            {
                sb.AppendLine(ObsMatrixType.ApproxVector.ToString());
                sb.AppendLine(this.ApproxVector.ToReadableText());
            }
            if (this.HasSecondApprox)
            {
                sb.AppendLine(ObsMatrixType.SecondApproxVector.ToString());
                sb.AppendLine(this.SecondApproxVector.ToReadableText());
            }
            if (this.HasApriori)
            {
                sb.AppendLine(ObsMatrixType.Apriori.ToString());
                sb.AppendLine(this.Apriori.ToReadableText(splitter));
            }
            if (this.Coefficient != null)
            {
                sb.AppendLine(ObsMatrixType.Coefficient.ToString());
                sb.AppendLine(Matrix.ToReadableText(Coefficient, splitter));
            }
            if (this.SecondCoefficient != null)
            {
                sb.AppendLine(ObsMatrixType.SecondCoefficient.ToString());
                sb.AppendLine(Matrix.ToReadableText(SecondCoefficient, splitter));
            }

            if (this.HasTransfer)
            {
                sb.AppendLine(ObsMatrixType.Transfer.ToString());
                sb.AppendLine(this.Transfer.ToReadableText(splitter));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 批量解析.以AdjustObsMatrix分隔
        /// </summary>
        /// <param name="text"></param>
        /// <param name="splitterOfMatrixItems"></param>
        /// <returns></returns>
        public static List<AdjustObsMatrix> ParseList(string text, string[] splitterOfMatrixItems = null)
        {
            List<AdjustObsMatrix> list = new List<AdjustObsMatrix>();
            string [] strs = text.Split(new string[] { typeof(AdjustObsMatrix).Name },  StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in strs)
            {
                var obs = Parse(str);
                if (obs != null && obs.Observation != null)
                {
                    list.Add(obs);
                }
            }
            return list;
        }
         
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="text"></param>
        /// <param name="splitterOfMatrixItems">分隔矩阵元素的分隔符</param>
        /// <returns></returns>
        public  static AdjustObsMatrix Parse(string text, string[] splitterOfMatrixItems = null)
        {
            AdjustObsMatrix obs = new AdjustObsMatrix();
            if (splitterOfMatrixItems == null)
            {
                splitterOfMatrixItems = new string[] { ",", ";", "\t", " " };
            }

            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.None); //保留空格以分隔

            //解析本对象属性
            ParseHeader(obs, lines);

            int len = lines.Length;
            var typeNames = Enum.GetNames(typeof(ObsMatrixType));

            StringBuilder currentBlockSb = new StringBuilder();
            ObsMatrixType currentType = ObsMatrixType.Apriori;
            ObsMatrixType nextType = ObsMatrixType.Apriori;

            for (int i = 0; i < len; i++)
            {
                string line = lines[i].Trim();//当前行
                if (typeNames.Contains(line))//新区
                {
                    currentType = nextType;//
                    nextType = Geo.Utils.EnumUtil.Parse<ObsMatrixType>(line); //这是解析的下一个区的名称

                    if (currentBlockSb.Length > 2)
                    {
                        ParseBufferText(splitterOfMatrixItems, obs, currentBlockSb, currentType);
                    }
                    continue;
                }

                currentBlockSb.AppendLine(line);
            }

            if (currentBlockSb.Length > 2)
            {
                currentType = nextType;//
                //解析最后一个
                ParseBufferText(splitterOfMatrixItems, obs, currentBlockSb, currentType);
            }
            return obs;
        }

        /// <summary>
        /// 解析本对象属性
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="lines"></param>
        private static void ParseHeader(AdjustObsMatrix obs, string[] lines)
        {
            var headerSpliter = new char[] { ':', '：' };
            var headerItemSpliter = new char[] { ',', ' ', '\t', ';' };
            foreach (var line in lines)
            {
                if (String.IsNullOrEmpty(line)) { continue; }
                if (line.Contains("×"))
                {
                    break;
                }

                var items = line.Split(headerSpliter, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length == 1) { continue; }

                var header = items[0];              
                var content = items[1];
                switch (header)
                {
                    case "ParamNames":
                        obs.ParamNames = new List<string>(content.Split(headerItemSpliter, StringSplitOptions.RemoveEmptyEntries));
                        break;
                    case "SecondParamNames":
                        obs.SecondParamNames = new List<string>(content.Split(headerItemSpliter, StringSplitOptions.RemoveEmptyEntries));
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 解析为对象
        /// </summary>
        /// <param name="splitterOfMatrixItems"></param>
        /// <param name="obs"></param>
        /// <param name="currentBlockSb"></param>
        /// <param name="currentType"></param>
        private static void ParseBufferText(string[] splitterOfMatrixItems, AdjustObsMatrix obs, StringBuilder currentBlockSb, ObsMatrixType currentType)
        {
            switch (currentType)
            {
                case ObsMatrixType.Apriori:
                    obs.Apriori = WeightedVector.Parse(currentBlockSb.ToString());
                    break;
                case ObsMatrixType.Observation:
                    obs.Observation = WeightedVector.Parse(currentBlockSb.ToString(), splitterOfMatrixItems);
                    break;
                case ObsMatrixType.Coefficient:
                    obs.Coefficient = Matrix.Parse(currentBlockSb.ToString(), splitterOfMatrixItems);
                    break;
                case ObsMatrixType.SecondCoefficient:
                    obs.SecondCoefficient = Matrix.Parse(currentBlockSb.ToString(), splitterOfMatrixItems);
                    break;
                case ObsMatrixType.Transfer:
                    obs.Transfer = WeightedMatrix.Parse(currentBlockSb.ToString(), splitterOfMatrixItems);
                    break;
                case ObsMatrixType.ApproxVector:
                    obs.ApproxVector = Vector.Parse(currentBlockSb.ToString());
                    break;
                case ObsMatrixType.SecondApproxVector:
                    obs.SecondApproxVector = Vector.Parse(currentBlockSb.ToString());
                    break;
                case ObsMatrixType.FreeVector:
                    obs.FreeVector = Vector.Parse(currentBlockSb.ToString());
                    break;
                case ObsMatrixType.SecondFreeVector:
                    obs.SecondFreeVector = Vector.Parse(currentBlockSb.ToString());
                    break;
                default:
                    break;
            }
            currentBlockSb.Clear();
        }

        #endregion
        #endregion

        public static AdjustObsMatrix BuildATtest(int paramCount, int obsCount, AdjustmentType adjustmentType = AdjustmentType.参数平差)
        {
            bool isParamOrCondition = adjustmentType == AdjustmentType.参数平差;
            int restCount = obsCount - paramCount;
            int rowOfCoeef = isParamOrCondition ? obsCount : restCount;
            int colOfCoeef = isParamOrCondition ? paramCount : obsCount;

            AdjustObsMatrix obsMatrix = new AdjustObsMatrix();
            obsMatrix.Observation = WeightedVector.GenerateATest(obsCount);
            obsMatrix.Apriori = WeightedVector.GenerateATest(paramCount);
            obsMatrix.Coefficient = BuildACoeefient(rowOfCoeef, colOfCoeef);
            obsMatrix.Transfer = new WeightedMatrix(Matrix.CreateIdentity(paramCount), Matrix.CreateIdentity(paramCount));

            obsMatrix.FreeVector = new Vector(rowOfCoeef, 1);
            obsMatrix.SecondFreeVector = new Vector(paramCount, 1);
            obsMatrix.ApproxVector = new Vector(paramCount, 1);
            obsMatrix.SecondApproxVector = new Vector(paramCount, 1);

            if (adjustmentType == AdjustmentType.具有参数的条件平差)
            {
                obsMatrix.SecondCoefficient = BuildACoeefient(rowOfCoeef, colOfCoeef);
            }


            return obsMatrix;
        }

        /// <summary>
        /// 生成一个系数阵
        /// </summary>
        /// <param name="rowOfCoeef"></param>
        /// <param name="colOfCoeef"></param>
        /// <returns></returns>
        private static Matrix BuildACoeefient(int rowOfCoeef, int colOfCoeef)
        {
            Matrix mat = new Matrix(rowOfCoeef, colOfCoeef);
            int colIndex = 0;
            for (int i = 0; i < rowOfCoeef; i++)
            {
                colIndex = i;

                while (colIndex >= colOfCoeef)
                {
                    colIndex -= colOfCoeef;
                }
                mat[i, colIndex] = 1;
            }

            return mat;
        }
    }
}