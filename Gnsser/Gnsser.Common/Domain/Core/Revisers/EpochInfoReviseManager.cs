//2015.02.08, cuiyang edit in 武大图书馆, 星历赋值与钟差赋值
//2015.10.26, czs, refactor in 洪庆,  历元处理管理器
//2016.05.02, czs, edit in hongqing, 重构
//2017.09.05, czs, edit in hongqing, 电离层模型改正
//2017.10.25, czs, edit in hongqing, 优先考虑周跳探测器开关

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Geo.IO;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Correction;
using Gnsser.Filter;

namespace Gnsser
{ 
    /// <summary>
    /// 历元信息处理器
    /// </summary>
    public class EpochInfoReviseManager : ReviserManager<EpochInformation>
    {
        static Log log = new Log(typeof(EpochInfoReviseManager));
       
        /// <summary>
        /// 构造函数
        /// </summary> 
        public EpochInfoReviseManager( ) {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="PositionOption"></param>
        public EpochInfoReviseManager(DataSourceContext Context,
            GnssProcessOption PositionOption)
        {
            this.Context = Context;
            this.Option = PositionOption;
        }

        #region 核心属性
        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext Context { get; set; }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        #endregion

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<IReviser<EpochInformation>> FindAllByKey(string key)
        {
            List<IReviser<EpochInformation>> list = new List<IReviser<EpochInformation>>();
            foreach (var precessor in this.Precessors)
            {
                if (precessor.Name.Contains(key))
                {
                    list.Add(precessor);
                }                
            }
            return list;
        }

        public override void Init()
        {
            base.Init();
            foreach (var item in Precessors)
            {
                item.Init();
            }
        }
        /// <summary>
        /// 结束时调用
        /// </summary>
        public override void Complete()
        {
            base.Complete();
            foreach (var item in Precessors)
            {
                item.Complete();
            }
        }
         
        #region 实例应用
        /// <summary>
        /// 默认的，对原始观测数据源进行处理。这是最全的矫正器，
        /// 包含检核、星历赋予、改正数赋予等。
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <param name="SatTimeInfoManager"></param>
        /// <returns></returns>
        public static EpochInfoReviseManager GetDefaultEpochInfoReviser(DataSourceContext Context, GnssProcessOption Option, SatTimeInfoManager SatTimeInfoManager)
        {
            EpochInfoReviseManager processer = GetFirstStepEpochInfoReviser(Context, Option);
            processer.AddProcessor(GetProducingReviser(Context, Option, SatTimeInfoManager));

            return processer;
        }

        /// <summary>
        /// 获取初步的历元信息矫正器，包含观测值教研、星历赋予、周跳探测与标记等。
        /// 在分开的历元信息校验处理中，本方法应该在第一次执行。
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <param name="withCycleSlip"></param>
        /// <returns></returns>
        public static EpochInfoReviseManager GetFirstStepEpochInfoReviser(DataSourceContext Context, GnssProcessOption Option, bool withCycleSlip=true)
        {
            EpochInfoReviseManager processer = GetRangeOnlyEpochInfoReviser(Context, Option);
            if (Option.IsEnableSatAppearenceService)
            {
                //时间有待确定
                if(Context.SiteSatAppearenceService == null)//多站情况下，可能已经赋值，因此需要判断
                {
                    Context.SiteSatAppearenceService = new SiteSatAppearenceService(Option.MaxBreakingEpochCount);
                }
                var reviser = new SiteSatAppearenceServiceRegister(Context.SiteSatAppearenceService, Option.MaxBreakingEpochCount);
                processer.AddProcessor(reviser);
            }
            if (Option.IsEnableSiteSatPeriodDataService)
            {
                //时间有待确定
                if(Context.SiteSatPeriodDataService == null)//多站情况下，可能已经赋值，因此需要判断
                {
                    Context.SiteSatPeriodDataService = new SiteSatPeriodDataService(Option.MaxBreakingEpochCount);
                }
                var reviser = new SiteSatParamDataServiceRegister(Context.SiteSatPeriodDataService, Option.MaxBreakingEpochCount,  Option.IsOutputPeriodData, Option.OutputDirectory);
                processer.AddProcessor(reviser);
            }


            if (Option.IsRemoveSmallPartSat)
            {
                if (Option.MinContinuouObsCount > Option.BufferSize)
                {
                    throw new ArgumentException("缓存小于最小连续卫星数，将剔除所有数据！");
                }
                var reviser = new SatSpanFilter(Option.MinContinuouObsCount, Option.MaxBreakingEpochCount, Geo.Times.Time.MaxValue);
                processer.AddProcessor(reviser);
            }

            if (Option.IsAliningPhaseWithRange)
            {
                processer.AddProcessor(new AliningIonoFreePhaseProcessor());
            }
             

            //通用站星距离改正,用于缓冲计算
            //RangeCorrectionReviser rangeCorrector = new RangeCorrectionReviser(CorrectChianType.Common);
            //processer.AddProcessor(rangeCorrector);
            //if (Option.IsTropCorrectionRequired)
            //{
            //    rangeCorrector.Add(new TroposphericModelCorrector(Option, Context)); //对流层改正 2,ZUIXINJIARU
            //}

            return processer;
        }
        /// <summary>
        /// 获取伪距校准器。包含星历 设置、
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <returns></returns>
        public static EpochInfoReviseManager GetRangeOnlyEpochInfoReviser(DataSourceContext Context, GnssProcessOption Option)
        {
            EpochInfoReviseManager processer = new EpochInfoReviseManager(Context, Option);
            IEphemerisService EphemerisService = Context.EphemerisService;
            //注意和哪些数据源相关
            #region 首先对观测值进行过滤


            if (Option.IsExcludeMalfunctioningSat)
            {
                processer.AddProcessor(new GnssSysRemover(Option));
                 processer.AddProcessor(new ZeroObsRemover());
                if (Context.SatStateDataSource != null) { processer.AddProcessor(new ObsSatExcludeFilter(Context.SatStateDataSource)); }
            }
       
            if (Option.IsRemoveOrDisableNotPassedSat)
            {
                processer.AddProcessor(new IonoFreeUnavailableRemover());
            }

            if (Option.IsEnableRemoveSats)
            {
                processer.AddProcessor(new SatelliteRemover(Option));
            }


            #endregion

            #region 星历、钟差赋值和检核过滤
            if (Option.IsEphemerisRequired)
            {
                processer.AddProcessor(new EpochEphemerisSetter(Context, Option));//星历赋值与钟差赋值 cuiyang 2015.02.08 武大图书馆
                processer.AddProcessor(new EphemerisFilter(EphemerisService, Option.VertAngleCut));//星历过滤应该在星历赋值后执行

                if (Option.IsDisableEclipsedSat)
                {
                    processer.AddProcessor(new EclipsedSatFilter());
                }
            }
            #endregion
            return processer;
        }

        /// <summary>
        /// 获取默认的周跳探测标记器。
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <returns></returns>
        public static EpochInfoReviseManager GetDefaultCycleSlipDetectReviser(DataSourceContext Context, GnssProcessOption Option)
        {
            EpochInfoReviseManager processer = new EpochInfoReviseManager(Context, Option);

            #region 周跳管理器，这个应该放在最后，以免有的数据漏标或错标

            //需要一个维护卫星状态的类，避免新卫星受老数据的影响 
            if (Option.IsCycleSlipDetectionRequired)
            {
                //优先考虑周跳探测器开关
                //若无，或全关，则选择默认。
                if (Option.CycleSlipDetectSwitcher.Count != 0 || Option.CycleSlipDetectSwitcher.Values.Count(m=>!m) !=0 )
                {
                    CycleSlipDetectReviser resise = new CycleSlipDetectReviser();
                    foreach (var item in Option.CycleSlipDetectSwitcher)
                    {
                        if(item.Value){
                            var obj = CycleSlipDetectReviser.Create(item.Key, Option);
                            resise.Add(obj);
                        }
                    }
                    processer.AddProcessor(resise);
                    log.Info("采用了指定的周跳探测方法。");
                }

                if(processer.Precessors.Count == 0)                
                {
                    //首先根据观测类型设置周跳探测器
                    if (Option.ObsDataType == SatObsDataType.PhaseA
                        || Option.ObsDataType == SatObsDataType.PhaseRangeA
                        || Option.ObsDataType == SatObsDataType.PhaseB
                        || Option.ObsDataType == SatObsDataType.PhaseRangeB
                        || Option.ObsDataType == SatObsDataType.PhaseC
                       || Option.ObsDataType == SatObsDataType.PhaseRangeC)
                    {
                        if (Option.GnssSolverType == GnssSolverType.非差非组合PPP)
                        {
                            processer.AddProcessor(CycleSlipDetectReviser.DefaultDoubleFrequencyDetector(Option));

                            log.Info("采用了双频默认周跳探测方法。");
                        }
                        else
                        {
                            processer.AddProcessor(CycleSlipDetectReviser.DefaultSingeFrequencyDetector(Option));
                            log.Info("采用了单频默认周跳探测方法。");
                        }                        
                    }
                    else if (Option.MinFrequenceCount >= 2)
                    {
                        //throw new NotSupportedException("周跳探测不应该出现在这里。");
                        processer.AddProcessor(CycleSlipDetectReviser.DefaultDoubleFrequencyDetector(Option));

                        log.Info("采用了双频默认周跳探测方法。");
                    }
                    if (Option.ApproxDataType == SatApproxDataType.ApproxPhaseRangeOfTriFreq || Option.ApproxDataType == SatApproxDataType.ApproxPseudoRangeOfTriFreq)
                    {
                     
                        processer.Clear();
                        processer.AddProcessor(CycleSlipDetectReviser.DefaultTripleFrequencyDetector());
                        log.Info("采用了三频默认周跳探测方法。");
                    }
                }
            }

            #endregion

            return processer;
        }

        /// <summary>
        /// 逆序探测与区段删除
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <param name="SatTimeInfoManager"></param>
        /// <returns></returns>
        public static EpochInfoReviseManager GetBufferedReviser(DataSourceContext Context, GnssProcessOption Option, SatTimeInfoManager SatTimeInfoManager)
        {
            EpochInfoReviseManager processer = new EpochInfoReviseManager(Context, Option);
            //这个方法被上面替代了，因此不再使用
            if (false && SatTimeInfoManager != null && Option.IsRemoveSmallPartSat)
            {
                if (Option.MinContinuouObsCount > Option.BufferSize)
                {
                    throw new ArgumentException("缓存小于最小连续卫星数，将剔除所有数据！");
                }
                processer.AddProcessor(new SatSpanFilterOld(SatTimeInfoManager, Option.MinContinuouObsCount));
            }

            if (Option.IsReverseCycleSlipeRevise) { processer.AddProcessor(new ReverseCycleSlipeReviser(Option)); }
            if (Option.IsEnableBufferCs)
            {
                processer.AddProcessor(new BufferedCycleSlipDetector(Option));
            }

            return processer;
        }

        #region  历元处理器（赋值器、过滤器、改正器)的配置
        /// <summary>
        /// 改正数,在即将执行 Producing 时触发。
        /// 包括周跳逆序探测，平滑伪距
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <param name="SatTimeInfoManager"></param>
        /// <returns></returns>
        public static EpochInfoReviseManager GetProducingReviser(DataSourceContext Context, GnssProcessOption Option, SatTimeInfoManager SatTimeInfoManager)
        {

            EpochInfoReviseManager processer = new EpochInfoReviseManager(Context, Option);

          //掐头去尾
            if (Option.IsBreakOffBothEnds)
            {
                processer.AddProcessor(new BreakOffBothEndsReviser(Context.SiteSatAppearenceService, Option.MinuteOfBreakOffBothEnds));
            }


            #region 周跳管理器，这个应该放在数据删除之后，以免有的数据漏标或错标,比如高度截止角出现后，会漏探

            if (Option.IsDetectClockJump) { processer.AddProcessor(new ClockJumpDetector(Option)); }

            if (Option.IsCycleSlipDetectionRequired) { processer.AddProcessor(GetDefaultCycleSlipDetectReviser(Context, Option)); }
            #endregion
            
            //周跳逆序探测，缓存探测，
            processer.AddProcessor(GetBufferedReviser(Context, Option, SatTimeInfoManager));




            var co =  GetCorrectionRevisers(Context, Option);
            co.AddProcessor(processer);
            return co;
        }

        /// <summary>
        ///模型改正
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <returns></returns>
        private static EpochInfoReviseManager GetCorrectionRevisers(DataSourceContext Context, GnssProcessOption Option)
        {
            EpochInfoReviseManager processer = new EpochInfoReviseManager(Context, Option);

            #region 观测值改正
            if (Option.IsObsCorrectionRequired)
            {
                processer.AddProcessor(BuildObsCorrector(Context, Option));
                if (Option.IsSmoothRange)
                {
                    switch (Option.SmoothRangeType)
                    {
                        case SmoothRangeType.PhaseSmoothRange:
                            processer.AddProcessor(new PhaseSmoothRangeReviser(Option));
                            break;
                        case SmoothRangeType.PolyfitRange:
                            processer.AddProcessor(new BufferPolyRangeSmoothReviser(Option));
                            break;
                        default:
                            log.Warn("打开了平滑伪距总开关，但是并没有指定具体的平滑算法。");
                            break;
                    } 
                }
            }
            #endregion

            #region 估值模型改正

            if (Option.IsApproxModelCorrectionRequired)
            {
                processer.AddProcessor(BuildModelCorrector(Context, Option));
            }
            #endregion
            return processer;
        }
        /// <summary>
        /// 观测值改正数设值。观测值改正直接添加到观测值上。
        /// </summary>
        /// <returns></returns>
        private static EpochInfoReviseManager BuildObsCorrector(DataSourceContext Context, GnssProcessOption Option)
        {
            var processer = new EpochInfoReviseManager(Context, Option);
            RangeCorrectionReviser rangeCorrector = new RangeCorrectionReviser(CorrectChianType.Self);
            if (Option.IsDcbCorrectionRequired && Context.DcbDataService != null)
            {
                rangeCorrector.Add(new DcbRangeCorrector(Context.DcbDataService, Option.IsDcbOfP1P2Enabled)); //DCB改正
            }

            if (Option.IsP1DcbToLcOfGridIonoRequired && Context.GridIonoDcbDataService != null)
            {
                rangeCorrector.Add(new IonoDcbP1Corrector(Context.GridIonoDcbDataService)); //P1 单频 DCB改正
            }


            processer.AddProcessor(rangeCorrector);
            return  processer;
        }

        /// <summary>
        /// 模型改正数对象，改正在估值上。
        /// </summary>
        /// <returns></returns>
        private static EpochInfoReviseManager BuildModelCorrector(DataSourceContext Context, GnssProcessOption Option)
        {
            var modelCorrectors = new EpochInfoReviseManager(Context, Option);

            //测站、地球相关改正
            if (Option.IsSiteCorrectionsRequired)
            {
                //接收机改正
                EpochNeuCorrectionReviser neuChain = new EpochNeuCorrectionReviser();
                modelCorrectors.AddProcessor(neuChain);
                if (Option.IsReceiverAntSiteBiasCorrectionRequired)
                {
                    neuChain.Add(new RecAntennaArpCorrector()); //接收机天线ARP改正，基线不可忽略
                }
                if (Option.IsOceanTideCorrectionRequired && Option.IsOceanLoadingFileRequired)
                {
                    neuChain.Add(new OceanTidesCorrector(Context));//海洋负荷（潮汐）改正，短基线可忽略
                }
                if (Option.IsPoleTideCorrectionRequired)
                {
                    neuChain.Add(new PoleTidesCorrector(Context));//极潮改正，短基线可忽略
                }
                if (Option.IsSolidTideCorrectionRequired)
                {
                    neuChain.Add(new SolidTidesCorrector2(Context)); //固体潮改正2，短基线可忽略
                }
            }
            //等效距离改正
            if (Option.IsRangeCorrectionsRequired)
            {
                //通用站星距离改正
                RangeCorrectionReviser rangeCorrector = new RangeCorrectionReviser(CorrectChianType.Common);
                modelCorrectors.AddProcessor(rangeCorrector);

                if (Option.IsSatClockBiasCorrectionRequired)
                {
                    rangeCorrector.Add(new SatClockBiasCorrector());//卫星钟差改正 
                }
                if (Option.IsTropCorrectionRequired)
                {//rangeCorrector.Add(new RelativeCorrector());//钟差的相对论改正，与光速相差转换为伪距改正 
                    //rangeCorrector.Add(new SagnacEffectCorrector());//地球自转的相对论改正，Sagnac effect 伪距改正 
                    // rangeCorrector.Add(new RangeTropoCorrector()); //对流层改正 1
                    //rangeCorrector.Add(new TroposphericModelCorrector()); //对流层改正 2
                    rangeCorrector.Add(new TroposphericModelCorrector( Option, Context)); //对流层改正 2,ZUIXINJIARU
                }
                if (Option.IsGravitationalDelayCorrectionRequired)
                {
                    rangeCorrector.Add(new GravitationalDelayCorrector()); //相对论效应：引力延迟效应// 
                }
            }
            //电离层等效距离改正
            if (Option.IsIonoCorretionRequired)//电离层总开关
            {
                SetIonoCorrectionReviser(Context, Option, modelCorrectors);
            }

            // 频率改正，需要天线
            if (Option.IsFrequencyCorrectionsRequired && Option.IsAntennaFileRequired)
            {
                //按卫星频率分类，对NEU改正
                FreqBasedNeuCorrectionReviser frequencyNeuCorrectorChain = new FreqBasedNeuCorrectionReviser();
                modelCorrectors.AddProcessor(frequencyNeuCorrectorChain);
                
                if (Option.IsRecAntPcoCorrectionRequired)
                {
                     frequencyNeuCorrectorChain.Add(new RecAntennaPcoCorrector());//测站天线 PCO NEU，对于每个天线和卫星频率是固定的
                }               
                
                //按照频率，对距离改正
                FreqBasedRangeCorrectionReviser frequencyRangCorrectorChain = new FreqBasedRangeCorrectionReviser();
                modelCorrectors.AddProcessor(frequencyRangCorrectorChain);

                if (Option.IsRecAntPcvCorrectionRequired)
                {
                    frequencyRangCorrectorChain.Add(new RecAntennaPcvCorrector());//测站天线 PCV
                }
                if (Option.IsSatAntPvcCorrectionRequired)
                {
                    frequencyRangCorrectorChain.Add(new SatAntennaPcvCorrector(Context));
                }

                if (Option.IsSatAntPcoCorrectionRequired)
                {
                    frequencyRangCorrectorChain.Add(new SatAntennaPcoCorrector(Context)); //lly增加卫星PCO
                }
                //相位改正
                PhaseCorrectionReviser phaseCorrector = new PhaseCorrectionReviser();
                modelCorrectors.AddProcessor(phaseCorrector);

                if (Option.IsPhaseWindUpCorrectionRequired)
                {
                    //天线相位缠绕效应改正，2018.08.03，czs，疑问：此处增加到了相位改正数上，是否伪距也需要增加？？   
                    phaseCorrector.Add(new PhaseWindUpCorrector(Context.SatInfoService.SatInfoFile, Context)); 
                }

            }
            return modelCorrectors;
        }
        /// <summary>
        /// 设置电离层改正
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Option"></param>
        /// <param name="modelCorrectors"></param>
        private static void SetIonoCorrectionReviser(DataSourceContext Context, GnssProcessOption Option, EpochInfoReviseManager modelCorrectors)
        {
            if (!Option.IsIonoCorretionRequired) { return; }

            RangeCorrectionReviser phaseCorrector = new RangeCorrectionReviser(Gnsser.Correction.CorrectChianType.PhaseRangeOnly);
            RangeCorrectionReviser rangeCorrector = new RangeCorrectionReviser(Gnsser.Correction.CorrectChianType.RangeOnly);
            modelCorrectors.AddProcessor(phaseCorrector);
            modelCorrectors.AddProcessor(rangeCorrector);


            bool isOk = false;
            switch (Option.IonoSourceTypeForCorrection)
            {
                case IonoSourceType.IgsGrid: 
                    if (Context.IgsGridIonoFileService != null)
                    {
                        //格网电离层改正和频率相关
                        RangeCorrectionReviser ionoRangeCorrector = new RangeCorrectionReviser(Gnsser.Correction.CorrectChianType.Self);
                        ionoRangeCorrector.Add(new IonoGridModelCorrector(Context.IgsGridIonoFileService)); //电离层模型改正  
                        modelCorrectors.AddProcessor(ionoRangeCorrector);
                        isOk = true;
                        log.Info("采用了IGS格网电离层服务来改正观测值 " + Context.IgsGridIonoFileService);
                    }
                    break;
                case IonoSourceType.CodeSphericalHarmonics:
                    if (Context.IgsCodeHarmoIonoFileService != null)
                    {
                        //格网电离层改正和频率相关
                        RangeCorrectionReviser ionoRangeCorrector = new RangeCorrectionReviser(Gnsser.Correction.CorrectChianType.Self);
                        ionoRangeCorrector.Add(new IonoGridModelCorrector(Context.IgsCodeHarmoIonoFileService)); //电离层模型改正  
                        modelCorrectors.AddProcessor(ionoRangeCorrector);
                        isOk = true;

                        log.Info("采用了IGS CODE 球谐电离层服务来改正观测值 " + Context.IgsCodeHarmoIonoFileService);
                    }
                    break;
                case IonoSourceType.GNSSerIonoFile:
                    //首先考虑GNSSSer电离层文件
                    if (Context.IonoEpochParamService != null)
                    {
                        rangeCorrector.Add(new EpochParamIonoCorrector(Context.IonoEpochParamService)); //电离层模型改正 
                        phaseCorrector.Add(new EpochParamIonoCorrector(Context.IonoEpochParamService, true)); //电离层模型改正 
                        isOk = true;
                        log.Info("采用了GNSSer历元电离层服务来改正观测值 " + Context.IonoEpochParamService);
                    }
                    break;
                case IonoSourceType.Klobchar: 
                    if (Context.IonoKlobucharParamService != null)//最后考虑导航模型
                    {
                        rangeCorrector.Add(new IonoParamModelCorrector(Context.IonoKlobucharParamService)); //电离层模型改正 
                        phaseCorrector.Add(new IonoParamModelCorrector(Context.IonoKlobucharParamService, true)); //电离层模型改正 
                        log.Info("采用了导航电离服务来改正观测值 " + Context.IonoKlobucharParamService);
                        isOk = true;
                    }
                    break;
                default:
                    break;
            } 

            if (!isOk)
            {
                log.Error("设置为需要电离层改正 " + Option.IonoSourceTypeForCorrection + "，但是没有加载成功，请打开相应开关。需要打开总开关，再打开一个数据源并指定相应的改正类型。");
            }
        }
        #endregion
        #endregion
    }
    /// <summary>
    /// 电离层改正类型
    /// </summary>
    public enum IonoSourceType
    {
        /// <summary>
        /// IGS 格网电离层模型
        /// </summary>
        IgsGrid,
        /// <summary>
        /// CODE 球谐函数模型
        /// </summary>
        CodeSphericalHarmonics,
        /// <summary>
        /// GNSSer 电离层文件
        /// </summary>
        GNSSerIonoFile,
        /// <summary>
        /// GPS 导航模型
        /// </summary>
        Klobchar

    }
}
