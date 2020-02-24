//2014.12.13, czs, create in namu, 枚举类型选择器


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 枚举类型选择器
    /// </summary>
    public interface IEnumSelector<TEnum>  
    {
        /// <summary>
        /// 当前所选
        /// </summary>
        TEnum CurrentdType { get; }

    }
}
