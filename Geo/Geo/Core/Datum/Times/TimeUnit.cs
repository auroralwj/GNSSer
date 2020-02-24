//2014.06.27,czs, edit, 进行了一些梳理，解决了高精度问题
//2016.01.24, czs, create in hongqing, 时间单位

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Times;

namespace Geo.Times
{
    /// <summary>
    /// 时间单位
    /// </summary>
    public enum TimeUnit
    {
        Year,
        Month,
        Day,
        Week,
        Date,
        DayOfYear,
        DayOfWeek,
        Hour,
        Minute,
        Second,
        MiniSecond,
        MicroSecond,
        NaroSecond,
    }
}
