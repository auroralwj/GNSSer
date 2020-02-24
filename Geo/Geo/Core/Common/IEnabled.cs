//2015.01.09, czs, create in namu, 是否启用的标记接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 是否启用的标记接口
    /// </summary>
    public interface IEnabled
    {       
        /// <summary>
        /// 是否可用，是否启用。
        /// </summary>
        bool Enabled { get; set; }
    } 
}
