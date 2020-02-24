//2015.05.03, czs, create in hongqing, 信息码

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 信息码，通常是错误码。
    /// </summary>
    public enum InfoCode
    {       
        /// <summary>
        /// 位置
        /// </summary>
        Unkown = 0,
        /// <summary>
        /// 数量不足
        /// </summary>
        ShortageOfCount,

    }

    /// <summary>
    /// 分类类型 信息接口
    /// </summary>
    public interface IInfoMessage
    {
        Dictionary<InfoCode, string> Messages { get; }

    }
}
