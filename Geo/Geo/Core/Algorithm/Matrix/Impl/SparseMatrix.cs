//2013.06.05, czs,  created, 稀疏矩阵存储器

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using System.Threading.Tasks;

namespace Geo.Algorithm
{
    //2016.12.12,czs & double, create in hongqing, 双键字典类
    /// <summary>
    /// 双键字典类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DoubleKeyDictionary<TKey, TValue> : BaseDictionary<TKey, Dictionary<TKey, TValue>>
          where TKey : IComparable<TKey>

    {

        public DoubleKeyDictionary()
        {
           // this.data = new Dictionary<TKey, Dictionary<TKey, TValue>>();

        }

        //protected Dictionary<TKey, Dictionary<TKey, TValue>> data { get; set; }
        public override Dictionary<TKey, TValue> Create(TKey key)
        {
            return new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="val"></param>
        public void SetValue(TKey key1, TKey key2, TValue val)
        {
            this.GetOrCreate(key1)[key2] = val;
            //if (!data.ContainsKey(key1)) { data[key1] = new Dictionary<TKey, TValue>(); }
            //data[key1][key2] = val;

        }

        /// <summary>
        /// 直接获取数值，速度最快，但是可能出错。
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public TValue GetValue(TKey key1, TKey key2)
        {
            return this[key1][key2];
        }
        /// <summary>
        /// 判断后获取，如果失败则返回默认值。
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public TValue TryGetValue(TKey key1, TKey key2)
        {
            if (!Data.ContainsKey(key1))
            {
                return default(TValue);
            }
            var dic2 = Data[key1];
            if (!dic2.ContainsKey(key2))
            {
                return default(TValue);
            }
            return dic2[key2];
        }

         
    }


    /// <summary>
    ///  稀疏矩阵存储器
    ///  以二进制文件形式保存。
    ///  格式定义：1. 前3个为int，分别指行、列和集合数量；2.后面子项集合，分别为行（int）列（int）标号和双精度值。
    /// </summary>
    public class SparseMatrix : BaseMatrix
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SparseMatrix(int row, int col) :
            base(MatrixType.Sparse)
        {
            this.rowCount = row;
            this.colCount = col;
            this.data = new DoubleKeyDictionary<int, double>(); 
        }
        /// <summary>
        /// 稀疏矩阵存储器
        /// </summary>
        /// <param name="matrix"></param>
        public SparseMatrix(double[][] matrix) :
            base(MatrixType.Sparse)
        {
             
            Init(matrix);
        }

        private void Init(double[][] matrix)
        {
            this.rowCount = matrix.Length;
            this.colCount = matrix[0].Length;
            this.data = new DoubleKeyDictionary<int, double>();

            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColCount; j++)
                {
                    if (matrix[i][j] != 0)
                        this.Add( i,  j, matrix[i][j] );
                }
            }
        }

        public SparseMatrix(IMatrix matrix) :
            base(MatrixType.Sparse)
        {
            this.ColNames = matrix.ColNames;
            this.RowNames = matrix.RowNames;
           SetRowColCount(matrix.RowCount, matrix.ColCount);

            if (matrix.MatrixType == Algorithm.MatrixType.Diagonal)
            {
                this.data = new DoubleKeyDictionary<int, double>();
                for (int i = 0; i < this.RowCount; i++)
                {
                    this.Add(i, i, matrix[i, i]);
                }
            }else if(matrix.MatrixType == MatrixType.Sparse)
            {
                var mat = matrix as SparseMatrix;
                this.data = new DoubleKeyDictionary<int, double>();
                foreach (var kv in mat.data.Data)
                {
                    foreach (var item in kv.Value)
                    {
                        this.Add(kv.Key, item.Key, item.Value);
                    }
                } 
            }
            else
            {
                Init(matrix.Array);
            } 
        }

        //KeyDictionary<>

        private int rowCount;
        private int colCount;
        /// <summary>
        /// 行数
        /// </summary>
        public override int RowCount { get { if (rowCount == 0) { if (this.RowNames != null) { return this.RowNames.Count; } } return rowCount; } }
        /// <summary>
        /// 列数量
        /// </summary>
        public override int ColCount { get { return colCount; } }
        /// <summary>
        /// 设置矩阵大小，必须设置，否则不知道真实大小。
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        public void SetRowColCount(int rowCount, int colCount)
        {
            this.rowCount = rowCount;
            this.colCount = colCount;

        }
        /// <summary>
        /// 非 0 值集合。
        /// </summary>
        public List<MatrixItem> MatrixItems
        {
            get
            {
                var list = new List<MatrixItem>();
                foreach (var item in data.Keys)
                {
                    var dic = data[item];
                    foreach (var item1 in dic)
                    {
                        var it = new MatrixItem(item, item1.Key, item1.Value);
                        list.Add(it);
                    }

                }
                return list;
            }
        }
        public DoubleKeyDictionary<int, double> Data { get { return data; } }
        private DoubleKeyDictionary<int, double> data { get; set; }

        public override int ItemCount
        {
            get { return MatrixItems.Count * 3; }
        }

        /// <summary>
        /// 增加一个系数和值
        /// </summary>
        /// <param name="rowName"></param>
        /// <param name="colName"></param>
        /// <param name="val"></param>
        public void Add(string rowName, string colName, double val)
        {
            var iRow = this.RowNames.IndexOf(rowName);
            var iCol = this.ColNames.IndexOf(colName);

            if (iRow ==-1)
            {
                iRow = this.RowNames.Count;
                this.RowNames.Add(rowName);
                this.SetRowColCount(this.RowCount + 1, this.ColCount);
            }
            if (iCol == -1)
            {
                iCol = ColNames.Count;
                this.ColNames.Add(colName);
                this.SetRowColCount(this.RowCount, this.ColCount + 1);
            }
            //当Names共享时，自动判断
            this.SetRowColCount( this.RowNames.Count, this.ColNames.Count);
            this.Add(iRow, iCol, val);

        }

        /// <summary>
        /// 获取二维数组矩阵。
        /// </summary>
        /// <returns></returns>
        public double[][] GetMatrix()
        {
            double[][] matrix = MatrixUtil.Create(RowCount, ColCount);
            foreach (var item in MatrixItems)
            {
                matrix[item.Row][item.Col] = item.Value;
            }
            return matrix;
        }

        #region 二进制IO
        /// <summary>
        /// 以二进制文件形式保存。
        /// 格式定义：1. 前3个为int，分别指行、列和集合数量；2.后面子项集合，分别为行（int）列（int）标号和双精度值。
        /// </summary> 
        /// <param name="path">路径</param>
        public void ToBinary(string path)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create, FileAccess.Write)))
            {
                bw.Write(RowCount);
                bw.Write(ColCount);
                bw.Write(MatrixItems.Count);


                foreach (var item in MatrixItems)
                {
                    bw.Write(item.Row);
                    bw.Write(item.Col);
                    bw.Write(item.Value);
                }
            }
        }
        /// <summary>
        /// 从二进制文件中读取。
        ///  格式定义：1. 前3个为int，分别指行、列和集合数量；2.后面子项集合，分别为行（int）列（int）标号和双精度值。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SparseMatrix FromBinary(string path)
        {
            SparseMatrix matr = null;
            using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
            {
                int RowCount = br.ReadInt32();
                int ColCount = br.ReadInt32();
                matr = new SparseMatrix(RowCount, ColCount);
                int itemCount = br.ReadInt32();
                for (int i = 0; i < itemCount; i++)
                {
                    //各个节点位置无关，可以考虑并行存取，但是由于内存共享，还得序列化读取。
                    MatrixItem item = new MatrixItem() { Row = br.ReadInt32(), Col = br.ReadInt32(), Value = br.ReadDouble() };
                    matr.MatrixItems.Add(item);
                }

                return matr;
            }
        }
        #endregion

        #region 通用矩阵接口的实现
        /// <summary>
        /// 返回二维数组
        /// </summary>
        public override double[][] Array { get { return GetMatrix(); } }

        #endregion

        public override double this[int i, int j]
        {
            get
            {
                return data.TryGetValue(i, j);
            }
            set { if (value != 0) { this.Add(i, j, value); } }
        }

        public override double this[string rowName, string colName]
        {
            get
            {
                var row = this.RowNames.IndexOf(rowName);
                var col = this.ColNames.IndexOf(colName);
                return data.TryGetValue(row, col);
            }
            set { if (value != 0) { this.Add(rowName, colName, value); } }
        }


        public override IMatrix Clone()
        {
            throw new NotImplementedException();
        }



        public override IMatrix Transposition
        {
            get
            {
                IMatrix matrix = new ArrayMatrix(ColCount, RowCount);
                foreach (var item in MatrixItems)
                {
                    matrix[item.Col, item.Row] = item.Value;
                }
                return new SparseMatrix(matrix);
            }
        }

        public override IMatrix Multiply(IMatrix right)
        {
            int row = this.rowCount;
            int col = right.ColCount;
            double[][] rightArray = right.Transposition.Array;
            double[] rightVector = new double[right.ColCount];
            double[][] x = new double[row][];

            if (row < 800)
            {
                for (int i = 0; i < this.rowCount; i++)
                {
                    x[i] = MultiplyRowOfSparse(rightArray,  i);
                }
            }
            else
            {
                Parallel.For(0, row, new Action<int>(delegate(int i)
                {
                    x[i] = MultiplyRowOfSparse(rightArray,  i);
                }));
            }

            return new ArrayMatrix(x);
        }

        private double[] MultiplyRowOfSparse(double[][] rightArray, int i)
        {
            int col = rightArray.Length;
            double[] rowX = new double[col];
            for (int j = 0; j < col; j++)
            {
                if (!this.data.Contains(i)) continue;
                double[] rightVector = rightArray[j];
                double sum = MultiplyAndSumOfSparse(i, rightVector);
                rowX[j] = sum;
            }
            return rowX;
        }

        private double MultiplyAndSumOfSparse(int i, double[] rightVector)
        {
            double sum = 0;
            foreach (var item in this.data[i])
            {
                int q = item.Key;
                sum += item.Value * rightVector[q];
            }
            return sum;
        }

        public override IMatrix GetInverse()
        {
            return base.GetInverse();
            //throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            SparseMatrix sm = obj as SparseMatrix;
            if (sm != null)
            {
                if (this.ItemCount != sm.ItemCount
                    || this.RowCount != sm.RowCount
                    || this.ColCount != sm.ColCount
                    )
                    return false;
                var list = this.MatrixItems;
                list.Sort();
                var list2 = sm.MatrixItems;
                list2.Sort();
                int i = 0;
                foreach (var item in this.MatrixItems)
                {
                    if (!list[i].Equals(list2[i])) return false;
                    i++;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return this.ItemCount * 13;
        }
        /// <summary>
        /// 增加一个，如已有，则覆盖。
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="item"></param>
        public void Add(int i, int j, double item)
        {
            this.data.SetValue(i, j, item); 
        }

        public override IMatrix SubMatrix(int fromIndex, int count)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "SparseMatrix " + base.ToString();
        }
    }

}