//2015.10.02, czs, create in 彭州, URL生成器
//2015.10.06, czs, edit in 彭州到成都动车C6186, 时间段字符串生成器
//2015.10.07, czs, edit in 安康到西安临客K8182, 星历地址生成器，继承TimeScopeStringGeneratorParam


using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Geo.Common;
using Geo.Coordinates;
using Geo.IO;
using System.ComponentModel.DataAnnotations;

namespace Gnsser.Api
{
    /// <summary>
    /// 时间段字符串生成器
    /// </summary>
    public class Sp3UrlGeneratorParam : TimeScopeStringGeneratorParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Sp3UrlGeneratorParam()
        {
            this.StartTime = DateTime.MinValue;
            this.EndTime = DateTime.MinValue;
            this.Interval = 86400;
        }
         
        /// <summary>
        /// 数据源名称
        /// </summary>
      

        [Display(Name = "数据源名称")]  public string SourceName { get; set; }

        /// <summary>
        /// 下载后的文件夹
        /// </summary>
         [Display(Name = "本地目录")]  public string LocalDirectory { get; set; }
        
    }
}
