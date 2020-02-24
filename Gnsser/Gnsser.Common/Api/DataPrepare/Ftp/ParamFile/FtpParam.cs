//2015.09.29, czs, create in xi'an hongqing, Ftp下载参数
//2016.11.28, czs, edit in hongqing, 通用 FtpParam 参数

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
    public class FtpParam : IoParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FtpParam()
        { 

            this.OrderedProperties = new System.Collections.Generic.List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m=>m.IsOverwrite), 
                Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m=>m.UserName),
                Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m=>m.Password),
            };
        }

        /// <summary>
        /// 文件类型，只有下载时起作用
        /// </summary>
        [Display(Name = "文件类型")]
        public string Extension { get; set; } 
        
        /// <summary>
        /// 用户名
        /// </summary>
       [Display(Name = "用户名")]  public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>       
        [Display(Name = "密码")]  public string Password { get; set; } 
    }
}
