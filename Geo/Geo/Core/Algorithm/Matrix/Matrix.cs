//2017.07.18, czs, create in hongqing, 创建一个通用、方便的矩阵类。
//2017.07.19, czs, edit in hongqing, 合并了 Orbit matrix 部分函数

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 一个通用、方便的矩阵类。
    /// 适配器模式。
    /// 封装一个矩阵对象，提供方便的操作符计算功能。
    /// </summary>
    public class Matrix : Geo.Algorithm.AbstractMatrix, IDisposable, IReadable
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="convertToSymmetric"></param>
        public Matrix(IMatrix matrix, bool convertToSymmetric = false)
            : base(matrix.MatrixType)
        {
            if (convertToSymmetric)
            {
                if (!matrix.IsSquare) { throw new Exception("指定为对称阵，但是为非对称阵。"); }

                this._matrix = SymmetricMatrix.Parse(matrix);
            }
            else
            {
                this._matrix = matrix;
            }
            if (matrix.RowNames!=null && matrix.RowNames.Count != 0)
            {
                this.RowNames = matrix.RowNames;
            }
            if (matrix.ColNames != null && matrix.ColNames.Count != 0)
            {
                this.ColNames = matrix.ColNames;
            }
            this.Name = matrix.Name;
            this.Tolerance = matrix.Tolerance;
        } 

        /// <summary>
        /// 构造函数，构建一个方阵。默认初始为二维数组矩阵。
        /// </summary>
        /// <param name="rowColCount"></param> 
        public Matrix(int rowColCount)
            : this(new Geo.Algorithm.ArrayMatrix(rowColCount, rowColCount))
        {
        }
        /// <summary>
        /// 构造函数。默认初始为二维数组矩阵。
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        public Matrix(int rowCount, int colCount)
            : this(new Geo.Algorithm.ArrayMatrix(rowCount, colCount))
        {
        }

        public Matrix(double[][] p, int RowCount, int ColCount)
            : this(new Geo.Algorithm.ArrayMatrix(p)) // Array copy
        {
        }
        /// <summary>
        /// 一维数组初始化非对角矩阵，将顺序初始化为二维矩阵
        /// </summary>
        /// <param name="array"></param>
        /// <param name="RowCount"></param>
        /// <param name="ColCount"></param>
        public Matrix(double[] array, int RowCount, int ColCount)
            : this(new Geo.Algorithm.ArrayMatrix(RowCount, ColCount, array)) // Array copy
        {
        }
        /// <summary>
        /// 一维数组初始化N x 1矩阵
        /// </summary>
        /// <param name="vector"></param> 
        public Matrix(IVector vector)
            : this(new ArrayMatrix(vector,true)) // Array copy
        {
            this.RowNames = vector.ParamNames;
            this.ColNames = new List<string>() { "Value" };
        }
        /// <summary>
        /// 向量转换矩阵，一行或一列。
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="isColOrRowVector">一列或一行</param>
        public Matrix(IVector vector, bool isColOrRowVector)
            : this(new ArrayMatrix(vector, isColOrRowVector)) // Array copy
        {
        }
        public Matrix(double[][] p)
            : this(new Geo.Algorithm.ArrayMatrix(p)) // Array copy
        {
        }
        #region core data structures
        public IMatrix _matrix { get; set; }

        public double[][] Data { get { return this.Array; } }
        #endregion
        /// <summary>
        /// 转换为对称阵返回
        /// </summary>
        /// <returns></returns>
        public Matrix GetSymmetric()
        {
            if (this.IsSymmetric)
            {
                return this;
            }
            return new Matrix(this, true);
        }

        #region 简化表示
        /// <summary>
        /// 转置
        /// </summary>
        public virtual Matrix Trans { get { return new Matrix(Transposition); } }
        /// <summary>
        /// 逆矩阵
        /// </summary>
        public virtual Matrix Inversion { get { return new Matrix(GetInverse()); } }
        #endregion

        #region utils
        /// <summary>
        /// 矩阵左上角第一个数值。
        /// </summary>
        public double FirstValue { get { return this[0, 0]; } }
        #endregion

        #region 实现 AbstractMatrix
        /// <summary>
        /// 返回二维数组
        /// </summary>
        public override double[][] Array { get { return _matrix.Array; } }

        /// <summary>
        /// 返回一个按照新名称扩展或缩小的矩阵，无则补0.
        /// </summary>
        /// <param name="newRowParams"></param>
        /// <param name="newColParams"></param>
        /// <returns></returns>
        public Matrix Expand(List<string> newRowParams, List<string> newColParams)
        {
            int rowCount = this.RowCount;
            int colCount = this.ColCount;
            int newRowCount = newRowParams.Count;
            int newColCount = newColParams.Count;

            Matrix matrix = new Matrix(newRowCount, newColCount);
            matrix.ColNames = newColParams;
            matrix.RowNames = this.RowNames;
            int i = 0;
            foreach (var rowName in newRowParams)
            {
                var iOld = this.GetColIndex(rowName);
                if (iOld != -1)
                {
                    continue;
                }
                int j = 0;
                foreach (var colName in newColParams)
                {
                    var jOld = this.GetColIndex(colName);
                    if (jOld != -1)
                    {
                        matrix[i, j] = this[iOld, jOld];
                    }
                    j++;
                }
                i++;
            }
            return matrix;
        }
        /// <summary>
        /// 返回一个按照新名称扩展或缩小的列的矩阵
        /// </summary>
        /// <param name="newColParams"></param>
        /// <returns></returns>
        public Matrix ExpandCols(List<string> newColParams)
        {
            int rowCount = this.RowCount;
            int colCount = this.ColCount;
            int newColCount = newColParams.Count;

            Matrix matrix = new Matrix(rowCount, newColCount);
            matrix.ColNames = newColParams;
            matrix.RowNames = this.RowNames;
            for (int i = 0; i < rowCount; i++)
            {
                int j = 0;
                foreach (var item in newColParams)
                {
                    var jOld = this.GetColIndex(item);
                    if(jOld != -1)
                    {
                        matrix[i, j] = this[i, jOld];
                    }
                    j++;
                }
            }
            return matrix;
          }

        /// <summary>
        /// 有效元素总和，二维数组为 n*m ，对角线为 n。
        /// </summary>
        public override int ItemCount { get { return _matrix.ItemCount; } }
        /// <summary>
        /// 获取子矩阵
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override IMatrix SubMatrix(int fromIndex, int count) { return _matrix.SubMatrix(fromIndex, count); }
        /// <summary>
        /// 可阅读的字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return "Matrix: " + _matrix.ToString(); }
        #endregion

        #region 操作符重载
        /// <summary>
        /// +
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix left, IMatrix right) {
            if (left == null) return new Matrix(right);
            if (right == null) return left;
            return new Matrix(left.Plus(right)); }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix left, IMatrix right) {
            if (left == null) return - new Matrix(right);
            if (right == null) return left;

            return new Matrix(left.Minus(right));
        }
        /// <summary>
        /// *
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix left, IMatrix right) {

            if (left == null || right == null) return null;
            return new Matrix(left.Multiply(right));
        }
        /// <summary>
        /// *
        /// </summary>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix left, double right) { return new Matrix(left.Multiply(right)); }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix val) {  return new Matrix(val.Opposite()); }

        /// <summary>
        ///  Scalar multiplication and division of a vector
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix operator *(double left, Matrix right) { return right * left; }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix operator /(Matrix left, double right) { return new Matrix(left.Divide(right)); }

        /// <summary>
        /// Matrix addition and subtraction
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix left, Matrix right) {

            if (left == null) return (right);
            if (right == null) return left;
            return new Matrix(left.Plus(right));
        }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix left, Matrix right) {

            if (left == null) return -(right);
            if (right == null) return left;
            return new Matrix(left.Minus(right));
        }

        /// <summary>
        /// Matrix product
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix left, Matrix right) {
            if (left == null || right == null) return null;
            return new Matrix(left.Multiply(right));
        }

        /// <summary>
        /// 矩阵与列向量相乘。 n x m 阶矩阵，等于 m x 1 的矩阵，即 m 维的列向量。
        /// Vector/matrix product
        /// </summary>
        /// <param name="Mat"></param>
        /// <param name="Vec"></param>
        /// <returns></returns>
        public static Vector operator *(Matrix Mat, Vector Vec)
        {
            if (Mat.ColCount != Vec.Dimension)
            {
                throw new ArgumentException("维数不符合要求！");
            };
            Vector Aux = new Vector(Mat.RowCount);
            double Sum;
            for (int i = 0; i < Mat.RowCount; i++)
            {
                Sum = 0.0;
                for (int j = 0; j < Mat.ColCount; j++)
                    Sum += Mat[i, j] * Vec[j];
                Aux[i] = Sum;
            }
            return Aux;
        }
        /// <summary>
        /// 横向量与矩阵相乘， 1 x n 的横向量与 n x m 的列向量相乘，结果为 1 x m 的横向量。
        /// </summary>
        /// <param name="Vec"></param>
        /// <param name="Mat"></param>
        /// <returns></returns>
        public static Vector operator *(Vector Vec, Matrix Mat)
        {
            if (Mat.RowCount != Vec.Dimension) { throw new ArgumentException("维数不符合要求！"); }

            Vector Aux = new Vector(Mat.ColCount);
            double Sum;
            for (int j = 0; j < Mat.ColCount; j++)
            {
                Sum = 0.0;
                for (int i = 0; i < Mat.RowCount; i++)
                    Sum += Vec[i] * Mat[i, j];
                Aux[j] = Sum;
            }
            return Aux;
        }

        #endregion

        #region override
        /// <summary>
        /// 行名称
        /// </summary>
        public override List<string> RowNames { get { return _matrix.RowNames; } set { if(value==null)return; if (_matrix.RowCount != value.Count){ log.Error("名称维数不等！" + _matrix.RowCount +"!=" +  value.Count); }_matrix.RowNames = value; } }

        /// <summary>
        /// 列名称
        /// </summary>
        public override List<string> ColNames { get { return _matrix.ColNames; } set { if (value == null) return; if (_matrix.ColCount != value.Count) { log.Error("名称维数不等！" + _matrix.ColCount + "!=" + value.Count); } _matrix.ColNames = value; } }


        /// <summary>
        /// 名称
        /// </summary>
        public override string Name { get { return _matrix.Name; } set { _matrix.Name = value; } }

        /// <summary>
        /// 转置
        /// </summary>
        public override IMatrix Transposition { get { return _matrix.Transposition; } }
        /// <summary>
        /// 求逆
        /// </summary>
        /// <returns></returns>
        public override IMatrix GetInverse() { return _matrix.GetInverse(); }
        /// <summary>
        /// 列数
        /// </summary>
        public override int ColCount { get { return _matrix.ColCount; } }
        /// <summary>
        /// 行数
        /// </summary>
        public override int RowCount { get { return _matrix.RowCount; } }
        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public override double this[int i, int j]
        {
            get { return _matrix[i, j]; }
            set { _matrix[i, j] = value; }
        }
        /// <summary>
        /// 是否对称
        /// </summary>
        public override bool IsSymmetric { get { return _matrix.IsSymmetric; } }
        /// <summary>
        /// 是否为对角阵
        /// </summary>
        public override bool IsDiagonal { get { return _matrix.IsDiagonal; } }
 
        /// <summary>
        /// 获取指定的元素。
        /// </summary>
        /// <param name="rowName">行名称</param>
        /// <param name="colName">列名称</param>
        /// <returns></returns>
        public override double this[string rowName, string colName]
        {
            set { _matrix[rowName, colName] = value; }
            get { return _matrix[rowName, colName]; }
        }
        /// <summary>返回一个完全复制品.</summary>
        public override IMatrix Clone() { return _matrix.Clone(); }
        /// <summary>
        /// 相加
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public override IMatrix Plus(IMatrix right) { return _matrix.Plus(right); }
        /// <summary>
        /// 相减
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public override IMatrix Minus(IMatrix right) { return _matrix.Minus(right); }
        /// <summary>
        /// 求反
        /// </summary>
        /// <returns></returns>
        public override IMatrix Opposite() { return _matrix.Opposite(); }
        /// <summary>
        /// 乘以
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public override IMatrix Multiply(IMatrix right) { return _matrix.Multiply(right); }
        /// <summary>
        /// 乘以
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public override IMatrix Multiply(double right) { return _matrix.Multiply(right); }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) { return _matrix.Equals(obj); }
        /// <summary>
        /// 标识
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() { return _matrix.GetHashCode(); }
        #endregion
        /// <summary>
        /// 检查或初始化名称，确保每行列有名称对应。
        /// </summary>
        public void CheckOrInitRowColNames()
        {
            int row = this.RowCount;
            int col = this.ColCount;
            if (this.RowNames == null || this.RowNames.Count != this.RowCount)
            {
                List<string> rowNames = this.RowNames;
                if (rowNames == null) { rowNames = new List<string>(); }

                for (int i = rowNames.Count; i < row; i++)
                {
                    rowNames.Add(i + "");
                }
                this.RowNames = rowNames;
            }
            if (this.ColNames == null || this.ColNames.Count != this.ColCount)
            {
                List<string> colNames = this.ColNames;
                if (colNames == null) { colNames = new List<string>(); } 

                for (int i = colNames.Count; i < col; i++)
                {
                    colNames.Add(i + "");
                }
                this.ColNames = colNames;
            }
        }
        #region form orbit

        /// <summary>
        /// 创建一个对角阵。 Diagonal matrix。
        /// </summary>
        /// <param name="Vec"></param>
        /// <returns></returns>
        static public Matrix CreateDiagonal(Vector Vec) { return new Matrix(new DiagonalMatrix(Vec.OneDimArray)); }

        /// <summary>
        /// 创建一个单位阵。 
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        static public Matrix CreateIdentity(int dimension) { return new Matrix(DiagonalMatrix.GetIdentity(dimension)); }
        /// <summary>
        /// 返回对角线
        /// </summary>
        /// <returns></returns>
        public Vector GetDiagonal()
        {
            if (RowCount != ColCount)
            {
                throw new ArgumentException("维数不符合要求！");
            };
            Vector Vec = new Vector(RowCount);
            for (int i = 0; i < RowCount; i++) Vec[i] = this[i, i];
            return Vec;
        }

        // Elementary rotations
        /// <summary>
        /// 绕X轴旋转
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        static public Matrix RotateX3D(double Angle)
        {
            double C = Math.Cos(Angle);
            double S = Math.Sin(Angle);
            Matrix U = new Matrix(3, 3);
            U[0, 0] = 1.0; U[0, 1] = 0.0; U[0, 2] = 0.0;
            U[1, 0] = 0.0; U[1, 1] = +C; U[1, 2] = +S;
            U[2, 0] = 0.0; U[2, 1] = -S; U[2, 2] = +C;
            return U;
        }
        /// <summary>
        /// 绕Y轴旋转
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        static public Matrix RotateY3D(double Angle)
        {
            double C = Math.Cos(Angle);
            double S = Math.Sin(Angle);
            Matrix U = new Matrix(3, 3);
            U[0, 0] = +C; U[0, 1] = 0.0; U[0, 2] = -S;
            U[1, 0] = 0.0; U[1, 1] = 1.0; U[1, 2] = 0.0;
            U[2, 0] = +S; U[2, 1] = 0.0; U[2, 2] = +C;
            return U;
        }
        /// <summary>
        /// 绕 Z 轴旋转
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        static public Matrix RotateZ3D(double Angle)
        {
            double C = Math.Cos(Angle);
            double S = Math.Sin(Angle);
            Matrix U = new Matrix(3, 3);
            U[0, 0] = +C; U[0, 1] = +S; U[0, 2] = 0.0;
            U[1, 0] = -S; U[1, 1] = +C; U[1, 2] = 0.0;
            U[2, 0] = 0.0; U[2, 1] = 0.0; U[2, 2] = 1.0;
            return U;
        }

        /// <summary>
        /// Transposition
        /// </summary>
        /// <returns></returns> 
        public Matrix Transpose() { return MatrixUtilNew.Transpose(this); }

        /// <summary>
        /// 求逆
        /// </summary>
        /// <param name="Mat"></param>
        /// <returns></returns>
        public Matrix Inverses(Matrix Mat)
        {
            int n = Mat.RowCount;

            Matrix LU = new Matrix(n, n), Inverse = new Matrix(n, n);
            Vector b = new Vector(n), Indx = new Vector(n);

            if (Mat.ColCount != Mat.RowCount) { throw new ArgumentException("维数不符合要求！"); }

            // LU decomposition 
            LU = Mat;
            MatrixUtilNew.LU_Decomp(LU, Indx);

            // Solve Ax=b for  unit vectors b_1..b_n

            for (int j = 0; j < n; j++)
            {
                b[j] = 1.0;                     // Set b to j-th unit vector 
                MatrixUtilNew.LU_BackSub(LU, Indx, b);           // Solve Ax=b 
                Inverse.SetCol(j, b);                  // Copy result
            };

            return Inverse;

        }
        #endregion

        /// <summary>
        /// 求幂
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public override IMatrix Pow(double power)
        {
            return _matrix.Pow(power);
        }
        /// <summary>
        /// 逆序,必须是方阵,注意不是Inverse！！！！
        /// </summary>
        /// <returns></returns>
        public Matrix GetReverse()
        {
            if (!this.IsSquare) { throw new Exception("必须是方阵"); }
            var result = new Matrix(this.RowCount, this.RowCount);
            int count = this.RowCount;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    result[count-1-i, count-1-j] = this[j, i];
                }
            }
            return result;
        }

        /// <summary>
        /// 获取子矩阵
        /// </summary>
        /// <param name="startRowIndex">从0开始</param>
        /// <param name="startColIndex">从0开始</param>
        /// <param name="lenOfRow">行长度</param>
        /// <param name="lenOfCol">列长度</param>
        /// <returns></returns>
        public Matrix GetSub(int startRowIndex = 0, int startColIndex = 0, int lenOfRow = int.MaxValue, int lenOfCol = int.MaxValue)
        {
            if (lenOfRow > this.RowCount - startRowIndex) { lenOfRow = this.RowCount - startRowIndex; }
            if (lenOfCol > this.ColCount - startColIndex) { lenOfCol = this.ColCount - startColIndex; }

            //不可小于0
            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;
            startColIndex = startColIndex < 0 ? 0 : startColIndex; 


            Matrix Aux = new Matrix(lenOfRow, lenOfCol);
            for (int i = 0; i < lenOfRow; i++)
                for (int j = 0; j < lenOfCol; j++)
                    Aux[i, j] = this[i + startRowIndex, j + startColIndex];
            return Aux;
        }

        /// <summary>
        /// 设置矩阵内容
        /// </summary>
        /// <param name="subMatrix"></param>
        /// <param name="startMainRowIndex"></param>
        /// <param name="startMainColIndex"></param>
        /// <param name="startSubRowIndex"></param>
        /// <param name="startSubColIndex"></param>
        /// <param name="maxSubRowLen"></param>
        /// <param name="maxSubColLen"></param>
        public void SetSub(
            IMatrix subMatrix,
            int startMainRowIndex = 0,
            int startMainColIndex = 0,
            int startSubRowIndex = 0,
            int startSubColIndex = 0,
            int maxSubRowLen = int.MaxValue,
            int maxSubColLen = int.MaxValue
            )
        {
            for (int i = 0;
                   i + startMainRowIndex < this.RowCount //主矩阵行编号不可大于其长度
                && i + startSubRowIndex < subMatrix.RowCount //
                && i < maxSubRowLen; i++)
            {
                for (int j = 0;
                    startMainColIndex + j < this.ColCount
                    && j + startSubColIndex < subMatrix.ColCount
                    && j < maxSubColLen; j++)
                {
                    this[startMainRowIndex + i, startMainColIndex + j] = subMatrix[i + startSubRowIndex, j + startSubColIndex];
                }
            }
        }

        public void Dispose()
        {
            this._matrix = null;
        }
        /// <summary>
        /// 是否为空。
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsEmpty(IMatrix matrix)
        {
            return matrix == null || matrix.RowCount == 0 || matrix.ColCount == 0;
        }
        /// <summary>
        /// 根据矩阵名称进行同步。将按照新的进行同步。删除没有的，增加新增的。使得二者可以相加。
        /// </summary>
        /// <param name="tobeChange"></param>
        /// <param name="reference"></param>
        /// <param name="defaultValue"></param>
        /// <param name=""></param>
        public static Matrix Synchronize(Matrix tobeChange, Matrix reference, double defaultValue = 0)
        {
            var oldRowNames = tobeChange.RowNames;
            var oldColNames = tobeChange.ColNames;
            var newRowNames = reference.RowNames;
            var newColNames = reference.ColNames;

            if (tobeChange.RowCount == reference.RowCount && tobeChange.ColCount == reference.ColCount)
            {
                if (Geo.Utils.ListUtil.IsEqual(oldRowNames, newRowNames) 
                    && Geo.Utils.ListUtil.IsEqual(oldColNames, newColNames))
                {
                    return tobeChange;
                }
            }
            ChangeableMatrix changeable = new ChangeableMatrix(tobeChange);
            changeable.ArrangeRowCols(newRowNames, newColNames, defaultValue);
            return new Matrix(changeable.ToArrayMatrix()); 
        }



        #region  IO
        /// <summary>
        /// 对象表格返回
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetObectTable()
        {
            int row = this.RowCount;
            int col = this.ColCount;
            var result = new ObjectTableStorage(this.Name);

            CheckOrInitRowColNames();

            for (int i = 0; i < row; i++)
            {
                result.NewRow();
                var rowName = this.RowNames[i];
               result.AddItem("Name", rowName);
                for (int j = 0; j < col; j++)
                {
                    var colName = this.ColNames[j];
                    var val = this[i, j];
                    result.AddItem(colName, val);
                }
            }

            return result;
        }

        /// <summary>
        /// 解析表格
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static Matrix Parse(ObjectTableStorage table)
        {
            var row = table.RowCount;
            var col = table.ColCount - 1;//行名称占据一行

            Matrix result = new Matrix(row, col);
            result.ColNames = table.ParamNames.GetRange(1, col);
            result.RowNames = table.GetColStrings("Name");
            for (int i = 0; i < row; i++)
            {
                int j = 0;
                foreach (var item in result.ColNames)
                { 
                    result[i, j] = (double) table.BufferedValues[i][item];
                    j++;
                }
            }

            return result;
        }

        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <returns></returns>
        public virtual string ToReadableText(string separator = ",")
        {
            StringBuilder sb = new StringBuilder();
            int rowCount = this.RowCount;
            int colCount = this.ColCount;// Data.Length, ColCount = Data[0].Length;
            sb.AppendLine(rowCount + "×" + colCount);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    if (j != 0) { sb.Append(separator); }
                    var val = Data[i][j];

                    sb.Append(Geo.Utils.DoubleUtil.ToReadableText(val));

                    // sb.Append(String.Format("{0, 12:F5}  ", Data[i][j]));
                    // Geo.Utils.StringUtil.FillSpaceRight(item, 9);// String.Format("{0, G}  ", ]));
                }
                sb.AppendLine();
            }
            Geo.Utils.StringUtil.AppendListLine(sb, RowNames, RowNamesMarker);
            Geo.Utils.StringUtil.AppendListLine(sb, ColNames, ColNamesMarker); 

            return sb.ToString();
        }


        const string ColNamesMarker = "ColNames:";
        const string RowNamesMarker = "RowNames:";

        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToReadableText(IMatrix mat, string separator = ",")
        {
            return new Matrix(mat).ToReadableText(separator);
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public static Matrix Parse(string text, string[] splitter = null)
        {
            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return Parse(lines, splitter);
        }
        /// <summary>
        ///解析数组 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public static Matrix Parse(string[] lines, string[] splitter = null)
        {
            if (splitter == null)
            {
                splitter = new string[] { ",", ";", "\t", " " };
            }
            int currentIndex = 0;
            string line0 = lines[currentIndex];

            int rowCount = lines.Length;
            int colCount = line0.Split(splitter, StringSplitOptions.RemoveEmptyEntries).Length;
            if (line0.Contains("×"))
            {
                var items = line0.Split(new string[] { "×" }, StringSplitOptions.RemoveEmptyEntries);
                rowCount = int.Parse(items[0]);
                if (items.Length >= 2)
                {
                    colCount = int.Parse(items[1]);
                }
                currentIndex = 1;
            }


            Matrix matrix = new Matrix(rowCount, colCount);

            string rowLine = null;
            for (int lineIndex = currentIndex, rowIndex = 0; lineIndex < rowCount + currentIndex; lineIndex++, rowIndex++)
            {
                rowLine = lines[lineIndex];
                string[] vals = rowLine.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < vals.Length; j++)
                {
                    matrix[rowIndex, j] = Geo.Utils.DoubleUtil.TryParse(vals[j], 0);
                }
            }

            matrix.RowNames = Geo.Utils.StringUtil.ExtractNames(lines, RowNamesMarker, splitter);
            matrix.ColNames = Geo.Utils.StringUtil.ExtractNames(lines, ColNamesMarker, splitter);


            return matrix;
        }



        #endregion
    }
}