//2014.08.29, czs, edit, 行了继承设计

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using  Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser.Service
{

    /// <summary>
    /// 伪距单点定位结果。
    /// </summary>
    public class RangePointPositionResult : SingleSiteGnssResult
    {
        /// <summary>
        /// 以观测信息进行初始化
        /// </summary>
        /// <param name="epochInfo">历元观测信息</param>
        /// <param name="PointPositionType">单点定位类型</param>
        public RangePointPositionResult(
            EpochInformation epochInfo,
            AdjustResultMatrix Adjustment, GnssParamNameBuilder positioner)
            : base(epochInfo, Adjustment,positioner)
        { 
        }

        
        /// <summary>
        /// 精度衰减因子。
        /// </summary>
        
        

        public override string ToString()
        {
            return base.ToString() + ",  "+ DilutionOfPrecision.ToString();
        }


    }
}