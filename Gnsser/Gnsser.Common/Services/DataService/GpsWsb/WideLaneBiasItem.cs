//2016.08.21, czs, create in fujian yong'an, 法国宽项读取器
//2017.03.02, czs, edit in hongqing, 为了和MW结果一致，此处采用基准星减去其它星。
//2018.09.06, czs, edit in hmx, 修改，此处采用基准星减去其它星,归算到MW区间

using System;
using System.IO;
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
using Geo.Times;
using Geo.IO;
using Gnsser;
using Gnsser.Checkers;
using Geo.Referencing;
using Geo.Utils;

namespace Gnsser
{
    /// <summary>
    /// 法国宽项读取器。 
    /// </summary>
    public class WideLaneBiasItem : OrderedProperty
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WideLaneBiasItem()
        {
            Data = new BaseDictionary<SatelliteNumber, double>();
            OrderedProperties = new List<string>() { "Time", "Value" };
        } 
        #region 属性
        /// <summary>
        /// 属性
        /// </summary>
        public BaseDictionary<SatelliteNumber, double> Data { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }   
        #endregion

        public int Count { get => Data.Count; }

        public override bool Equals(object obj)
        {
            var o = obj as WideLaneBiasItem;
            if (o == null) return false;

            return Time == o.Time;
        }
        /// <summary>
        /// 为了和MW结果一致，此处采用基准星减去其它星。归算到0-1区间，根据N=B-f使用即可。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, double> GetMwDiffer(SatelliteNumber basePrn)
        {
            Dictionary<SatelliteNumber, double> data = new Dictionary<SatelliteNumber, double>();
            double baseValue = 0;
            
            if (this.Data.Contains(basePrn))
            {
                baseValue = Data[basePrn];
            }
            else
            {
                return data;
            }
            
            foreach (var item in Data.Keys)
            {
                //如果要利用此产品进行宽巷模糊度固定，1)将其进行星间差分(减去基准卫星)；2)反向或取其相反数；3)归算到[0, 1]区间,得f；4)根据N=B-f使用。
                data[item] = Geo.Utils.DoubleUtil.GetRoundFraction( -(Data[item]- baseValue));// - ;
            }
            return data;
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
            var splitter = "\t";
            StringBuilder sb = new StringBuilder();
            sb.Append(Time.ToString());
           sb.Append(splitter);
            foreach (var item in Data.Keys)
            {
                sb.Append(item + " : " + Data[item]);
                sb.Append(splitter);
            }
             
            return sb.ToString();
        }

        public string ToLines()
        { 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Time.ToString()); 
            foreach (var item in Data.Keys)
            {
                sb.AppendLine(item + "\t" + Data[item]); 
            }
             
            return sb.ToString();
        }

        public string ToLineString()
        {
            var splitter = "\t";
            StringBuilder sb = new StringBuilder();
            sb.Append(Time.ToString()); 
            sb.Append(splitter);
            foreach (var item in Data)
            {
                sb.Append(item); 
                sb.Append(splitter);
            }
             

            return sb.ToString();
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static WideLaneBiasItem Parse(string line)
        {
            var items = line.Split(new char[] { '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            WideLaneBiasItem item = new WideLaneBiasItem();
            int i = 0;
            Time time = new Time(int.Parse(items[i++]), int.Parse(items[i++]), int.Parse(items[i++]), int.Parse(items[i++]));
            for (int j = 4; j < 41; j++)
            {
                var prn = new SatelliteNumber(j - 4, SatelliteType.G);
                var val = Double.Parse(items[i++]);
                if(val == 0) { val = Double.NaN; }
                item.Data.Add(prn, val);
            }
            return item;
       
        }
         
    }

}