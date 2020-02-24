//2018.11.05, czs, create in HMX, 单差网解定位

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
using Gnsser;

namespace Gnsser.Service
{
    /// <summary>
    ///  单差网解定位
    /// </summary>
    public class NetSingleDifferPositionSolver : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public NetSingleDifferPositionSolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        :base( DataSourceContext,   GnssOption)
        { 
            this.Name = "单差网解定位";
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

            this.MatrixBuilder = new NetSingleDifferPositionMatrixBuilder(Option, this.BaseParamCount);
        }          
        #endregion         
    

        /// <summary>
        /// 结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new NetSingleDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn, BaseParamCount);
            return result;
        }
    }
}
