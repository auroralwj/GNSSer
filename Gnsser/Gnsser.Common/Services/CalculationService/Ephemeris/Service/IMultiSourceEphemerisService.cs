//2014.12.25, czs, edit, 将 ICollectionEphemerisService 重构为 IMultiSourceEphemerisService，意为多数据源星历服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using System.IO;
using Gnsser.Times;
using Geo.Times;

namespace Gnsser.Service
{
    /// <summary>
    /// 包含多个数据源的星历数据源。
    /// 这里的多文件，是每一个时段，一个文件。
    /// </summary>
    public interface IMultiSourceEphemerisService :  IEphemerisService
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="satelliteType">卫星类型</param>
        /// <returns></returns>
        bool IsAvailable(Time time, SatelliteType satelliteType);

        /// <summary>
        /// 有效时间段。用于多文件。
        /// </summary>
        List<BufferedTimePeriod> GetTimePeriods(SatelliteType satelliteType);

        /// <summary>
        /// 数据源最大的间隔，超过这个间隔则不可以提供服务。
        /// </summary>
        TimeSpan MaxGap { get;  }

    }
}
