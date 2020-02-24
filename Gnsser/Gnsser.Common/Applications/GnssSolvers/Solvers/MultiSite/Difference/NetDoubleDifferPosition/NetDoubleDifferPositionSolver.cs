//2018.11.02, czs, create in HMX, 双差网解定位

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
    public class NetDoubleDifferPositionSolver : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public NetDoubleDifferPositionSolver(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        :base( DataSourceContext,   GnssOption)
        { 
            this.Name = "双差网解定位";
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

            this.MatrixBuilder = new NetDoubleDifferPositionMatrixBuilder(Option, this.BaseParamCount);
        }
        #endregion
        
        public override BaseGnssResult CaculateKalmanFilter(MultiSiteEpochInfo epochInfo, BaseGnssResult last = null)
        {
            var result = base.CaculateKalmanFilter(epochInfo, last);
            //外部模糊度文件直接固定
            if (IsFixingAmbiguity && Option.IsUseFixedParamDirectly && File.Exists(Option.AmbiguityFilePath) && Option.IsUsingAmbiguityFile)
            {
                return result;
            }

            return result;
        }
         
        /// <summary>
        /// 默认采用Lambda算法直接固定。
        /// 如果是无电离层组合，则需要分别对待，不能直接固定，需要子类进行实现，//2018.11.06，czs， hmx
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <returns></returns>
        protected override WeightedVector DoFixAmbiguity(WeightedVector rawFloatAmbiCycles)
        {
            return DoFixIonoFreeDoubleDifferAmbiguity(rawFloatAmbiCycles, true);
        }
       

        /// <summary>
        /// 结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new NetDoubleDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn, BaseParamCount);
            return result;
        }
    }
}
