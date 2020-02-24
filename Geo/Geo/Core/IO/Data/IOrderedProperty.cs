//2015.10.09, czs, create in xi'an hongqing, 具有输出参数的接口

using System;
using System.Collections.Generic;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 顶层参数类
    /// </summary>
    public abstract class OrderedProperty : IOrderedProperty
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderedProperty() { }
            
        /// <summary>
        /// 排好序的属性名称
        /// </summary>
        public virtual List<string> OrderedProperties { get; protected set; }
        /// <summary>
        /// 排好序的属性名称,具有单位
        /// </summary>
       public  List<ValueProperty> Properties{ get; protected set; }
    } 

    /// <summary>
    /// 排好序的属性名称
    /// </summary>
    public interface IOrderedProperty
    {
        /// <summary>
        /// 排好序的属性名称
        /// </summary>
        List<string> OrderedProperties { get; }
        /// <summary>
        /// 排好序的属性名称,具有单位
        /// </summary>
        List<ValueProperty> Properties { get; }
    }
}
