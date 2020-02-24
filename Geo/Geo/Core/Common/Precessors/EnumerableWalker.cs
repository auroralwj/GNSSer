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
    /// 数据遍历处理器
    /// </summary>
    /// <typeparam name="TMaster">要访问的数据类型</typeparam>
    public class EnumerableWalker<TMaster> : IWalker
    {
        /// <summary>
        /// 数据遍历处理器，构造函数。
        /// </summary>
        /// <param name="data"></param>
        public EnumerableWalker(IEnumerable<TMaster> data)
        {
            this.EnumerableData = data;
            this.ProcessorChain = new ReviserManager<TMaster>(); 
        }
        /// <summary>
        /// 可遍历的数据源。
        /// </summary>
        public IEnumerable<TMaster> EnumerableData { get; set; }
        /// <summary>
        /// 处理器。访问者设计模式。一个责任链。
        /// </summary>
        public ReviserManager<TMaster> ProcessorChain { get; set; } 

        /// <summary>
        /// 遍历数据
        /// </summary> 
        public void Walk()
        {
            foreach (var data in EnumerableData)
            {
                var obj = data;
                if (!ProcessorChain.Revise(ref obj))
                {
                    //throw new Exception(ProcessorChain.Message);
                }
            }
        }         
    } 
 
}