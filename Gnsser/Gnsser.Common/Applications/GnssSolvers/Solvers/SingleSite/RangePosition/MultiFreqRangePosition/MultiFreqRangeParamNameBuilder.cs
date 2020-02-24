//2018.08.12, czs, create in hmx, 多频率伪距定位


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
    /// 多频率伪距定位
    /// </summary>
    public class MultiFreqRangeParamNameBuilder : MultiSysParamNameBuilder
    {
        /// <summary>
        /// 多系统伪距定位参数命名器 构造函数
        /// </summary>
        /// <param name="option"></param>
        public MultiFreqRangeParamNameBuilder(GnssProcessOption option, bool IsEstIonoParamOfL1) : base(option)
        {
            this.IsEstIonoParamOfL1 = IsEstIonoParamOfL1;
        }
         public bool   IsEstIonoParamOfL1 {get;set;}
        // <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>();
            if (!Option.IsFixingCoord)
            {
                paramNames.AddRange(Gnsser.ParamNames.Dxyz);
            }
            paramNames.Add(Gnsser.ParamNames.RcvClkErrDistance);

            paramNames.AddRange(BuildSysTimeDifferParams());

            paramNames.Add(ParamNames.Dcb); 

            if (IsEstIonoParamOfL1)
            {
                foreach (var item in this.EnabledPrns)
                {
                    paramNames.Add(GetParamName(item));
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
            return prn.ToString() + Gnsser.ParamNames.Iono;
        } 
    }
}