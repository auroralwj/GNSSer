//2014.10.02, czs,  created, 对角矩阵
//2016.10.20, czs & double edit in hongqing, 改进了乘法

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geo.Common;
using Geo.Utils;
using Geo.Algorithm;

namespace Geo.Algorithm
{
    /// <summary>
    ///  对角矩阵，以一个一维数组存储对角线的矩阵。
    /// </summary>
    public class DiagonalMatrix : BaseMatrix, IEquatable<DiagonalMatrix>
    {
        #region 工厂类
        public class DiagonalMatrixFactory : IMatrixFactory
        {
            public IMatrix Create(int rowCount, int colCount)
            {
                return new DiagonalMatrix(rowCount, colCount);
            }

            public IMatrix Create(double[][] array)
            {
                return new DiagonalMatrix(array);
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 对角矩阵 构造函数
        /// </summary>
        /// <param name="dimension">维数</param>
        /// <param name="initVal">初始值</param>
        public DiagonalMatrix(int dimension, double initVal = 0)
            : base(MatrixType.Diagonal)
        {
            this._vector = new double[dimension]; 

            if (initVal != 0)
                for (int i = 0; i < dimension; i++)
                {
                    _vector[i] = initVal;
                }
        }
        /// <summary>
        /// 对角矩阵 构造函数
        /// </summary>
        /// <param name="diagonalVector">对角一维数组</param>
        public DiagonalMatrix(double[] diagonalVector, double Tolerance = 1e-13)
            : base(MatrixType.Diagonal)
        {
            this.Tolerance = Tolerance;
            this._vector = diagonalVector; 
        }
        /// <summary>
        /// 对角矩阵 构造函数
        /// </summary>
        /// <param name="diagonalVector">对角一维数组</param>
        public DiagonalMatrix(IVector diagonalVector, double Tolerance = 1e-13)
            : base(MatrixType.Diagonal)
        {
            this.Tolerance = Tolerance;
            this._vector = diagonalVector.OneDimArray;
            this.ColNames = diagonalVector.ParamNames;
            this.RowNames = diagonalVector.ParamNames;
        }
        /// <summary>
        /// 对角矩阵 构造函数
        /// </summary>
        /// <param name="matrix">二维数组</param>
        public DiagonalMatrix(double[][] matrix)
            : base(MatrixType.Array)
        {
            if (matrix == null) throw new ArgumentException("参数不能为空！");
            if (matrix.Length != matrix[0].Length)
                throw new DimentionException("必须是方阵！");

            this._vector = MatrixUtil.GetDiagonal(matrix); 
        }
        /// <summary>
        /// 对角矩阵
        /// </summary>
        /// <param name="rows"></param>
        public DiagonalMatrix(IVector[] rows)
            : base(MatrixType.Diagonal)
        {
            if (rows == null) throw new ArgumentException("参数不能为空！");
            if (rows.Length != rows[0].Count)
                throw new DimentionException("必须是方阵！");

            int count = rows.Length;
            this._vector = new double[count];
            for (int i = 0; i < count; i++)
            {
                _vector[i] = rows[i][i];
            } 
        }
        #endregion

        #region 核心存储
        double[] _vector;
        /// <summary>
        /// 对角向量，本对象特有属性。
        /// </summary>
        public double[] Vector { get { return this._vector; } }

        #endregion

        #region 操作数
        #region  与自身的操作数
        public static DiagonalMatrix operator +(DiagonalMatrix left, DiagonalMatrix right)
        {
            return new DiagonalMatrix(MatrixUtil.GetPlus(left.Vector, right.Vector));
        }
        public static DiagonalMatrix operator -(DiagonalMatrix left, DiagonalMatrix right)
        {
            return new DiagonalMatrix(MatrixUtil.GetMinus(left.Vector, right.Vector));
        }
        public static DiagonalMatrix operator *(DiagonalMatrix left, DiagonalMatrix right)
        {
            return new DiagonalMatrix(MatrixUtil.GetMultiply(left.Vector, right.Vector));
        }
        public static DiagonalMatrix operator *(DiagonalMatrix left, double right)
        {
            return new DiagonalMatrix(MatrixUtil.GetMultiply(left.Vector, right));
        }
        public static DiagonalMatrix operator -(DiagonalMatrix left)
        {
            return new DiagonalMatrix(MatrixUtil.GetOpposite(left.Vector));
        }
        #endregion
        #region  与其他的操作数，如对称矩阵，则返回一个对称矩阵
        public static SymmetricMatrix operator +(DiagonalMatrix left, SymmetricMatrix right)
        {
            return (SymmetricMatrix)right.GetPlus(left);
        }


        #endregion
        #endregion

        #region 通用矩阵接口的实现

        /// <summary>
        /// 行数
        /// </summary>
        public override int RowCount { get { return _vector.Length; } }
        /// <summary>
        /// 列数量
        /// </summary>
        public override int ColCount { get { return RowCount; } }
        /// <summary>
        /// 返回二维数组
        /// </summary>
        public override double[][] Array { get { return MatrixUtil.CreateWithDiagonal(_vector); } }
        /// <summary>
        /// 是否对称
        /// </summary>
        public override bool IsSymmetric { get { return true; } }
        /// <summary>
        /// 检索器
        /// </summary>
        /// <param name="i">行号</param>
        /// <param name="j">列号</param>
        /// <returns></returns>
        public override double this[int i, int j]
        {
            get { if (i == j)  return _vector[i]; return 0; }
            set { if (i == j)  _vector[i] = value; }
        }
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="rowOrColIndex"></param>
        /// <returns></returns>
        public double this[int rowOrColIndex]
        {
            get { return _vector[rowOrColIndex]; }
            set { _vector[rowOrColIndex] = value; }
        }
        /// <summary>
        /// 直接设置
        /// </summary>
        /// <param name="rowOrColIndex"></param>
        /// <param name="val"></param>
        public void SetValue(int rowOrColIndex, double val) { _vector[rowOrColIndex] = val; }
        /// <summary>
        /// 转置
        /// </summary>
        public override IMatrix Transposition { get { return Clone(); } }
        /// <summary>
        /// 矩阵求逆。
        /// </summary>
        /// <returns></returns>
        public override IMatrix GetInverse()
        {
            return Inverse();
        }

        public DiagonalMatrix Inverse()
        {
            int length = _vector.Length;
            double[] inverse = new double[length];
            for (int i = 0; i < length; i++)
            {
                inverse[i] = 1.0 / this._vector[i];
            }
            return new DiagonalMatrix(inverse);
        }

        /// <summary>
        /// 完全克隆
        /// </summary>
        /// <returns></returns>
        public override IMatrix Clone() { return new DiagonalMatrix(MatrixUtil.Clone(this._vector)); }


        public override IMatrix Plus(IMatrix right)
        {
            if (right is DiagonalMatrix) return GetPlus(right as DiagonalMatrix);
            return base.Plus(right);
        }
        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public override IMatrix Multiply(double right)
        { 
            DiagonalMatrix result = new DiagonalMatrix(this.ColCount);
            for (int i = 0; i < this.ColCount; i++)
            {
                result.SetValue(i, this[i] * right);
            }

            return result;
        }

        public override IMatrix Multiply(IMatrix right)
        {
            if (this.ColCount != right.RowCount)
            {
                throw new ArgumentException("右边的行不等于左边的列数量。");
            }
            if (right is IVectorMatrix)
            {
                var right1 = right as IVectorMatrix;
                return Multiply(right1);
            }

            if (right is DiagonalMatrix)
            {
                return Multiply(right as DiagonalMatrix);
            }             

            return base.Multiply(right);
        }

        public IMatrix Multiply(IVectorMatrix right1)
        {
            var result = new VectorMatrix(right1.Count);

            for (int i = 0; i < result.Count; i++)
            {
                result[i] = right1[i] * this[i];
            }
            return result;
        }

        public IMatrix Multiply(DiagonalMatrix right)
        {
            DiagonalMatrix result = new DiagonalMatrix(this.ColCount);
            for (int i = 0; i < this.ColCount; i++)
            {
                result.SetValue(i, this[i] * right[i, i]);
            }
            return result;
        }

        /// <summary>
        /// 矩阵与矩阵的加法运算
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns>返回新的结果矩阵</returns>
        public DiagonalMatrix GetPlus(DiagonalMatrix right)
        {
            if (right == null) throw new ArgumentException("参数不能为空！");
            if (right.ColCount != this.ColCount) throw new DimentionException("维数相同才可以计算！");

            return new DiagonalMatrix(MatrixUtil.GetPlus(this.Vector, right.Vector));
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

            int count = (right.RowCount + 1) * right.RowCount / 2;

            double[] plus = new double[count];
            for (int i = 0; i < right.RowCount; i++)
            {
                //
                for (int j = 0; j <= i; j++)
                {
                    plus[i * (i + 1) / 2 + j] = right.Vector[i * (i + 1) / 2 + j];
                    if (i == j) plus[i * (i + 1) / 2 + j] += this.Vector[i];
                }
            }

            return new SymmetricMatrix(plus);
        }


        #endregion
        /// <summary>
        /// 返回新的乘法结果。
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public IMatrix Multiply(VectorMatrix right)
        {
            int row = this.RowCount;
            int col = this.ColCount;
            IMatrix matrix = new ArrayMatrix(row, 1);

            for (int i = 0; i < row; i++)
            {
                matrix[i, 0] = this[i, i] * right[i,0];
            }
            return matrix;
        }

        public override IMatrix Pow(double power)
        {
            DiagonalMatrix m = new DiagonalMatrix(this.ItemCount);
            for (int i = 0; i < this._vector.Length; i++)
            {
                m._vector[i] = Math.Pow(_vector[i], power);
            }
            return m;
        }
        public override bool Equals(object obj)
        {
            if (obj is DiagonalMatrix)
                return Equals(obj as DiagonalMatrix);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(DiagonalMatrix other)
        {
            return MatrixUtil.IsEqual(this._vector, other._vector, Tolerance);
        }
        public override string ToString()
        {
            return "DiagonalMatrix " +　 MatrixUtil.GetFormatedText(this.Vector);//base.ToString();//
        }

        public override int ItemCount
        {
            get { return Vector.Length; }
        }

        public static DiagonalMatrix GetIdentity(int rowColCount)
        {
            return new DiagonalMatrix(rowColCount, 1);
        }

        public override IMatrix SubMatrix(int fromIndex, int count)
        {
            var matrix = new DiagonalMatrix(count, count);
            for (int i = fromIndex, j = 0; i < count + fromIndex; i++, j++)
            {
                matrix._vector[j] = this.Vector[i];
            }
            return matrix;
        }



        /// <summary>
        /// 获取对角向量
        /// </summary>
        /// <returns></returns>
        public IVector GetDiagonal()
        {
            Vector vector = new Vector(Vector);
            return vector;
        }
    }
}
