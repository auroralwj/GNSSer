//2015.01.08, czs, create in namu shuangliao, 卫星时段信息管理器
//2015.01.09, czs, create in namu shuangliao, 抽离实现数据提取部分，本类只负责存储
//2015.10.18, czs, edit in pengzhou railway station, 相反的时段，即有时段的为空，没有时段的为时段。

using System;
using System.Collections.Generic;
using System.Text;
using System.IO; 
using Gnsser.Times;
using Geo; 
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm; 
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times; 

namespace Gnsser
{ 
    /// <summary>
    /// 卫星时段信息标记器.通常作为一个观测文件的统计信息。如周跳统计，卫星可见性等。
    /// 本类主要作为一种存储数据结构，而不管具体的算法。
    /// </summary>
    public class SatPeriodInfoManager : BaseDictionary<SatelliteNumber, BufferedSuccessiveTimePeriod>
    {
        /// <summary>
        /// 卫星时段信息管理器，构造函数
        /// </summary> 
        public SatPeriodInfoManager(double Interval = 30.0)
        {
            this.Interval = Interval; 
        } 
         

        /// <summary>
        /// 时间间隔
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// 管理器的总的有效时段
        /// </summary>
        public BufferedTimePeriod TimePeriod
        {
            get
            {
                Time start = Time.MaxValue;
                Time end = Time.MinValue;
                foreach (var item in Data)
                {
                    if (item.Value.Start < start && item.Value.Start > Time.MinValue)
                    { start = item.Value.Start; }
                    if (item.Value.End > end && item.Value.End < Time.MaxValue) { end = item.Value.End; }
                }
                return new BufferedTimePeriod(start, end);
            }
        }
        /// <summary>
        /// 卫星编号。
        /// </summary>
        public List<SatelliteNumber> Prns { get { return new List<SatelliteNumber>(Data.Keys); } }

        /// <summary>
        /// 添加观测时段.自动根据 间隔 Interval 生成区段
        /// </summary>
        /// <param name="prn">卫星</param>
        /// <param name="GpsTime">时刻，自动根据 间隔 Interval 生成区段</param>
        public void AddTimePeriod(SatelliteNumber prn, Time GpsTime)
        {
            double halfInterval = this.Interval / 2.0 + 0.00000001;//防止舍入误差
            var from = GpsTime - halfInterval;
            var to = GpsTime + halfInterval;

            this.AddTimePeriod(prn, new BufferedTimePeriod(from, to));
        }


        /// <summary>
        /// 卫星的指定时刻是否包含
        /// </summary>
        /// <param name="satelliteType">卫星</param>
        /// <param name="Time">时刻</param>
        /// <returns></returns>
        public bool Contains(SatelliteNumber prn, Time GpsTime)
        {
            return GetPeriods(prn).Contains(GpsTime);// || (GetPeriods(satelliteType).End >= GpsTime && GetPeriods(satelliteType).Start <= GpsTime);
        }

        /// <summary>
        /// 添加观测时段
        /// </summary>
        /// <param name="satelliteType">卫星</param>
        /// <param name="period">一个时段</param>
        public void AddTimePeriod(SatelliteNumber prn, BufferedTimePeriod period)
        {
            GetPeriods(prn).Add(period);
        }

        /// <summary>
        /// 返回指定卫星的时段信息。
        /// </summary>
        /// <param name="satelliteType">卫星</param>
        /// <returns></returns>
        public BufferedSuccessiveTimePeriod GetPeriods(SatelliteNumber prn)
        {
            if (!this.Contains(prn)) this[prn] = new BufferedSuccessiveTimePeriod();

            return this[prn];
        }

        /// <summary>
        /// 指定时间中，剩余时段（往未来方向）最长（从大到小）的卫星排序。
        /// 注意：与默认的排序（从小到大）是相反的。
        /// </summary>
        /// <param name="gpsTime">时间</param>
        /// <returns></returns>
        public List<SatPeriod> GetSortedSatPeriods(Time gpsTime)
        {
            List<SatPeriod> list = new List<SatPeriod>();
            foreach (var kv in this.Data)
            {
                var period = kv.Value.GetSegment(gpsTime);
                if (period == null) continue;
                BufferedTimePeriod newPeriod = new BufferedTimePeriod(gpsTime, period.End + 0.1, period.EndBuffer);

                list.Add(new SatPeriod(kv.Key, newPeriod));
            }

            list.Sort();
            list.Reverse();
            return list;
        }

