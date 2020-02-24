//2018.10.24, czs, create in hmx, 非差轨道确定


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Geo.Algorithm.Adjust;
using Geo; 
using Geo.Times;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 非差轨道确定
    /// </summary>
    public class ZeroDifferOrbitResult : RangeOrbitResult
    {
        /// <summary>
        /// 非差轨道确定
        /// </summary>
        /// <param name="epochInfo">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        /// <param name="ClockEstimationer">钟差估计器</param>
        /// <param name="previousResult">上一历元结果</param>
        public ZeroDifferOrbitResult(
            MultiSiteEpochInfo epochInfo,
            AdjustResultMatrix Adjustment,
            ZeroDifferOrbitParamNameBuilder ClockEstimationer,
            MultiSiteGnssExtentResult previousResult = null)
            : base(epochInfo, Adjustment, ClockEstimationer)
        { 
        }   
    }  
    
}
