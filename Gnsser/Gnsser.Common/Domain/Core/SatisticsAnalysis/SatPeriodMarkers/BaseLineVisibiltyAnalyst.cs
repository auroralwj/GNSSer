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
    /// 基线卫星可见性提取处理器，提取到 SatPeriodInfoManager 中。
    /// </summary>
    public class BaseLineVisibiltyAnalyst : TwinsReviser<EpochInformation>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="SatelliteTypes">卫星类型</param>
        /// <param name="interval">采样间隔</param>
        public BaseLineVisibiltyAnalyst(List<SatelliteType> SatelliteTypes, double interval = 30.0)
        {
            this.SatPeriodMarker = new SatPeriodInfoManager(interval);
            this.SatelliteTypes = SatelliteTypes; 
        } 

        /// <summary>
        /// 提取结果
        /// </summary>
        public SatPeriodInfoManager SatPeriodMarker { get; set; }

        /// <summary>
        /// 卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="obsA">观测数据 A</param>
        /// <param name="obsB">观测数据 B</param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obsA, ref EpochInformation obsB)
        {
            foreach (var sat in obsA.EnabledPrns)
            {
                if (SatelliteTypes.Contains(sat.SatelliteType) && (obsB.EnabledPrns.Contains(sat)))
                {  
                    SatPeriodMarker.AddTimePeriod(sat, obsA.ReceiverTime);  
                }

            }
            return true;
        }
    } 
}
