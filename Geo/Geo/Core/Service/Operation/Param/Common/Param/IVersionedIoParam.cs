//2015.10.09, czs, create in xi'an hongqing, 具有输出参数的接口
//2015.10.28, czs, edit in hongqing, 具有输出版本号的参数

using System;
using System.ComponentModel.DataAnnotations;

namespace Geo.IO
{ 
    public interface IVersionedIoParam : IIoParam{
        /// <summary>
        /// 版本
        /// </summary>
        double OutputVersion{get;}
    }
    /// <summary>
    /// 顶层文件输入输出参数类
    /// </summary>
    public class VersionedIoParam: IoParam,IVersionedIoParam 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VersionedIoParam()
        {
            this.OutputVersion = 3.02;
            this.OrderedProperties = new System.Collections.Generic.List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<VersionedIoParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<VersionedIoParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<VersionedIoParam>(m=>m.IsOverwrite),
                Geo.Utils.ObjectUtil.GetPropertyName<VersionedIoParam>(m=>m.OutputVersion),
            };
        }

        /// <summary>
        /// 版本
        /// </summary> 
        [Display(Name = "输出版本")]  
        public double OutputVersion{get; set;}         
    }  

}
