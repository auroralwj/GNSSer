//2018.11.05, czs, create in HMX, 单差网解定位
using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser.Service
{

    /// <summary>
    /// 单差网解定位参数命名器
    /// </summary>
    public class NetSingleDifferPositionParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NetSingleDifferPositionParamNameBuilder(GnssProcessOption option)  :base(option){
            this.IsEstimateTropWetZpd = option.IsEstimateTropWetZpd;
        }
        /// <summary>
        /// 是否估计对流层湿延迟参数。
        /// </summary>
        public bool IsEstimateTropWetZpd { get; set; }
        MultiSiteEpochInfo Obj { get => (MultiSiteEpochInfo)this.Material; }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            if (String.IsNullOrWhiteSpace(BaseSiteName))
            {
                BaseSiteName = Obj.First.SiteName;
            }

            List<string> paramNames = new List<string>(); 

            foreach (var site in this.Obj)
            {
                if (site.SiteName == BaseSiteName) { continue; }

                var names = this.GetSiteDxyz(site.SiteName);
                paramNames.AddRange(names);
            }
            //接收机钟差
            foreach (var site in this.Obj)
            {
                if (site.SiteName == BaseSiteName) { continue; }

                var names = this.GetSiteClockDiffer(site.SiteName, BaseSiteName);
                paramNames.Add(names);
            }

            //对流层
            if (IsEstimateTropWetZpd)
            {
                var name = GetSiteWetTropZpdName(BaseSiteName);
                paramNames.Add(name);
                foreach (var site in this.Obj)
                {
                    if (site.SiteName == BaseSiteName) { continue; }
                    name = GetSiteWetTropZpdName(site.SiteName);
                    paramNames.Add(name);
                }
            }
            //模糊度
            foreach (var site in this.Obj)
            {
                var siteName = site.SiteName;
                if (siteName == BaseSiteName) { continue; }

                foreach (var prn in this.EnabledPrns)
                { 
                    paramNames.Add(GetSingleDifferAmbiParamName(siteName , prn));
                } 
            }
            return paramNames;
        }
        /// <summary>
        /// 生成卫星编号相关的参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            return prn.ToString() + "-" + BasePrn + Gnsser.ParamNames.PhaseLengthSuffix;
        }
         
    }
}