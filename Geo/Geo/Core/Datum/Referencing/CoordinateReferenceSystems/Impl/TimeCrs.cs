//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 时间参照系统，记录一维时间的参考系。
    /// </summary>
    public class TimeCrs : SingleCrs
    {
        public TimeCrs() { }

        /// <summary>
        /// 基准。
        /// </summary>
        public new TimeDatum Datum { get; set; }
    }
}