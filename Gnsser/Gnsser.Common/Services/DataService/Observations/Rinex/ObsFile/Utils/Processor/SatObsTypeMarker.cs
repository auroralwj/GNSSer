//2015.02.09, czs, create in 麦克斯 双辽， 观测文件瘦身，去除不必要的观测量。

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Geo.Utils;
using System.Text;
using Geo.Coordinates; 
using Gnsser.Times;

namespace Gnsser.Data.Rinex
{ 
    /// <summary>
    /// 观测类型标记，用于统计观测类型的出勤次数，进而进行数据筛选。
    /// </summary>
    public class SatObsTypeMarker : IEnumerable<string>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SatObsTypeMarker()
        {
            Markers = new Dictionary<string, int>();
        }
        /// <summary>
        /// 卫星类型
        /// </summary>
        public SatelliteType SatelliteType { get; set; }
        /// <summary>
        /// 字典标记，标记出现的次数
        /// </summary>
        private Dictionary<string, int> Markers { get; set; }

        /// <summary>
        /// 设置该标记是否有值
        /// </summary>
        /// <param name="type"></param>
        public void Markes(string type)
        {
            if (!Markers.ContainsKey(type)) Markers[type] = 0;

            Markers[type]++;
        }
        /// <summary>
        /// 设置一个数字
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        public void SetObsTypeCount(string type, int count)
        { 
            Markers[type] = count;
        }

        /// <summary>
        /// 是否包含记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Contains(string type)
        {
            return Markers.ContainsKey(type);
        }
        /// <summary>
        /// 若有，则返回，若无，则返回0.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int TryGetCount(string type)
        {
            if (Contains(type)) return Markers[type];
            return 0;
        }

        /// <summary>
        /// 获取最大的数量
        /// </summary>
        /// <returns></returns>
        public int GetMaxCount()
        {
            int max = 0;
            foreach (var item in this.Markers)
            {
                if (item.Value > max) max = item.Value;
            }
            return max;
        }

        /// <summary>
        /// 获取百分比。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public double GetPercentage(string type)
        {
            return TryGetCount(type) * 1.0 / GetMaxCount();
        }

        /// <summary>
        /// 获取百分比
        /// </summary>
        /// <param name="type"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public double GetPercentage(string type, int totalCount)
        {
            return TryGetCount(type) * 1.0 / totalCount;
        }

        #region IEnumerator
        public IEnumerator<string> GetEnumerator()
        {
            return Markers.Keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Markers.Keys.GetEnumerator();
        }
        #endregion
    }

}
