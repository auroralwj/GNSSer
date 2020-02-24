//2017.07.15, czs, create in hongqing, 双差网解定位框架搭建

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
    /// 双差网解定位。
    ///  </summary>
    public class NetDoubleDifferPositioner : MultiSiteEpochSolver
    {
        #region 构造函数
        public NetDoubleDifferPositioner(DataSourceContext DataSourceContext, GnssProcessOption model)
            : base(DataSourceContext, model)
        {
            this.MatrixBuilder = new NetDoubleDifferMatrixBuilder(model);
        }


        #endregion
 
         
        public override BaseGnssResult BuildResult()
        {
            NetDoubleDifferResult result = new NetDoubleDifferResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
           // result.PreviousResult = this.CurrentProduct;
            result.BasePrn = this.CurrentBasePrn;
            return result;
        } 

    }
}
