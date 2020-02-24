//2016.05.03, czs, create in hongqing, 新建多历元测站数据模型

using System;
using System.Text;
using System.Linq;
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
using Gnsser.Filter;
using Gnsser.Checkers; 
using Geo.Times;
using Geo.IO;

namespace Gnsser.Domain
{ 
    /// <summary>
    ///多历元测站数据模型,数据不应该更改 EpochInformation 本身。
    /// </summary>
    public class PeriodInformation :  BaseList<EpochInformation>, IToTabRow,  ISiteSatObsInfo
    {
        #region 构造函数和初始化函数
        /// <summary>
        /// 构造函数，初始化基本变量。
        /// </summary> 
        public PeriodInformation(bool IsRequireSameSats, int AssignedEpochCount=2)
        {

            this.Enabled = true; 
            this.Name = "单站多历元数据";
            this.IsRequireSameSats = IsRequireSameSats;
            this.AssignedEpochCount = AssignedEpochCount;
        }
        #endregion

        #region 属性，检索器
        /// <summary>
        /// 记录已经移除的卫星编号
        /// </summary>
        public List<SatelliteNumber> RemovedPrns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                foreach (var epochInfo in this)
                {
                    if (list.Count == 0) { list = epochInfo.RemovedPrns; continue; }

                    list.AddRange(epochInfo.RemovedPrns);
                }
                list = list.Distinct().ToList();
                list.Sort();
                return list;
            }
        }
        /// <summary>
        /// 指定的历元数量
        /// </summary>
        public int AssignedEpochCount { get; set; }
        /// <summary>
        /// 是否已经满了。
        /// </summary>
        public bool IsFull { get { return this.AssignedEpochCount >= this.Count; } }
        /// <summary>
        /// 测站信心
        /// </summary>
        public ISiteInfo SiteInfo { get { return this.First.SiteInfo; } }
        /// <summary>
        /// 是否需要相同的卫星
        /// </summary>
        public bool IsRequireSameSats { get; set; }
        /// <summary>
        /// 指示是否满足计算条件。
        /// </summary>
        public bool Enabled { get; set; }  

        /// <summary>
        /// 历元数量,743333
        /// </summary>
        public int EpochCount { get { return Data.Count; } }
        /// <summary>
        /// 时段
        /// </summary>
        public TimePeriod TimePeriod { get { return new TimePeriod(First.ReceiverTime, Last.ReceiverTime); } }

        /// <summary>
        /// 历元（时间）列表
        /// </summary>
        public List<Time> Epoches
        {
            get
            {
                List<Time> epochs = new List<Time>();
                foreach (var item in Data)
                {
                    epochs.Add(item.ReceiverTime);
                }
                return epochs;
            }
        } 

         /// <summary>
        /// 可以计算的卫星数量
        /// </summary>
        public int EnabledSatCount { get { return EnabledPrns.Count; } }

        /// <summary>
        /// 参与计算的卫星编号列表
        /// </summary>
        public List<SatelliteNumber> EnabledPrns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                int i = -1;
                foreach (var item in this)
                {
                    if (item.EnabledSatCount == 0 && IsRequireSameSats) {   return new List<SatelliteNumber>(); }

                    i++;
                    if (i == 0 && list.Count == 0) { list.AddRange(item.EnabledPrns); continue; }

                    if (IsRequireSameSats)
                    {
                        list = SatelliteNumberUtils.GetCommons(item.EnabledPrns, list);
                    }
                    else
                    {
                        list.AddRange(SatelliteNumberUtils.GetNews(item.EnabledPrns, list));
                    }
                }
                return list;
            }
        }
        /// <summary>
        /// 不稳定的卫星，通常为具有周跳的卫星。
        /// </summary>
        public List<SatelliteNumber> UnstablePrns
        {
            get
            {
                List<SatelliteNumber> unstablePrns = new List<SatelliteNumber>();
                foreach (var item in this)
                {
                    foreach (var prn in item.UnstablePrns)
                    {
                        if (!unstablePrns.Contains(prn))
                        {
                            unstablePrns.Add(prn);
                        }
                    }
                }
                return unstablePrns;
            }
        }

        #endregion

        /// <summary>
        /// 可用卫星系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes => (from prn in EnabledPrns select prn.SatelliteType).Distinct().ToList();
        public override string ToString()
        {
            return TimePeriod + "" + String.Format("{0}", new EnumerableFormatProvider(), First.Keys); 
        }


        public string GetTabTitles()
        {
            throw new NotImplementedException();
        }

        public string GetTabValues()
        {
            throw new NotImplementedException();
        }


        public Time ReceiverTime
        {
            get { return Last.ReceiverTime; }
        }

        public bool HasCycleSlip(SatelliteNumber prn)
        {
            foreach (var item in this)
            {
                if (item.HasCycleSlip(prn)) ;
                return true;
            }
            return false;
        }
         

        internal List<Vector> GetAdjustVector(SatObsDataType ObsDataType, bool enabledSatOnly  = true)
        {
            List<Vector> list = new List<Vector>();
             foreach (var item in this)
            {
                list.Add(item.GetAdjustVector(ObsDataType, enabledSatOnly));
            } 

            return list;
        }

        public void RemoveUnStableMarkers()
        {
            foreach (var item in this)
            {
                item.RemoveUnStableMarkers();
            }
        }

        /// <summary>
        /// 具有星历的卫星列表
        /// </summary>
        /// <returns></returns>
        public List<EpochSatellite> GetEpochSatWithEphemeris()
        {
            List<EpochSatellite> sats = new List<EpochSatellite>();
            foreach (var item in this)
            {
                sats.AddRange(item.GetEpochSatWithEphemeris());
            }
            return sats;
        }
    }
}
