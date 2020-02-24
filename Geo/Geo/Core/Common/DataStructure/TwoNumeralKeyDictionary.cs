//2017.07.22, czs, create in hongqing, 双键字典

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;

namespace Geo
{
    /// <summary>
    /// 双键字典,采用双字典快速检索方法。
    /// </summary>
    public class TwoNumeralKeyDictionary<TValue> : TwoKeyDictionary<double, double, TValue>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TwoNumeralKeyDictionary()
            : base()
        {
        }
        /// <summary>
        /// 初始化统计参数
        /// </summary>
        public virtual void Init()
        {
            var keyAList = this.KeyAs;
            keyAList.Sort();
            this.IntervalA = Math.Abs( keyAList[1] - keyAList[0]);

            var keyBList = this.KeyBs;
            keyBList.Sort();
            this.IntervalB = Math.Abs(keyBList[1] - keyBList[0]);

            if (KeyASpan == null) { KeyASpan = new NumerialSegment(this.KeyAs[0]); }
            this.KeyAs.ForEach(m => KeyASpan.CheckOrExpand(m));

            if (KeyBSpan == null) { KeyBSpan = new NumerialSegment(this.KeyBs[0]); }       
            this.KeyBs.ForEach(m => KeyBSpan.CheckOrExpand(m));            
        }
        /// <summary>
        /// 关键字A的范围。对应行标识。
        /// </summary>
        public NumerialSegment KeyASpan { get; set; }
        /// <summary>
        /// 关键字B的范围。对应列标识。
        /// </summary>
        public NumerialSegment KeyBSpan { get; set; }
        /// <summary>
        /// 关键字大小，从0开始计算。
        /// </summary>
        public XY KeySize { get { return new XY(KeyASpan.Span, KeyBSpan.Span); } }
        /// <summary>
        /// A 、行关键字的间距
        /// </summary>
        public double IntervalA { get; set; }
        /// <summary>
        ///  B、列关键字的间距
        /// </summary>
        public double IntervalB { get; set; }
        /// <summary>
        /// 左下角，即A、B最小值。
        /// </summary>
        public XY LeftDown { get { return new XY(KeyASpan.Start, KeyBSpan.Start); } }
    }

    /// <summary>
    /// 双键字典,采用双字典快速检索方法。
    /// </summary>
    public class TwoNumeralKeyAndValueDictionary : TwoNumeralKeyDictionary<double>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TwoNumeralKeyAndValueDictionary()
            : base()
        {
        }
        /// <summary>
        /// 最大最小数值范围
        /// </summary>
        public NumerialSegment ValueSpan { get; set; }

       
        /// <summary>
        /// 初始化，计算参数。
        /// </summary>
        public override void Init()
        {
            base.Init();

            double maxVal = double.MinValue;
            double minVal = double.MaxValue;
            foreach (var keys in this.KeyAs)
            {
                foreach (var kv in this.GetValuesByKeyA(keys))
                {
                    var val = kv.Value;
                    if (maxVal < val) { maxVal = val; }
                    if (minVal > val) { minVal = val; }
                }
            }

            ValueSpan = new NumerialSegment(minVal, maxVal);
        }

       
    }
}