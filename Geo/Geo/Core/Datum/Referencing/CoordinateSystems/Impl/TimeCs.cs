//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    /// <summary>
    /// 测量时间的一维坐标系统。 
    /// </summary>
    public class TimeCs : CoordinateSystem
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TimeCs() : base() { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public TimeCs(IAxis axis,string name, string id) 
            : base( new List<IAxis>(){axis},name,id) {
            if (axis.Unit is TimeUnit)
                this.TimeUnit = (TimeUnit)axis.Unit;        
        }


        /// <summary>
        /// Gets the units used along the vertical axis.
        /// </summary>
        public TimeUnit TimeUnit { get; set; }
    }
}
