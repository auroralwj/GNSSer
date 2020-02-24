using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// 测站，及其属性。
    /// </summary>
    public class SinexSite
    {
        /// <summary>
        /// 名称，代码。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public Time DateStart { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public Time DateEnd { get; set; }
        /// <summary>
        /// 初始估值。
        /// </summary>
        public GeoCoord ApproxGeoCoord { get; set; }
        /// <summary>
        /// 偏心坐标
        /// </summary>
        public Geo.Coordinates.HEN Eccentricity { get; set; }
        /// <summary>
        /// 先验大地坐标
        /// </summary>
        public GeoCoord AprioriGeoCoord { get; set; }
        /// <summary>
        /// 先验空间直角坐标
        /// </summary>
        public XYZ AprioriXYZ { get; set; }
        /// <summary>
        /// 先验空间直角坐标均方差（标准差）
        /// </summary>
        public XYZ AprioriXyzStdDev { get; set; }

        /// <summary>
        /// 估值大地坐标
        /// </summary>
        public GeoCoord EstimateGeoCoord { get; set; }
        /// <summary>
        /// 估值空间直角坐标。
        /// </summary>
        public XYZ EstimateXYZ { get; set; }
        /// <summary>
        /// 估值空间直角坐标均方差（？标准差）
        /// </summary>
        public XYZ EstimateXyzStdDev { get; set; }

        /// <summary>
        /// 接收机
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 天线
        /// </summary>
        public string Antenna { get; set; }

        
    }
}
