using System;
namespace Geo.Referencing
{
    /// <summary>
    /// 时间基准
    /// </summary>
    public interface ITimeDatum
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        DateTime StartTime { get; set; }
    }
}
