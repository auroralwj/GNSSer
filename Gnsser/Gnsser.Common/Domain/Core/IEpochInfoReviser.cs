//2014.10.14，czs, edit, 处理器，输入对象本身改变对象内部结构，属于构造函数的延续。
//2016.03.26, czs, create in hongqing, 缓存历元处理器

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using Gnsser.Data;
using Geo;
using Geo.IO;

namespace Gnsser
{


    /// <summary>
    /// 历元卫星数据处理器。
    /// </summary>
    public interface IEpochSatReviser : IReviser<EpochSatellite>
    {
    }
    /// <summary>
    /// 历元数据处理器。
    /// </summary>
    public interface IEpochInfoReviser : IReviser<EpochInformation>
    {
    }

    /// <summary>
    /// 缓存历元处理器。
    /// </summary>
    public interface IBufferedEpochInfoReviser : IReviser<IWindowData<EpochInformation>>
    {
    }    

}
