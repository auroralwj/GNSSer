//2014.11.18， czs, create in namu,  向量矩阵。只有一列的矩阵。
//2014.11.19, czs, edit in namu, 提取 VectorMatrix， IWeightedVector
//2017.08.31, czs, edit in hongqing, 实现元素求幂接口

using System;
using System.Text;
using System.Collections.Generic;
using Geo.Common;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Algorithm;

namespace Geo.Algorithm
{
    /// <summary>
    ///  向量矩阵。只有一列的矩阵。
    /// </summary>
    public interface IVectorMatrix : IVector, IMatrix
    {

    }

    /// <summary>
    /// 向量矩阵。只有一列的矩阵。
    /// </summary>
    public class VectorMatrix : Vector, IVectorMatrix
    {
        #region 构造函数
        public VectorMatrix()
        {

        }

        /// <summary>
        /// 构建一个二维向量
        /// </summary>
        /// <param name="prevObj">第一元素</param>
        /// <param name="second">第二元素</param>
        public VectorMatrix(double first, double second) : this(new double[] { first, second }) { }
        /// <summary>
        /// 构建一个三维向量
        /// </summary>
        /// <param name="prevObj">第一元素</param>
        /// <param name="second">第二元素</param>
        /// <param name="third">第3元素</param>
        public VectorMatrix(double first, double second, double third) : this(new double[] { first, second, third }) { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dimension">维数</param> 
        public VectorMatrix(int dimension)
            : this(new Double[dimension])
        {  
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vector">一维数组</param>
        public VectorMatrix(double[] vector)
            : base(vector)
        {
        }
        public VectorMatrix(IVector vector)
            : base(vector)
        {
        }
        #endregion

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取行编号
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <returns></returns>
        public int GetRowIndex(String paramName)
        {
            return this.RowNames.IndexOf(paramName);
        }
        /// <summary>
        /// 获取列编号
        /// </summary>
        /// <param name="paramName">参数名称</param>
        public int GetColIndex(String paramName)
        {
            return this.ColNames.IndexOf(paramName);
        }
        /// <summary>
        /// 设置某行的值为统一的数值
        /// </summary>
        /// <param name="rowIndex">行编号</param>
        /// <param name="val">数值</param>
        public void SetRowValue(int rowIndex, double val)
        {
            for (int i = 0; i < this.ColCount; i++)
            {
                this[rowIndex, i] = val;
            }
        }

        /// <summary>
        /// 设置某列的值为统一的数值
        /// </summary>
        /// <param name="colIndex">列编号</param>
        /// <param name="val">数值</param>
        public void SetColValue(int colIndex, double val)
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                this[i, colIndex] = val;
            }
        }
        /// <summary>
        /// 获取指定的元素。
        /// </summary>
        /// <param name="rowName">行名称</param>
        /// <param name="colName">列名称</param>
        /// <returns></returns>
        public virtual double this[string rowName, string colName]
        {
            set { this[GetRowIndex(rowName), GetRowIndex(colName)] = value; }
            get { return this.Array[GetRowIndex(rowName)][GetRowIndex(colName)]; }
        }
        /// <summary>
        /// 获取指定参数的矩阵
        /// </summary>
        /// <param name="paramNames">参数列表</param>
        /// <returns></returns>
        public IMatrix GetMatrix(List<String> paramNames)
        {
            return GetMatrix(paramNames, paramNames);
        }

        /// <summary>
        /// 获取指定参数的矩阵
        /// </summary>
        /// <param name="rowNames">行参数列表</param>
        /// <param name="colNames">列参数列表</param>
        /// <returns></returns>
        public IMatrix GetMatrix(List<String> rowNames, List<String> colNames)
        {
            IMatrix newVector = new ArrayMatrix(rowNames.Count, rowNames.Count);
            newVector.RowNames = rowNames;
            newVector.ColNames = colNames;
            int i = 0;
            foreach (var row in rowNames)
            {
                if (!this.ContainsRowName(row)) continue;

                foreach (var col in colNames)
                {
                    if (!this.ContainsColName(col)) continue;

                    newVector[row, col] = this[row, col];
                }
                i++;
            }
            return newVector;
        }


        /// <summary>
        /// 是否包含行名称
        /// </summary>
        /// <param name="rowName">行名称</param>
        /// <returns></returns>
        public bool ContainsRowName(string rowName)
        {
            return this.RowNames.Contains(rowName);
        }

        /// <summary>
        /// 是否包含列名称
        /// </summary>
        /// <param name="colName">列名称</param>
        /// <returns></returns>
        public bool ContainsColName(string colName)
        {
            return this.ColNames.Contains(colName);
        }

        /// <summary>
        /// 行名称
        /// </summary>
        public virtual List<string> RowNames { get; set; }

