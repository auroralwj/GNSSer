//2016.05.204, czs, create in hongqing, 多站多时段解算器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Service
{
     
    /// <summary>
    /// 多站多时段解算器。
    /// </summary>
    public abstract class MultiSitePeriodSolver : AbstractGnssSolver<BaseGnssResult, MultiSitePeriodInfo>
    {
        protected Log log = new Log(typeof(MultiSitePeriodSolver));
      
          /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="option"></param> 
        public MultiSitePeriodSolver(DataSourceContext context,GnssProcessOption option) 
            : base(context, option)
        {  
        }
         

    }
}