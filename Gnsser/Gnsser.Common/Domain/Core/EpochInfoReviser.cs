//2014.10.14，czs, create in hailutu, 星历处理器，输入对象本身改变对象内部结构，属于构造函数的延续。

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Geo;
using Geo.Utils;
using Geo.Common;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.IO;

namespace Gnsser
{
    //2017.08.06, czs, edit in hongqing, IEphemerisProcessor 命名为 IEphemerisReviser
    /// <summary>
    /// 星历处理器,星历改正等。
    /// </summary>
    public interface IEphemerisReviser : IReviser<IEphemeris>
    { 
    }

    /// <summary>
    /// 历元数据处理器。对历元信息进行赋值、过滤、检核等。
    /// </summary>
    public abstract class EpochSatReviser : Reviser<EpochSatellite>, IEpochSatReviser
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EpochSatReviser()
        {
            log = Log.GetLog(typeof(EpochSatReviser));
        }
    }

    /// <summary>
    /// 历元数据处理器。对历元信息进行赋值、过滤、检核等。
    /// </summary>
    public abstract class EpochInfoReviser : Reviser<EpochInformation>, IEpochInfoReviser
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EpochInfoReviser()
        {
            log = Log.GetLog(typeof(EpochInfoReviser));
        }
    }

    /// <summary>
    /// 历元数据处理器。对历元信息进行赋值、过滤、检核等。
    /// </summary>
    public abstract class BufferedEpochInfoProcessor : Reviser<IWindowData<EpochInformation>>, IBufferedEpochInfoReviser
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BufferedEpochInfoProcessor()
        {
            log = Log.GetLog(typeof(BufferedEpochInfoProcessor));
        }



    }
}
