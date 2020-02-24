//2015.01.07, czs, edit in namu, PositionResult检核
//2016.05.04, czs, edit in hongqing, 重命名为 GnssResultCheckingManager
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Gnsser.Service;
using Geo.Algorithm.Adjust;


namespace Gnsser.Checkers
{
    /// <summary>
    /// 结果检核，组合检核。
    /// </summary>
    public class GnssResultCheckingManager : GnssResultChecker
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public GnssResultCheckingManager()
        {
            Checkers = new List<IGnssResultChecker>();
            log.Info("启用结果检核。");
        }
        /// <summary>
        /// 卫星数量
        /// </summary>
        private List<IGnssResultChecker> Checkers { get; set; } 

        /// <summary>
        /// 添加一个检核器。
        /// </summary>
        /// <param name="checker"></param>
        public void Add(IGnssResultChecker checker)
        {
            Checkers.Add(checker);
        }

        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="checkee">被检核者</param>
        public override bool Check(BaseGnssResult checkee)
        {
            bool result = true;
            foreach (var item in Checkers)
            {
                result = item.Check(checkee);
                if(!result)
                {
                    this.Exception = item.Exception; 
                    return result;
                }                
            }
            return result;
        }
        /// <summary>
        /// 默认结果检核
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        static public GnssResultCheckingManager GetDefault(GnssProcessOption Option)
        {
            GnssResultCheckingManager checker = new GnssResultCheckingManager();

            if (Option.FilterCourceError)
            {
                checker.Add(new MaxStdChecker(Option.MaxStdDev));
               if(Option.GnssSolverType != GnssSolverType.最简伪距定位)
                   checker.Add(new CliffyStdChecker(Option.MaxMeanStdTimes, Option.ExemptedStdDev));
            }
            return checker;
        }
    }
}
