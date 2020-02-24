//2015.10.02, czs, create in 彭州, URL生成器
//2015.10.06, czs, edit in 彭州到成都动车C6186, 时间段字符串生成器


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
    /// 时间段字符串生成器
    /// </summary>
    public class TimeScopeStringGeneratorParam : OutputParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TimeScopeStringGeneratorParam()
        {
            this.StartTime = DateTime.MinValue;
            this.EndTime = DateTime.MinValue;
            this.Interval = 86400;
        }
         
        /// <summary>
        /// 地址模板
        /// </summary>
       
        [Display(Name = "地址模板")] public string Pattern { get; set; }
        /// <summary>
        /// 起始时间，默认值 DateTime.MinValue
        /// </summary>
       [Display(Name = "起始时间")] public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间，默认值 DateTime.MinValue
        /// </summary>
      [Display(Name = "结束时间")]  public DateTime EndTime { get; set; }
        /// <summary>
        /// 时间间隔，默认为1天,即86400秒。最高分辨率秒。
        /// </summary>
      [Display(Name = "采样间隔")]  public int Interval { get; set; } 
    }
}
