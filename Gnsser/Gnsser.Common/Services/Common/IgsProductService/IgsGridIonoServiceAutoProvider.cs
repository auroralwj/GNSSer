//2018.03.15, czs, create in hmx,  IGS 星历服务提供器，提供无间断，多系统的星历。
//2018.05.02, czs, create in hmx, IGS Erp 服务提供器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Common;
using Gnsser.Service;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times; 
using Geo.IO;
using System.IO;
 

namespace Gnsser.Data
{ 
    /// <summary>
    /// IGS ERP服务提供器，提供无间断，多系统的星历。
    /// 自动匹配提供。
    /// </summary>
    public class IgsGridIonoServiceAutoProvider : IgsServiceAutoProvider< IGridIonoFileService>
    {
        protected Log log = new Log(typeof(IgsClockServiceAutoProvider));

        /// <summary>
        /// 默认构造函数
        /// </summary> 
        public IgsGridIonoServiceAutoProvider()
        { 
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="opt"></param>
        public IgsGridIonoServiceAutoProvider(IgsProductSourceOption opt):base(opt)
        { 
        } 
        /// <summary>
        /// 通过给定的时间创建服务
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        protected override IGridIonoFileService CreateService(Time epoch)
        {
            var startLatency = IgsProductTimeAvailable.GetLatency(epoch);
            log.Info("当前历元" + epoch + "可以获得IGS " + startLatency + " 产品");
               IgsProductType igsProduct = IgsProductType.I;
            Option.SatelliteTypes = new List<SatelliteType> { SatelliteType.U };
            var startService  = new IgsGridIonoSourceProvider(Option, igsProduct).GetDataSourceService(); ;
           

            return startService;
        }
         
    }
}
