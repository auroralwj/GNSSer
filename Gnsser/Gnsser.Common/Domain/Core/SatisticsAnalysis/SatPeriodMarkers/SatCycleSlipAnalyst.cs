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
    /// 卫星周跳提取处理器，提取到 SatPeriodInfoManager 中。
    /// </summary>
    public class SatCycleSlipAnalyst : SatAnalyst
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="SatelliteTypes">卫星类型</param>
        /// <param name="interval">采样间隔</param>
        public SatCycleSlipAnalyst( List<SatelliteType> SatelliteTypes, double interval = 30.0)
            : base(interval)
        {  
            this.SatelliteTypes = SatelliteTypes;
          //  this.obsBuilder = new BaseEpochInfoReviser(SatelliteTypes, null);
            //this.obsBuilder.InputingProcessor.AddProcessor(CycleSlipDetectorChainProcessor.Default());
        }
        /// <summary>
        /// 历元信息构建器
        /// </summary>
        EpochInfoReviseManager obsBuilder { get; set; } 

        /// <summary>
        /// 卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="obs">观测数据</param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation epochInfo)
        { 
            if (epochInfo == null) return false;

           // obsBuilder.Revise(ref epochInfo);

            foreach (var sat in epochInfo)
            {
                if (sat.IsUnstable)
                { SatSequentialPeriod.AddTimePeriod(sat.Prn, sat.Time.Value); }
            }
            return true;
        }
    }
 
}
