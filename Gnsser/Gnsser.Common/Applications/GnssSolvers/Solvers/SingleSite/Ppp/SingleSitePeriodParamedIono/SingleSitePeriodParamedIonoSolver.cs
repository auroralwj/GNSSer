//2017.11.08, czs, create in hongqing,单站单频多历元电离层参数化定位

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
    ///  单站单频多历元电离层参数化定位
    /// </summary>
    public class SingleSitePeriodParamedIonoSolver : AbstracSingleSitePeriodPositioner
    {
        #region 构造函数
        /// <summary>
        /// 单站多历元GNSS计算预留测试类
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public SingleSitePeriodParamedIonoSolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
            : base(DataSourceContext, GnssOption)
        {
            //InitDetect(); 
            this.Name = "单站单频多历元电离层参数化定位";
            this.MatrixBuilder = new SingleSitePeriodParamedIonoMatrixBuilder(GnssOption);
        }
         
        #endregion  
         

        /// <summary>
        /// 结果,固定解
        /// </summary>
        /// <returns></returns>
        public override  BaseGnssResult BuildResult()
        {
            return new SingleSitePeriodInfoGnssResult(this.CurrentMaterial, Adjustment,this.MatrixBuilder.GnssParamNameBuilder);
        }
    }
}
