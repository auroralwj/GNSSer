using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Security;
using Geo.Utils;

namespace Gnsser.Winform
{
    public partial class HttpOptRequesterForm : LogListenerForm
    {
        public HttpOptRequesterForm()
        {
            InitializeComponent();
        }

        protected override void ShowInfo(string info)
        {
            string time = Geo.Utils.DateTimeUtil.GetTimeStringWithMiniSecondNow();
            Geo.Utils.FormUtil.InsertLineToTextBox(richTextBoxControl1,time + "\t" + info);         
        }
         

        private void FtpDownloaderForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_opt.FilePath = @"Data\GNSS\Options\无电离层组合PPP.opt";
        }

        private void button_visit_Click(object sender, EventArgs e)
        {
            try
            {
                var url = this.textBox_url.Text;
                var gnssOptionText = this.richTextBoxControl_postParams.Text;

                if(String.IsNullOrWhiteSpace( gnssOptionText))
                {
                    MessageBox.Show("请输入配置文件再试！");
                    return;
                }

                try
                {
                    OptionManager manager = new OptionManager();
                    var opt =  manager.Read(gnssOptionText, "nameOfOpt");
                    if(opt.ObsFiles == null || opt.ObsFiles.Count ==0)
                    {
                        MessageBox.Show("必须指定观测文件网址！O文件，D文件或O.Z都可以。");
                        return;
                    }
                    foreach (var item in opt.ObsFiles)
                    {
                        if (item.Contains(@":\"))
                        {
                            MessageBox.Show("请采用Internet的 FTP 或 网址，不要采用本地地址！O文件，D文件或O.Z都可以。\n" + item);
                            return;
                        }
                    }

                }catch(Exception ex)
                {
                    MessageBox.Show("配置文件不合法，请仔细检查后再试！" + ex.Message);
                    return;
                }

                ShowInfo("即将发出计算服务请求，请耐心等待。。。。");

                Encoding encoding = Encoding.GetEncoding("utf-8");
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("gnssOptionText", gnssOptionText);
                HttpWebResponse response = HttpWebRequestUtil.CreatePostHttpResponse(url, parameters, null, null, encoding, null);
                string cookieString = response.Headers["Set-Cookie"];

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                string srcString = reader.ReadToEnd();
                //返回值赋值
                reader.Close();

                ShowInfo("计算完毕，返回结果。");

                ShowInfo(srcString);

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button_read_Click(object sender, EventArgs e)
        {
            var path = fileOpenControl_opt.FilePath;

            if (String.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("请输入配置文件路径后再试！");
                return;
            }

            if (!File.Exists(path))
            {
                MessageBox.Show("请输入配置文件路径后再试！");
                return;
            }
            this.richTextBoxControl_postParams.Text = File.ReadAllText(path);
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            this.richTextBoxControl_postParams.Text = "# [Adjustment]\n"
+"PhaseCovaProportionToRange = 0.0001	# 卫星相位与伪距观测量的权比\n"
+"StdDevOfWhiteNoiseModel = 300000	#  随机模型参数，白噪声模型的标准差。\n"
+"StdDevOfTropoRandomWalkModel = 0.0004	# 随机模型参数， 对流层随机游走模型的标准差。\n"
+"StdDevOfStaticTransferModel = 1E-10	# 随机模型参数， 静态模型的标准差。\n"
+"StdDevOfIonoRandomWalkModel = 0.13	#  随机模型参数， 电离层随机游走模型的标准差。\n"
+"StdDevOfCycledPhaseModel = 20000000	#  随机模型参数， 发生周跳时，载波相位模型的标准差。\n"
+"StdDevOfPhaseModel = 1E-05	#  随机模型参数， 载波相位模型的标准差。\n"
+"StdDevOfRandomWalkModel = 0.0175	# 随机模型参数， 随机游走模型的标准差。\n"
+"AdjustmentType = 卡尔曼滤波	# 平差类型选项\n"
+"StdDevOfSysTimeRandomWalkModel = 0.0004	# 随机模型参数， 随机游走模型的标准差。\n"
+"\n"
+"# [Basic]\n"
+"Version = 1	# 自动生成\n"
+"Author = GNSSer, www.gnsser.com	# 自动生成\n"
+"CreationTime = 2018 - 03 - 22 11:56:06	# 自动生成\n"
+"Name = 未命名	# Name of this Option\n"
+"\n"
+"# [Calculation]\n"
+"CaculateType = Filter	# 计算方式\n"
+"MinSuccesiveEphemerisCount = 6	# 用于拟合的最小连续星历数量。\n"
+"MultiEpochCount = 3	# 系数阵历元数量\n"
+"MutliEpochSameSatCount = 5	# 参与差分卫星的数量\n"
+"IsRangeValueRequired = True	# 是否需要伪距值\n"
+"IsPhaseValueRequired = True	# 是否需要相位值\n"
+"IsSetApproxXyzWithCoordService = False	# 是否需要用坐标服务设置测站初值\n"
+"IsRemoveOrDisableNotPassedSat = True	# 是否移除未通过检核的卫星，否则标记为未启用。\n"
+"IsRemoveSmallPartSat = True	# 是否移除观测段太小的卫星\n"
+"IsExcludeMalfunctioningSat = True	# 是否移除故障卫星(通常从外部文件指定)\n"
+"IsDisableEclipsedSat = True	#  是否禁用太阳阴影影响的卫星\n"
+"MinContinuouObsCount = 10	#  卫星连续观测的最小历元数量(单位：历元次)。即如果小于这个间隔，则抹去，不参与计算，以免影响精度。\n"
+"MinSatCount = 4	# 最小卫星数量\n"
+"MinFrequenceCount = 2	# 至少的观测频率数量\n"
+"ObsPhaseDataType = IonoFreePhaseRange	# 用于载波计算的观测值变量类型,此设置用于周跳探测，近似观测值获取等。\n"
+"PhaseTypeToSmoothRange = IonoFreePhaseRange	# 平滑伪距的相位类型\n"
+"MinDistanceOfLongBaseLine = 50000	# 长基线的最小长度,单位米\n"
+"MaxMeanStdTimes = 100	# 最大平均均方根倍数\n"
+"EnableLoop = False	# 是否启用迭代\n"
+"MaxLoopCount = 5	# 最大迭代次数\n"
+"FilterCourceError = True	# 是否过滤粗差\n"
+"VertAngleCut = 5	# 高度截止角\n"
+"IsPreciseOrbit = False	# 是否是精密轨道\n"
+"MaxStdDev = 100000	#  最大均方差，阈值。\n"
+"RejectGrossError = False	# 是否剔除粗差\n"
+"SatelliteTypes = G	# 参与计算的卫星类型，系统类型。\n"
+"MaxDistanceOfShortBaseLine = 1000	# 短基线的最大长度，单位米\n"
+"BufferSize = 50	# 缓存数量\n"
+"IsTropAugmentEnabled = False	# 是否对流层增强启用\n"
+"OrdinalAndReverseCount = 0	# 正反算， 顺序-逆序计算. 0表示只按照默认配置，单向计算一次。\n"
+"IsReversedDataSource = False	# 是否逆序数据流正反算\n"
+"PositionType = 静态定位	# 静态定位还是动态定位\n"
+"ProcessType = 仅计算	# 数据处理类型，是否为预处理等\n"
+"GnssSolverType = 无电离层组合PPP	# GNSS 解算器类型\n"
+"StartIndex = 0	# 计算起始历元编号。\n"
+"CaculateCount = 10000000	# 计算数量，从起始编号开始计算。\n"
+"IsApproxXyzRequired = True	# 是否需要测站初始值,如果需要，而测站值为空，则将自动进行计算设置\n"
+"InitApproxXyzRms = 100.00000, 100.00000, 100.00000	# 坐标初始中误差.用于初始赋权.默认为100米。\n"
+"InitApproxXyz = 0,      0,      0	# 坐标初始.用于初始赋权\n"
+"IsSameSatRequired = True	# 是否要求相同卫星\n"
+"Interval = 30	# 采样间隔，单位秒，用于周跳探测初值赋予等，需要想办法设置。默认为30秒。\n"
+"IsIonoParamModelCorrectionRequired = False	# 是否需要电离层导航参数模型\n"
+"IsEpochIonoFileRequired = False	# 是否需要GNSSer历元电离层文件\n"
+"\n"
+"# [Correction]\n"
+"IsFrequencyCorrectionsRequired = True	# 是否需要频率改正\n"
+"IsApproxModelCorrectionRequired = True	# 是否需要近似模型改正\n"
+"IsSiteCorrectionsRequired = True	# 测站改正\n"
+"IsDcbCorrectionRequired = True	# 是否需要DCB改正\n"
+"IsReceiverAntPcoCorrectionRequired = True	# 接收机天线PCO改正\n"
+"IsOceanTideCorrectionRequired = True	# 海洋潮汐改正\n"
+"IsSolidTideCorrectionRequired = True	# 固体潮改正\n"
+"IsIonoCorretionRequired = True	# 是否启用电离层模型改正。顶层接口，如果要采用电离层改正观测近似值，则必须设定。\n"
+"IsPoleTideCorrectionRequired = True	# 极潮改正\n"
+"IsRangeCorrectionsRequired = True	# 伪距改正\n"
+"IsTropCorrectionRequired = True	# 对流层改正\n"
+"IsSatClockBiasCorrectionRequired = True	# 卫星钟差改正\n"
+"IsSatAntennaPhaseCenterCorrectionRequired = True	# 卫星天线相位中心改正\n"
+"IsRecAntennaPcoCorrectionRequired = True	# 接收机天线PCO改正\n"
+"IsRecAntennaPcvRequired = True	# 接收机天线PCV改正\n"
+"IsPhaseWindUpCorrectionRequired = True	# 相位缠绕改正\n"
+"IsGravitationalDelayCorrectionRequired = True	# 重力延迟改正\n"
+"IsObsCorrectionRequired = True	# 是否需要观测值改正\n"
+"\n"
+"# [DataSource]\n"
+"IsStationInfoRequired = False	# 测站信息文件，主要包含天线时段信息。\n"
+"IsIndicatingStationInfoFile = False	# 是否指定测站信息文件\n"
+"IsSiteCoordServiceRequired = False	# 是否需要坐标服务\n"
+"EnableClockService = True	#  是否启用单独的钟差服务（文件）\n"
+"Isgpt2File1DegreeRequired = True	# 是否需要 GPT2的1度格网文件\n"
+"IsEnableNgaEphemerisSource = True	# 是否启用NGA星历匹配，作为实时计算的备份。\n"
+"IsUniqueSource = True	# 是否使用唯一数据源，当自动匹配时使用\n"
+"IsSwitchWhenEphemerisNull = False	# 在获取星历失败后，是否切换星历数据源\n"
+"ObsFiles = ftp://cddis.gsfc.nasa.gov/pub/gps/data/daily/2013/001/13o/aber0010.13o.Z	# 观测文件路径，逗号“,”分隔\n"
+"IndicatedSourceCode = ig	# 指定的IGS数据源，前两个字作为代码\n"
+"Isgpt2FileRequired = True	# 是否采用GPT2通用文件改正\n"
+"IsVMF1FileRequired = True	# 是否需要VMF1文件\n"
+"IsDCBFileRequired = True	# 是否需要DCB文件\n"
+"IsOceanLoadingFileRequired = True	# 是否需要潮汐文件\n"
+"IsSatInfoFileRequired = True	# 是否需要卫星信息文件\n"
+"IsSatStateFileRequired = True	# 是否需要卫星状态文件\n"
+"IsAntennaFileRequired = True	# 是否需要天线文件\n"
+"IsPreciseEphemerisFileRequired = True	# 是否需要精密星历文件\n"
+"IsEphemerisRequired = True	# 是否需要星历， 默认需要\n"
+"IsPreciseClockFileRequired = True	# 是否需要精密钟差文件\n"
+"StationInfoPath = Data\\GNSS\\Common\\StationInfo.stainfo	# 测站信息文件路径\n"
+"IsIgsIonoFileRequired = False	# 是否需要IGS电离层格网文件\n"
+"TropAugmentFilePath =	# 对流层增强文件\n"
+"EphemerisFilePath = Data\\GNSS\\Rinex\\2013.01.01_17212\\brdc0010.13n	# 指定的星历路径\n"
+"ClockFilePath = Data\\GNSS\\IgsProduct\\igs17212.clk	# 指定的钟差路径，具有最高优先权\n"
+"IsIndicatingCoordFile = False	# 是否指定坐标文件\n"
+"IsIndicatingEphemerisFile = False	# 是否指定星历数据源，具有最高优先权\n"
+"EpochIonoParamFilePath =	# 历元电离层参数文件路径\n"
+"IsIndicatingGridIonoFile = False	# 是否指定格网电离层文件\n"
+"IonoGridFilePath = Data\\GNSS\\IgsProduct\\igsg0010.13i	# 格网电离层文件路径\n"
+"IsP2C2Enabled = False	# 是否需要 P2C2 \n"
+"IsIndicatingClockFile = False	# 是否指定钟差\n"
+"IsObsDataRequired = True	# 是否需要观测数据源\n"
+"IsLengthPhaseValue = False	# 载波相位是否是以米为单位，如Android接收机单位为米。\n"
+"NavIonoModelPath =	# 导航文件路径，用于提取电离层改正等\n"
+"IsEnableRealTimeCs = True	# 是否启用实时周跳探测\n"
+"IsErpFileRequired = True	# 是否采用ERP文件改正\n"
+"\n"
+"# [Default]\n"
+"IsCycleSlipReparationRequired = False\n"
+"IsAllowMissingEpochSite = False\n"
+"ApproxDataType = ApproxPseudoRangeA\n"
+"IsIndicateObsType = False\n"
+"RangeType = IonoFreeRangeOfAB\n"
+"IsOutputSinex = False\n"
+"IsBaseSatelliteRequried = False\n"
+"IsRequireSameSats = False\n"
+"IsFixingAmbiguity = False\n"
+"IsUpdateStationInfo = False\n"
+"IsIndicatingApproxXyzRms = False\n"
+"IsIndicatingApproxXyz = False\n"
+"IsEnableInitApriori = False\n"
+"InitApriori =\n"
+"RinexObsFileFormatType        =单站单历元\n"
+"IsFixingCoord                 =False\n"
+"CoordFilePath                 =\n"
+"IsOutputSummery               =False\n"
+"IsUpdateEstimatePostition     =False\n"
+"IsSmoothingRangeByPhase       =False\n"
+"IgnoreCsedOfBufferCs          =False\n"
+"IsOutputWetTrop               =False\n"
+"IsOutputIono                  =False\n"
+"IsPromoteTransWhenResultValueBreak=False\n"
+"IsResidualCheckEnabled        =False\n"
+"\n"
+"# [Output]\n"
+"IsOutputAdjust                =False	# 是否输出平差文件\n"
+"OutputDirectory               =Temp\\	# 结果输出目录\n"
+"IsOutputEpochResult           =True	# 是否输出逐个历元计算结果\n"
+"OutputBufferCount             =10000	# 输出结果缓存大小\n"
+"IsOutputResult                =True	# 是否输出结果的总开关，只有此为true才会判断下面的输出\n"
+"IsOpenReportWhenCompleted     =True	# 是否在计算结束时打开平差报告\n"
+"OutputRinexVersion            =3.02	# RINEX 输出版本 2.11 或 3.02\n"
+"IsOutputSatInfo               =False	# 是否输出卫星信息\n"
+"\n"
+"# [PreProcess]\n"
+"MaxEpochSpan                  =120	# 最大的时间跨度，单位：秒，如果历元之间超过了这个时段，则清空以往数据，重新构建对象。\n"
+"DifferTimesOfBufferCs         =1	# 缓存周跳差分次数\n"
+"IsUsingRecordedCycleSlipInfo  =True	# 是否采用数据源信息标记的周跳，若已标记周跳，则认为有。\n"
+"MinAllowedRange               =15000000	# 允许最小的伪距\n"
+"PolyFitOrderOfBufferCs        =2	# 缓存周跳拟合阶次\n"
+"MinWindowSizeOfCs             =5	# 缓存周跳 最小窗口大小，小于此，都认为有周跳。\n"
+"CycleSlipDetectSwitcher       =	# 周跳开关.优先考虑周跳探测器开关,如为空，然后考虑默认周跳探测器。\n"
+"IsDopplerShiftRequired        =False	# 是否需要多普勒频率\n"
+"IsAliningPhaseWithRange       =False	# 是否将初始相位采用伪距对齐\n"
+"IsOutputCycleSlipFile         =False	# 是否输出周跳文件\n"
+"MaxErrorTimesOfBufferCs       =3	# 缓存周跳差分次数\n"
+"IsCycleSlipDetectionRequired  =True	# 是否进行周跳探测\n"
+"MaxBreakingEpochCount         =4	# 周跳探测允许最大断裂的时间间隔\n"
+"MaxValueDifferOfHigherDifferCs=14	# 高次差周跳探测中，允许的最大的误差\n"
+"MaxRmsTimesOfLsPolyCs         =50	# 多项式拟合周跳探测中，最大的误差倍数\n"
+"MaxDifferValueOfMwCs          =8.6	# MW周跳探测中，最大的误差\n"
+"IsReverseCycleSlipeRevise     =True	# 启用逆序周跳探测\n"
+"MaxAllowedRange               =40000000	# 允许最大的伪距\n"
+"IsEnableBufferCs              =False	# 启用缓存周跳探测\n"
+"\n";

        }
    }

}
