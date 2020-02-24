//2016.08.29, czs, create in hongqing, 循环控制类型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 循环控制类型
    /// </summary>
    public enum LoopControlType
    {
        /// <summary>
        /// 继续本次循环，继续做.默认选项。
        /// </summary>
        GoOn = 0,
        /// <summary>
        /// 忽略本次后继续循环，再做
        /// </summary>
        Continue,
        /// <summary>
        /// 跳出本次循环
        /// </summary>
        Break,
        /// <summary>
        /// 终止此方法
        /// </summary>
        Return
    }
}
