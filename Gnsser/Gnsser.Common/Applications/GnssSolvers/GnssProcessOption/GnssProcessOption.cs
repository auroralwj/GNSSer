//2014.10.06，czs, create in hailutu, 单点定位选项
//2015.10.15, czs, edit in 西安五路口袁记肉夹馍店, 增加延迟数量
//2016.05.01, czs, edit in hongqing, 更改为 Gnss 数据处理选项
//2016.08.02, czs, edit in fujian yongan, 增加固定参考站PPP
//2016.12.26, czs & cuiyang, edit in hongqing, 增加基线长度设置
//2017.07.19, czs, edit in hongqing, 增加平差类型
//2017.11.09, czs, edit in hongqing, 增加载波伪距方差比参数
//2017.11.10, zz, edit in zz, 增加对流层增强参数文件配置
//2018.03.20, czs, edit in hmx, 存储核心改为Config，利于文件存储和交互

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Times;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Core;
using Geo;
using Geo.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;

namespace Gnsser
{
    /// <summary>
    /// 观测相位类型
    /// </summary>
    public enum ObsPhaseType { L1, L2, L1AndL2 }
    //2016.09.24, czs, create in hongqign, 数据处理流程类型
    /// <summary>
    /// 数据处理流程类型
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// 预处理后直接计算，不输出预处理中间过程
        /// </summary>
        预处理并计算,
        /// <summary>
        /// 预处理,对输入数据进行判断、标记、格式化
        /// </summary>
        预处理,
        /// <summary>
        /// 仅计算，默认数据已经预处理了。
        /// </summary>
        仅计算,
    }

    //2017.08.10, czs, create in hongqing, 定位类型
    /// <summary>
    /// 定位类型
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// 静态定位
        /// </summary>
        静态定位,
        /// <summary>
        /// 动态定位
        /// </summary>
        动态定位
    }
    /// <summary>
    /// 平滑伪距方法
    /// </summary>
    public enum SmoothRangeType
    {
        /// <summary>
        /// 载波相位平滑伪距
        /// </summary>
        PhaseSmoothRange,
        /// <summary>
        /// 多项式拟合伪距
        /// </summary>
        PolyfitRange,
    }


    /// <summary>
    /// Gnss 计算配置信息，数据处理选项。配置数据源。
    /// </summary>
    public class GnssProcessOption : Option
    {
        Log log = new Log(typeof(GnssProcessOption));


        /// <summary>
        /// 核心存储对象
        /// </summary>
        public OptionConfig Data { get; set; }

        #region 未转换类型 
        /// <summary>
        /// 对象绑定
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        ///MW最大允许的RMS
        /// </summary>
        public double MaxAllowedRmsOfMw { get; set; }
        /// <summary>
        /// 是否输出注册的周期数据
        /// </summary>
        public bool IsOutputPeriodData { get; set; }
        /// <summary>
        /// 是否启用测站时段参数注册服务
        /// </summary>
        public bool IsEnableSiteSatPeriodDataService { get; set; } 
        /// <summary>
        /// 是否启用测站时段注册服务
        /// </summary>
        public bool IsEnableSatAppearenceService { get; set; }
        /// <summary>
        /// 掐头去尾历元分钟
        /// </summary>
        public double MinuteOfBreakOffBothEnds { get; set; }
        /// <summary>
        /// 观测数据掐头去尾
        /// </summary>
        public bool IsBreakOffBothEnds { get; set; }
        /// <summary>
        /// 无法组成无电离层组合的观测量
        /// </summary>
        public bool IsRemoveIonoFreeUnavaliable { get; set; }
        /// <summary>
        /// 相位类型
        /// </summary>
        public ObsPhaseType ObsPhaseType { get; set; }
        /// <summary>
        /// 接收机标称精度
        /// </summary>
        public GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy { get; set; }

        #region 结果精度
        /// <summary>
        /// 历元连续稳定数量
        /// </summary>
        public int SequentialEpochCountOfAccuEval { get; set; }
        /// <summary>
        /// 最大允许偏差
        /// </summary>
        public double MaxDifferOfAccuEval { get; set; }
        /// <summary>
        /// 最大允许的RMS
        /// </summary>
        public double MaxAllowedRmsOfAccuEval { get; set; }
        /// <summary>
        /// 收敛后允许的最大偏差
        /// </summary>
        public double MaxAllowedDifferAfterConvergence { get; set; }
        /// <summary>
        /// 最大允许的收敛时间
        /// </summary>
        public double MaxAllowedConvergenceTime { get; set; }
        /// <summary>
        /// 文本字符长度
        /// </summary>
        public int KeyLabelCharCount { get; set; }
        #endregion
        /// <summary>
        /// 相位单位是米，还是周。由于频繁设置，因此设置为内属性。
        /// </summary>
        public bool IsPhaseInMetterOrCycle { get; set; }//{ get => GetBool(OptionName.IsPhaseInMetterOrCycle); set => Set(OptionName.IsPhaseInMetterOrCycle, value, Calculation, "短基线的最大长度，单位米"); }

        /// <summary>
        /// 当外部文件模糊度固定失败后，是否采用实时模糊度固定算法
        /// </summary>
        public bool IsRealTimeAmbiFixWhenOuterAmbiFileFailed { get; set; }
        /// <summary>
        /// 移除指定卫星
        /// </summary>
        public List<SatelliteNumber> SatsToBeRemoved { get; set; }
        /// <summary>
        /// 是否移除指定卫星，在某些场合使用
        /// </summary>
        public bool IsEnableRemoveSats { get; set; }
        /// <summary>
        /// 基准测站名称
        /// </summary>
        public string IndicatedBaseSiteName { get; set; }
        /// <summary>
        /// 迭代最小二乘阶段
        /// </summary>
        public StepOfRecursive StepOfRecursive { get; set; }
        /// <summary>
        /// 是否直接进行模糊度固定，通常用于外部文件。
        /// </summary>
        public bool IsUseFixedParamDirectly { get; set; }
        /// <summary>
        /// 不同历元是否只允许处理相同的参数，否则处理结束，如卫星改变。主要用来测试。
        /// </summary>
        public bool IsOnlySameParam { get; set; }
        /// <summary>
        /// 电离层改正采用的模型
        /// </summary>
        public IonoSourceType IonoSourceTypeForCorrection { get; set; }
        /// <summary>
        /// 基线选择
        /// </summary>
        public BaseLineSelectionType BaseLineSelectionType { get;  set; }
        /// <summary>
        /// 基准星选择算法
        /// </summary>
        public BaseSatSelectionType BaseSatSelectionType { get; set; }
        /// <summary>
        /// 平滑伪距方法
        /// </summary>
        public SmoothRangeType SmoothRangeType { get; set; }

        /// <summary>
        /// 平滑算法类型，测试用的
        /// </summary>
        public SmoothRangeSuperpositionType SmoothRangeSuperPosType { get; set; }

        /// <summary>
        /// 相位平滑伪距值是否采用电离层拟合变化
        /// </summary>
        public IonoDifferCorrectionType IonoDifferCorrectionType { get; set; }
        //{ get => GetBool(OptionName.IonoDifferCorrectionType); set => Set(OptionName.IonoDifferCorrectionType, value, Calculation, "相位平滑伪距值是否采用电离层拟合变化"); } 
       /// <summary>
       /// 快速模式
       /// </summary>
       public bool TopSpeedModel { get; set; }
        #endregion

        /// <summary>
        /// 定位选项 默认构造函数
        /// </summary>
        public GnssProcessOption()
            : base()
        {
            this.Data = new OptionConfig();

            IsOutputEpochParam = true;
            IsOutputEpochParamRms = true;
            MaxAllowedRmsOfMw = 0.5;
            IsEnableSatAppearenceService = false;
            IsRemoveIonoFreeUnavaliable = true;
            IsBreakOffBothEnds = false;
            MinuteOfBreakOffBothEnds = 5;
            this.ObsPhaseType = ObsPhaseType.L1;
            this.GnssReveiverNominalAccuracy = new GnssReveiverNominalAccuracy(5, 10, 1, 1);
            this.Name = "默认未命名";
            ProcessType = ProcessType.仅计算;
            AnalysisParamNamesString = "De,Dn,Du";
            CycleSlipDetectSwitcher = new Dictionary<CycleSlipDetectorType, bool>();
            var systemStdDevs = new Dictionary<SatelliteType, double>();
            systemStdDevs[SatelliteType.G] = 0.3;
            systemStdDevs[SatelliteType.C] = 1;
            systemStdDevs[SatelliteType.R] = 0.4;
            systemStdDevs[SatelliteType.E] = 1;
            this.SystemStdDevFactors = systemStdDevs;//必须后赋值

            var satelliteStdDevs = new Dictionary<SatelliteNumber, double>();
            satelliteStdDevs[SatelliteNumber.Parse("C01")] = 4.0;
            satelliteStdDevs[SatelliteNumber.Parse("C02")] = 4.0;
            satelliteStdDevs[SatelliteNumber.Parse("C03")] = 4.0;
            satelliteStdDevs[SatelliteNumber.Parse("C04")] = 4.0;
            satelliteStdDevs[SatelliteNumber.Parse("C05")] = 4.0;
            this.SatelliteStdDevs = satelliteStdDevs;

            this.IsResultCheckEnabled = true;
            this.IsResidualCheckEnabled = true;
            this.MaxErrorTimesOfPostResdual = 5.0;
            if (Setting.GnsserConfig != null)
            {
                this.GnsserEpochIonoFilePath = Setting.GnsserConfig.GnsserEpochIonoPath;
                this.CoordFilePath = Setting.GnsserConfig.SiteCoordFile;
                this.StationInfoPath = Setting.GnsserConfig.StationInfoPath;
                this.EphemerisFilePath = Setting.GnsserConfig.SampleSP3File;
                this.NavIonoModelPath = Setting.GnsserConfig.NavPath;
                this.ErpFilePath = Setting.GnsserConfig.SampleErpFile;
                this.IonoGridFilePath = Setting.GnsserConfig.IonoFilePath;
                this.ClockFilePath = Setting.GnsserConfig.ClkPath;
                this.GnsserFcbFilePath = Setting.GnsserConfig.GnsserFcbFilePath;
                ObsFiles = new List<string>() { Setting.GnsserConfig.SampleOFile };
            }
            this.WindowSizeOfPhaseSmoothRange = 30;
            this.IsUseGNSSerSmoothRangeMethod = true;
            this.IsWeightedPhaseSmoothRange = true;
            this.IonoFitEpochCount = 30;
            this.OutputBufferCount = 10000;
            this.OutputRinexVersion = 3.02;
            //this.DataSourceOption = DataSourceOption;
            this.MaxLoopCount = 5; //如果初始坐标极差，则需要多循环几次才行 
            this.IsPreciseOrbit = false;
            this.VertAngleCut = 7;
            this.MaxMeanStdTimes = 100;
            this.ExemptedStdDev = 0.5;
            this.SatelliteTypes = new List<SatelliteType>() { SatelliteType.G };
            this.CaculateType = CaculateType.Filter;
            this.RejectGrossError = false;
            this.MinSatCount = 2;
            this.ExtraStreamLoopCount = 0;
            this.MaxStdDev = 100.0;
            this.MaxEpochSpan = 30 * 4;//
            this.EnableLoop = false;
            this.MinContinuouObsCount = 30;//小于15分钟，则 15 * 60 / 30 = 30 
            this.BufferSize = 60; //数据缓存大小。
            this.IsReverseCycleSlipeRevise = false;
            this.OutputDirectory = Setting.TempDirectory;
            this.InitApproxXyzRms = new XYZ(100, 100, 100);
            this.IsApproxXyzRequired = true;
            this.InitApproxXyz = new XYZ(0, 0, 0);
            this.MultiEpochCount = 3;
            this.MutliEpochSameSatCount = 5;
            this.ObsDataType = SatObsDataType.IonoFreeRange;
            this.StartIndex = 0;
            this.CaculateCount = 10000000;
            this.IsEphemerisRequired = true;//默认需要
            this.IsDisableEclipsedSat = true;//默认需要
            this.Isgpt2File1DegreeRequired = true;
            MinAllowedApproxXyzLen = 1000000;

            #region 周跳探测参数
            this.IsEnableBufferCs = false;
            this.IsEnableRealTimeCs = true;
            this.IsCycleSlipDetectionRequired = true;//默认需要
            this.IsDetectClockJump = false;
            this.IsUsingRecordedCycleSlipInfo = true;
            this.MaxDifferValueOfMwCs = 8.6;
            this.MaxRmsTimesOfLsPolyCs = 5; //探测范围
            this.MaxBreakingEpochCount = 4;
            this.MaxValueDifferOfHigherDifferCs = 14;
            #region 基于缓存的周跳
            MaxErrorTimesOfBufferCs = 3;
            DifferTimesOfBufferCs = 1;
            PolyFitOrderOfBufferCs = 2;
            MinWindowSizeOfCs = 5;
            #endregion

            #endregion

            this.IsAliningPhaseWithRange = false;//默认不需要
            this.IsRemoveSmallPartSat = true;
            this.IsRemoveOrDisableNotPassedSat = true;
            this.IsSameSatRequired = true;
            this.IsExcludeMalfunctioningSat = true;
            this.BaseSiteSelectType = BaseSiteSelectType.GeoCenter;
            this.MinAllowedRange = 15000000.0;
            this.MaxAllowedRange = 40000000.0; //35770602.136176817，同步卫星
            IsRangeValueRequired = true;
            IsOutputEpochResult = true;
            IsPhaseValueRequired = true;
            IsOutputResult = true;
            this.IsOutputAdjustMatrix = false;
            this.IsAdjustEnabled = true;
            #region  数据源


            this.IsObsDataRequired = true;
            MinFrequenceCount = 1;
            IsPreciseClockFileRequired = true;
            IsEphemerisRequired = true;
            IsPreciseEphemerisFileRequired = true;
            IsAntennaFileRequired = true;
            IsSatStateFileRequired = true;
            IsSatInfoFileRequired = true;
            IsOceanLoadingFileRequired = true;
            IsDCBFileRequired = true;
            IsVMF1FileRequired = true;
            Isgpt2FileRequired = true;
            IsErpFileRequired = true;
            IsIonoCorretionRequired = false;
            IsEnableNgaEphemerisSource = true;
            #endregion
            this.EphemerisInterpolationOrder = 10;
            #region 改正数
            this.IsGnsserEpochIonoFileRequired = false;
            this.IsObsCorrectionRequired = true;
            this.IsApproxModelCorrectionRequired = true;
            this.IsDcbCorrectionRequired = true;
            this.IsReceiverAntSiteBiasCorrectionRequired = true;
            this.IsOceanTideCorrectionRequired = true;
            this.IsSolidTideCorrectionRequired = true;
            this.IsPoleTideCorrectionRequired = true;
            this.IsSatClockBiasCorrectionRequired = true;
            this.IsTropCorrectionRequired = true;
            this.IsGravitationalDelayCorrectionRequired = true;
            this.IsSatAntPcoCorrectionRequired = true;
            this.IsSatAntPvcCorrectionRequired = true;
            this.IsRecAntPcoCorrectionRequired = true;
            this.IsRecAntPcvCorrectionRequired = true;
            this.IsPhaseWindUpCorrectionRequired = true;
            this.IsSiteCorrectionsRequired = true;
            this.IsRangeCorrectionsRequired = true;
            this.IsFrequencyCorrectionsRequired = true;
            #endregion

            MinSuccesiveEphemerisCount = 11;
            IsSwitchWhenEphemerisNull = false;
            IsSetApproxXyzWithCoordService = false;

            MinDistanceOfLongBaseLine = 50 * 1000;
            MaxDistanceOfShortBaseLine = 20000;
            this.AdjustmentType = AdjustmentType.卡尔曼滤波;
            PositionType = Gnsser.PositionType.静态定位;
            this.IsReversedDataSource = false;
            this.OrdinalAndReverseCount = 0;
            #region 随机模型参数默认值
            this.StdDevOfRandomWalkModel = 1.75e-2;
            this.StdDevOfPhaseModel = 1e-5;
            this.StdDevOfCycledPhaseModel = 2e7;
            this.StdDevOfIonoRandomWalkModel = 0.013;
            this.StdDevOfStaticTransferModel = 1e-10;
            this.StdDevOfTropoRandomWalkModel = 4e-4;
            this.StdDevOfSysTimeRandomWalkModel = 4e-4;
            this.StdDevOfRevClockWhiteNoiseModel = 3e5;
            this.StdDevOfSatClockWhiteNoiseModel = 3e4;
            #endregion

            IsOpenReportWhenCompleted = true;
            PhaseCovaProportionToRange = 1e-4;
            IsSmoothMoveInMultiEpoches = false;

            OrderOfDeltaIonoPolyFit = 1;

            SmoothRangeSuperPosType = SmoothRangeSuperpositionType.快速更新算法;
            this.OutputMinInterval = 0.0001;
            this.StdDevOfWhiteNoiseOfDynamicPosition = 10;

            this.IsIndicatedPrn = false;
            this.IndicatedPrn = new SatelliteNumber(1, SatelliteType.G);
            //模糊度
            this.MaxAmbiDifferOfIntAndFloat = 0.5;
            this.MaxRoundAmbiDifferOfIntAndFloat = 0.3;
            this.MinFixedAmbiRatio = 0.5;
            this.MaxRatioOfLambda = 3.0;
            this.MaxFloatRmsNormToFixAmbiguity = 0.1;
            this.SmoothRangeType = SmoothRangeType.PhaseSmoothRange;
            StepOfRecursive = StepOfRecursive.SequentialConst;

            IsConnectIgsDailyProduct = false;
            #region  精度评估

            this.SequentialEpochCountOfAccuEval = 20;
            this.MaxDifferOfAccuEval = 0.1;
            this.MaxAllowedConvergenceTime = 240;
            this.KeyLabelCharCount = 4;
            this.MaxAllowedDifferAfterConvergence = 0.25;
            this.MaxAllowedRmsOfAccuEval = 0.1;
            #endregion
        }

        /// <summary>
        /// 构造。
        /// </summary>
        /// <param name="config"></param>
        public GnssProcessOption(OptionConfig config)
        {
            this.Data = config;
            this.Name = config.Name;
        }

        #region 分组
        const string Basic = "Basic";
        const string Path = "Path";
        const string Adjustment = "Adjustment";
        const string DataSource = "DataSource";
        const string Output = "Output";
        const string Correction = "Correction";
        const string Calculation = "Calculation";
        const string PreProcess = "PreProcess";
        #endregion

        #region 内置转换
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="group"></param>
        /// <param name="comment"></param>
        public void Set(OptionName name, Object obj, string group = "Default", string comment = "")
        {
            var item = this.Data.GetOrCreate(name);
            item.Value = obj;
            item.Group = group;
            item.Comment = comment;
        }
        /// <summary>
        /// 获取枚举类型，仅枚举类型适用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        private T GetEnumType<T>(OptionName name)
        {
            var obj = Data[name];
            if (obj.Value == null)
            {
                obj.Value = default(T);
            }
            return (T)obj.Value;
        }
        /// <summary>
        /// 获取XYZ对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private XYZ GetXYZ(OptionName name)
        {
            var obj = Data[name];
            if (obj.Value == null)
            {
                obj.Value = new XYZ();
            }
            return (XYZ)obj.Value;
        }
        /// <summary>
        /// 获取List<string>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
         public List<string> GetListString(OptionName name)
        {
            return (List<string>)Data[name].Value;
        }
        /// <summary>
        /// 获取双精度浮点数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetDouble(OptionName name)
        {
            return (double)Data[name].Value;
        }
        /// <summary>
        /// 获取整形
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInt(OptionName name)
        {
            return (Int32)Data[name].Value;
        }
        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetBool(OptionName name, bool defaultVal = false)
        {
            var obj = Data[name];
            if (obj == null || obj.Value == null || String.IsNullOrWhiteSpace(obj.Name + "") || String.IsNullOrWhiteSpace(obj.Value + "")) { return defaultVal; }
            return (bool)obj.Value;
        }
        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DateTime GetDateTime(OptionName name)
        {
            return (DateTime)Data[name].Value;
        }

        private SatelliteNumber GetSatelliteNumber(OptionName name)
        {
            return (SatelliteNumber)Data[name].Value;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(OptionName name)
        {
            return (string)Data[name].Value;
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetStringLines(OptionName name)
        {
            var str = (string)Data[name].Value;
            return new List<string>(str.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public GnssProcessOption Set(OptionName name, List<string> lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in lines)
            {
                sb.AppendLine(  item);
            }
            Data[name].Value = sb.ToString();
            return this;
        }
        #endregion

        #region 基本属性
        /// <summary>
        /// 是否拼接IGS产品
        /// </summary>
        public bool IsConnectIgsDailyProduct { get; set; }

        //  public TimePeriod TimePeriod { }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get {
                var name = GetString(OptionName.Name);
                if(name == "" || name == "未命名"|| name == "配置文件")
                {
                    name = GnssSolverType.ToString();
                }
                return name;
            } set { Set(OptionName.Name, value, Basic, "Name of this Option"); } }
        /// <summary>
        /// 正反算， 顺序-逆序计算. 0表示只按照默认配置，单向计算一次。
        /// </summary>
        public int OrdinalAndReverseCount { get { return GetInt(OptionName.OrdinalAndReverseCount); } set { Set(OptionName.OrdinalAndReverseCount, value, Calculation, "正反算， 顺序-逆序计算. 0表示只按照默认配置，单向计算一次。"); } }
        /// <summary>
        /// 是否逆序数据流
        /// </summary>
        public bool IsReversedDataSource { get { return GetBool(OptionName.IsReversedDataSource); } set { Set(OptionName.IsReversedDataSource, value, Calculation, "是否逆序数据流正反算"); } }
        /// <summary>
        /// 在正反算时是否清空输出缓存
        /// </summary>
        public bool IsClearOutBufferWhenReversing { get { return GetBool(OptionName.IsClearOutBufferWhenReversing); } set { Set(OptionName.IsClearOutBufferWhenReversing, value, Calculation, "在正反算时是否清空输出缓存"); } }
        /// <summary>
        /// 定位类型
        /// </summary>
        public PositionType PositionType { get => GetEnumType<PositionType>(OptionName.PositionType); set { Set(OptionName.PositionType, value, Calculation, "静态定位还是动态定位"); } }
        /// <summary>
        /// 数据处理类型，是否为预处理等
        /// </summary>
        public ProcessType ProcessType { get { return GetEnumType<ProcessType>(OptionName.ProcessType); } set { Set(OptionName.ProcessType, value, Calculation, "数据处理类型，是否为预处理等"); } }
        /// <summary>
        /// 是否启用平差
        /// </summary>
        public bool IsAdjustEnabled { get; set; }
        /// <summary>
        /// 平差类型选项
        /// </summary>
        public AdjustmentType AdjustmentType { get => GetEnumType<AdjustmentType>(OptionName.AdjustmentType); set { Set(OptionName.AdjustmentType, value, Adjustment, "平差类型选项"); } }

        /// <summary>
        /// 观测文件格式化处理类型
        /// </summary>
        public RinexObsFileFormatType RinexObsFileFormatType { get => GetEnumType<RinexObsFileFormatType>(OptionName.RinexObsFileFormatType); set { Set(OptionName.RinexObsFileFormatType, value, Calculation, "观测文件格式化处理类型"); } }
        /// <summary>
        /// RINEX 输出版本 2.11 或 3.02
        /// </summary>
        public double OutputRinexVersion { get { return GetDouble(OptionName.OutputRinexVersion); } set { Set(OptionName.OutputRinexVersion, value, Output, "RINEX 输出版本 2.11 或 3.02"); } }

        #region 循环控制
        /// <summary>
        /// GNSS 解算器类型
        /// </summary>
        public GnssSolverType GnssSolverType { get => GetEnumType<GnssSolverType>(OptionName.GnssSolverType); set { Set(OptionName.GnssSolverType, value, Calculation, "GNSS 解算器类型"); } }
        /// <summary>
        /// 计算起始历元编号。
        /// </summary>
        public int StartIndex { get => GetInt(OptionName.StartIndex); set => Set(OptionName.StartIndex, value, Calculation, "计算起始历元编号。"); } 
        /// <summary>
        /// 计算数量，从起始编号开始计算。
        /// </summary>
        public int CaculateCount { get => GetInt(OptionName.CaculateCount); set => Set(OptionName.CaculateCount, value, Calculation, "计算数量，从起始编号开始计算。"); }
        
        /// <summary>
        /// 数据流一次计算（正算或反算）的额外的循环次数
        /// </summary>
        public int ExtraStreamLoopCount { get => GetInt(OptionName.ExtraStreamLoopCount); set => Set(OptionName.ExtraStreamLoopCount, value, Calculation, "数据流一次计算（正算或反算）的循环次数"); }

        #endregion

        #region 测站初始值
        /// <summary>
        /// 是否需要测站初始值,如果需要，而测站值为空，则将自动进行计算设置。
        /// </summary>
        public bool IsApproxXyzRequired { get => GetBool(OptionName.IsApproxXyzRequired); set => Set(OptionName.IsApproxXyzRequired, value, Calculation, "是否需要测站初始值,如果需要，而测站值为空，则将自动进行计算设置"); } 
        /// <summary>
        /// 坐标初始中误差.用于初始赋权.默认为100米。
        /// </summary>
        public XYZ InitApproxXyzRms { get => GetXYZ(OptionName.InitApproxXyzRms); set { Set(OptionName.InitApproxXyzRms, value, Calculation, "坐标初始中误差.用于初始赋权.默认为100米。"); } }
        /// <summary>
        /// 坐标初始.用于初始赋权
        /// </summary>
        public XYZ InitApproxXyz { get => GetXYZ(OptionName.InitApproxXyz); set { Set(OptionName.InitApproxXyz, value, Calculation, "坐标初始.用于初始赋权"); } }
        /// <summary>
        /// 是否指定初始坐标
        /// </summary>
        public bool IsIndicatingApproxXyz { get => GetBool(OptionName.IsIndicatingApproxXyz); set => Set(OptionName.IsIndicatingApproxXyz, value, Calculation, "是否指定初始坐标"); } 
        /// <summary>
        /// 是否指定近似坐标
        /// </summary>
        public bool IsIndicatingApproxXyzRms { get => GetBool(OptionName.IsIndicatingApproxXyzRms); set => Set(OptionName.IsIndicatingApproxXyzRms, value, Calculation, "是否指定近似坐标"); } 
        /// <summary>
        /// 是否更新测站信息
        /// </summary>
        public bool IsUpdateStationInfo { get => GetBool(OptionName.IsUpdateStationInfo); set => Set(OptionName.IsUpdateStationInfo, value, Calculation, "是否更新测站信息"); }
        #endregion

        #region 数据源
        /// <summary>
        /// 是否制定了星历，哪怕一个
        /// </summary>
        public bool IsIndicatedEphemerisPathes
        {
            get
            {
                return IsIndicatingEphemerisFile
                    || IsIndicatingBdsEphemerisFile
                    || IsIndicatingGloEphemerisFile 
                    || IsIndicatingGalEphemerisFile;
            }
        }
        /// <summary>
        /// 获取已经指定路径的系统星历，若没有指定的，则为0个
        /// </summary>
        /// <returns></returns>
        public Dictionary<SatelliteType, string> GetIndicatedEphemerisPathes()
        {
            Dictionary<SatelliteType, string> dic = new Dictionary<SatelliteType, string>();
            if (IsIndicatingEphemerisFile)//GPS
            {
                dic[SatelliteType.G] = EphemerisFilePath;
            }
            if (IsIndicatingBdsEphemerisFile)//GPS
            {
                dic[SatelliteType.C] = BdsEphemerisFilePath;
            }
            if (IsIndicatingGloEphemerisFile)//GPS
            {
                dic[SatelliteType.R] = GloEphemerisFilePath;
            }
            if (IsIndicatingGalEphemerisFile)//GPS
            {
                dic[SatelliteType.E] = GalEphemerisFilePath;
            }
            return dic;
        }

        /// <summary>
        /// 单个观测文件，第一个
        /// </summary>
        public string ObsFilePath
        {
            get
            {
                if (ObsFiles.Count > 0) return ObsFiles[0];
                return "";
            }
            set
            {
                ObsFiles = new List<string>() { value };
            }
        }


        /// <summary>
        /// 观测文件路径，逗号“,”分隔
        /// </summary>
        public List<string> ObsFiles { get => GetListString(OptionName.ObsFiles); set => Set(OptionName.ObsFiles, value, DataSource, "观测文件路径，逗号“,”分隔"); }
        
        /// <summary>
        /// 载波相位是否是以米为单位，如Android接收机单位为米。
        /// </summary>
        public bool IsLengthPhaseValue { get; set; }// { get => GetBool(OptionName.IsLengthPhaseValue); set => Set(OptionName.IsLengthPhaseValue, value, DataSource, "载波相位是否是以米为单位，如Android接收机单位为米。"); } 
        /// <summary>
        /// 是否需要观测数据源
        /// </summary>
        public bool IsObsDataRequired { get => GetBool(OptionName.IsObsDataRequired); set => Set(OptionName.IsObsDataRequired, value, DataSource, "是否需要观测数据源"); }
        /// <summary>
        /// 是否指定钟差
        /// </summary>
        public bool IsIndicatingClockFile { get => GetBool(OptionName.IsIndicatingClockFile); set => Set(OptionName.IsIndicatingClockFile, value, DataSource, "是否指定钟差"); }
        /// <summary>
        /// 是否指定ERP
        /// </summary>
        public bool IsIndicatingErpFile { get => GetBool(OptionName.IsIndicatingErpFile); set => Set(OptionName.IsIndicatingErpFile, value, DataSource, "是否指定ERP"); }

        /// <summary>
        /// 是否需要 P2C2 
        /// </summary>
        public bool IsP2C2Enabled { get => GetBool(OptionName.IsP2C2Enabled); set => Set(OptionName.IsP2C2Enabled, value, DataSource, "是否需要 P2C2 "); } 

        /// <summary>
        /// 格网电离层文件路径
        /// </summary>
        public string IonoGridFilePath { get => GetString(OptionName.IonoGridFilePath); set => Set(OptionName.IonoGridFilePath, value, DataSource, "格网电离层文件路径"); } 
        /// <summary>
        /// 是否指定格网电离层文件
        /// </summary>
        public bool IsIndicatingGridIonoFile { get => GetBool(OptionName.IsIndicatingGridIonoFile); set => Set(OptionName.IsIndicatingGridIonoFile, value, DataSource, "是否指定格网电离层文件"); } 
        /// <summary>
        /// 历元电离层参数文件路径
        /// </summary>
        public string GnsserEpochIonoFilePath { get => GetString(OptionName.EpochIonoParamFilePath); set => Set(OptionName.EpochIonoParamFilePath, value, DataSource, "历元电离层参数文件路径"); }
        /// <summary>
        /// 是否全部使用一个星历数据源，将采用 EphemerisFilePath
        /// </summary>
        public bool IsUseUniqueEphemerisFile { get => GetBool(OptionName.IsUseUniqueEphemerisFile); set => Set(OptionName.IsUseUniqueEphemerisFile, value, DataSource, "是否全部使用一个星历数据源，将采用 EphemerisFilePath，具有最高优先权"); }
        /// <summary>
        /// 是否指定星历数据源默认GPS，具有最高优先权
        /// </summary>
        public bool IsIndicatingEphemerisFile { get => GetBool(OptionName.IsIndicatingEphemerisFile); set => Set(OptionName.IsIndicatingEphemerisFile, value, DataSource, "是否指定星历数据源默认GPS，具有最高优先权"); }
        /// <summary>
        /// 是否指定BDS星历数据源，具有最高优先权
        /// </summary>
        public bool IsIndicatingBdsEphemerisFile { get => GetBool(OptionName.IsIndicatingBdsEphemerisFile); set => Set(OptionName.IsIndicatingBdsEphemerisFile, value, DataSource, "是否指定BDS星历数据源，具有最高优先权"); }
        /// <summary>
        /// 是否指定GLONASS星历数据源，具有最高优先权
        /// </summary>
        public bool IsIndicatingGloEphemerisFile { get => GetBool(OptionName.IsIndicatingGloEphemerisFile); set => Set(OptionName.IsIndicatingGloEphemerisFile, value, DataSource, "是否指定GLONASS星历数据源，具有最高优先权"); }
        /// <summary>
        /// 是否指定伽利略星历数据源，具有最高优先权
        /// </summary>
        public bool IsIndicatingGalEphemerisFile { get => GetBool(OptionName.IsIndicatingGalEphemerisFile); set => Set(OptionName.IsIndicatingGalEphemerisFile, value, DataSource, "是否指定伽利略星历数据源，具有最高优先权"); }
        /// <summary>
        /// 是否指定坐标文件
        /// </summary>
        public bool IsIndicatingCoordFile { get => GetBool(OptionName.IsIndicatingCoordFile); set => Set(OptionName.IsIndicatingCoordFile, value, DataSource, "是否指定坐标文件"); }
        /// <summary>
        /// 指定的钟差路径
        /// </summary>
        public string ClockFilePath { get => GetString(OptionName.ClockFilePath); set => Set(OptionName.ClockFilePath, value, DataSource, "指定的钟差路径，具有最高优先权"); }
        /// <summary>
        /// 指定的ERP路径
        /// </summary>
        public string ErpFilePath { get => GetString(OptionName.ErpFilePath); set => Set(OptionName.ErpFilePath, value, DataSource, "指定的ERP路径，具有最高优先权"); }
        /// <summary>
        /// GNSSer FCB 文件路径
        /// </summary>
         public string GnsserFcbFilePath { get => GetString(OptionName.GnsserFcbFilePath); set => Set(OptionName.GnsserFcbFilePath, value, DataSource, "GNSSer FCB 文件路径"); }
        /// <summary>
        /// 是否启用  GNSSer FCB 文件路径
        /// </summary>
        public bool IsGnsserFcbOfDcbRequired { get => GetBool(OptionName.IsGnsserFcbOfDcbRequired); set => Set(OptionName.IsGnsserFcbOfDcbRequired, value, DataSource, "是否启用  GNSSer FCB 文件路径"); }
      
        /// <summary>
        /// 指定的星历路径，默认GPS
        /// </summary>
        public string EphemerisFilePath { get => GetString(OptionName.EphemerisFilePath); set => Set(OptionName.EphemerisFilePath, value, DataSource, "指定的星历路径"); }
        /// <summary>
        /// 指定的北斗星历路径
        /// </summary>
        public string BdsEphemerisFilePath { get => GetString(OptionName.BdsEphemerisFilePath); set => Set(OptionName.BdsEphemerisFilePath, value, DataSource, "指定的北斗星历路径"); }
        /// <summary>
        /// 指定的GLONASS星历路径
        /// </summary>
        public string GloEphemerisFilePath { get => GetString(OptionName.GloEphemerisFilePath); set => Set(OptionName.GloEphemerisFilePath, value, DataSource, "指定的GLONASS星历路径"); }
        /// <summary>
        /// 指定的Galileo星历路径
        /// </summary>
        public string GalEphemerisFilePath { get => GetString(OptionName.GalEphemerisFilePath); set => Set(OptionName.GalEphemerisFilePath, value, DataSource, "指定的Galileo星历路径"); }
        /// <summary>
        /// 导航文件路径，用于提取电离层改正等
        /// </summary>
        public string NavIonoModelPath { get => GetString(OptionName.NavIonoModelPath); set => Set(OptionName.NavIonoModelPath, value, DataSource, "导航文件路径，用于提取电离层改正等"); } 
        /// <summary>
        /// 坐标文件路径
        /// </summary>
        public string CoordFilePath { get => GetString(OptionName.CoordFilePath); set => Set(OptionName.CoordFilePath, value, DataSource, "坐标文件路径"); } 
        /// <summary>
        /// 对流层增强文件。
        /// </summary>
        public string TropAugmentFilePath { get => GetString(OptionName.TropAugmentFilePath); set => Set(OptionName.TropAugmentFilePath, value, DataSource, "对流层增强文件"); } 

        /// <summary>
        /// 测站信息文件路径
        /// </summary>
        public string StationInfoPath { get => GetString(OptionName.StationInfoPath); set => Set(OptionName.StationInfoPath, value, DataSource, "测站信息文件路径"); }
        /// <summary>
        /// 是否需要精密钟差文件
        /// </summary>
        public bool IsPreciseClockFileRequired { get => GetBool(OptionName.IsPreciseClockFileRequired); set => Set(OptionName.IsPreciseClockFileRequired, value, DataSource, "是否需要精密钟差文件"); }
        /// <summary>
        /// 是否启用全钟差服务,否则使用简单钟差服务
        /// </summary>
        public bool IsUsingFullClockService { get => GetBool(OptionName.IsUsingFullClockService, false); set => Set(OptionName.IsUsingFullClockService, value, DataSource, "是否启用全钟差服务,否则使用简单钟差服务"); }
        /// <summary>
        /// 是否需要星历， 默认需要
        /// </summary>
        public bool IsEphemerisRequired { get => GetBool(OptionName.IsEphemerisRequired); set => Set(OptionName.IsEphemerisRequired, value, DataSource, "是否需要星历， 默认需要"); } 
        /// <summary>
        /// 是否需要精密星历文件
        /// </summary>
        public bool IsPreciseEphemerisFileRequired { get => GetBool(OptionName.IsPreciseEphemerisFileRequired); set => Set(OptionName.IsPreciseEphemerisFileRequired, value, DataSource, "是否需要精密星历文件"); } 
        /// <summary>
        /// 是否需要天线文件
        /// </summary>
        public bool IsAntennaFileRequired { get => GetBool(OptionName.IsAntennaFileRequired); set => Set(OptionName.IsAntennaFileRequired, value, DataSource, "是否需要天线文件"); } 
        /// <summary>
        /// 是否需要卫星状态文件
        /// </summary>
        public bool IsSatStateFileRequired { get => GetBool(OptionName.IsSatStateFileRequired); set => Set(OptionName.IsSatStateFileRequired, value, DataSource, "是否需要卫星状态文件"); } 
        /// <summary>
        /// 是否需要卫星信息文件
        /// </summary>
        public bool IsSatInfoFileRequired { get => GetBool(OptionName.IsSatInfoFileRequired); set => Set(OptionName.IsSatInfoFileRequired, value, DataSource, "是否需要卫星信息文件"); } 
        /// <summary>
        /// 是否需要潮汐文件
        /// </summary>
        public bool IsOceanLoadingFileRequired { get => GetBool(OptionName.IsOceanLoadingFileRequired); set => Set(OptionName.IsOceanLoadingFileRequired, value, DataSource, "是否需要潮汐文件"); } 
        /// <summary>
        /// 是否需要DCB文件
        /// </summary>
        public bool IsDCBFileRequired { get => GetBool(OptionName.IsDCBFileRequired); set => Set(OptionName.IsDCBFileRequired, value, DataSource, "是否需要DCB文件"); } 
        /// <summary>
        /// 是否需要VMF1文件
        /// </summary>
        public bool IsVMF1FileRequired { get => GetBool(OptionName.IsVMF1FileRequired); set => Set(OptionName.IsVMF1FileRequired, value, DataSource, "是否需要VMF1文件"); } 
        /// <summary>
        /// 是否采用GPT2通用文件改正
        /// </summary>
        public bool Isgpt2FileRequired { get => GetBool(OptionName.Isgpt2FileRequired); set => Set(OptionName.Isgpt2FileRequired, value, DataSource, "是否采用GPT2通用文件改正"); } 
        /// <summary>
        /// 是否采用ERP文件改正
        /// </summary>
        public bool IsErpFileRequired { get => GetBool(OptionName.IsErpFileRequired); set => Set(OptionName.IsErpFileRequired, value, DataSource, "是否采用ERP文件改正"); } 
        #endregion

        #region 数据源选择


        /// <summary>
        /// 是否需要IGS电离层文件，格网或球谐
        /// </summary>
        public bool IsIgsIonoFileRequired { get => GetBool(OptionName.IsIgsIonoFileRequired); set => Set(OptionName.IsIgsIonoFileRequired, value, DataSource, "是否需要IGS电离层格网文件"); }

        #endregion
        /// <summary>
        /// 是否估计接收机DCB参数
        /// </summary>
        public bool IsEstDcbOfRceiver { get => GetBool(OptionName.IsEstDcbOfRceiver); set => Set(OptionName.IsEstDcbOfRceiver, value, Output, "是否估计接收机DCB参数"); }
        /// <summary>
        /// 是否估计测站对流层湿延迟参数
        /// </summary>
        public bool IsEstimateTropWetZpd { get => GetBool(OptionName.IsEstimateTropWetZpd); set => Set(OptionName.IsEstimateTropWetZpd, value, Output, "是否估计测站对流层湿延迟参数"); }

        /// <summary>
        /// 是否在计算结束时打开平差报告
        /// </summary>
        public bool IsOpenReportWhenCompleted { get => GetBool(OptionName.IsOpenReportWhenCompleted); set => Set(OptionName.IsOpenReportWhenCompleted, value, Output, "是否在计算结束时打开平差报告"); } 

        /// <summary>
        /// 是否启用电离层模型改正。顶层接口，如果要采用电离层改正观测近似值，则必须设定。
        /// </summary>
        public bool IsIonoCorretionRequired { get => GetBool(OptionName.IsIonoCorretionRequired); set => Set(OptionName.IsIonoCorretionRequired, value, Correction, "是否启用电离层模型改正。顶层接口，如果要采用电离层改正观测近似值，则必须设定。"); } 
        /// <summary>
        /// 是否需要伪距值
        /// </summary>
        public bool IsRangeValueRequired { get => GetBool(OptionName.IsRangeValueRequired); set => Set(OptionName.IsRangeValueRequired, value, Calculation, "是否需要伪距值"); } 
        /// <summary>
        /// 是否需要相位值
        /// </summary>
        public bool IsPhaseValueRequired { get => GetBool(OptionName.IsPhaseValueRequired); set => Set(OptionName.IsPhaseValueRequired, value, Calculation, "是否需要相位值"); } 
    /// <summary>
        /// 平滑伪距总开关
        /// </summary>
        public bool IsSmoothRange { get => GetBool(OptionName.IsSmoothRange); set => Set(OptionName.IsSmoothRange, value, Calculation, "平滑伪距总开关"); } 
      
                 /// <summary>
        /// 相位平滑伪距采用电离层拟合的阶数
        /// </summary>
        public int OrderOfDeltaIonoPolyFit { get => GetInt(OptionName.OrderOfDeltaIonoPolyFit); set => Set(OptionName.OrderOfDeltaIonoPolyFit, value, Calculation, "相位平滑伪距采用电离层拟合的阶数"); }
        /// <summary>
        /// 电离层拟合历元数量
        /// </summary>
        public int IonoFitEpochCount { get => GetInt(OptionName.IonoFitEpochCount); set => Set(OptionName.IonoFitEpochCount, value, Calculation, "电离层拟合历元数量"); }


        /// <summary>
        /// 定位过程中，是否更新测站估值坐标。
        /// </summary>
        public bool IsUpdateEstimatePostition { get => GetBool(OptionName.IsUpdateEstimatePostition); set => Set(OptionName.IsUpdateEstimatePostition, value, Calculation, "定位过程中，是否更新测站估值坐标。"); } 
        /// <summary>
        /// 是否移除未通过检核的卫星，否则标记为未启用。
        /// </summary>
        public bool IsRemoveOrDisableNotPassedSat { get => GetBool(OptionName.IsRemoveOrDisableNotPassedSat); set => Set(OptionName.IsRemoveOrDisableNotPassedSat, value, Calculation, "是否移除未通过检核的卫星，否则标记为未启用。"); } 

        /// <summary>
        /// 是否移除观测段太小的卫星
        /// </summary>
        public bool IsRemoveSmallPartSat { get => GetBool(OptionName.IsRemoveSmallPartSat); set => Set(OptionName.IsRemoveSmallPartSat, value, Calculation, "是否移除观测段太小的卫星"); } 

        /// <summary>
        /// 是否移除故障卫星(通常从外部文件指定)
        /// </summary>
        public bool IsExcludeMalfunctioningSat { get => GetBool(OptionName.IsExcludeMalfunctioningSat); set => Set(OptionName.IsExcludeMalfunctioningSat, value, Calculation, "是否移除故障卫星(通常从外部文件指定)"); } 
        /// <summary>
        /// 是否禁用太阳阴影影响的卫星
        /// </summary>
        public bool IsDisableEclipsedSat { get => GetBool(OptionName.IsDisableEclipsedSat); set => Set(OptionName.IsDisableEclipsedSat, value, Calculation, " 是否禁用太阳阴影影响的卫星"); } 
        #region 周跳处理
        /// <summary>
        /// 启用缓存周跳探测
        /// </summary>
        public bool IsEnableBufferCs { get => GetBool(OptionName.IsEnableBufferCs); set => Set(OptionName.IsEnableBufferCs, value, PreProcess, "启用缓存周跳探测"); } 
        /// <summary>
        /// 是否启用实时周跳探测
        /// </summary>
        public bool IsEnableRealTimeCs { get => GetBool(OptionName.IsEnableRealTimeCs); set => Set(OptionName.IsEnableRealTimeCs, value, DataSource, "是否启用实时周跳探测"); } 
   
        #region 基于缓存的周跳
        /// <summary>
        /// 缓存周跳差分次数
        /// </summary>
        public double MaxErrorTimesOfBufferCs { get => GetDouble(OptionName.MaxErrorTimesOfBufferCs); set => Set(OptionName.MaxErrorTimesOfBufferCs, value, PreProcess, "缓存周跳差分次数"); } 
        /// <summary>
        /// 缓存周跳差分次数
        /// </summary>
        public int DifferTimesOfBufferCs { get => GetInt(OptionName.DifferTimesOfBufferCs); set => Set(OptionName.DifferTimesOfBufferCs, value, PreProcess, "缓存周跳差分次数"); } 
        /// <summary>
        /// 缓存周跳拟合阶次
        /// </summary>
        public int PolyFitOrderOfBufferCs { get => GetInt(OptionName.PolyFitOrderOfBufferCs); set => Set(OptionName.PolyFitOrderOfBufferCs, value, PreProcess, "缓存周跳拟合阶次"); } 

        /// <summary>
        /// 是否忽略已经标记为周跳的历元卫星。
        /// </summary>
        public bool IgnoreCsedOfBufferCs { get => GetBool(OptionName.IgnoreCsedOfBufferCs); set => Set(OptionName.IgnoreCsedOfBufferCs, value, PreProcess, "是否忽略已经标记为周跳的历元卫星"); } 
        #endregion
        /// <summary>
        /// 缓存周跳 最小窗口大小，小于此，都认为有周跳。
        /// </summary>
        public int MinWindowSizeOfCs { get => GetInt(OptionName.MinWindowSizeOfCs); set => Set(OptionName.MinWindowSizeOfCs, value, PreProcess, "缓存周跳 最小窗口大小，小于此，都认为有周跳。"); } 

        /// <summary>
        /// 是否采用数据源信息标记的周跳，若已标记周跳，则认为有。
        /// </summary>
        public bool IsUsingRecordedCycleSlipInfo { get => GetBool(OptionName.IsUsingRecordedCycleSlipInfo); set => Set(OptionName.IsUsingRecordedCycleSlipInfo, value, PreProcess, "是否采用数据源信息标记的周跳，若已标记周跳，则认为有。"); } 

        /// <summary>
        /// MW周跳探测中，最大的误差
        /// </summary>
        public double MaxDifferValueOfMwCs { get => GetDouble(OptionName.MaxDifferValueOfMwCs); set => Set(OptionName.MaxDifferValueOfMwCs, value, PreProcess, "MW周跳探测中，最大的误差"); } 
        /// <summary>
        /// 多项式拟合周跳探测中，最大的误差倍数
        /// </summary>
        public double MaxRmsTimesOfLsPolyCs { get => GetDouble(OptionName.MaxRmsTimesOfLsPolyCs); set => Set(OptionName.MaxRmsTimesOfLsPolyCs, value, PreProcess, "多项式拟合周跳探测中，最大的误差倍数"); } 
        /// <summary>
        /// 高次差周跳探测中，允许的最大的误差
        /// </summary>
        public double MaxValueDifferOfHigherDifferCs { get => GetDouble(OptionName.MaxValueDifferOfHigherDifferCs); set => Set(OptionName.MaxValueDifferOfHigherDifferCs, value, PreProcess, "高次差周跳探测中，允许的最大的误差"); } 
        /// <summary>
        ///主要周跳探测。 历元分段最大的断裂间隔，小于此则认为连续，大于则认为断裂，如 周跳探测允许最大断裂的时间间隔
        /// </summary>
        public int MaxBreakingEpochCount { get => GetInt(OptionName.MaxBreakingEpochCount); set => Set(OptionName.MaxBreakingEpochCount, value, PreProcess, "周跳探测允许最大断裂的时间间隔"); } 

        /// <summary>
        /// 是否进行周跳探测
        /// </summary>
        public bool IsCycleSlipDetectionRequired { get => GetBool(OptionName.IsCycleSlipDetectionRequired); set => Set(OptionName.IsCycleSlipDetectionRequired, value, PreProcess, "是否进行周跳探测"); } 
        /// <summary>
        /// 是否修复周跳
        /// </summary>
        public bool IsCycleSlipReparationRequired { get => GetBool(OptionName.IsCycleSlipReparationRequired); set => Set(OptionName.IsCycleSlipReparationRequired, value, PreProcess, "是否修复周跳"); }

        #region 钟跳
        /// <summary>
        /// 钟跳总开关
        /// </summary>
        public bool IsOpenClockJumpSwitcher { get => GetBool(OptionName.IsOpenClockJumpSwitcher); set => Set(OptionName.IsOpenClockJumpSwitcher, value, PreProcess, "是否进行钟跳探测并标记"); }

        /// <summary>
        /// 是否进行钟跳探测并标记
        /// </summary>
        public bool IsDetectClockJump { get => GetBool(OptionName.IsDetectClockJump); set => Set(OptionName.IsDetectClockJump, value, PreProcess, "是否进行钟跳探测并标记"); } 
        /// <summary>
        /// 是否修复钟跳
        /// </summary>
        public bool IsClockJumpReparationRequired { get => GetBool(OptionName.IsClockJumpReparationRequired); set => Set(OptionName.IsClockJumpReparationRequired, value, PreProcess, "是否修复钟跳"); } 
       
        /// <summary>
        /// 外部钟跳文件
        /// </summary>
        public string OuterClockJumpFile { get => GetString(OptionName.OuterClockJumpFile); set => Set(OptionName.OuterClockJumpFile, value, DataSource, "外部钟跳文件"); }

        #endregion
        /// <summary>
        /// 是否输出钟跳文件
        /// </summary>
        public bool IsOutputJumpClockFile { get => GetBool(OptionName.IsOutputJumpClockFile); set => Set(OptionName.IsOutputJumpClockFile, value, PreProcess, "是否输出钟跳文件"); }

        /// <summary>
        /// 是否输出周跳文件
        /// </summary>
        public bool IsOutputCycleSlipFile { get => GetBool(OptionName.IsOutputCycleSlipFile); set => Set(OptionName.IsOutputCycleSlipFile, value, PreProcess, "是否输出周跳文件"); } 
        #endregion
        /// <summary>
        /// 是否将初始相位采用伪距对齐
        /// </summary>
        public bool IsAliningPhaseWithRange { get => GetBool(OptionName.IsAliningPhaseWithRange); set => Set(OptionName.IsAliningPhaseWithRange, value, PreProcess, "是否将初始相位采用伪距对齐"); } 
        /// <summary>
        /// 是否需要多普勒频率
        /// </summary>
        public bool IsDopplerShiftRequired { get => GetBool(OptionName.IsDopplerShiftRequired); set => Set(OptionName.IsDopplerShiftRequired, value, PreProcess, "是否需要多普勒频率"); } 
        /// <summary>
        /// 多历元处理中，是否需要相同的卫星
        /// </summary>
        public bool IsRequireSameSats { get => GetBool(OptionName.IsRequireSameSats); set => Set(OptionName.IsRequireSameSats, value, PreProcess, "多历元处理中，是否需要相同的卫星"); } 
        /// <summary>
        /// 多测站中，是否允许某历元丢失个别站。
        /// </summary>
        public bool IsAllowMissingEpochSite { get => GetBool(OptionName.IsAllowMissingEpochSite); set => Set(OptionName.IsAllowMissingEpochSite, value, PreProcess, " 多测站中，是否允许某历元丢失个别站。"); } 

        /// <summary>
        /// 允许最小的伪距
        /// </summary>
        public double MinAllowedRange { get => GetDouble(OptionName.MinAllowedRange); set => Set(OptionName.MinAllowedRange, value, PreProcess, "允许最小的伪距"); } // = 15000000.0;
        /// <summary>
        /// 允许最大的伪距
        /// </summary>
        public double MaxAllowedRange { get => GetDouble(OptionName.MaxAllowedRange); set => Set(OptionName.MaxAllowedRange, value, PreProcess, "允许最大的伪距"); } //= 40000000.0; //35770602.136176817，同步卫星
        #region 输出
        /// <summary>
        /// 是否启用对历元结果的分析，是对平差过程的评估
        /// </summary>
        public bool IsEnableEpochParamAnalysis { get => GetBool(OptionName.IsEnableEpochParamAnalsis); set => Set(OptionName.IsEnableEpochParamAnalsis, value, Output, "是否启用对历元结果的分析，是对平差过程的评估。"); }
        /// <summary>
        /// 待分析的参数名称字符串
        /// </summary>
        public string AnalysisParamNamesString { get => GetString(OptionName.AnalysisParamNamesString); set => Set(OptionName.AnalysisParamNamesString, value, Output, "待分析的参数名称字符串"); }
        
        /// <summary>
        /// 待分析的参数名称
        /// </summary>
        public List<string> AnalysisParamNames { get => Geo.Utils.ListUtil.Parse(AnalysisParamNamesString); }

        /// <summary>
        /// 是否输出平差矩阵文本文件，主要用于平差测试
        /// </summary>
        public bool IsOutputAdjustMatrix { get => GetBool(OptionName.IsOutputAdjustMatrix); set => Set(OptionName.IsOutputAdjustMatrix, value, Output, "是否输出平差矩阵文本文件，主要用于平差测试。"); }

        #region 输出目录设置
        /// <summary>
        /// 输出目录设置
        /// </summary>
        public event Func<GnssProcessOption ,string> GettingOutputDirectory;
        /// <summary>
        /// 计算目录
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        public string GetSolverDirectory(TimePeriod timePeriod)
        {
            return GetSolverDirectory(timePeriod, this.GnssSolverType);
        }
        /// <summary>
        /// 指定算法的目录
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="gnssSolverType"></param>
        /// <returns></returns>
        public string GetSolverDirectory(TimePeriod timePeriod, GnssSolverType gnssSolverType)
        {
            var path = System.IO.Path.Combine(OutputDirectory, timePeriod.ToDefualtPathString(), gnssSolverType.ToString());
            Geo.Utils.FileUtil.CheckOrCreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 原始输出目录
        /// </summary>
        public string OringalOutputDirectory { get; set; }

        private string _OutputDirectory { get; set; }
        /// <summary>
        /// 结果输出目录
        /// </summary>
        public string OutputDirectory
        {
            get
            {
                if (GettingOutputDirectory != null)
                {
                    return GettingOutputDirectory?.Invoke(this);
                }
                return _OutputDirectory;
            }
            set
            {
                _OutputDirectory = value;
            }
        }
        #endregion

        /* { get => GetString(OptionName.OutputDirectory);
             set => Set(OptionName.OutputDirectory, value, Output, "结果输出目录"); } */
        /// <summary>
        /// 是否输出SINEX文件
        /// </summary>
        public bool IsOutputSinex { get => GetBool(OptionName.IsOutputSinex); set => Set(OptionName.IsOutputSinex, value, Output, "是否输出SINEX文件"); } 
        /// <summary>
        /// 是否输出汇总文件
        /// </summary>
        public bool IsOutputSummery { get => GetBool(OptionName.IsOutputSummery); set => Set(OptionName.IsOutputSummery, value, Output, "是否输出汇总文件"); } 
        /// <summary>
        /// 输出结果缓存大小
        /// </summary>
        public int OutputBufferCount { get => GetInt(OptionName.OutputBufferCount); set => Set(OptionName.OutputBufferCount, value, Output, "输出结果缓存大小"); }
        /// <summary>
        /// 是否输出历元坐标
        /// </summary>
        public bool IsOutputEpochCoord { get => GetBool(OptionName.IsOutputEpochCoord); set => Set(OptionName.IsOutputEpochCoord, value, Output, "是否输出历元坐标"); }
        /// <summary>
        /// 是否输出历元DOP值
        /// </summary>
        public bool IsOutputEpochDop { get => GetBool(OptionName.IsOutputEpochDop); set => Set(OptionName.IsOutputEpochDop, value, Output, "是否输出历元DOP值"); }
        /// <summary>
        /// 是否输出历元观测残差
        /// </summary>
        public bool IsOutputObservation { get => GetBool(OptionName.IsOutputObservation); set => Set(OptionName.IsOutputObservation, value, Output, "是否输出历元观测残差"); }
     /// <summary>
        /// 是否输出历元残差
        /// </summary>
        public bool IsOutputResidual { get => GetBool(OptionName.IsOutputResidual); set => Set(OptionName.IsOutputResidual, value, Output, "是否输出历元算后残差"); }


        /// <summary>
        /// 历元输出最小间隔
        /// </summary>
        public double OutputMinInterval { get => GetDouble(OptionName.OutputMinInterval); set => Set(OptionName.OutputMinInterval, value, Output, "历元输出最小间隔"); }
        #endregion
        /// <summary>
        /// 缓存数量
        /// </summary>
        public int BufferSize { get => GetInt(OptionName.BufferSize); set => Set(OptionName.BufferSize, value, Calculation, "缓存数量"); } 
        /// <summary>
        /// 历元间最大的时间间隙，单位：秒，如果历元之间超过了这个时段，则清空以往数据，重新构建对象。
        /// </summary>
        public double MaxEpochSpan { get => GetDouble(OptionName.MaxEpochSpan); set => Set(OptionName.MaxEpochSpan, value, PreProcess, "历元间最大的时间间隙，单位：秒，如果历元之间超过了这个时段，则清空以往数据，重新构建对象。"); } 

        /// <summary>
        /// 卫星连续观测的最小历元数量(单位：历元次)。即如果小于这个间隔，则抹去，不参与计算，以免影响精度。 
        /// </summary>
        public int MinContinuouObsCount { get => GetInt(OptionName.MinContinuouObsCount); set => Set(OptionName.MinContinuouObsCount, value, Calculation, " 卫星连续观测的最小历元数量(单位：历元次)。即如果小于这个间隔，则抹去，不参与计算，以免影响精度。"); } 
        /// <summary>
        /// 最小卫星数量
        /// </summary>
        public int MinSatCount { get; set; }// { get => GetInt(OptionName.MinSatCount); set => Set(OptionName.MinSatCount, value, Calculation, "最小卫星数量"); } 
        /// <summary>
        /// 至少的观测频率数量
        /// </summary>
        public int MinFrequenceCount { get => GetInt(OptionName.MinFrequenceCount); set => Set(OptionName.MinFrequenceCount, value, Calculation, "至少的观测频率数量"); }

        #region  多频多系统

        /// <summary>
        /// 多系统定位权值
        /// </summary>
        public Dictionary<SatelliteType, double> SystemStdDevFactors
        {
            get { return Geo.Utils.DictionaryUtil.Parse<SatelliteType, double>(SystemStdDevFactorString, Geo.Utils.EnumUtil.Parse<SatelliteType>, double.Parse); }
            set { this.SystemStdDevFactorString = Geo.Utils.DictionaryUtil.ToString(value); }
        }
        /// <summary>
        /// 多系统标准差存储
        /// </summary>
        public string SystemStdDevFactorString { get { return (string)Data.Get(OptionName.SystemStdDevFactorString).Value; } set => Set(OptionName.SystemStdDevFactorString, value, Calculation, "多系统标准差"); }
      
        /// <summary>
        /// 不同卫星的固有权值
        /// </summary>
        public Dictionary<SatelliteNumber, double> SatelliteStdDevs
        {
            get { return Geo.Utils.DictionaryUtil.Parse<SatelliteNumber, double>(SatelliteStdDevsString, SatelliteNumber.Parse, double.Parse); }
            set { this.SatelliteStdDevsString = Geo.Utils.DictionaryUtil.ToString(value); }
        }  /// <summary>
        /// 不同卫星的固有标准差
        /// </summary>
        public string SatelliteStdDevsString { get { return (string)Data.Get(OptionName.SatelliteCovasString).Value; } set => Set(OptionName.SatelliteCovasString, value, Calculation, "不同卫星的固有标准差"); }

        #endregion


        #region 通用
        /// <summary>
        /// 伪距类型
        /// </summary>
        public RangeType RangeType { get => GetEnumType<RangeType>(OptionName.RangeType); set => Set(OptionName.RangeType, value, Calculation, "伪距类型"); }

        /// <summary>
        /// 是否采用双频无电离层组合观测值
        /// </summary>
        public bool IsDualIonoFreeComObservation=> ObsDataType.ToString().Contains("IonoFree");
        /// <summary>
        /// 用于计算的观测值变量类型,此设置用于观测值的获取，周跳探测，近似观测值，载波相位平滑伪距等。
        /// </summary>
        public SatObsDataType ObsDataType { get; set; }// { get => GetEnumType<SatObsDataType>(OptionName.ObsDataType); set => Set(OptionName.ObsDataType, value, Calculation, "用于载波计算的观测值变量类型,此设置用于周跳探测，近似观测值获取等。"); } 
                                                       /// <summary>
                                                       /// 近似值的数据类型，在观测方程的右手边的数值，用于计算残差
                                                       /// </summary>
        public SatApproxDataType ApproxDataType { get; set; }// { get => GetEnumType<SatApproxDataType>(OptionName.ApproxDataType); set => Set(OptionName.ApproxDataType, value, Calculation, "近似值的数据类型，用于计算残差"); } 
        /// <summary>
        /// 参与计算的卫星类型，系统类型。
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get => (List<SatelliteType>)Data.Get(OptionName.SatelliteTypes).Value; set => Set(OptionName.SatelliteTypes, value, Calculation, "参与计算的卫星类型，系统类型。"); } 
        #endregion 

        /// <summary>
        ///  计算方式
        /// </summary>
        public CaculateType CaculateType { get => GetEnumType<CaculateType>(OptionName.CaculateType); set => Set(OptionName.CaculateType, value, Calculation, "计算方式"); } 
        /// <summary>
        /// 是否剔除粗差
        /// </summary>
        public bool RejectGrossError { get => GetBool(OptionName.RejectGrossError); set => Set(OptionName.RejectGrossError, value, Calculation, "是否剔除粗差"); } 
        /// <summary>
        /// 是否启用单独的钟差服务（文件）
        /// </summary>
        public bool EnableClockService { get => GetBool(OptionName.EnableClockService); set => Set(OptionName.EnableClockService, value, DataSource, " 是否启用单独的钟差服务（文件）"); } 

        /// <summary>
        /// 最大均方差，阈值。
        /// </summary>
        public double MaxStdDev { get => GetDouble(OptionName.MaxStdDev); set => Set(OptionName.MaxStdDev, value, Calculation, " 最大均方差，阈值。"); } 
        /// <summary>
        /// 是否是精密轨道 precise orbit?
        /// </summary>
        public bool IsPreciseOrbit { get => GetBool(OptionName.IsPreciseOrbit); set => Set(OptionName.IsPreciseOrbit, value, Calculation, "是否是精密轨道"); } 
        /// <summary>
        ///  高度截止角 vert angle cutoff (deg)
        /// </summary>
        public double VertAngleCut { get => GetDouble(OptionName.VertAngleCut); set => Set(OptionName.VertAngleCut, value, Calculation, "高度截止角"); } 
        /// <summary>
        /// 是否过滤粗差
        /// </summary>
        public bool FilterCourceError { get => GetBool(OptionName.FilterCourceError); set => Set(OptionName.FilterCourceError, value, Calculation, "是否过滤粗差"); } 

        /// <summary>
        /// 最大迭代次数。
        /// </summary>
        public int MaxLoopCount { get; set; }//{ get => GetInt(OptionName.MaxLoopCount); set => Set(OptionName.MaxLoopCount, value, Calculation, "最大迭代次数"); } 
        /// <summary>
        /// 是否启用迭代
        /// </summary>
        public bool EnableLoop { get => GetBool(OptionName.EnableLoop); set => Set(OptionName.EnableLoop, value, Calculation, "是否启用迭代"); } 
        /// <summary>
        /// 最大平均均方根倍数。
        /// </summary>
        public double MaxMeanStdTimes { get => GetDouble(OptionName.MaxMeanStdTimes); set => Set(OptionName.MaxMeanStdTimes, value, Calculation, "最大平均均方根倍数"); } 
            /// <summary>
        /// 免检均方根数值
        /// </summary>
        public double ExemptedStdDev { get => GetDouble(OptionName.ExemptedStdDev); set => Set(OptionName.ExemptedStdDev, value, Calculation, "免检均方根数值"); } 
        
        /// <summary>
        /// 启用逆序周跳探测
        /// </summary>
        public bool IsReverseCycleSlipeRevise { get => GetBool(OptionName.IsReverseCycleSlipeRevise); set => Set(OptionName.IsReverseCycleSlipeRevise, value, PreProcess, "启用逆序周跳探测"); } 
        #endregion

        #region 随机模型参数
        /// <summary>
        /// 卫星相位与伪距观测量的权比。
        /// </summary>
        public double PhaseCovaProportionToRange { get => GetDouble(OptionName.PhaseCovaProportionToRange); set => Set(OptionName.PhaseCovaProportionToRange, value, Adjustment, "卫星相位与伪距观测量的权比"); } 


        /// <summary>
        /// 随机模型参数， 随机游走模型的标准差。
        /// </summary>
        public double StdDevOfSysTimeRandomWalkModel { get => GetDouble(OptionName.StdDevOfSysTimeRandomWalkModel); set => Set(OptionName.StdDevOfSysTimeRandomWalkModel, value, Adjustment, "随机模型参数， 随机游走模型的标准差。"); } 
        /// <summary>
        /// 随机模型参数， 随机游走模型的标准差。
        /// </summary>
        public double StdDevOfRandomWalkModel { get => GetDouble(OptionName.StdDevOfRandomWalkModel); set => Set(OptionName.StdDevOfRandomWalkModel, value, Adjustment, "随机模型参数， 随机游走模型的标准差。"); } 
        /// <summary>
        /// 随机模型参数， 载波相位模型的标准差。
        /// </summary>
        public double StdDevOfPhaseModel { get => GetDouble(OptionName.StdDevOfPhaseModel); set => Set(OptionName.StdDevOfPhaseModel, value, Adjustment, " 随机模型参数， 载波相位模型的标准差。"); } 
        /// <summary>
        /// 随机模型参数， 发生周跳时，载波相位模型的标准差。
        /// </summary>
        public double StdDevOfCycledPhaseModel { get => GetDouble(OptionName.StdDevOfCycledPhaseModel); set => Set(OptionName.StdDevOfCycledPhaseModel, value, Adjustment, " 随机模型参数， 发生周跳时，载波相位模型的标准差。"); } 
        /// <summary>
        /// 随机模型参数， 电离层随机游走模型的标准差。
        /// </summary>
        public double StdDevOfIonoRandomWalkModel { get => GetDouble(OptionName.StdDevOfIonoRandomWalkModel); set => Set(OptionName.StdDevOfIonoRandomWalkModel, value, Adjustment, " 随机模型参数， 电离层随机游走模型的标准差。"); } 
        /// <summary>
        /// 随机模型参数， 静态模型的标准差。
        /// </summary>
        public double StdDevOfStaticTransferModel { get => GetDouble(OptionName.StdDevOfStaticTransferModel); set => Set(OptionName.StdDevOfStaticTransferModel, value, Adjustment, "随机模型参数， 静态模型的标准差。"); } 
        /// <summary>
        /// 随机模型参数， 对流层随机游走模型的标准差。
        /// </summary>
        public double StdDevOfTropoRandomWalkModel { get => GetDouble(OptionName.StdDevOfTropoRandomWalkModel); set => Set(OptionName.StdDevOfTropoRandomWalkModel, value, Adjustment, "随机模型参数， 对流层随机游走模型的标准差。"); }
        /// <summary>
        /// 接收机钟差随机模型参数，白噪声模型的标准差。
        /// </summary>
        public double StdDevOfRevClockWhiteNoiseModel { get => GetDouble(OptionName.StdDevOfRevClockWhiteNoiseModel); set => Set(OptionName.StdDevOfRevClockWhiteNoiseModel, value, Adjustment, " 钟差随机模型参数，白噪声模型的标准差。"); }

        /// <summary>
        /// 卫星钟差随机模型参数，白噪声模型的标准差。
        /// </summary>
        public double StdDevOfSatClockWhiteNoiseModel { get => GetDouble(OptionName.StdDevOfSatClockWhiteNoiseModel); set => Set(OptionName.StdDevOfSatClockWhiteNoiseModel, value, Adjustment, " 卫星钟差随机模型参数，白噪声模型的标准差。"); }
        /// <summary>
        /// 动态定位随机模型参数，白噪声模型的标准差。
        /// </summary>
        public double StdDevOfWhiteNoiseOfDynamicPosition { get => GetDouble(OptionName.StdDevOfWhiteNoiseOfDynamicPosition); set => Set(OptionName.StdDevOfWhiteNoiseOfDynamicPosition, value, Adjustment, " 动态定位随机模型参数，白噪声模型的标准差。"); } 

        #endregion

        /// <summary>
        /// 是否需要用坐标服务设置测站初值
        /// </summary>
        public bool IsSetApproxXyzWithCoordService { get => GetBool(OptionName.IsSetApproxXyzWithCoordService); set => Set(OptionName.IsSetApproxXyzWithCoordService, value, Calculation, "是否需要用坐标服务设置测站初值"); } 

        #region 来自于多历元的设置

        /// <summary>
        /// 参与差分卫星的数量
        /// </summary>
        public int MutliEpochSameSatCount { get => GetInt(OptionName.MutliEpochSameSatCount); set => Set(OptionName.MutliEpochSameSatCount, value, Calculation, "参与差分卫星的数量"); }
   

        /// <summary>
        /// 系数阵历元数量
        /// </summary>
        public int MultiEpochCount { get => GetInt(OptionName.MultiEpochCount); set => Set(OptionName.MultiEpochCount, value, Calculation, "系数阵历元数量"); }
        /// <summary>
        /// 是否平滑移动多历元窗口，否则采用分段移动
        /// </summary>
        public bool IsSmoothMoveInMultiEpoches { get => GetBool(OptionName.IsSmoothMoveInMultiEpoches); set => Set(OptionName.IsSmoothMoveInMultiEpoches, value, Calculation, "是否平滑移动多历元窗口"); }

        #endregion

        #region 文件 输入输出
        /// <summary>
        /// 是否输出结果的总开关，只有此为true才会判断下面的输出
        /// </summary>
        public bool IsOutputResult { get => GetBool(OptionName.IsOutputResult); set => Set(OptionName.IsOutputResult, value, Output, "是否输出结果的总开关，只有此为true才会判断下面的输出"); }
        /// <summary>
        /// 是否输出历元卫星信息
        /// </summary>
        public bool IsOutputEpochSatInfo { get => GetBool(OptionName.IsOutputEpochSatInfo); set => Set(OptionName.IsOutputEpochSatInfo, value, Output, "是否输出历元卫星信息"); } 
        /// <summary>
        /// 是否输出平差文件
        /// </summary>
        public bool IsOutputAdjust { get; set; }// { get => GetBool(OptionName.IsOutputAdjust); set => Set(OptionName.IsOutputAdjust, value, Output, "是否输出平差文件"); } 
       /// <summary>
       /// 是否输出观测方程
       /// </summary>
        public bool IsOutputObsEquation { get; set; }
        /// <summary>
        /// 是否输出电离层产品文件
        /// </summary>
        public bool IsOutputIono { get => GetBool(OptionName.IsOutputIono); set => Set(OptionName.IsOutputIono, value, Output, "是否输出电离层产品文件"); } 
        /// <summary>
        /// 是否输出对流层湿延迟产品文件
        /// </summary>
        public bool IsOutputWetTrop { get => GetBool(OptionName.IsOutputWetTrop); set => Set(OptionName.IsOutputWetTrop, value, Output, "是否输出对流层湿延迟产品文件"); } 
       /// <summary>
       /// 是否以GNSSer格式输出
       /// </summary>
        public bool IsOutputInGnsserFormat { get => GetBool(OptionName.IsOutputInGnsserFormat); set => Set(OptionName.IsOutputInGnsserFormat, value, Output, "是否以GNSSer格式输出"); } 
       
        /// <summary>
        /// 是否输出逐个历元计算结果
        /// </summary>
        public bool IsOutputEpochResult { get; set; }//{ get => GetBool(OptionName.IsOutputEpochResult); set => Set(OptionName.IsOutputEpochResult, value, Output, "是否输出逐个历元计算结果"); } 
        #endregion

        #region 星历计算选项
        /// <summary>
        /// 用于拟合的最小连续星历数量。
        /// </summary>
        public int MinSuccesiveEphemerisCount { get => GetInt(OptionName.MinSuccesiveEphemerisCount); set => Set(OptionName.MinSuccesiveEphemerisCount, value, Calculation, "用于拟合的最小连续星历数量。"); } 

        /// <summary>
        /// 在获取星历失败后，是否切换星历数据源
        /// </summary>
        public bool IsSwitchWhenEphemerisNull { get => GetBool(OptionName.IsSwitchWhenEphemerisNull); set => Set(OptionName.IsSwitchWhenEphemerisNull, value, DataSource, "在获取星历失败后，是否切换星历数据源"); } 
        #endregion

        #region 基线选项
        /// <summary>
        /// 长基线的最小长度,单位米
        /// </summary>
        public double MinDistanceOfLongBaseLine { get => GetDouble(OptionName.MinDistanceOfLongBaseLine); set => Set(OptionName.MinDistanceOfLongBaseLine, value, Calculation, "长基线的最小长度,单位米"); } 
        /// <summary>
        /// 短基线的最大长度，单位米
        /// </summary>
        public double MaxDistanceOfShortBaseLine { get => GetDouble(OptionName.MaxDistanceOfShortBaseLine); set => Set(OptionName.MaxDistanceOfShortBaseLine, value, Calculation, "短基线的最大长度，单位米"); } 
        #endregion

        #region 模糊度与固定参数算法选项
        /// <summary>
        /// 固定参数（模糊度）采用条件平差还是无限权解法
        /// </summary>
        public bool IsFixParamByConditionOrHugeWeight { get; set; }// { get => GetBool(OptionName.IsFixParamByConditionOrHugeWeight); set => Set(OptionName.IsFixParamByConditionOrHugeWeight, value, Calculation, "固定参数（模糊度）采用条件平差还是无限权解法"); }
        /// <summary>
        ///模糊度固定成功的最小比例
        /// </summary>
        public double MinFixedAmbiRatio { get; set; }//  { get => GetDouble(OptionName.MinFixedAmbiRatio); set => Set(OptionName.MinFixedAmbiRatio, value, Calculation, "模糊度固定成功的最小比例"); }
        /// <summary>
        /// Lambda算法的最大Ratio值
        /// </summary>
        public double MaxRatioOfLambda { get; set; }// { get => GetDouble(OptionName.MaxRatioOfLambda); set => Set(OptionName.MaxRatioOfLambda, value, Calculation, "模糊度固定成功的最小比例"); }
        /// <summary>
        ///当浮点解RMS二范数小于此时尝试固定模糊度
        /// </summary>
        public double MaxFloatRmsNormToFixAmbiguity { get; set; }//  { get => GetDouble(OptionName.MaxFloatRmsNormToFixAmbiguity); set => Set(OptionName.MaxFloatRmsNormToFixAmbiguity, value, Calculation, "当浮点解RMS二范数小于此时尝试固定模糊度"); }
        /// <summary>
        /// 是否启用模糊度文件
        /// </summary>
        public bool IsUsingAmbiguityFile { get; set; }// { get => GetBool(OptionName.IsUsingAmbiguityFile); set => Set(OptionName.IsUsingAmbiguityFile, value, Calculation, "是否启用模糊度文件"); }
        /// <summary>
        /// 模糊度文件
        /// </summary>
        public string AmbiguityFilePath { get => GetString(OptionName.AmbiguityFilePath); set => Set(OptionName.AmbiguityFilePath, value, DataSource, "模糊度文件"); }

        /// <summary>
        /// 模糊度浮点数与整数允许的最大偏差，周,不应大于0.5
        /// </summary>
        public double MaxAmbiDifferOfIntAndFloat { get; set; }//  { get => GetDouble(OptionName.MaxAmbiDifferOfIntAndFloat); set => Set(OptionName.MaxAmbiDifferOfIntAndFloat, value, Calculation, "模糊度浮点数与整数允许的最大偏差，周"); }
         /// <summary>
         /// 四舍五入法的最大偏差
         /// </summary>
        public double MaxRoundAmbiDifferOfIntAndFloat { get; set; }
        #endregion

        /// <summary>
        /// 周跳开关.优先考虑周跳探测器开关,如为空，然后考虑默认周跳探测器。
        /// </summary>
        public Dictionary<CycleSlipDetectorType, bool> CycleSlipDetectSwitcher { get => (Dictionary<CycleSlipDetectorType, bool>)Data.Get(OptionName.CycleSlipDetectSwitcher).Value; set => Set(OptionName.CycleSlipDetectSwitcher, value, PreProcess, "周跳开关.优先考虑周跳探测器开关,如为空，然后考虑默认周跳探测器。"); }

        #region 结果检核
        /// <summary>
        /// 验后残差最大允许倍数
        /// </summary>
        public double MaxErrorTimesOfPostResdual { get; set; }//{ get => GetDouble(OptionName.MaxErrorTimesOfPostResdual); set => Set(OptionName.MaxErrorTimesOfPostResdual, value, PreProcess, "验后残差最大允许倍数"); }
        /// <summary>
        /// 是否启用结果检核，总开关
        /// </summary>
        public bool IsResultCheckEnabled { get; set; }//{ get => GetBool(OptionName.IsResultCheckEnabled); set => Set(OptionName.IsResultCheckEnabled, value, Calculation, "是否启用验后残差检核"); } 

        /// <summary>
        /// 是否启用验后残差检核
        /// </summary>
        public bool IsResidualCheckEnabled { get; set; }// { get => GetBool(OptionName.IsResidualCheckEnabled); set => Set(OptionName.IsResidualCheckEnabled, value, Calculation, "是否启用验后残差检核"); } 
        /// <summary>
        /// 当结果变化时，是否进行手动升高状态转移矩阵的噪声
        /// </summary>
        public bool IsPromoteTransWhenResultValueBreak { get; set; }//{ get => GetBool(OptionName.IsPromoteTransWhenResultValueBreak); set => Set(OptionName.IsPromoteTransWhenResultValueBreak, value, Calculation, " 当结果变化时，是否进行手动升高状态转移矩阵的噪声"); } 
        #endregion

        #region 改正相关配置
        /// <summary>
        ///是否需要观测值改正
        /// </summary>
        public bool IsObsCorrectionRequired { get => GetBool(OptionName.IsObsCorrectionRequired); set => Set(OptionName.IsObsCorrectionRequired, value, Correction, "是否需要观测值改正"); } 
        /// <summary>
        /// 是否需要近似模型改正，固体潮等
        /// </summary>
        public bool IsApproxModelCorrectionRequired { get => GetBool(OptionName.IsApproxModelCorrectionRequired); set => Set(OptionName.IsApproxModelCorrectionRequired, value, Correction, "是否需要近似模型改正"); } 
        /// <summary>
        /// 是否需要DCB改正
        /// </summary>
        public bool IsDcbCorrectionRequired { get => GetBool(OptionName.IsDcbCorrectionRequired); set => Set(OptionName.IsDcbCorrectionRequired, value, Correction, "是否需要DCB改正"); } 
        /// <summary>
        /// 接收机天线PCO改正
        /// </summary>
        public bool IsReceiverAntSiteBiasCorrectionRequired { get => GetBool(OptionName.IsReceiverAntSiteBiasCorrectionRequired); set => Set(OptionName.IsReceiverAntSiteBiasCorrectionRequired, value, Correction, "接收机天线PCO改正"); } 
        /// <summary>
        /// 海洋潮汐改正
        /// </summary>
        public bool IsOceanTideCorrectionRequired { get => GetBool(OptionName.IsOceanTideCorrectionRequired); set => Set(OptionName.IsOceanTideCorrectionRequired, value, Correction, "海洋潮汐改正"); } 
        /// <summary>
        /// 固体潮改正
        /// </summary>
        public bool IsSolidTideCorrectionRequired { get => GetBool(OptionName.IsSolidTideCorrectionRequired); set => Set(OptionName.IsSolidTideCorrectionRequired, value, Correction, "固体潮改正"); } 
        /// <summary>
        /// 极潮改正
        /// </summary>
        public bool IsPoleTideCorrectionRequired { get => GetBool(OptionName.IsPoleTideCorrectionRequired); set => Set(OptionName.IsPoleTideCorrectionRequired, value, Correction, "极潮改正"); } 
        /// <summary>
        /// 卫星钟差改正
        /// </summary>
        public bool IsSatClockBiasCorrectionRequired { get => GetBool(OptionName.IsSatClockBiasCorrectionRequired); set => Set(OptionName.IsSatClockBiasCorrectionRequired, value, Correction, "卫星钟差改正"); } 
        /// <summary>
        /// 对流层改正
        /// </summary>
        public bool IsTropCorrectionRequired { get => GetBool(OptionName.IsTropCorrectionRequired); set => Set(OptionName.IsTropCorrectionRequired, value, Correction, "对流层改正"); } 
        /// <summary>
        /// 重力延迟改正
        /// </summary>
        public bool IsGravitationalDelayCorrectionRequired { get => GetBool(OptionName.IsGravitationalDelayCorrectionRequired); set => Set(OptionName.IsGravitationalDelayCorrectionRequired, value, Correction, "重力延迟改正"); }
        /// <summary>
        /// 卫星天线相位中心改正PCO
        /// </summary>
        public bool IsSatAntPcoCorrectionRequired { get => GetBool(OptionName.IsSatAntPcoCorrectionRequired); set => Set(OptionName.IsSatAntPcoCorrectionRequired, value, Correction, "卫星天线相位中心改正PCO"); }
        /// <summary>
        /// 卫星天线相位中心改正PVC
        /// </summary>
        public bool IsSatAntPvcCorrectionRequired { get => GetBool(OptionName.IsSatAntPvcCorrectionRequired); set => Set(OptionName.IsSatAntPvcCorrectionRequired, value, Correction, "卫星天线相位中心改正PVC"); }
        /// <summary>
        /// 接收机天线PCO改正
        /// </summary>
        public bool IsRecAntPcoCorrectionRequired { get => GetBool(OptionName.IsRecAntPcoCorrectionRequired); set => Set(OptionName.IsRecAntPcoCorrectionRequired, value, Correction, "接收机天线PCO改正"); } 
        /// <summary>
        /// 接收机天线PCV改正
        /// </summary>
        public bool IsRecAntPcvCorrectionRequired { get => GetBool(OptionName.IsRecAntPcvCorrectionRequired); set => Set(OptionName.IsRecAntPcvCorrectionRequired, value, Correction, "接收机天线PCV改正"); } 
        /// <summary>
        /// 相位缠绕改正
        /// </summary>
        public bool IsPhaseWindUpCorrectionRequired { get => GetBool(OptionName.IsPhaseWindUpCorrectionRequired); set => Set(OptionName.IsPhaseWindUpCorrectionRequired, value, Correction, "相位缠绕改正"); } 
        /// <summary>
        /// 测站改正
        /// </summary>
        public bool IsSiteCorrectionsRequired { get => GetBool(OptionName.IsSiteCorrectionsRequired); set => Set(OptionName.IsSiteCorrectionsRequired, value, Correction, "测站改正"); } 
        /// <summary>
        /// 伪距改正
        /// </summary>
        public bool IsRangeCorrectionsRequired { get => GetBool(OptionName.IsRangeCorrectionsRequired); set => Set(OptionName.IsRangeCorrectionsRequired, value, Correction, "伪距改正"); } 
        /// <summary>
        /// 是否需要频率改正
        /// </summary>
        public bool IsFrequencyCorrectionsRequired { get => GetBool(OptionName.IsFrequencyCorrectionsRequired); set => Set(OptionName.IsFrequencyCorrectionsRequired, value, Correction, "是否需要频率改正"); } 
        /// <summary>
        /// 是否需要GNSSer历元电离层文件
        /// </summary>
        public bool IsGnsserEpochIonoFileRequired { get => GetBool(OptionName.IsEpochIonoFileRequired); set => Set(OptionName.IsEpochIonoFileRequired, value, Calculation, "是否需要GNSSer历元电离层文件"); } 
        /// <summary>
        /// 是否需要电离层导航参数模型
        /// </summary>
        public bool IsNavIonoModelCorrectionRequired { get => GetBool(OptionName.IsNavIonoModelCorrectionRequired); set => Set(OptionName.IsNavIonoModelCorrectionRequired, value, Calculation, "是否需要电离层导航参数模型"); } 
        #endregion

        /// <summary>
        /// 是否对流层增强启用。
        /// </summary>
        public bool IsTropAugmentEnabled { get => GetBool(OptionName.IsTropAugmentEnabled); set => Set(OptionName.IsTropAugmentEnabled, value, Calculation, "是否对流层增强启用"); } 

        /// <summary>
        /// 是否需要 GPT2的1度格网文件
        /// </summary>
        public bool Isgpt2File1DegreeRequired { get => GetBool(OptionName.Isgpt2File1DegreeRequired); set => Set(OptionName.Isgpt2File1DegreeRequired, value, DataSource, "是否需要 GPT2的1度格网文件"); }
        /// <summary>
        /// 基准站选择方法
        /// </summary>
        public BaseSiteSelectType BaseSiteSelectType { get; set; }

        /// <summary>
        /// 是否要求相同卫星
        /// </summary>
        public bool IsSameSatRequired { get => GetBool(OptionName.IsSameSatRequired); set => Set(OptionName.IsSameSatRequired, value, Calculation, "是否要求相同卫星"); } 
        /// <summary>
        /// 是否选择基准卫星
        /// </summary>
        public bool IsBaseSatelliteRequried { get => GetBool(OptionName.IsBaseSatelliteRequried); set => Set(OptionName.IsBaseSatelliteRequried, value, Calculation, "是否选择基准卫星"); } 
               /// <summary>
        /// 是否选择基准测站
        /// </summary>
        public bool IsBaseSiteRequried { get => GetBool(OptionName.IsBaseSiteRequried); set => Set(OptionName.IsBaseSiteRequried, value, Calculation, "是否选择基准测站"); }
 
        /// <summary>
        /// 是否固定模糊度
        /// </summary>
        public bool IsFixingAmbiguity { get => GetBool(OptionName.IsFixingAmbiguity); set => Set(OptionName.IsFixingAmbiguity, value, Calculation, "是否固定模糊度"); } 
       
        
        /// <summary>
        /// 是否固定坐标,若true，则不用解算坐标。
        /// </summary>
        public bool IsFixingCoord { get => GetBool(OptionName.IsFixingCoord); set => Set(OptionName.IsFixingCoord, value, Calculation, "是否固定坐标。若是，则不用解算坐标。"); } 


        /// <summary>
        /// 是否需要坐标服务
        /// </summary>
        public bool IsSiteCoordServiceRequired { get => GetBool(OptionName.IsSiteCoordServiceRequired); set => Set(OptionName.IsSiteCoordServiceRequired, value, DataSource, "是否需要坐标服务"); } 

        /// <summary>
        ///是否指定测站信息文件
        /// </summary>
        public bool IsIndicatingStationInfoFile { get => GetBool(OptionName.IsIndicatingStationInfoFile); set => Set(OptionName.IsIndicatingStationInfoFile, value, DataSource, "是否指定测站信息文件"); } 
        /// <summary>
        /// 测站信息文件，主要包含天线时段信息。
        /// </summary>

        public bool IsStationInfoRequired { get => GetBool(OptionName.IsStationInfoRequired); set => Set(OptionName.IsStationInfoRequired, value, DataSource, "测站信息文件，主要包含天线时段信息。"); } 

        /// <summary>
        /// 是否启用先验值
        /// </summary>
        public bool IsEnableInitApriori { get => GetBool(OptionName.IsEnableInitApriori); set => Set(OptionName.IsEnableInitApriori, value, Adjustment, "是否启用先验值"); } 
        /// <summary>
        /// 初始先验值
        /// </summary>
        public WeightedVector InitApriori { get => (WeightedVector)Data.Get(OptionName.InitApriori).Value;
            set
            {
                Set(OptionName.InitApriori, value, Adjustment, "初始先验值");
                if(value == null) { return; }

                var str = "设置了初始先验值 Value and Cova: \r\n";
                str += value.ToTwoLineText();
                log.Info(str);
            }
        }
        /// <summary>
        /// 是否启用NGA星历匹配，作为实时计算的备份。
        /// </summary>
        public bool IsEnableNgaEphemerisSource { get => GetBool(OptionName.IsEnableNgaEphemerisSource); set => Set(OptionName.IsEnableNgaEphemerisSource, value, DataSource, "是否启用NGA星历匹配，作为实时计算的备份。"); }
        /// <summary>
        /// 从电离层文件获取， 是否启用P1到无电离层的硬件延迟改正，在单频P1中使用LC产品时需要。
        /// </summary>
        public bool IsP1DcbToLcOfGridIonoRequired { get => GetBool(OptionName.IsP1DcbToLcRequired); set => Set(OptionName.IsP1DcbToLcRequired, value, DataSource, "是否启用P1到无电离层的硬件延迟改正，在单频P1中使用LC产品时需要,此数据源来格网电离层文件。"); }
        /// <summary>
        /// 是否启用P1-P2到无电离层的硬件延迟改正，在单频P1中使用LC产品时需要。
        /// 与格网电离层中基本一样，但这个精度更低。
        /// </summary>
        public bool IsDcbOfP1P2Enabled { get => GetBool(OptionName.IsDcbOfP1P2Enabled); set => Set(OptionName.IsDcbOfP1P2Enabled, value, DataSource, "是否启用P1到无电离层的硬件延迟改正，在单频P1中使用LC产品时需要，数据源来自CODE的DCB文件。"); }

        /// <summary>
        /// 是否指定卫星，在某些场合使用
        /// </summary>
        public bool IsIndicatedPrn { get => GetBool(OptionName.IsIndicatedPrn); set => Set(OptionName.IsIndicatedPrn, value, Calculation, " 是否指定卫星，在某些场合使用"); }
        /// <summary>
        /// 指定的卫星
        /// </summary>
        public SatelliteNumber IndicatedPrn { get => GetSatelliteNumber(OptionName.IndicatedPrn); set => Set(OptionName.IndicatedPrn, value, Calculation, " 指定的卫星，在某些场合使用"); }
        /// <summary>
        /// 载波相位平滑伪距的窗口大小
        /// </summary>
        public int WindowSizeOfPhaseSmoothRange { get => GetInt(OptionName.WindowSizeOfPhaseSmoothRange); set => Set(OptionName.WindowSizeOfPhaseSmoothRange, value, Calculation, "载波相位平滑伪距的窗口大小"); }
        /// <summary>
        /// 是否采用GNSSer改进平滑伪距算法
        /// </summary>
        public bool IsUseGNSSerSmoothRangeMethod { get => GetBool(OptionName.IsUseGNSSerSmoothRangeMethod); set => Set(OptionName.IsUseGNSSerSmoothRangeMethod, value, Calculation, "是否采用GNSSer改进平滑伪距算法"); }
        /// <summary>
        /// 是否加权而非推估的平滑伪距
        /// </summary>
        public bool IsWeightedPhaseSmoothRange { get => GetBool(OptionName.IsWeightedPhaseSmoothRange); set => Set(OptionName.IsWeightedPhaseSmoothRange, value, Calculation, "是否加权而非推估的平滑伪距"); }

        /// <summary>
        /// 数据处理时，是否需要伪距定位，动态定位时需要
        /// </summary>
        public bool IsNeedPseudorangePositionWhenProcess { get => GetBool(OptionName.IsNeedPseudorangePositionWhenProcess); set => Set(OptionName.IsNeedPseudorangePositionWhenProcess, value, Calculation, "数据处理时，是否需要伪距定位，动态定位时需要"); }


        /// <summary>
        /// 预先伪距定位时，是否需要平滑伪距
        /// </summary>
        public bool IsSmoothRangeWhenPrevPseudorangePosition { get => GetBool(OptionName.IsSmoothRangeWhenPrevPseudorangePosition); set => Set(OptionName.IsSmoothRangeWhenPrevPseudorangePosition, value, Calculation, "预先伪距定位时，是否需要平滑伪距"); }
        /// <summary>
        /// 电离层变化率文件
        /// </summary>
        public string IonoDeltaFilePath { get => GetString(OptionName.IonoDeltaFilePath); set => Set(OptionName.IonoDeltaFilePath, value, DataSource, "电离层变化率文件"); }
         
        #region 方法
        /// <summary>
        /// 初始先验值是否可用。
        /// </summary>
        /// <returns></returns>
        public bool IsInitAprioriAvailable
        {
            get=> IsEnableInitApriori && InitApriori != null;
        }
        /// <summary>
        /// 所有GNSS系统是否采用同一个时间系统
        /// </summary>
        public bool IsSameTimeSystemInMultiGnss { get => GetBool(OptionName.IsSameTimeSystemInMultiGnss); set => Set(OptionName.IsSameTimeSystemInMultiGnss, value, Calculation, "所有GNSS系统是否采用同一个时间系统"); }

        /// <summary>
        /// 参数是否包含测站名称
        /// </summary>
        public bool IsSiteNameIncluded { get; set; }
        /// <summary>
        /// 是否输出历元参数
        /// </summary>
        public bool IsOutputEpochParam { get;  set; }
        /// <summary>
        /// 是否输出利用参数RMS
        /// </summary>
        public bool IsOutputEpochParamRms { get;  set; }
        /// <summary>
        /// 基线文件路径，必须指定基线类型后启用。
        /// </summary>
        public string BaseLineFilePath { get; set; }
        /// <summary>
        /// 中心站名称
        /// </summary>
        public string CenterSiteName { get; set; }
        /// <summary>
        /// 星历插值阶次
        /// </summary>
        public int EphemerisInterpolationOrder { get; set; }
        /// <summary>
        /// 测站近似值最大允许距离地心长度，超出则重新计算初始坐标
        /// </summary>
        public double MinAllowedApproxXyzLen { get;  set; }

        /// <summary>
        /// 检核参数是否矛盾
        /// </summary>
        /// <returns></returns>
        public EnabledMessage Check()
        {
            EnabledMessage msg = EnabledMessage.Ok;
            if (this.MinContinuouObsCount > this.BufferSize)
            {
                msg.Enabled = false;
                msg.Message = ("缓存小于最小连续卫星数，将剔除所有数据！");
            }
            return msg;
        }


        #region 常用
        /// <summary>
        /// 计算选项。
        /// </summary>
        /// <returns></returns>
        public static GnssProcessOption GetDefault(GnsserConfig GnsserConfig, IObsInfo obsInfo, List<SatelliteType> types = null)
        {
            if (types == null) types = obsInfo.SatelliteTypes;
            BufferedTimePeriod GpsTimePeriod = new BufferedTimePeriod(obsInfo.StartTime, obsInfo.StartTime + TimeSpan.FromDays(0.999));//??????时间设置
            GnssProcessOption model = new GnssProcessOption()
            {
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                EnableClockService = true,
                SatelliteTypes = types
            };
            return model;
        }

        /// <summary>
        /// 默认的精密单点定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultIonoFreePppOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.无电离层组合PPP,
                IsDcbOfP1P2Enabled = false,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                IsOutputResidual = true,
                EnableClockService = true,
                SatelliteTypes = types,
                MinSatCount = 2,
                VertAngleCut = 7,
                MinFrequenceCount = 2,
                IsAliningPhaseWithRange = false,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                IsEphemerisRequired = true,
                IsOutputEpochSatInfo = false,
                IsOutputCycleSlipFile = false,
                IsOutputEpochResult = true,
                IsIonoCorretionRequired = false,
                IsEnableEpochParamAnalysis = true,
                AnalysisParamNamesString= ParamNames.De + "," + ParamNames.Dn + "," + ParamNames.Du,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            return model;
        }

        /// <summary>
        /// 伪距定位选项
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="isSmoothRangeWithPhase">当无钟跳时，可以采用</param>
        /// <returns></returns>
        public static GnssProcessOption GetPsuedoRangeOption(double interval, bool isSmoothRangeWithPhase=false)
        {
            //实时定位
            var opt = GnssProcessOption.GetDefaultSimplePseudoRangePositioningOption();
            opt.IsCycleSlipDetectionRequired = isSmoothRangeWithPhase;
            opt.IsReverseCycleSlipeRevise = isSmoothRangeWithPhase;
            opt.AdjustmentType = AdjustmentType.参数平差;
            opt.IsUpdateEstimatePostition = true;
            opt.MaxStdDev = 1000000;
            //伪距平滑
            opt.IsSmoothRange = isSmoothRangeWithPhase;
            opt.SmoothRangeType = SmoothRangeType.PhaseSmoothRange;
            opt.IsResidualCheckEnabled = false;
                

            if (interval <= 0) { interval = 30; }
            opt.WindowSizeOfPhaseSmoothRange = (int)((10 * 60) / interval);

            //如果双频则直接采用无电离层组合
            if (true || opt.MinFrequenceCount >= 2)
            {
                opt.ObsDataType = SatObsDataType.IonoFreeRange;
                opt.ApproxDataType = SatApproxDataType.IonoFreeApproxPseudoRange;
                if (isSmoothRangeWithPhase)
                {
                    opt.CycleSlipDetectSwitcher[CycleSlipDetectorType.首次出现标记法] = true;
                    opt.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
                    opt.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
                }
            }
            else
            {
                opt.ObsDataType = SatObsDataType.PhaseRangeA;
                opt.ApproxDataType = SatApproxDataType.ApproxPseudoRangeA;
                //单频电离层
                opt.IsIonoCorretionRequired = true;
                opt.IonoSourceTypeForCorrection = IonoSourceType.CodeSphericalHarmonics;

                //伪距平滑及电离层改正
                if (isSmoothRangeWithPhase)
                {
                    opt.SmoothRangeSuperPosType = SmoothRangeSuperpositionType.快速更新算法;
                    opt.OrderOfDeltaIonoPolyFit = 1;
                    //opt.CycleSlipDetectSwitcher[CycleSlipDetectorType.高次差法] = true;
                    opt.CycleSlipDetectSwitcher[CycleSlipDetectorType.首次出现标记法] = true;
                    //opt.CycleSlipDetectSwitcher[CycleSlipDetectorType.数值平均法] = true;
                }
            }

            return opt;
        }

        /// <summary>
        /// 默认的精密单点定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultRecursiveIonoFreePppOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.递归无电离层组合PPP,
                FilterCourceError = true,
                AdjustmentType = AdjustmentType.递归最小二乘,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = Int32.MaxValue,
                IsOnlySameParam = false,
                EnableClockService = true,
                SatelliteTypes = types,
                
                VertAngleCut = 7,
                MinFrequenceCount = 2,
                IsResidualCheckEnabled = false,
                IsAliningPhaseWithRange = false,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                IsEphemerisRequired = true, 
                IsOutputEpochSatInfo = false,
                IsOutputCycleSlipFile = false,
                IsOutputEpochResult = true,
                StepOfRecursive = StepOfRecursive.SequentialConst
            };
            return model;
        }
        /// <summary>
        /// 默认的精密单点定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultFixedIonoFreePppOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.固定参考站PPP,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                IsOutputResidual = true,
                IsFixingAmbiguity = true,
                MaxFloatRmsNormToFixAmbiguity = 100000,
                IsEnableSiteSatPeriodDataService = true,
                IsOutputPeriodData = true,
                EnableClockService = true,
                VertAngleCut = 7,
                SatelliteTypes = types,
                IsAliningPhaseWithRange = false,

                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                IsEphemerisRequired = true,
                IsOutputEpochResult = true,
            };
            return model;
        }
        /// <summary>
        /// 默认的双差定轨参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultDoubleDifferOrbitOption(List<SatelliteType> types = null)
        { 
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.双差定轨,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = 100000,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 10,
                
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                ApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange,
                IsEphemerisRequired = true,
                IsSameSatRequired = true,
                IsBaseSatelliteRequried = true,
                IsAllowMissingEpochSite = false,
                IsSiteCoordServiceRequired = true,
                IsSetApproxXyzWithCoordService = true,
                IsApproxXyzRequired = true,
                MinDistanceOfLongBaseLine = 50 * 1000,
                MaxDistanceOfShortBaseLine = 20000,
                IsOutputEpochResult = true,
                IsBaseSiteRequried = true,
                StdDevOfWhiteNoiseOfDynamicPosition = 1000,
                IsResidualCheckEnabled = false,
                IsResultCheckEnabled = false,
                PositionType = PositionType.动态定位,
                IsEstimateTropWetZpd = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }


        /// <summary>
        /// 默认的单差网解参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultNetSinglePostionDifferOption(List<SatelliteType> types = null)
        { 
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.网解单差定位,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = 100000,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 10,
                IsRemoveIonoFreeUnavaliable = false,
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                ApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange,
                IsEphemerisRequired = true,
                IsSameSatRequired = true,
                IsBaseSatelliteRequried = true,
                IsAllowMissingEpochSite = false,
                IsSiteCoordServiceRequired = false,
                IsSetApproxXyzWithCoordService = false,
                IsApproxXyzRequired = true,
                MinDistanceOfLongBaseLine = 50 * 1000,
                MaxDistanceOfShortBaseLine = 20000,
                IsOutputEpochResult = true,
                IsBaseSiteRequried = true,
                IsResidualCheckEnabled = false,
                IsResultCheckEnabled = false,
                IsEstimateTropWetZpd = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }
   /// <summary>
        /// 默认的递归最小二乘双差网解参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultRecursiveNetDoublePostionDifferOption(List<SatelliteType> types = null)
        { 
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.递归网解双差定位,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                AdjustmentType = AdjustmentType.递归最小二乘,
                MaxStdDev = 100000,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 10,
                
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                ApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange,
                IsEphemerisRequired = true,
                IsSameSatRequired = true,
                IsBaseSatelliteRequried = true,
                IsAllowMissingEpochSite = false,
                IsSiteCoordServiceRequired = false,
                IsSetApproxXyzWithCoordService = false,
                IsApproxXyzRequired = true,
                MinDistanceOfLongBaseLine = 50 * 1000,
                MaxDistanceOfShortBaseLine = 20000,
                IsOutputEpochResult = true,
                IsBaseSiteRequried = true,
                IsResidualCheckEnabled = false,
                IsResultCheckEnabled = false,
                IsEstimateTropWetZpd = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }

        /// <summary>
        /// 默认的双差网解参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultNetDoublePostionDifferOption(List<SatelliteType> types = null)
        { 
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.网解双差定位,
                IsIonoCorretionRequired = true, //单频启用电离层改正
                IsFixingAmbiguity = true,
                IsEnableSiteSatPeriodDataService = true,
                MaxFloatRmsNormToFixAmbiguity = 0.2,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,                
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 12,                
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.PhaseRangeA,
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                //ObsDataType = SatObsDataType.IonoFreePhaseRange,
                //ApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange,
                IsEphemerisRequired = true,
                IsSameSatRequired = true,
                IsBaseSatelliteRequried = true,
                IsAllowMissingEpochSite = false,
                IsSiteCoordServiceRequired = false,//作弊不用指定
                IsSetApproxXyzWithCoordService = false,
                IsApproxXyzRequired = true,
                MinDistanceOfLongBaseLine = 100 * 1000,
                MaxDistanceOfShortBaseLine = 30 * 1000,
                IsOutputEpochResult = true,
                IsBaseSiteRequried = true,
                IsResidualCheckEnabled = true,
                IsPhaseInMetterOrCycle = false,//默认为波长,无电离层组合的距离长度
                IsResultCheckEnabled = true,
                IsEstimateTropWetZpd = false,
                IsEnableEpochParamAnalysis = true,
                IsOutputResidual = true,
                SequentialEpochCountOfAccuEval = 20,
                MaxDifferOfAccuEval = 0.01,
                MaxAllowedConvergenceTime = 240,
                KeyLabelCharCount = 6,
                MaxAllowedDifferAfterConvergence = 0.02,
                MaxAllowedRmsOfAccuEval = 0.05

            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }
        /// <summary>
        /// 默认的伪距定轨参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultSimpleRangeOrbitOption(List<SatelliteType> types = null)
        {
            GnssProcessOption model = GetDefaultZeroDifferOrbitOption(types);
            model.GnssSolverType = Gnsser.GnssSolverType.简易伪距定轨; 
            return model;
        }
        /// <summary>
        /// 默认的非差定轨参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultZeroDifferOrbitOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.非差定轨,
                OutputBufferCount = 30000,
                StdDevOfWhiteNoiseOfDynamicPosition = 10000,
                IsSetApproxXyzWithCoordService = true,
                IsRequireSameSats = false,
                IsAllowMissingEpochSite = true,
                FilterCourceError = true,
                IsSiteCoordServiceRequired = true,
                IsOutputEpochResult = true,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = 1000000,
                EnableClockService = true,
                SatelliteTypes = types,
                IsAliningPhaseWithRange = false,
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                IsEphemerisRequired = true,
                IsRemoveSmallPartSat = false,
                IsBaseSatelliteRequried = false,
                IsSameSatRequired = false,
                IsResultCheckEnabled = false,
                PositionType = PositionType.动态定位, 
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }

        /// <summary>
        /// 默认的精密单点定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultClockOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.钟差网解,
                OutputBufferCount = 3000,
                IsSetApproxXyzWithCoordService = true,
                IsRequireSameSats = false,
                IsAllowMissingEpochSite = true,
                FilterCourceError = true,
                IsSiteCoordServiceRequired = true,
                IsOutputEpochResult = true,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = 100000,
                EnableClockService = true,
                SatelliteTypes = types,
                IsAliningPhaseWithRange = false,
                MinSatCount = 2,
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                IsEphemerisRequired = true,
                IsRemoveSmallPartSat = false,
                IsBaseSatelliteRequried = false,
                IsSameSatRequired = false,
            };
            return model;
        }
        /// <summary>
        /// 默认的双差网解定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultNetDoubleDifferOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.双差网解定位,
                OutputBufferCount = 3000,
                IsSetApproxXyzWithCoordService = false,
                IsRequireSameSats = false,
                IsAllowMissingEpochSite = true,
                FilterCourceError = true,
                IsSiteCoordServiceRequired = true,
                IsOutputEpochResult = true,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = 100000,
                EnableClockService = true,
                SatelliteTypes = types,
                IsAliningPhaseWithRange = false,
                MinSatCount = 2,
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                IsEphemerisRequired = true,
                IsRemoveSmallPartSat = false,
                IsBaseSatelliteRequried = true,
                IsSameSatRequired = true,
                IsEnableEpochParamAnalysis = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }

        /// <summary>
        /// 无电离层双差的默认配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultIonoFreeDoubleDifferOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                IsEnableSiteSatPeriodDataService = true,
                GnssSolverType = Gnsser.GnssSolverType.无电离层双差,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                IsOutputResidual = true,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 15,
                MinSatCount = 2, 
                MinFrequenceCount = 2,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                ApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange,
                IsEphemerisRequired = true,
                IsSameSatRequired = true,
                IsBaseSatelliteRequried = true,
                IsAllowMissingEpochSite = false, 
                IsSetApproxXyzWithCoordService = false,
                IsApproxXyzRequired = true,
                IsFixingAmbiguity = true,
                MinDistanceOfLongBaseLine = 50 * 1000,
                MaxDistanceOfShortBaseLine = 20000,
                IsOutputEpochResult = true,
                IsEnableEpochParamAnalysis = true,
                SequentialEpochCountOfAccuEval = 20,
                MaxDifferOfAccuEval = 0.01,
                MaxAllowedConvergenceTime = 240,
                KeyLabelCharCount = 6,
                MaxAllowedDifferAfterConvergence = 0.02,
                MaxAllowedRmsOfAccuEval = 0.05
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }
        /// <summary>
        /// 默认的最简伪距定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultSimplePseudoRangePositioningOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.最简伪距定位,
                ObsDataType = SatObsDataType.PseudoRangeA,
                ApproxDataType = SatApproxDataType.ApproxPseudoRangeA,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                AdjustmentType = AdjustmentType.参数平差,
                MaxStdDev = 10000,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 10,
                
                IsEphemerisRequired = true,
                IsOutputEpochResult = true,
                IsErpFileRequired = false,
                IsPreciseClockFileRequired = false,
                IsPreciseEphemerisFileRequired = false,
                IsRemoveSmallPartSat = false,
                IsP1DcbToLcOfGridIonoRequired = true,
                IsIgsIonoFileRequired = true
            };
            return model;
        }

        /// <summary>
        /// 默认的伪距定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultPseudoRangePositioningOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.多系统伪距定位,
                ObsDataType = SatObsDataType.PseudoRangeA,
                ApproxDataType = SatApproxDataType.ApproxPseudoRangeA,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 10,
                
                IsEphemerisRequired = true,
                IsOutputEpochResult = true,
                IsErpFileRequired = true,
                IsPreciseClockFileRequired = true,
                IsPreciseEphemerisFileRequired = true,
                IsRemoveSmallPartSat = false,
                IsP1DcbToLcOfGridIonoRequired = true,
                IsIgsIonoFileRequired = true,
            };
            return model;
        }
        /// <summary>
        /// 参数化对流层伪距定位
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultPseudoRangePositioningWithTropOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.参数化对流层伪距定位,
                ObsDataType = SatObsDataType.PseudoRangeA,
                ApproxDataType = SatApproxDataType.ApproxPseudoRangeA,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                IsIgsIonoFileRequired = true,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 10,
                IsEphemerisRequired = true,
                IsOutputEpochResult = true,
                IsErpFileRequired = true,
                IsPreciseClockFileRequired = true,
                IsPreciseEphemerisFileRequired = true,
                IsRemoveSmallPartSat = false,
                IsP1DcbToLcOfGridIonoRequired = true
            };
            return model;
        }

        /// <summary>
        /// 默认的伪距定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultSingleSatOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.电离层硬件延迟计算,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,

                IsRemoveIonoFreeUnavaliable = false,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 10,
                MinSatCount = 1,
                IsOutputEpochResult = true,
                IsRemoveSmallPartSat = false,
                IsIndicatedPrn = true,
                IndicatedPrn = new SatelliteNumber(1, SatelliteType.G),
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }
        /// <summary>
        /// 默认的伪距定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultSinglePeriodSatOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.电离层延迟变化计算,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                AdjustmentType = AdjustmentType.参数平差,

                IsRemoveIonoFreeUnavaliable = false,
                EnableClockService = true,
                SatelliteTypes = types,
                VertAngleCut = 15,
                MinSatCount = 1,
                IsOutputEpochResult = true,
                IsRemoveSmallPartSat = false,
                IsIndicatedPrn = true,
                IndicatedPrn = new SatelliteNumber(1, SatelliteType.G),
                MultiEpochCount = 10,
                 IsSmoothMoveInMultiEpoches = false,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }

        /// <summary>
        /// 默认的差分定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultDifferPositioningOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.多历元载波单差,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                ObsDataType = SatObsDataType.AlignedIonoFreePhaseRange,
                ApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange,
                EnableClockService = true,
                SatelliteTypes = types,
                IsAliningPhaseWithRange = false,
                
                IsEphemerisRequired = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }

        /// <summary>
        /// 默认的差分定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultNoRelevantDifferPositioningOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.多历元载波无相关单差,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                ObsDataType = SatObsDataType.AlignedIonoFreePhaseRange,
                ApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange,
                EnableClockService = true,
                SatelliteTypes = types,
                IsAliningPhaseWithRange = false,
                
                IsEphemerisRequired = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }

        /// <summary>
        /// 默认的差分定位参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultPeriodDoubleDifferPositioningOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.多历元载波双差,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                IsOutputResidual = true,
                ObsDataType = SatObsDataType.PhaseRangeA,
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                EnableClockService = true,
                SatelliteTypes = types,
                IsAliningPhaseWithRange = false,
                IsReverseCycleSlipeRevise = false,
                MultiEpochCount = 2,
                IsEphemerisRequired = true,
                IsBaseSatelliteRequried = true,
                IsFixingAmbiguity = true,
                MaxFloatRmsNormToFixAmbiguity = 0.1,
                IsEnableEpochParamAnalysis = true,
                SequentialEpochCountOfAccuEval = 20,
                MaxDifferOfAccuEval = 0.01,
                MaxAllowedConvergenceTime = 240,
                KeyLabelCharCount = 6,
                MaxAllowedDifferAfterConvergence = 0.02,
                MaxAllowedRmsOfAccuEval = 0.05
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;


            return model;
        }

        /// <summary>
        /// 默认的格式化参数配置
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultFormatOption(List<SatelliteType> types = null)
        {
            var option = GetDefaultSimpleEpochDoubleDifferPositioningOption(types);
            option.GnssSolverType = Gnsser.GnssSolverType.单历元双频载波双差;
            return option; 
        }
        /// <summary>
        /// 默认的模糊度固定的单历元纯载波双差定位选项
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultAmbiFixedEpochDoubleDifferPositioningOption(List<SatelliteType> types = null)
        {

            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.模糊度固定的单历元纯载波双差,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                ObsDataType = SatObsDataType.PhaseRangeA,
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                EnableClockService = true,
                SatelliteTypes = types,
                
                IsAliningPhaseWithRange = false,
                MultiEpochCount = 1,
                IsEphemerisRequired = true,
                IsBaseSatelliteRequried = true,
                IsIonoCorretionRequired = false,
                IsOutputEpochCoord = true,
                MinFrequenceCount = 2,
                IsEnableEpochParamAnalysis = true,
                IsRealTimeAmbiFixWhenOuterAmbiFileFailed = true
            };
            //model.CycleSlipDetectSwitcher[CycleSlipDetectorType.数值平均法] = true;
            //model.CycleSlipDetectSwitcher[CycleSlipDetectorType.高次差法] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;

            return model;
        }


        /// <summary>
        /// 默认的简单单历元双差定位选项
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultSimpleEpochDoubleDifferPositioningOption(List<SatelliteType> types = null)
        {

            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                IsEnableSatAppearenceService = true,
                IsFixingAmbiguity = true,
                GnssSolverType = Gnsser.GnssSolverType.单历元单频双差,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                ObsDataType = SatObsDataType.PhaseRangeA,
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                EnableClockService = true,
                SatelliteTypes = types,
                IsReverseCycleSlipeRevise=false,
                IsAliningPhaseWithRange = false,
                MultiEpochCount = 1,
                VertAngleCut=15,//按照规范，默认为15度
                IsEphemerisRequired = true,
                IsBaseSatelliteRequried = true,
                IsIonoCorretionRequired = true, //单频启用电离层改正
                IsOutputEpochCoord = true,
                IsOutputResidual = true,
                MinFrequenceCount = 2,
                IsEnableEpochParamAnalysis = true,
                IsRealTimeAmbiFixWhenOuterAmbiFileFailed = true,
                 SequentialEpochCountOfAccuEval = 20,
                 MaxDifferOfAccuEval = 0.01,
                 MaxAllowedConvergenceTime = 240,
                 KeyLabelCharCount = 8,
                 MaxAllowedDifferAfterConvergence = 0.02,
                 MaxAllowedRmsOfAccuEval = 0.05,
                 IsIgsIonoFileRequired=true, 
            };
            //model.CycleSlipDetectSwitcher[CycleSlipDetectorType.数值平均法] = true;
            //model.CycleSlipDetectSwitcher[CycleSlipDetectorType.高次差法] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;

            return model;
        }
        /// <summary>
        /// 默认的单历元双差定位选项
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultEpochDoubleDifferPositioningOption(List<SatelliteType> types = null)
        {
            var option = GetDefaultSimpleEpochDoubleDifferPositioningOption(types);
            option.GnssSolverType = Gnsser.GnssSolverType.单历元载波双差;
            return option;
        }
        /// <summary>
        /// 默认的单历元双频伪距载波双差定位选项
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultEpochDueFreqDifferPositioningOption(List<SatelliteType> types = null)
        {
            var option = GetDefaultSimpleEpochDoubleDifferPositioningOption(types);
            option.GnssSolverType = Gnsser.GnssSolverType.单历元双频双差;
            return option;             
        }

        /// <summary>
        /// 默认的单历元双差定位选项
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultEpochDoubleDueDifferPositioningOption(List<SatelliteType> types = null)
        {

            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.单历元双频载波双差,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                ObsDataType = SatObsDataType.PhaseRangeA,
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                EnableClockService = true,
                SatelliteTypes = types,
                
                IsAliningPhaseWithRange = false,
                MultiEpochCount = 1,
                IsEphemerisRequired = true,
                IsBaseSatelliteRequried = true,
                IsEnableEpochParamAnalysis = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }
        /// <summary>
        /// 默认的多站单历元定位选项
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultMultiSiteEpochOption(List<SatelliteType> types = null)
        {

            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.多站单历元扩展计算,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                ObsDataType = SatObsDataType.PhaseRangeA,
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                EnableClockService = true,
                SatelliteTypes = types,
                
                IsAliningPhaseWithRange = false,
                MultiEpochCount = 1,
                IsEphemerisRequired = true,
                IsBaseSatelliteRequried = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }
        /// <summary>
        /// 默认的多站多历元定位选项
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultPeriodMultiSiteOption(List<SatelliteType> types = null)
        {

            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.多站多历元扩展计算,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                ObsDataType = SatObsDataType.PhaseRangeA,
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                EnableClockService = true,
                SatelliteTypes = types,
                
                IsAliningPhaseWithRange = false,
                MultiEpochCount = 2,
                IsEphemerisRequired = true,
                IsBaseSatelliteRequried = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }
        /// <summary>
        /// 非差非组合PPP
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultUncombinedPppOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.非差非组合PPP,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                
                EnableClockService = true,
                SatelliteTypes = types,
                MinSatCount = 4,
                VertAngleCut = 10,
                MinFrequenceCount = 2,
                IsAliningPhaseWithRange = false,
                ObsDataType = SatObsDataType.PhaseRangeA,//有待验证
                IsEphemerisRequired = true,
                IsIonoCorretionRequired = false,//非差非组合不需要电离层改正 
                IsOutputIono = true,
                IsP1DcbToLcOfGridIonoRequired = true,
                IsEnableEpochParamAnalysis = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.MW组合] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.LI组合] = true;
            return model;
        }

        /// <summary>
        /// 专用于启用星历服务
        /// </summary>
        /// <returns></returns>
        public static GnssProcessOption GetEphemerisSourceOption()
        {
            GnssProcessOption model = new GnssProcessOption()
            {
                IsObsDataRequired = false,
                IsEphemerisRequired = true,

                IsPreciseEphemerisFileRequired = false,
                IsErpFileRequired = false,
                IsPreciseClockFileRequired = false,
                IsAntennaFileRequired = false,
                IsSatStateFileRequired = false,
                IsSatInfoFileRequired = false,
                IsOceanLoadingFileRequired = false,
                IsDCBFileRequired = false,
                IsVMF1FileRequired = false,
            };
            return model;
        }


        #region 单频定位
        /// <summary>
        /// 非差非组合PPP
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static GnssProcessOption GetDefaultSingleFreqPppOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.单频PPP,
                AdjustmentType = Geo.Algorithm.AdjustmentType.均方根滤波,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = 100000,
                EnableClockService = true,
                SatelliteTypes = types, 

                IsRemoveIonoFreeUnavaliable = false,
                VertAngleCut = 10,
                MinFrequenceCount = 1,
                IsAliningPhaseWithRange = false,
                ObsDataType = SatObsDataType.PhaseRangeA,//有待验证
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                IsEphemerisRequired = true,
                IsDcbCorrectionRequired = true,//由于采用的IGS钟差是双频计算的结果，因此需要将其转换为单频结果
                //IsDcbOfP1P2Enabled  =true, //
                IsIonoCorretionRequired = false,
                IsDCBFileRequired = true,
                IsP1DcbToLcOfGridIonoRequired =true,
                IsOutputIono = true,
                IsNeedPseudorangePositionWhenProcess = true,//单频秩亏，需要比较精确的初始值
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.数值平均法] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.高次差法] = true;
            return model;
        }
        /// <summary>
        /// 海天一体无电离层组合PPP
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultSingleSiteOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.单站单历元扩展计算,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                EnableClockService = true,
                SatelliteTypes = types,

                IsRemoveIonoFreeUnavaliable = false,
                VertAngleCut = 10,
                MinFrequenceCount = 2,
                IsAliningPhaseWithRange = false,
                ObsDataType = SatObsDataType.IonoFreePhaseRange,
                IsEphemerisRequired = true, 
                IsOutputEpochSatInfo = false,
                IsOutputCycleSlipFile = false,
                IsOutputEpochResult = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.数值平均法] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.高次差法] = true;
            return model;
        }
        #endregion

        /// <summary>
        /// 电离层建模单频PPP
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal static GnssProcessOption GetDefaultIonoModeledSingleFreqPppOption(List<SatelliteType> types = null)
        {
            if (types == null) types = new List<SatelliteType>() { SatelliteType.G };
            GnssProcessOption model = new GnssProcessOption()
            {
                GnssSolverType = Gnsser.GnssSolverType.单频PPP,
                AdjustmentType = Geo.Algorithm.AdjustmentType.均方根滤波,
                FilterCourceError = true,
                CaculateType = Gnsser.CaculateType.Filter,
                MaxStdDev = 100000,
                EnableClockService = true,
                SatelliteTypes = types,

                IsRemoveIonoFreeUnavaliable = false,
                VertAngleCut = 10,
                MinFrequenceCount = 1,
                IsAliningPhaseWithRange = false,
                ObsDataType = SatObsDataType.PhaseRangeA,//有待验证
                ApproxDataType = SatApproxDataType.ApproxPhaseRangeA,
                IsEphemerisRequired = true,
                IsDcbCorrectionRequired = false,
                IsIonoCorretionRequired = true,
                IsIgsIonoFileRequired = true,
            };
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.数值平均法] = true;
            model.CycleSlipDetectSwitcher[CycleSlipDetectorType.高次差法] = true;

            return model;
        }
        #endregion
        #endregion
    }
}
