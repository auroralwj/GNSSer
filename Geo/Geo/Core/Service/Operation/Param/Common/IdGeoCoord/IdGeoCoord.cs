//2015.09.27, czs, create in xi'an hongqing, XYZ 坐标文件读取

using System;
using Geo.Common;
using Geo.Coordinates;
using Geo.IO;
using Geo;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// XYZ 坐标文件读取
    /// </summary>
    public class IdGeoCoord : StrIdValueRow<GeoCoord>, IRowClass
    {
        public IdGeoCoord() { }
        public IdGeoCoord(string name, GeoCoord geoCoord) : base(name, geoCoord) { }


        public List<string> OrderedProperties
        {
            get { return new System.Collections.Generic.List<string>() {   }; }
        }
        public List<ValueProperty> Properties { get; protected set; }

    }
    /// <summary>
    /// 具有坐标系统的大地坐标
    /// </summary>
    public class IdGeoCoordWithCoordSys : IdGeoCoord
    { 

        public IdGeoCoordWithCoordSys()
        {

        }

        public IdGeoCoordWithCoordSys(string name, GeoCoord coord, string coordSys)
            :base(name, coord)
        { 
            this.CoordSystem = coordSys;
        }
        /// <summary>
        /// 坐标系统
        /// </summary>
        public string CoordSystem { get; set; }
    }
}
