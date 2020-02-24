//2015.01.07, czs, edit in namu, 名称由 IAdujustChecker 改为 IPositionResultChecker


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Gnsser.Service;

namespace Gnsser.Checkers
{
    /// <summary>
    /// IPositionResultChecker 质量检核
    /// </summary>
    public interface IGnssResultChecker
    {
        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="GnssResult"></param>
        bool Check(BaseGnssResult GnssResult);
        
        /// <summary>
        /// 异常或错误信息，当且仅当检查不通过时，才具有该信息。
        /// </summary>
        Exception Exception { get; }

    }
}
