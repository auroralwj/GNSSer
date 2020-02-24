using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser
{
    /// <summary>
    /// 计算类型
    /// </summary>
    public enum CaculateType
    { 
        /// <summary>
        /// 滤波，使用先验信息
        /// </summary>
        Filter=0,
        /// <summary>
        /// 独立计算
        /// </summary>
        Independent=1,

    }
}
