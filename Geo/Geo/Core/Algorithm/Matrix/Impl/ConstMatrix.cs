//2014.11.19， czs, create, namu, 全 同一数字常量 矩阵。节约内存，快速计算。

using System;
using Geo.Algorithm;
using Geo.Common;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 全 同一数字常量 矩阵。节约内存，快速计算。
    /// </summary>
    public  class ConstMatrix : AbstractMatrix, IMatrix
    { 
        /// <summary>
        /// 初始化一个全 同一数字常量 方阵。
        /// </summary>
        /// <param name="rowCol"></param>
        public ConstMatrix(int rowCol, double val = 0) : this(rowCol, rowCol, val) { }
        /// <summary>
        /// 初始化全 同一数字常量 矩阵
        /// </summary>
        /// <param name="rowCount">行数</param>
        /// <param name="colCount">列数</param>
        public ConstMatrix(int rowCount = 1, int colCount = 1, double val = 0) :base( MatrixType.ConstMatrix )
        { 
            this.ColCount = colCount;
            this.RowCount = rowCount;
            this.Value = val;
        }
        /// <summary>
        /// 数值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 请后面的同志去实现吧!!
        /// </summary>
        public override  double[][] Array
        {
            get { return MatrixUtil.Create(RowCount, ColCount, Value); }
        }

        /// <summary>
        /// 所有元素总和。指有效的内容表示数，矩阵内容必须的数。
        /// </summary>
        public override int ItemCount { get { return 1; } }
          
        public new int ColCount { get; private set; }

        public new int RowCount { get; private set; }
         
        public override bool IsSymmetric
        {
            get { return IsSquare; }
        }

        public override double this[int i, int j]
        {
            get
            {
                return Value;
            }
            set
            {
                throw new NotImplementedException("省省吧！无论怎么设置，都是0.");
            }
        }

        public override Vector GetRow(int index)
        {
           return new Vector(ColCount, Value);
        }

        public override Vector GetCol(int index)
        {
            return new Vector(RowCount, Value);
        }
         

        public override IMatrix Transposition
        {
            get { return new ConstMatrix( ColCount, RowCount, Value ); }
        }

        public override IMatrix Pow(double power)
        {
            return new ConstMatrix(ColCount, RowCount, Value * Value);
        }

        public override IMatrix Clone()
        {
            return new ConstMatrix(RowCount, ColCount, Value);
        }

        //public IMatrix GetPlus(IMatrix right)
        //{
        //    return right.GetPlus(;
        //}

        //public IMatrix GetMinus(IMatrix right)
        //{
        //    return right.GetMinus();
        //}

        //public IMatrix GetMulti(IMatrix right)
        //{
        //    return new ZeroMatrix(this.RowCount, right.ColCount);
        //}

        //public IMatrix GetMulti(double right)
        //{
        //    return this;
        //}

        //public IMatrix GetMinus()
        //{
        //    return this;
        //}

        public override IMatrix SubMatrix(int fromIndex, int count)
        {
            var m = new ConstMatrix(count, count, this.Value);
            if (this.ColNames != null && this.ColNames.Count > 0) { m.ColNames = this.ColNames.GetRange(fromIndex, count - fromIndex); }
            if (this.RowNames != null && this.RowNames.Count > 0) { m.RowNames = this.RowNames.GetRange(fromIndex, count - fromIndex); }

            return m;
        }
    }
}
