//2016.10.26, czs, create in hongqing, 搭建单历元相位双差计算框架
//2018.07.26, czs, create in HMX, 简易近距离单历元载波相位双差
//2018.12.30, czs, create in hmx, 单历元纯相位双差

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
    /// 简易近距离单历元载波相位双差参数命名器
    /// </summary>
    public class EpochDoublePhaseDifferParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 简易近距离单历元载波相位双差参数命名器
        /// </summary>
        /// <param name="Option"></param> 
        /// <param name="baseParamCount"></param> 
        public EpochDoublePhaseDifferParamNameBuilder(GnssProcessOption Option, int baseParamCount)
            : base(Option)
        {
            this.BaseParamCount = baseParamCount;
            IsDueFrequence = Option.ObsPhaseType == ObsPhaseType.L1AndL2;
        }

        /// <summary>
        /// 是否双频
        /// </summary>
        public bool IsDueFrequence { get; set; }
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

            foreach (var prn in this.EnabledPrns)
            {
                if (prn.Equals(BasePrn)) { continue; }
                var l1Name = GetParamName(prn);
                paramNames.Add(l1Name);
            }
            if(IsDueFrequence)
            foreach (var prn in this.EnabledPrns)
            {
                if (prn.Equals(BasePrn)) { continue; }
                var name = GetParamNameL2(prn);
                paramNames.Add(name);
            }
            return paramNames;
        }


        /// <summary>
        /// 卫星参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            return prn + Gnsser.ParamNames.Pointer + BasePrn + Gnsser.ParamNames.DoubleDifferL1AmbiguitySuffix;
        }
        public   string GetParamNameL2(SatelliteNumber prn)
        {
            return prn + Gnsser.ParamNames.Pointer + BasePrn + Gnsser.ParamNames.DoubleDifferL2AmbiguitySuffix;
        }
    }
}