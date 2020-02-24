using System;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 单位。如度，分，秒，弧度，角度，米，千米等。
    /// </summary>
    public interface IUnit
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 尺度转换因子。
        /// </summary>
        Double ConversionFactor { get; set; }
    }
}
