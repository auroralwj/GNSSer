//2018.09.08, czs, create in hmx, 分组数据
//2018.10.20, czs, edit in hmx, 各类分类型单独存储


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Geo.Times;

namespace Geo
{ /// <summary>
    /// 历元数据
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class EpochNumeralStorage<TKey> : EpochValueStorage<TKey, double>
    {
        /// <summary>
        /// 总共的观测时段
        /// </summary>
        public TimePeriod TimePeriod
        {
            get
            {
                TimePeriod timePeriod = new TimePeriod(this.FirstKey, this.LastKey);
                return timePeriod;
            }
        }
        /// <summary>
        /// 获取历元信息
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public BaseDictionary<TKey, double> GetEpochValues(Time epoch)
        {
            return Get(epoch);
        }
    }

    /// <summary>
    /// 历元数据
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class EpochRmsNumeralStorage<TKey> : EpochValueStorage<TKey, RmsedNumeral>
    {
        /// <summary>
        /// 总共的观测时段
        /// </summary>
        public TimePeriod TimePeriod
        {
            get
            {
                TimePeriod timePeriod = new TimePeriod(this.FirstKey, this.LastKey);
                return timePeriod;
            }
        }
        /// <summary>
        /// 获取历元信息
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public BaseDictionary<TKey, RmsedNumeral> GetEpochValues(Time epoch)
        {
            return Get(epoch);
        }
    }

    /// <summary>
    /// 分组数据存储
    /// </summary>
    /// <typeparam name="TKey"></typeparam> 
    /// <typeparam name="TValue"></typeparam>
    public class EpochValueStorage<TKey, TValue> : BaseDictionary<Time, BaseDictionary<TKey, TValue>>
    {
        public override BaseDictionary<TKey, TValue> Create(Time key)
        {
            return new BaseDictionary<TKey, TValue>();
        }
    }

    /// <summary>
    /// 历元分组数据列表存储
    /// </summary>
    /// <typeparam name="TKey"></typeparam> 
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TValueKey"></typeparam>
    public class EpochValueListStorage<TKey, TValueKey, TValue> : EpochValueStorage<TKey, Dictionary<TValueKey, TValue>>
    {
        public override BaseDictionary<TKey, Dictionary<TValueKey, TValue>> Create(Time key)
        {
            return new BaseDictionary<TKey, Dictionary<TValueKey, TValue>>(key.ToString(), TValueKey => new Dictionary<TValueKey, TValue>());
        }
    }
  
}
