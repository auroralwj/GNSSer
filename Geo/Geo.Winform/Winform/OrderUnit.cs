using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
namespace Geo.Winform
{
    /// <summary>
    /// 排序单元。
    /// </summary>
    public class OrderUnit
    {
        /// <summary>
        /// 绑定的属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 顺序或是逆序
        /// </summary>
        public bool IsAsc { get; set; }

        public override bool Equals(object obj)
        {
            OrderUnit o = obj as OrderUnit;
            if (o == null) return false;

            return Name == o.Name && o.IsAsc == IsAsc;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name + " " + (IsAsc ? "ASC" : " DESC");
        }

    }
}