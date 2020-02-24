
//2016.08.11, czs, create in fujian yong'an, 基于无电离层组合星间单差模糊度的宽窄项

using System;
using System.IO;
using System.Text;
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
using Geo.Times;
using Geo.IO;
using Gnsser;
using Gnsser.Checkers;
using Geo.Referencing;
using Geo.Utils;

namespace Gnsser
{
    /// <summary>
    /// 基于无电离层组合星间单差模糊度的宽窄项。 
    /// </summary>
    public class DifferFcbOfSatDcbItem : OrderedProperty
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DifferFcbOfSatDcbItem()
        {
            this.OrderedProperties = new List<string>() { "Time", "Key", "WideLaneValue", "NarrowLaneValue" };
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <param name="wValue"></param>
        public DifferFcbOfSatDcbItem(string key, DateTime time, double wValue) : this(key, new Time( time), wValue) { }
        /// <summary>
        /// 常用构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <param name="wValue"></param>
        public DifferFcbOfSatDcbItem(string key, Time time, double wValue)
        {
            string[] prnString = key.Trim().Substring(0, 7).Split('-');
            SatelliteNumber rovPrn = SatelliteNumber.Parse(prnString[0]);
            SatelliteNumber refPrn = SatelliteNumber.Parse(prnString[1]);
            this.BasePrn = refPrn;
            this.Prn = rovPrn;
            this.Time = time;
            this.WideLaneValue = wValue;
            this.OrderedProperties = new List<string>() { "Time", "Key", "WideLaneValue", "NarrowLaneValue" };
        }
        #region 属性
         
        /// <summary>
        /// 关键字 格式如 G02-G01。
        /// </summary>
        public string Key { get { return Prn + "-" + BasePrn; } }

        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 基准卫星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 待算卫星
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 宽项值
        /// </summary>
        public double WideLaneValue { get; set; }
        /// <summary>
        /// 宽项值
        /// </summary>
        public double NarrowLaneValue { get; set; }
        #endregion

        public override bool Equals(object obj)
        {
            var o = obj as DifferFcbOfSatDcbItem;
            if (o == null) return false;

            return o.BasePrn == BasePrn
                && Prn == o.Prn
                && WideLaneValue == o.WideLaneValue
                && NarrowLaneValue == o.NarrowLaneValue;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToLineString();
        }

        public string ToLineString()
        {
            var splitter = "\t";
            StringBuilder sb = new StringBuilder();
            sb.Append(Time.ToString());
            sb.Append(splitter);
            sb.Append(Prn + "");
            sb.Append("-");
            sb.Append(BasePrn + "");
            sb.Append(splitter);
            sb.Append(WideLaneValue);
            sb.Append(splitter);
            sb.Append(NarrowLaneValue);

            return sb.ToString();
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DifferFcbOfSatDcbItem Parse(string line)
        {
            var strs = line.Split(new char[] { '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var time = Time.Parse(strs[0]);
            var prns = strs[1].Split('-');
            var prn = SatelliteNumber.Parse(prns[0]);
            var basePrn = SatelliteNumber.Parse(prns[1]);
            var wValue = double.Parse(strs[2]);
            var nValue = double.Parse(strs[3]);

            return new DifferFcbOfSatDcbItem()
            {
                Time = time,
                BasePrn = basePrn,
                Prn = prn,
                WideLaneValue = wValue,
                NarrowLaneValue = nValue
            };
       
        }
         
    }

}