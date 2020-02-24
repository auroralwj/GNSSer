//2014.09.12, czs, create, 多文件处理。
//2014.09.16, czs,  edit, 多核并行计算。
//2014.09.16, czs,  edit in jinxinliaomao shuangliao, 修正并行计算，计算结果与串行相同
//2014.09.16, czs,  edit in jinxinliaomao shuangliao, 复制修改为基线解算

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;  
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Gnsser.Service;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times; 
using System.Threading.Tasks;

namespace Gnsser.Winform
{
    /// <summary>
    /// 批量流动站基线解算。
    /// </summary>
    public partial class MultiBaseLinePositionForm : Form, Gnsser.Winform.IShowLayer
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public MultiBaseLinePositionForm() { InitializeComponent(); }

        #region 属性

        public event ShowLayerHandler ShowLayer;
        List<ISingleSiteObsStream> _obsDataSources;
        int _processedEpochCount = 0;
        int _processedFileCount = 0;
        StringBuilder _globalInfo;
        BufferedTimePeriod TimePeriod { get; set; }

        /// <summary>
        /// 是否取消计算了。
        /// </summary>
        bool _IsCanceled { get; set; }
        DateTime _startTime;
        List<BaselineTask> _positioners = new List<BaselineTask>();
        IGnssSolver _curentPositioner;
        #region 界面输入属性
        bool IsOutputResultFile { get { return checkBox_outputReslultFile.Checked; } set { checkBox_outputReslultFile.Checked = value; } }
        bool IgnoreException { get { bool check = false; this.Invoke(new Action(delegate() { check = checkBox_ignoreException.Checked; })); return check; } }
        /// <summary>
        /// 项目输出目录
        /// </summary>
        public string ProjectOutputDirectory { get { return Path.Combine(Setting.GnsserConfig.OutputDirectory, ProjectName); } }
        /// <summary>
        /// 项目名称
        /// </summary>
        string ProjectName { get { return textBox_taskName.Text; } set { textBox_taskName.Text = value; } }
        /// <summary>
        /// 是否显示进度。
        /// </summary>
        public bool IsShowProgress { get { return checkBox_showProgress.Checked; } }
        /// <summary>
        /// 是否采用手动星历
        /// </summary>
        bool IsSetEphemerisFile { get { return this.checkBox_setEphemerisFile.Checked; } set { this.checkBox_setEphemerisFile.Checked = value; } }

        #endregion
        #endregion

