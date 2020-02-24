//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标参考系统，通常由坐标系统和坐标基准构成。
    /// </summary>
    public interface ICoordinateReferenceSystem
    {        
        /// <summary>
        /// 坐标系统，计量表达。
        /// </summary>
        ICoordinateSystem CoordinateSystem { get; }

        /// <summary>
        /// 基准，计量参照。
        /// </summary>
        IDatum Datum { get; set; }
    }
}