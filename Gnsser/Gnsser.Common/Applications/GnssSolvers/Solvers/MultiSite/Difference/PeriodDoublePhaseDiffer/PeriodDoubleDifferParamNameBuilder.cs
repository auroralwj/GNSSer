//2015.01.13, czs, create in namu, 双差参数命名器
//2018.07.31, czs, edit in hmx, 名称前冠名Period

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
    public class PeriodDoubleDifferParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 双差参数命名器
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="baseParamCount"></param>
        public PeriodDoubleDifferParamNameBuilder(GnssProcessOption Option)
            : base(Option)
        {  
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
            //如果相距较远，考虑对流层影响
            if (this.BaseParamCount == 4) { ParamNames.Add(Gnsser.ParamNames.WetTropZpd); }
            if (this.BaseParamCount == 5) { ParamNames.Add(Gnsser.ParamNames.WetTropZpd); 
                ParamNames.Add(Gnsser.ParamNames.RefWetTrop); }

            foreach (var prn in this.EnabledPrns)
            {
                if (prn.Equals(BasePrn)) { continue; }

                ParamNames.Add(GetParamName(prn));
            }
            return ParamNames;
        }



        public override string GetParamName(SatelliteNumber prn)
        {
            return prn + Gnsser.ParamNames.Pointer + BasePrn + Gnsser.ParamNames.DoubleDifferAmbiguitySuffix;
        }
    }
}