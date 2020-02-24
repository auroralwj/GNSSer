//2015.01.10, czs, create in namu shangliao, 卫星可见性提取处理器


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

namespace Gnsser
{    
    /// <summary>
    /// 卫星可见性提取处理器，提取到 SatPeriodInfoManager 中。
    /// </summary>
    public class SatVisibiltyAnalyst : SatAnalyst
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="SatelliteTypes">卫星类型</param>
        /// <param name="interval">采样间隔</param>
        public SatVisibiltyAnalyst(List<SatelliteType> SatelliteTypes, double interval = 30.0) : base(interval)
        { 
            this.SatelliteTypes = SatelliteTypes;
        } 

        /// <summary>
        /// 卫星类型
        /// </summary>
       public List<SatelliteType> SatelliteTypes { get; set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="obs">观测数据</param>
        /// <returns></returns>
       public override bool Revise(ref EpochInformation obs)
       {
           if (obs == null)
               return false;
           foreach (var prn in obs.EnabledPrns)
           {
               if (SatelliteTypes.Contains(prn.SatelliteType))
               {
                   this.SatSequentialPeriod .AddTimePeriod(prn, obs.ReceiverTime);
               }
           }
           return true;
       }
    } 
    
}
