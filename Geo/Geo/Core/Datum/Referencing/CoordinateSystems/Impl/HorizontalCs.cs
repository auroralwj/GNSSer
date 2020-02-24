//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    /// <summary>
    /// 地球表面的二维的水平坐标系。经过投影后的平面坐标，也成为格网坐标（Grid Coordinate）
    /// </summary>
    public class HorizontalCs : CoordinateSystem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HorizontalCs() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="HorizontalDatum">平面基准</param>
        /// <param name="axes">坐标轴</param>
        /// <param name="name">坐标系统名称</param>
        /// <param name="id">坐标系统编号</param>
        public HorizontalCs(HorizontalDatum HorizontalDatum, List<IAxis> axes, string name = null, string id = null)
            :base(axes,  name,  id)
        {
            this.HorizontalDatum = HorizontalDatum;
        } 

        /// <summary>
        /// 水平基准。
        /// </summary>
       public HorizontalDatum HorizontalDatum { get; set; }
    }
 

}
