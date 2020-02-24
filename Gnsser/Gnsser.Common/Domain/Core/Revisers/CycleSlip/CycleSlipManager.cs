//2016.08.03, czs, create in fujian yongan, 模糊度存储器

using System;
using Geo;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 模糊度存储器
    /// </summary>
    public class CycleSlipManager 
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public CycleSlipManager(GnssProcessOption GnssOption)
        {
            this.CycleSlipeStorage = new InstantValueStorage();
             
            this.GnssOption = GnssOption;
        }

        #region 主要属性 
        /// <summary>
        /// 数据处理选项
        /// </summary>
        GnssProcessOption GnssOption { get; set; } 

        /// <summary>
        /// 周跳存储器
        /// </summary>
        public InstantValueStorage CycleSlipeStorage { get; set; }

        #endregion
         


    }     
}
