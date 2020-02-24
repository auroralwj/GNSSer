//2014.08.19, czs, create
//2015.10.15, czs, edit in 洪庆， 直接获取 EpochInformation，合并 IObservationDataSource 与 IStreamObservationDataSource，以后均采用数据流形式管理和操作数据

using System;
using Geo.Coordinates;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Domain; 
using System.Collections;
using System.Collections.Generic;
using Geo;

namespace Gnsser.Data.Rinex
{
    //2018.07.27, czs, create in HMX, 提取通用观测数据源接口，支持多种类型
    /// <summary>
    /// 通用观测数据源接口。所有观测数据源都应该实现此接口。
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    public interface IObservationStream<TMaterial> : IStreamService<TMaterial>
        where TMaterial : ISiteSatObsInfo
    {
        /// <summary>
        /// 测站信息
        /// </summary>
        ISiteInfo SiteInfo { get; }
        /// <summary>
        /// 观测概略信息
        /// </summary>
        IObsInfo ObsInfo{ get; }

    }

    /// <summary>
    /// 单站通用观测数据源接口。所有观测数据源都应该实现此接口。
    /// 以数据流方式传递的观测数据源。
    /// 这种数据源比较节约内存，但是只能从起始位置一步一步的往下读取数据，不能获知整个数据流大小。
    /// </summary>
    public interface ISingleSiteObsStream : IObservationStream<EpochInformation>
    {
        #region 基本属性

        /// <summary>
        /// 路径
        /// </summary>
        string Path { get; }
        /// <summary>
        /// 导航文件路径
        /// </summary>
        string NavPath { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 获取指定时刻的观测数据。这种获取一般是原始数据，根据需要，采用处理器处理。
        /// </summary>
        /// <param name="gpsTime">时刻</param>
        /// <param name="toleranceSeccond">允许的时间差，单位秒</param>
        /// <returns></returns>
        EpochInformation Get(Time gpsTime, double toleranceSeccond =1e-15); 
        #endregion
    }
}
