//2014.09.16, czs, create,  具有一个Value属性

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 具有一个Value属性。
    /// </summary>
    public  interface IValue <TValue> 
    {
        /// <summary>
        /// Value 属性
        /// </summary>
        TValue Value { get; set; }
    }
}
