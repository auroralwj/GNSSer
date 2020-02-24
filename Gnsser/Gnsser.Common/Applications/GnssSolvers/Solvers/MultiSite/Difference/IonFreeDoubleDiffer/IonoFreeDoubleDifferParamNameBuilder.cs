//2016.04.21, czs & cuiyang, create in hongqing 帝都天元酒店,  双差参数命名器

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
    /// 双差参数命名器
    /// </summary>
    public class IonoFreeDoubleDifferParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IonoFreeDoubleDifferParamNameBuilder(GnssProcessOption option)  :base(option){ }


        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            var material = this.Material as MultiSiteEpochInfo;
            var rovSiteName = material.RovSiteNames[0];
            var refSiteName = material.BaseSiteName;

            List<string> paramNames = new List<string>();
            if (IsSiteNameIncluded)
            {
                var namer = new NetDifferName(material.RovSiteNames[0], material.BaseSiteName);
                var prefix = namer.ToString();
                paramNames = Gnsser.ParamNames.GetDxyz(prefix);
            }           
            else
            {
                paramNames = new List<string>(Gnsser.ParamNames.Dxyz);
            }

            //对流层参数


            if (BaseParamCount == 5)
            {
                if (IsSiteNameIncluded)
                {
                    paramNames.AddRange(new String[] {
                       material.RovSiteNames[0] + "_" +   Gnsser.ParamNames.WetTropZpd,
                        material.BaseSiteName + "_" +  Gnsser. ParamNames.WetTropZpd,
                        });
                }
                else
                {
                    paramNames.AddRange(new String[] {
                           Gnsser.ParamNames.WetTropZpd,
                           Gnsser. ParamNames.RefWetTrop,
                        });
                }
            }
            else if (BaseParamCount == 4)
            {
                if (IsSiteNameIncluded)
                {
                    paramNames.AddRange(new String[] {
                    material.RovSiteNames[0] + "_" +  Gnsser.  ParamNames.WetTropZpd,
                });
                }
                else
                {
                    paramNames.AddRange(new String[] {
                        Gnsser.  ParamNames.WetTropZpd,
                });
                }
            }

            foreach (var item in this.EnabledPrns) { if (BasePrn == item) continue; paramNames.Add(GetParamName(item)); }
            return paramNames;
        }

        /// <summary>
        /// 生成卫星编号相关的参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            if (IsSiteNameIncluded)
            {
                var material = this.Material as MultiSiteEpochInfo;
                NetDoubleDifferName namer = new NetDoubleDifferName(material.RovSiteNames[0], material.BaseSiteName, prn, BasePrn);
                return namer.ToString() + Gnsser.ParamNames.DoubleDifferAmbiguitySuffix;

            }
            return prn.ToString() + "-" + BasePrn + Gnsser.ParamNames.DoubleDifferAmbiguitySuffix;
            //return prn.ToString() + "-" + BasePrn + Gnsser.ParamNames.PhaseLengthSuffix;
        }
         
    }
}