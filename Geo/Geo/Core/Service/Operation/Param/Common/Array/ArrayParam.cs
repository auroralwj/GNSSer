//2015.09.27, czs, create in xi'an hongqing,基线标记文件

using System;
using System.Text;
using Geo.Common;
using Geo.Coordinates;
using Geo;

namespace Geo.IO
{
    /// <summary>
    /// 基线标记文件
    /// </summary>
    public class ArrayParam : RowClass
    {
        public ArrayParam()
        {
            this.OrderedProperties = new System.Collections.Generic.List<string>()
            { 
            };
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Items"></param> 
        public ArrayParam(string[] Items) { this.Items = Items;  }

        public string[] Items { get; set; }

        public int Count { get { return Items.Length; } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Items)
            {
                sb.Append(item + ",");
            }
            return sb.ToString() ;
        }

        public override bool Equals(object obj)
        {
            var o = obj as ArrayParam;
            if (o == null) { return false; }
            
            if(o.Count != this.Count) return false;
            
            int i = 0;
            foreach (var item in Items)
            {
                if (!Items[i].Equals(o.Items[i]))
                    return false;
                i++;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return Items.GetHashCode();
        }
    }
}
