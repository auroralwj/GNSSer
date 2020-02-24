//2015.01.10, czs, create in namu, RINEX 观测值数据源游走器/遍历器。

using System;
using System.Collections.Generic;
using System.Text;
using Geo;

namespace Gnsser.Domain
{
    /// <summary>
    /// RINEX 观测值数据源游走器/遍历器。
    /// </summary>
    public class EpochDataSourceWalker : EnumerableWalker<EpochInformation>
    { 
        /// <summary>
        /// 数据遍历处理器，构造函数。
        /// </summary>
        /// <param name="satData"></param>
        public EpochDataSourceWalker(IEnumerable<EpochInformation> data)
            : base(data)
        { 
        }

    }
}
