//2017.09.05, czs, create in hongqing, 多系统伪距定位参数命名器

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
    /// 多系统伪距定位参数命名器
    /// </summary>
    public class MultiSysRangeParamNameBuilder : MultiSysParamNameBuilder
    {
        /// <summary>
        /// 多系统伪距定位参数命名器 构造函数
        /// </summary>
        /// <param name="option"></param>
        public MultiSysRangeParamNameBuilder(GnssProcessOption option) :base(option)
        { 
            
        }  

        // <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>(Gnsser.ParamNames.DxyzClk);

            List<string> sysTimeDifferParams = BuildSysTimeDifferParams();
            paramNames.AddRange(sysTimeDifferParams);
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