//2016.01.22, czs, create in hongqing, 多文件钟差组合服务类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Data.Rinex;

namespace Gnsser.Service
{
    /// <summary>
    /// 基于多文件的钟差服务类
    /// </summary> 
    public class MultiFileClockService : MultiSatProductService<AtomicClock>, IClockService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiFileClockService(string [] fileNames)
        {
            this.Name = "多钟差文件组合服务";

            ClockServices = new List<ClockService>();
            foreach (var item in fileNames)
            {
                var service = new ClockService(item);
                ClockServices.Add(service);
            }
            //排序
            ClockServices.Sort();

         }
        /// <summary>
        /// 服务列表
        /// </summary>
        List<ClockService> ClockServices { get; set; }

        public int ServiceCount { get{ return ClockServices.Count;} }

        /// <summary>
        /// 逐个判断读取，失败返回null
        /// </summary>
        /// <param name="nameOrPrn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public override AtomicClock Get(string nameOrPrn, Time time)
        {
            AtomicClock  clock = null;
            foreach (var item in ClockServices)
            {
                if (item.TimePeriod.Contains(time))
                {
                    clock = item.Get(nameOrPrn, time);
                    if (clock == null) continue;
                    else
                    {
                        return clock;
                    }
                }
            } 
            return clock;
        }

        public override BufferedTimePeriod TimePeriod
        {
            get {
                Time start = ClockServices[0].TimePeriod.Start;
                Time end = ClockServices[ServiceCount - 1].TimePeriod.End;

                BufferedTimePeriod peridod = new BufferedTimePeriod(start , end);
                return peridod;
            }
        } 

        public override AtomicClock Get(SatelliteNumber prn, Time time)
        {
            AtomicClock clock = null;
            foreach (var item in ClockServices)
            {
                if (item.TimePeriod.Contains(time))
                {
                    clock = item.Get(prn, time);
                    if (clock == null) continue;
                    else
                    {
                        return clock;
                    }
                }
            }
            return clock;
        }

        public override List<AtomicClock> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            List<AtomicClock> list = new List<AtomicClock>();
            foreach (var item in ClockServices)
            {
                list.AddRange(item.Gets(prn, timeStart, timeEnd));
            } 

            return list;
        }

        /// <summary>
        /// 打印输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ", " + this.TimePeriod.ToString();// + ", " + Geo.Utils.StringUtil.GetArrayString<SatelliteType>(this.SatelliteTypes.ToArray());
        }
    }
}
