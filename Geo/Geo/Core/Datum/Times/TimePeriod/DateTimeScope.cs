//2015.05.10, czs, create in namu, 时间范围，采用系统时间 DateTime 描述

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Times;

namespace Geo.Times
{
    /// <summary>
    /// 时间范围，采用系统时间 DateTime 描述
    /// </summary>
    public class DateTimeScope : Segment<DateTime, TimeSpan>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        public DateTimeScope( DateTime StartTime, DateTime EndTime ):base(StartTime, EndTime)
        { 
        } 
        /// <summary>
        /// 时间段
        /// </summary>
        public override TimeSpan Span { get { return End - Start; } }

        /// <summary>
        /// 中间数
        /// </summary>
        public override DateTime Median
        {
            get { return Start + TimeSpan.FromMinutes(Span.TotalMinutes / 2.0); }
        }
    }
}