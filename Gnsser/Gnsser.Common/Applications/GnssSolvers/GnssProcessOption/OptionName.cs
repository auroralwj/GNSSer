//2018.03.20, czs, create in hmx, 选项与文件
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
    /// 选项名称
    /// </summary>
    public enum OptionName
    {
        /// <summary>
        ///版本
        /// </summary>
        Version,
        /// <summary>
        /// 作者
        /// </summary>
        Author,
        /// <summary>
        /// 创建时间
        /// </summary>
        CreationTime,
        #region 基本属性
        /// <summary>
        /// 名称
        /// </summary>
        Name,
        /// <summary>
        /// 正反算， 顺序-逆序计算. 0表示只按照默认配置，单向计算一次。
        /// </summary>
        OrdinalAndReverseCount,

        /// <summary>
        /// 是否逆序数据流
        /// </summary>
        IsReversedDataSource,
        /// <summary>
        /// 定位类型
        /// </summary>
        PositionType,
        /// <summary>
        /// 数据处理类型，是否为预处理等
        /// </summary>
        ProcessType,
        /// <summary>
        /// 平差类型选项
        /// </summary>
        AdjustmentType,
        /// <summary>
        /// 观测文件格式化处理类型
        /// </summary>
        RinexObsFileFormatType,
        /// <summary>
        /// RINEX 输出类型
        /// </summary>
        OutputRinexVersion,

        #region 循环控制
        /// <summary>
        /// GNSS 解算器类型
        /// </summary>
        GnssSolverType,
        /// <summary>
        /// 计算起始历元编号。
        /// </summary>
        StartIndex,
        /// <summary>
        /// 计算起始历元编号。
        /// </summary>
        CaculateCount,
        #endregion

        #region 测站初始值
        /// <summary>
        /// 是否需要测站初始值,如果需要，而测站值为空，则将自动进行计算设置。
        /// </summary>
        IsApproxXyzRequired,
        /// <summary>
        /// 坐标初始中误差.用于初始赋权.默认为100米。
        /// </summary>
        InitApproxXyzRms,
        /// <summary>
        /// 坐标初始.用于初始赋权
        /// </summary>
        InitApproxXyz,
        /// <summary>
        /// 是否指定初始坐标
        /// </summary>
        IsIndicatingApproxXyz,
        /// <summary>
        /// 是否指定近似坐标
        /// </summary>
        IsIndicatingApproxXyzRms,
        /// <summary>
        /// 是否更新测站信息
        /// </summary>
        IsUpdateStationInfo,
        #endregion

        #region 数据源
        /// <summary>
        /// 载波相位是否是以米为单位，如Android接收机单位为米。
        /// </summary>
        IsLengthPhaseValue,
        /// <summary>
        /// 是否需要观测数据源
        /// </summary>
        IsObsDataRequired,
        /// <summary>
        /// 是否指定钟差
        /// </summary>
        IsIndicatingClockFile,

        /// <summary>
        /// P2C2 启动
        /// </summary>
        IsP2C2Enabled,

        /// <summary>
        /// 电离层文件路径
        /// </summary>
        IonoGridFilePath,
        /// <summary>
        /// 是否指定电离层文件
        /// </summary>
        IsIndicatingGridIonoFile,
        /// <summary>
        /// 历元电离层参数文件路径
        /// </summary>
        EpochIonoParamFilePath,
        /// <summary>
        /// 是否指定星历
        /// </summary>
        IsIndicatingEphemerisFile,
        /// <summary>
        /// 是否指定坐标文件
        /// </summary>
        IsIndicatingCoordFile,
        /// <summary>
        /// 指定的钟差路径
        /// </summary>
        ClockFilePath,
        /// <summary>
        /// 指定的星历路径
        /// </summary>
        EphemerisFilePath,
        /// <summary>
        /// 导航文件路径，用于提取电离层改正
        /// </summary>
        NavIonoModelPath,
        /// <summary>
        /// 坐标文件路径
        /// </summary>
        CoordFilePath,
        /// <summary>
        /// 对流层增强文件。
        /// </summary>
        TropAugmentFilePath,

        /// <summary>
        /// 测站信息文件路径
        /// </summary>
        StationInfoPath,
        /// <summary>
        /// 是否需要精密钟差文件
        /// </summary>
        IsPreciseClockFileRequired,
        /// <summary>
        /// 是否需要星历， 默认需要
        /// </summary>
        IsEphemerisRequired,
        /// <summary>
        /// 是否需要精密星历文件
        /// </summary>
        IsPreciseEphemerisFileRequired,
        /// <summary>
        /// 是否需要天线文件
        /// </summary>
        IsAntennaFileRequired,
        /// <summary>
        /// 是否需要卫星状态文件
        /// </summary>
        IsSatStateFileRequired,
        /// <summary>
        /// 是否需要卫星信息文件
        /// </summary>
        IsSatInfoFileRequired,
        /// <summary>
        /// 是否需要潮汐文件
        /// </summary>
        IsOceanLoadingFileRequired,
        /// <summary>
        /// 是否需要DCB文件
        /// </summary>
        IsDCBFileRequired,
        /// <summary>
        /// 是否需要VMF1文件
        /// </summary>
        IsVMF1FileRequired,
        /// <summary>
        /// 是否采用GPT2通用文件改正
        /// </summary>
        Isgpt2FileRequired,
        /// <summary>
        /// 是否采用ERP文件改正
        /// </summary>
        IsErpFileRequired,
        #endregion

        #region 数据源选择


        /// <summary>
        /// 是否需要IGS电离层格网文件
        /// </summary>
        IsIgsIonoFileRequired,

        #endregion

        /// <summary>
        /// 是否在计算结束时打开平差报告
        /// </summary>
        IsOpenReportWhenCompleted,

        /// <summary>
        /// 是否启用电离层模型改正。顶层接口，如果要采用电离层改正观测近似值，则必须设定。
        /// </summary>
        IsIonoCorretionRequired,
        /// <summary>
        /// 是否需要伪距值
        /// </summary>
        IsRangeValueRequired,
        /// <summary>
        /// 是否需要相位值
        /// </summary>
        IsPhaseValueRequired,
        /// <summary>
        /// 定位过程中，是否更新测站估值坐标。
        /// </summary>
        IsUpdateEstimatePostition,
        /// <summary>
        /// 是否移除未通过检核的卫星，否则标记为未启用。
        /// </summary>
        IsRemoveOrDisableNotPassedSat,

        /// <summary>
        /// 是否移除观测段太小的卫星
        /// </summary>
        IsRemoveSmallPartSat,

        /// <summary>
        /// 是否移除故障卫星(通常从外部文件指定)
        /// </summary>
        IsExcludeMalfunctioningSat,
        /// <summary>
        /// 是否禁用太阳阴影影响的卫星
        /// </summary>
        IsDisableEclipsedSat,
        #region 周跳处理
        /// <summary>
        /// 启用缓存周跳探测
        /// </summary>
        IsEnableBufferCs,
        /// <summary>
        /// 是否启用实时周跳探测
        /// </summary>
        IsEnableRealTimeCs,
        #region 基于缓存的周跳
        /// <summary>
        /// 缓存周跳差分次数
        /// </summary>
        MaxErrorTimesOfBufferCs,
        /// <summary>
        /// 缓存周跳差分次数
        /// </summary>
        DifferTimesOfBufferCs,
        /// <summary>
        /// 缓存周跳拟合阶次
        /// </summary>
        PolyFitOrderOfBufferCs,

        /// <summary>
        /// 是否忽略已经标记为周跳的历元卫星。
        /// </summary>
        IgnoreCsedOfBufferCs,
        #endregion
        /// <summary>
        /// 缓存周跳 最小窗口大小，小于此，都认为有周跳。
        /// </summary>
        MinWindowSizeOfCs,

        /// <summary>
        /// 是否采用数据源信息标记的周跳，若已标记周跳，则认为有。
        /// </summary>
        IsUsingRecordedCycleSlipInfo,

        /// <summary>
        /// MW周跳探测中，最大的误差
        /// </summary>
        MaxDifferValueOfMwCs,
        /// <summary>
        /// 多项式拟合周跳探测中，最大的误差倍数
        /// </summary>
        MaxRmsTimesOfLsPolyCs,
        /// <summary>
        /// 高次差周跳探测中，允许的最大的误差
        /// </summary>
        MaxValueDifferOfHigherDifferCs,
        /// <summary>
        /// 周跳探测允许最大断裂的时间间隔
        /// </summary>
        MaxBreakingEpochCount,

        /// <summary>
        /// 是否进行周跳探测
        /// </summary>
        IsCycleSlipDetectionRequired,
        /// <summary>
        /// 是否修复周跳
        /// </summary>
        IsCycleSlipReparationRequired,
        /// <summary>
        /// 是否输出周跳文件
        /// </summary>
        IsOutputCycleSlipFile,
        #endregion
        /// <summary>
        /// 是否将初始相位采用伪距对齐
        /// </summary>
        IsAliningPhaseWithRange,
        /// <summary>
        /// 是否需要多普勒频率
        /// </summary>
        IsDopplerShiftRequired,
        /// <summary>
        /// 多历元处理中，是否需要相同的卫星
        /// </summary>
        IsRequireSameSats,
        /// <summary>
        /// 多测站中，是否允许某历元丢失个别站。
        /// </summary>
        IsAllowMissingEpochSite,

        /// <summary>
        /// 允许最小的伪距
        /// </summary>
        MinAllowedRange,// = 15000000.0;
                        /// <summary>
                        /// 允许最大的伪距
                        /// </summary>
        MaxAllowedRange,//= 40000000.0; //35770602.136176817，同步卫星
        #region 输出
        /// <summary>
        /// 结果输出目录
        /// </summary>
        OutputDirectory,


        /// <summary>
        /// 是否输出SINEX文件
        /// </summary>
        IsOutputSinex,
        /// <summary>
        /// 是否输出汇总文件
        /// </summary>
        IsOutputSummery,
        /// <summary>
        /// 输出结果缓存大小
        /// </summary>
        OutputBufferCount,
        #endregion
        /// <summary>
        /// 缓存数量
        /// </summary>
        BufferSize,
        /// <summary>
        /// 最大的时间跨度，单位：秒，如果历元之间超过了这个时段，则清空以往数据，重新构建对象。
        /// </summary>
        MaxEpochSpan,

        /// <summary>
        /// 卫星连续观测的最小历元数量(单位：历元次)。即如果小于这个间隔，则抹去，不参与计算，以免影响精度。
        /// 一般为20个历元。
        /// </summary>
        MinContinuouObsCount,
        /// <summary>
        /// 最小卫星数量
        /// </summary>
        MinSatCount,
        /// <summary>
        /// 至少的观测频率数量
        /// </summary>
        MinFrequenceCount,

        #region 通用
        /// <summary>
        /// 伪距类型
        /// </summary>
        RangeType,
        /// <summary>
        /// 用于计算的观测值变量类型,此设置用于周跳探测，近似观测值获取等。
        /// </summary>
        ObsDataType,
        /// <summary>
        /// 平滑伪距的相位类型
        /// </summary>
        PhaseTypeToSmoothRange,
        /// <summary>
        /// 近似值的数据类型，用于计算残差
        /// </summary>
        ApproxDataType,
        /// <summary>
        /// 参与计算的卫星类型，系统类型。
        /// </summary>
        SatelliteTypes,
        #endregion

        /// <summary>
        ///  计算方式
        /// </summary>
        CaculateType,
        /// <summary>
        /// 是否剔除粗差
        /// </summary>
        RejectGrossError,
        /// <summary>
        /// 是否启用单独的钟差服务（文件）
        /// </summary>
        EnableClockService,

        /// <summary>
        /// 最大均方差，阈值。
        /// </summary>
        MaxStdDev,


        /// <summary>
        /// precise orbit?
        /// </summary>
        IsPreciseOrbit,
        /// <summary>
        ///  vert angle cutoff (deg)
        /// </summary>
        VertAngleCut,
        /// <summary>
        /// 是否过滤粗差
        /// </summary>
        FilterCourceError,

        /// <summary>
        /// 最大迭代次数。
        /// </summary>
        MaxLoopCount,
        /// <summary>
        /// 是否启用迭代
        /// </summary>
        EnableLoop,
        /// <summary>
        /// 最大平均均方根倍数。
        /// </summary>
        MaxMeanStdTimes,
        /// <summary>
        /// 启用逆序周跳探测
        /// </summary>
        IsReverseCycleSlipeRevise,
        #endregion

        #region 随机模型参数
        /// <summary>
        /// 卫星相位与伪距观测量的权比。
        /// </summary>
        PhaseCovaProportionToRange,


        /// <summary>
        /// 随机模型参数， 随机游走模型的标准差。
        /// </summary>
        StdDevOfSysTimeRandomWalkModel,
        /// <summary>
        /// 随机模型参数， 随机游走模型的标准差。
        /// </summary>
        StdDevOfRandomWalkModel,
        /// <summary>
        /// 随机模型参数， 载波相位模型的标准差。
        /// </summary>
        StdDevOfPhaseModel,
        /// <summary>
        /// 随机模型参数， 发生周跳时，载波相位模型的标准差。
        /// </summary>
        StdDevOfCycledPhaseModel,
        /// <summary>
        /// 随机模型参数， 电离层随机游走模型的标准差。
        /// </summary>
        StdDevOfIonoRandomWalkModel,
        /// <summary>
        /// 随机模型参数， 静态模型的标准差。
        /// </summary>
        StdDevOfStaticTransferModel,
        /// <summary>
        /// 随机模型参数， 对流层随机游走模型的标准差。
        /// </summary>
        StdDevOfTropoRandomWalkModel,
        /// <summary>
        /// 随机模型参数，白噪声模型的标准差。
        /// </summary>
        StdDevOfRevClockWhiteNoiseModel,

        #endregion

        /// <summary>
        /// 是否需要用坐标服务设置测站初值
        /// </summary>
        IsSetApproxXyzWithCoordService,

        #region 来自于差分的设置

        /// <summary>
        /// 参与差分卫星的数量
        /// </summary>
        MutliEpochSameSatCount,

        /// <summary>
        /// 系数阵历元数量
        /// </summary>
        MultiEpochCount,
        #endregion

        #region 文件 输入输出
        /// <summary>
        /// 是否输出结果的总开关，只有此为true才会判断下面的输出
        /// </summary>
        IsOutputResult,
        /// <summary>
        /// 是否输出卫星信息
        /// </summary>
        IsOutputEpochSatInfo,
        /// <summary>
        /// 是否输出平差文件
        /// </summary>
        IsOutputAdjust,
        /// <summary>
        /// 是否输出平差矩阵文本文件，主要用于平差测试
        /// </summary>
        IsOutputAdjustMatrix,
        /// <summary>
        /// 是否输出电离层产品文件
        /// </summary>
        IsOutputIono,
        /// <summary>
        /// 是否输出对流层湿延迟产品文件
        /// </summary>
        IsOutputWetTrop,
        /// <summary>
        /// 是否输出逐个历元计算结果
        /// </summary>
        IsOutputEpochResult,
        #endregion

        #region 星历计算选项
        /// <summary>
        /// 用于拟合的最小连续星历数量。
        /// </summary>
        MinSuccesiveEphemerisCount,

        /// <summary>
        /// 在获取星历失败后，是否切换星历数据源
        /// </summary>
        IsSwitchWhenEphemerisNull,
        #endregion


        #region 基线选项
        /// <summary>
        /// 长基线的最小长度,单位米
        /// </summary>
        MinDistanceOfLongBaseLine,
        /// <summary>
        /// 短基线的最大长度，单位米
        /// </summary>
        MaxDistanceOfShortBaseLine,
        #endregion
        /// <summary>
        /// 周跳开关.优先考虑周跳探测器开关,如为空，然后考虑默认周跳探测器。
        /// </summary>
        CycleSlipDetectSwitcher,
        #region 结果检核

        /// <summary>
        /// 是否启用验后残差检核
        /// </summary>
        IsResidualCheckEnabled,
        /// <summary>
        /// 当结果变化时，是否进行手动升高状态转移矩阵的噪声
        /// </summary>
        IsPromoteTransWhenResultValueBreak,
        #endregion


        #region 改正相关配置
        /// <summary>
        ///是否需要观测值改正
        /// </summary>
        IsObsCorrectionRequired,
        /// <summary>
        /// 是否需要近似模型改正
        /// </summary>
        IsApproxModelCorrectionRequired,
        /// <summary>
        /// 是否需要DCB改正
        /// </summary>
        IsDcbCorrectionRequired,
        /// <summary>
        /// 接收机天线PCO改正
        /// </summary>
        IsReceiverAntSiteBiasCorrectionRequired,
        /// <summary>
        /// 海洋潮汐改正
        /// </summary>
        IsOceanTideCorrectionRequired,
        /// <summary>
        /// 固体潮改正
        /// </summary>
        IsSolidTideCorrectionRequired,
        /// <summary>
        /// 极潮改正
        /// </summary>
        IsPoleTideCorrectionRequired,
        /// <summary>
        /// 卫星钟差改正
        /// </summary>
        IsSatClockBiasCorrectionRequired,
        /// <summary>
        /// 对流层改正
        /// </summary>
        IsTropCorrectionRequired,
        /// <summary>
        /// 重力延迟改正
        /// </summary>
        IsGravitationalDelayCorrectionRequired,
        /// <summary>
        /// 卫星天线相位中心改正
        /// </summary>
        IsSatAntPcoCorrectionRequired,
        /// <summary>
        /// 接收机天线PCO改正
        /// </summary>
        IsRecAntPcoCorrectionRequired,
        /// <summary>
        /// 接收机天线PCV改正
        /// </summary>
        IsRecAntPcvCorrectionRequired,
        /// <summary>
        /// 相位缠绕改正
        /// </summary>
        IsPhaseWindUpCorrectionRequired,
        /// <summary>
        /// 测站改正
        /// </summary>
        IsSiteCorrectionsRequired,
        /// <summary>
        /// 伪距改正
        /// </summary>
        IsRangeCorrectionsRequired,
        /// <summary>
        /// 频率改正
        /// </summary>
        IsFrequencyCorrectionsRequired,
        /// <summary>
        /// 是否需要GNSSer历元电离层文件
        /// </summary>
        IsEpochIonoFileRequired,
        /// <summary>
        /// 是否需要电离层导航参数模型
        /// </summary>
        IsNavIonoModelCorrectionRequired,
        #endregion

        /// <summary>
        /// 是否对流层增强启用。
        /// </summary>
        IsTropAugmentEnabled,

        /// <summary>
        /// Isgpt2File1DegreeRequired
        /// </summary>
        Isgpt2File1DegreeRequired,
        /// <summary>
        /// 是否要求相同卫星
        /// </summary>
        IsSameSatRequired,
        /// <summary>
        /// 是否选择基准卫星
        /// </summary>
        IsBaseSatelliteRequried,
        /// <summary>
        /// 是否固定模糊度
        /// </summary>
        IsFixingAmbiguity,
        /// <summary>
        /// 是否固定坐标
        /// </summary>
        IsFixingCoord,


        /// <summary>
        /// 是否需要坐标服务
        /// </summary>
        IsSiteCoordServiceRequired,

        /// <summary>
        ///是否指定测站信息文件
        /// </summary>
        IsIndicatingStationInfoFile,
        /// <summary>
        /// 测站信息文件，主要包含天线时段信息。
        /// </summary>

        IsStationInfoRequired,

        /// <summary>
        /// 是否启用先验值
        /// </summary>
        IsEnableInitApriori,
        /// <summary>
        /// 初始先验值
        /// </summary>
        InitApriori,
        /// <summary>
        /// 是否启用NGA星历匹配，作为实时计算的备份。
        /// </summary>
        IsEnableNgaEphemerisSource,
        /// <summary>
        /// 是否使用唯一数据源，当自动匹配时使用
        /// </summary>
        IsUniqueEphSource,
        /// <summary>
        /// 指定的IGS数据源，前两个字作为代码
        /// </summary>
        IndicatedSourceCode,
        /// <summary>
        /// 观测数据源
        /// </summary>
        ObsFiles,
        /// <summary>
        /// 指定的 BDS 星历路径
        /// </summary>
        BdsEphemerisFilePath,
        /// <summary>
        /// 指定的 GLONASS 星历路径
        /// </summary>
        GloEphemerisFilePath,
        /// <summary>
        /// 指定的Galileo星历路径
        /// </summary>
        GalEphemerisFilePath,
        /// <summary>
        /// 是否指定BDS星历数据源，具有最高优先权
        /// </summary>
        IsIndicatingBdsEphemerisFile,
        /// <summary>
        /// 是否指定GLONASS星历数据源，具有最高优先权
        /// </summary>
        IsIndicatingGloEphemerisFile,
        /// <summary>
        /// 是否指定伽利略星历数据源，具有最高优先权
        /// </summary>
        IsIndicatingGalEphemerisFile,
        /// <summary>
        ///是否全部使用一个星历数据源，将采用 EphemerisFilePath 
        /// </summary>
        IsUseUniqueEphemerisFile,
        /// <summary>
        /// 是否指定ERP路径，具有最高优先权
        /// </summary>
        IsIndicatingErpFile,
        /// <summary>
        /// 指定的ERP路径，具有最高优先权
        /// </summary>
        ErpFilePath,
        /// <summary>
        /// 是否启用全钟差服务,否则使用简单钟差服务
        /// </summary>
        IsUsingFullClockService,
        IsP1DcbToLcRequired,
        IsDcbOfP1P2Enabled,
        IsIndicatedPrn,
        IndicatedPrn,
        WindowSizeOfPhaseSmoothRange,
        IsUserGridOrHarmoIonoFile,
        /// <summary>
        /// 数据处理时，是否需要伪距定位，动态定位时需要
        /// </summary>
        IsNeedPseudorangePositionWhenProcess,
        /// <summary>
        /// 是否平滑移动多历元窗口，否则采用分段移动
        /// </summary>
        IsSmoothMoveInMultiEpoches,
        IonoDifferCorrectionType,
        OrderOfDeltaIonoPolyFit,
        IsUseGNSSerSmoothRangeMethod,
        IsOutputEpochCoord,
        IsOutputEpochDop,
        IsOutputObservation,
        OutputMinInterval,
        IsWeightedPhaseSmoothRange,
        IonoFitEpochCount,
        StdDevOfWhiteNoiseOfDynamicPosition,
        IsClearOutBufferWhenReversing,
        IsPhaseInMetterOrCycle,
        MinFixedAmbiRatio,
        MaxFloatRmsNormToFixAmbiguity,
        MaxRatioOfLambda,
        IsSatAntPvcCorrectionRequired,
        IsSmoothRange,
        IonoDeltaFilePath,
        IsEstDcbOfRceiver,
        IsDetectClockJump,
        IsClockJumpReparationRequired,
        IsSmoothRangeWhenPrevPseudorangePosition,
        IsOutputInGnsserFormat,
        OuterClockJumpFile,
        IsOpenClockJumpSwitcher,
        IsOutputJumpClockFile,
        GnsserFcbFilePath,
        IsGnsserFcbOfDcbRequired,
        IsUsingAmbiguityFile,
        AmbiguityFilePath,
        MaxAmbiDifferOfIntAndFloat,
        IsSameTimeSystemInMultiGnss,
        /// <summary>
        /// 不同系统的权值
        /// </summary>
        SystemStdDevFactorString,
        SatelliteCovasString,
        MaxErrorTimesOfPostResdual,
        ExtraStreamLoopCount,
        IsFixParamByConditionOrHugeWeight,
        IsResultCheckEnabled,
        StdDevOfSatClockWhiteNoiseModel,
        IsBaseSiteRequried,
        IsIndicateBaseSite,
        IsEstimateTropWetZpd,
        ExemptedStdDev,
        IsEnableEpochParamAnalsis,
        AnalysisParamNamesString,
        IsOutputResidual,
    }
     
     
}
