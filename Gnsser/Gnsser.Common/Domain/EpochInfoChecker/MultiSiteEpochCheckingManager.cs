//2016.05.03, czs, create in hongqing, 检核多站历元数据。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Gnsser.Service;


namespace Gnsser.Checkers
{ 
    /// <summary>
    /// 历元信息检核管理器。
    /// 目前检核只针对当前历元观测数据，可以并行、共用、重用等，无记录效用和互相影响。
    /// </summary>
    public class MultiSiteEpochCheckingManager : CheckerChain<MultiSiteEpochInfo>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public MultiSiteEpochCheckingManager(DataSourceContext DataSourceContext, GnssProcessOption Option)
        {
            this.DataSourceContext = DataSourceContext;
            this.Option = Option;
        }

        DataSourceContext DataSourceContext { get; set; }
        GnssProcessOption Option { get; set; }
        //public virtual bool Check(MultiSiteEpochInfo epochInfo)
        //{ 
        //    foreach (var key in epochInfo)
        //    {
        //        if( !Check(key)){
        //            return false;
        //        }                
        //    }
        //    return true;
        //}

        //public override bool Check(EpochInformation epochInfo)
        //{
        //    return Check(epochInfo);
        //}

        /// <summary>
        /// 提供一个基础的检核，默认需要载波数据，观测5颗星。如果不用满足这些条件，请override。
        /// </summary>
        /// <returns></returns>
        public static MultiSiteEpochCheckingManager GetDefault(DataSourceContext DataSourceContext, GnssProcessOption option)
        {
            //检查
            var checker = new MultiSiteEpochCheckingManager(DataSourceContext, option);
            checker.Add(new EpochObsValueValidityChecker(option));
            if (option.IsEphemerisRequired)
            {
                checker.Add(new EphemerisTimePeriodAvailableChecker(DataSourceContext.EphemerisService));
            }
            if (!option.IsAllowMissingEpochSite)
            {
                checker.Add(new EpochSitesMissingChecker(DataSourceContext.ObservationDataSources.DataSources.Count));
            }

            return checker;
        }

    }
}
