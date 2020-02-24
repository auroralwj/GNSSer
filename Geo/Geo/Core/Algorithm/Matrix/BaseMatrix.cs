//2014.10.05， czs, create, 海鲁吐嘎查 通辽,一个基本的矩阵类

using System;
using Geo.Algorithm;
using Geo.Common;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 基础实现矩阵。提供一些通用的方法和设置。
    /// </summary>
    public  abstract class BaseMatrix: AbstractMatrix
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MatrixType"></param>
        /// <param name="tolerance"></param>
        public BaseMatrix(MatrixType MatrixType, double tolerance = 1e-13)
            : base(MatrixType)
        {
            this.Tolerance = tolerance;
        }

        /// <summary>
        /// 请后面的同志去实现吧!!
        /// </summary>
        //public override double[][] Array
        //{
        //    get { throw new NotImplementedException(); }
        //}

        /// <summary>
        /// 所有元素总和。指有效的内容表示数，矩阵内容必须的数。
        /// </summary>
      //  public abstract int ItemCount { get; }
    }
}
