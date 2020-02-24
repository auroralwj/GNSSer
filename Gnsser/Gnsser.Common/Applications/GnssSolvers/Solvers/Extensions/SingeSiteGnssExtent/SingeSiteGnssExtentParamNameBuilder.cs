//2017.08.28, czs & kyc, create in hongqing, 单站单历元GNSS计算预留测试类


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
    /// 单站单历元GNSS计算预留测试类参数命名器
    /// </summary>
    public class SingeSiteGnssExtentParamNameBuilder : MultiSysParamNameBuilder
    {  
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public SingeSiteGnssExtentParamNameBuilder(GnssProcessOption option)
            : base(option)
        { 
        }


        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>(Gnsser.ParamNames.DxyzClk);
             
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
            return prn.ToString() + Gnsser.ParamNames.WaveLengthSuffix;
        }
    }
}