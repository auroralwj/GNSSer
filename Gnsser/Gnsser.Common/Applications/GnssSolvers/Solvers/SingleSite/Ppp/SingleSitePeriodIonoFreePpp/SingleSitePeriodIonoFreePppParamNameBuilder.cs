//2017.09.18, czs, create in hongqing, 单站多历元单频GNSS计算


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
    /// 单站多历元单频GNSS计算
    /// </summary>
    public class SingleSitePeriodIonoFreePppParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 单站多历元单频GNSS计算命名器
        /// </summary>
        /// <param name="option"></param>
        public SingleSitePeriodIonoFreePppParamNameBuilder(GnssProcessOption option)
            : base(option)
        { 

        }
        

        /// <summary>
        /// 生成。
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {           
            //双差才参数名称，只有坐标和模糊度互差
            List<string> paramNames = new List<string>();
            if (!this.Option.IsFixingCoord)
            {
                paramNames.AddRange(Gnsser.ParamNames.Dxyz);
            }
            int i = 0;
            foreach (var item in this.Epoches)
            {
                paramNames.Add(ParamNames.cDt + (i++));
            }         

            paramNames.Add(Gnsser.ParamNames.WetTropZpd);
            paramNames.AddRange(GetSysTimeRagneDifferName(Option.SatelliteTypes));  //系统间
            foreach (var prn in this.EnabledPrns)
            {
                paramNames.Add(GetParamName(prn));
            }
            return paramNames;
        }

        public override string GetParamName(SatelliteNumber prn)
        {
            return (prn) + ParamNames.PhaseLengthSuffix;
        }
    }
}