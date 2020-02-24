//2014.09.24， czs, create， 创建观测值过滤器

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
using Geo;
using Gnsser.Data;

namespace Gnsser.Filter
{
    /// <summary>
    /// 观测值检查过滤。将具有故障卫星PRN的观测值删除。
    /// </summary>
    public class ObsSatExcludeFilter : EpochInfoReviser
    {
        /// <summary>
        /// 观测值检查过滤。将具有故障卫星PRN的观测值删除。
        /// </summary>  
        public ObsSatExcludeFilter(SatExcludeFileService dataSource)
        {
            this.Name = "观测值检查过滤器";
           this.SatExcludeDataSource = dataSource;
           this.NoticedPrns = new List<SatelliteNumber>();
            log.Info("将删除不稳定的或有故障的卫星");
        }
        /// <summary>
        /// 是否通知过了，为了整洁界面，只通知一次。
        /// </summary> 
        List<SatelliteNumber> NoticedPrns { get; set; }

        private SatExcludeFileService SatExcludeDataSource;

        /// <summary>
        /// 矫正方法。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation info)
        {
            if (SatExcludeDataSource == null) { return true; }

            List<SatelliteNumber> toRemovePrns = new List<SatelliteNumber>();

            foreach (var item in info)
            {
                if (SatExcludeDataSource.IsExcluded( info.ReceiverTime.Date, item.Prn))
                {
                    toRemovePrns.Add(item.Prn);
                }
            }

            if (toRemovePrns.Count > 0)
            {
                var toNotice = toRemovePrns.FindAll(m=> !NoticedPrns.Contains(m));
                if (toNotice.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("删除 "+info.ReceiverTime.ToDateString()+" 故障卫星：");
                    sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", toRemovePrns));
                    log.Debug(sb.ToString());
             
                    NoticedPrns.AddRange(toNotice);
	              }                 
                
                info.Remove(toRemovePrns, true, "删除 "+info.ReceiverTime.ToDateString()+" 故障卫星"); 
            }
            return info.EnabledPrns.Count > 0;
        } 
    }
}

