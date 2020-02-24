//2015.12.22, czs, create in hongqing, ERP服务接口


using System;
using Gnsser.Service;
using Geo;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// ERP服务接口
    /// </summary>
    public interface IErpFileService :  ITimedService<BufferedTimePeriod> 
    {
        /// <summary>
        /// 服务是否为空
        /// </summary>
        bool IsEmpty { get; }
        ErpItem Get(Time time);
    }
}
