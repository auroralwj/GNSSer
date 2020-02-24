//2017.09.18, czs, create in hongqing, 单站多历元GNSS计算


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
    /// 单站多历元GNSS计算参数命名器
    /// </summary>
    public class SingleSitePeriodGnssExtentParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 单站多历元GNSS计算参数命名器
        /// </summary>
        /// <param name="option"></param>
        public SingleSitePeriodGnssExtentParamNameBuilder(GnssProcessOption option):base(option) { 

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
            int i = 1;
            //接收机钟差，必须每个历元不同
            foreach (var epoch in this.Epoches)
            {
                paramNames.Add(ParamNames.cDt + "_" + i);
                i++;
            }         
            //paramNames.Add(ParamNames.cDt);
            paramNames.Add(Gnsser.ParamNames.WetTropZpd);
            paramNames.AddRange(GetSysTimeRagneDifferName(Option.SatelliteTypes));  //系统间

            //电离层参数
            foreach (var prn in this.EnabledPrns)
            {
                var name = (prn) + ParamNames.Divider + ParamNames.Iono;
                paramNames.Add(name);
            }

            //具有硬件延迟的模糊度浮点解
            foreach (var prn in this.EnabledPrns)
            {
                var phaseAmbi = prn + ParamNames.PhaseLengthSuffix;
                paramNames.Add(phaseAmbi);
            }
            return paramNames;
        }

        public override string GetParamName(SatelliteNumber prn)
        {
            return (prn) + ParamNames.PhaseLengthSuffix;
        }
    }
}