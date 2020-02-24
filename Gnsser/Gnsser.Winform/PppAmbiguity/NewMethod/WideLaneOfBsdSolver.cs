//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, 差分计算器，剥离了MW提取和平滑出去。
//2017.03.21, czs, edit in hognqing,分离提取BsdProductSolver

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser.Winform
{
    /// <summary>
    /// BSD 宽巷计算器.只需要输入平滑后的MW值。
    /// 需要指定基准卫星。
    /// </summary>
    public class WideLaneOfBsdSolver : BsdProductSolver
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="smoothedMwPathes"></param>
        public WideLaneOfBsdSolver(string[] smoothedMwPathes, SatelliteNumber BasePrn, int MinSatCount, int MinEpoch,string OutputDirectory = null)
            : this(ObjectTableManager.Read(smoothedMwPathes), BasePrn, MinSatCount, MinEpoch,  OutputDirectory)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathes"></param>
        public WideLaneOfBsdSolver(ObjectTableManager SmoothedMwValue, SatelliteNumber BasePrn, int MinSiteCount, int MinEpoch, string OutputDirectory = null)
        :base("WL", BasePrn, OutputDirectory){
            this.MinSiteCount = MinSiteCount;
            this.MinEpoch = MinEpoch;
            this.SmoothedMwValue = SmoothedMwValue;
        }

        #region 属性
        /// <summary>
        /// MW 平滑数据
        /// </summary>
        public ObjectTableManager SmoothedMwValue { get; set; }
        
        #region 中间产品
        /// <summary>
        /// 基于测站的宽巷平滑数据的差分结果
        /// </summary>
        public ObjectTableManager SiteDifferMwTables { get; set; }
        #endregion
        #endregion

        /// <summary>
        /// 运行
        /// </summary>
        public override void Run()
        { 
            //----------基于测站--------               
            //基于测站的MW星间单差，消除了测站硬件延迟
            log.Debug("基于测站的MW星间单差");
            this.SiteDifferMwTables = SmoothedMwValue.GetNewByMinusCol(BasePrn+"","",true);

            //----------基于卫星---------
            //基于卫星的MW星间单差，各个测站针对同一卫星差分值显示在同一个表格中
            this.FloatValueTables = SiteDifferMwTables.GetSameColAssembledTableManager("FloatOf" + ProductTypeMarker );

            //生成结果，包括一些数据过滤工作
            base.BuildProducts();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (this.SiteDifferMwTables != null) { SiteDifferMwTables.Dispose(); } 
            base.Dispose();
        }
    } 
}
