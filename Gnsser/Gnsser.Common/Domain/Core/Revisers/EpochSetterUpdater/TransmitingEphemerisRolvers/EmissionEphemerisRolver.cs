//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;
using Geo.IO;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// 信号发射时刻的卫星位置的计算。
    /// </summary>
    public abstract class EmissionEphemerisRolver : AbstractService<IEphemeris>
    {
        public EmissionEphemerisRolver(IEphemerisService EphemerisService, DataSourceContext DataSouceProvider, EpochSatellite sat)
        {
            this.Name = "信号发射时刻的卫星位置的计算";
            this.EphemerisService = EphemerisService;
            this.DataSouceProvider = DataSouceProvider;
            this.EpochSat = sat;

            log = Log.GetLog(this);
        }

        protected ILog log;
        protected DataSourceContext DataSouceProvider;
        /// <summary>
        /// 测站卫星向量信息
        /// </summary>
        protected EpochSatellite EpochSat { get; set; }
        /// <summary>
        /// 星历服务
        /// </summary>
        public IEphemerisService EphemerisService { get; set; }  


        /// <summary>
        /// 获取最终的产品，如果失败则返回默认对象，通常为null。每次生产都要执行。
        /// </summary>
        /// <returns></returns>
        public abstract IEphemeris Get();
    }
}
