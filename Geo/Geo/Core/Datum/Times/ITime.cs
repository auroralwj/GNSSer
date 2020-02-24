//2015.04.24, czs, create in namu, 提取时间接口

using System;

namespace Geo.Times
{
    /// <summary>
    /// 时间接口
    /// </summary>
    public interface ITime 
    {
        /// <summary>
        /// 日期部分
        /// </summary>
        DateTime Date { get; }
        /// <summary>
        /// 系统时间
        /// </summary>
        DateTime DateTime { get; }
        /// <summary>
        /// 周几
        /// </summary>
        DayOfWeek DayOfWeek { get; }
        /// <summary>
        /// 年积日
        /// </summary>
        int DayOfYear { get; }
        /// <summary>
        /// GPS 周
        /// </summary>
        int GpsWeek { get; }
        /// <summary>
        /// 小时
        /// </summary>
        int Hour { get; }
        /// <summary>
        /// 毫秒
        /// </summary>
        double MilliSeconds { get; }
        /// <summary>
        /// 分
        /// </summary>
        int Minute { get; }
        /// <summary>
        /// 秒
        /// </summary>
        int Second { get; }
        /// <summary>
        /// 秒 [0, 60)
        /// </summary>
        double Seconds { get; }
        /// <summary>
        /// 日秒
        /// </summary>
        double SecondsOfDay { get; }
        /// <summary>
        /// 周秒
        /// </summary>
        double SecondsOfWeek { get; }
        /// <summary>
        /// 总共天
        /// </summary>
        double TotalDays { get; }
        /// <summary>
        /// 总共小时
        /// </summary>
        double TotalHours { get; }
        /// <summary>
        /// 总共分钟
        /// </summary>
        double TotalMinutes { get; }
        /// <summary>
        ///总周数
        /// </summary>
        double TotalWeeks { get; }
        /// <summary>
        /// 时间字符串
        /// </summary>
        /// <returns></returns>
        string ToTimeString();
        /// <summary>
        /// 周
        /// </summary>
        int Week { get; }
    }
}
