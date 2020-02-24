//2015.10.09, czs, create in  xi'an hongqing, 坐标转换

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Geo.Common;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.IO;
using Geo;
using System.ComponentModel.DataAnnotations;
       

namespace Gnsser.Api
{
    /// <summary>
    /// 时间段字符串生成器
    /// </summary>
    public class XyzToGeoCoordParam :  OutputParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XyzToGeoCoordParam()
        {
            this.Ellipsoid = Ellipsoid.WGS84;
            this.AngleUnit = Geo.Coordinates.AngleUnit.Degree;
        }
        /// <summary>
        /// 空间直角坐标
        /// </summary>
        [Display(Name = "空间直角坐标")]
        public XYZ Xyz { get; set; }

        /// <summary>
        /// 参考椭球
        /// </summary>
        [Display(Name = "参考椭球")]
        public Ellipsoid Ellipsoid { get; set; }

        /// <summary>
        /// 角度单位。
        /// </summary>
        [Display(Name = "角度单位")]
        public AngleUnit AngleUnit { get; set; }
    }
}
