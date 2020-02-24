//2015.01.13, czs, create in namu, 单差参数命名器

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
using Geo.Times; 

namespace Gnsser.Service
{

    /// <summary>
    /// 单差参数命名器
    /// </summary>
    public class SingleDifferParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 单差参数命名器
        /// </summary>
        public SingleDifferParamNameBuilder(GnssProcessOption Option):base(Option) { }
  

        /// <summary>
        /// 生成。
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        { 
            //参数的名称
            List<string> ParamNames = new List<string>(Gnsser.ParamNames.Dxyz);

            //ParamNames.Add("ΔdtrC");//钟差等效距离，此变量变化异常，其统计特性采用高斯噪声。

            foreach (var item in this.Epoches)//卫星已经没有钟差，只有接收机钟差互差
            {
                ParamNames.Add(GetSingleDifferTimedClockParamName(item));//.ToShortTimeString() + Gnsser.ParamNames.SplittedDifferRcvClkErrDistance);//一个时刻一个钟差
            }
            foreach (var item in this.EnabledPrns)
                ParamNames.Add(GetSingleDifferSatAmbiParamName(item));// + Gnsser.ParamNames.SplittedDifferAmbiguitySuffix);
            return ParamNames; 
        }



        public override string GetParamName(SatelliteNumber prn)
        {
            return GetSingleDifferSatAmbiParamName(prn);
        }
    }
}