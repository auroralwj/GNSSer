//2016.03.22, czs, created in hongqing, 基于卫星结果的管理器 
//2017.03.27, czs, create in hognqing, 泛型表对象设计

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Gnsser; 
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Common;
using Geo;
using Geo.Algorithm;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 指定了键值和数值类型的表对象。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class TypedTableStorage<TKey, TValue> : BaseDictionary<TKey, Dictionary<string, TValue>>
          where TKey : IComparable<TKey>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public TypedTableStorage(string name = "")
            : base(name)
        {
        }

        /// <summary>
        /// 参数
        /// </summary>
        public List<string> ParamNames { get; set; }


        /// <summary>
        /// 缓存的数据,核心存储
        /// </summary>
        //public Dictionary<TKey, Dictionary<string, TValue>> BufferedValues { get; set; }

        /// <summary>
        /// 当前行,保存在数据中的最新行。具体位置决定于采用的方法，如 Add or Insert 
        /// </summary>
        public Dictionary<string, TValue> CurrentRow { get; set; }


    }



}