        /// <summary>
        /// 提取长度小于或大于指定参数的时段
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public SatPeriodInfoManager GetFilteredPeriods(double span, bool smallerOrLarger = true)
        {
            SatPeriodInfoManager periods = new Gnsser.SatPeriodInfoManager(Interval);

            var timePeriod = this.TimePeriod;

            foreach (var item in this.Data)
            {
                var p = item.Value;
                periods[item.Key] = item.Value.GetFilteredPeriods(span, smallerOrLarger);
            }

            return periods;
        }

        /// <summary>
        /// 相反的时段
        /// </summary>
        public SatPeriodInfoManager Opposite
        {
            get
            {
                SatPeriodInfoManager periods = new Gnsser.SatPeriodInfoManager(Interval);

                var timePeriod = this.TimePeriod;

                foreach (var item in this.Data)
                {
                    var p = item.Value;
                    periods[item.Key] = item.Value.Oppersite;
                }

                return periods;
            }
        }

        #region IO
        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToFormatedString();
        }
        /// <summary>
        /// 获取格式化字符串。
        /// </summary>
        /// <returns></returns>
        public string ToFormatedString()
        {
            StringBuilder sb = new StringBuilder();

            List<SatelliteNumber> prns = new List<SatelliteNumber>(Prns);
            prns.Sort();
            foreach (var item in prns)
            {
                sb.AppendLine(item + ": " + Data[item].ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 保存为文本文档。
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="isSortPrn">是否按照卫星编号排序</param>
        /// <param name="intervalSec">多长时间输出一个符号</param>
        /// <param name="splitCount">多少字符输入一个头部分隔符号</param>
        public void SaveSatPeriodText(string path, bool isSortPrn = false , double intervalSec = 2 * 60, int splitCount = 8)
        {
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));

            StringBuilder total = GetTextChart(isSortPrn, intervalSec, splitCount);
            File.WriteAllText(path, total.ToString());
        }

        /// <summary>
        /// 绘制文本图形。
        /// </summary>
        /// <param name="sys">系统</param>
        /// <param name="isSortPrn">是否排序</param>
        /// <param name="intervalSec">多长时间输出一个符号</param>
        /// <param name="splitCount">多少字符输入一个头部分隔符号</param>
        /// <returns></returns>
        public StringBuilder GetTextChart( bool isSortPrn = false, double intervalSec = 2 * 60, int splitCount = 8)
        {
            List<SatelliteNumber> all = new List<SatelliteNumber>(this.Prns);
            List<SatelliteNumber> prns = new List<SatelliteNumber>();
            foreach (var item in all)
            {
               prns.Add(item);
            }

            if (isSortPrn) prns.Sort();


            var span = this.TimePeriod;
            if (span.End <= Time.MinValue)
            {
                log.Debug("没有数据，最大时间小于 " + Time.MinValue);
                return new StringBuilder();
            }
            double interval = this.Interval;
            ;
            Dictionary<SatelliteNumber, StringBuilder> dic = new Dictionary<SatelliteNumber, StringBuilder>();
            foreach (var sat in prns)
            {
                dic[sat] = new StringBuilder();
                dic[sat].Append(sat + " ");
            }
            StringBuilder title = new StringBuilder();
            title.Append("   +");
            int i = 0;
            for (DateTime time = span.Start.DateTime; time < span.End.DateTime; time += TimeSpan.FromSeconds(intervalSec), i++)
            {
                if (i % splitCount == 0) title.Append("|");
                else title.Append("-");

                foreach (var sat in prns)
                {
                    var periods = this.GetPeriods(sat);

                    //如果包含，就显示该卫星的编号
                    var gnsserTime = Time.Parse(time);
                    bool has = (periods.Contains(gnsserTime));// || (periods.Start <= gnsserTime && periods.End >= gnsserTime));
                    if (has) dic[sat].Append("o");
                    else dic[sat].Append(" ");
                }
            }

            //write 
            StringBuilder total = new StringBuilder();
            total.AppendLine(title.ToString());
            foreach (var sat in prns)
            {
                total.AppendLine(dic[sat].ToString() + " " + sat.ToString());
            }
            total.AppendLine(title.ToString());
            return total;
        }

        #endregion
    }
}