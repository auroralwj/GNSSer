//2017.09.09, czs, create in hongqing, 通用单站单频计算，高度可配置

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

namespace Gnsser.Service
{ 
    /// <summary>
    /// 通用单站单频计算，高度可配置
    /// </summary>
    public class CommonSingeFreqGnssSolver : SingleSiteGnssSolver
    {
        #region 构造函数 
        
        /// <summary>
        ///  通用单站单频计算，高度可配置 
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public CommonSingeFreqGnssSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "通用单站单频计算";
             this.MatrixBuilder = new CommonSingeFreqGnssMatrixBuilder(PositionOption); 
        }

        #endregion

        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
             return new CommonSingeFreqGnssResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }
}
