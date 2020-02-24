//2017.08.31, czs, edit in hongqing, 重构

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 平差结果,具有原材料
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    public class AdjustmentResult<TMaterial> : AdjustmentResult
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public AdjustmentResult()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Adjustment">平差结果</param> 
        /// <param name="material">原材料</param> 
        public AdjustmentResult(AdjustResultMatrix Adjustment, TMaterial material )
        {
            this.ResultMatrix = Adjustment;
            this.Material = material;
        }
        /// <summary>
        /// 数据源实体
        /// </summary>
        public virtual TMaterial Material { get; set; }
    }

    /// <summary>
    /// 平差结果
    /// </summary>
    public class AdjustmentResult : IAdjustmentResult
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public AdjustmentResult()
        { 
        } 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Adjustment">平差结果</param> 
        public AdjustmentResult(AdjustResultMatrix Adjustment)
        {
            this.ResultMatrix = Adjustment;
        }

        /// <summary>
        /// 平差结果
        /// </summary>
        public AdjustResultMatrix ResultMatrix { get; set; }  
    }


    /// <summary>
    /// 平差结果
    /// </summary>
    public interface IAdjustmentResult
    {

        /// <summary>
        /// 平差结果
        /// </summary>
        AdjustResultMatrix ResultMatrix { get; set; }
    }
}
