//2018.06.04, czs, edit in hmx, 增加单站单星多历元GNSS计算



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
    public class IonoDeltaSolveParamNameBuilder : MultiSysParamNameBuilder
    {  
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public IonoDeltaSolveParamNameBuilder(GnssProcessOption option)
            : base(option)
        { 
        }

        List<string> paramNames = new List<string>()
        {
            ParamNames.AmbiguityLen + "_" + ParamNames.Iono,  ParamNames.DifferIono// "N", "b"//, "c", "d"
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