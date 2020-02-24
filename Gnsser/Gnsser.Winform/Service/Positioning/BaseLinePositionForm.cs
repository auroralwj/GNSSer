//2014.06.24,  cy, edit, 读取卫星精密钟差文件,崔阳，
//2014.08.19, czs, edit, 采用数据流的方式读取观测文件，适合于大文件读取

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser;
using Geo.Times; 
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service; 
using Geo.Coordinates;  
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.IO;


namespace Gnsser.Winform
{
    /// <summary>
    /// 单点定位。
    /// </summary>
    public partial class BaseLinePositionForm : Form, Gnsser.Winform.IShowLayer
    {
        public event ShowLayerHandler ShowLayer;
        Log log = new Log(typeof(BaseLinePositionForm));
        /// <summary>
        /// 构造函数
        /// </summary> 
        public BaseLinePositionForm()
        {
            InitializeComponent();
        }

        LogWriter LogWriter = LogWriter.Instance;
        void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer)
        {
            if (IsShowProcessInfo)// && LogType != Geo.IO.LogType.Debug)
            {
                var info = LogType.ToString() + "\t" + msg;// +"\t" + msgProducer.Name;
                ShowInfo(info);
            }
        }
        #region 变量、属性
        ISingleSiteObsStream RefObsDataSource;
        ISingleSiteObsStream RovObsDataSource;
        string  rovPath;
        FileEphemerisService ephemerisDataSource;
        Data.ISimpleClockService clockFile;
        List<BaseGnssResult> _results = new List<BaseGnssResult>();
        List<Geo.Algorithm.RmsedVector> Adjustments = new List<Geo.Algorithm.RmsedVector>();
        DateTime startTime = DateTime.Now;
        IGnssSolver _positioner = null;
        CaculateType CaculateType = CaculateType.Filter;
        int ProcessCount = 0;
        private bool IsCancel;


        #endregion

        #region 界面属性
        /// <summary>
        /// 是否采用手动星历数据源
        /// </summary>
        bool IsSetEphemerisFile { get { return this.checkBox_setEphemerisFile.Checked; } set { this.checkBox_setEphemerisFile.Checked = value; } }

        /// <summary>
        /// 工程目录
        /// </summary>
        public string ProjectOutputDirectory { get { return Path.Combine(Setting.GnsserConfig.OutputDirectory, ProjectName); } }
        /// <summary>
        /// 项目名称
        /// </summary>
        string ProjectName { get { return textBox_taskName.Text; } set { textBox_taskName.Text = value; } }
        bool IsOutputReslultFile { get { return checkBox_outputReslultFile.Checked; } set { checkBox_outputReslultFile.Checked = value; } }
        #endregion

        #region 界面设置 数据源路径设置
        /// <summary>
        /// 是否显示过程信息。
        /// </summary>
        bool IsShowProcessInfo { get { return checkBox_showProcessInfo.Checked; } }
        private void button_getObsPath_Click(object sender, EventArgs e) { if (this.openFileDialog_obs.ShowDialog() == DialogResult.OK)   this.textBox_obsPath_ref.Text = this.openFileDialog_obs.FileName; }
        private void button_setObsPath_rov_Click(object sender, EventArgs e) { if (this.openFileDialog_obs.ShowDialog() == DialogResult.OK)   this.textBox_obsFile_rov.Text = this.openFileDialog_obs.FileName; }

        private void button_getNavPath_Click(object sender, EventArgs e) { if (this.openFileDialog_nav.ShowDialog() == DialogResult.OK)    this.textBox_navPath.Text = this.openFileDialog_nav.FileName; }
        private void PointPositionForm_Load(object sender, EventArgs e)
        {

            this.checkBox_enableNet.Checked = Setting.EnableNet;
            checkBox_debugModel.Checked = Setting.IsShowDebug;
            this.checkBox_autoMatchingFile.Checked = Setting.GnsserConfig.EnableAutoFindingFile;
            LogWriter.MsgProduced += LogWriter_MsgProduced;
            rmsedXyzControl_ref.Text = "参考站坐标";
            rmsedXyzControl_rov.Text = "流动站坐标";
            this.satDataTypeSelectControl1.Text = "观测值类型";
            this.satDataTypeSelectControl2.Text = "近似值类型";
            IsSetEphemerisFile = true; IsSetEphemerisFile = false; textBox_taskName.Text = "单基线差分_" + DateTimeUtil.GetDateTimePathStringNow(); SetBeidou3Path();
        }

