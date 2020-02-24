//2018.08.31, czs, create in HMX, GPS 窄巷服务器
//2018.09.12, czs, edit in hmx, 宽巷窄巷格式定义

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Times;
using Geo;
using Geo.IO;
using System.Collections;

namespace Gnsser.Service
{
    /// <summary>
    /// 宽窄项FCB文件，相位未校准延迟的小数部分(Fraction Code Bias of Uncalibrated phase delay)
    /// </summary>

    public class FcbOfUpdFile : IEnumerable<FcbOfUpd> //: BaseDictionary<string, FcbOfUpd>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FcbOfUpdFile()
        {
            WideLanes = new BaseDictionary<Time, FcbOfUpd>();
            NarrowLanes = new BaseDictionary<Time, FcbOfUpd>();
            Name = "FcbOfUpdFile";
            NarrowSpanSecond = 15 * 60;
        }
        /// <summary>
        /// 支持GPS卫星数量
        /// </summary>
        public const int TotalGpsSatCount = 32;
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = Name + ", Count:" + Count + ", W:" + WCount + ",N:" + NCount;
             if(Count > 0)
            {
                str += ", TimePeriod:" + TimePeriod;
            }
            return str;
        }
        /// <summary>
        /// 支持时段
        /// </summary>
        public TimePeriod TimePeriod
        {
            get
            {
                Time start = Time.MaxValue;
                Time end = Time.MinValue;
                if (WCount > 0)
                {
                    var times = WideLanes.Keys;
                    times.Sort();
                    start = times[0];
                    end = times[WCount - 1];
                }
                if (NCount > 0)
                {
                    var times = NarrowLanes.Keys;
                    times.Sort();
                    start =Time.Min( times[0],  start );
                    end = Time.Max(times[NCount - 1], start);
                }
                return new TimePeriod(start, end);
            }
        }
        /// <summary>
        /// 窄巷认为小于此为相同。
        /// </summary>
        public double NarrowSpanSecond =  186400;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 宽巷集合
        /// </summary>
        public BaseDictionary<Time, FcbOfUpd> WideLanes { get; set; }
        /// <summary>
        /// 窄巷集合
        /// </summary>
        public BaseDictionary<Time, FcbOfUpd> NarrowLanes { get; set; }
        /// <summary>
        /// 总数。含宽和窄巷。
        /// </summary>
        public int Count { get { return WCount + NCount; } }
        /// <summary>
        /// 总数。宽巷。
        /// </summary>
        public int WCount { get { return WideLanes.Count  ; } }
        /// <summary>
        /// 总数。窄巷。
        /// </summary>
        public int NCount { get { return   NarrowLanes.Count; } }

        /// <summary>
        /// 转换基准
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public FcbOfUpdFile ToFile(SatelliteNumber basePrn)
        {
            FcbOfUpdFile result = new FcbOfUpdFile();
            foreach (var item in this)
            {
                var toVal = item.ConvertTo(basePrn);
                if(toVal == null) { continue; }
                result.Add(toVal);
            }
            return result;
        }

        public static List<string> BuildTitles()
        {
            var PropertieNames = new List<string>()
            {
               "Epoch", "WnMarker","BasePrn", "Count", 
               //"G01", "G02",  "G03", "G04",  "G05", "G06",  "G07", "G08",  "G09",
               //"G10",  "G11", "G12",  "G13", "G14",  "G15", "G16",  "G17", "G18",  "G19",
               //"G20",  "G21", "G22",  "G23", "G24",  "G25", "G26",  "G27", "G28",  "G29",
               //"G30",  "G31", "G32",
            };

            for (int i = 1; i <= FcbOfUpdFile.TotalGpsSatCount; i++)
            {
                string prn = new SatelliteNumber(i, SatelliteType.G).ToString();
                PropertieNames.Add(prn);
            }
            //for (int i = 1; i <= FcbOfUpdFile.TotalGpsSatCount; i++)
            //{
            //    string prn = new SatelliteNumber(i, SatelliteType.G).ToString();
            //    PropertieNames.Add(prn + "_RMS");
            //}

            return PropertieNames;
        }
        /// <summary>
        /// BSD
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral GetBsdOfWideLane(SatelliteNumber prn, SatelliteNumber basePrn, Time time)
        {
            var fcb = this.GetWideLane(time);
            return fcb.GetBsdValue(prn, basePrn);
        }
        /// <summary>
        /// BSD
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral GetBsdOfNarrowLane(SatelliteNumber prn, SatelliteNumber basePrn, Time time)
        {
            var fcb = this.GetNarrowLane(time);
            return fcb.GetBsdValue(prn, basePrn);
        }
        /// <summary>
        /// 窄巷查找最近的一个
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public FcbOfUpd GetNarrowLane(Time time)
        {
            if (NarrowLanes.Count == 0) { return null; }

            if (NarrowLanes.Contains(time))
            {
                return NarrowLanes[time];
            }

            var near = TimeUtil.GetNearst(this.NarrowLanes.Keys, time);
            if (near == null) { return null; }
            //return NarrowLanes[near.Date];

            var nearest = near[0];
            //如果超过一定的范围，则返回null。

            if (Math.Abs(nearest - time) > NarrowSpanSecond)
            {
                return null;
            }

            if (NarrowLanes.Contains(nearest))
            {
                return NarrowLanes[nearest];
            }
            return null;
        }

        /// <summary>
        /// 宽巷一日发布一个
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public FcbOfUpd GetWideLane(Time time)
        {
            if (WideLanes.Count == 0) { return null; }

            if (WideLanes.Contains(time))
            {
                return WideLanes[time];
            }

            if (WideLanes.Contains(time.Date))
            {
                return WideLanes[time.Date];
            }
            //查找前一天或后一天的
            time = time.Date + TimeSpan.FromDays(1);
            if (WideLanes.Contains(time.Date))
            {
                return WideLanes[time.Date];
            }
            time = time.Date + TimeSpan.FromDays(-2);
            if (WideLanes.Contains(time.Date))
            {
                return WideLanes[time.Date];
            }
            return null;
        }

        /// <summary>
        /// 添加一个。
        /// </summary>
        /// <param name="val"></param>
        public void Add(FcbOfUpd val)
        {
            if (val.IsWideOrNarrowLane)
            {
                WideLanes[val.Epoch.Date] = val;
            }
            else
            {
                NarrowLanes[val.Epoch] = val;
            }
        }

        public IEnumerator<FcbOfUpd> GetEnumerator()
        {
            List<FcbOfUpd> list = new List<FcbOfUpd>(WideLanes);
            list.AddRange(NarrowLanes);
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// 返回第一个匹配上的。先宽巷，再窄巷。
        /// </summary>
        /// <param name="epoch"></param>
        public FcbOfUpd Get(Time epoch)
        {
            var val1 = GetWideLane(epoch);
            if (val1 == null)
            {
                return GetNarrowLane(epoch);
            }
            return val1;
        }
    }

}