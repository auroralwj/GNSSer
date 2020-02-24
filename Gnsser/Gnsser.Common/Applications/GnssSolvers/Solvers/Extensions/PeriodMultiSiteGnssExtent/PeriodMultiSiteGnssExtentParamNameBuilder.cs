//2017.08.28, czs & kyc, create in hongqing, 多站多历元GNSS计算预留测试类参数命名器

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
    /// 多站多历元GNSS计算预留测试类参数命名器
    /// </summary>
    public class PeriodMultiSiteGnssExtentParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 多站多历元GNSS计算预留测试类参数命名器
        /// </summary>
        /// <param name="option"></param>
        public PeriodMultiSiteGnssExtentParamNameBuilder(GnssProcessOption option  ) :base(option) { 

        }

        /// <summary>
        /// 生成。
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            if (SatelliteNumber.IsNullOrDefault( BasePrn)) throw new Geo.ShouldNotHappenException("请设置基础卫星");
            List<SatelliteNumber> enabledPrns = new List<SatelliteNumber>();


            //双差才参数名称，只有坐标和模糊度互差
            List<string> ParamNames = new List<string>(Gnsser.ParamNames.Dxyz);
            if (this.BaseParamCount == 4) { ParamNames.Add(Gnsser.ParamNames.WetTropZpd); }
            if (this.BaseParamCount == 5) { ParamNames.Add(Gnsser.ParamNames.WetTropZpd); ParamNames.Add(Gnsser.ParamNames.RefWetTrop); }

            foreach (var prn in this.EnabledPrns)
            {
                if (prn.Equals(BasePrn)) { continue; }

                ParamNames.Add(GetParamName(prn));
            }
            return ParamNames;
        }



        public override string GetParamName(SatelliteNumber prn)
        {
            return GetDoubleDifferAmbiParamName(prn);
        }
    }
}