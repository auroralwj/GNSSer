//2014.11.07, czs, create in namu, 索引器，常用数组、集合、坐标等。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 索引器
    /// </summary>
    public interface IIndexing<TValueType>
    {
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        TValueType this[int i] { get; set; }
    }

    /// <summary>
    /// 索引器
    /// </summary>
    public interface INumeralIndexing : IIndexing<Double>
    { 
    }
}
