using System;
//2014.05.24, czs, created 

using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 大地坐标系，有经纬度和大地高组成的三维坐标系统。
    /// 大地测量中以参考椭球面（水平面）为基准面的坐标。
    /// </summary>
    public class GeodeticCs : EllipsoidalCs
    {
        /// <summary>
        /// 参考椭球。
        /// </summary>
        public Ellipsoid Ellipsoid { get; set; }

    }
}
