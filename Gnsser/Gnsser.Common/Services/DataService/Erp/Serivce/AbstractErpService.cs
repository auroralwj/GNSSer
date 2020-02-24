//2018.05.02, czs, create in hmx, ERP服务接口


using System;
using Gnsser.Service;
using Geo;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// ERP服务接口
    /// </summary>
    public abstract class AbstractErpService : IErpFileService, IService
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 服务的时段信息
        /// </summary>
        public abstract BufferedTimePeriod TimePeriod { get; set; }

        public virtual bool IsEmpty => throw new NotImplementedException();

        /// <summary>
        ///获取服务
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public abstract ErpItem Get(Time time);
    }
}
