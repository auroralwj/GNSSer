//2015.10.01, czs, create in K385宝鸡到成都列车上, 解压

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Geo.Common;
using Geo.Coordinates;
using Geo.IO;
using Geo;
using System.ComponentModel.DataAnnotations;

namespace Gnsser.Api
{
    /// <summary>
    /// 通用表格文件
    /// </summary>
    public class DecompressParam : IoParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DecompressParam()
        {
            IsDeleteSource = false; 
            this.OrderedProperties = new System.Collections.Generic.List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.IsOverwrite),
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.IsDeleteSource),
            };
        } 
 
        /// <summary>
        /// 是否覆盖 overwrite。默认否
        /// </summary>
      
        [Display(Name = "是否删除源文件")] 
        public bool IsDeleteSource { get; set; } 
 }
}
