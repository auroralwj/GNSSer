using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Gnsser.Excepts; 
using Geo.Algorithm.Adjust;
using Gnsser.Service;

namespace Gnsser.Checkers
{
    /// <summary>
    /// 均方差/标准差检核。
    /// </summary>
    public class MaxStdChecker : GnssResultChecker
    {
        /// <summary>
        /// 标准差检核构造函数。
        /// </summary>
        /// <param name="maxStdDev"></param>
        public MaxStdChecker(double maxStdDev = 100.0)
        {
            this.MaxStdDev = maxStdDev;
        }
        /// <summary>
        /// 最大标准差
        /// </summary>
        public double MaxStdDev { get; set; }

        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="adjust"></param>
        public override bool Check(BaseGnssResult adjust)
        {
            bool isTrue = (MaxStdDev >= adjust.ResultMatrix.StdDev);

            if (!isTrue)
            {
                var msg = adjust.Name + " " + adjust.Material.ReceiverTime + " 标准差超限!  " + adjust.ResultMatrix.StdDev + " > " + MaxStdDev;
                log.Error(msg);
                Exception = new SatCountException(msg);
            }
            return isTrue;
        }

    }
}
