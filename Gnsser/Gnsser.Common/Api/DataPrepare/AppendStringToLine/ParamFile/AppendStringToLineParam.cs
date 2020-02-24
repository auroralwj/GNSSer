//2015.10.07, create in xi'an, 追加参数到Gnsser参数文件

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
    public class AppendStringToLineParam : RowClass
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppendStringToLineParam()
        {
        } 
        /// <summary>
        /// 目标目录或者文件
        /// </summary>
           [Display(Name = "内容")]
        public string Content { get; set; }
        /// <summary>
        /// 源目录或者文件
        /// </summary>
           [Display(Name = "源目录或者文件")]
        public string FileToAppdend { get; set; }
    }
}
