//2018.11.28, czs, create in hmx, 基线名称

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
    /// 简易基线名称
    /// </summary>
    public class BaseLineName
    {
        /// <summary>
        /// 基线名称，流动站减去基准站
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 向量名称，基准站指向流动站
        /// </summary>
        public string VectorName => RefName + "→" + RovName;
        /// <summary>
        ///  参考站名称
        /// </summary>
        public string RefName { get; set; }
        /// <summary>
        ///  流动站名称
        /// </summary>
        public string RovName { get; set; }

        /// <summary>
        /// 基线分割字符串
        /// </summary>
        public const string BaseLineSplitter = "-";// Gnsser.ParamNames.BaseLinePointer;
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
            return RefName == siteName || RovName == siteName;
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
                return RefName == refName && RovName == rovName;
            }

            return (RefName == refName && RovName == rovName) || (RefName == rovName && RovName == refName);
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
            if (!(obj is BaseLineName)) { return false; }
            var o = obj as BaseLineName;

            return o.RefName == this.RefName && o.RovName == this.RovName;
        }

        /// <summary>
        /// 哈希表
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return RefName.GetHashCode() * 13 + RovName.GetHashCode() * 5;
        }


    }


}
