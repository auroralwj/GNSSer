//2017.06.14, czs, create in hongqing, FCB 数据服务

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Times;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo.Times;
using Geo;

namespace Gnsser.Data
{ 

    /// <summary>
    /// FCB 数据文件。IGS发布的产品。
    /// </summary>
    public class FcbFile : EpochValueStorage<SatelliteNumber, FcbValue>// BaseDictionary<Time, <BaseDictionary<SatelliteNumber, IEnumerable<FcbValue>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public FcbFile(FcbFileHeader Header = null)
        {
            this.Header = Header; 
            this.Interval = TimeSpan.FromMinutes(15);
        } 
        /// <summary>
        /// 头部信息。
        /// </summary>
        public FcbFileHeader Header { get; set; }
        /// <summary>
        /// 采样间隔
        /// </summary>
        public TimeSpan Interval { get; set; }

        public KeyValuePair<Time, BaseDictionary<SatelliteNumber, FcbValue>> Current;
        /// <summary>
        /// 数据列表
        /// </summary>
        public Dictionary<Time, FcbValue> FcbInfos { get; set; }

        /// <summary>
        /// Method to clear all previously loaded satellite satData.
        /// </summary>
        public void Clear()
        {
            FcbInfos.Clear();
        }
        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="time"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> GetFcbOfBsdDic(Time time, SatelliteNumber basePrn)
        {
            var result = new Dictionary<SatelliteNumber, RmsedNumeral>();
            var fcbDic =  GetFcbDic(time);
            if (!fcbDic.Contains(basePrn))
            {
                return result;                
            }
            var baseVal = fcbDic[basePrn];

            foreach (var item in fcbDic.KeyValues)
            {
                result[item.Key] = item.Value - baseVal;
            }
            return result;
        }

        /// <summary>
        /// FcbOfBsdValue 
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public RmsedNumeral GetFcbOfBsdValue( Time Time, SatelliteNumber prn, SatelliteNumber basePrn)
        {
            FcbValue thisVal = GetFcbValue(prn, Time);
            if (thisVal == null) { return RmsedNumeral.NaN; }
            FcbValue baseVal = GetFcbValue(basePrn, Time);
            if (baseVal == null) { return RmsedNumeral.NaN; }

            var bsd = thisVal.Value - baseVal;
            bsd = Geo.Utils.DoubleUtil.GetRoundFraction(bsd);
            return bsd;
        }


        /// <summary>
        /// 检索获取
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public double GetValue(SatelliteNumber prn, Time Time)
        {
            FcbValue sat = GetFcbValue(prn, Time);
            if (sat == null) return 0.0;
            return sat.Value;
        }

        /// <summary>
        /// 获取第一个满足条件的对象
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public FcbValue GetFcbValue(SatelliteNumber prn, Time time)
        {
            BaseDictionary<SatelliteNumber, FcbValue> dic = GetFcbDic(time);

            if (dic!=null && dic.Contains(prn))
            {
                return dic[prn];
            }

            return null;
        }

        public BaseDictionary<SatelliteNumber, FcbValue> GetFcbDic(Time time)
        {
            var mTime = Current.Key;
            BaseDictionary<SatelliteNumber, FcbValue> dic = null;
            if (IsInTime(time, mTime))
            {
               dic = Current.Value;
            }
            else
            {
                var epoch = this.Keys.Find(m => m >= (time - Interval) && m <= (time + Interval));
                dic = this[epoch];
            }

            return dic;
        }

        private bool IsInTime(Time time, Time mTime)
        {
            return mTime >= (time - Interval) && mTime <= (time + Interval);
        }

        #region override
        //public IEnumerator<FcbValue> GetEnumerator()
        //{
        //    return FcbInfos.GetEnumerator();
        //}

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    return FcbInfos.GetEnumerator();
        //}
        #endregion

        //internal void Add(FcbValue item)
        //{
        //   this.GetOrCreate(item).GetOrCreate(item.
        //}
    }
  
}
