//2017.08.28, czs & kyc, create in hongqing, 单站单历元GNSS计算预留测试类

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
using Gnsser.Filter;

namespace Gnsser.Service
{
    /// <summary>
    /// 单站单历元GNSS计算预留测试类
    /// </summary>
    public class SingeSiteGnssExtentSolver : SingleSiteGnssSolver
    {
        #region 构造函数
     
        /// <summary>
        /// 单站单历元GNSS计算预留测试类
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public SingeSiteGnssExtentSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "单站单历元GNSS计算预留测试类";
            this.BaseParamCount = 5;
            this.MatrixBuilder = new SingeSiteGnssExtentMatrixBuilder(PositionOption); 
        }
        #endregion  
          
        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            return new SingeSiteGnssExtentResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    } 
}