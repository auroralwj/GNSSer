//2015.11.06, czs & cy, create in 洪庆, 历元概略信息


using System;
using System.Linq;
using Geo.Coordinates;
using Gnsser.Data;
using System.Collections.Generic;
using Gnsser.Correction;
using Geo.Times;
using Gnsser.Times;
using Geo;
using Gnsser.Domain;

namespace Gnsser
{ 
    /// <summary>
    /// 历元概略信息，区别于 EpochInformation
    /// </summary>
    public interface IEpochInfo  : IOrderedProperty, ISiteSatObsInfo
    {
        /// <summary>
        /// 测站名称
        /// </summary>
        string SiteName { get; } 
        /// <summary>
        /// 卫星编号列表
        /// </summary>
        List<SatelliteNumber> TotalPrns { get;  } 
    }

    /// <summary>
    /// 历元概略信息，区别于 EpochInformation
    /// </summary>
    public class EpochInfo :OrderedProperty,   IEpochInfo 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EpochInfo() {
            OrderedProperties = new List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<EpochInfo>(m=>m.SiteName),
                Geo.Utils.ObjectUtil.GetPropertyName<EpochInfo>(m=>m.ReceiverTime),
                Geo.Utils.ObjectUtil.GetPropertyName<EpochInfo>(m=>m.TotalPrns),
            };
        }
        /// <summary>
        /// 测站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 观测时间
        /// </summary>
        public List<Time> Epoches => new List<Time>() { ReceiverTime };
        /// <summary>
        /// 观测时间
        /// </summary>
        public Time ReceiverTime { get; set; }
        /// <summary>
        /// 卫星编号列表
        /// </summary>
        public List<SatelliteNumber> TotalPrns { get; set; }
        /// <summary>
        /// 可用卫星编号
        /// </summary>
         public List<SatelliteNumber> EnabledPrns { get; set; }

         /// <summary>
         /// 记录已经移除的卫星编号
         /// </summary>
         public List<SatelliteNumber> RemovedPrns { get; set; }
        /// <summary>
        /// 可用卫星数量
        /// </summary>
         public int EnabledSatCount
         {
             get { return EnabledPrns.Count; }
         }


         public bool HasCycleSlip(SatelliteNumber prn)
         {
             throw new NotImplementedException();
         }


         public List<SatelliteNumber> UnstablePrns { get; set; }

         public string GetTabTitles()
         {
             throw new NotImplementedException();
         }

         public string GetTabValues()
         {
             throw new NotImplementedException();
         }

        public void RemoveUnStableMarkers()
        {
            throw new NotImplementedException();
        } 

        public void Dispose()
         {
             throw new NotImplementedException();
         }

        public List<EpochSatellite> GetEpochSatWithEphemeris()
        {
            throw new NotImplementedException();
        }

        public virtual string Name { get; set; }

        /// <summary>
        /// 可用卫星系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes => (from prn in EnabledPrns select prn.SatelliteType).Distinct().ToList();
    }
}
