//2018.06.04, czs, create in HMX, 新建多历元测站单星数据模型构建器

using System;
using System.Text;
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
    /// 多历元测站单星数据模型构建器
    /// </summary>
    public class PeriodSatelliteBuilder : AbstractBuilder<PeriodSatellite>
    {
        Log log = new Log(typeof(PeriodSatelliteBuilder));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Prn">卫星编号</param>
        /// <param name="AssignedEpochCount">历元大小</param>
        /// <param name="isSmoothEpcohes">历元是否滑动向前,则会出现重叠</param>
        /// <param name="MaxTimeSpan"> 最大时间间隔，超出后，则重新构建</param>
        public PeriodSatelliteBuilder(SatelliteNumber Prn, int AssignedEpochCount = 5, bool isSmoothEpcohes = false, double MaxTimeSpan = 121)
            :this(AssignedEpochCount, isSmoothEpcohes, MaxTimeSpan)
        { 
            this.Prn = Prn; 
        } 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="AssignedEpochCount">历元大小</param>
        /// <param name="isSmoothEpcohes">历元是否滑动向前,则会出现重叠</param>
        /// <param name="MaxTimeSpan"> 最大时间间隔，超出后，则重新构建</param>
        public PeriodSatelliteBuilder(int AssignedEpochCount = 5, bool isSmoothEpcohes = false, double MaxTimeSpan = 121)
        {
            if(AssignedEpochCount < 2)
            {
                throw new Exception("朋友，至少两个历元才行！");
            }
            this.AssignedEpochCount = AssignedEpochCount;
            this.IsSmoothEpcohes = isSmoothEpcohes;
            this.MaxTimeSpan = MaxTimeSpan; 
            this.Data = new WindowData<Time, EpochSatellite>(AssignedEpochCount); 
        } 
        #region 属性，检索器 
        /// <summary>
        /// 当前卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 历元是否滑动向前,则会出现重叠
        /// </summary>
        public bool IsSmoothEpcohes { get; set; } 

        /// <summary>
        /// 指定的历元数量，如果超过数量，则剔除第一历元。
        /// </summary>
        public int AssignedEpochCount { get; set; }   
        /// <summary>
        /// 最大时间间隔，超出后，则重新构建
        /// </summary>
        public double MaxTimeSpan { get; set; }
        WindowData<Time, EpochSatellite>  Data { get; set; }
        /// <summary>
        /// 是否已满
        /// </summary>
        public bool IsFull { get => Data.IsFull; }

        /// <summary>
        /// 差分定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        #endregion


        /// <summary>
        /// 增加一个,若满，则返回true。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Add(EpochInformation info)
        {
            if (Data.Contains(info.ReceiverTime) || info.Contains(Prn)) { return false; }
            var sat = info[Prn];
            Data.Add(sat.ReceiverTime, sat); //add prevObj

            return Data.IsFull;
        }
        /// <summary>
        /// 增加一个,若满，则返回true。
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public bool Add(EpochSatellite sat)
        {
            if (Data.Contains(sat.ReceiverTime)) { return false; } 
            Data.Add(sat.ReceiverTime, sat); //add prevObj

            return Data.IsFull;
        }

        /// <summary>
        /// 生成。如果历元不足，则标记 Enabled 为 false；果失败，则返回 null.
        /// </summary>
        /// <returns></returns>
        public override PeriodSatellite Build()
        {
            if (!IsFull)
            {
                return null;
            }
            var orderedKeys = this.Data.OrderedKeys;

            var keys =  TimeUtil.GetOrderedNoJumpTimes(orderedKeys, MaxTimeSpan);
            this.Data.RemoveNotExist(keys);
            if (!IsFull)
            {
                return null;
            }
            PeriodSatellite infos = new PeriodSatellite(AssignedEpochCount);
            infos.Add(Data.OrderedValues); //首先添加数据	 

            infos.Enabled = infos.IsFull;

            if (IsSmoothEpcohes)//平滑，则减去第一个
            {
                this.Data.RemoveFirst();
            }
            else
            {
                this.Data.Clear();
            }
            return infos; 
        }
       
    }
}