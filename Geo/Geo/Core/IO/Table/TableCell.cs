//2017.03.09, czs, create in hongqing, 表格数据

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
    /// 表格数据
    /// </summary>
    public class TableCell
    {
        /// <summary>
        /// 行对应的检索值
        /// </summary>
        //public object IndexValue { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// 行编号，从0开始。
        /// </summary>
        public int RowNumber { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 数据承载用。
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// 是否值为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Value == null;
        }

        public override string ToString()
        {
            return ColName + "[" + RowNumber + "]: "+ Value;
        }
    }
}