using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Winform
{
    /// <summary>
    /// 用于显示匹配条件的 Combox 列表值。
    /// </summary>
    public class ComboxOperation
    {
        public Restriction Restriction { get; set; }

        public override string ToString()
        {
            string name = "";
            switch (Restriction)
            {
                case Winform.Restriction.NotEq:
                    name = "不等于"; break;
                case Winform.Restriction.Between:
                    name = "介于之间"; break;
                case Winform.Restriction.Is:
                    name = "是"; break;
                case Winform.Restriction.Eq:
                    name = "等于"; break;
                case Winform.Restriction.Gt:
                    name = "大于"; break;
                case Winform.Restriction.Like:
                    name = "模糊匹配"; break;
                case Winform.Restriction.Lt:
                    name = "小于"; break;
                default:
                    name = "不可能哦，看到我请联系管理员"; break;

            }
            return name;
        }

        public override int GetHashCode()
        {
            return (int)Restriction * 13;
        }

        public override bool Equals(object obj)
        {
            ComboxOperation o = obj as ComboxOperation;
            if (o == null) return false;
            return Restriction == o.Restriction;
        }

        public static IList<ComboxOperation> GetOperateList(List<Restriction> list)
        {
            List<ComboxOperation> items = new List<ComboxOperation>();
            foreach (Restriction r in list)
            {
                items.Add(new ComboxOperation() { Restriction = r });
            }
            return items;
        }
    }

}
