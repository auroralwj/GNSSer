//2015.10.24, czs, create in 彭州, 单点定位

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
    public class DoubleDifferParam : PointPositionParam// VersionedIoParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DoubleDifferParam()
        {
            this.ParallelProcessCount = 4;
            this.IsParallel = true;

            this.OrderedProperties = new System.Collections.Generic.List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.IsOverwrite),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.OutputVersion),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.BaselinePath),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.SiteInfoPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.IsParallel),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.ParallelProcessCount),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.EphemerisPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.ClockPath),
            };
        }

        /// <summary>
        /// 输入路径
        /// </summary> 
        [Display(Name = "基线地址")]
        public string BaselinePath { get; set; }
        /// <summary>
        /// 输入路径
        /// </summary> 
        [Display(Name = "测站信息地址")]
        public string SiteInfoPath { get; set; } 
 }
}
