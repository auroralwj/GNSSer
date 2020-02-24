//20140.12.11, czs, create in jinxinliaomao shuangliao, 卫星数量统计器

using System;
using System.Collections.Generic;
using System.Text; 

namespace Gnsser
{
    /// <summary>
    /// 卫星出现的比率或者次数。 主要用于卫星选择。
    /// </summary>
    public class SatelliteCounter  
    {
        /// <summary>
        ///  卫星出现次数，构造函数。
        /// </summary>
        public SatelliteCounter(){
            this.Counter = new Dictionary<SatelliteNumber, int>();
        }
        /// <summary>
        /// 统计器
        /// </summary>
        public Dictionary<SatelliteNumber, int> Counter { get; set; }
        /// <summary>
        /// 获取最大出现次数
        /// </summary>
        public int MaxCount
        {
            get
            {
                int max = 0;
                foreach (var item in this.Counter)
                {
                    if (max < item.Value) max = item.Value;
                }
                return max;
            }
        }

        /// <summary>
        /// 获取出现次数最大的卫星列表
        /// </summary>
        /// <returns></returns>
        public List<SatelliteNumber> MaxPrns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                int max = this.MaxCount;
                foreach (var item in this.Counter)
                {
                    if (max == item.Value) list.Add(item.Key);
                }
                return list;
            }
        }
        /// <summary>
        /// 获取非最大次数的卫星列表
        /// </summary>
        /// <returns></returns>
        public List<SatelliteNumber> NotMaxPrns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                int max = this.MaxCount;
                foreach (var item in this.Counter)
                {
                    if (max != item.Value) list.Add(item.Key);
                }
                return list;
            }
        }
        /// <summary>
        /// 统计一个卫星编号。
        /// </summary>
        /// <param name="satelliteType"></param>
        public void Add(SatelliteNumber prn)
        {
            if (!Counter.ContainsKey(prn))  Counter[prn] =  1;
            else Counter[prn] = Counter[prn] + 1;
        }
        /// <summary>
        /// 添加列表
        /// </summary>
        /// <param name="prns">列表</param>
        public void Add(List<SatelliteNumber> prns)
        {
            foreach (var prn in prns)
            {
                Add(prn);
            }
        }
        /// <summary>
        /// 直接添加列表
        /// </summary>
        /// <param name="satData">字典</param>
        public void Add(Dictionary<SatelliteNumber, int> dic)
        {
            foreach (var prn in dic.Keys)
            {
                this.Add(prn);
            }
        }

        /// <summary>
        /// 合并卫星数量统计器。
        /// </summary>
        /// <param name="dicA">字典 A</param>
        /// <param name="dicB">字典 B</param>
        /// <returns></returns>
        public static Dictionary<SatelliteNumber, int> Merge(Dictionary<SatelliteNumber, int> dicA, Dictionary<SatelliteNumber, int> dicB)
        {
            Dictionary<SatelliteNumber, int> data = new Dictionary<SatelliteNumber, int>();
            foreach (var item in dicA)
            {
                data.Add(item.Key, item.Value);
            }
            foreach (var prn in dicB.Keys)
            {
                if (!data.ContainsKey(prn))
                {
                    data.Add(prn, 1);
                }
                else data.Add(prn, data[prn] + dicB[prn]);
            }
            return data;
        }

        /// <summary>
        /// 统计出现的次数。从 1 开始。
        /// </summary>
        /// <param name="prns">卫星编号列表</param>
        /// <returns></returns>
        public static Dictionary<SatelliteNumber, int> GetAppearCount(List<SatelliteNumber> prns)
        {
            Dictionary<SatelliteNumber, int> data = new Dictionary<SatelliteNumber, int>();
            foreach (var prn in prns)
            {
                if (!data.ContainsKey(prn))
                {
                    data.Add(prn, 1);
                }
                else data.Add(prn, data[prn]++);
            }
            return data;
        }
    }
}
