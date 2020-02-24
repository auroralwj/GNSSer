//2014.08.29, czs, edit, 行了继承设计
//2014.12.08, czs, edit, 再次提升为通用定位结果
//2016.10.04, czs, refactor in hongqing, 全新继承设计
//2017.09.18, czs, edit in hongqing, 单站多历元单频GNSS计算

using System;
using System.Collections.Generic;
using Gnsser.Times;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Algorithm;
using Geo.Times; 
using Geo.IO;

namespace Gnsser.Service
{
    /// <summary>
    ///单站多历元单频GNSS计算
    /// </summary>
    public class SingleSitePeriodIonoFreePppResult : BaseGnssResult<PeriodInformation>
    {
        /// <summary>
        /// 单站多历元单频GNSS计算
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="adjust"></param>
        /// <param name="nameBuilder"></param>
        public SingleSitePeriodIonoFreePppResult(PeriodInformation epochInfo, AdjustResultMatrix adjust, GnssParamNameBuilder nameBuilder)
            : base(epochInfo, adjust, nameBuilder)
        {

        }


    }
}