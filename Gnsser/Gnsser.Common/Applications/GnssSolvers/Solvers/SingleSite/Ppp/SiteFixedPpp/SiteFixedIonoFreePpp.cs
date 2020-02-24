//2014.08.30, czs, edit, 开始重构
//2014.08.31, czs, edit, 重构于 西安 到 沈阳 的航班上，春秋航空。
//2015.10.25, czs, eidt in pengzhou, Ppp 重命名为 IonoFreePpp
//2016.01.31, czs, edit in hongqing, 修复精度，简化流程
//2016.03.10, czs, edit in hongqing, 重构设计

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
using Geo.Times;
using Gnsser.Filter;
using System.IO;
using Gnsser.Data;

namespace Gnsser.Service
{
    /// <summary>
    ///固定参考站的精密单点定位
    /// </summary>
    public class SiteFixedIonoFreePpp : SingleSiteGnssSolver
    {
        #region 构造函数
        /// <summary>
        /// 固定参考站的精密单点定位
        /// </summary>
        /// <param name="DataSourceContext">观测数据源</param>
        /// <param name="PositionOption">解算模型</param>
        public SiteFixedIonoFreePpp(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        { 
            this.Name = "固定站模糊度PPP";
            this.BaseParamCount = 2;
            //this.BufferSize = 40;
            this.MatrixBuilder = new SiteFixedIonoFreePppMatrixBuilder(PositionOption);
             
            if (this.IsFixingAmbiguity)
            {
                this.WideLaneBiasService = new WideLaneBiasService(Setting.GnsserConfig.IgnWideLaneFile);
                this.IsBaseSatelliteRequried = true;
            } 
        }
        #endregion
        //武大FCB验证 2015.01.01-2015.05
        FcbDataService FcbDataService { get; set; }
        WideLaneBiasService WideLaneBiasService { get; set; } 



        /// <summary>
        /// 默认采用Lambda算法直接固定。
        /// 如果是无电离层组合，则需要分别对待，不能直接固定，需要子类进行实现，//2018.11.06，czs， hmx
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <returns></returns>
        protected override WeightedVector DoFixAmbiguity(WeightedVector rawFloatAmbiCycles)
        {
            return DoFixIonoFreeAmbiguity(rawFloatAmbiCycles, true);
        }
        #region 无电离层双差模糊度固定 

        /// <summary>
        /// 执行无电离层双差模糊度固定
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <param name="isNetSolve">是否网解</param>
        /// <returns></returns>
        protected WeightedVector DoFixIonoFreeAmbiguity(WeightedVector rawFloatAmbiCycles, bool isNetSolve)
        {
            //-----------以下为无电离层组合模糊度固定算法--------------------
            if (this.DataSourceContext.SiteSatPeriodDataService == null)
            {
                log.Warn("必须开启时段数据服务，才能实现无电离层模糊度固定！");
                return new WeightedVector();
            }
            var time = this.CurrentMaterial.ReceiverTime;

            //指定系统的无电离层组合参数计算器
            var IonoFreeAmbiguitySolver = IonoFreeAmbiguitySolverManager.GetOrCreate(CurrentBasePrn.SatelliteType);
            IonoFreeAmbiguitySolver.CheckOrInit(CurrentBasePrn, CurrentMaterial.ReceiverTime, !Option.IsLengthPhaseValue);

            //----------------------第一步 MW 宽巷星间单差 ------------------------ 
            var intMwDoubleDiffers = GetIntMwDiffersBeweenSat(isNetSolve);

            //----------------------第二步 MW 宽巷和模糊度浮点解求窄巷模糊度--------
            var ambiFloatVal = rawFloatAmbiCycles.GetNameRmsedNumeralVector();

            //求窄巷模糊度浮点解//单位周
            var narrowFloat = IonoFreeAmbiguitySolver.GetNarrowFloatValue(intMwDoubleDiffers, ambiFloatVal);

            //--------记录小数偏差部分------- 
            var satPeriodService = this.DataSourceContext.SiteSatPeriodDataService.Get(this.CurrentMaterial.SiteName);
            foreach (var item in narrowFloat)
            {
                var prn = SatelliteNumber.Parse( item.Key);
               satPeriodService.GetOrCreate(prn).Regist(CurrentBasePrn + "-" + ParamNames.NarrowLaneBsdCycle  , time, item.Value.Value, this.CurrentMaterial[prn].IsUnstable);
            } 

            return new WeightedVector();
        }

        /// <summary>
        /// 获取星间单差MW整数
        /// </summary>
        /// <param name="isNetSolve"></param>
        /// <returns></returns>
        public NameRmsedNumeralVector GetIntMwDiffersBeweenSat(bool isNetSolve)
        {
            var mwDifferVal = this.DataSourceContext.SiteSatPeriodDataService.GetMwCycleDifferValueBeweenSat(CurrentMaterial, CurrentBasePrn);

            var time = this.CurrentMaterial.ReceiverTime;
            var wmFcb = WideLaneBiasService.Get(time).GetMwDiffer(CurrentBasePrn);//用于模糊度固定，精度要求不高，一次任务获取一次即可

            var wmFcbVector = new NameRmsedNumeralVector<SatelliteNumber>(wmFcb);

            var mwDoubleDiffers = mwDifferVal - wmFcbVector;

            var toRemoves = new List<SatelliteNumber>();
            //移除RMS太大的数据，RMS太大可能发生了周跳。
            toRemoves = mwDoubleDiffers.GetRmsLargerThan(this.Option.MaxAllowedRmsOfMw);
            mwDoubleDiffers.Remove(toRemoves);//移除

            //直接四舍五入，求MW双差模糊度
            var intMwDoubleDiffers = mwDoubleDiffers.GetRound();

            ////求残差,由于前面采用四舍五入的方法求取，因此，大于0.5的判断是没有意义的
            if (Option.MaxRoundAmbiDifferOfIntAndFloat < 0.5)    //移除残差大于指定的数据
            {
                var residuals = mwDoubleDiffers - intMwDoubleDiffers;
                toRemoves = residuals.GetAbsLargerThan(Option.MaxRoundAmbiDifferOfIntAndFloat);
                intMwDoubleDiffers.Remove(toRemoves);//移除
            }

            return ToStringVector(intMwDoubleDiffers);
        }

        /// <summary>
        /// 解析名称
        /// </summary>
        /// <param name="satVector"></param>
        /// <returns></returns>
        public NameRmsedNumeralVector ToStringVector(NameRmsedNumeralVector<SatelliteNumber> satVector)
        {
            IonoFreePppParamNameBuilder nameBuilder = new IonoFreePppParamNameBuilder(this.Option);
            NameRmsedNumeralVector result = new NameRmsedNumeralVector();
            foreach (var item in satVector)
            {
                var name = nameBuilder.GetParamName(item.Key);
                result[name] = item.Value;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 滤波计算
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="lastResult"></param>
        /// <returns></returns>
        public override SingleSiteGnssResult CaculateKalmanFilter(EpochInformation epochInfo, SingleSiteGnssResult lastResult)
        {
            //极速模式。
            epochInfo.RemoveIonoFreeUnavailable();
            if (epochInfo.Count < 2)
            {
                log.Error("卫星可用数量不足：" + epochInfo.Count);
                return null;
            }
            var result = base.CaculateKalmanFilter(epochInfo, lastResult) as PppResult;

            //外部模糊度文件直接固定
            if (Option.IsFixingAmbiguity && Option.IsUseFixedParamDirectly && File.Exists(Option.AmbiguityFilePath) && Option.IsUsingAmbiguityFile)
            {
                return result;
            }


            return result;
        }


        /// <summary>
        /// 生成结果
        /// </summary> 
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            return new PppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, 2) ;//{ PreviousResult = this.CurrentProduct };
        }


















    } 
}
