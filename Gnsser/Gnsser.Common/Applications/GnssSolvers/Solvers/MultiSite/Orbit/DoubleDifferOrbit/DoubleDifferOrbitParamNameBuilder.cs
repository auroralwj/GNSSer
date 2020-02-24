//2018.10.28, czs, create in HMX, 无电离层组合双差定轨

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
    /// 无电离层组合双差定轨参数命名器
    /// </summary>
    public class DoubleDifferOrbitParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DoubleDifferOrbitParamNameBuilder(GnssProcessOption option)  :base(option){ 
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
            foreach (var prn in this.EnabledPrns)//卫星坐标
            {
                if (prn == BasePrn) { continue; }
                var names = this.GetSatDxyz(prn);
                paramNames.AddRange(names);
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
                    if (BasePrn == prn) { continue; }
                    paramNames.Add(GetDoubleDifferAmbiParamName(siteName , prn));
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