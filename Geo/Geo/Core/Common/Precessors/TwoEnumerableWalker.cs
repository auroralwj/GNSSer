//2015.01.10, czs, create in namu,  数据遍历器  StreamDataWalker

using System;
using System.Collections.Generic;
using System.Text;
using System.IO; 
using Geo; 
using Geo.Utils;
using Geo.Common; 

namespace Geo
{  
    /// <summary>
    /// 两个数据源的数据遍历处理器
    /// </summary>
    /// <typeparam name="TMaster">要访问的数据类型</typeparam>
    public abstract class TwoEnumerableWalker<TMaster> : IWalker
    {
        /// <summary>
        /// 数据遍历处理器，构造函数。
        /// </summary>
        /// <param name="dataA">数据源A</param>
        /// <param name="dataB">数据源B</param>
        public TwoEnumerableWalker(IEnumerable<TMaster> dataA, IEnumerable<TMaster> dataB)
        {
            this.EnumerableDataA = dataA;
            this.EnumerableDataB = dataB;
            this.ProcessorChain = new TwinsReviserManager<TMaster>(); 
        }
        /// <summary>
        /// 可遍历的数据源A
        /// </summary>
        public IEnumerable<TMaster> EnumerableDataA { get; set; }
        /// <summary>
        /// 可遍历的数据源B 
        /// </summary>
        public IEnumerable<TMaster> EnumerableDataB { get; set; }
        /// <summary>
        /// 处理器。访问者设计模式。一个责任链。
        /// </summary>
        public TwinsReviserManager<TMaster> ProcessorChain { get; set; } 

        /// <summary>
        /// 遍历数据
        /// </summary> 
        public abstract void Walk(); 
    } 
}