//2014.08.26, czs, create, 抽象化单点定位计算
//2014.09.16, czs, refactor, 梳理各个过程，分为历元算前、算中和算后，增加初始化、检核等方法。
//2014.10.06，czs, edit in hailutu, 将EpochInfomation的构建独立开来，采用历元信息构建器IEpochInfoBuilder初始化
//2014.11.20，czs, edit in namu, 将PointPositioner命名为AbstractPointPositioner
//2016.03.10, czs, edit in hongqing, 重构设计
//2016.04.23, czs, edit in huoda, 分离数据源，名称修改为 StreamGnssService，意思为GNSS产品服务
//2018.05.29, czs, edit in hmx, 为计算提供计算坐标初值 
//2018.08.03, czs, edit in HMX, 提取公共函数，适合动态定位

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Service
{
     /// <summary>
    /// GNSS 单站计算器
    /// </summary>
    public abstract class SingleSiteGnssSolver : AbstractGnssSolver<SingleSiteGnssResult, EpochInformation>
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected Log log = new Log(typeof(SingleSiteGnssSolver));

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public SingleSiteGnssSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        { 

        }
         
        #region 子类必须实现的方法，定位计算核心
     
        /// <summary>
        /// 创建定位结果对象
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            var result = new SingleSiteGnssResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
            return result;
        }

        #endregion
    }
}