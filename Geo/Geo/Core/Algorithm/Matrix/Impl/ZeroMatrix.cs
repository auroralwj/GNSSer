//2014.11.19， czs, create, namu, 全 0 矩阵。节约内存，快速计算。

using System;
using System.Collections.Generic;
using Geo.Algorithm;
using Geo.Algorithm;
using Geo.Common;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 全 0 矩阵。节约内存，快速计算。
    /// </summary>
    public  class ZeroMatrix : IMatrix
    { 
        /// <summary>
        /// 初始化一个全 0 方阵。
        /// </summary>
        /// <param name="rowCol"></param>
        public ZeroMatrix(int rowCol) : this(rowCol, rowCol) { }
        /// <summary>
        /// 初始化一个全零矩阵。
        /// </summary>
        /// <param name="rowCount">行数</param>
        /// <param name="colCount">列数</param>
        public ZeroMatrix(int rowCount = 1, int colCount = 1) 
        {  
            this.ColCount = colCount;
            this.RowCount = rowCount;
        }  /// <summary>
        /// 获取指定参数的矩阵
        /// </summary>
        /// <param name="paramNames">参数列表</param>
        /// <returns></returns>
        public IMatrix GetMatrix(List<String> paramNames)
        {
            return GetMatrix(paramNames, paramNames);
        }/// <summary>
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
        /// 获取指定的元素。
        /// </summary>
        /// <param name="rowName">行名称</param>
        /// <param name="colName">列名称</param>
        /// <returns></returns>
        public virtual double this[string rowName, string colName]
        {
            set { this.Array[GetRowIndex(rowName)][GetRowIndex(colName)] = value; }
            get { return this.Array[GetRowIndex(rowName)][GetRowIndex(colName)]; }
        }
        /// <summary>
        /// 行名称
        /// </summary>
        public List<string> RowNames { get; set; }

        /// <summary>
        /// 列名称
        /// </summary>
        public List<string> ColNames { get; set; }
        
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
        /// <summary>
        /// 请后面的同志去实现吧!!
        /// </summary>
        public double[][] Array
        {
            get { return MatrixUtil.Create(RowCount, ColCount); }
        }

        /// <summary>
        /// 所有元素总和。指有效的内容表示数，矩阵内容必须的数。
        /// </summary>
        public int ItemCount { get { return 0; } }

        public string Name { get; set; }

        public MatrixType MatrixType
        {
            get { return MatrixType.ZeroMatrix;; }
        }

        public double Tolerance { get; set; }

        public int ColCount { get; set; }

        public int RowCount { get; set; }

        public bool IsSquare
        {
            get { return ColCount == RowCount; }
        }

        public bool IsSymmetric
        {
            get { return IsSquare; }
        }

        public double this[int i, int j]
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotImplementedException("省省吧！无论怎么设置，都是0.");
            }
        }

        public Vector GetRow(int index)
        {
           return new Vector(ColCount);
        }

        public Vector GetCol(int index)
        {
            return new Vector(RowCount);
        }
         
        public IMatrix Transposition
        {
            get { return new ZeroMatrix( ColCount, RowCount ); }
        }

        public IMatrix GetInverse()
        {
            throw new NotImplementedException("全 0 矩阵，无此功能！"); 
        }

        public IMatrix Clone()
        {
            return new ZeroMatrix(RowCount, ColCount);
        }

        public IMatrix Plus(IMatrix right)
        {
            return right;
        }

        public IMatrix Minus(IMatrix right)
        {
            return right.Opposite();
        }

        public IMatrix Multiply(IMatrix right)
        {
            return new ZeroMatrix(this.RowCount, right.ColCount);
        }

        public IMatrix Multiply(double right)
        {
            return this;
        }

        public IMatrix Opposite()
        {
            return this;
        }


        public IMatrix SubMatrix(int fromIndex, int count)
        {
            var m = new ZeroMatrix(count);
            if (this.ColNames != null && this.ColNames.Count > 0) { m.ColNames = this.ColNames.GetRange(fromIndex, count - fromIndex); }
            if (this.RowNames != null && this.RowNames.Count > 0) { m.RowNames = this.RowNames.GetRange(fromIndex, count - fromIndex); }
            return m;
        }




        public bool IsDiagonal
        {
            get { return RowCount == ColCount; }
        }


        public IMatrix Divide(double right)
        {
            return this;
        }


        public IMatrix Pow(double power)
        {
            return this;
        }

        public IMatrix GetRowRemovedMatrix(List<int> rowIndexes)
        {
            throw new NotImplementedException();
        }

        public IMatrix GetRowRemovedMatrix(int rowIndex)
        {
            throw new NotImplementedException();
        }

        public IMatrix GetColRemovedMatrix(List<int> colIndexes)
        {
            throw new NotImplementedException();
        }

        public IMatrix GetColRemovedMatrix(int colIndex)
        {
            throw new NotImplementedException();
        }
    }
}
