//2015.10.26, czs, edit in hongqing, 提取抽象接口，对某一对象进行检核
//2016.05.01, czs, edit in hongqing, 重命名为 EpochCheckingManager

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
    public class EpochCheckingManager : CheckerChain<EpochInformation>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public EpochCheckingManager( DataSourceContext DataSourceContext, GnssProcessOption Option)
        {
            this.DataSourceContext = DataSourceContext;
            this.Option = Option;
        }

        DataSourceContext DataSourceContext { get; set; }
        GnssProcessOption Option { get; set; }

        /// <summary>
        /// 提供一个基础的检核，默认需要载波数据，观测5颗星。如果不用满足这些条件，请override。
        /// </summary>
        /// <returns></returns>
        public static EpochCheckingManager GetDefaultCheckers(DataSourceContext DataSourceContext,GnssProcessOption option)
        {
            //检查
            EpochCheckingManager checker = new EpochCheckingManager(DataSourceContext, option);
            checker.Add(new EpochObsValueValidityChecker(option));
            if (option.IsEphemerisRequired)
            {
                checker.Add(new EphemerisTimePeriodAvailableChecker(DataSourceContext.EphemerisService));
            }
            return checker;
        }

    }
}
