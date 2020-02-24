//2014.10.24, czs, edit in namu shuangliao, 实现 IClockBiasService 接口
//2016.02.05, czs, edit in hongqing, 时段属性源自File
//2018.05.02, czs, edit in hmx, 提取抽象钟差服务类
//2018.05.08, czs, edit in hmx, 分出简单的钟差服务

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm;
using Gnsser.Service;
using Geo.Times;
using Geo;

namespace Gnsser.Data
{

    /// <summary>
    /// 抽象的钟差服务类
    /// </summary>
    public abstract class AbstractClockService : AbstractClockService<AtomicClock>, IClockService
    {

    }

    /// <summary>
    /// 抽象的钟差服务类
    /// </summary>
    public abstract class AbstractSimpleClockService : AbstractClockService<SimpleClockBias>, ISimpleClockService
    {

        public virtual List<SatelliteType> SatelliteTypes { get; set; }// => new List<SatelliteType>();
    }

    /// <summary>
    /// 抽象的钟差服务类
    /// </summary>
    public abstract class AbstractClockService<TAtomicClock> : MultiSatProductService<TAtomicClock>, IClockService<TAtomicClock>
        where TAtomicClock : ISimpleClockBias
    { 
    }







}