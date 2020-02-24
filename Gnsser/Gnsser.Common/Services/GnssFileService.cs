//2014.09.20， czs, create, 数据源统一设计。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Common;

namespace Gnsser.Data
{ 

    /// <summary>
    /// GNSS 数据源服务。
    /// </summary>
    public abstract class GnssFileService<TProduct> : FileBasedService<TProduct>, IGnssFileService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public GnssFileService(string option) : base(option) { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public GnssFileService(FileOption option) : base(option) { }
    } 
}
