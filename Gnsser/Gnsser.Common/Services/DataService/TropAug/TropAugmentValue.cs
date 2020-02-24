//2017.10, lly, create in zz, 对流层服务
//2017.11.10, czs, edit in hongqing, 服务重构合并

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data;

namespace Gnsser
{
    /// <summary>
    /// 增强值
    /// </summary>
    public class TropAugmentValue
    {
        public TropAugmentValue(Time time, double zwd)
        {
             this.time = time;
             //this.sitaname = sitename;
             this.zwd = zwd;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public Time time;

        /// <summary>
        /// 对流层天顶湿延迟
        /// </summary>
        public double  zwd;
    }


}
