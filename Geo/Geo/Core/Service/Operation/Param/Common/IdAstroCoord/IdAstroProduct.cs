//2016.02.19, czs, create in xi'an hongqing, 天文坐标文件读取

using System;
using Geo.Common;
using Geo.Coordinates;
using Geo.IO;
using Geo;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// NamedAstroCoord 坐标文件读取
    /// </summary>
    public class IdAstroProduct : IdAstroCoord, IRowClass, IVectorStringId
    {
        public IdAstroProduct(string name, LonLat geoCoord) : base(name, geoCoord) { }


        public List<string> OrderedProperties
        {
            get { return new System.Collections.Generic.List<string>() {   }; }
        } 
        /// <summary>
        /// 定向点标识
        /// </summary>
        public string ToId { get; set; }
        /// <summary>
        /// 方位角
        /// </summary>
        public double Azimuth { get; set; }
    }
}
