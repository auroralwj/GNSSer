//2015.09.27，czs, created in hongqing, 具有名称的值，如命名的空间直角坐标

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo
{
    /// <summary>
    /// 具有名称的值，如命名的空间直角坐标
    /// </summary>
    [Serializable]
    public class NamedValue<TValue>  : BaseValue<TValue> , Namable
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public NamedValue() { }
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public NamedValue(string name, TValue val) : base(val)
        {
            this.Name = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name + ":" + Value.ToString();
        }
        /// <summary>
        /// 如果只是一个同名字符串，也会相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            NamedValue<TValue> o = obj as NamedValue<TValue> ;
             if(o == null) return false;

            return Name.Equals(o.Name) && base.Equals(o.Value);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
 
}
