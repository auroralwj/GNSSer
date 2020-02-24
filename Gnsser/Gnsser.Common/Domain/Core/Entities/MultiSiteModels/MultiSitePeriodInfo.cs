//2016.05.03, czs, create in hongqing, 新建多历元多测站数据模型

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
    ///多历元多测站数据模型
    /// </summary>
    public class MultiSitePeriodInfo : BaseList<MultiSiteEpochInfo>, IToTabRow, IEnabled, ISiteSatObsInfo
    {
        #region 构造函数和初始化函数
        /// <summary>
        /// 构造函数，初始化基本变量。
        /// </summary> 
        public MultiSitePeriodInfo()
        {
            this.Enabled = true;
        }
        #endregion

        #region 属性，检索器
        /// <summary>
        /// 记录已经移除的卫星编号
        /// </summary>
        public List<SatelliteNumber> RemovedPrns {  
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
        /// 最新一个历元的时间。
        /// </summary>
        public Time ReceiverTime { get { return Last.ReceiverTime; } }

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
        /// 共同的卫星
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
                foreach (var item in this)
                {
                    if (list.Count == 0) { list = item.EnabledPrns; continue; }
                    list = SatelliteNumberUtils.GetCommons(item.EnabledPrns, list);
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
        /// <summary>
        /// 所有出现过的卫星编号
        /// </summary>
        public List<SatelliteNumber> TotalPrns
        {
            get
            {
                List<SatelliteNumber> totalPrns = new List<SatelliteNumber>();
                foreach (var item in this)
                {
                    foreach (var prn in item.TotalPrns)
                    {
                        if (!totalPrns.Contains(prn))
                        {
                            totalPrns.Add(prn);
                        }
                    }
                }
                return totalPrns;
            }
        }

        /// <summary>
        /// 禁用其它卫星
        /// </summary>
        /// <param name="prns"></param>
        public void DisableOthers(List<SatelliteNumber> prns) { foreach (var info in this) { info.DisableOthers(prns); } }
        /// <summary>
        /// 禁用指定卫星
        /// </summary>
        /// <param name="differs"></param>
        public void Disable(List<SatelliteNumber> differs) { foreach (var info in this) { info.Disable(differs); } }
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="prns"></param>
        public void Enable(List<SatelliteNumber> prns) { foreach (var info in this) { info.Enable(prns); } }
        #endregion


        /// <summary>
        /// 可用卫星系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes => (from prn in EnabledPrns select prn.SatelliteType).Distinct().ToList();
        #region 方法

        public void RemoveUnStableMarkers()
        {
            foreach (var item in this)
            {
                item.RemoveUnStableMarkers();
            }
        }

        /// <summary>
        /// 指定卫星是否具有周跳，任一历元有，则有。
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <returns></returns>
        public bool HasCycleSlip(SatelliteNumber prn) {

            foreach (var item in this)
            {
                if (item.HasCycleSlip(prn))
                {
                    return true;
                }
            }
            return false;
        }

    
        #endregion

        #region  数据向量获取

        /// <summary>
        /// 获取指定测站，指定卫星的多历元数据向量。
        /// 名称为时间。
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="dataType"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Vector GetVector(string siteName, SatelliteNumber prn, SatObsDataType dataType, double defaultValue = Double.NaN)
        {
            Vector vector = new Vector(this.EpochCount);
            int i = 0;
            string name = "";
            foreach (var epochSites in this)
            {
                double val = defaultValue;
                name = epochSites.ReceiverTime.ToShortTimeString();
                foreach (var epochSite in epochSites)
                {
                    if (epochSite.Name == siteName)
                    {
                        if (epochSite.Contains(prn))
                        {
                            val = epochSite[prn][dataType].Value;
                        }
                    } 
                }
                vector[i] = val;
                vector.ParamNames[i] = name;
            }
            return vector;
        }
        /// <summary>
        /// 返回测站卫星列表。
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public List<EpochSatellite> GetEpochSatellites(string siteName, SatelliteNumber prn)
        {
            List<EpochSatellite> vector = new List<EpochSatellite>(this.EpochCount); 
            foreach (var epochSites in this)
            { 
                foreach (var epochSite in epochSites)
                {
                    if (epochSite.Name == siteName)
                    {
                        if (epochSite.Contains(prn))
                        {
                            vector.Add(epochSite[prn]);
                        }
                    }
                } 
            }
            return vector;
        }

        #endregion

        #region 用于平差的方法
        /// <summary>
        /// 单差的观测值平差向量，观测值减去估计值。参数顺序为先坐标参数x y z，再钟差，最后模糊度。
        /// </summary>
        /// <param name="dataType">观测类型</param>
        /// <param name="approxType">近似类型</param>
        /// <param name="basePrn">基准卫星</param>
        /// <returns></returns>
        public Vector GetSingleDifferResidualVector(SatObsDataType dataType, SatApproxDataType approxType, SatelliteNumber basePrn)
        {
            var obsCount = this.EpochCount * this.EnabledSatCount;
            Vector vector = new Vector(obsCount);
            int i = 0;
            foreach (var epochBaseLineInfo in this)
            {
                Vector epochDiffer = epochBaseLineInfo.GetTwoSiteSingleDifferResidualVector(dataType, approxType, this.EnabledPrns, basePrn);
                vector.SetSubVector(epochDiffer, i * epochDiffer.Count);
                i++;
            }
            return vector;
        }

        /// <summary>
        /// 双差平差残差
        /// </summary>
        /// <param name="dataType">观测类型</param>
        /// <param name="approxType">近似类型</param>
        /// <param name="basePrn">基准卫星</param>
        /// <returns></returns>
        public Vector GetDoubleDifferResidualVector(SatObsDataType dataType, SatApproxDataType approxType, SatelliteNumber basePrn)
        {
            var obsCount = this.EpochCount * (this.EnabledSatCount - 1);
            Vector vector = new Vector(obsCount);
            int i = 0;
            foreach (var item in this)
            {
                Vector epochDiffer = item.GetTwoSiteDoubleDifferResidualVector(dataType, approxType,this.EnabledPrns, basePrn);
                vector.SetSubVector(epochDiffer, i * epochDiffer.Count);
                i++;
            }
            return vector;
        }      

        #region IO
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("历元×卫星:" + EpochCount + "×" + EnabledSatCount + " ");
            sb.Append(String.Format(new EnumerableFormatProvider(), "({0:,})", this.EnabledPrns));
            sb.Append(", " + this.TimePeriod + ", ");
            sb.Append("Epoch:");
            foreach (var item in Epoches)
            {
                sb.Append(" " + item.ToShortTimeString());
            }

            sb.Append("PRN:");
            foreach (var item in EnabledPrns)
            {
                sb.Append(" " + item);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Epoch");
            sb.Append("\t");
            sb.Append("Epochs");
            sb.Append("\t");
            sb.Append("PRN");
            sb.Append("\t");
            sb.Append("PRNs");
            return sb.ToString();
        }
        /// <summary>
        /// 获取表格值
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(EpochCount);
            sb.Append("\t");
            foreach (var item in Epoches)
            {
                sb.Append(" " + item.ToShortTimeString());
            }

            sb.Append("\t");
            sb.Append(EnabledSatCount);

            sb.Append("\t");
            foreach (var item in EnabledPrns)
            {
                sb.Append(" " + item);
            }
            return sb.ToString();
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
        #endregion

        #endregion
    }
}