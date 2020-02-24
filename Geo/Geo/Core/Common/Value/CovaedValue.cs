//2019.03.09, czs, create, 具有协方差阵的对象

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 具有协方差阵的对象
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TCova"></typeparam>
    public class CovaedValue<TValue, TCova>
    {
        public CovaedValue()
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Cova"></param>
        public CovaedValue(TValue Value, TCova Cova)
        {
            this.Value = Value;
            this.Cova = Cova;
        }
        /// <summary>
        /// 数值
        /// </summary>
        public TValue Value { get; set; }
        /// <summary>
        /// 协方差阵
        /// </summary>
        public TCova Cova { get; set; }

        public override bool Equals(object obj)
        {
            var val = obj as CovaedValue<TValue, TCova>;
            if (val == null) return false;
            return Value.Equals(val.Value) && Cova .Equals(val.Cova);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode() + Cova.GetHashCode() * 13;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }


}