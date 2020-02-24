//2015.12.20, czs, create in hongqing, 增加版本类型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 版本类型
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        ///  公开版，like Express
        /// </summary>
        Public,
        /// <summary>
        /// 开发版
        /// </summary>
        Development,
        /// <summary>
        /// 发行版
        /// </summary>
        Distribution,
        /// <summary>
        /// 测试版
        /// </summary>
        DistributionTesting,
        /// <summary>
        /// 基线网版本
        /// </summary>
        BaselineNet,

    }
}
