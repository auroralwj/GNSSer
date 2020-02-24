//2019.03.04, czs, create in hongqing, 表格数据服务

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
    /// 基于表数据的服务
    /// </summary>
    public abstract class ObjectTableBasedService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        public ObjectTableBasedService(ObjectTableStorage table)
        {
            this.Storage = table;
        }

        /// <summary>
        /// 存储器
        /// </summary>
        public ObjectTableStorage Storage { get; set; }

    }


}