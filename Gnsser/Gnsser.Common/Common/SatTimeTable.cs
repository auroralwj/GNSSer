//2017.07.22, czs, create in hongqing, 双键表
//2017.10.09, czs, edit in hongqing, 解析B

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// 时间 卫星 表格类。
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class SatTimeTable<TValue> :TwoKeyTable<Time, SatelliteNumber, TValue>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        public SatTimeTable(ObjectTableStorage Table, string keyA = "Epoch", string keyB= "PRN")
            :base(Table, keyA, keyB)
        { 
        }
        public override SatelliteNumber ParseKeyB(object keyBData)
        {
            return SatelliteNumber.Parse(keyBData.ToString());
        }
    }
}
