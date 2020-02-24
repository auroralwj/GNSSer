//2018.11.02, czs, create in HMX, 双差网解定位
//2018.11.07, czs, edit in hmx, 递归最小二乘双差网解定位

using System;
using System.IO;
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
using Gnsser;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    ///  双差网解定位
    /// </summary>
    public class RecursiveNetDoubleDifferPositionSolver : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public RecursiveNetDoubleDifferPositionSolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        :base( DataSourceContext,   GnssOption)
        { 
            this.Name = "递归双差网解定位";
            this.BaseParamCount = 5;

            this.IsBaseSatelliteRequried = true;//强制启用基准星
    
             var distanceOfBaseline = (DataSourceContext.ObservationDataSources.BaseDataSource.SiteInfo.ApproxXyz - DataSourceContext.ObservationDataSources.OtherDataSource.SiteInfo.ApproxXyz).Length;
             if (distanceOfBaseline <= GnssOption.MaxDistanceOfShortBaseLine)
            {
                this.BaseParamCount = 3;
            }
             else if (GnssOption.MaxDistanceOfShortBaseLine < distanceOfBaseline && distanceOfBaseline < GnssOption.MinDistanceOfLongBaseLine)
            {
                this.BaseParamCount = 4;
            }
            else
            {
                this.BaseParamCount = 5;
            }

            this.MatrixBuilder = new RecursiveNetDoubleDifferPositionMatrixBuilder(Option, this.BaseParamCount); 
        }
        #endregion
         
        /// <summary>
        /// 结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new RecursiveNetDoubleDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn, BaseParamCount);
            return result;
        }
    }
}
