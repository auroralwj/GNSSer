//2014.10.02, czs,  created, 对称矩阵
//2016.10.20, czs & double edit in hongqing, 改进了乘法和求逆
//2017.06.22, czs , edit in hongqing, 修正对称阵减法错误。

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geo.Common;
using Geo.Utils;
using System.Threading.Tasks;

namespace Geo.Algorithm
{

    /// <summary>
    ///  对称矩阵(应该是对称正定矩阵，即对角线全是非零元素，否则不可求逆)，只存储下三角，以一个一维数组存储的矩阵。 宋力杰模式。
    /// </summary>
    public class SymmetricMatrix : BaseMatrix
    {
         
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dimension">维数</param>
        /// <param name="initVal">初始值</param>
        public SymmetricMatrix(int dimension, double initVal = 0)
            : base(MatrixType.Symmetric)
        {
            int count = (dimension + 1) * dimension / 2; 

            this._vector = new double[count];
            this.dimension = dimension;
             

            if (initVal != 0) //是非 0 才赋值，节约内存
                for (int i = 0; i < count; i++)
                {
                    _vector[i] = initVal;
                }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isSymDownTriOrDiagnoal">一维数组</param>
        /// <param name="vector">一维数组</param>
        public SymmetricMatrix(double[] vector, bool isSymDownTriOrDiagnoal = true)
            : base(MatrixType.Symmetric)
        {
            if (isSymDownTriOrDiagnoal)
            {
                this._vector = vector;
                SetDimention(_vector.Length);
            }
            else
            {
                this.dimension = vector.Length;
                int count = (dimension + 1) * dimension / 2;
                //  int count = rows.Length;
                this._vector = new double[count];
                for (int i = 0; i < dimension; i++)
                {
                    _vector[GetIndex(i, i)] = vector[i];
                }
            }
        }

        private void SetDimention(int len)
        {
            //笨办法解一元二次方程
            for (int i = 1; i <= len; i++)
            {
                int count = (i + 1) * i / 2;
                if (count == len)
                {
                    this.dimension = i;
                    break;
                }
            }
            if (dimension == 0)
                throw new Exception(" 输入的元素数量不正确！ 应该满足 N!");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matrix">下三角一维数组</param>
        static public SymmetricMatrix Parse(Matrix matrix)
        {
            IMatrix mat = matrix;
            while(mat is Matrix)
            {
                mat = ((Matrix)mat)._matrix;
            }
            return Parse(mat);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matrix">下三角一维数组</param>
        static public SymmetricMatrix Parse(IMatrix matrix)
        {
            if (matrix is SymmetricMatrix)
            {
                return (matrix as SymmetricMatrix);
            }
            if (matrix is DiagonalMatrix)
            {
                return new SymmetricMatrix((matrix as DiagonalMatrix).Vector, true);
            }
            return new SymmetricMatrix(matrix.Array);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matrix">二维数组</param>
        public SymmetricMatrix(double[][] matrix)
            : base(MatrixType.Symmetric)
        {
            if (matrix == null) throw new ArgumentException("参数不能为空！");
            if (matrix.Length != matrix[0].Length)
                throw new DimentionException("必须是方阵！");
            this.dimension = matrix.Length;
            this._vector = MatrixUtil.GetDownTriangle(matrix); 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        public SymmetricMatrix(IVector[] rows)
            : base(MatrixType.Symmetric)
        {
            if (rows == null) throw new ArgumentException("参数不能为空！");
            if (rows.Length != rows[0].Count)
                throw new DimentionException("必须是方阵！");
            this.dimension = rows.Length;
            int count = (rows.Length + 1) * rows.Length / 2;
          //  int count = rows.Length;
            this._vector = new double[count];
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    _vector[GetIndex(i, j)] = rows[i][ j];
                }
            }
            this.dimension = rows.Length;  
        }
        #endregion

        #region 核心存储
        double[] _vector;
        int dimension = 0;
        /// <summary>
        /// 一维数组表示的矩阵
        /// </summary>
        public double[] Vector { get { return this._vector; } }
        /// <summary>
        /// 数量
        /// </summary>
        public override int ItemCount
        {
            get { return Vector.Length; }
        }
        #endregion

        #region 操作数
        #region  与自身的操作数
        public static SymmetricMatrix operator +(SymmetricMatrix left, SymmetricMatrix right)
        {
            return new SymmetricMatrix(MatrixUtil.GetPlus(left.Vector, right.Vector));
        }
        public static SymmetricMatrix operator -(SymmetricMatrix left, SymmetricMatrix right)
        {
            return new SymmetricMatrix(MatrixUtil.GetMinus(left.Vector, right.Vector));
        }
        /// <summary>
        /// 对称阵相乘，不再是对称阵？
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ArrayMatrix operator *(SymmetricMatrix left, SymmetricMatrix right)
        {
            //是否有更简便的方法？？？
            ArrayMatrix mleft = new ArrayMatrix(left.Array);
            ArrayMatrix mright = new ArrayMatrix(right.Array);
            var result = mleft * mright;
            return result;  
        }
        public override IMatrix Multiply(IMatrix right)
        {
            if (right.RowCount != this.ColCount)
            {
                throw new ArgumentException("Matrix dimensions are not valid.");
            }
            if (!(right is SparseMatrix)) return base.Multiply(right);            
            var right1=right as SparseMatrix;
            int rowCount = this.RowCount;
            int col = right.ColCount;
            double[][] x = new double[rowCount][];// X.Array;
            double[][] aArray = this.Array;
            double[][] bArray = right.Transposition.Array;
            var right1T = right1.Transposition as SparseMatrix;
            if (rowCount <= 50)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    x[i] = MultiplyRow(aArray[i], right1T.Data, col);
                }
            }
            else
            {
                Parallel.For(0, rowCount, new Action<int>(delegate(int i)
                {
                    x[i] = MultiplyRow(aArray[i], right1T.Data, col);
                }));
            }
            return new Geo.Algorithm.ArrayMatrix(x);   
        }
        private static double[] MultiplyRow(double[] leftArray, DoubleKeyDictionary<int, double> rightSparseMatrix, int col)
        {
            double[] rowX = new double[col];
            
            for (int colIndex = 0; colIndex < col; colIndex++)
            {
                if (!rightSparseMatrix.Contains(colIndex)) continue;
                double cellVal = 0;
                foreach (var sparseVector in rightSparseMatrix[colIndex])
                {
                    int q = sparseVector.Key;
                    cellVal += sparseVector.Value * leftArray[q];
                }
                rowX[colIndex] = cellVal;
            }
            return rowX;
        }
        public static SymmetricMatrix operator *(SymmetricMatrix left, double right)
        {
            return new SymmetricMatrix(MatrixUtil.GetMultiply(left.Vector, right));
        }
        public static SymmetricMatrix operator -(SymmetricMatrix left)
        {
            return new SymmetricMatrix(MatrixUtil.GetOpposite(left.Vector));
        }
        #endregion
        #region  与其他的操作数，如对称矩阵，则返回一个对称矩阵
        public static SymmetricMatrix operator +(SymmetricMatrix left,DiagonalMatrix  right)
        {
            return (SymmetricMatrix)right.GetPlus(left);
        }


        #endregion
        #endregion


        #region 通用矩阵接口的实现 
        /// <summary>
        /// 行数
        /// </summary>
        public override int RowCount { get { return dimension; } }
        /// <summary>
        /// 列数量
        /// </summary>
        public override int ColCount { get { return RowCount; } }
        /// <summary>
        /// 返回二维数组
        /// </summary>
        public override double[][] Array
        {
            get
            {
                double[][] array = new double[RowCount][];
                for (int i = 0; i < RowCount; i++)
                {
                    array[i] = new double[RowCount];
                    for (int j = 0; j <= i; j++)
                    {
                        array[i][j] = _vector[GetIndex(i, j)];

                        if (i != j) array[j][i] = array[i][j];
                    }
                }

                return array;
            }
        }
        /// <summary>
        /// 是否对称
        /// </summary>
        public override bool IsSymmetric     {   get { return true; }   }
        /// <summary>
        /// 检索器
        /// </summary>
        /// <param name="i">行号</param>
        /// <param name="j">列号</param>
        /// <returns></returns>
        public override double this[int i, int j]
        {
            get
            {
                if (i >= j) return _vector[GetIndex(i, j)];
                else return _vector[j * (j + 1) / 2 + i];
            }
            set
            {
                if (i >= j) _vector[GetIndex(i, j)] = value;
                else _vector[j * (j + 1) / 2 + i] = value;
            }
        }
        /// <summary>
        /// 转置相同。
        /// </summary>
        public override IMatrix Transposition { get {  return Clone(); } }
        /// <summary>
        /// 矩阵求逆。
        /// 参照宋力杰的红包书提供的求逆方法。
        /// </summary>
        /// <returns></returns>
        public override IMatrix GetInverse()
        {

            return Inverse();
            
        }

        /// <summary>
        /// 求逆
        /// </summary>
        /// <returns></returns>
        public SymmetricMatrix Inverse()
        {
            //Geo.Algorithm.ArrayMatrix matrix = new Geo.Algorithm.ArrayMatrix(this.Array);
            //return new SymmetricMatrix( matrix.Inverse.Array);
            SymmetricMatrix result = null;
            if(this.dimension > 1000)
            {
                result =   InverseDecimal();
            }
            else
            {
                result = InverseDouble();
            }
              

            //check
            if (false)
            {
                var checkIdentity = result * this;
                if (!checkIdentity.IsIdentity)
                {
                    int aa = 0;
                    log.Warn("宋力杰求逆非单位阵");
                }
            }

            return result;
        }

        public SymmetricMatrix InverseDouble()
        {
            int i, j, k;
            int count = (this.RowCount + 1) * this.RowCount / 2;//阶乘

            var inverse = new double[count];
            for (i = 0; i < count; i++)//copy
            {
                inverse[i] = (this.Vector[i]);////copy 
            }
            var dat = new double[this.RowCount];
            for (k = 0; k < this.RowCount; k++)
            {
                var a00 = inverse[0];
                if (a00 == 0)
                {
                    log.Error("三角阵求逆出错，第一个数字为 0 ");
                    throw new Exception("三角阵求逆出错，第一个数字为 0 ");
                }
                for (i = 1; i < RowCount; i++)
                {
                    var startIndexOfRow = GetStarIndexOfRow(i);
                    var ai0 = inverse[startIndexOfRow];
                    if (i <= RowCount - k - 1) { dat[i] = -ai0 / a00; }
                    else { dat[i] = ai0 / a00; }

                    for (j = 1; j <= i; j++)
                    {
                        inverse[GetIndex(i - 1, j - 1)] = inverse[GetIndex(i, j)] + ai0 * dat[j];
                    }
                }
                if (RowCount > 500)//并行赋值
                {
                    Parallel.For(1, RowCount, new Action<int>(delegate (int ii)
                    {
                        inverse[GetIndex(RowCount - 1, ii - 1)] = dat[ii];
                    }));
                }
                else
                {
                    for (i = 1; i < RowCount; i++)
                    {
                        inverse[GetIndex(RowCount - 1, i - 1)] = dat[i];
                    }
                }

                inverse[GetIndex(RowCount, -1)] = 1.0/ a00; //最后一个
            }

            var result = new SymmetricMatrix((inverse));
            return result;
        }
        public SymmetricMatrix InverseDecimal()
        {
            int i, j, k;
            int count = (this.RowCount + 1) * this.RowCount / 2;//阶乘

            decimal[] inverse = new decimal[count];
            for (i = 0; i < count; i++)
            {
                inverse[i] = new decimal(this.Vector[i]);//copy, 转换成decimal
            }
            decimal[] dat = new decimal[this.RowCount];
            for (k = 0; k < this.RowCount; k++)
            {
                var a00 = inverse[0];
                if (a00 == 0)
                {
                    log.Error("三角阵求逆出错，第一个数字为 0 ");
                    throw new Exception("三角阵求逆出错，第一个数字为 0 ");
                }
                for (i = 1; i < RowCount; i++)
                {
                    var startIndexOfRow = GetStarIndexOfRow(i);
                    var ai0 = inverse[startIndexOfRow];
                    if (i <= RowCount - k - 1) { dat[i] = -ai0 / a00; }
                    else { dat[i] = ai0 / a00; }

                    for (j = 1; j <= i; j++)
                    {
                        inverse[GetIndex(i - 1, j - 1)] = inverse[GetIndex(i, j)] + ai0 * dat[j];
                    }
                }
                if (RowCount > 500)//并行赋值
                {
                    Parallel.For(1, RowCount, new Action<int>(delegate (int ii)
                    { 
                        inverse[GetIndex(RowCount - 1, ii - 1)] = dat[ii];
                    }));
                }
                else
                {
                    for (i = 1; i < RowCount; i++)
                    {                       
                        inverse[GetIndex(RowCount - 1, i - 1)] = dat[i];
                    }
                }

                inverse[GetIndex(RowCount, - 1)] = 1.0m / a00; //最后一个
            }

            var result = new SymmetricMatrix(Geo.Utils.DoubleUtil.GetDouble(inverse));
            return result;
        }

        /// <summary>
        /// 完全克隆
        /// </summary>
        /// <returns></returns>
        public override IMatrix Clone() { return new SymmetricMatrix(MatrixUtil.Clone(this._vector)); }


        /// <summary>
        /// 分多种情况，如果是对称阵，则计算一般就可以了，如果是对角阵，则只计算对角。
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public override IMatrix Plus(IMatrix right)
        {
            if(right is Matrix)
            {
                return Plus(((Matrix)right)._matrix);
            }
            if (right is SymmetricMatrix) { return GetPlus(right as SymmetricMatrix); }
            if (right is DiagonalMatrix) return GetPlus(right as DiagonalMatrix);

            return base.Plus(right);
        }

        /// <summary>
        /// 矩阵与矩阵的加法运算
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns>返回新的结果矩阵</returns>
        public SymmetricMatrix GetPlus(SymmetricMatrix right)
        {
            if (right == null) throw new ArgumentException("参数不能为空！");
            if (right.ColCount != this.ColCount || right.RowCount != this.RowCount) throw new DimentionException("维数相同才可以计算！");

            return new SymmetricMatrix(MatrixUtil.GetPlus(this.Vector, right.Vector));
        }

        /// <summary>
        /// 矩阵与矩阵的加法运算
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns>返回新的结果矩阵</returns>
        public SymmetricMatrix GetPlus(DiagonalMatrix right)
        {
            if (right == null) throw new ArgumentException("参数不能为空！");
            if (right.ColCount != this.ColCount || right.RowCount != this.RowCount) throw new DimentionException("维数相同才可以计算！");

            //var result = new SymmetricMatrix(new List<double>(this.vector).ToArray());
            //for (int i = 0; i < result.RowCount; i++)
            //{
            //    result[i,i] += right[i];
            //}

            var result = new SymmetricMatrix(new List<double>(this._vector).ToArray());
            
            var resultVector = result.Vector;
            for (int i = 0; i < this.RowCount; i++)
            {
                resultVector[GetDiagonalIndex(i)] += right[i];
            }
            return result;

              int count = (dimension + 1) * dimension / 2;

              double[] plus = new double[count];
              for (int i = 0; i < this.ColCount; i++)
              {
                  //
                  for (int j = 0; j <= i; j++)
                  {
                      var index = GetIndex(i, j);
                      plus[index] = this._vector[GetIndex(i, j)];
                      if (i == j) plus[GetIndex(i, j)] += right.Vector[i];
                  }
              }

              var r2 = new SymmetricMatrix(plus);

              return r2;
        }
        /// <summary>
        /// 获取下标
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static int GetIndex(int rowIndex, int colIndex)
        {
            return GetStarIndexOfRow(rowIndex) + colIndex;
        }
        /// <summary>
        /// 计算行起始编号
        /// </summary>
        /// <returns></returns>
        public static int GetStarIndexOfRow(int rowIndex)
        {
            return rowIndex * (rowIndex + 1) / 2;
        }

        /// <summary>
        /// 获取对角线对应的一维数组下标。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int GetDiagonalIndex(int i)
        {
            return i * (i + 3) / 2;
        }
        public override IMatrix Minus(IMatrix right)
        {
            if (right is Matrix)
            {
                return Minus(((Matrix)right)._matrix);
            }
            if (right is SymmetricMatrix) return GetMinus(right as SymmetricMatrix);
            if (right is DiagonalMatrix) return GetPlus(right as DiagonalMatrix);

            return base.Minus(right);
        }
        /// <summary>
        /// 矩阵与矩阵的加法运算
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns>返回新的结果矩阵</returns>
        public SymmetricMatrix GetMinus(SymmetricMatrix right)
        {
            if (right == null) throw new ArgumentException("参数不能为空！");
            if (right.ColCount != this.ColCount || right.RowCount != this.RowCount) throw new DimentionException("维数相同才可以计算！");

            return new SymmetricMatrix(MatrixUtil.GetMinus(this.Vector, right.Vector));
        }
        /// <summary>
        /// 矩阵与矩阵的加法运算
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns>返回新的结果矩阵</returns>
        public SymmetricMatrix GetMinus(DiagonalMatrix right)
        {
            if (right == null) throw new ArgumentException("参数不能为空！");
            if (right.ColCount != this.ColCount || right.RowCount != this.RowCount) throw new DimentionException("维数相同才可以计算！");


            var result = new SymmetricMatrix(new List<double>(this._vector).ToArray());

            var resultVector = result.Vector;
            for (int i = 0; i < this.RowCount; i++)
            {
                resultVector[i * (i + 3) / 2] -= right[i];
            }
            return result;            
        }
        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is SymmetricMatrix)
                return Equals(obj as SymmetricMatrix);
            return base.Equals(obj);
        }
        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SymmetricMatrix other)
        {
            return MatrixUtil.IsEqual(this._vector, other._vector, Tolerance);
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// 打印输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SymmetricMatrix " + base.ToString();
        }

        #endregion 
    
        public override IMatrix SubMatrix(int fromIndex, int count)
        {
            var m = new SymmetricMatrix(count);
            int endIndexExcluded =  fromIndex + count;
            for (int i = fromIndex, k =0; i <endIndexExcluded ; i++,k++)
            {
                for (int j = fromIndex, n = 0; j <= i; j++, n++)
                {
                    m[k, n] = this[i, j];
                }
            }

            if (this.ColNames != null && this.ColNames.Count > 0) { m.ColNames = this.ColNames.GetRange(fromIndex, count - fromIndex); }
            if (this.RowNames != null && this.RowNames.Count > 0) { m.RowNames = this.RowNames.GetRange(fromIndex, count - fromIndex); }
            return m;
        }


        public override IMatrix Pow(double power)
        {
            SymmetricMatrix m = new SymmetricMatrix(this.dimension);
            for (int i = 0; i < this._vector.Length; i++)
            {
                m._vector[i] = Math.Pow(_vector[i], power);
            }
            return m;
        }

        /// <summary>
        /// 获取对角向量
        /// </summary>
        /// <returns></returns>
        public IVector GetDiagonal()
        {
            Vector vector = new Vector(this.RowCount);
            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] = this[i, i];
            }
            return vector;
        }
         

         
    }
}
