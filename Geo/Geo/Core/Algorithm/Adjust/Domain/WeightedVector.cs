//2014.09.01,czs,create,封装具有权值的向量
//2014.11.19, czs, edit in namu, 提取 VectorMatrix， IWeightedVector，也是一个单列矩阵

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.IO;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 具有权值的数据向量。也是一个单列矩阵。
    /// 其权值采用 InverseWeight 属性专门存储。
    /// 平差计算中，数据向量值与数值的协方差是平差的基本单元。
    /// 本类可以用于存储先验值，估计值，推估值等。
    /// </summary>
    public class WeightedVector : VectorMatrix, IDisposable, IWeightedVector, IReadable
    {
        Log log = new Log(typeof(WeightedVector));
        
        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WeightedVector()
        {

        }

        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector">数据向量</param>
        /// <param name="inverseWeight">权逆阵对角线,如果为null，则为单位阵</param> 
        public WeightedVector(double[] vector, double[] inverseWeight)
            : this(
            new Vector(vector),
            inverseWeight == null ? new DiagonalMatrix(vector.Length, 1) : new DiagonalMatrix(inverseWeight)
            )
        {
        }
        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector">数据向量</param>
        /// <param name="names">名称</param>
        public static WeightedVector Parse(List<RmsedNumeral> vector, List<string> names)          
        {
            Vector veces = new Vector();
            Vector rmsed = new Vector();
            int i = 0;
            foreach (var item in vector)
            {
                var name = names[i];
                veces.Add(item.Value, name);
                rmsed.Add(item.Rms, name);
                i++;
            }
            return new WeightedVector(veces, rmsed);
        }
        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector">数据向量</param>
        /// <param name="rmsVector">权逆阵对角线,如果为null，则为单位阵</param> 
        public WeightedVector(Vector vector, Vector rmsVector)
            : this(
            (vector),
            rmsVector == null ? new DiagonalMatrix(vector.Count, 1) : new DiagonalMatrix(rmsVector.Pow(2.0))
            )
        {
        }
        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector">数据向量</param>
        /// <param name="rms">权逆阵对角线，采用统一的 RMS</param> 
        public WeightedVector(Vector vector, double rms)
            : this(
            (vector),
              new DiagonalMatrix(vector.Count, rms)
            )
        {
        }
        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector">数据向量</param>
        /// <param name="inverseWeight">权逆阵,如果为null，则为单位阵</param> 
        public WeightedVector(double[] vector, double[][] inverseWeight = null)
            : this(
            new Vector(vector),
            inverseWeight == null ? new DiagonalMatrix(vector.Length, 1) : new DiagonalMatrix(inverseWeight)
            )
        {
        }
        /// <summary>
        /// 对象表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetObjectTable()
        {
            ObjectTableStorage result = new ObjectTableStorage();
            var rmsVec = this.GetRmsedVector();
            for (int i = 0; i < rmsVec.Count; i++)
            {
                var item = rmsVec.GetItem(i);
                result.NewRow();
                result.AddItem("Name", item.Name);
                result.AddItem("Value", item.Value);
                result.AddItem("Rms", item.Rms); 
            }
            return result;
        }

        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="inverseWeight"></param>
        public WeightedVector(IMatrix vector, IMatrix inverseWeight = null)
            : this(vector.GetCol(0), inverseWeight)
        {
            if (vector.RowNames != null && vector.RowNames.Count != 0)
            {
                this.ParamNames = vector.RowNames;
            }
        }
        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector">数据以矩阵形式初始化</param>
        /// <param name="inverseWeight">权逆阵,如果为null，则为单位阵</param>
        public WeightedVector(IVector vector, IMatrix inverseWeight = null)
            : base(vector.OneDimArray)
        {
            if (vector.ParamNames != null && vector.ParamNames.Count != 0)
            {
                this.ParamNames = vector.ParamNames;
            }
            if(inverseWeight == null)
            {
                inverseWeight = Matrix.CreateIdentity(vector.Count);
            }
            SetInverseWeight(new Matrix( inverseWeight));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public WeightedVector(Dictionary<string, RmsedNumeral> data)
        {
            this.Data = new List<double>();
            this.ParamNames = new List<string>();
            var dia = new DiagonalMatrix(data.Count);
            int i = 0;
            foreach (var item in data)
            {
                this.Data.Add(item.Value.Value);
                this.ParamNames.Add(item.Key);
                dia.SetValue(i++, item.Value.Variance);
            }
            this.InverseWeight = new Matrix(dia);
        }


        /// <summary>
        /// 设置权逆阵
        /// </summary>
        /// <param name="inverseWeight"></param>
        public void SetInverseWeight(Matrix inverseWeight)
        {
            if (inverseWeight != null)
            {
                if (!inverseWeight.IsSquare) log.Error("inverseWeight 必须传入方阵！");
                if (this.Count != inverseWeight.ColCount) log.Error("inverseWeight 参数个数与权逆阵维数必须相等！");
            }
            else { inverseWeight = new Matrix( new DiagonalMatrix(this.Count, 1)); }
            this.InverseWeight =  inverseWeight;
        }
        #endregion
        /// <summary>
        /// 行名称
        /// </summary>
        public override List<string> RowNames { get=> base.RowNames ?? ParamNames; set => base.RowNames = value; } 
        /// <summary>
        /// 设置参数名称，包括权阵的名称。
        /// </summary>
        public override List<string> ParamNames
        {
            get { return base.ParamNames; }
            set
            {
                if(value == null) { return; }

                if(this.Count != value.Count)
                {
                    new Geo.IO.Log(typeof(WeightedVector)).Error("参数名称维数不正确！");
                    //throw new Exception("名称维数不正确！");
                }
                if (this.InverseWeight != null)
                {
                    this.InverseWeight.ColNames = value;
                    this.InverseWeight.RowNames = value;
                }
                base.ParamNames = value;
            }
        }

        /// <summary>
        /// 移除指定参数
        /// </summary>
        /// <param name="paramNames"></param>
        public override void Remove(IEnumerable<string> paramNames)
        {
            if (paramNames.Count() == 0) { return; }

            var oldNames = new List<string>(ParamNames);

            base.Remove(paramNames);
            if (this.ParamNames.Count == 0)
            {
                this.InverseWeight = new Matrix(0);
            }
            else
            {
                this.InverseWeight = new Matrix(NamedMatrix.GetNewMatrix(this.ParamNames, oldNames, this.InverseWeight.Array));
            }
        }
        /// <summary>
        /// 按名称顺序排序
        /// </summary>
        /// <returns></returns>
        public new WeightedVector SortByName()
        {
            Dictionary<string, double> dic = GetDictionary();
            var ordered = (from entry in dic
                           orderby entry.Key ascending
                           select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            List<string> oldParamNames = ParamNames;
            List<string> newParamNames = new List<string>(dic.Keys);
            IMatrix invWeight = this.InverseWeight;
            if (!Geo.Utils.ListUtil.IsEqual(oldParamNames, newParamNames))
            {
                invWeight = NamedMatrix.GetSymmetricInOrder(newParamNames, oldParamNames, this.InverseWeight.Array);
            }
            Vector vec = new Vector(dic);

            var result = new WeightedVector(vec, invWeight);
            return result;
        }

        public WeightedVector FilterContains(string key)
        {
            var names = this.ParamNames.FindAll(m=>m.Contains(key));
            return this.GetWeightedVector(names.ToList());
        }

        #region  核心变量 

        public Matrix _InverseWeight { get; protected set; }
        /// <summary>
        /// 强制转换为对称阵，一般以 SymmetricMatrix，或 DiagonalMatrix 表示。
        /// 参数的权逆阵（协因数阵） Inverse Weight Matrix（Cofactor Matrix ）of Some Vector。
        /// 协因数阵。InverseWeight=Weight^(-1)
        /// 法方程系数阵的逆阵为未知参数向量的权逆阵.
        /// ??此处应该为残差的权逆阵？？
        /// </summary> 
        public Matrix InverseWeight { get { return _InverseWeight; }  set
            {
                if (!value.IsSymmetric)//强制转换为对称阵
                {
                    _InverseWeight = new Matrix( new SymmetricMatrix(value.Array));
                }
                else
                {
                    _InverseWeight = value;
                }
            }
        }
        #endregion

        #region 计算属性
        /// <summary>
        /// 是否具有权值。
        /// </summary>
        public bool IsWeighted { get { return InverseWeight != null && InverseWeight.ColCount == Count; } }


        /// <summary>
        /// 权阵
        /// </summary>
        public Matrix Weights { get { return InverseWeight.Inversion; } }
        /// <summary>
        /// 数值数量。
        /// </summary>
        //public int Count { get { return this.Value.Length; } }

        #endregion

        #region 方法

         
        public Matrix GetVectorMatrix()
        {
            return new Matrix((IVector)this, true);
        }

        public bool CheckAndCorrectCova()
        {
            bool isOk = true;
            int rank = this.InverseWeight.ColCount;
            for (int i = 0; i < rank; i++)
            {
                var val = this.InverseWeight[i, i];
                if (val < 0)
                {
                    if (i == 0) { this.InverseWeight[i, i] = 1E-2; }
                    else { this.InverseWeight[i, i] = 1E-10; }
                    isOk = false;
                }
            }
            return isOk;
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="i"></param>
        /// <param name="val"></param>
        /// <param name="cova">方差</param>
        public void Set(int i, double val, double cova)
        {
            this[i] = val;
            this.InverseWeight[i, i] = cova;
        }

        /// <summary>
        /// 获取具有 RMS 的值。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public RmsedNumeral Get(int i)
        {
            return new RmsedNumeral(this[i], Math.Sqrt(this.InverseWeight[i, i]));
        }
        /// <summary>
        /// 获取具有 RMS 的值。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isFuzzyMatching">如果模糊匹配，则返回第一个匹配上的</param>
        /// <returns></returns>
        public RmsedNumeral Get(string key, bool isFuzzyMatching = false)
        {
            int i = -1;
            if (isFuzzyMatching)
            {
                var matchedKey = ParamNames.FirstOrDefault(m => String.Equals(m, key, StringComparison.CurrentCultureIgnoreCase));
                if (!String.IsNullOrEmpty(matchedKey))
                {
                    i = ParamNames.IndexOf(matchedKey);
                }
            }
            else
            {
                i = ParamNames.IndexOf(key);
            }

            if (i == -1) { return RmsedNumeral.NaN; }

            return Get(i);
        }
        /// <summary>
        /// 获取具有 RMS 的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, RmsedNumeral> GetAll(string key)
        {
            var list = new Dictionary<string, RmsedNumeral>();

            var matchedKey = ParamNames.FindAll(m => m.Contains(key));

            foreach (var item in matchedKey)
            {
                int i = -1;
                i = ParamNames.IndexOf(item);

                list.Add(item, Get(i));
            }

            return list;
        }

        /// <summary>
        /// 按照矩阵对角线排序返回。
        /// </summary>
        /// <param name="ascendingOrDescending"></param>
        /// <returns></returns>
        public WeightedVector GetCovaOrdered(bool ascendingOrDescending = true)
        {
            var raw = this.GetRmsVector();
            Vector vector = null;
            vector = ascendingOrDescending ? raw.Sort() : raw.SortDescending();

            return this.GetWeightedVector(vector.ParamNames);
        }

        /// <summary>
        /// 中误差以向量形式返回
        /// </summary>
        /// <returns></returns>
        public Vector GetRmsVector()
        {
            Vector vec = new Vector(this.Count) { ParamNames = new List<string>(ParamNames) };
            for (int i = 0; i < Count; i++)
            {
                double rms = 0;
                var cova = this.InverseWeight[i, i];
                if (cova > 0) { rms = Math.Sqrt(this.InverseWeight[i, i]); }
                else
                {
                    int j = 0;
                }
                vec[i] = rms;
            }
            return vec;
        }

        /// <summary>
        /// 返回一个具有中误差的向量。这个比WeightedVector更加轻量级，更通用。获取了后就可以释放原对象。
        /// </summary>
        /// <returns></returns>
        public RmsedVector GetRmsedVector()
        {
            return new RmsedVector(this.OneDimArray, this.GetRmsVector().OneDimArray, this.ParamNames.ToArray());
        }
        /// <summary>
        /// 转换为参数化的向量
        /// </summary>
        /// <returns></returns>
        public NameRmsedNumeralVector GetNameRmsedNumeralVector()
        {
            return new NameRmsedNumeralVector(this.OneDimArray, this.GetRmsVector().OneDimArray, this.ParamNames.ToArray());
        }

        /// <summary>
        /// 以向量形式返回权向量
        /// </summary>
        /// <returns></returns>
        public Vector GetWeightVector()
        {
            Vector vec = new Vector(this.Count) { ParamNames = ParamNames };
            for (int i = 0; i < Count; i++)
            {
                vec[i] = 1 / (this.InverseWeight[i, i]);
            }
            return vec;
        }
        /// <summary>
        /// 具有协方差的向量，按照指定的参数顺序返回。
        /// </summary>
        /// <param name="paramNames">参数名称</param>
        /// <returns></returns>
        public WeightedVector GetWeightedVector(List<string> paramNames)
        {
            IVector vector = this.GetVector(paramNames);
            if (!this.InverseWeight.IsColNameAvailable)
            { 
                this.InverseWeight.ColNames = this.ParamNames;
            }
            if (!this.InverseWeight.IsRowNameAvailable)
            {
                this.InverseWeight.RowNames = this.ParamNames; 
            }

            IMatrix matrix = this.InverseWeight.GetMatrix(paramNames);
            var vec = new WeightedVector(vector, matrix);
            vec.ParamNames = new List<string>(paramNames);
            return vec;
        }
        /// <summary>
        /// 提取具有协方差的向量
        /// </summary>
        /// <param name="fromParamIndex">其实编号，0开始</param>
        /// <param name="paramCount">参数数量</param>
        /// <returns></returns>
        public WeightedVector GetWeightedVector(int fromParamIndex, int paramCount)
        {
            var vector = this.GetVector(fromParamIndex, paramCount);

            IMatrix matrix = this.InverseWeight.SubMatrix(fromParamIndex, paramCount);
            var vec = new WeightedVector(vector, matrix);
            return vec;
        }

        /// <summary>
        /// 由传入的方差因子计算协方差。
        /// </summary>
        /// <param name="varianceFactor">方差因子</param>
        /// <returns></returns>
        public Matrix GetCovariance(double varianceFactor = 1)
        {
            return this.InverseWeight * varianceFactor;
        }

        /// <summary>
        /// 获取指定编号值的方差。
        /// </summary>
        /// <param name="i"></param>
        /// <param name="varianceFactor">方差因子</param>
        /// <returns></returns>
        public double GetVarianceValue(int i, double varianceFactor = 1)
        {
            return this.InverseWeight[i, i] * varianceFactor;
        }

        /// <summary>
        /// 获取指定编号值的权值，即方差倒数。
        /// </summary>
        /// <param name="i"></param>
        /// <param name="varianceFactor">方差因子</param>
        /// <returns></returns>
        public double GetWeithValue(int i, double varianceFactor = 1)
        {
            return 1.0 / GetVarianceValue(i, varianceFactor);
        }

        /// <summary>
        /// 数值，及其均方差。
        /// </summary>
        /// <param name="i">编号</param>
        /// <returns></returns>
        public Geo.RmsedNumeral GetInverseWeightedValue(int i)
        {
            return new RmsedNumeral(this[i], Math.Sqrt(this.InverseWeight[i, i]));
        }

        /// <summary>
        /// 只对 Vector 做乘法
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static WeightedVector operator *(WeightedVector right, double left)
        { 
            var vector = right.Multiply(left);
            return new WeightedVector(vector, right.InverseWeight) { ParamNames = right.ParamNames };
        }
        ///// <summary>
        ///// 加上。
        ///// </summary>
        ///// <param name="right"></param>
        ///// <param name="left"></param>
        ///// <returns></returns>
        //public static WeightedVector operator +(WeightedVector right, WeightedVector left)
        //{
        //    Vector vector = (Vector)right + (Vector)left;
        //    IMatrix InverseWeight = right.InverseWeight.Plus(left.InverseWeight);
        //    return new WeightedVector(vector, InverseWeight) { ParamNames = right.ParamNames };
        //}

        /// <summary>
        /// 乘以一个常量向量（无误差），应用协方差传播定律。
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static RmsedNumeral operator *(WeightedVector right, Vector left)
        {
           
            if (right.Count != left.Count) { throw new Exception("只支持同维数的向量！"); }
            var length = right.Count;
            double val = 0;

            for (int i = 0; i < length; i++)
            {
                val += right[i] * left[i];
            }
            Matrix mRight = new Matrix(right.InverseWeight);
            Matrix mLeft = new Matrix(left.OneDimArray, length, 1);//一维的列矩阵
            var Dx = mLeft.Trans * mRight * mLeft;
            var rms = Math.Sqrt(Dx[0, 0]);

            return new RmsedNumeral(val, rms);
        }

        /// <summary>
        /// 加上一个常量向量（无误差）。
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static WeightedVector operator +(WeightedVector right, Vector left)
        {
            if (left == null) { return right; }
            Vector vector = (Vector)right + (Vector)left;
            IMatrix InverseWeight = right.InverseWeight;
            return new WeightedVector(vector, InverseWeight) { ParamNames = right.ParamNames };
        }
        /// <summary>
        /// 直接减去一个常量向量（无误差）。
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static WeightedVector operator -(WeightedVector right, Vector left)
        {
            if(left == null) { return right; }
            Vector vector = (Vector)right - (Vector)left;
            IMatrix InverseWeight = right.InverseWeight;
            return new WeightedVector(vector, InverseWeight) { ParamNames = right.ParamNames };
        }

        /// <summary>
        /// 减去。
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static WeightedVector operator -(WeightedVector right, WeightedVector left)
        {
            if (left == null) { return right; }
            Vector vector = (Vector)right - (Vector)left;
            IMatrix InverseWeight = right.InverseWeight.Plus(left.InverseWeight);
            return new WeightedVector(vector, InverseWeight) { ParamNames = right.ParamNames };
        }

        #endregion

        public void Dispose()
        {
            this.InverseWeight = null;
        }
        /// <summary>
        /// 值全为 0,权阵也为 1 。
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static WeightedVector GetZeroVector(int len, double inverseWeight = 1)
        {
            return new WeightedVector(new Vector(len), new ArrayMatrix(len, len, inverseWeight, true));
        }
        /// <summary>
        /// 生成一个随机的
        /// </summary>
        /// <param name="len"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static WeightedVector GenerateATest(int len, int seed = 1)
        {
            return new WeightedVector( new Vector(len, 1.0), Matrix.CreateIdentity(len));
        }

        #region  IO


        /// <summary>
        /// 显示值与均方差。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Count.ToString());
            sb.Append(" ");
            int i = 0;
            foreach (double item in this)
            {
                sb.Append(item.ToString("0.0000"));
                sb.Append("(");
                var cova = this.InverseWeight[i, i];
                sb.Append(String.Format(new NumeralFormatProvider(), "{0}", cova >= 0 ? Math.Sqrt(cova) : 1e-20));
                sb.Append(")");

                sb.Append(" ");
                i++;
            }
            return sb.ToString();

            return String.Format(new EnumerableFormatProvider(), "{0:\t}", this);
        }
        /// <summary>
        /// 格式化的文本，方便阅读。
        /// </summary>
        /// <returns></returns>
        public string ToFormatedText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MatrixUtil.GetFormatedText(this.OneDimArray));
            sb.Append(MatrixUtil.GetFormatedText(InverseWeight.Array));
            return sb.ToString();
        }
        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <returns></returns>
        public override string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            var vecStr = base.ToReadableText();
            var matStr = new Matrix( this.InverseWeight).ToReadableText(splitter);

            sb.Append(vecStr);
            sb.Append(matStr);

            return sb.ToString();
        }
        /// <summary>
        /// 返回两行可读数据
        /// </summary>
        /// <returns></returns>
        public string ToTwoLineText()
        {
            var str = Geo.Utils.StringUtil.ToString(this.OneDimArray);
            str += "\r\n";
            str += Geo.Utils.StringUtil.ToString(this.InverseWeight.GetDiagonal().OneDimArray);
            return str;
        }


        /// <summary>
        /// 解析.如果没有矩阵数据，则采用单位阵 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public new static WeightedVector Parse(string text, string[] splitter = null)
        {
            if(String.IsNullOrWhiteSpace(text)) { return null; }
            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) { return null; }

            return Parse(lines, splitter);
        }
        /// <summary>
        /// 解析行.如果没有矩阵数据，则采用单位阵 
        /// </summary>
        /// <param name="splitter"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static WeightedVector Parse(string[] lines, string[] splitter = null)
        {
            if (splitter == null)
            {
                splitter = new string[] { ",", ";", "\t", " " };
            }
            int len = lines.Length;
            //首先查找是否有乘号,以及其位置
            int startIndexOfVector = 0;
            int rowCountOfVector = 0;

            int startIndexOfMatrix = 0;
            int rowCountOfMatrix = 0;

            bool isFirstX = true;
            for (int i = 0; i < len; i++)
            {
                var line = lines[i];
                if (line.Contains("×"))
                {

                    var items = line.Split(new string[] { "×" }, StringSplitOptions.RemoveEmptyEntries);
                    int rowCount = int.Parse(items[0]);
                    int colCount = 0;
                    if (items.Length >= 2)
                    {
                        colCount = int.Parse(items[1]);
                    }

                    if (isFirstX)
                    {
                        startIndexOfVector = i;
                        rowCountOfVector = rowCount;
                        isFirstX = false; 
                    }
                    else
                    {
                        startIndexOfMatrix = i;
                        rowCountOfMatrix = rowCount;
                        break;
                    }
                }
            }

            List<string> vecLines = new List<string>();
            int endIndex = startIndexOfVector + rowCountOfVector;//包含第一个乘号行
            for (int i = startIndexOfVector; i <= endIndex; i++)
            {
                vecLines.Add(lines[i]);
            }

            Vector vector = Vector.Parse(vecLines.ToArray());

            List<string> matLines = new List<string>();
            endIndex = startIndexOfMatrix + rowCountOfMatrix;
            for (int i = startIndexOfMatrix; i <= endIndex; i++)
            {
                matLines.Add(lines[i]);
            }
            Matrix matrix = null;
            if (matLines.Count <= 1)//如果没有矩阵数据，则采用单位阵 
            {
                matrix = Matrix.CreateIdentity(vector.Count);
            }
            else
            {
                matrix = Matrix.Parse(matLines.ToArray());
            } 

            return new WeightedVector(vector, matrix);
        }

        #endregion

    }
}