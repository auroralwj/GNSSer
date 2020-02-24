//2014.12.02, czs, create, 具有协方差的值

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;
using Geo;

namespace Geo.Algorithm
{
    /// <summary>
    /// 具有权值的值。
    /// </summary>
    public class CovariancedVector : Vector, ICovarianced
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="val">数值</param>
        /// <param name="weight">每个分量的均方根</param>
        public CovariancedVector(IVector val, IMatrix weight)
            : base(val)
        { 
            this.Covariance = weight;
        }

        /// <summary>
        /// RMS 属性
        /// </summary>
        public virtual IMatrix Covariance { get; set; }

        #region override
        public override bool Equals(object obj)
        {
            CovariancedVector o = obj as CovariancedVector;

            return  base.Equals(o) && o.Covariance.Equals(this.Covariance);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * 3 + Covariance.GetHashCode() * 5;
        }

        public override string ToString()
        {
            return base.ToString() + "(" + Covariance.ToString() + ")";
        }
        #endregion
    }
}
