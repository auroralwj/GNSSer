//2018.11.27, czs, create in HMX, 模糊度固定的纯载波双差

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
    /// 模糊度固定的纯载波双差
    /// </summary>
    public class AmbiFixedEpochDoublePhaseOnlytDifferParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 简易近距离单历元载波相位双差参数命名器
        /// </summary>
        /// <param name="Option"></param> 
        /// <param name="baseParamCount"></param> 
        public AmbiFixedEpochDoublePhaseOnlytDifferParamNameBuilder(GnssProcessOption Option, int baseParamCount)
            : base(Option)
        {
            this.BaseParamCount = baseParamCount;
        }

        /// <summary>
        /// 生成。
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            if (SatelliteNumber.IsNullOrDefault( BasePrn)) throw new Geo.ShouldNotHappenException("请设置基础卫星");

            //双差才参数名称，只有坐标和模糊度互差
            List<string> paramNames = new List<string>(Gnsser.ParamNames.Dxyz);//if (BaseParamCount == 3)

            if (BaseParamCount == 5)
            {
                paramNames.Add(Gnsser.ParamNames.WetTropZpd);
                paramNames.Add(Gnsser.ParamNames.RefWetTrop);  
            }
            else if (BaseParamCount == 4)
            {
                paramNames.Add(Gnsser.ParamNames.WetTropZpd); 
            } 

            //foreach (var prn in this.EnabledPrns)
            //{
            //    if (prn.Equals(BasePrn)) { continue; }

            //    paramNames.Add(GetParamName(prn));
            //}
            return paramNames;
        }


        /// <summary>
        /// 卫星参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            return prn + Gnsser.ParamNames.Pointer + BasePrn + Gnsser.ParamNames.DoubleDifferAmbiguitySuffix;
        }
    }
}