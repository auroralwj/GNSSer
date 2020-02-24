//2016.10.26, czs, create in hongqing, 搭建单历元相位双差计算框架
//2018.07.26, czs, create in HMX, 简易近距离单历元载波相位双差
//2018.12.30, czs, create in hmx, 单历元纯相位双差


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
    ///  简易近距离单历元载波相位双差
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站)
    /// </summary>
    public class EpochDoublePhaseDifferPositioner : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        public EpochDoublePhaseDifferPositioner(DataSourceContext Context, GnssProcessOption Option)
        :base( Context,   Option)
        {   
            this.Name = "单历元载波相位双差";
            
            //默认双差基础参数为3个坐标坐标
            var distanceOfBaseline = (Context.ObservationDataSources.BaseDataSource.SiteInfo.ApproxXyz - Context.ObservationDataSources.OtherDataSource.SiteInfo.ApproxXyz).Length;
            if (distanceOfBaseline <= Option.MaxDistanceOfShortBaseLine)
            {
                this.BaseParamCount = 3;
            }
            else if (distanceOfBaseline > Option.MaxDistanceOfShortBaseLine  && distanceOfBaseline < Option.MinDistanceOfLongBaseLine)
            {
                this.BaseParamCount = 4;
            }

            if (distanceOfBaseline > Option.MaxDistanceOfShortBaseLine)
            {
                this.BaseParamCount = 4;
                log.Warn("基线距离有点长： " + distanceOfBaseline.ToString("0.0000") +"m, 我们默认为  " + Option.MaxDistanceOfShortBaseLine + "m 内， 电离层可能影响精度，请使用无电离层计算。");
            }

            if (this.Option.IsEstimateTropWetZpd)
            {
                this.BaseParamCount = 4;
                log.Warn("指定了对流层湿延迟估计");
            }

            this.IsBaseSatelliteRequried = true;//强制启用基准星 
            this.MatrixBuilder = new EpochDoublePhaseDifferMatrixBuilder(base.Option, BaseParamCount);
        }

        #endregion

        #region 属性  
          

        #endregion         
         
        /// <summary>
        ///结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new EpochDoublePhaseDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn);
            return result;
        }
    }
}
