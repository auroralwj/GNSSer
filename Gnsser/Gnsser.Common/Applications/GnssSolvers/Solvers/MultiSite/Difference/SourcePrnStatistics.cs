//2012.05, czs, create, 差分
//2015.01.01, czs, edit in namu, 提取为专门统计卫星出勤率类

using System;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Text;
using Gnsser.Data.Rinex;
using Geo.Coordinates;

namespace Gnsser.Service
{
    /// <summary>
    ///统计卫星出勤率
    /// </summary>
    public  class SourcePrnStatistics
    { 
        /// <summary>
        /// 统计卫星出勤率 构造函数。
        /// </summary>
        /// <param name="obsFileA"></param>
        /// <param name="obsFileB"></param>
        public SourcePrnStatistics(ISingleSiteObsStream obsFileA, ISingleSiteObsStream obsFileB, List<SatelliteType> satTypes, int SatCount = 5)
        {
            this.SatCount = SatCount;
            this.satTypes = satTypes;
            //卫星总出勤率，选择前五个利用。
            this.CommonPRNs = GetComonSatPRNs(obsFileA, obsFileB, SatCount, satTypes); 
        }
        List<SatelliteType> satTypes { get; set; }
        /// <summary>
        /// 共有的卫星，该值可能与接收机能观测到的卫星不一致。应该采用本值。
        /// 该列表还保存了卫星对应的顺序。
        /// </summary>
        public List<SatelliteNumber> CommonPRNs { get; set; }
        /// <summary>
        /// 指定卫星的数量
        /// </summary>
        public int SatCount { get; set; }
        #region  选择待差分的卫星

        /// <summary>
        /// 卫星总出勤率，选择前五个利用。
        /// </summary>
        /// <param name="obsFileA"></param>
        /// <param name="obsFileB"></param>
        /// <returns></returns>
        public static List<SatelliteNumber> GetComonSatPRNs(
            ISingleSiteObsStream obsFileA, 
            ISingleSiteObsStream obsFileB, 
            int SatCount,
            List<SatelliteType> satTypes
            )
        {
            List<SatelliteNumberRatio> satApearRatio = SatAppearRatios(obsFileA, obsFileB, satTypes);
            List<SatelliteNumber> commonPrns = new List<SatelliteNumber>(); 
            foreach (SatelliteNumberRatio s in satApearRatio)
            {
                if (commonPrns.Count < SatCount) commonPrns.Add(s.PRN);
                else break;
            }
            return commonPrns;
        }       

        /// <summary>
        ///  数据预处理，卫星的选择条件：两个站观测历元中出现次数最多的卫星，卫星数量大于4.
        ///  以卫星的出勤率为参考。 
        /// </summary>
        /// <param name="obsFileA"></param>
        /// <returns></returns>
        private static List<SatelliteNumberRatio> SatAppearRatios(ISingleSiteObsStream obsFileA, ISingleSiteObsStream obsFileB, List<SatelliteType> satTypes)
        {
          //  double baseRatio = 1.0 / (obsFileA.Count + obsFileB.Count);

            List<SatelliteNumberRatio> satApearRatio = new List<SatelliteNumberRatio>();
            foreach (var oSec in obsFileA)
            {
                foreach (SatelliteNumber prn in oSec.EnabledPrns)
                {
                    if (!satTypes.Contains(prn.SatelliteType)) continue;

                    SatelliteNumberRatio s = new SatelliteNumberRatio() { PRN = prn, Ratio = 1 };
                    if (!satApearRatio.Contains(s)) satApearRatio.Add(s);
                    else satApearRatio.Find(mbox=>mbox.PRN.Equals(prn)).Ratio += 1;
                }
            }
            foreach (var oSec in obsFileB)
            {
                foreach (SatelliteNumber prn in oSec.EnabledPrns)
                {
                    if (!satTypes.Contains(prn.SatelliteType)) continue;

                    SatelliteNumberRatio s = new SatelliteNumberRatio() { PRN = prn, Ratio = 1 };
                    if (!satApearRatio.Contains(s)) satApearRatio.Add(s);
                    else satApearRatio.Find(mbox => mbox.PRN.Equals(prn)).Ratio += 1;
                }
            }
            //排序
            satApearRatio.Sort();

            //obsFileA.Reset();
            //obsFileB.Reset();
            return satApearRatio;
        }
        #endregion 

    }
}
