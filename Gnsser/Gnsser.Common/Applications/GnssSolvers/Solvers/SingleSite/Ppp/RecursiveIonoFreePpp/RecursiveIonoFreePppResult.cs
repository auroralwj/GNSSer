//2014.08.29, czs, edit, 行了继承设计
//2018.10.16, czs, edit in hmx, 增加固定参数内容

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 递归最小二乘精密单点定位结果。
    /// </summary>
    public class RecursiveIonoFreePppResult : SingleSiteGnssResult, IFixableParamResult// PhasePositionResult
    {
        /// <summary>
        /// 精密单点定位结果构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="positioner">名称</param>
        /// <param name="baseParamCount">基础参数数量</param>
        public RecursiveIonoFreePppResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder positioner,
            int baseParamCount = 5
            )
            : base(receiverInfo, Adjustment,  positioner)
        { 
        }

        /// <summary>
        /// 固定后的模糊度,单位周，值应该为整数。
        /// </summary>
        public WeightedVector FixedParams { get; set; }  
         
        /// <summary>
        /// 获取模糊度参数估计结果。单位依然为米。
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetFixableVectorInUnit()
        {
            this.ResultMatrix.Estimated.ParamNames = this.ResultMatrix.ParamNames;
    
            var ambiParamNames = new List<string>();
            foreach (var name in ResultMatrix.ParamNames)
            {
                //if (HasUnstablePrn(name)) { continue; }

                if (name.Contains(Gnsser.ParamNames.AmbiguityLen))
                { ambiParamNames.Add(name); }
            }
            var estVector = this.ResultMatrix.Estimated.GetWeightedVector(ambiParamNames);

            return estVector;
        }
    }
}