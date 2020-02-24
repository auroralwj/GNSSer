//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 由一些列坐标参考系统组成。
    /// </summary>
    public class CompoundCrs : CoordinateReferenceSystem
    {
        /// <summary>
        /// 参考系分量列表
        /// </summary>
        public List<CoordinateReferenceSystem> Components { get; set; }
    }
}