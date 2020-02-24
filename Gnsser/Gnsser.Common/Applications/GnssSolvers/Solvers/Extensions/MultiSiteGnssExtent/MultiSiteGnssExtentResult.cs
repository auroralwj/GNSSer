//2017.08.28, czs & kyc, create in hongqing, 多站单历元GNSS计算预留测试类


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
    /// 非差轨道确定
    /// </summary>
    public class MultiSiteGnssExtentResult : MultiSiteGnssResult
    {
        /// <summary>
        /// 双差网解定位构造函数
        /// </summary>
        /// <param name="epochInfo">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        /// <param name="ClockEstimationer">钟差估计器</param>
        /// <param name="previousResult">上一历元结果</param>
        public MultiSiteGnssExtentResult(
            MultiSiteEpochInfo epochInfo,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder ClockEstimationer,
            MultiSiteGnssExtentResult previousResult = null)
            : base(epochInfo, Adjustment, ClockEstimationer)
        {
            Vector vector = Adjustment.Corrected.CorrectedValue;
             
            //update 
            foreach (var epoch in epochInfo)
            {
                var key = NameBuilder.GetSiteWetTropZpdName(epoch);
                epoch.NumeralCorrections[Gnsser.ParamNames.WetTropZpd] = vector[Adjustment.GetIndexOf(key)];
            }             
        }  

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
            return base.ToString() + " ";
        }
    }   
    
}
