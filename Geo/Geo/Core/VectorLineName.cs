//2018.11.28, czs, create in hmx, 基线名称
//2019.01.12, czs, create in hmx, 向量名称

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Geo;

namespace Geo
{
    /// <summary>
    /// 向量名称
    /// </summary>
    public class VectorLineName
    {
        public VectorLineName()
        {

        }
        public VectorLineName(string Start, string End)
        {
            this.Start = Start;
            this.End = End;
            this.Name = Start + BaseLineSplitter + End;
        }

        /// <summary>
        /// 基线名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  参考站名称
        /// </summary>
        public string Start { get; set; }
        /// <summary>
        ///  流动站名称
        /// </summary>
        public string End { get; set; }
        /// <summary>
        /// 反转
        /// </summary>
        public VectorLineName Reversed => new VectorLineName(End, Start);
        /// <summary>
        /// 基线分割字符串
        /// </summary>
        public const string BaseLineSplitter = "→";// Gnsser.ParamNames.BaseLinePointer;
        /// <summary>
        /// 基线名称解析，分解
        /// </summary>
        /// <param name="baseLineName"></param>
        /// <returns></returns>
        public static string[] GetRefRovName(string baseLineName)
        {
            return baseLineName.Split(new string[] { BaseLineSplitter }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 是否包含测站
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public bool Contains(string siteName)
        {
            return Start == siteName || End == siteName;
        }
        /// <summary>
        /// 是否包含测站
        /// </summary>
        /// <param name="refName"></param>
        /// <param name="rovName"></param>
        /// <param name="reverseAble"></param>
        /// <returns></returns>
        public bool Contains(string refName, string rovName, bool reverseAble = true)
        {
            if (!reverseAble)
            {
                return Start == refName && End == rovName;
            }

            return (Start == refName && End == rovName) || (Start == rovName && End == refName);
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is VectorLineName)) { return false; }
            var o = obj as VectorLineName;

            return o.Start == this.Start && o.End == this.End;
        }

        /// <summary>
        /// 哈希表
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Start.GetHashCode() * 13 + End.GetHashCode() * 5;
        }

    }
}
