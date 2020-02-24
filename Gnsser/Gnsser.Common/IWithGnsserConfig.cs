//2015.10, czs, create in xi'an hongqing, 是否具有全局配置参数 GnsserConfig

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;
using Gnsser;

namespace Geo
{
    /// <summary>
    ///  是否具有全局配置参数 GnsserConfig
    /// </summary>
    public interface IWithGnsserConfig
    {
        /// <summary>
        /// 配置属性
        /// </summary>
        GnsserConfig GnsserConfig { get; set; }
    }
}