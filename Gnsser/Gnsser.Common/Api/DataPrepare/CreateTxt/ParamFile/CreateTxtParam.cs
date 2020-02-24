//2015.10.07, czs, create in 安康到西安临客K8182, 文本文件生成器

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
    public class CreateTxtParam : OutputParam 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateTxtParam()
        {
        }
         
        /// <summary>
        /// 文本内容
        /// </summary>
        [Display(Name = "文本内容")]
        public string Content { get; set; }
    }
}
