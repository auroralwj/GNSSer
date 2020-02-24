//2015.10.09, czs, create in xi'an hongqing, 具有输出参数的接口

using System;
using System.ComponentModel.DataAnnotations;

namespace Geo.IO
{ 
    /// <summary>
    /// 顶层文件输入输出参数类
    /// </summary>
    public class IoParam : OutputParam, IIoParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IoParam()
        {
            this.InputPath = @"..\..\Observation";
            this.OutputPath = @"..\..\Output";

            this.OrderedProperties = new System.Collections.Generic.List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.IsOverwrite),
            };
        }

        /// <summary>
        /// 输入路径
        /// </summary> 
        [Display(Name = "输入路径")]
        public string InputPath { get; set; }         
    }

    /// <summary>
    /// 具有输入和输出参数的接口
    /// </summary>
    public interface IIoParam : IOutputParam, IInputParam
    { 
    } 

}
