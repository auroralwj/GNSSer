using System;
using Gnsser.Domain;
using System.Collections.Generic;

using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Data;
using Geo.Utils;
using Geo.IO;
using Geo;

namespace Gnsser.Filter
{
    /// <summary>
    /// 星历过滤器，采用面向对象模型。
    /// </summary>
    public class EphemerisFilter : EpochInfoReviser
    {
        /// <summary>
        /// 卫星检查过滤。
        /// </summary>
        /// <param name="EphemerisService">星历</param>
        /// <param name="VertAngleCut">高度截止角</param>
        public EphemerisFilter(IEphemerisService EphemerisService, double VertAngleCut = 10)
        {
            this.Name = "星历过滤器";
            this.VertAngleCut = VertAngleCut;
            this.EphemerisService = EphemerisService;

            log.Info("将删除高度截止角低于 "+VertAngleCut+" 的卫星。");

            RemovedPrn = new List<SatelliteNumber>();
        }
        List<SatelliteNumber> RemovedPrn;
        double VertAngleCut = 10;

        IEphemerisService EphemerisService;

        /// <summary>
        /// 矫正
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation info)
        {
            info.RemoveNoEphemeris();

            if(info.Count == 0) { return false; }

            if (XYZ.IsZeroOrEmpty(info.SiteInfo.EstimatedXyz))
            {
                return true;//此处让可以继续执行
            }

            List<SatelliteNumber> tobeDeleteSatsOfLowAnlge = new List<SatelliteNumber>();
            List<SatelliteNumber> tobeDeleteSatsOfUnhealth = new List<SatelliteNumber>();
            List<SatelliteNumber> tobeDeleteSatsOfNoEphs = new List<SatelliteNumber>();
            foreach (var sat in info.EnabledSats)
            {
                if (!sat.HasEphemeris)
                {
                    tobeDeleteSatsOfNoEphs.Add(sat.Prn);
              //      log.Error(sat.Prn + ", " + info.ReceiverTime + " , 没有星历！");

                    sat.Enabled = false;
                    continue;
                }

                //高度截止角 
                Polar polar = sat.Polar; 
                if (polar.Elevation < VertAngleCut)
                {
                    sat.Enabled = false;
                    tobeDeleteSatsOfLowAnlge.Add(sat.Prn);//是佛不用删除？？？？2014.12.17
                }

                //是否健康
             //   if (!EphemerisService.IsHealth(key.Prn, info.CorrectedTime))
                if (!EphemerisService.IsAvailable(sat.Prn, info.ReceiverTime))
                {
                    sat.Enabled = false;
                    tobeDeleteSatsOfUnhealth.Add(sat.Prn);
                } 
            }

            //debug
            if (tobeDeleteSatsOfLowAnlge.Count > 0 && tobeDeleteSatsOfLowAnlge.FindAll(m => !RemovedPrn.Contains(m)).Count > 0)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(info.Name + "," + info.ReceiverTime + ", 高度截止角低于 " + VertAngleCut + " 的卫星: ");
                sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", tobeDeleteSatsOfLowAnlge));
                log.Debug(sb.ToString());
                RemovedPrn.AddRange(tobeDeleteSatsOfLowAnlge.FindAll(m => !RemovedPrn.Contains(m)));
            }
            info.Remove(tobeDeleteSatsOfLowAnlge, true, "删除高度截止角低于 " + VertAngleCut + " 的卫星");
            info.Remove(tobeDeleteSatsOfNoEphs, true, "删除没有星历的卫星");
                       

            if (tobeDeleteSatsOfUnhealth.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("不健康的卫星: ");
                sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", tobeDeleteSatsOfUnhealth));
                log.Debug(sb.ToString());

                info.Remove(tobeDeleteSatsOfUnhealth,true, "不健康的卫星");
            }

            return info.EnabledPrns.Count > 0;
        } 
 
    }
}
