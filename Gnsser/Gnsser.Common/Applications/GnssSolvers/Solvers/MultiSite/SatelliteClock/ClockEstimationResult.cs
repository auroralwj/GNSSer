//2016.04.06 double create in Zhengzhou 参考PointPositionResult


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Geo.Algorithm.Adjust;
using Geo;
using Geo.Algorithm;
using Geo.Times;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 通用的钟差估计结果
    /// </summary>
    public class ClockEstimationResult : AbstractClockEstimationResult
    {
        /// <summary>
        /// 钟差估计构造函数
        /// </summary>
        /// <param name="epochInfo">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        /// <param name="ClockEstimationer">钟差估计器</param>
        /// <param name="previousResult">上一历元结果</param>
        public ClockEstimationResult(
            MultiSiteEpochInfo epochInfo,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder ClockEstimationer,
            ClockEstimationResult previousResult = null)
            : base(epochInfo, Adjustment, ClockEstimationer)
        {
            Vector vector = Adjustment.Corrected.CorrectedValue;
             

            //update 
            foreach (var epoch in epochInfo)
            {
                var key = NameBuilder.GetSiteWetTropZpdName(epoch);
                epoch.NumeralCorrections[Gnsser.ParamNames.WetTropZpd] = vector[Adjustment.GetIndexOf(key)];
            }

            this.AmbiguityDic = new Dictionary<string, double>();
            int length = ParamCount;
            for (int i = 2 * MaterialObj.Count + EnabledPrns.Count; i < length; i++)
            {
                double val = vector[i];
                AmbiguityDic.Add(ParamNames[i], val);
            }
            //this.PrnWithSlips = new List<SatelliteNumber>();
            this.PrnWithSlipss = new List<string>();
            foreach (var item in epochInfo)
            {
                foreach (var item1 in item)
                    if (item1.IsUnstable)
                    {
                        //PrnWithSlips.Add(item1.Prn);
                        PrnWithSlipss.Add(item.SiteName + "-" + item1.Prn);
                    }
            }
        }
        //public ClockEstimationResult(
        //    MultiSitePeriodInfo epochInfo,
        //    Adjustment Adjustment,
        //    GnssParamNameBuilder ClockEstimationer,
        //    ClockEstimationResult previousResult = null)
        //    : base(epochInfo, Adjustment, ClockEstimationer)
        //{
        //    Vector vector = Adjustment.Corrected;


        //    //update 
        //    foreach (var epoch in epochInfo)
        //    {
        //        var keyPrev = NameBuilder.GetReceiverWetTropParamName(epoch);
        //        epoch.NumeralCorrections[Gnsser.ParamNames.Trop] = vector[Adjustment.GetIndexOf(keyPrev)];
        //    }

        //    this.AmbiguityDic = new Dictionary<string, double>();
        //    int length = ParamCount;
        //    for (int i = 2 * MaterialObj.Count + EnabledPrns.Count; i < length; i++)
        //    {
        //        double val = vector[i];
        //        AmbiguityDic.Add(ParamNames[i], val);
        //    }
        //    //this.PrnWithSlips = new List<SatelliteNumber>();
        //    this.PrnWithSlipss = new List<string>();
        //    foreach (var key in epochInfo)
        //    {
        //        foreach (var item1 in key)
        //            if (item1.IsUnstable)
        //            {
        //                //PrnWithSlips.Add(item1.Prn);
        //                PrnWithSlipss.Add(key.SiteName + "-" + item1.Prn);
        //            }
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public double GetAmbiguityDistace(string SiteName,SatelliteNumber prn)
        {
            if (AmbiguityDic.ContainsKey(SiteName+"-"+prn))
                return AmbiguityDic[SiteName + "-" + prn];
            else return 0;
        }
        private Dictionary<string, double> AmbiguityDic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            return base.GetTabTitles() + "\t" + "CycleClip";
        }
        /// <summary>
        /// 
        /// </summary>
        public List<string> PrnWithSlipss { get; private set; }
        
        public override string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            if (this.PrnWithSlipss.Count > 0)
            {
                sb.Append("CS:");
                foreach (var item in PrnWithSlipss)
                {
                    sb.Append(item.ToString());
                    sb.Append(" ");
                }
            }
            return base.ToString() + " " + sb.ToString();
        }
    }   
    
}
