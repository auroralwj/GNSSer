using System;
using Geo.Times;

namespace Gnsser.Times
{
    /// <summary>
    /// GNSS Time 接口
    /// </summary>
    public interface IGnssTime
    {
        /// <summary>
        /// 日历
        /// </summary>
        Calendar Calendar { get; }
        /// <summary>
        /// GNSS 系统
        /// </summary>
        GnssSystem GnssSystem { get; }
        /// <summary>
        /// 周内秒
        /// </summary>
        ISecond SecondsOfWeek { get; }
        /// <summary>
        /// GNSS 周
        /// </summary>
        int GnssWeek { get; }
    }
}
