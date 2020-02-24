using System;

namespace Geo.Times
{
    /// <summary>
    /// 日历接口。日历，将时间进行年月日分段表示。
    /// </summary>
    public interface ICalendar
    {
        /// <summary>
        /// 只有日期部分，没有小时以下部分。
        /// </summary>
        Calendar Date { get; }
        /// <summary>
        /// 系统时间，精度在100纳秒。
        /// </summary>
        DateTime DateTime { get; }
        /// <summary>
        /// 儒略日。
        /// </summary>
        decimal JulianDay { get; }
        /// <summary>
        /// 平儒略日
        /// </summary>
        decimal MJulianDay { get; }
        /// <summary>
        /// 年
        /// </summary>
        int Year { get; }
        /// <summary>
        /// 月 1-12
        /// </summary>
        int Month { get; }
        /// <summary>
        /// 月内日。1-31
        /// </summary>
        int Day { get; }
        /// <summary>
        /// 小时，0-23
        /// </summary>
        int Hour { get; }
        /// <summary>
        /// 分钟
        /// </summary>
        int Minute { get; }
        /// <summary>
        /// 秒
        /// </summary>
        int Second { get; }
        /// <summary>
        /// 毫秒。0-999
        /// </summary>
        decimal MilliSeconds { get; }
        /// <summary>
        /// 是否闰年
        /// </summary>
        bool IsLeapYear { get; }
        /// <summary>
        /// 周几
        /// </summary>
        DayOfWeek DayOfWeek { get; }
        /// <summary>
        /// 年纪日
        /// </summary>
        Int32 DayOfYear { get; }
        /// <summary>
        /// 秒小数
        /// </summary>
        decimal Seconds { get; }

        /// <summary>
        ///  获取此实例的当天的时间。   System.TimeSpan，它表示当天自午夜以来已经过时间的部分。
        /// </summary>
        CalendarSpan TimeOfDay { get; }
    }
}
