//2016.02.10, czs, create in hongqing, 具有名称的值，并且为表的一行。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo
{
    public class StrIdValueRow<TValue> : IdentifiedValueRow<string, TValue>, IStringId
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public StrIdValueRow() { }
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public StrIdValueRow(string name, TValue val)
            : base(name, val)
        {
        }

    }


    public class IdentifiedValueRow<TKey, TValue> : IdentifiedValue<TKey, TValue>, IRowClass
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IdentifiedValueRow() { }
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public IdentifiedValueRow(TKey name, TValue val)
            : base(name, val)
        {
        }
        /// <summary>
        /// 具有排序的属性名称列表
        /// </summary>
        public List<string> OrderedProperties { get; protected set; }

        /// <summary>
        ///  具有数值单位的属性列表
        /// </summary>
        public List<IO.ValueProperty> Properties { get; protected set; }
    }


    /// <summary>
    /// 具有名称的值，并且为表的一行。
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class NamedValueRow<TValue> : NamedValue<TValue>, IRowClass
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public NamedValueRow() { }
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public NamedValueRow(string name, TValue val)
            : base(name, val)
        {
        }
        /// <summary>
        /// 具有排序的属性名称列表
        /// </summary>
        public List<string> OrderedProperties { get; protected set; }

        /// <summary>
        ///  具有数值单位的属性列表
        /// </summary>
        public List<IO.ValueProperty> Properties { get; protected set; }
    }
    #region  单值
    /// <summary>
    /// 单值行，只有名称和数值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SingleValueRow<TValue> : StrIdValueRow<TValue>
    {
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public SingleValueRow(string name, TValue val)
            : base(name, val)
        {
            OrderedProperties = new List<string>() { "Id", "Value" };
        }
    }
    /// <summary>
    /// 双精度浮点数单行
    /// </summary>
    public class IdFloatRow : SingleValueRow<Double>
    {
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public IdFloatRow(string name, Double val)
            : base(name, val)
        {
        }
    }
    #endregion

    #region 双值

    /// <summary>
    /// 双值行，只有名称和2个数值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class TwoValueRow<TValue> : SingleValueRow<TValue>
    {
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public TwoValueRow(string name, TValue val, TValue val2)
            : base(name, val)
        {
            OrderedProperties = new List<string>() { "Id", "Value", "Value2", };
            this.Value2 = val2;
        }

        /// <summary>
        /// 第二个值
        /// </summary>
        public TValue Value2 { get; set; }
    }


    /// <summary>
    /// 双精度浮点数单行
    /// </summary>
    public class IdTwoFloatRow : TwoValueRow<Double>
    {
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public IdTwoFloatRow(string name, Double val, double val2)
            : base(name, val, val2)
        {
        }
    }
    #endregion

    #region 三值

    /// <summary>
    /// 3值行，只有名称和3个值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ThreeValueRow<TValue> : TwoValueRow<TValue>
    {
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public ThreeValueRow(string name, TValue val, TValue val2, TValue val3)
            : base(name, val, val2)
        {
            OrderedProperties = new List<string>() { "Id", "Value", "Value2", "Value3" };
            this.Value3 = val3;
        }

        /// <summary>
        /// 第3个值
        /// </summary>
        public TValue Value3 { get; set; }
    }


    /// <summary>
    /// 3个双精度浮点数单行
    /// </summary>
    public class IdThreeFloatRow : ThreeValueRow<Double>
    {
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public IdThreeFloatRow(string name, Double val, double val2, double val3)
            : base(name, val, val2, val3)
        {
        }
    }
    #endregion
}
