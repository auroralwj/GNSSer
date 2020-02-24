//2016.03.22, czs, created in hongqing, 基于卫星结果的管理器 
//2016.03.29, czs, edit in hongqing, 名称修改为 NamedValueTableManager
//2016.08.05, czs, edit in fujian yongan, 重构
//2016.10.03, czs, edit in hongqing,增加缓存结果数量控制，便于控制内存大小
//2016.10.19, czs, edit in hongqing, 增加一些计算分析功能
//2016.10.26, czs, edit in hongqing, 表格值从字符串修改为 Object，减少转换损失
//2018.05.27, czs, create in HMX, 参数数值表格管理器算工具

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
using Geo.Times;
using Geo.Algorithm;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 类型检索管理器
    /// </summary>
    public class TypedIndexHelper
    {
        Log log = new Log(typeof(TypedIndexHelper));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        public TypedIndexHelper(ObjectTableStorage table)
        {
            this.Table = table;

            double interval = 10;
            if (table.FirstIndex is Time)
            {
                IsTime = true;
                interval = (Time)table.SecondIndex - (Time)table.FirstIndex;
            }
            if (table.FirstIndex is DateTime)
            {
                IsDataTime = true;
                interval = ((DateTime)table.SecondIndex - (DateTime)table.FirstIndex).TotalSeconds;
            }
            this.Interval = DoubleUtil.GetInterval(interval, 0, 10);
            this.BaseIndexObject = table.FirstIndex;
        }

        /// <summary>
        /// 表对象
        /// </summary>
        public ObjectTableStorage Table { get; set; }
        /// <summary>
        /// 是否 Time
        /// </summary>
        public bool IsTime { get; set; }
        /// <summary>
        /// 是否 DataTime
        /// </summary>
        public bool IsDataTime { get; set; }

        /// <summary>
        /// 间隔
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// 第一个或基础索引对象，默认为第一个，可以手动设置
        /// </summary>
        public object BaseIndexObject { get; set; }
        /// <summary>
        /// 设置当前的基础检索对象
        /// </summary>
        /// <param name="BaseIndexObject"></param>
        /// <returns></returns>
        public TypedIndexHelper SetBaseIndexObject(object BaseIndexObject) { this.BaseIndexObject = BaseIndexObject; return this; }

        /// <summary>
        ///获取键值和第一个值的差值。
        /// </summary>
        /// <param name="indexValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public double GetDifferKeyValue(object indexValue, int index)
        {
            double k = index;
            if (IsTime)
            {
                k = ((Time)indexValue) - (Time)(BaseIndexObject);
            }
            if (IsDataTime)
            {
                k = (((DateTime)indexValue) - (DateTime)(BaseIndexObject)).TotalSeconds;
            }
            return k;
        }

    }
}