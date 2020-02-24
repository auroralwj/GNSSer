//2018.05.15, czs, create in hmx, 电离层硬件延迟计算



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
    /// 电离层硬件延迟计算参数命名器
    /// </summary>
    public class IonoHardwareDelaySolveParamNameBuilder : MultiSysParamNameBuilder
    {  
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public IonoHardwareDelaySolveParamNameBuilder(GnssProcessOption option)
            : base(option)
        { 
        }

        List<string> paramNames = new List<string>()
        {
            "a", "b", "c", "d"
        };

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        { 
            return paramNames;
        }

        /// <summary>
        /// 生成卫星编号相关的参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public override string GetParamName(SatelliteNumber prn)
        {
            return prn.ToString() + Gnsser.ParamNames.WaveLengthSuffix;
        }
    }
}