//2014.08.19, czs, create, 以数据流方式传递的观测数据源
//2014.12.27, czs, refactor in namu, 提取抽象方法
//2015.10.15, czs, edit in hongqing, 重新设计接口，顶层观测数据源抽象类
//2015.10.26, czs, edit in hongqing, 实现数据流服务

using System;
using System.Collections;
using System.Collections.Generic;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Domain;
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 顶层观测数据源抽象类。
    /// </summary>
    public abstract class AbstractObservationStream : 
        AbstractStreamService<EpochInformation>, 
        ISingleSiteObsStream
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbstractObservationStream()
        {
          //  IsEndTimeAvailable = true;
        }

        #region 属性
        /// <summary>
        /// 观测概略信息，
        /// </summary>
        public IObsInfo ObsInfo { get; set; }
        /// <summary>
        /// 测站信息
        /// </summary>
        public ISiteInfo SiteInfo { get; set; } 
        #endregion

        #region 方法
        /// <summary>
        /// 获取,顾及了舍入时差
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="toleranceSeccond"></param>
        /// <returns></returns>
        public abstract Domain.EpochInformation Get(Time gpsTime, double toleranceSeccond = 1e-15);
        
        #endregion
         
        /// <summary>
        /// 观测数据路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 导航文件路径
        /// </summary>
        public string NavPath { get; set; }
         
    }
}