        /// <summary>
        /// 列名称
        /// </summary>
        public virtual List<string> ColNames { get; set; } 
        public MatrixType MatrixType
        {
            get { return Algorithm.MatrixType.VectorMatrix; }
        }

        public double[][] Array
        {
            get { return MatrixUtil.Create(this.OneDimArray); }
        }

        public int ItemCount
        {
            get { return Count; }
        }

        public int ColCount
        {
            get {  return 1;}
        }

        public int RowCount
        {
            get { return Count; }
        }

        public bool IsSquare   {  get { return RowCount == ColCount; }  }

        public bool IsSymmetric { get { return IsSquare; } }
        public double this[int i, int j]
        {
            get
            {
                if (j != 0) { throw new Exception("单列向量列号始终为0"); }
                return this[i];
            }
            set
            {
                if (j != 0) { throw new Exception("单列向量列号始终为0"); }
                this[i] = value;
            }
        }

        public Vector GetRow(int index)
        {
            return new Vector( new Double[]{ this[index]});
        }

        public Vector GetCol(int index)
        {
            if (index == 0) return this;
            throw new ArgumentException("向量矩阵只有一维。"); 
        }

        public IMatrixFactory Factory { get; set; }

        public IMatrix Transposition
        {
            get { return  new ArrayMatrix(this).Transposition; }
        }

        public IMatrix GetInverse()
        {
            if (RowCount == 1) return new ArrayMatrix(1, 1, 1 / this[0]);
            throw new NotImplementedException("非方阵，没有逆矩阵。");
        }

        public new IMatrix Clone()
        {
            return new VectorMatrix(this.OneDimArray); 
        }

        public IMatrix Plus(IMatrix right)
        {
            if (right == null) { return this; }

            if (this.RowCount != right.RowCount ||
                ColCount != right.ColCount
                )
            throw new ArgumentException("维数不正确！不可及操作。");

            VectorMatrix matrix = new VectorMatrix(this.Count);
           // var right1 = right as VectorMatrix;
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                matrix[i] = this[i] + right[i, 0];
            }
            return matrix;
        }

