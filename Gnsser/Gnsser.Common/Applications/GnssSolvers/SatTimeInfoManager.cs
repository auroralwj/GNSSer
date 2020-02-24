//2016.03.18 czs, create in hongqing, 卫星出现管理器

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Times;

namespace Gnsser
{
    //2016.09.26 czs, create in hongqing, 多站卫星出现管理器
    /// <summary>
    /// 多站卫星出现管理器
    /// </summary>
    public class MultiSiteSatTimeInfoManager : BaseDictionary<string, SatTimeInfoManager>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultiSiteSatTimeInfoManager(double ObsInterval) { this.ObsInterval = ObsInterval; }

        /// <summary>
        /// 观测间隔
        /// </summary>
        public double ObsInterval { get; set; }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override SatTimeInfoManager Create(string key)
        {
            return new SatTimeInfoManager(ObsInterval);
        }
        /// <summary>
        /// 登记记录
        /// </summary>
        /// <param name="material"></param>
        public void Record(MultiSiteEpochInfo material)
        {
            foreach (var item in material)
            {
                this.GetOrCreate(item.Name).Record(item);
            }
        }
    }



    /// <summary>
    /// 卫星时间
    /// </summary>
    /// <param name="prn"></param>
    public delegate void SatelliteEventHandler(SatelliteNumber prn);

    /// <summary>
    /// 卫星出现管理器
    /// </summary>
    public class SatTimeInfoManager : BaseDictionary<SatelliteNumber, SatTimeInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SatTimeInfoManager(double ObsInterval) { this.ObsInterval = ObsInterval; }
        /// <summary>
        /// 观测间隔
        /// </summary>
        public double ObsInterval { get; set; }
        /// <summary>
        /// 可用卫星
        /// </summary>
        public List<SatelliteNumber> Prns { get { return Keys; } }

        /// <summary>
        /// 添加历元信息，统计之
        /// </summary>
        /// <param name="epochInfo"></param>
        public void Record(EpochInformation epochInfo)
        {
            foreach (var sat in epochInfo)
            {
                var prn = sat.Prn;
                if (!this.Contains(prn)) {
                    this[prn] = new SatTimeInfo(prn, sat.ReceiverTime, ObsInterval); 
                }

                var item = this[prn];
                item.Update( epochInfo.ReceiverTime);
            }
        }
    }

    /// <summary>
    /// 测站观测卫星统计器
    /// </summary>
    public class SatTimeInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="interval">观测采样率</param>
        /// <param name="prn">卫星编号</param>
        /// <param name="maxTime第一次出现的时间</param>
        public SatTimeInfo(SatelliteNumber prn, Time firstTime, double interval = 30)
        {
            if (interval == 0) interval = 30;
            this.ObsInterval = interval;
            this.MaxAllowedInterval = interval + 1;
            this.FirstTime = firstTime;
            this.Prn = prn;
            this.TimePeriod = new BufferedSuccessiveTimePeriod();
             
        }

        #region 属性
        /// <summary>
        /// 时段信息
        /// </summary>
        public BufferedSuccessiveTimePeriod TimePeriod { get; private set; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; private set; }
        /// <summary>
        /// 最后一次出现时间
        /// </summary>
        public Time LastTime { get; private set; }

        /// <summary>
        /// 一个连续阶段第一次出现的时间。
        /// </summary>
        public Time FirstTime { get; private set; }

        /// <summary>
        /// 总共出现的次数
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 观测采样间隔，单位：秒。
        /// </summary>
        public double ObsInterval { get; private set; }
        /// <summary>
        /// 与前一次观测的间隔，单位：秒。
        /// </summary>
        public double LastInterval { get; private set; }
        /// <summary>
        /// 间隔的最大阈值，如果超出这个阈值，则重新统计分段。
        /// 默认值是观测采样间隔+1.
        /// </summary>
        public double MaxAllowedInterval { get;   set; }
        #endregion

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="time"></param>
        public void Update(Time time)
        {
            this.LastInterval = time - LastTime;
            if (this.LastInterval > MaxAllowedInterval)//重新分段统计
            {
                this.FirstTime = time;
            }
            this.LastTime = time;

            this.AddTimePeriod(time);

            this.Count++;
        }

        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Prn + ", " + LastTime + ", " + ObsInterval + ", " + Count + ", " + TimePeriod;
        }

        /// <summary>
        /// 历元数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetEpochCount(Time time)
        {
            var segment = TimePeriod.GetSegment(time);
            if(segment == null){
                return 0;
            }
            var count = (int) (segment.Span / ObsInterval);

            return count;               
        }


        /// <summary>
        /// 添加观测时段.自动根据 间隔 Interval 生成区段
        /// </summary> 
        /// <param name="Time">时刻，自动根据 间隔 Interval 生成区段</param>
        public void AddTimePeriod(Time Time)
        {
            double halfInterval = this.ObsInterval / 2.0 + 0.001;//防止舍入误差
            var from = Time - halfInterval;
            var to = Time + halfInterval;

            this.TimePeriod.Add(new BufferedTimePeriod(from, to));
        }         
    }
}