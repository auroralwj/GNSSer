//2014.05.22, Cui Yang, created
//2014.07.22, czs, Refactoring.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;
using Gnsser.Data.Rinex;

namespace Gnsser
{
    /// <summary>
    /// Phase Center Variation（PCV）天线相位中心变化类型。
    /// PCV TYPE / REFANT 。
    /// 主要具有绝对和相对两种。
    /// </summary>
    public enum PcvType
    {
        /// <summary>
        /// Unknown PCV
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Absolute PCV
        /// </summary>
        Absolute = 1, 
        /// <summary>
        ///  Relative PCV
        /// </summary>
        Relative, 
    }

}