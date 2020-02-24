//2017.06.08，czs, create in hongqing, 字符串关键字构建器

using System;
using System.Text;
using Geo.IO;
using System.Collections.Generic;

namespace Geo
{
    /// <summary>
    /// 字符串关键字构建器
    /// </summary>
    public class StringKeyBuilder : AbstractBuilder<string>
    {
        public StringKeyBuilder()
        {
            this.Splitter = "_";
            this.Markers = new List<object>();
        }

        public event Action<StringKeyBuilder> KeyBuildingEventHandler;

        public string Splitter { get; set; }
        /// <summary>
        /// 标记集合
        /// </summary>
        public List<Object> Markers { get; set; }
        /// <summary>
        /// 增加一个
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public StringKeyBuilder Append(Object obj) { this.Markers.Add(obj); return this; }
        /// <summary>
        /// 最关键的一个。第一个。
        /// </summary>
        public Object KeyMarker { get { if (Markers.Count > 0) return Markers[0]; return null; } set { if (Markers.Count > 0)  Markers[0] = value; else Markers.Add(value); } }

        public void Clear() { this.Markers.Clear(); }

        public override string Build()
        {
            OnKeyBuilding();

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in Markers)
            {
                if (i != 0) { sb.Append(Splitter); }
                sb.Append(item.ToString());
                i++;
            }
            return sb.ToString();
        }

        public void OnKeyBuilding()
        {
            if (KeyBuildingEventHandler != null) { KeyBuildingEventHandler(this); }

        }
    }

}