        private void button_getClockPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_clock.ShowDialog() == DialogResult.OK)
                this.textBox_ClockPath.Text = this.openFileDialog_clock.FileName;
        }
        private void checkBox_setEphemerisFile_CheckedChanged(object sender, EventArgs e) { textBox_navPath.Enabled = IsSetEphemerisFile; button_getNavPath.Enabled = IsSetEphemerisFile; }
        private void button_openProjDir_Click(object sender, EventArgs e) { FileUtil.CheckOrCreateDirectory(ProjectOutputDirectory); FileUtil.OpenDirectory(this.ProjectOutputDirectory); }

        private void button_datasourceSeting_Click(object sender, EventArgs e) { new ComonSourceSettingForm().ShowDialog(); }

        private void checkBox_enableClockFile_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox_ClockPath.Enabled = this.checkBox_enableClockFile.Checked;
            this.button_getClockPath.Enabled = this.checkBox_enableClockFile.Checked;
        }

        #region 默认地址的设置
        private void radioButton_setRinexPathes_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_gps2_0.Checked) { SetDefaultGps2_0Path(); }
            if (this.radioButtonGPSClose.Checked)
            {
                this.textBox_obsPath_ref.Text = Application.StartupPath + @"\Data\GNSS\Rinex\2013.01.01_001_17212_V2_GPS\zimm0010.13o";
                this.textBox_obsFile_rov.Text = Application.StartupPath + @"\Data\GNSS\Rinex\2013.01.01_001_17212_V2_GPS\zim20010.13o";
                this.textBox_navPath.Text = Setting.GnsserConfig.SampleSP3File;
                this.textBox_ClockPath.Text = Setting.GnsserConfig.SampleClkFile;
            }
            if (this.radioButton_gps5km.Checked)
            {
                this.textBox_obsPath_ref.Text = Application.StartupPath + @"\Data\GNSS\Rinex\2013.01.01_001_17212_V2_GPS\zimm0010.13o";
                this.textBox_obsFile_rov.Text = Application.StartupPath + @"\Data\GNSS\Rinex\2013.01.01_001_17212_V2_GPS\wab20010.13o";
                this.textBox_navPath.Text = Setting.GnsserConfig.SampleSP3File;
                this.textBox_ClockPath.Text = Setting.GnsserConfig.SampleClkFile;
            }

            if (this.radioButton_beidou3.Checked || this.radioButton_gps3_0.Checked) { SetBeidou3Path(); }

            if (this.radioButton_beidou2_0.Checked)
            {
                this.textBox_obsPath_ref.Text = Application.StartupPath + @"\Data\GNSS\Rinex\2013.03.06_065_17303_V2MultiSyStem\BD\0800065A00.13O";
                this.textBox_navPath.Text = Application.StartupPath + @"\Data\GNSS\Rinex\2013.03.06_065_17303_V2MultiSyStem\BD\wum17303.sp3";
                this.textBox_ClockPath.Text = Application.StartupPath + @"\Data\GNSS\Rinex\2013.03.06_065_17303_V2MultiSyStem\BD\wum17303.clk";
            }
        }

        private void SetBeidou3Path()
        {
            this.textBox_obsPath_ref.Text = Setting.GnsserConfig.SampleOFileV3A;
            this.textBox_navPath.Text = Setting.GnsserConfig.SampleNFileV3;
            this.textBox_obsFile_rov.Text = Setting.GnsserConfig.SampleOFileV3B;
            this.textBox_ClockPath.Text = Setting.GnsserConfig.SampleClkFileV3;
        }

        private void SetDefaultGps2_0Path()
        {
            this.textBox_obsPath_ref.Text = Setting.GnsserConfig.SampleOFileA;
            this.textBox_obsFile_rov.Text = Setting.GnsserConfig.SampleOFileB;
            this.textBox_navPath.Text = Setting.GnsserConfig.SampleSP3File;
            this.textBox_ClockPath.Text = Setting.GnsserConfig.SampleClkFile;
        }
        #endregion

        #endregion

        #region 计算

        //历元独立计算
        private void button_cacuIndependent_Click(object sender, EventArgs e)
        {
            try
            {
                this.CaculateType = CaculateType.Independent; 

                Caculate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //滤波
        private void button_kalman_Click(object sender, EventArgs e)
        {
            //try
            //{
            this.CaculateType = CaculateType.Filter;


            Caculate();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        #region 计算细节
        IObsDataAnalyst ObsDataAnalyst { get; set; }
        /// <summary>
        /// 数据分析
        /// </summary>
        public void Analysis()
        {

            if (RefObsDataSource == null || RovObsDataSource == null)
            {
                MessageBox.Show("请先读取数据。");
                return;
            }
            FormUtil.ShowWaittingForm("正在对数据进行分析。。。");

            GnssProcessOption option = GetModel(RovObsDataSource.ObsInfo.StartTime);
            LoadDataSource();

            DataSourceContext DataSourceContext = DataSourceContext.LoadDefault(option, this.textBox_obsPath_ref.Text, ephemerisDataSource, clockFile);

            ObsDataAnalyst = new BaseLineObsDataAnalyst(DataSourceContext, RefObsDataSource, RovObsDataSource, option);

            ObsDataAnalyst.SatCycleSlipMaker.SaveSatPeriodText(ProjectOutputDirectory + "\\SatCycleSlipMaker_"+DateTime.Now.Ticks+".txt");
            ObsDataAnalyst.SatVisibleMaker.SaveSatPeriodText(ProjectOutputDirectory + "\\SatVisibleMaker_" + DateTime.Now.Ticks + ".txt");

            File.WriteAllText(ProjectOutputDirectory + "\\BaseSat_" + DateTime.Now.Ticks + ".txt", ObsDataAnalyst.SatelliteSelector.GetFormatedString());

            var title = ("\r\n----------已经选择的基准卫星-----------\r\n");
            ShowInfo(title + ObsDataAnalyst.SatelliteSelector.GetFormatedString());
            title = ("\r\n----------卫星的可见性-----------\r\n");
            ShowInfo(title + ObsDataAnalyst.SatVisibleMaker.ToFormatedString());
            title  = ("\r\n----------卫星周跳-----------\r\n");
            ShowInfo(title + ObsDataAnalyst.SatCycleSlipMaker.ToFormatedString());
        }


        /// <summary>
        /// 数据源读取
        /// </summary>
        private void LoadDataSource()
        {
            startTime = DateTime.Now;

            //星历数据源
            ephemerisDataSource = null;
            if (IsSetEphemerisFile)//读取星历数据
            {
                string ephemerisPath = this.textBox_navPath.Text;
                FileEphemerisType ephType = EphemerisDataSourceFactory.GetFileEphemerisTypeFromPath(ephemerisPath);
                ephemerisDataSource = EphemerisDataSourceFactory.Create(ephemerisPath);
            }
            //加载文件数据
            this.rovPath = this.textBox_obsFile_rov.Text;
            this.RefObsDataSource = new RinexFileObsDataSource(this.textBox_obsPath_ref.Text);
            this.RovObsDataSource = new RinexFileObsDataSource(rovPath);

            //使用外部设置的概略坐标。
            if (this.rmsedXyzControl_rov.IsEnabled)
            {
                this.RovObsDataSource.SiteInfo.SetApproxXyz ( this.rmsedXyzControl_rov.RmsedXyz.Value);
                this.RovObsDataSource.SiteInfo.EstimatedXyzRms = this.rmsedXyzControl_rov.RmsedXyz.Rms;
            }
            //概略坐标显示到界面上。
            rmsedXyzControl_rov.SetRmsedXyz(new RmsedXYZ(RovObsDataSource.SiteInfo.ApproxXyz, RovObsDataSource.SiteInfo.EstimatedXyzRms));


            //使用外部设置的概略坐标。
            if (this.rmsedXyzControl_ref.IsEnabled)
            {
                this.RefObsDataSource.SiteInfo.SetApproxXyz( this.rmsedXyzControl_ref.RmsedXyz.Value);
                this.RefObsDataSource.SiteInfo.EstimatedXyzRms = this.rmsedXyzControl_ref.RmsedXyz.Rms;
            }
            //概略坐标显示到界面上。
            rmsedXyzControl_ref.SetRmsedXyz(new RmsedXYZ(RefObsDataSource.SiteInfo.ApproxXyz, RefObsDataSource.SiteInfo.EstimatedXyzRms));



            //检查是否读取钟差数据
            clockFile = null;
            if (this.checkBox_enableClockFile.Checked)
            { 
                clockFile = new Data.SimpleClockService(this.textBox_ClockPath.Text);
            }

            TimeSpan span = DateTime.Now - startTime;

            ShowInfo("数据已读入,时间（秒）：" + span.TotalSeconds);
        }

        private void ShowInfo(string msg) {
         //   if (this.IsClosed) { return; }
            FormUtil.InsertLineWithTimeToTextBox(this.textBox_estParam, msg);     
          //  this.Invoke(new Action(delegate() { this.textBox_estParam.Text += DateTimeUtil.GetFormatedTimeNow() + ":\t" + msg + "\r\n"; })); 
        }

        /// <summary>
        /// 计算选项。
        /// </summary>
        /// <returns></returns>
        private GnssProcessOption GetModel(Time startTime)
        {
            GnssProcessOption model = new GnssProcessOption()
            {
                FilterCourceError = checkBox_ignoreCourceError.Checked,
                CaculateType = this.CaculateType,
                MaxStdDev = double.Parse(this.textBox_maxStd.Text),
                EnableClockService = this.checkBox_enableClockFile.Checked,
                SatelliteTypes = multiGnssSystemSelectControl1.SatelliteTypes,
                ObsDataType = satDataTypeSelectControl1.CurrentdType,
                ApproxDataType = satDataTypeSelectControl2.CurrentdType,
                MutliEpochSameSatCount = int.Parse(this.textBox_differSatCount.Text),
                MultiEpochCount = int.Parse(this.textBox_differEpochCount.Text),
                MaxMeanStdTimes = Double.Parse(this.textBox_maxstdTimes.Text),
                IsFixingAmbiguity = checkBox_ambiguityFixed.Checked,
                IsBaseSatelliteRequried = true
            };
            return model;
        }
        private void Caculate()
        {
            this.IsCancel = false;
            this.EnableButton(false);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            startTime = DateTime.Now;
            //线程访问，避免等待。
            this.Invoke(new Action(delegate() { LoadDataSource(); }));

            ObsDataAnalyst = null;
            if (checkBox_preAnalysis.Checked && this.CaculateType == CaculateType.Filter) // 分析一次就够了，否则浪费时间。
            {
                Analysis();
            }

            ShowInfo("数据分析时间：" + (DateTime.Now - startTime));

            //  try
            //{
            ProcessCount = 0;
            startTime = DateTime.Now;

            int MaxProcessCount = int.Parse(this.textBox_caculateCount.Text);
            int startEphoch = int.Parse(textBox_startEpoch.Text);

            _results.Clear();
            Adjustments.Clear();

            GnssProcessOption option = GetModel(RovObsDataSource.ObsInfo.StartTime);
            //统计共同卫星
            //   SourcePrnStatistics sta = new SourcePrnStatistics(RefObsDataSource, RovObsDataSource, option.SatelliteTypes, 6);
            //   option.CommonPrns = sta.CommonPRNs;
            List<string> oPathes = new List<string>() { this.textBox_obsPath_ref.Text, rovPath };
            var MultiSiteObsDataSource = new MultiSiteObsStream(oPathes, option.BaseSiteSelectType, true, option.IndicatedBaseSiteName);
            
            DataSourceContext context = DataSourceContext.LoadDefault(option, MultiSiteObsDataSource, ephemerisDataSource, clockFile);

            //选择适当的差分算法
            if (radioButton_pppResidualDiffer.Checked)
            {
                _positioner = new IonFreeDoubleDifferPositioner(context, option);
                ((IonFreeDoubleDifferPositioner)_positioner).Produced += pp_ProgressIncreased;
            }
            if (radioButton_phaseSingleDiffer.Checked)
            {
                //   option.IsUseFiexedSats = true;
                _positioner = new SingleDifferPositioner(context, option);
                ((SingleDifferPositioner)_positioner).Produced += pp_ProgressIncreased;
            }
            if (radioButton_norelevant.Checked)
            {
                //   option.IsUseFiexedSats = true;
                _positioner = new SingleDifferNoRelevantPositioner(context, option);
                ((SingleDifferNoRelevantPositioner)_positioner).Produced += pp_ProgressIncreased;
            }
            if (radioButton_doubleDiffer.Checked)
            {
                // option.IsUseFiexedSats = true;
                _positioner = new PeriodDoublePhaseDifferPositioner(context, option);
                ((PeriodDoublePhaseDifferPositioner)_positioner).Produced += pp_ProgressIncreased;
            }


            int CurrentIndex = 0;
            var MultiSiteSatTimeInfoManager = new Gnsser.MultiSiteSatTimeInfoManager(MultiSiteObsDataSource.BaseDataSource.ObsInfo.Interval);
            var checker = MultiSiteEpochCheckingManager.GetDefault(context, option);
            var reviser = MultiSiteEpochInfoReviseManager.GetDefault(context, option, MultiSiteSatTimeInfoManager);
            var rawreviser = MultiSiteEpochInfoReviseManager.GetDefaultRaw(context, option);
            var bufferedStream = new Geo.BufferedStreamService<MultiSiteEpochInfo>(context.ObservationDataSources, option.BufferSize);
            var MultiSitePeriodInfoBuilder = new Domain.MultiSitePeriodInfoBuilder(option);
            foreach (var item in bufferedStream)
            {
                if (IsCancel) { log.Info("计算被手动取消。"); break; }
                if (CurrentIndex < option.StartIndex) { log.Info("略过历元编号 " + CurrentIndex + ", 将开始于 " + option.StartIndex); continue; }
                if (CurrentIndex > option.StartIndex + option.CaculateCount) { log.Info("计算达到设置的最大数量" + option.CaculateCount); break; }
                //if (key.First.ReceiverTime > this.EphemerisEndTime)
                //{
                //    log.Error("星历服务停止！无法继续计算。");
                //    break;
                //}
                if (!checker.Check(item)) { continue; }
                var val = item;
                if (!rawreviser.Revise(ref val)) { continue; }

                if (!reviser.Revise(ref val)) { continue; }

                BaseGnssResult GnssResult = null;
                if (_positioner is MultiSitePeriodSolver)
                {
                    MultiSitePeriodInfoBuilder.Add(item);
                    var period = MultiSitePeriodInfoBuilder.Build();
                    if (period == null || !period.Enabled) { continue; }

                    GnssResult = ((MultiSitePeriodSolver)_positioner).Get(period);
                }
                else
                {
                    GnssResult = ((MultiSiteEpochSolver)_positioner).Get(item);
                }

                if (GnssResult == null) { continue; }


                //GnssResults.Add(GnssResult);

                ////注册参数名称
                //NamedValueRowManager.Regist(GnssResult.ParamNames);

                //if (IsCancel) { break; }
                //ShowNotice("当前进度：" + CurrentIndex++);
            }

            //_positioner.Gets(startEphoch, MaxProcessCount);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        #endregion
        #endregion

        #region 状态控制
        private void button_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                EnableButton(true);
                //_positioner.IsCancel = true;
                backgroundWorker1.CancelAsync();
                this.IsCancel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PointPositionForm_FormClosing(object sender, FormClosingEventArgs e) {

            if (backgroundWorker1.IsBusy)
            {
                if (FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                   // IsClosed = true; //解除绑定                   
                    LogWriter.MsgProduced -= LogWriter_MsgProduced;
                }
            }
            else
            {
              //  IsClosed = true;
                LogWriter.MsgProduced -= LogWriter_MsgProduced;
            }            
        }

        void pp_ProgressIncreased(BaseGnssResult e, MultiSiteEpochInfo sender)
        {
            BaseGnssResult result = e as BaseGnssResult;
            if (result == null) return;
            _results.Add(result);
            Adjustments.Add(result.ResultMatrix.Estimated.GetRmsedVector());
            ShowCurrent(result);
        }
        void pp_ProgressIncreased(BaseGnssResult e, MultiSitePeriodInfo sender)
        {
            BaseGnssResult result = e as BaseGnssResult;
            if (result == null) return;
            _results.Add(result);
            ShowCurrent(result);
        }

        private void ShowCurrent(BaseGnssResult result)
        {
            ProcessCount++;
            this.Invoke(new Action(delegate()
            {
                this.backgroundWorker1.ReportProgress(ProcessCount);

                if (this.radio_out100.Checked && ProcessCount > 100) return;
                if (radio_noOut.Checked == true) return;

                // this.textBox_rms.SuspendLayout();
                this.textBox_appriori.Text += "\r\n" + result.GetParamStdVectorString();
                this.textBox_estParam.Text += "\r\n" + result.GetEstimatedVectorString();
                this.textBox_result.Text += "\r\n" + result.ToString();
                //   this.textBox_rms.Update();
                //   this.Update();
            }));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableButton(true);

            if (_results == null || _results.Count == 0) return;

            ShowDifferXyz();
            ShowDeltaEst();
            ShowResult();
            ShowApprioriResult();
            ShowobsMinusApriori();
            ShowMetaInfo();

            if (IsOutputReslultFile)
            {
                FormUtil.ShowWaittingForm("请稍后！正在写入计算结果。。。");
                GnssResultWriter writer = new GnssResultWriter(_positioner.Option,  this.checkBox_outputAdjust.Checked);
                writer.WriteFinal(_results[_results.Count -1]);
            }
            if (IsOutputReslultFile)
                FormUtil.ShowOkAndOpenDirectory(ProjectOutputDirectory, "计算完毕！共处理 " + ProcessCount + " 条数据！是否查看改正数文件？");
            else MessageBox.Show(" 计算完毕！共处理 " + ProcessCount + " 条数据！");
        }

        private void EnableButton(bool enable = true) { button_kalman.Enabled = enable; button_cacuAll.Enabled = enable; }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Invoke(new Action(delegate()
            {
                this.label_progress.Text = e.ProgressPercentage + "";
            }));
        }
        #endregion

        #region 显示结果,输出

        #region 文本框显示
        private void ShowMetaInfo()
        {
            BaseGnssResult last = _results[_results.Count - 1];

            TimeSpan span = DateTime.Now - startTime;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时：" + DateTimeUtil.GetFloatString(span));
            sb.AppendLine("共处理 " + ProcessCount + " 条数据。");

            sb.AppendLine("数据处理类型： " + CaculateType + " ");
            //sb.AppendLine("        概略坐标：" + last.ApproxXyz.GetTabValues() + ", RMS : " + last.EstimatedXyzRms.GetTabValues());
            //sb.AppendLine("最后历元平差结果：" + last.EstimatedXyz.GetTabValues());
            //sb.AppendLine("最后历元偏差：" + (last.EstimatedXyz - last.ApproxXyz).GetTabValues());
            //var vector = (last.EstimatedXyz - this.RefObsDataSource.SiteInfo.EstimatedXyz);
            //sb.AppendLine("最后历元基线：" + vector.GetTabValues());

            //sb.AppendLine("最后基线长度：" + vector.Length);
            sb.AppendLine();
            sb.AppendLine("参考站概略坐标：" + this.RefObsDataSource.SiteInfo.EstimatedXyz.GetTabValues());
            sb.AppendLine("概略基线：" + (this.RovObsDataSource.SiteInfo.EstimatedXyz - this.RefObsDataSource.SiteInfo.ApproxXyz).GetTabValues());
            sb.AppendLine("基线长度：" + (this.RovObsDataSource.SiteInfo.EstimatedXyz - this.RefObsDataSource.SiteInfo.ApproxXyz).Radius());
            sb.AppendLine();
            //sb.AppendLine("接收机天线信息：" + last.Material.SiteInfo.Antenna == null ? "没有天线信息" : last.Material.SiteInfo.Antenna + "");

            this.Invoke(new Action(delegate() { this.textBox_resultInfo.Text = sb.ToString(); }));
        }
        private void ShowobsMinusApriori()
        {
            StringBuilder sb = new StringBuilder();

            BaseGnssResult last = _results[_results.Count - 1];
            sb.AppendLine(last.ResultMatrix.ToString());

            this.Invoke(new Action(delegate()
            {
                this.textBox_obsMinusApriori.Text = sb.ToString();
            }));
        }

        private void ShowDifferXyz()
        {
            TimeSpan span = DateTime.Now - startTime;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时：" + DateTimeUtil.GetFloatString(span));
            sb.Append("时间:\t");
            if (_results == null) return;
            sb.AppendLine(_results[0].GetParamNameString());
            int i = 0;
            foreach (var item in _results)
            {
                //sb.AppendLine(i.ToString("0000") + "  " + (key.CorrectedXyz).GetTabValues());
                i++;
            }
            this.Invoke(new Action(delegate()
            {
                this.textBox_differXyz.Text = sb.ToString();
            }));
        }
        private void ShowDeltaEst()
        {
            TimeSpan span = DateTime.Now - startTime;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时：" + DateTimeUtil.GetFloatString(span));
            sb.Append("时间:\t");
            sb.AppendLine(_results[0].GetParamNameString());
            int i = 0;
            foreach (var item in _results)
            {
                sb.AppendLine(i.ToString("0000") + "  " + item.GetEstimatedVectorString());
                i++;
            }
            this.Invoke(new Action(delegate()
            {
                this.textBox_estParam.Text = sb.ToString();
            }));
        }
        private void ShowResult()
        {
            TimeSpan span = DateTime.Now - startTime;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时：" + DateTimeUtil.GetFloatString(span));
            sb.Append("编号\t时间:\t");
            sb.AppendLine(_results[0].GetParamNameString());
            int i = 0;
            foreach (var item in _results)
            {
                sb.AppendLine(i.ToString("0000") + "  " + item.ToString() + "  " + item.BasePrn.ToString());
               
                i++;
            }
            this.Invoke(new Action(delegate()
            {
                this.textBox_result.Text = sb.ToString();
            }));
        }

        private void ShowApprioriResult()
        {
            TimeSpan span = DateTime.Now - startTime;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时：" + DateTimeUtil.GetFloatString(span));
            sb.Append("时间:\t");
            sb.AppendLine(_results[0].GetParamNameString());
            int i = 0;
            foreach (var item in _results)
            {
                string stdDevStr = Geo.Utils.StringUtil.FillSpaceLeft(item.ResultMatrix.StdDev.ToString("0.00"), 10);
                sb.Append(i.ToString("0000") + "  " + stdDevStr);
                sb.Append(" ");
                sb.Append(item.GetApprioriVectorString());
                //sb.Append(" ");
                //sb.Append(path.GetParamNameString());
                sb.AppendLine();
                i++;
            }
            this.Invoke(new Action(delegate()
            {
                this.textBox_appriori.Text = sb.ToString();
            }));
        }
        #endregion

        #region 绘图
        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowLayer != null && _results.Count != 0)
                {
                    int start = this.positonResultRenderControl11.StartIndex; ;
                    PostionResultLayerBuilder builder = new PostionResultLayerBuilder(_results, start);
                    builder.AddPt(this.RovObsDataSource.SiteInfo.ApproxXyz, this.RovObsDataSource.SiteInfo.SiteName + "_Rov");
                    builder.AddPt(this.RefObsDataSource.SiteInfo.ApproxXyz, this.RefObsDataSource.SiteInfo.SiteName + "_Ref");
                    ShowLayer(builder.Build());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_drawRmslines_Click(object sender, EventArgs e) { this.positonResultRenderControl11.SetResult(Adjustments); this.positonResultRenderControl11.DrawParamRmsLine(); }
        private void button_drawDifferLine_Click(object sender, EventArgs e) { this.positonResultRenderControl11.SetResult(Adjustments); this.positonResultRenderControl11.DrawParamLines(); }
        #endregion

        #region 输出
        private void button_toSinex_Click(object sender, EventArgs e)
        {
            GnssResultWriter writer = new GnssResultWriter(_positioner.Option, this.checkBox_outputAdjust.Checked);
            //writer.WriteSinexFile(_results[_results.Count - 1]);
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(ProjectOutputDirectory);
        }
        #endregion

        private void checkBox_outputReslultFile_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_outputAdjust.Enabled = IsOutputReslultFile;
        } 
        #endregion
         

        private void button_analysisData_Click(object sender, EventArgs e)
        {
            LoadDataSource();
            this.Analysis();
        }

        private void checkBox_autoMatchingFile_CheckedChanged(object sender, EventArgs e)
        {

            Setting.GnsserConfig.EnableAutoFindingFile = this.checkBox_autoMatchingFile.Checked;
        }

        private void checkBox_enableNet_CheckedChanged(object sender, EventArgs e)
        {

            Setting.EnableNet = this.checkBox_enableNet.Checked;
        }

        private void checkBox_debugModel_CheckedChanged(object sender, EventArgs e)
        {
             Setting.IsShowDebug =checkBox_debugModel.Checked;
        }
    }
}