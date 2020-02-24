
//2016.03.27, czs, create in hongqing, 基准星选择器

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 基准星选择器,根据观测缓存，选择卫星出现可能性大的卫星。
    /// </summary>
    public class BaseSatSelector
    {
        public BaseSatSelector()
        {

        }
        /// <summary>
        /// 选择唯一
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public SatelliteNumber SelectSingle(List<EpochInformation> infos, List<SatelliteNumber> exceptions = null)
        {
            var sats = Select(infos, exceptions);
            if (sats.Count <= 1) return SatelliteNumber.Default;
            return sats[0];
        }
        /// <summary>
        /// 选择卫星，依次返回待选卫星的顺序
        /// </summary>
        /// <param name="infos">至少两个历元</param>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public List<SatelliteNumber> Select(List<EpochInformation> infos, List<SatelliteNumber> exceptions = null)
        {
            if (infos.Count < 2)
            {
                throw new Exception("历元数量至少两个！");
            }
            if (exceptions == null) { exceptions = new List<SatelliteNumber>(); }

            EpochInformation baseEpoch = infos[0];
            EpochInformation nextEpoch = infos[1];

            var prns = SatelliteNumberUtils.GetCommons(baseEpoch.TotalPrns, nextEpoch.TotalPrns);
            if (prns.Count == 0) 
                return prns; 

            prns = prns.FindAll(m =>!exceptions.Contains( m));
            var SatMovingDiffers = new List<SatMovingDiffer>();
            foreach (var prn in prns)
            {

                var preEpoch = baseEpoch[prn];
                var nxtEpoch = nextEpoch[prn];

                var differ = new SatMovingDiffer(prn, preEpoch.Polar, nxtEpoch.Polar);
                SatMovingDiffers.Add(differ);
            }

            SatMovingDiffers.Sort();

            List<SatelliteNumber> results = new List<SatelliteNumber>();
            foreach (var item in SatMovingDiffers)
            {
                results.Add(item.Prn);
            }

            return prns;
        }

    }

    /// <summary>
    /// 卫星状态差分（速度）。
    /// </summary>
    public class SatMovingDiffer : IComparable<SatMovingDiffer>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public SatMovingDiffer(SatelliteNumber Prn, Polar first, Polar next)
        {
            this.Prn = Prn;
            this.FirstPolar = first;
            this.SecondPolar = next;
        }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }


        /// <summary>
        /// 高度角的偏差
        /// </summary>
        public double DifferOfElevation { get { return SecondPolar.Elevation - FirstPolar.Elevation; } }


        /// <summary>
        /// 上一历元极坐标
        /// </summary>
        public Polar FirstPolar { get; set; }

        /// <summary>
        /// 下一历元极坐标
        /// </summary>
        public Polar SecondPolar { get; set; }
        /// <summary>
        /// 排序，此处简单的排序。应该是综合考虑剩余时间的多少和卫星的稳定性等。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SatMovingDiffer other)
        {
            var result = this.DifferOfElevation - other.DifferOfElevation;
            var intresult = (int)(36000 * result);
            return intresult;
        }
    }

}