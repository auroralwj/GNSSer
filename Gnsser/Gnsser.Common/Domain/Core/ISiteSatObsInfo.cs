
//2016.05.05, czs, create , 指定时刻的可用卫星

using System;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;

namespace Gnsser
{ 
    /// <summary>
    /// 指定时刻的可用卫星
    /// </summary>
    public interface ISiteSatObsInfo : IToTabRow, IDisposable, Namable
    {
        /// <summary>
        /// 卫星系统类型
        /// </summary>
        List<SatelliteType> SatelliteTypes { get; }
        /// <summary>
        /// 可用卫星
        /// </summary>
        List<SatelliteNumber> EnabledPrns { get; }

        /// <summary>
        /// 信号接收时刻，接收机记录的时刻。
        /// </summary>
        Time ReceiverTime { get; }
        /// <summary>
        /// 历元列表
        /// </summary>
        List<Time> Epoches { get; }
        /// <summary>
        /// 可用卫星的数量
        /// </summary>
        int EnabledSatCount { get; }
        /// <summary>
        /// 是否具有周跳，如果有一个测站具有周跳，则认为有。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        bool HasCycleSlip(SatelliteNumber prn);

        /// <summary>
        /// 标记为不稳定的卫星，具有周跳的卫星的编号，或异常的卫星，或初次出现的卫星。
        /// </summary>
        List<SatelliteNumber> UnstablePrns { get; }
        /// <summary>
        /// 记录已经移除的卫星编号
        /// </summary>
        List<SatelliteNumber> RemovedPrns { get; }
        /// <summary>
        /// 移除周跳标记
        /// </summary>
        void RemoveUnStableMarkers();
        /// <summary>
        /// 获取具有星历的站星列表。
        /// </summary>
        /// <returns></returns>
        List<EpochSatellite> GetEpochSatWithEphemeris();
    }

}