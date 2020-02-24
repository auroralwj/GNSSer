//2015.09.27, czs, create in xi'an hongqing, XYZ 坐标文件读取
//2016.02.10, czs, edit in hongqing, 重构

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
    public class IdXyzRow : StrIdValueRow<XYZ>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xyz"></param>
        public IdXyzRow(string name, XYZ xyz)
            : base(name, xyz)
        {
            this.OrderedProperties = new System.Collections.Generic.List<string>() { "Id", "X", "Y", "Z" };
        }
    }
}