        #region 路径设置
        private void checkBox_setEphemerisFile_CheckedChanged(object sender, EventArgs e) { this.textBox_navPath.Enabled = IsSetEphemerisFile; this.button_getNavPath.Enabled = IsSetEphemerisFile; }
        private void textBox_obsPath_TextChanged(object sender, EventArgs e) { ShowInfo(textBox_obsPath.Lines.Length + " 个待计算观测文件" + "\r\n"); }
        /// <summary>
        /// 移除参考站
        /// </summary>
        private void BulidBaselineTaskInfo()
        {
            //List<string> colName = new List<string>();
            //foreach (var path in textBox_obsPath.Lines)
            //{
            //    if ( Path.GetFileName( path).Trim().ToUpper().Equals(Path.GetFileName( this.textBox_refObsPath.Text).Trim().ToUpper()))
            //        ShowInfo("已在流动站中移去参考站:\t" + path);
            //    else  colName.Add(path);
            //}
            //textBox_obsPath.Lines = colName.ToArray();


            //读取单基线信息文件

            string localPath = Path.GetDirectoryName(textBox_obsPath.Lines[0]);
            string localName = Path.GetFileName(textBox_obsPath.Lines[0]);


            StreamReader srApproXyz = new StreamReader(this.textBox_ApproXyzPath.Text.Trim());
            string lineApproXyz = srApproXyz.ReadLine();

            while (lineApproXyz != null)
            {
                string[] cha2 = lineApproXyz.Split(new Char[] { ' ', '\t' });
                string stationName = cha2[2].Trim().ToLower();
                XYZ xyz = new XYZ(Convert.ToDouble(cha2[5]), Convert.ToDouble(cha2[6]), Convert.ToDouble(cha2[7]));
               
                ApproXyzDictorary[stationName] =  xyz;
                lineApproXyz = srApproXyz.ReadLine();
            }


            StreamReader sr = new StreamReader(this.textBox_baseLinePath.Text.Trim());
            string line = sr.ReadLine();
            while (line != null)
            {
                string refStationName = line.Substring(0, 4).ToLower();
                string rovStationName = line.Substring(5, 4).ToLower();

                BaselineTask btask = new BaselineTask();
                btask.refStationPath = localPath + "\\" + refStationName + localName.Substring(4, localName.Length - 4);
                btask.rovStationPath = localPath + "\\" + rovStationName + localName.Substring(4, localName.Length - 4);

                //
                if (ApproXyzDictorary.ContainsKey(refStationName) || ApproXyzDictorary.ContainsKey(refStationName.ToUpper()))
                {
                    btask.refStationXyz = ApproXyzDictorary[refStationName];
                }
                if (ApproXyzDictorary.ContainsKey(rovStationName) || ApproXyzDictorary.ContainsKey(rovStationName.ToUpper()))
                {
                    btask.rovStationXyz = ApproXyzDictorary[rovStationName];
                }


                this.BaselineTaskList.Add(btask);

                line = sr.ReadLine();
            }

        }
        private void button_datasourceSeting_Click(object sender, EventArgs e) { new ComonSourceSettingForm().ShowDialog(); }
        private void PointPositionForm_Load(object sender, EventArgs e)
        {
            this.IsSetEphemerisFile = true;
            this.IsSetEphemerisFile = false;
            this.ProjectName = "差分定位_" + DateTimeUtil.GetDateTimePathStringNow();

            this.textBox_baseLinePath.Text = @"D:\Data\GNSS\Gnsser重构测试数据\欧洲区域\45条单基线.txt";// Setting.GnsserConfig.SampleOFileV3A;
            this.textBox_ApproXyzPath.Text = @"D:\Data\GNSS\Gnsser重构测试数据\欧洲区域\ppp.txt";
            this.textBox_obsPath.Text = Setting.GnsserConfig.SampleOFileV3B;
            this.textBox_navPath.Text = Setting.GnsserConfig.SampleNFileV3;
            this.textBox_ClockPath.Text = Setting.GnsserConfig.SampleClkFileV3;
        }
        private void checkBox_enableClockFile_CheckedChanged(object sender, EventArgs e) { this.textBox_ClockPath.Enabled = this.checkBox_enableClockFile.Checked; this.button_getClockPath.Enabled = this.checkBox_enableClockFile.Checked; }
        private void button_replaceObs_Click(object sender, EventArgs e) { if (this.openFileDialog_obs.ShowDialog() == System.Windows.Forms.DialogResult.OK)   this.textBox_obsPath.Lines = this.openFileDialog_obs.FileNames; }
        private void button_addObsFiles_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_obs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> list = new List<string>();
                if (this.textBox_obsPath.Lines.Length != 0)
                    list = new List<string>(this.textBox_obsPath.Lines);
                list.AddRange(this.openFileDialog_obs.FileNames);
                this.textBox_obsPath.Lines = list.ToArray();
            }
        }
        private void button_getNavPath_Click(object sender, EventArgs e) { if (this.openFileDialog_nav.ShowDialog() == System.Windows.Forms.DialogResult.OK)    this.textBox_navPath.Text = this.openFileDialog_nav.FileName; }
        private void button_getClockPath_Click(object sender, EventArgs e) { if (this.openFileDialog_clock.ShowDialog() == DialogResult.OK)  this.textBox_ClockPath.Text = this.openFileDialog_clock.FileName; }
        private void button_setRefObsPath_Click(object sender, EventArgs e) { if (this.openFileDialog_singleObs.ShowDialog() == System.Windows.Forms.DialogResult.OK)   this.textBox_baseLinePath.Text = this.openFileDialog_singleObs.FileName; }
        #endregion

        #region 计算

        #region 计算配置
        private void button_kalman_Click(object sender, EventArgs e)
        {
            #region 计算前的一些配置
            if (this.textBox_obsPath.Lines.Length == 0)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("请先读取数据。"); return;
            }
            BulidBaselineTaskInfo();

            ShowInfo("开始计算");
            this.label_fileCount.Text = "正在计算....";
            FileUtil.CheckOrCreateDirectory(ProjectOutputDirectory);

            this.button_kalman.Enabled = false;
            _processedFileCount = 0;
            this._processedEpochCount = 0;
            this._IsCanceled = false;
            this._startTime = DateTime.Now;
            this._globalInfo = new StringBuilder();

            LoadObsDatasources();

            //设置时间
            Time start = _obsDataSources[0].ObsInfo.StartTime;
            Time end = _obsDataSources[0].ObsInfo.StartTime + TimeSpan.FromHours(23.99);
            foreach (var item in _obsDataSources)
            {
                if (start > item.ObsInfo.StartTime) start = item.ObsInfo.StartTime;
                if (end < item.ObsInfo.StartTime) start = item.ObsInfo.StartTime + TimeSpan.FromHours(23.99);
            }
            this.TimePeriod = new BufferedTimePeriod(start, end);
            #endregion

            backgroundWorker1.RunWorkerAsync();
        }
        /// <summary>
        /// 载入观测数据
        /// </summary>
        private void LoadObsDatasources()
        {
            this._obsDataSources = new List<ISingleSiteObsStream>();

            StringBuilder sb = new StringBuilder();
            int errorCount = 0;
            foreach (var item in this.textBox_obsPath.Lines)
            {
                try
                {
                    _obsDataSources.Add(new RinexFileObsDataSource(item)); ;
                }
                catch (Exception ex)
                {
                    errorCount++;
                    string msg = "解析出错! " + ex.Message;
                    if (!IgnoreException)
                    {
                        MessageBox.Show(msg);
                    }
                    else sb.AppendLine(msg);
                }
            }
            string errorMsg = "成功载入观测文件 " + this._obsDataSources.Count + " 个 ";
            if (errorCount > 0)
            {
                errorMsg += errorCount + " 个载入失败:\r\n" + sb.ToString();
            }
            ShowInfo(errorMsg);
        }


        /// <summary>
        /// 基线任务
        /// </summary>
        public class BaselineTask
        {

            public List<string> Pathes { get { return new List<string> { refStationPath, rovStationPath }; } }

            /// <summary>
            /// 参考站的路径
            /// </summary>
            public string refStationPath { get; set; }
            /// <summary>
            /// 流动站的路径
            /// </summary>
            public string rovStationPath { get; set; }

            /// <summary>
            /// 参考站的概略坐标，通常由PPP计算
            /// </summary>
            public XYZ refStationXyz { get; set; }

            /// <summary>
            /// 流动站的概略坐标，通常由PPP计算
            /// </summary>
            public XYZ rovStationXyz { get; set; }

        }

        /// <summary>
        /// 近似坐标
        /// </summary>
        public SortedDictionary<string, XYZ> ApproXyzDictorary = new SortedDictionary<string, XYZ>();

        /// <summary>
        /// 基线任务列表
        /// </summary>
        public List<BaselineTask> BaselineTaskList = new List<BaselineTask>();


        RinexFileObsDataSource RefObservationDataSource;
        /// <summary>
        /// 创建单点定位
        /// </summary>
        /// <param name="obsPath">测站信息</param>
        /// <param name="startTime">起始计算时间</param>
        /// <returns></returns>
        private IGnssSolver BuildPositioner(string rovObsPath, BufferedTimePeriod startTime)
        {
            GnssProcessOption PositionOption = GetModel(startTime);

            #region 星历钟差数据源配置
            #region 星历数据配置
            FileEphemerisService ephemerisDataSource = null;
            if (IsSetEphemerisFile)
            {
                string ephemerisPath = this.textBox_navPath.Text;
                if (!File.Exists(ephemerisPath)) { throw new FileNotFoundException("指定星历文件不存在！\r\n" + ephemerisPath); }
                FileEphemerisType ephType = EphemerisDataSourceFactory.GetFileEphemerisTypeFromPath(ephemerisPath);
                ephemerisDataSource = EphemerisDataSourceFactory.Create(ephemerisPath);
            }
            #endregion
            #region 钟差数据配置
            Data.ISimpleClockService clock = null;
            if (this.checkBox_enableClockFile.Checked) clock = new Data.SimpleClockService(this.textBox_ClockPath.Text);
            #endregion
            #endregion

            #region 定位器构造


            //加载文件数据
            RefObservationDataSource = new RinexFileObsDataSource(this.textBox_baseLinePath.Text);
            var RovObservationDataSource = new RinexFileObsDataSource(rovObsPath);

            DataSourceContext context = DataSourceContext.LoadDefault(PositionOption, RovObservationDataSource, ephemerisDataSource, clock); 
            IonFreeDoubleDifferPositioner pp = new IonFreeDoubleDifferPositioner(context, PositionOption);

            //pp.Produced += pp_ProgressIncreased;
            return pp;
            #endregion
        }

        /// <summary>
        /// 创建单点定位
        /// </summary>
        /// <param name="obsPath">测站信息</param>
        /// <param name="startTime">起始计算时间</param>
        /// <returns></returns>
        private IonFreeDoubleDifferPositioner BuildPositioner(string obsPath, RinexFileObsDataSource refStationPath, BufferedTimePeriod startTime)
        {
            GnssProcessOption PositionOption = GetModel(startTime);

            #region 星历钟差数据源配置
            #region 星历数据配置
            FileEphemerisService ephemerisDataSource = null;
            if (IsSetEphemerisFile)
            {
                string ephemerisPath = this.textBox_navPath.Text;
                if (!File.Exists(ephemerisPath)) { throw new FileNotFoundException("指定星历文件不存在！\r\n" + ephemerisPath); }
                FileEphemerisType ephType = EphemerisDataSourceFactory.GetFileEphemerisTypeFromPath(ephemerisPath);
                ephemerisDataSource = EphemerisDataSourceFactory.Create(ephemerisPath);
            }
            #endregion
            #region 钟差数据配置
            Data.ISimpleClockService clock = null;
            if (this.checkBox_enableClockFile.Checked) clock = new Data.SimpleClockService(this.textBox_ClockPath.Text);
            #endregion
            #endregion

            #region 定位器构造


            //加载文件数据
            RinexFileObsDataSource refObservationDataSource = (refStationPath);

            var rovObservationDataSource = new RinexFileObsDataSource(obsPath);

            DataSourceContext context = DataSourceContext.LoadDefault(PositionOption, rovObservationDataSource, ephemerisDataSource, clock);
            IonFreeDoubleDifferPositioner pp = new IonFreeDoubleDifferPositioner(context, PositionOption);
             
            //pp.Produced += pp_ProgressIncreased;
            return pp;
            #endregion
        }




        /// <summary>
        /// 计算选项。!!此处采用同一的解算模型。如果多系统解算，需要修改。
        /// </summary>
        /// <returns></returns>
        private GnssProcessOption GetModel(BufferedTimePeriod startTime)
        {
            GnssProcessOption model = new GnssProcessOption()
            {
                FilterCourceError = checkBox_ignoreCourceError.Checked,
                CaculateType = CaculateType.Filter,
                MaxStdDev = double.Parse(this.textBox_maxStd.Text),
                EnableClockService = this.checkBox_enableClockFile.Checked,
                SatelliteTypes = this.multiGnssSystemSelectControl1.SatelliteTypes
            };
            return model;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (parallelConfigControl1.EnableParallel) ParallelProcess();
            else SerialProcess();
        }
        #endregion

        #region 并行计算

        private void ParallelProcess()
        {
            _positioners.Clear();

            ////Parallel.For(0, obsDataSources.Count, this.parallelConfigControl1.ParallelOptions, (time, state) =>
            //Parallel.ForEach(_obsDataSources, this.parallelConfigControl1.ParallelOptions, (obsData, state) =>
            //{
            //    try
            //    {
            //        //构建
            //        IPointPositioner PointPositioner = BuildPositioner(obsData.obsPath, TimePeriod);
            //        _positioners.Add(PointPositioner);
            //        //计算
            //        List<PositionResult> colName = PointPositioner.Gets(obsData);

            //        //输出
            //        if (IsOutputResultFile)
            //           SaveToDirectory(colName, obsData);

            //        //通知
            //        _processedFileCount++;
            //        ShowProcessedFileCount(_processedFileCount);

            //        //释放资源
            //        _positioners.Remove(PointPositioner);
            //        colName.ForEach(m => m.Dispose());
            //        colName = null;

            //        //是否终止计算
            //        if (_IsCanceled || PointPositioner.IsCancel) state.Break();
            //    }
            //    catch (Exception ex)
            //    {
            //        if (!IgnoreException)  FormUtil.ShowErrorMessageBox("并行计算错误！" + ex.Message);
            //    }
            //});

            //Parallel.For(0, obsDataSources.Count, this.parallelConfigControl1.ParallelOptions, (time, state) =>
            Parallel.ForEach(BaselineTaskList, this.parallelConfigControl1.ParallelOptions, (bTask, state) =>
            {
                try
                {
                    var obsData = new MultiSiteObsStream(bTask.Pathes,  BaseSiteSelectType.GeoCenter,  true);

                    RinexFileObsDataSource refObservationDataSource = new RinexFileObsDataSource(bTask.refStationPath);

                    if (bTask.refStationXyz != null || bTask.refStationXyz.Norm != 0)
                    {
                        obsData.OtherDataSource.SiteInfo.SetApproxXyz(bTask.refStationXyz); 
                    }

                    if (bTask.rovStationXyz != null || bTask.rovStationXyz.Norm != 0)
                    {
                        obsData.BaseDataSource.SiteInfo.SetApproxXyz(bTask.rovStationXyz);
                    }

                    //构建
                    var pp = BuildPositioner(bTask.rovStationPath, refObservationDataSource, TimePeriod);
                    _positioners.Add(bTask);
                    //计算
                    List<BaseGnssResult> list = new List<BaseGnssResult>();
                    foreach (var item in obsData)
                    {
                        list.Add( pp.Get(item));
                    }

                    //输出
                    if (IsOutputResultFile)
                        SaveToFile(list);

                    //通知
                    _processedFileCount++;
                    ShowProcessedFileCount(_processedFileCount);

                    //释放资源
                    _positioners.Remove(bTask);
                    list.ForEach(m => m.Dispose());
                    list = null;

                    //是否终止计算
                    if (_IsCanceled) state.Break();
                }
                catch (Exception ex)
                {
                    if (!IgnoreException) FormUtil.ShowErrorMessageBox("并行计算错误！" + ex.Message);
                }
            });


        }
        #endregion

        #region 串行计算
        private void SerialProcess()
        {
            //foreach (var path in _obsDataSources)
            //{
            //    try
            //    {
            //        //构建
            //        _curentPositioner = BuildPositioner(path.obsPath, TimePeriod);

            //        //计算
            //        List<PositionResult> results = _curentPositioner.Gets(path);

            //        //输出
            //        if (IsOutputResultFile)
            //            SaveToDirectory(results, path);

            //        //通知
            //        _processedFileCount++;
            //        ShowProcessedFileCount(_processedFileCount);

            //        //释放资源
            //        results.ForEach(m => m.Dispose());
            //        results = null;

            //        //是否终止计算
            //        if (_curentPositioner.IsCancel || _IsCanceled) break;
            //    }
            //    catch (Exception ex)
            //    {
            //        if (!IgnoreException)   FormUtil.ShowErrorMessageBox("计算出错了！" + ex.Message);  
            //    }
            //}

            foreach (var bTask in BaselineTaskList)
            {
                try
                {
                    ISingleSiteObsStream item = new RinexFileObsDataSource(bTask.rovStationPath);

                    RinexFileObsDataSource refObservationDataSource = new RinexFileObsDataSource(bTask.refStationPath);

                    if (bTask.refStationXyz != null || bTask.refStationXyz.Norm != 0)
                    {
                        refObservationDataSource.SiteInfo.SetApproxXyz( bTask.refStationXyz);
                    }
                    if (bTask.rovStationXyz != null || bTask.rovStationXyz.Norm != 0)
                    {
                        item.SiteInfo.SetApproxXyz( bTask.rovStationXyz);
                    }
                    //构建
                    _curentPositioner = BuildPositioner(bTask.rovStationPath, refObservationDataSource, TimePeriod);
                    //计算
                    List<BaseGnssResult> results = null;// _curentPositioner.Gets();

                    //输出
                    if (IsOutputResultFile)
                        SaveToFile(results);

                    //通知
                    _processedFileCount++;
                    ShowProcessedFileCount(_processedFileCount);

                    //释放资源
                    results.ForEach(m => m.Dispose());
                    results = null;

                    //是否终止计算
                    //if (_curentPositioner.IsCancel || _IsCanceled) break;
                }
                catch (Exception ex)
                {
                    if (!IgnoreException) FormUtil.ShowErrorMessageBox("计算出错了！" + ex.Message);
                }
            }
        }


        #endregion

        #endregion

        #region 计算状态控制
        void pp_ProgressIncreased(BaseGnssResult e, EpochInformation sender)
        {
            _processedEpochCount++;
            this.Invoke(new Action(delegate()
            {
                this.backgroundWorker1.ReportProgress(_processedEpochCount);
            }));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowResult();

            this.Invoke(new Action(delegate()
            {
                this.button_kalman.Enabled = true;
                this.label_fileCount.Text = "处理完毕！";
            }));

            string msg = "计算完毕！" + "处理了 " + _processedFileCount + "  个文件," + _processedEpochCount + "个历元！";
            ShowInfo(msg);
            msg += "\r\n" + "是否打开项目输出目录？";
            FormUtil.ShowIfOpenDirMessageBox(ProjectOutputDirectory, msg);
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (IsShowProgress)
                this.Invoke(new Action(delegate()
                {
                    this.label_progress.Text = e.ProgressPercentage + "";
                }));
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this._IsCanceled = true;

            //if (_positioners.Count > 0)
            //{
            //    foreach (var key in _positioners) key.IsCancel = true;
            //}
            //if (_curentPositioner != null)
            //{
            //    _curentPositioner.IsCancel = true;
            //}
            backgroundWorker1.CancelAsync();

            this.ShowInfo("已请求停止计算");
        }

        private void PointPositionForm_FormClosing(object sender, FormClosingEventArgs e) { if (backgroundWorker1.IsBusy)  if (FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No)     e.Cancel = true; }

        #endregion

        #region 显示结果，输出文件
        /// <summary>
        /// 显示已经处理的文件数量
        /// </summary>
        /// <param name="time"></param>
        private void ShowProcessedFileCount(int i)
        {
            this.Invoke(new Action(delegate()
            {
                this.label_fileCount.Text = "已经处理完基线：" + i + "/" + _obsDataSources.Count;
            }));
        }
        /// <summary>
        /// 在文本输入框提示信息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowInfo(string msg)
        {
            this.Invoke(new Action(delegate()
            {
                this.textBox_result.Text += DateTimeUtil.GetFormatedDateTimeNow() + ":\t" + msg + "\r\n";
            }));
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="PositionResults"></param>
        /// <param name="path"></param>
        private void SaveToFile(List<BaseGnssResult> _results)
        {
            if (_results == null || _results.Count == 0) return;
            //BaseGnssResult last = _results[_results.Count - 1];
            //PointPositionResultWriter writer = new PointPositionResultWriter(ProjectOutputDirectory, this.checkBox_outputAdjust.Checked);
            //writer.Write(_results);
            //string msg = last.SiteInfo.MarkerName + "\t"
            //      + last.Time + "\t"
            //      + last.EstimatedXyz.GetTabValues() + "\t"
            //      + last.CorrectedXyz.GetTabValues();
            //ShowInfo(msg);
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="PositionResults"></param>
        /// <param name="path"></param>
        //private void SaveToFile(List<GnssResult> _results)
        //{
        //    //if (_results == null || _results.Count == 0) return;
        //    //GnssResult last = _results[_results.Count - 1];
        //    //PointPositionResultWriter writer = new PointPositionResultWriter(ProjectOutputDirectory, this.checkBox_outputAdjust.Checked);
        //    //writer.Write(_results, refStationObsDataInformation);
        //    //string msg = refStationObsDataInformation.SiteInfo.MarkerName + "\t" +
        //    //    refStationObsDataInformation.SiteInfo.ApproxXyz.GetTabValues() + "\t" +
        //    //     last.SiteInfo.MarkerName + "\t"
        //    //      + last.SiteInfo.ApproxXyz.GetTabValues() + "\t"
        //    //   + last.SiteInfo.MarkerName + "\t"
        //    //      + last.GpsTime + "\t"
        //    //      + last.EstimatedXyz.GetTabValues() + "\t"
        //    //      + last.CorrectedXyz.GetTabValues();
        //    //ShowInfo(msg);
        //}

        /// <summary>
        /// 显示计算结果
        /// </summary>
        private void ShowResult()
        {
            TimeSpan span = DateTime.Now - _startTime;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("耗时：" + DateTimeUtil.GetFloatString(span));
            sb.AppendLine("计算完毕！工程输出目录：" + this.ProjectOutputDirectory);
            sb.AppendLine(_globalInfo.ToString());

            FormUtil.InvokeTextBoxSetText(textBox_result, sb.ToString(), true);
        }
        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            List<ISingleSiteObsStream> obsDataSources = RinexFileObsDataSource.LoadObsData(this.textBox_obsPath.Lines);
            List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();
            foreach (var item in obsDataSources) pts.Add(new AnyInfo.Geometries.Point(item.SiteInfo.ApproxGeoCoord, null, item.SiteInfo.SiteName));
            if (ShowLayer != null && pts.Count != 0) { ShowLayer(LayerFactory.CreatePointLayer(pts)); }
        }
        private void button_openProjDir_Click(object sender, EventArgs e) { FileUtil.CheckOrCreateDirectory(ProjectOutputDirectory); FileUtil.OpenDirectory(this.ProjectOutputDirectory); }
        #endregion

        private void checkBox_outputReslultFile_CheckedChanged(object sender, EventArgs e) { this.checkBox_outputAdjust.Enabled = this.checkBox_outputReslultFile.Checked; }

        private void button_setApproXyzPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_approXyzFile.ShowDialog() == System.Windows.Forms.DialogResult.OK) { this.textBox_ApproXyzPath.Text = this.openFileDialog_approXyzFile.FileName; }
        }
    }
}
