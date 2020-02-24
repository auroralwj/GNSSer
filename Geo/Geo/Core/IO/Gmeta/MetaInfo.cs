//2015.12.25, czs, create in xi'an hongqing, 元数据信息顶层类! 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 元数据，用于调试，查看等。
    /// </summary>
    public class MetaInfo : BaseDictionary<string, string>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="name">谁的元数据</param>
        public MetaInfo(string name) { this.Name = name; } 

      



    }
}
