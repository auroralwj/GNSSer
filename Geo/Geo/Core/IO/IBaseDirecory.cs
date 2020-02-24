//2015.10.28, czs, create in hongqing, 基础目录，如工程目录

using System;

namespace Geo
{

    /// <summary>
    /// 基础目录，如工程目录
    /// </summary>
    public interface IBaseDirecory
    {
        /// <summary>
        /// 基础目录，如工程目录，脚本目录。如果采用相对路径，且非当前程序默认路径，则必须赋值。
        /// </summary>
        string BaseDirectory { get; set; }
    }
}
