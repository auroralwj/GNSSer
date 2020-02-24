//2015.12.22, czs, create in hongqing, 建立 ITimeBasedService，基于指定时刻的服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Service;
using Geo.Times;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 获取服务。基于卫星和指定时刻的服务，如钟差、星历等。
    /// </summary>
    /// <typeparam name="TProduct"></typeparam>
    public interface ITimeBasedService<TProduct> : IService
    {
        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        TProduct Get( Time time);
    }
}
