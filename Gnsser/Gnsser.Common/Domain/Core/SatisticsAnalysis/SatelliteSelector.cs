//2015.01.08, czs, create in namu shuangliao, 卫星时段信息管理器

using System;
using System.Collections.Generic;
using System.Text; 
using Gnsser.Times;
using Geo;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// 卫星时段信息管理器.通常作为一个观测文件的统计信息。
    /// </summary>
    public class SatelliteSelector
    {
        /// <summary>
        ///  卫星时段信息管理器，构造函数。
        /// </summary>
        public SatelliteSelector(SatPeriodInfoManager SatVisiblePeriodMarker, SatPeriodInfoManager SatCycleSlipMaker)
        {
            this.SatVisiblePeriodMarker = SatVisiblePeriodMarker;
            this.SatCycleSlipMaker = SatCycleSlipMaker;

            InitSelector(SatVisiblePeriodMarker.TimePeriod.Start);
        }



        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="from"></param>
        /// <param name="exceptPrns"></param>
        public void InitSelector(Time from, SatelliteNumber exceptPrns)
        {
            InitSelector(from, new List<SatelliteNumber>() { exceptPrns });
        }
        /// <summary>
        /// 设置值
        /// </summary>
        public void InitSelector(Time from, List<SatelliteNumber> exceptPrns = null)
        {
            this.PeriodSats = new Dictionary<BufferedTimePeriod, SatelliteNumber>();

            //选星策略：选择时段最长的星，以此类推
            var nextStart = from;
            var end = SatVisiblePeriodMarker.TimePeriod.End;
            while (nextStart < end)
            {
                List<SatPeriod> satPeriods = SatVisiblePeriodMarker.GetSortedSatPeriods(nextStart);

                if (satPeriods.Count == 0)//没有搜索到区域，则添加时间继续搜索
                {
                    nextStart = nextStart + 60.0;//添加 1 分钟
                }
                else
                {
                    List<SatPeriodWeight> sats = new List<SatPeriodWeight>();
                    foreach (var period in satPeriods)
                    {
                        int count = 0;
                        for (var i = period.TimePeriod.Start; i < period.TimePeriod.End; i += SatCycleSlipMaker.Interval)
                        {
                            if (SatCycleSlipMaker.Contains(period.Prn, i))
                                count++;
                        }
                        sats.Add(new SatPeriodWeight(period.Prn, period.TimePeriod, count));
                    }
                    sats.Sort();
                    sats.Reverse();

                    var best = sats[0];
                    if (exceptPrns != null)
                    {
                        foreach (var item in sats)
                        {
                            if (!exceptPrns.Contains(item.Prn))
                            {
                                best = item;
                            }
                        }
                    }

                    //添加一个最好的。
                    PeriodSats.Add(best.TimePeriod, best.Prn);

                    //下一段
                    nextStart = best.TimePeriod.End;
                }
            }
        }


          /// <summary>
        /// 从当前时间选择最优的卫星序列,越往前越优。
        /// </summary>
        /// <param name="from">时间</param>
        /// <returns></returns>
        public List<SatelliteNumber> GetSortedPrns(Time from)
        {
             List<SatPeriodWeight> sats = GetSortedSatPeriods(from);
             List<SatelliteNumber> prns = new List<SatelliteNumber>();
             foreach (var item in sats)
             {
                 prns.Add(item.Prn);
             }
             return prns;
        }


        /// <summary>
        /// 从当前时间选择最优的卫星序列,越往前越优。
        /// </summary>
        /// <param name="from">时间</param>
        /// <returns></returns>
        public List<SatPeriodWeight> GetSortedSatPeriods(Time from)
        {
            //选星策略：选择时段最长的星，以此类推
            var nextStart = from;
            List<SatPeriod> satPeriods = null;
            while (nextStart < SatVisiblePeriodMarker.TimePeriod.End)
            {
                satPeriods = SatVisiblePeriodMarker.GetSortedSatPeriods(nextStart);

                if (satPeriods.Count == 0)//没有搜索到区域，则添加时间继续搜索,指导结束
                {
                    nextStart = nextStart + 60.0;//添加 1 分钟
                }
                else
                {
                    break;
                }
            }
            List<SatPeriodWeight> sats = new List<SatPeriodWeight>();
            foreach (var period in satPeriods)
            {
                int count = 0;
                for (var i = period.TimePeriod.Start; i < period.TimePeriod.End; i += SatCycleSlipMaker.Interval)
                {
                    if (SatCycleSlipMaker.Contains(period.Prn, i))
                        count++;
                }
                sats.Add(new SatPeriodWeight(period.Prn, period.TimePeriod, count));
            }
            sats.Sort();
            sats.Reverse();
            return sats;
        }

        /// <summary>
        /// 卫星历元标记器。
        /// </summary>
        public SatPeriodInfoManager SatVisiblePeriodMarker { get; set; }
        /// <summary>
        /// 卫星周跳标记器
        /// </summary>
        public SatPeriodInfoManager SatCycleSlipMaker { get; set; }

        /// <summary>
        /// 卫星历元字典。
        /// </summary>
        public Dictionary<BufferedTimePeriod, SatelliteNumber> PeriodSats { get; set; }

        /// <summary>
        /// 选择好的卫星。
        /// </summary>
        /// <param name="time">指定时间</param>
        /// <returns></returns>
        public SatelliteNumber GetSelectedPrn(Time time)
        {
            foreach (var period in PeriodSats.Keys)
            {
                if (period.Contains(time) ||(period.BufferedEnd>=time && period.BufferedStart<=time))
                    return PeriodSats[period];
            }
            return SatelliteNumber.Default;
        }

        #region IO
        public string GetFormatedString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var kv in PeriodSats)
            {
                sb.Append(kv.Value);
                sb.Append(" ");
                sb.Append(kv.Key);
                sb.Append("(");
                sb.Append((kv.Key.Span / 3600).ToString("0.00") + "h");
                sb.Append(")");
                sb.Append(",");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return GetFormatedString();
        }
        #endregion
    }

}