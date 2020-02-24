//2016.08.03, czs, create in fujian yongan, 模糊度存储器

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
    /// 数字时段存储器
    /// </summary>
    public class TimedNumberStorage :TimedValueStorage<Double> {

        /// <summary>
        /// 获取模糊度向量
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        internal IVector GetVector(System.Collections.Generic.List<string> paramNames, Time time)
        {
            var length = paramNames.Count;
            Vector v = new Vector(length);
            for (int i = 0; i < length; i++)
            {
                var name = paramNames[0];
                if (!this.IsAvailable(name, time)) { throw new GeoException("没有该参数的数值信息 " + name + ", Time :" + time); }
                v[i] = this.GetValue(name, time);
            }
            return v;
        }

        /// <summary>
        /// 批量注册
        /// </summary>
        /// <param name="time"></param>
        /// <param name="vector"></param>
        public void Regist(Time time, IVector vector)
        {
            var length = vector.Count;

            for (int i = 0; i < length; i++)
            {
                var name = vector.ParamNames[i];
                if (name == null) continue;
                Regist(name, time, vector[i]);
            }
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public override double Parse(string str)
        {
            return Double.Parse(str);
        }


        /// <summary>
        /// 解析文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TimedNumberStorage Load(string path)
        {
            TimedNumberStorage storage = new TimedNumberStorage();
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
}