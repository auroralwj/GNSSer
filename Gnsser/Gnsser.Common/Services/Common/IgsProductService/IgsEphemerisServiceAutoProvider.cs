//2018.03.15, czs, create in hmx,  IGS 星历服务提供器，提供无间断，多系统的星历。

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
    /// IGS 星历服务提供器，提供无间断，多系统的星历。
    /// 自动匹配提供。
    /// </summary>
    public class IgsEphemerisServiceAutoProvider : IgsServiceAutoProvider<IEphemerisService>
    {
        protected Log log = new Log(typeof(IgsEphemerisServiceAutoProvider));

        /// <summary>
        /// 默认构造函数
        /// </summary> 
        public IgsEphemerisServiceAutoProvider(int InerpolateOrder= 10, bool IsConnectIgsProduct =true)
        {
            this.InerpolateOrder = InerpolateOrder;
            this.IsConnectIgsProduct = IsConnectIgsProduct;
        }
        /// <summary>
        /// 插值阶次
        /// </summary>
        public int InerpolateOrder { get; set; }
        public bool IsConnectIgsProduct { get; set; }
         

        /// <summary>
        /// 通过给定的时间创建服务
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        protected override IEphemerisService CreateService(Time epoch)
        {
            Option.InterpolateOrder = InerpolateOrder;
            Option.IsConnectIgsProduct = IsConnectIgsProduct;

            if(epoch < Time.MinValue)
            {
                log.Error("时间计算错误！不提供 小于 " + Time.MinValue  + " 的服务！");
                return null;
            }


            var startLatency = IgsProductTimeAvailable.GetLatency(epoch);
            log.Info("当前历元"+ epoch +"可以获得IGS " + startLatency + " 产品");
            IEphemerisService startService = null;
            switch (startLatency)
            {
                case IgsProductLatency.Final:
                    startService = new IgsEphemerisSourceProvider(Option, IgsProductType.Sp3).GetDataSourceService();
                    break;
                case IgsProductLatency.Rapid:
                    startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igr_Sp3).GetDataSourceService();
                    break;
                default:
                    startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igu_Sp3).GetDataSourceService();
                    break;
            }
            //如果最终产品没有获取到，则尝试快速产品
            if (startService == null && startLatency == IgsProductLatency.Final)
            {
                startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igr_Sp3).GetDataSourceService();
            }

            //最后尝试预报产品,获取失败则回退
            if (startService == null)
            {
                startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igu_Sp3).GetDataSourceService();
                log.Warn("星历获取失败，再试超快星历， " + this.Option.TimePeriod);

                //最后尝试预报产品
                if (startService == null)
                {
                    //时间减去6小时候再试
                    this.Option.TimePeriod = this.Option.TimePeriod - TimeSpan.FromHours(6);
                    log.Warn("星历获取失败，时间减去6小时候再试超快星历， " + this.Option.TimePeriod);
                    startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igu_Sp3).GetDataSourceService();
                }
                //最后尝试预报产品
                if (startService == null)
                {
                    //时间减去6小时候再试
                    this.Option.TimePeriod = this.Option.TimePeriod - TimeSpan.FromHours(12);
                    log.Warn("星历获取失败，时间减去6小时候再试超快星历， " + this.Option.TimePeriod);
                    startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igu_Sp3).GetDataSourceService();
                }
            }
             
            return startService;
        }
    }
}
