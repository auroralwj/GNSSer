//2018.06.04, czs, create in HMX, 新建多历元卫星数据模型

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
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
    ///多历元卫星数据模型
    /// </summary>
    public class PeriodSatellite :  BaseList<EpochSatellite> 
    {
        #region 构造函数和初始化函数
        /// <summary>
        /// 构造函数，初始化基本变量。
        /// </summary> 
        public PeriodSatellite( int AssignedEpochCount=2)
        {

            this.Enabled = true; 
            this.Name = "单星多历元数据";
            this.AssignedEpochCount = AssignedEpochCount;
        }
        #endregion

        #region 属性，检索器 
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
         

        #endregion
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TimePeriod + "" + String.Format("{0}", new EnumerableFormatProvider(), First.Keys); 
        }

        /// <summary>
        /// 接收时间
        /// </summary>
        public Time ReceiverTime
        {
            get { return Last.ReceiverTime; }
        }
        /// <summary>
        /// 是否有周跳。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public bool HasCycleSlip(SatelliteNumber prn)
        {
            foreach (var item in this)
            {
                if (item.IsUnstable)
                return true;
            }
            return false;
        }
         

        internal Vector GetAdjustVector(SatObsDataType ObsDataType, bool enabledSatOnly  = true)
        {
            Vector list = new Vector(this.Count);
             foreach (var item in this)
            {
                list.Add(item.GetAdjustValue(ObsDataType), item.ReceiverTime.ToShortTimeString());
            } 

            return list;
        }
    }
}
