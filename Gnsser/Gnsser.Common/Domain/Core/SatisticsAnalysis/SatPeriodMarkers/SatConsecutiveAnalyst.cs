//2015.10.18, czs, create in pengzhou，简单时段统计器

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Utils;
using Geo.Common;
using Gnsser.Checkers;
using Gnsser.Filter;
using Geo.Times;


namespace Gnsser
{
    

    /// <summary>
    /// 卫星连续性分析器。
    /// </summary>
    public class SatConsecutiveAnalyst : SatAnalyst
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="interval">采样间隔</param>
        public SatConsecutiveAnalyst(double interval = 30.0)
            : base(interval)
        { 
        }
         

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="obs">观测数据</param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obs)
        {
            foreach (var item in obs)
            {
                SatSequentialPeriod.AddTimePeriod(item.Prn, item.Time.Value);
            }
             
            return true;
        }
    }
}
