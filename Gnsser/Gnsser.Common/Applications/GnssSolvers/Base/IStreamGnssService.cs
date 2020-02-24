//2014.08.26, czs, edit, 改进单点定位计算接口
//2015.12.25, czs, edit in hongqing, 丰富了属性内容，便于查看结果

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;
using Geo;
using Gnsser.Checkers;

namespace Gnsser.Service
{
    /// <summary>
    /// 单点定位
    /// </summary>
    public interface IStreamGnssService : IStreamService<SingleSiteGnssResult, EpochInformation>
    {
        /// <summary>
        /// 定位计算选项
        /// </summary>
        GnssProcessOption PositionOption { get; set; }

        /// <summary>
        /// 数据源上下文。
        /// </summary>
        DataSourceContext DataSourceContext { get; set; }

        SingleSiteGnssSolver GnssSolver { get; set; }
        /// <summary>
        /// 测站信息
        /// </summary>
        //ISiteInfo SiteInfo { get; }

        /// <summary>
        /// 处理一个已经完成初始化的历元
        /// </summary>
        /// <param name="epochInfo">一个历元的观测数据</param>
       //PositionResult Produce(EpochInformation epochInfo);
       /// <summary>
       /// 平差计算器
       /// </summary>
       //Adjustment Adjustment { get;  }

       /// <summary>
       /// 历元信息处理器，主要对历元信息进行完善，如星历赋值、误差改正、组合项生成等。revise
       /// </summary>
       //BaseEpochInfoReviser EpochInfoReviser { get; set; }

       /// <summary>
       /// 矩阵生成器
       /// </summary>
       SingleSiteGnssMatrixBuilder MatrixBuilder { get; set; } 
    }

}
