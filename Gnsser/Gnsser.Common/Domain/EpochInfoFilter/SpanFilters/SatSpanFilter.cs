//2016.03.19, czs, create in hongqing, 卫星时段检查过滤
//2016.12.31, lly, edit in zz, 直接删除卫星
//2018.08.16, czs, edit in hmx, 采用新类实现

using System;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust ;
using Gnsser.Service;
using Gnsser.Data;
using Geo.Utils;
using Geo;
using Geo.Times;

namespace Gnsser.Filter
{
    /// <summary>
    /// 卫星时段检查过滤
    /// </summary>
    public class SatSpanFilter : EpochInfoReviser
    {
        /// <summary>
        /// 卫星时段检查过滤。
        /// </summary> 
        public SatSpanFilter(int MinContinuouObsCount, int maxBreakCount, Time time)
        {
            this.Name = "卫星时段过滤器";
            log.Info("将过滤连续历元不足 " + MinContinuouObsCount + " 的数据。");

            Remover = new SmallObsPeriodRemover<EpochInformation>(MinContinuouObsCount, maxBreakCount, time);
        }
        /// <summary>
        /// 卫星时段检查过滤。
        /// </summary> 
        public SatSpanFilter(BufferedStreamService<EpochInformation> satTimeInfoManager, int MinContinuouObsCount, int maxBreakCount, Time time)
        {
            this.Name = "卫星时段过滤器";
            log.Info("将过滤连续历元不足 " + MinContinuouObsCount + " 的数据。");

            Remover = new SmallObsPeriodRemover<EpochInformation>(satTimeInfoManager, MinContinuouObsCount, maxBreakCount, time);
        }
        SmallObsPeriodRemover<EpochInformation> Remover { get; set; } 

        public override bool Revise(ref EpochInformation info)
        {
            if(Remover.Inverval == 0) { Remover.Inverval = info.ObsInfo.Interval; }

            Remover.Buffers = this.Buffers;
            Remover.Revise(ref info); 

            return true;
        }

        public override void Complete()
        {
            base.Complete();
            Remover.Complete();
        }

    }
}
