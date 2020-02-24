//2015.01.08, czs, create in namu shangliao, 观测文件分析者

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
    public class ObsDataAnalyst : BaseObsDataAnalyst
    {
        /// <summary>
        /// 观测文件分析者，构造函数
        /// </summary>
        public ObsDataAnalyst(ISingleSiteObsStream dataSource, List<SatelliteType> SatelliteTypes)
        {
            ObsDataSourceWalker ObsDataSourceWalker = new ObsDataSourceWalker(dataSource);

            //卫星可见性分析
            SatVisibiltyAnalyst analyst = new SatVisibiltyAnalyst(SatelliteTypes, dataSource.ObsInfo.Interval);
            //周跳探测
            SatCycleSlipAnalyst satCycleSlipAnalyst = new SatCycleSlipAnalyst(SatelliteTypes, dataSource.ObsInfo.Interval);
           
            ObsDataSourceWalker.ProcessorChain.AddProcessor(analyst);
            ObsDataSourceWalker.ProcessorChain.AddProcessor(satCycleSlipAnalyst); 

            ObsDataSourceWalker.Walk();

            dataSource.Reset();

            this.SatCycleSlipMaker = satCycleSlipAnalyst.SatSequentialPeriod;
            this.SatVisibleMaker = analyst.SatSequentialPeriod; 
            this.SatelliteSelector = new SatelliteSelector(SatVisibleMaker, SatCycleSlipMaker);
        }
         
         
    } 
}
