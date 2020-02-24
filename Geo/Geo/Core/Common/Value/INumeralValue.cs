//2014.09.16, czs, create, 
//2015.05.25，czs, add in namu, 增加是否值为 0 的判断属性。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 具有一个双精度Value属性。
    /// </summary>
    public interface INumeralValue : IValue<Double>
    {
        /// <summary>
        /// 值是否为 0 
        /// </summary>
        bool IsZero { get; }

    }
 
}
