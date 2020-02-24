//2014.08.30, czs, edit, 开始重构
//2014.08.31, czs, edit, 重构于 西安 到 沈阳 的航班上，春秋航空。
//2015.10.25, czs, eidt in pengzhou, Ppp 重命名为 IonoFreePpp
//2016.01.31, czs, edit in hongqing, 修复精度，简化流程
//2016.03.10, czs, edit in hongqing, 重构设计
//2018.08.03, czs, edit in hmx, 动态定位实现

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
    //包含4类参数，测站位置（x,y,z），钟差（Cdt），对流程天顶距延迟(zpd)和非整的整周模糊度（N）。

    /// PPP 计算核心方法。 Kalmam滤波。
    /// 观测量的顺序是先伪距观测量，后载波观测量，观测量的总数为卫星数量的两倍。
    /// 参数数量为卫星数量加5,卫星数量对应各个模糊度，5为3个位置量xyz，1个接收机钟差量，1个对流程湿分量。
    /// <summary>
    /// 精密单点定位。
    /// 此处采用观测值残差向量计算。
    /// 条件：必须是双频观测，且观测卫星数量大于5个。
    /// 参考：Jan Kouba and Pierre Héroux. GPS Precise Point Positioning Using IGS Orbit Products[J].2000,sep
    /// </summary>
    public class IonoFreePpp : SingleSiteGnssSolver
    {
        #region 构造函数

        /// <summary>
        /// 最简化的构造函数，可以多个定位器同时使用的参数，而不必多次读取
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public IonoFreePpp(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "无电离层组合PPP";
            this.BaseParamCount = 5;
            if (PositionOption.ApproxDataType == SatApproxDataType.ApproxPseudoRangeOfTriFreq || PositionOption.ApproxDataType == SatApproxDataType.ApproxPhaseRangeOfTriFreq)
            { this.MatrixBuilder = new IonoFreePppOfTriFreqMatrixBuilder(PositionOption); }
            else { this.MatrixBuilder = new IonoFreePppMatrixBuilder(PositionOption); }

            if (!Option.TopSpeedModel)
            {
                if (this.IsFixingAmbiguity)
                {
                    this.WideLaneBiasService = new WideLaneBiasService(Setting.GnsserConfig.IgnWideLaneFile);
                    this.IsBaseSatelliteRequried = true;
                }
                if (true)
                {
                    if (!this.Option.IsUseFixedParamDirectly && (this.IsFixingAmbiguity && this.DataSourceContext.GnsserFcbOfUpdService == null))
                    {
                        throw new Exception("PPP模糊度固定，请设置FCB文件路径!");
                    }
                }
                else
                {
                    FcbDataService = new FcbDataService("E:\\");
                }
                NarrawLaneFcbService = this.DataSourceContext.GnsserFcbOfUpdService;// new FcbOfUpdService( Option.GnsserFcbFilePath);

            }
        }
        #endregion
        //武大FCB验证 2015.01.01-2015.05
        FcbDataService FcbDataService { get; set; }
        WideLaneBiasService WideLaneBiasService { get; set; }
        FcbOfUpdService NarrawLaneFcbService{ get; set; }



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

            //----------------------第一步 MW 宽巷双差 ------------------------ 
            var intMwDoubleDiffers = GetIntMwDiffersBeweenSat(isNetSolve);

            //----------------------第二步 MW 宽巷和模糊度浮点解求窄巷模糊度--------
            var ambiFloatVal = rawFloatAmbiCycles.GetNameRmsedNumeralVector();

            //求窄巷模糊度浮点解//单位周
            var narrowFloat = IonoFreeAmbiguitySolver.GetNarrowFloatValue(intMwDoubleDiffers, ambiFloatVal);

            //--------获取小数偏差部分-------
            Dictionary<SatelliteNumber, RmsedNumeral> nlFcbOfBsd = null;
            if (NarrawLaneFcbService == null)
            {
                  nlFcbOfBsd = FcbDataService.GetNLFcbOfBsd(time, CurrentBasePrn);
            }
            else
            {
                nlFcbOfBsd = NarrawLaneFcbService.GetBsdOfNarrowLane(this.CurrentMaterial.EnabledPrns, CurrentBasePrn, time);
            }

            var narrowIntFloat = ToStringVector(new NameRmsedNumeralVector<SatelliteNumber>( nlFcbOfBsd));

            var narrowNearInt = narrowFloat - narrowIntFloat;

            var narrowFloatVect = narrowNearInt.GetWeightedVector();
            narrowFloatVect.InverseWeight = rawFloatAmbiCycles.GetWeightedVector(narrowFloatVect.ParamNames).InverseWeight; //追加系数阵，按照顺序------ 


            //--------------------------------窄巷模糊度浮点数减去小数部分-----------------------------------------
            //方法1：
            var intNarrowVector = base.DoFixAmbiguity(narrowFloatVect);
            var intNarrow = intNarrowVector.GetNameRmsedNumeralVector();// ParseVector(intNarrowVector); 
            //方法2：直接取整
            //var intNarrow = narrowFloatVect.GetRound();//不推荐使用直接取整 

            //检核窄巷
            var intDiffer = intNarrow - narrowFloat;
            var toRemoves = intDiffer.GetAbsLargerThan(this.Option.MaxAmbiDifferOfIntAndFloat);
            intNarrow.Remove(toRemoves);//移除

            //判断是否超限
            //计算双差载波模糊度固定值
            var fixedVal = IonoFreeAmbiguitySolver.GetIonoFreeAmbiValue(intMwDoubleDiffers, intNarrow);

            var result = fixedVal.GetWeightedVector();

            return result;
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

            return  ToStringVector(intMwDoubleDiffers);
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
            if( Option.IsFixingAmbiguity && Option.IsUseFixedParamDirectly && File.Exists( Option.AmbiguityFilePath) && Option.IsUsingAmbiguityFile)
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
            return new PppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }

    /// <summary>
    /// 卫星宽巷窄巷数据维护器。
    /// </summary>
    public class SatWideNarrowValueManager : BaseDictionary<SatelliteNumber, WideNarrowValue>
    {
        /// <summary>
        /// 卫星宽巷窄巷数据维护器
        /// </summary>
        public SatWideNarrowValueManager()
        {

        }
        public override WideNarrowValue Create(SatelliteNumber key)
        {
            return new WideNarrowValue();
        }
    }

    /// <summary>
    /// 卫星宽窄项数值。
    /// </summary>
    public class WideNarrowValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WideNarrowValue()
        {
            NarrowLane = new IntFractionNumber();
            WideLane = new IntFractionNumber();

        }
        /// <summary>
        /// 是否都具有值。
        /// </summary>
        public bool IsValid { get=> !NarrowLane.IsZero && !WideLane.IsZero; }
        /// <summary>
        /// 星间单差浮点解模糊度长度，单位：米
        /// </summary>
        public double FloatAmbiguityLength { get; set; }
        /// <summary>
        /// 星间单差固定解模糊度长度，单位：米
        /// </summary>
        public double FixedAmbiguityLength { get; set; }
        /// <summary>
        /// 固定解与浮点解的偏差
        /// </summary>
        public double DifferOfAmbiguityLength { get => FloatAmbiguityLength - FixedAmbiguityLength; } 

        /// <summary>
        /// 星间单差窄巷，单位：周
        /// </summary>
        public IntFractionNumber NarrowLane { get; set; }
        /// <summary>
        /// 星间单差宽巷，单位：周
        /// </summary>
        public IntFractionNumber WideLane { get; set; }

    }

     
}