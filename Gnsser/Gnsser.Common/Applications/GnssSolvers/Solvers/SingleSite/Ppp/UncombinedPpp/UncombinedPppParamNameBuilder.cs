using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 非差非组合参数
    /// </summary>
    public class UncombinedPppParamNameBuilder : MultiSysParamNameBuilder
    {
         /// <summary>
        /// 构造函数
        /// </summary>
        public UncombinedPppParamNameBuilder(GnssProcessOption option) :base(option)
        {
            this.IsEstDcb = option.IsEstDcbOfRceiver;
        }
        bool IsEstDcb;
        /// <summary>
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
             

            paramNames.Add(Gnsser.ParamNames.WetTropZpd);//对流层
            if (IsEstDcb) { paramNames.Add(Gnsser.ParamNames.Dcb); }

            foreach (var item in this.EnabledPrns) { paramNames.Add(item.ToString() + ParamNames.Divider + ParamNames.Iono); }
            foreach (var item in this.EnabledPrns) { paramNames.Add(item.ToString() + ParamNames.PhaseALengthSuffix); }
            foreach (var item in this.EnabledPrns) { paramNames.Add(item.ToString() + ParamNames.PhaseBLengthSuffix); }

            return paramNames;
        }
        /// <summary>
        /// 生成卫星编号相关的参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            return prn.ToString() + Gnsser.ParamNames.PhaseLengthSuffix;
        } 
    }
}
