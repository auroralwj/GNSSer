//2015.01.07, czs, edit in namu, 历元计算结果检核

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Gnsser.Service;
using Geo.Algorithm.Adjust;
using Geo.IO;

namespace Gnsser.Checkers
{
    /// <summary>
    /// 历元计算结果检核
    /// </summary>
    public abstract class GnssResultChecker : IGnssResultChecker
    {
        protected Log log = new Log(typeof(GnssResultChecker));
        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="positionResult">历元计算结果</param>
        public abstract bool Check(BaseGnssResult positionResult);

        /// <summary>
        /// 异常或错误信息，当且仅当检查不通过时，才具有该信息。
        /// </summary>
        public Exception Exception { get; protected set; }
    }
}
