//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 基准。一组计量参照标准，如时间基准、位置基准、质量基准等。
    /// </summary>
    public interface IDatum 
    {
        /// <summary>
        /// 基准名称
        /// </summary>
        string Name { get; set; }       
        
    }
}
