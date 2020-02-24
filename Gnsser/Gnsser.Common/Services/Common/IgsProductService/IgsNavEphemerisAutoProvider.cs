//2018.07.07, czs, create in hmx, IGS 导航星历服务提供器


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
    ///  IGS 导航星历服务提供器
    /// 自动匹配提供。
    /// </summary>
    public class IgsNavEphemerisAutoProvider : IgsServiceAutoProvider< IEphemerisService>
    {
        protected Log log = new Log(typeof(IgsNavEphemerisAutoProvider));

        /// <summary>
        /// 默认构造函数
        /// </summary> 
        public IgsNavEphemerisAutoProvider()
        { 
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="opt"></param>
        public IgsNavEphemerisAutoProvider(IgsProductSourceOption opt):base(opt)
        { 
        } 
        /// <summary>
        /// 通过给定的时间创建服务
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        protected override IEphemerisService CreateService(Time epoch)
        {
          //  var startLatency = IgsProductTimeAvailable.GetLatency(epoch);
          //  log.Info("当前历元" + epoch + "可以获得IGS " + startLatency + " 产品"); 
            Option.SatelliteTypes = new List<SatelliteType> { SatelliteType.U };
            Option.IsUniqueSource = false;
            var startService  = new IgsNavEphemerisSourceProvider(Option).GetDataSourceService(); ;
            
            return startService;
        }
         
    }
}
