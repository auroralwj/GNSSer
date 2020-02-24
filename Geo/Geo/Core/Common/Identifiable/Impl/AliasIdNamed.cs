//2014.05.24，czs, created

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Common
{
    /// <summary>
    /// 具有名称，别称和ID。
    /// </summary>
    public class AliasIdNamed : IdNamed
    {
        /// <summary>
        /// 名称简称
        /// </summary>
        public string Alias { get; set; }
    }

}
