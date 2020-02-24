// 2014.10.24, czs, create in namu shuangliao, 提取接口，一切皆服务

using System;
using Gnsser.Times;
using Geo;
using Geo.Times; 

namespace Gnsser.Data
{
    /// <summary>
    /// 天线服务接口
    /// </summary>
    public interface IAntennaFileService : IService<Antenna, string>
    {
        /// <summary>
        /// 获取一个天线对象。
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="epoch"></param>
        /// <returns></returns>
       Antenna Get(string serial, Time epoch); 
    }
}
