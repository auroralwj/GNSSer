//2018.10.28, czs, create in HMX, 无电离层组合双差定轨

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

namespace Gnsser.Service
{
    /// <summary>
    ///  无电离层组合双差定轨
    /// </summary>
    public class DoubleDifferOrbitSolver : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public DoubleDifferOrbitSolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        :base( DataSourceContext,   GnssOption)
        { 
            this.Name = "双差定轨";
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

            this.MatrixBuilder = new DoubleDifferOrbitMatrixBuilder(Option, this.BaseParamCount);
        }
        #endregion

        /// <summary>
        /// 结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new DoubleDifferOrbitResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn, BaseParamCount);
            return result;
        }
    }
}
