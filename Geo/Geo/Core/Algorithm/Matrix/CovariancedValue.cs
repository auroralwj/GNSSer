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
    public class CovariancedValue<TValue> : BaseValue<TValue>, ICovarianced
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="val">数值</param>
        /// <param name="weight">每个分量的均方根</param>
        public CovariancedValue(TValue val, IMatrix weight)
            : base(val)
        {
            this.Value = val;
            this.Covariance = weight;
        }

        /// <summary>
        /// RMS 属性
        /// </summary>
        public virtual IMatrix Covariance { get; set; }

        #region override
        public override bool Equals(object obj)
        {
            CovariancedValue<TValue> o = obj as CovariancedValue<TValue>;

            return o.Value.Equals(this.Value) && o.Covariance.Equals(this.Covariance);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() * 3 + Covariance.GetHashCode() * 5;
        }

        public override string ToString()
        {
            return Value.ToString() + "(" + Covariance.ToString() + ")";
        }
        #endregion
    }
}
