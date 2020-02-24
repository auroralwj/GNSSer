//2015.10.18, czs, create in 彭州到成都快铁C6186，断续小数据探测分析器

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
    /// 断续小数据探测分析器。
    /// </summary>
    public class SmallObsPartDetectAnalyst : SatAnalyst
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="interval">采样间隔</param>
        public SmallObsPartDetectAnalyst(double interval = 30.0)
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
