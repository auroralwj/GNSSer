//2018.05.27, czs, create in hmx, IGS Code 球谐函数服务提供器


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
    /// IGS Code 球谐函数服务提供器，提供无间断，多系统的星历。
    /// 自动匹配提供。
    /// </summary>
    public class IgsCodeHarmoIonoServiceAutoProvider : IgsServiceAutoProvider< IIonoService>
    {
        protected Log log = new Log(typeof(IgsCodeHarmoIonoServiceAutoProvider));

        /// <summary>
        /// 默认构造函数
        /// </summary> 
        public IgsCodeHarmoIonoServiceAutoProvider()
        { 
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="opt"></param>
        public IgsCodeHarmoIonoServiceAutoProvider(IgsProductSourceOption opt):base(opt)
        { 
        } 
        /// <summary>
        /// 通过给定的时间创建服务
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        protected override IIonoService CreateService(Time epoch)
        {
            var startLatency = IgsProductTimeAvailable.GetLatency(epoch);
            log.Info("当前历元" + epoch + "可以获得IGS " + startLatency + " 产品"); 
            Option.SatelliteTypes = new List<SatelliteType> { SatelliteType.U };
            Option.IsUniqueSource = false;
            var startService  = new IgsCodeHarmoIonoSourceProvider(Option).GetDataSourceService(); ;
            
            return startService;
        }
         
    }
}
