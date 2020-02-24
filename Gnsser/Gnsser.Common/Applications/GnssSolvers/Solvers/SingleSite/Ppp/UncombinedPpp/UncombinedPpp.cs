//2014.08.30, czs, edit, 开始重构
//2014.08.31, czs, edit, 重构于 西安 到 沈阳 的航班上，春秋航空。

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
    //包含4类参数，测站位置（x,y,z），钟差（Cdt），对流程天顶距延迟(zpd)和非整的整周模糊度（N）。

    /// <summary>
    /// 精密单点定位。
    /// 此处采用观测值残差向量计算。
    /// 条件：必须是双频观测，且观测卫星数量大于5个。
    /// 参考：Jan Kouba and Pierre Héroux. GPS Precise Point Positioning Using IGS Orbit Products[J].2000,sep
    /// </summary>
    public class UncombinedPpp : SingleSiteGnssSolver
    {
        #region 构造函数 
        
        /// <summary>
        /// 最简化的构造函数，可以多个定位器同时使用的参数，而不必多次读取
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public UncombinedPpp(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "非差非组合PPP";
            this.BaseParamCount = 6;

            if (PositionOption.ApproxDataType == SatApproxDataType.ApproxPseudoRangeOfTriFreq || PositionOption.ApproxDataType == SatApproxDataType.ApproxPhaseRangeOfTriFreq)
                this.MatrixBuilder = new IonoFreePppMatrixBuilder(PositionOption);
            //this.MatrixBuilder = new IonoFreePppOfTriFreqMatrixBuilder(PositionOption);
            else this.MatrixBuilder = new UncombinedPppMatrixBuilder(PositionOption); 
        }

        #endregion

        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            //return new PppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
            return new UncombinedPppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }

        //#region 卡尔曼滤波
        ///// <summary>
        ///// PPP 计算核心方法。 Kalmam滤波。
        ///// 观测量的顺序是先伪距观测量，后载波观测量，观测量的总数为卫星数量的两倍。
        ///// 参数数量为卫星数量加5,卫星数量对应各个模糊度，5为3个位置量xyz，1个接收机钟差量，1个对流程湿分量。
        ///// </summary>
        ///// <param name="recInfo">接收信息</param>
        ///// <param name="option">解算选项</param> 
        ///// <param name="lastPppResult">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        ///// <returns></returns>
        //public override SingleSiteGnssResult CaculateKalmanFilter(EpochInformation recInfo, SingleSiteGnssResult lastPppResult = null)
        //{ 
        //        PppResult last = null;
        //        if (lastPppResult != null) last = (PppResult)lastPppResult;
        //        //  ISatWeightProvider SatWeightProvider = new SatElevateAndRangeWeightProvider();
        //        ISatWeightProvider SatWeightProvider = new SatElevateWeightProvider();

        //        bool useInitApproxWeight = true;// CurrentIndex < 1;

        //        this.MatrixBuilder = new UncombinedPppMatrixBuilder(recInfo, this.Option, SatWeightProvider, StochasticModelMgr, last, useInitApproxWeight);

        //        if (this.MatrixBuilder.ObsCount > 0)
        //        {
        //            //this.Adjustment = new KalmanFilter( this.MatrixBuilder);
        //            this.Adjustment = new SimpleKalmanFilter(this.MatrixBuilder);
        //            this.Adjustment.Process();

        //            UncombinedPppResult result = new UncombinedPppResult(recInfo, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        //           // result.PreviousResult = lastPppResult;

        //            //模糊度设置
        //            //IonoFreeAmbiguityMgr.SetIonoFreeCombination(result);


        //            return result;
        //        }
        //        else
        //            return null; 
        //}

        //#endregion
    }
}
