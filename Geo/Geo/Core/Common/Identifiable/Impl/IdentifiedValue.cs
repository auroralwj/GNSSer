//2015.09.27，czs, created in hongqing, 具有名称的值，如命名的空间直角坐标
//2016.02.19, czs, create in hongqing, 具有标识的值

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo
{
    public class StringIdValue<TValue> : IdentifiedValue<string, TValue>, IStringId
    {
        
         /// <summary>
        /// 默认构造函数
        /// </summary>
        public StringIdValue() { }
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public StringIdValue(string name, TValue val)
            : base(name, val)
        { 
        }
    }

    public class IntIdValue<TValue> : IdentifiedValue<int, TValue>
    {
         /// <summary>
        /// 默认构造函数
        /// </summary>
        public IntIdValue() { }
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public IntIdValue(int name, TValue val)
            : base(name, val)
        { 
        }
    }

    /// <summary>
    /// 具有名称的值，如命名的空间直角坐标
    /// </summary>
    [Serializable]
    public class IdentifiedValue<TKey,TValue> : BaseValue<TValue>, Identifiable<TKey>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IdentifiedValue() { }
        /// <summary>
        /// 构造函数，为名称赋值。默认为空。
        /// </summary>
        /// <param name="name"></param>
        public IdentifiedValue(TKey name, TValue val) : base(val)
        {
            this.Id = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public TKey Id { get; set; }

        public override string ToString()
        {
            return Id + ":" + Value.ToString();
        }
        /// <summary>
        /// 如果只是一个同名字符串，也会相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var o = obj as IdentifiedValue<TKey, TValue>;
             if(o == null) return false;

             return Id.Equals(o.Id) && base.Equals(o.Value);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
 
}
