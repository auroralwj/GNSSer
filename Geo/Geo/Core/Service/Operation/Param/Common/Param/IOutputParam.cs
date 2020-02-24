//2015.10.09, czs, create in xi'an hongqing, 具有输出参数的接口
//2015.10.26, czs, edit ni xi'an hongqing, 增加属性 OrderedPropertyNames

using System;
using System.ComponentModel.DataAnnotations;
using Geo.IO;
using Geo;

namespace Geo.IO
{ 
    /// <summary>
    /// 顶层文件输出参数类
    /// </summary>
    public abstract class OutputParam : RowClass, IOutputParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public   OutputParam (){
            this.IsOverwrite = true;
            this.OrderedProperties = new System.Collections.Generic.List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<OutputParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<OutputParam>(m=>m.IsOverwrite), 
            };
        }

        /// <summary>
        /// 是否覆盖 overwrite，默认为真。
        /// </summary>
        [Display(Name = "是否覆盖")]
        public bool IsOverwrite { get; set; } 

        /// <summary>
        /// 输出路径，目录路径或文件路径。
        /// </summary>

        [Display(Name = "输出路径")]
        public string OutputPath { get; set; }
    }

    /// <summary>
    /// 具有输出参数的接口
    /// </summary>
    public interface IOutputParam : IRowClass
    {
        /// <summary>
        /// 输出路径
        /// </summary>
        string OutputPath { get; set; }
    } 

}
