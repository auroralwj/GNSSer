//2016.03.11, czs, create in hongqing, PPP参数命名器
//2019.01.01, czs, edit in hmx, 支持坐标固定计算

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
    /// 无电离层参数命名器
    /// </summary>
    public class IonoFreePppParamNameBuilder : MultiSysParamNameBuilder
    {  
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public IonoFreePppParamNameBuilder(GnssProcessOption option):base(option)
        { 
        }


        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>();
            if (this.Option.IsFixingCoord)
            {

                paramNames.Add(Gnsser.ParamNames.RcvClkErrDistance);
            }
            else
            {
                paramNames.AddRange(Gnsser.ParamNames.DxyzClk);
            }
        
            paramNames.AddRange(BuildSysTimeDifferParams());
            paramNames.Add(Gnsser.ParamNames.WetTropZpd);//对流层
            //模糊度
            foreach (var item in this.EnabledPrns) { paramNames.Add(GetParamName(item)); }
            return paramNames;
        }

        /// <summary>
        /// 生成卫星编号相关的参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prn.ToString()).Append(Gnsser.ParamNames.WaveLengthSuffix);
            return sb.ToString();

            return prn.ToString() + Gnsser.ParamNames.WaveLengthSuffix;
        }
    }
}