//2016.10.17, czs, create in hongqing, 管道滤波器
//2017.02.11 5:30, edit in hongqing, 增加取余，避免大量循环
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;

namespace Geo
{
    /// <summary>
    /// 周期滤波管理器
    /// </summary>
    public class PeriodPipeFilterManager : BaseDictionary<string, PeriodPipeFilter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Period"></param>
        /// <param name="ReferenceValue"></param>
        public PeriodPipeFilterManager(double Period, double ReferenceValue = 0)
        { 
            this.Period = Period;
            this.ReferenceValue = ReferenceValue; 
        }



        #region 属性
        /// <summary>
        /// 数字变换周期，即可以通过加减方式将数据放到管道中来。
        /// </summary>
        public double Period { get; set; } 
        /// <summary>
        /// 参考结果。滤波结果应该与其在半个周期内，同时也在管道内。
        /// </summary>
        public double ReferenceValue { get; set; } 
        #endregion
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override PeriodPipeFilter Create(string key)
        {
            return new PeriodPipeFilter(Period, ReferenceValue);
        }

        /// <summary>
        /// 采用已有表最后的数据进行初始化各个子过滤器
        /// </summary>
        /// <param name="product"></param>
        public void Init(ObjectTableStorage product)
        {
           var dic =  product.GetLastValueOfAllCols();
           foreach (var item in dic)
           {
               this.Add(item.Key, new PeriodPipeFilter(Period, item.Value));
           }
        }
    }

    /// <summary>
    /// 管道滤波器
    /// </summary>
    public class PeriodPipeFilter<T>
    {


    }



    /// <summary>
    /// 周期性数字管道滤波,将数据统一到指定大小的管道中来。
    /// 数值与给定的参考值差在半周内。
    /// </summary>
    public class PeriodPipeFilter : PeriodPipeFilter<double>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ReferenceValue">参考数值</param>
        /// <param name="Period">周期</param>
        public PeriodPipeFilter(double Period, double ReferenceValue = 0)
        { 
            this.Period = Period;
            if (!Geo.Utils.DoubleUtil.IsValid(ReferenceValue)) { ReferenceValue = Period * 0.5;  }
            this.ReferenceValue = ReferenceValue;
            this.SemiPeriod = Period / 2.0;
            this.Index = -1;
        }
        /// <summary>
        /// 对齐周数改变了
        /// </summary>
        public event IntEventHandler CutingPeriodCountChanged;
        /// <summary>
        /// 改变了
        /// </summary>
        /// <param name="periodCount"></param>
        protected void OnCutingPeriodCountChanged(int periodCount)
        {
            if (CutingPeriodCountChanged != null) { CutingPeriodCountChanged(periodCount); }
        }
        #region 属性 
             
        /// <summary>
        /// 数字变换周期，即可以通过加减方式将数据放到管道中来。
        /// </summary>
        public double Period { get; protected set; }
        /// <summary>
        /// 半个周期
        /// </summary>
        public double SemiPeriod { get; set; }
        /// <summary>
        /// 参考结果。滤波结果应该与其在半个周期内，同时也在管道内。
        /// </summary>
        public double ReferenceValue { get; set; }
        /// <summary>
        /// 滤波次数
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 最后一个被减去的周期数量。
        /// </summary>
        public int LastCutingPeriodCount { get;  set; }
        #endregion

        /// <summary>
        /// 滤波并返回结果。结果与指定值差在半周内。
        /// 注意，如果多次使用，可能回造成趋势性漂移。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public double Filter(double val)
        {
            if (!Geo.Utils.DoubleUtil.IsValid(val)) { return double.NaN; }

            Index++;

            var result = val;
            var periods = (long)((result - ReferenceValue) / Period); //与参考数据之差的周期
            result -= periods;

            var differ = (result - ReferenceValue); //周期内的余数，对于大数据，可以避免大量循环

            while (differ > SemiPeriod)
            { 
                result -= Period;
                differ = result - ReferenceValue;
            }

            while (differ < -SemiPeriod)
            {
                result += Period;
                differ = result - ReferenceValue;
            }

            //更新参考数据
            // this.ReferenceValue = (ReferenceValue + result) / 2.0;
            this.ReferenceValue += (result - this.ReferenceValue) / (1 + Index); //多个结果的平均
            var newCutingPeriodCount = (int)(val - result);
            if (newCutingPeriodCount != this.LastCutingPeriodCount) { this.OnCutingPeriodCountChanged(newCutingPeriodCount); }
            this.LastCutingPeriodCount = newCutingPeriodCount;

            return result;
        }
    }
}
