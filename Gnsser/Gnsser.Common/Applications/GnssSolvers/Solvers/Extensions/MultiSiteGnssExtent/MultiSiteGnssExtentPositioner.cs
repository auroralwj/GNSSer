//2017.08.28, czs & kyc, create in hongqing, 多站单历元GNSS计算预留测试类

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Gnsser.Checkers;
using Geo.Algorithm;
using Geo.Times;
using Gnsser.Service;

namespace Gnsser
{
   
    /// <summary>
    /// 多站单历元GNSS计算预留测试类。
    ///  </summary>
    public class MultiSiteGnssExtentPositioner : MultiSiteEpochSolver
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="model"></param>
        public MultiSiteGnssExtentPositioner(DataSourceContext DataSourceContext, GnssProcessOption model)
            : base(DataSourceContext, model)
        {
            this.MatrixBuilder = new MultiSiteGnssExtentMatrixBuilder(model);
        }


        #endregion
 
         
        public override BaseGnssResult BuildResult()
        {
            MultiSiteGnssExtentResult result = new MultiSiteGnssExtentResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
           // result.PreviousResult = this.CurrentProduct;
            result.BasePrn = this.CurrentBasePrn;
            return result;
        } 

    }
}
