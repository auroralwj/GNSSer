using System;
namespace Geo.Referencing
{
    /// <summary>
    /// 大地基准
    /// </summary>
    public interface IGeodeticDatum : IDatum
    {
        /// <summary>
        /// 参考椭球
        /// </summary>
       Ellipsoid Ellipsoid { get; set; }
        /// <summary>
        /// 首子午线。
        /// </summary>
       PrimeMeridian PrimeMeridian { get; set; }
        /// <summary>
        /// 向WGS84转换的七参数。
        /// </summary>
       BursaTransParams TransParamsToWgs84 { get; set; }
    }
}
