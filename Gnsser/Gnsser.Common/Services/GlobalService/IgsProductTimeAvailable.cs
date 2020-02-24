//2018.03.14, czs, create in hmx, IGS产品时间可用性计算

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Times;

namespace Gnsser
{
    //2018.03.15, czs, create in hmx, IGS产品时间滞后
    /// <summary>
    /// 产品滞后性
    /// </summary>
    public enum IgsProductLatency
    {
        /// <summary>
        /// real time
        /// </summary>
        Realtime,
        /// <summary>
        /// real time
        /// </summary>
        //UltraRapid,
        /// <summary>
        /// 17 - 41 hours	at 17 UTC daily
        /// </summary>
        Rapid,
        /// <summary>
        /// 12 - 18 days	every Thursday
        /// </summary>
        Final
    }


    /// <summary>
    /// IGS产品时间可用性计算
    /// 依据：http://www.igs.org/products/data
    /// </summary>
    public class IgsProductTimeAvailable
    {
        /// <summary>
        /// 根据时间判断可用的产品类型。
        /// </summary>
        /// <param name="obsTimeUtc"></param>
        /// <returns></returns>
        public static IgsProductLatency GetLatency(Time obsTimeUtc)
        {
            if (IsFinalAvailableGps(obsTimeUtc)) { return IgsProductLatency.Final; }
            if (IsRapidAvailableGps(obsTimeUtc)) { return IgsProductLatency.Rapid; }

            return IgsProductLatency.Realtime;
        }

        /// <summary>
        /// IGS Final products (IGS) 
        /// 延迟 12 - 18 天的每周四
        /// </summary>
        /// <param name="obsTimeUtc"></param>
        /// <param name="triedBufferDays">延缓时间（天），以防万一如果已经更新了呢！</param>
        /// <returns></returns>
        public static bool IsFinalAvailableGps(Time obsTimeUtc, double triedBufferDays = 0.5)
        {
            var nowUtc = Time.UtcNow;
            var oldOutDay = nowUtc - 18 * 24 * 3600; 
            if (oldOutDay >= obsTimeUtc) { return true; } //超过18天，则最终产品出
            var newOutDay = nowUtc - 12 * 24 * 3600;
            if (newOutDay < obsTimeUtc) { return false; } //不足12天，则无最终产品出

            var dayDiffer = nowUtc.DayOfWeek - DayOfWeek.Thursday;// nowUtc.DayOfWeek >= DayOfWeek.Thursday ?  : 
            if(dayDiffer < 0) { dayDiffer += 7; }
           
            var outDays = 12 + dayDiffer;
            var okTime = nowUtc - TimeSpan.FromDays(outDays + triedBufferDays);

            return okTime >= obsTimeUtc;
        }


        /// <summary>
        /// 包含星历钟差和ERP。如果在发布时间之后，则可以。  IGS Rapid products (IGR) 
        /// 17 - 41 小时	at 17 UTC daily
        /// </summary>
        /// <param name="obsTimeUtc"></param>
        /// <param name="triedBufferHours">延缓时间（小时），以防万一如果已经更新了呢！</param>
        /// <returns></returns>
        public static bool IsRapidAvailableGps(Time obsTimeUtc, double triedBufferHours = 1)
        {
            var nowUtc = Time.UtcNow;
            var latestRapidReleaseTime = nowUtc.Date - (17.0 + triedBufferHours) * 3600; // 17 点 UTC

            bool isOk =  latestRapidReleaseTime >= obsTimeUtc;
            return isOk;
        }

        /// <summary>
        /// 注意只有星历和钟差。
        /// IGS Ultra-rapid products (IGU) 
        /// To reduce the age of the prior, discontinued Predicted orbits, 
        /// the IGS started the Ultra-rapid products officially week 1087 in November 2000 (see IGSMAIL-3088) .
        /// Like the former IGS Predicted products, the Ultra-rapid products are available for real time and near real time use.
        /// The Ultra-rapid products are released four times per day, at 03:00, 09:00, 15:00, and 21:00 UTC. 
        /// (Until week 1267 they were released twice daily.) 
        /// In this way the average age of the predictions is reduced to 6 hours (compared to 36 hours for the old IGS Predicted products and 9 hours for the twice-daily Ultra-rapids). 
        /// The shorter latency should lead to significantly improved orbit predictions and reduced errors for user applications.
        /// Contrary to all other IGS orbit products the IGS Ultra-rapid orbit files contain 48 hours of tabulated orbital ephemerides, 
        /// and the start/stop epochs continuously shift by 6 hours with each update. All other orbit products contain strictly the 24 hours from 00:00 to 23:45. 
        /// The first 24 hours of each IGS Ultra-rapid orbit are based on the most recent GPS observational data from the IGS hourly tracking network. At the time of release,
        /// the observed orbits have an initial latency of 3 hours. The next 24 hours of each file are predicted orbits, extrapolated from the observed orbits.
        /// The orbits within each Ultra-rapid product file are, however, continuous at the boundary between the observed and predicted parts. Normally,
        /// the predicted orbits between 3 and 9 hours into the second half of each Ultra-rapid orbit file are most relevant for true real time applications.
        /// </summary>
        /// <returns></returns>
        public static bool IsUltraRapidAvailableGps(Time obsTimeUtc)
        {
            return true;
        } 

    }
}
