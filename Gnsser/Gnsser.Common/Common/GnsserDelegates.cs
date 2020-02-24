//2016.10.19, czs, create in hongqing, 将所有的委托集中于此

using System;
using Geo.Coordinates;
using Gnsser.Data;
using System.Collections.Generic;
using Gnsser.Correction;
using Geo.Times;
using Gnsser.Times;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// 卫星类型改变时激发。
    /// </summary>
    /// <param name="SatelliteType"></param>
    public delegate void SatelliteTypeChangedEventHandler(SatelliteType SatelliteType);



}
