//2015.10.22, czs, create in xi'an hongqing, 顶层，具有输入文件或目录，或URL的接口

using System;
using System.ComponentModel.DataAnnotations;
using Geo;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// 顶层，具有输入文件或目录，或URL的接口
    /// </summary>
    public interface IInputParam : IRowClass
    {
        /// <summary>
        /// 输入路径，可以是文件、目录或URL
        /// </summary>
        string InputPath { get; set; }
    }

    /// <summary>
    /// 顶层，具有输入文件或目录，或URL的接口
    /// </summary>
    public class InputParam : IInputParam
    {
        public InputParam()
        {
             InputPath = "Observation";
            OrderedProperties = new System.Collections.Generic.List<string>
            { 
                Geo.Utils.ObjectUtil.GetPropertyName<InputParam>(m=>m.InputPath)
            };
        }
        
        /// <summary>
        /// 输入路径，可以是文件、目录或URL
        /// </summary>
       [Display(Name = "输入路径")] public string InputPath { get; set; }

       public List<ValueProperty> Properties { get; protected set; }

       public  List<string> OrderedProperties { get; protected set; }
    }
}
