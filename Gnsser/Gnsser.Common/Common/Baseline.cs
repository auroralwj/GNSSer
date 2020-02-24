//2015.11.05, czs & cy, create in 洪庆, 基线类


using System;
using Geo.Coordinates;
using Gnsser.Data;
using System.Collections.Generic;
using Gnsser.Correction;
using Geo.Times;
using Gnsser.Times;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// 基线
    /// </summary>
    public class Baseline : OrderedProperty
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Baseline()
        {
            this.OrderedProperties = new List<string>()
            {
                Geo.Utils.ObjectUtil.GetPropertyName<Baseline>(m=>m.StartName),
                Geo.Utils.ObjectUtil.GetPropertyName<Baseline>(m=>m.EndName),
            };
        }
        /// <summary>
        /// 参考站名称
        /// </summary>
        public string StartName { get; set; }
        /// <summary>
        /// 流动站名称
        /// </summary>
        public string EndName { get; set; }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return StartName + "-" + EndName;
        }
        /// <summary>
        /// 等于否
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Baseline o = obj as Baseline;
            if (o == null) { return false; }
            return o.StartName.Equals(this.StartName) && o.EndName.Equals(this.EndName);
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return StartName.GetHashCode() + EndName.GetHashCode();
        }
    }
}
