// 2014.10.09, czs, create in hailutu, 去掉改正数

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Correction;

namespace Gnsser
{
    /// <summary>
    /// 改正数去掉器。 一般是为了重新赋值。
    /// </summary>
    public class CorrectionCleaner : EpochInfoReviser
    {
        /// <summary>
        /// 改正数处理器构造函数。
        /// </summary>
        public CorrectionCleaner()
        {
        }


        public override bool Revise(ref EpochInformation info)
        {
            info.ClearCorrections();

            return info.EnabledPrns.Count > 0;
        }
    }
}
