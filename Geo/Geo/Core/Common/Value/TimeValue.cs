//2016.08.05, czs, create in 福建永安大湖, 具有时间属性的值

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;

namespace Geo
{
    /// <summary>
    /// 具有 Time 属性。
    /// </summary>
    public class TimeValue<TValue> : BaseValue<TValue>
    {
        /// <summary>
        /// Time 属性
        /// </summary>
        Time Time { get; set; }
    }
}
