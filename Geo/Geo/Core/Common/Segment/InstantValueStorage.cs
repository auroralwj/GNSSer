//2016.08.04, czs, create in fujian yongan, 时段数据存储器

using System;
using System.IO;
using Geo;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using System.Text;
using System.Collections.Generic;

namespace Geo
{ 
    /// <summary>
    /// 时段信息，绑定对象为默认的字符串。
    /// </summary>
    public class InstantValueStorage : InstantValueStorage<string>
    {
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
        public static InstantValueStorage Load(string path)
        {
            var storage = new InstantValueStorage();
            using (StreamReader r = new StreamReader(path))
            {
                string line = null;
                while ((line = r.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }
                    var divideLen = line.IndexOf(':');
                    var name = line.Substring(0, divideLen);
                    var content = line.Substring(divideLen + 1);
                    var times = content.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (!storage.Contains(name))
                    {
                        storage[name] = new BaseList<Time>();
                    }

                    var list = storage[name];
                    foreach (var timeStr in times)
                    { 
                        var time = Time.ParseTimeOfDayWithTagString(timeStr); 
                        list.Add(time);
                    }
                }
            }
            return storage;
        }
    }

    /// <summary>
    /// 时刻数据存储器。通过时刻的 Tag 存储对象。
    /// </summary>
    /// <typeparam name="T">时间相关的数据</typeparam>
    public abstract class InstantValueStorage<T> : BaseDictionary<string, BaseList<Time>>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public InstantValueStorage()
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
            return GetValuue(time, timePeriods);
        }

        private T GetValuue(Time time, BaseList<Time> timePeriods)
        {
            foreach (var item in timePeriods)
            {
                if (item.Equals(time)) { return Parse(item.Tag.ToString()); }
            }

            return default(T);
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
            return GetValuue(time, timePeriods);
        }
        #endregion

        #region 注册

        /// <summary>
        /// 注册一个模糊度信息。
        /// 注册后，默认为后续一天的模糊度都不变化，除非新的参数进来。
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="time">时间</param> 
        public void Regist(string name, Time time)
        {
            if (!this.Contains(name)) { this[name] = new  BaseList<Time>(); }

            var timePeriods = this[name];
            //崭新的模糊度。
            if (!timePeriods.Contains(time))
            {
                timePeriods.Add(time);
            } 
        }
        /// <summary>
        /// 批量注册
        /// </summary>
        /// <param name="name"></param>
        /// <param name="times"></param>
        public void Regist(string name, IEnumerable<Time> times)
        {
            if (!this.Contains(name)) { this[name] = new BaseList<Time>(); }

            var timePeriods = this[name];
            //崭新的模糊度。 
            foreach (var time in times)
            {
                if (!timePeriods.Contains(time))
                {
                    timePeriods.Add(time);
                } 
            }
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
               
                foreach (var key in this.Keys)
                {
                    sr.Write(key + ":");
                    int i = 0;
                    foreach (var item in this[key])
                    {
                        if (i != 0) { sr.Write("; "); }
                        sr.Write(item.ToShortTimeOfDayStringWithTag()); 
                        i++;
                    }
                    sr.WriteLine();
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sr = new StringBuilder();

            foreach (var key in this.Keys)
            {
                sr.Append(key + ":");
                int i = 0;
                foreach (var item in this[key])
                {
                    if (i != 0) { sr.Append("; "); }
                    sr.Append(item.ToShortTimeOfDayStringWithTag());
                    i++;
                }
                sr.AppendLine();
            }
            return sr.ToString();
        }

        #endregion
    }      
}
