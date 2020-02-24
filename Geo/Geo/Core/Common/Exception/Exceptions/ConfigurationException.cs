//2015.07.02, czs, create in Tianjing, 配置文件错误

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Exceptions
{
    /// <summary>
    /// 配置错误
    /// </summary>
    public class ConfigurationException : GeoException
    {
        public ConfigurationException(string msg) : base(msg) { }
    }
}
