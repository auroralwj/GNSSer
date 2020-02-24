//2018.10.28, czs, create in hmx, 定轨结果接口

using Geo;

namespace Gnsser
{
    /// <summary>
    /// 定轨结果
    /// </summary>
    public interface IOrbitResult
    {
        /// <summary>
        /// 星历结果
        /// </summary>
        BaseDictionary<SatelliteNumber, EphemerisResult> EphemerisResults { get; set; }
    }
}