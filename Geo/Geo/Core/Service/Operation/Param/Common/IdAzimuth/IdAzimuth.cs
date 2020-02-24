//2016.07.14, czs, create in xi'an hongqing, 天文坐标文件读取

using System;
using Geo.Common;
using Geo.Coordinates;
using Geo.IO;
using Geo;
using System.Collections.Generic;

namespace Geo.IO
{
    //2016.08.22, czs, create in 福建永安大湖, 指定了坐标系统的方位角
    /// <summary>
    /// 指定了坐标系统的方位角
    /// </summary>
    public class IdAzimuthWithCoordSys : IdAzimuth
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IdAzimuthWithCoordSys()
        {

        }

        /// <summary>
        /// 坐标系统
        /// </summary>
        public string CoordSystem { get; set; }
        /// <summary>
        /// 天文方位角
        /// </summary>
        public double AstroAzimuth { get; set; }
        /// <summary>
        /// 化归海水面改正
        /// </summary>
        public double CorrectionToTheSeaLevel { get; set; }
        /// <summary>
        /// 大地方位角
        /// </summary>
        public double GeoAzimuth { get; set; }

    }

    /// <summary>
    /// 方位角。需要指明大地方位角、天文访问角、磁方位角等。
    /// </summary>
    public class IdAzimuth : StringIdValue<double>, IRowClass, IVectorStringId
    {
        public IdAzimuth() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toId"></param>
        /// <param name="Azimuth"></param>
        public IdAzimuth(string id, string toId, double Azimuth) : base(id, Azimuth) { this.ToId = toId; }

        /// <summary>
        /// 属性的顺序。
        /// </summary>
        public List<string> OrderedProperties
        {
            get { return new List<string>() {   }; }
        }

        /// <summary>
        /// 属性。
        /// </summary>
        public List<ValueProperty> Properties { get; protected set; }

        /// <summary>
        /// 定向点标识
        /// </summary>
        public string ToId { get; set; }

        /// <summary>
        /// 方位角，值同 value。
        /// </summary>
        public double Azimuth { get { return Value; } set { Value = value; } }

        /// <summary>
        /// 误差，中误差，标准差，默认为 0。
        /// </summary>
        public double StdDev { get; set; }
    }
}
