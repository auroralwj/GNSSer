//2017.08.28, czs & kyc, create in hongqing, 多站多历元GNSS计算预留测试类

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
    ///  多站多历元GNSS计算预留测试类
    /// </summary>
    public class PeriodMultiSiteGnssExtentSolver : AbstracMultiSitePeriodPositioner
    {
        #region 构造函数
        /// <summary>
        /// 多站多历元GNSS计算预留测试类
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public PeriodMultiSiteGnssExtentSolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
            : base(DataSourceContext, GnssOption)
        {
            //InitDetect(); 
            this.IsBaseSatelliteRequried = true;//强制启用基准星 
            this.Name = "多站多历元GNSS计算预留测试类";
            this.BaseParamCount = 3;
            this.MatrixBuilder = new PeriodDoubleDifferMatrixBuilder(GnssOption);
        }
         
        #endregion  

      
         
        /// <summary>
        /// 结果,固定解
        /// </summary>
        /// <returns></returns>
        public override  BaseGnssResult BuildResult()
        {            
            return new PeriodMultiSiteGnssExtentPositionResult(this.CurrentMaterial, Adjustment, this.CurrentBasePrn, Option, this.MatrixBuilder.GnssParamNameBuilder);
        } 
    }
}
