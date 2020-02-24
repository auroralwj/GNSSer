//2018.09.08, czs, create in hmx, 分组数据
//2018.10.20, czs, edit in hmx, 各类分类型单独存储


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Geo.Times;
using Geo.Algorithm.Adjust;

namespace Geo
{
    /// <summary>
    /// 时刻数据，安装时刻存储
    /// </summary>
    public class TimedRmsedNumeralStorage : TimedValueStorage<string, RmsedNumeral>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TimedRmsedNumeralStorage()
        {

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ambiguities"></param>
        public void Regist(Time time, WeightedVector ambiguities)
        {
            var rmses = ambiguities.GetRmsVector();
            foreach (var item in ambiguities.ParamNames)
            {
                var sto = this.GetOrCreate(item);
                sto[time] = new RmsedNumeral(ambiguities[item], rmses[item]);
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="time"></param>
        /// <param name="paramsDic"></param>
        public void Regist(Time time, Dictionary<string, object> paramsDic)
        {
            foreach (var kv in paramsDic)
            {
                var sto = this.GetOrCreate(kv.Key);
                if (kv.Value is RmsedNumeral)
                {
                    RmsedNumeral val = (RmsedNumeral)(kv.Value);
                    Regist(time, kv.Key, val);
                }
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="rms"></param>
        public void Regist(Time time, string name, double val, double rms)
        {
            var val2 = new RmsedNumeral(val, rms);
            Regist(time, name, val2);
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void Regist(Time time, string name, RmsedNumeral val)
        {
            var sto = this.GetOrCreate(name);
            sto[time] = val;
        }

        /// <summary>
        /// 生成时段产品
        /// </summary>
        /// <returns></returns>
        public PeriodRmsedNumeralStoarge GetProduct()
        {
            PeriodRmsedNumeralStoarge result = new PeriodRmsedNumeralStoarge();
            var keys = this.Keys;
            foreach (var key in keys)
            {
                var vals = this.Get(key);
                result[key] = BuildProduct(vals);
            }
            return result;
        }
        /// <summary>
        /// 一个参数的产品生成
        /// </summary>
        /// <param name="vals"></param>
        private static BaseDictionary<TimePeriod, RmsedNumeral> BuildProduct(BaseDictionary<Time, RmsedNumeral> vals)
        {
            BaseDictionary<TimePeriod, RmsedNumeral> store = new BaseDictionary<TimePeriod, RmsedNumeral>();
            var times = vals.Keys; ;
            times.Sort();

            TimePeriod timePeriod = null;
            RmsedNumeral lastVal = RmsedNumeral.Zero;

            foreach (var time in times)
            {
                var currentVal = vals[time];
                if (timePeriod == null) //初始化第一个
                {
                    timePeriod = new TimePeriod(time, time);
                    store[timePeriod] = currentVal;
                }
                else
                {
                    if (Math.Abs(currentVal.Value - lastVal.Value) > 1e-3) //注意小数偏差
                    {                                 //保存上一个                     
                        timePeriod = new TimePeriod(time, time);   //新建下一个
                        store[timePeriod] = currentVal;
                    }
                }

                //更新
                lastVal = currentVal;
                timePeriod.End = time;
            }

            return store;
        }
    }

    /// <summary>
    /// 时刻数据存储器。以时刻进行分类的数据。
    /// </summary>
    /// <typeparam name="TKey">数据键标识</typeparam>
    /// <typeparam name="TValue">数据</typeparam>
    public class TimedValueStorage<TKey, TValue> : BaseDictionary<TKey, TimedDictionary<TValue>>
    {
        /// <summary>
        /// 建立
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override TimedDictionary<TValue> Create(TKey key)
        {
            return new TimedDictionary<TValue>();
        }
    }

    /// <summary>
    /// 时刻数据字典
    /// </summary>
    /// <typeparam name="TValue">数据</typeparam>
    public class TimedDictionary<TValue> : BaseDictionary<Time, TValue>
    {

    }
    
}
