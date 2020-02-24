//2017.07.22, czs, create in hongqing, 双键表
//2017.10.09, czs, edit in hongqing, 重构，提取键解析方法

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// 卫星权值
    /// </summary>
    public class SatWeightTable : SatTimeTable<double>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="maxTimeDelta"></param>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <param name="ValueTitle"></param>
        public SatWeightTable(ObjectTableStorage Table, double maxTimeDelta =  24 * 3600, string keyA = "Epoch", string keyB= "PRN", string ValueTitle = "Weight")
            : base(Table, keyA, keyB)
        {
            this.MaxTimeDelta = maxTimeDelta;
            this.ValueTitle = ValueTitle;
        }
        /// <summary>
        /// 最大时间偏差
        /// </summary>
        public double MaxTimeDelta { get; set; }
        /// <summary>
        /// 值标题
        /// </summary>
        public string ValueTitle { get; set; }
         

        public bool Contains(SatelliteNumber prn)
        {
            return this.Data.ContainsKeyB(prn);
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public override double RowToValue(Dictionary<string, object> row)
        {
            var val = Geo.Utils.StringUtil.ParseDouble(row[ValueTitle]);
            return val;
        }
         

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public override double Get(Time keyA, SatelliteNumber keyB)
        {
            if (this.Data.ContainsKeyAB(keyA, keyB))
            {
                return base.Get(keyA, keyB);
            }
            if (!this.Data.ContainsKeyB(keyB)) { return double.NaN; }

            var list = this.Data[keyB];
            var time = Time.GetNearst(list.Keys, keyA, MaxTimeDelta,0.000001);
            if (time == Time.Zero)
            {
                return double.NaN;
            }
            return list[time];
        }
    }
}
