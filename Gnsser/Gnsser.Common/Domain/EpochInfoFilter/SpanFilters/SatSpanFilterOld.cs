//2016.03.19, czs, create in hongqing, 卫星时段检查过滤
//2016.12.31, lly, edit in zz, 直接删除卫星

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

namespace Gnsser.Filter
{
    /// <summary>
    /// 卫星时段检查过滤
    /// </summary>
    public class SatSpanFilterOld : EpochInfoReviser
    {
        /// <summary>
        /// 卫星时段检查过滤。
        /// </summary> 
        public SatSpanFilterOld(SatTimeInfoManager satTimeInfoManager, int MinContinuouObsCount)
        {
            this.Name = "卫星时段过滤器";
            this.SatTimeInfoManager = satTimeInfoManager;
            this.MinContinuouObsCount = MinContinuouObsCount;
            log.Info("将过滤连续历元不足 " + MinContinuouObsCount + " 的数据。");
             
        }

        int MinContinuouObsCount { get; set; }
        SatTimeInfoManager SatTimeInfoManager;
        /// <summary>
        /// 当前信息，不包含在缓存中。
        /// </summary>
        public EpochInformation CurrentEpcohInfo { get; set; }

        public override bool Revise(ref EpochInformation info)
        {
            CurrentEpcohInfo = info;
            var infos = Buffers;

            if (infos == null || infos.Count == 0) return true;
            List<SatelliteNumber> tobeDeleteSatsOfUnhealth = new List<SatelliteNumber>();

            foreach (var item in info)
            {
                var satTimeInfo = SatTimeInfoManager.Get(item.Prn);
                if (satTimeInfo == null)
                {
                    log.Debug("satTimeInfo == null");
                    break;
                }
                var period = satTimeInfo.TimePeriod;
                var segment = period.GetSegment(info.ReceiverTime);
                var timeSpanCount = segment.Span / SatTimeInfoManager.ObsInterval;
                if (timeSpanCount < MinContinuouObsCount)
                {
                    string msg = "禁用：" + item.Prn + "(" + info.ReceiverTime.ToShortTimeString() + ")"
                        + "时段数：" + (int)timeSpanCount
                        + "/" + MinContinuouObsCount
                        + ",时段：" + segment.ToString();
                    log.Warn(msg);

                    item.Enabled = false;
                    tobeDeleteSatsOfUnhealth.Add(item.Prn);                    
                }
            }

            if (tobeDeleteSatsOfUnhealth.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("过滤并删除连续时段不足“" + MinContinuouObsCount + "”的卫星: ");
                sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", tobeDeleteSatsOfUnhealth));
                log.Debug(sb.ToString());

                info.Remove(tobeDeleteSatsOfUnhealth, true, "过滤并删除连续时段不足“" + MinContinuouObsCount + "”的卫星");
            }

            return true;
        }
         
    }
}
