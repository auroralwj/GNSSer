//2016.02.19, czs, create in xi'an hongqing, 天文坐标文件读取
//2016.08.22, czs, create in xi'an hongqing, 天文坐标文件读取

using System;
using Geo.Common;
using Geo.Coordinates;
using Geo.IO;
using Geo;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// IdAstroCoord 坐标文件读取
    /// </summary>
    public class IdAstroCoord : StringIdValue<LonLat>, IRowClass
    {
        public IdAstroCoord(string name, LonLat geoCoord) : base(name, geoCoord) { }


        public List<string> OrderedProperties
        {
            get { return new System.Collections.Generic.List<string>() {   }; }
        }
        public List<ValueProperty> Properties { get; protected set; }
    }
}
