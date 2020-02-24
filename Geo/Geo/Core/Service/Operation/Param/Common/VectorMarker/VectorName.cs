//2015.09.27, czs, create in xi'an hongqing,基线标记文件

using System;
using Geo.Common;
using Geo.Coordinates;
using Geo;

namespace Geo.IO
{
    /// <summary>
    /// 基线标记文件
    /// </summary>
    public class VectorName : RowClass
    {
        public VectorName()
        {
            this.OrderedProperties = new System.Collections.Generic.List<string>()
            {
                Geo.Utils.ObjectUtil.GetPropertyName<VectorName>(m=>m.StartName),
                Geo.Utils.ObjectUtil.GetPropertyName<VectorName>(m=>m.EndName), 
            };
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="starName"></param>
        /// <param name="endName"></param>
        public VectorName(string starName, string endName) { this.StartName = starName; this.EndName = endName; }

        /// <summary>
        /// 起始的名称
        /// </summary>
        public string StartName { get; set; }
        /// <summary>
        /// 目标的名称
        /// </summary>
        public string EndName { get; set; }

        public override string ToString()
        {
            return StartName + "-" + EndName;
        }
        public override bool Equals(object obj)
        {
            var o = obj as VectorName;
            if (o == null) { return false; }
            return (o.StartName == StartName && o.EndName == EndName);
        }

        public override int GetHashCode()
        {
            return StartName.GetHashCode() + EndName.GetHashCode();
        }
    }
}
