//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 单个坐标参考系统，与复合相对。
    /// </summary>
    public class SingleCrs : CoordinateReferenceSystem
    {
        /// <summary>
        /// 构造
        /// </summary>
        public SingleCrs() { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="datum"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public SingleCrs(ICoordinateSystem coordinateSystem, IDatum datum, string name = null, string id = null)
            : base(coordinateSystem, datum, name, id)
        { 
        }
    }
}