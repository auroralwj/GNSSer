//2015.01.08, czs, create in namu shuangliao, 卫星时段信息管理器

using System;
using System.Collections.Generic;
using System.Text; 
using Gnsser.Times;
using Geo;
using Geo.Times; 

namespace Gnsser
{ 
    /// <summary>
    /// 卫星选择标准。权重越大，越应该为标准星。
    /// 注意：排序是按照从小到大的方法排序的。
    /// </summary>
    public class SatPeriodWeight : SatPeriod, IComparable<SatPeriodWeight>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SatPeriodWeight( SatelliteNumber Prn, BufferedTimePeriod Period, int CycleSlipCount)
            : base(Prn, Period)
        { 
            this.CycleSlipCount = CycleSlipCount;
        } 

        /// <summary>
        /// 周跳权重
        /// </summary>
        public int CycleSlipCount { get; set; }

        /// <summary>
        /// 总权重，权重越大，越应该为标准星。
        /// </summary>
        public double TotalWeight
        {
            get
            {
                //让一个周跳等价于 10 分钟，600 秒 
                double weight = TimePeriod.Span * 1 - CycleSlipCount * 600 ;
                return weight;
            }
        } 

        public int CompareTo(SatPeriodWeight other)
        {
            return TotalWeight.CompareTo(other.TotalWeight);
        }

        public override string ToString()
        {
            return Prn + " : " + CycleSlipCount  + ", "+ TimePeriod;
        }


    }
}
