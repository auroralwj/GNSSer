//2018.03.16, czs, create in hmx, 添加结果类型


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 添加结果类型
    /// </summary>
    public enum AddResultType
    {
        /// <summary>
        /// 已添加
        /// </summary>
        Added,
        /// <summary>
        /// 跳过
        /// </summary>
        Skiped,
        /// <summary>
        /// 替换
        /// </summary>
        Replaced
    }
}
