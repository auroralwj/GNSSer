//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    
    /// <summary>
    /// 本地坐标系统，其基准比较随意。
    /// </summary>
    /// <remarks>In general, a local coordinate system cannot be related to other coordinate 
    /// systems. However, if two objects supporting this interface have the same dimension, 
    /// axes, units and datum then client code is permitted to assume that the two coordinate
    /// systems are identical. This allows several datasets from a common source (e.g. a CAD
    /// system) to be overlaid. In addition, some implementations of the Coordinate 
    /// Transformation (CT) package may have a mechanism for correlating local datums. (E.g. 
    /// from a database of transformations, which is created and maintained from real-world 
    /// measurements.)
    /// </remarks>
    public class LocalCs : CoordinateSystem
    {
         /// <summary>
        /// 构造函数
        /// </summary>
        public LocalCs() :base() {  }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LocalDatum">基准</param>
        /// <param name="axes">坐标轴</param>
        /// <param name="name">坐标系统名称</param>
        /// <param name="id">坐标系统编号</param>
        /// <param name="abbrev">坐标系统简称</param>
        public LocalCs(LocalDatum LocalDatum, List<IAxis> axes, string name = null, string id = null)
            :base(axes,  name,  id)
        {
            this.LocalDatum = LocalDatum;
        } 
        /// <summary>
        ///  本地基准
        /// </summary>
        LocalDatum LocalDatum { get; set; }
    }

}
