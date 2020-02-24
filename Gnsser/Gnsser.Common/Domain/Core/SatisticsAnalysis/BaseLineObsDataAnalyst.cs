//2005.01.10, czs, create in namu shangliao, 基线（两个站）观测文件分析者

using System;
using System.Text;
using System.IO;
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

namespace Gnsser
{
    /// <summary>
    /// 观测文件分析者。
    /// 观测数据先分析再处理可以得到更好的结果。
    /// </summary>
    public class BaseLineObsDataAnalyst : BaseObsDataAnalyst
    {
        /// <summary>
        /// 观测文件分析者，构造函数
        /// </summary>
        public BaseLineObsDataAnalyst(
            DataSourceContext DataSourceContext, 
            ISingleSiteObsStream refDataSource, 
            ISingleSiteObsStream rovDataSource, 
            GnssProcessOption SatelliteTypes)
        {            
            BaseLineObsDataSourceWalker BaseLineObsDataSourceWalker = new Data.Rinex.BaseLineObsDataSourceWalker(refDataSource, rovDataSource);
            
            //卫星可见性分析
            BaseLineVisibiltyAnalyst baseLineVisibiltyAnalyst = new BaseLineVisibiltyAnalyst(SatelliteTypes.SatelliteTypes, refDataSource.ObsInfo.Interval);
            //周跳探测
            BaseLineSatCycleSlipAnalyst baseLineSatCycleSlipAnalyst = new BaseLineSatCycleSlipAnalyst(DataSourceContext,
                SatelliteTypes,
                refDataSource.ObsInfo.Interval);

            BaseLineObsDataSourceWalker.ProcessorChain.AddProcessor(baseLineVisibiltyAnalyst);
            BaseLineObsDataSourceWalker.ProcessorChain.AddProcessor(baseLineSatCycleSlipAnalyst);

            BaseLineObsDataSourceWalker.Walk();

            this.SatCycleSlipMaker = baseLineSatCycleSlipAnalyst.SatPeriodMarker;
            this.SatVisibleMaker = baseLineVisibiltyAnalyst.SatPeriodMarker;
            this.SatelliteSelector = new SatelliteSelector(SatVisibleMaker, SatCycleSlipMaker);
        }
         
         
    } 
}
