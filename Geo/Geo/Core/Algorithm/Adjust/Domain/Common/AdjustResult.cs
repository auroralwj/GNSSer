//2017.08.31, czs, edit in hongqing, 重构
//2018.03.24, czs edit in hmx, 重构为模板

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 平差结果
    /// </summary>
    public class AdjustResult : AdjustResult<AdjustResultMatrix>
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public AdjustResult()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Adjustment">平差器</param> 
        public AdjustResult(AdjustResultMatrix Adjustment):base(Adjustment)
        { 
        }
    }

    /// <summary>
    /// 平差结果
    /// </summary>
    public class AdjustResult<TResultMatrix> where TResultMatrix : AdjustResultMatrix
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public AdjustResult()
        { 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Adjustment">平差器</param> 
        public AdjustResult(TResultMatrix Adjustment)
        {
            this.Adjustment = Adjustment;
        }

        /// <summary>
        /// 平差器
        /// </summary>
        public TResultMatrix Adjustment { get; set; } 
    }
}
