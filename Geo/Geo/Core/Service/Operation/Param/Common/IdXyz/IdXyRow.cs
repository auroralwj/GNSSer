//2015.09.27, czs, create in xi'an hongqing, XYZ 坐标文件读取
//2016.02.10, czs, create in hongqing, XY 坐标文件读取

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
    public class IdXyRow : StrIdValueRow< XY>
    {
        public IdXyRow(string name, XY xyz)
            : base(name, xyz)
        {
            this.OrderedProperties = new System.Collections.Generic.List<string>() { "Name", "X", "Y" };
        }
    }
}
