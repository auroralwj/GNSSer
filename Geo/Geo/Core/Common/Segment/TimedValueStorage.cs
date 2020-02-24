//2016.08.03, czs, create in fujian yongan, 时段数据存储器

using System;
using System.IO;
using Geo;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using System.Collections.Generic;

namespace Geo
{ 
    /// <summary>
    /// 时段信息，绑定对象为默认的字符串。
    /// </summary>
    public class TimedStringStorage :TimedValueStorage<string>{
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public override string Parse(string str)
        {
            return (str);
        }


        /// <summary>
        /// 解析文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TimedStringStorage Load(string path)
        {
            var storage = new TimedStringStorage();
            using (StreamReader r = new StreamReader(path))
            {
                string line = null;
                while ((line = r.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }
                    var divideLen = line.IndexOf(':');
                    var name = line.Substring(0, divideLen);
                    var content = line.Substring(divideLen + 1);
                    var periods = SuccessiveTimePeriod.Parse(content);
                    if (!storage.Contains(name))
                    {
                        storage.Add(name, periods);
                    }
                }
            }
            return storage;
        }
    }

    /// <summary>
    /// 时段数据存储器
    /// </summary>
    /// <typeparam name="T">时间相关的数据</typeparam>
    public abstract class TimedValueStorage<T> : BaseDictionary<string, SuccessiveTimePeriod>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public TimedValueStorage()
        {
        }

        #region 获取
        /// <summary>
        /// 是否可以获取。是否已经存储指定的模糊度。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsAvailable(string name, Time time)
        {
            if (!this.Contains(name)) { return false; } 

            var timePeriods = this[name];
            if (!timePeriods.Contains(time)) { return false; } 
            
            return true;
        }

        /// <summary>
        /// 得到模糊度
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual T GetValue(string name, Time time)
        {
            if (!this.IsAvailable(name, time)) { return default(T); }

            var timePeriods = this[name];
            var tag = timePeriods.GetSegment(time).Tag;

            return Parse(tag.ToString());
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public abstract T Parse(string str);
        /// <summary>
        /// 采用关键字获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public T GetFirstMatchedValue(string key, Time time)
        {
            var timePeriods = this.GetFirstMatched(key);
            if (timePeriods == null)
            {
                return default(T);
            }
            var tag = timePeriods.GetSegment(time).Tag;

            return Parse(tag.ToString());
        }


        #endregion

        #region 注册

        /// <summary>
        /// 注册一个模糊度信息。
        /// 注册后，默认为后续一天的模糊度都不变化，除非新的参数进来。
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="time">时间</param>
        /// <param name="val">绑定的数据：周跳或模糊度</param>
        public void Regist(string name, Time time, T val)
        {
            if (!this.Contains(name)) { this[name] = new SuccessiveTimePeriod(); }

            var timePeriods = this[name];
            //崭新的模糊度。
            if (!timePeriods.Contains(time))
            {
                var newPeriod = BuildNewAmbiguityPeriod(time, val);
                timePeriods.Add(newPeriod);
            }
            else//时段已经包含，说明之前已经有模糊度入驻,判断或新赋值。
            {
                var period = timePeriods.GetSegment(time);
                if (!period.Tag.ToString().Equals(val.ToString())) //模糊度已经变化，需要重新赋值，
                {
                    period.End = time - 0.5;//终止上一个模糊度数据，终止时间为前 0.5 秒。

                    var newPeriod = BuildNewAmbiguityPeriod(time, val);
                    timePeriods.Add(newPeriod);
                }
                else //否则只延续结束时间
                {
                    period.End = time + TimeSpan.FromDays(1);
                }
            }
        }

        /// <summary>
        /// 建立一个新的时段，存储新模糊度信息，该模糊度默认一天不变。
        /// </summary>
        /// <param name="time"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public TimePeriod BuildNewAmbiguityPeriod(Time time, T val)
        {
            var newPeriod = new TimePeriod(time, time + TimeSpan.FromDays(1)) { Tag = val+"" };
            return newPeriod;
        }
        #endregion

        #region IO
        /// <summary>
        /// 写到文件
        /// </summary>
        /// <param name="path"></param>
        public void WriteToFile(string path)
        {
            using (StreamWriter sr = new StreamWriter(path, true))
            {
                foreach (var item in this.Keys)
                {
                    sr.WriteLine(item + ":" + this[item].ToString());
                }
            }
        }

        #endregion
    }      
}
