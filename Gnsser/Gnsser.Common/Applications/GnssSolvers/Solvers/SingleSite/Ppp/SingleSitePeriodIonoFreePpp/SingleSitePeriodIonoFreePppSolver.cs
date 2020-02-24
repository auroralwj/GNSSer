//2017.09.18, czs, create in hongqing, 单站多历元单频GNSS计算

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Geo.IO;
using Geo.Algorithm;

namespace Gnsser.Service
{
    /// <summary>
    ///  单站多历元单频GNSS计算
    /// </summary>
    public class SingleSitePeriodIonoFreePppSolver : AbstracSingleSitePeriodPositioner
    {
        #region 构造函数
        /// <summary>
        ///单站多历元单频GNSS计算
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public SingleSitePeriodIonoFreePppSolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
            : base(DataSourceContext, GnssOption)
        {
            //InitDetect(); 
            this.Name = "单站多历元单频GNSS计算";
            this.MatrixBuilder = new SingleSitePeriodIonoFreePppMatrixBuilder(GnssOption);
        }
         
        #endregion  
         

        /// <summary>
        /// 结果,固定解
        /// </summary>
        /// <returns></returns>
        public override  BaseGnssResult BuildResult()
        {
            return new SingleSitePeriodIonoFreePppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }
}