        public IMatrix Minus(IMatrix right)
        {
            if (right == null ){ return this; }

            if (this.RowCount != right.RowCount ||
                ColCount != right.ColCount
                )
                throw new ArgumentException("维数不正确！不可及操作。");

            VectorMatrix matrix = new VectorMatrix(this.Count);
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                matrix[i] = this[i] - right[i, 0];
            }
            return matrix;
        }
        /// <summary>
        /// 矩阵乘法，左边列数应该等于右边行
        /// N * 1 列向量，只乘以 1 * M 的行向量，结果为 N*M 的矩阵。
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public IMatrix Multiply(IMatrix right)
        {
            if (ColCount != right.RowCount)
                throw new ArgumentException("维数不正确！不可及操作。");

            if (right.ColCount == 1)//返回仍然为列向量
            {
                VectorMatrix matrix = new VectorMatrix(this.Count);
                double val = right[0, 0];
                for (int i = 0; i < this.Count; i++)
                {
                    matrix[i] = this[i] * val;
                }
                return matrix;
            }
            ArrayMatrix my = new ArrayMatrix(this);
            return my.Multiply(right);
        }

        IMatrix IArithmeticOperation<IMatrix>.Multiply(double right)
        {
            VectorMatrix matrix = new VectorMatrix(this.Count);
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                matrix[i] = this[i] * right;
            }
            return matrix;
        }
        IMatrix IArithmeticOperation<IMatrix>.Divide(double right)
        {
            VectorMatrix matrix = new VectorMatrix(this.Count);
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                matrix[i] = this[i] / right;
            }
            return matrix;
        }

        IMatrix IArithmeticOperation<IMatrix>.Opposite()
        { 
            VectorMatrix matrix = new VectorMatrix(this.Count);
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                matrix[i] = - this[i];
            }
            return matrix;
        }
        /// <summary>
        /// +
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static VectorMatrix operator +(VectorMatrix vector, IMatrix matrix) { return (VectorMatrix)vector.Plus(matrix); }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static VectorMatrix operator -(VectorMatrix vector, IMatrix matrix) { return (VectorMatrix)vector.Minus(matrix); }
        /// <summary>
        /// *
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static VectorMatrix operator *(VectorMatrix vector, IMatrix matrix) { return (VectorMatrix)vector.Multiply(matrix); }
        /// <summary>
        /// *
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static VectorMatrix operator *(VectorMatrix vector, double matrix) { return (VectorMatrix)vector.Multiply(matrix); }

        /// <summary>
        /// 迭代
        /// </summary>
        /// <returns></returns>
        public  System.Collections.IEnumerator GetEnumerator()
        { 
            return  base.GetEnumerator();
           // throw new NotImplementedException();
        }

        //public IMatrix Plus(IMatrix right)
        //{
        //    throw new NotImplementedException();
        //}

        //public IMatrix Minus(IMatrix right)
        //{
        //    throw new NotImplementedException();
        //}

        //public IMatrix Multiply(IMatrix right)
        //{
        //    throw new NotImplementedException();
        //}

        public IMatrix SubMatrix(int fromIndex, int count)
        {
            var vm = new VectorMatrix(count);
            var endIndexExcluded = fromIndex + count;
            for (int i = fromIndex, j = 0; i < endIndexExcluded; i++, j++)
            {
                vm.Data[j] = this.Data[i];
            }
            return vm;
        }



        public bool IsDiagonal
        {
            get {  return RowCount == 1; }
        }
        
        /// <summary>
        /// 行名称是否有效，可用。
        /// </summary>
        public bool IsRowNameAvailable { get => (this.RowNames != null && this.RowNames.Count == this.RowCount); }

        /// <summary>
        /// 列名称是否有效，可用。
        /// </summary>
        public bool IsColNameAvailable { get => (this.ColNames != null && this.ColNames.Count == this.ColCount); }
        /// <summary>
        /// 是否具有有效的行列名称
        /// </summary>
        public bool HasParamNames => IsColNameAvailable && IsRowNameAvailable;


        public IMatrix Pow(double power)
        {
            VectorMatrix m = new VectorMatrix(this.Count);
            for (int i = 0; i < this.Count; i++)
            {
                m[i] = Math.Pow(this[i], power);
            }

            return m;
        }

        /// <summary>
        /// 批量移除行，返回新矩阵
        /// </summary>
        /// <param name="rowIndexes"></param>
        /// <returns></returns>
        public virtual IMatrix GetRowRemovedMatrix(List<int> rowIndexes)
        {
            foreach (var item in rowIndexes)
            {
                if (item >= this.RowCount) { throw new Exception("索引太大，超界！"); }
            }
            var data = this.Array;
            int newRowCount = this.RowCount - rowIndexes.Count;
            var newData = new double[newRowCount][];

            for (int i = 0, j = 0; i < RowCount; i++)
            {
                if (rowIndexes.Contains(i)) { continue; }
                newData[j] = data[i];//这样直接赋值，省了代码和空间，但是和母对象公用对象，若更改可能引起连锁反映。
                j++;
            }
            return new ArrayMatrix(newData);
        }
        /// <summary>
        /// 移除指定行,返回新矩阵
        /// </summary>
        /// <param name="rowIndex"></param>
        public virtual IMatrix GetRowRemovedMatrix(int rowIndex)
        {
            if (RowCount <= rowIndex) { return this; }
            var data = this.Array;
            int newRowCount = this.RowCount - 1;
            var newData = new double[newRowCount][];

            for (int i = 0, j = 0; i < RowCount; i++)
            {
                if (i == rowIndex) { continue; }
                newData[j] = data[i];//这样直接赋值，省了代码和空间，但是和母对象公用对象，若更改可能引起连锁反映。
                j++;
            }
            return new ArrayMatrix(newData);
        }

        /// <summary>
        /// 批量移除列，返回新矩阵
        /// </summary>
        /// <param name="colndexes"></param>
        /// <returns></returns>
        public virtual IMatrix GetColRemovedMatrix(List<int> colndexes)
        {
            foreach (var item in colndexes)
            {
                if (item >= this.ColCount) { throw new Exception("索引太大，超界！"); }
            }

            var data = this.Array;
            int newColCount = this.ColCount - colndexes.Count;
            var newData = Geo.Utils.MatrixUtil.Create(RowCount, newColCount);

            for (int i = 0; i < RowCount; i++)
            {
                for (int k = 0, j = 0; k < ColCount; k++)
                {
                    if (colndexes.Contains(k)) { continue; }
                    newData[i][j] = data[i][k];
                    j++;
                }
            }
            return new ArrayMatrix(newData);
        }

        /// <summary>
        /// 移除指定列,返回新矩阵
        /// </summary>
        /// <param name="colIndex"></param>
        public virtual IMatrix GetColRemovedMatrix(int colIndex)
        {
            if (ColCount <= colIndex) { return this; }
            var data = this.Array;
            int newColCount = this.ColCount - 1;
            var newData = Geo.Utils.MatrixUtil.Create(RowCount, newColCount);

            for (int i = 0; i < RowCount; i++)
            {
                for (int k = 0, j = 0; k < ColCount; k++)
                {
                    if (k == colIndex) { continue; }
                    newData[i][j] = data[i][k];
                    j++;
                }
            }
            return new ArrayMatrix(newData);
        }

    }
}
