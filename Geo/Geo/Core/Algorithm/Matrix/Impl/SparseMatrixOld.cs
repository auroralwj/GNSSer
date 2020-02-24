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
   
    /// <summary>
    ///  稀疏矩阵存储器
    ///  以二进制文件形式保存。
    ///  格式定义：1. 前3个为int，分别指行、列和集合数量；2.后面子项集合，分别为行（int）列（int）标号和双精度值。
    /// </summary>
    public class SparseMatrixOld : BaseMatrix
    {
        public class SparseMatrixOldFactory : IMatrixFactory
        {
            public IMatrix Create(int rowCount, int colCount)
            {
                return new SparseMatrixOld(rowCount, colCount);
            }

            public IMatrix Create(double[][] array)
            {
                return new SparseMatrixOld(array.Length, array[0].Length);
            }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SparseMatrixOld(int row, int col):
            base(MatrixType.Sparse)
        {
            this.rowCount = row;
            this.colCount = col;
            this.data = new Dictionary<int, Dictionary<int, MatrixItem>>();  
        }
        /// <summary>
        /// 稀疏矩阵存储器
        /// </summary>
        /// <param name="matrix"></param>
        public SparseMatrixOld(double[][] matrix) :
            base(MatrixType.Sparse)
        { 
            Init(matrix);
        }

        private void Init(double[][] matrix)
        {
            this.rowCount = matrix.Length;
            this.colCount = matrix[0].Length;
            this.data = new Dictionary<int, Dictionary<int, MatrixItem>>(); 

            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColCount; j++)
                {
                    if (matrix[i][j] != 0)
                        this.Add(new MatrixItem() { Row = i, Col = j, Value = matrix[i][j] });
                }
            }
        }

        public SparseMatrixOld(IMatrix matrix) :
            base(MatrixType.Sparse)
        {
            if (matrix.MatrixType == Algorithm.MatrixType.Diagonal)
            {
                this.rowCount = matrix.RowCount;
                this.colCount = matrix.ColCount;
                this.data = new Dictionary<int, Dictionary<int, MatrixItem>>(); 

                for (int i = 0; i < this.RowCount; i++)
                {
                    this.MatrixItems.Add(new MatrixItem() { Row = i, Col = i, Value = matrix[i, i] });
                }
            }
            else
            {
                Init(matrix.Array);
            } 
        }

        //KeyDictionary<>

        int rowCount;
        int colCount;
        /// <summary>
        /// 行数
        /// </summary>
        public override int RowCount { get { return rowCount; } }
        /// <summary>
        /// 列数量
        /// </summary>
        public override int ColCount { get { return colCount; } }
        /// <summary>
        /// 非 0 值集合。
        /// </summary>
        public List<MatrixItem> MatrixItems
        {
            get { var list = new List<MatrixItem>(); foreach (var item in data) { list.AddRange(item.Value.Values); } return list; }
        }

        public  Dictionary<int, Dictionary<int, MatrixItem>> data { get; set; }

        public override int ItemCount
        {
            get { return MatrixItems.Count *3; }
        }

        /// <summary>
        /// 获取二维数组矩阵。
        /// </summary>
        /// <returns></returns>
        public double[][] GetMatrix()
        {
            double [][] matrix = MatrixUtil.Create(RowCount, ColCount);
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
        public static SparseMatrixOld FromBinary(string path)
        {
            SparseMatrixOld matr = null;
            using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
            {
                int RowCount = br.ReadInt32();
                int ColCount = br.ReadInt32();
                matr = new SparseMatrixOld(RowCount, ColCount);
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
                if (this.data.ContainsKey(i)) { if (this.data[i].ContainsKey(j)) { return data[i][j].Value; } }
                return 0;
            }
            set { if (value != 0) { this.Add( new MatrixItem(i, j, value)); } }
        }


        public override IMatrix Clone()
        {
            throw new NotImplementedException();
        }



        public SparseMatrixOld Transpositions
        {
            get
            {
                IMatrix matrix = new ArrayMatrix(ColCount, RowCount);
                foreach (var item in MatrixItems)
                {
                    matrix[item.Col,item.Row] = item.Value;
                }
                return new SparseMatrixOld(matrix);                
            }
        }

        public override IMatrix Multiply(IMatrix right)
        {
            int row = this.rowCount;
            int col = right.ColCount;
            double[][] rightArray = right.Transposition.Array;
            double[] rightVector = new double[right.ColCount];
            double[][] x = new double[row][];

            if (row < 100)
            {
                for (int i = 0; i < this.rowCount; i++)
                {
                    if (this.data.ContainsKey(i)) continue;

                    x[i] = MultiplyRowOfSparse(rightArray, rightVector, i);
                }
            }
            else 
            {
                Parallel.For(0, row, new Action<int>(delegate(int i)
                {
                    x[i] = MultiplyRowOfSparse(rightArray, rightVector, i);
                }));
            }

            return new ArrayMatrix(x); 
        }

        private double[] MultiplyRowOfSparse( double[][] rightArray, double[] rightVector, int i)
        {
            int col = rightArray.Length;
            double[] rowX = new double[col];
            for (int j = 0; j < col; j++)
            {
                rightVector = rightArray[j];
                double sum = 0;
                foreach (var item in this.data[i])
                {
                    int q = item.Key;
                    sum += item.Value.Value * rightVector[q];
                }
                rowX[j] = sum;
            }
            return rowX;
        }

        public override IMatrix GetInverse()
        {
            return base.GetInverse();
            //throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            SparseMatrixOld sm = obj as SparseMatrixOld;
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
            return  this.ItemCount * 13; 
        }
        /// <summary>
        /// 增加一个，如已有，则覆盖。
        /// </summary>
        /// <param name="key"></param>
        public void Add(MatrixItem item)
        {
            if(!this.data.ContainsKey(item.Row)){this.data[item.Row]= new Dictionary<int,MatrixItem>();}
            this.data[item.Row][item.Col] = item;
        }

        public override IMatrix SubMatrix(int fromIndex, int count)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "SparseMatrixOld " + base.ToString();
        }
    }

   /// <summary>
   /// 矩阵中的一个数。三元组。
   /// 改进方法：行列标号无符号，可用uint表示。
   /// </summary>
    public  class MatrixItem : IComparable<MatrixItem>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MatrixItem() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="val"></param>
        public MatrixItem(int row, int col, double val) { this.Row = row; this.Col = col; this.Value = val; }
        /// <summary>
        /// 行标号，从0 开始
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// 列标号，从 0 开始
        /// </summary>
        public int Col { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public double Value { get; set; }

        public override bool Equals(object obj)
        {
            MatrixItem other = obj as MatrixItem;
            if (other == null) return false;

            return other.Col == this.Col
                && this.Row == other.Row
                && this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() * 3 + Col.GetHashCode() * 5;
        }

        public override string ToString()
        {
            return  Row.ToString() + ","+ Col +","+ Value;
        }

        public int CompareTo(MatrixItem other)
        {
            if (this.Row == other.Row) return this.Col - other.Col;
            return (this.Row - other.Row) ;
        }
    }
}
