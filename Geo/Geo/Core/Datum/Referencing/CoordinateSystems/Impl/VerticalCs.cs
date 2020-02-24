//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    /// <summary>
    /// 适用于垂直测量的一维坐标系统，如高程参考系。
    /// </summary>
    public class VerticalCs : CoordinateSystem
    {
        /// <summary>
        /// Gets the vertical datum, which indicates the measurement method
        /// </summary>
       public  VerticalDatum VerticalDatum { get; set; }
        /// <summary>
        /// Gets the units used along the vertical axis.
        /// </summary>
       public LinearUnit VerticalUnit { get; set; }
    }
}
