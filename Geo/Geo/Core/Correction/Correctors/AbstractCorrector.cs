//2014.09.14, czs, create, 指定了改正数类型的改正器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Correction
{ 
    /// <summary>
    /// 指定了改正数类型的改正器。
    /// </summary>
    /// <typeparam name="TValue">改正数类型</typeparam>
    /// <typeparam name="TInput">需要计算的类型</typeparam>
    public abstract class AbstractCorrector<TValue, TInput> : ICorrector<TValue, TInput> ,ICorrector 
    {
        /// <summary>
        /// 改正数
        /// </summary>
        public virtual TValue Correction { get; protected set; }
        /// <summary>
        /// 改正器或改正数的名称
        /// </summary>
        public string Name { get;  set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {

        }
        /// <summary>
        /// 完成
        /// </summary>
        public virtual void Complete()
        {

        }
        /// <summary>
        /// 执行改正。
        /// </summary>
        public abstract void Correct(TInput input); 

         
        /// <summary>
        /// 字符显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = Name ?? this.GetType().ToString();

            return str + ":" + Correction;
        }
    }  
}
