//2014.10.06， czs, create， 差分历元信息同步器

using System;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;

namespace Gnsser.Filter
{
    /// <summary>
    /// 差分历元信息同步器.
    /// 两个不同测站的相同历元的观测数据同步：卫星的顺序和数量都保持已知。
    /// </summary>
    public class EpochInfoSynchronous : EpochInfoReviser
    {
        /// <summary>
        /// 差分历元信息同步器
        /// </summary> 
        public EpochInfoSynchronous(EpochInformation refInfo)
        {
            this.refInfo = refInfo;
        }

        EpochInformation refInfo;

        public override bool Revise(ref EpochInformation info)
        {
            List<SatelliteNumber> tobeDisable = SatelliteNumberUtils.GetDiffers(this.refInfo.EnabledPrns, info.EnabledPrns);

            if (tobeDisable.Count > 0)
            {
                info.Disable(tobeDisable);
                this.refInfo.Disable(tobeDisable);
            }
            return true;
        } 
    }
}
