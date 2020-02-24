//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 时间基准
    /// </summary>
    public class TimeDatum : Datum, Geo.Referencing.ITimeDatum
    {
        public TimeDatum(DateTime startTime, string name=null)
            :base(name)
        {
            this.StartTime = startTime;
        }

        /// <summary>
        /// 开始计时的起点
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}
