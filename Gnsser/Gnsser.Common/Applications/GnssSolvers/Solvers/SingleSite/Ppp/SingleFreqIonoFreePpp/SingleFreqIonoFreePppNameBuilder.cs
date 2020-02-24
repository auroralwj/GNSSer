//2017.09.14, czs, create in hongqing, 单频消电离层组合

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
    ///  单频消电离层组合
    /// </summary>
    public class SingleFreqIonoFreePppNameBuilder : GnssParamNameBuilder
    {
         /// <summary>
        /// 构造函数
        /// </summary>
        public SingleFreqIonoFreePppNameBuilder(GnssProcessOption option) :base(option)
        { 
        }
          
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>();
            if (!this.Option.IsFixingCoord)
            {
                paramNames.AddRange(Gnsser.ParamNames.Dxyz);
            }         
   
            paramNames.Add( Gnsser. ParamNames.RcvClkErrDistance);//接收机钟差
            paramNames.Add(Gnsser.ParamNames.WetTropZpd);//对流层

            paramNames.AddRange(GetSysTimeRagneDifferName(Option.SatelliteTypes));  //系统间

            foreach (var item in this.EnabledPrns) { paramNames.Add(GetSatPhaseRangeName(item)); }//相位

            //foreach (var key in this.EnabledPrns) { paramNames.Add(GetSatIonoName(key)); }//相位

            return paramNames;
        }

        public String GetSatPhaseRangeName(SatelliteNumber prn)
        {
            return "" + prn + ParamNames.PhaseLengthSuffix;
        }
        public String GetSatIonoName(SatelliteNumber prn)
        {
            return "" + prn + ParamNames.Divider + ParamNames.Iono;
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
