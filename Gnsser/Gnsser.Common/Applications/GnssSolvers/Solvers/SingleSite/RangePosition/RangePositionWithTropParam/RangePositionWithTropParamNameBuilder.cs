//2017.10.20, czs, create in hongqing, 对流层模型改正伪距定位

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
    /// 对流层模型改正伪距定位参数
    /// </summary>
    public class RangePositionWithTropParamNameBuilder : GnssParamNameBuilder
    {
         /// <summary>
        /// 构造函数
        /// </summary>
        public RangePositionWithTropParamNameBuilder(GnssProcessOption option)
            : base(option)
        {
           
        } 
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
            paramNames.Add( Gnsser. ParamNames.RcvClkErrDistance);//接收机钟差
            paramNames.Add(Gnsser.ParamNames.WetTropZpd);//对流层

            paramNames.AddRange(GetSysTimeRagneDifferName(Option.SatelliteTypes));  //系统间

            return paramNames;
        }
        /// <summary>
        /// 以第一个系统为基准
        /// </summary>
        /// <param name="satTypes"></param>
        /// <returns></returns>
        public List<String> GetSysTimeRagneDifferName(List<SatelliteType> satTypes)
        {
            if (satTypes == null || satTypes.Count <= 1) { return new List<string>(); }

            var list = new List<String>();
            SatelliteType first = satTypes[0];
            foreach (var type in satTypes)
            {
                if (first == type) { continue; }
                var name =GetSysTimeRagneDifferName(first, type);
                list.Add(name);
            }

            return list;
        }
        public String GetSysTimeRagneDifferName(SatelliteType satA, SatelliteType satB)
        {
            return "" + ParamNames.SysTimeDistDifferOf + satA + satB;
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
