
//2014.12.26, czs, create in namu shuangliao,  星历服务类型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Service;

namespace Gnsser.Service
{


    /// <summary>
    /// 星历服务类型
    /// </summary>
    public enum EphemerisServiceType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown,
        /// <summary>
        /// 精密星历，精度在厘米级别
        /// </summary>
        Precise,
        /// <summary>
        /// 导航星历，精度在 10米级别
        /// </summary>
        Navigation,
        /// <summary>
        /// 组合的服务。
        /// </summary>
        Composed
    }
     
}
