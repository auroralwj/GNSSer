//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
  
    /// <summary>
    /// 一个临时性的基准。
    /// </summary>
    public class TemporalDatum : Datum
    {
        /// <summary>
        /// 有效时间。
        /// </summary>
        public TimeSpan TimeSpan { get; set; }
    }
}
