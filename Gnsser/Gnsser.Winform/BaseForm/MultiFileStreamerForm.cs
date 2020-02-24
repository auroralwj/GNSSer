//2016.03.29, czs, create in hongqing, 具有日志监听的窗口
//2016.08.15, czs, edit in hongqing, 多测站多历元数据流
//2017.02.06, czs, edit in hongqing, 更名为MultiFileStreamerForm多文件数据流处理窗口

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Geo.Winform.Controls;

namespace Gnsser.Winform
{
    /// <summary>
    /// 多文件数据流处理窗口
    /// </summary>
    public partial class MultiFileStreamerForm : Geo.Winform.LogListenerForm
    {
        protected new ILog log = Log.GetLog(typeof(MultiFileStreamerForm));

        /// <summary>
        /// 构造函数
        /// </summary> 
        public MultiFileStreamerForm()
        {
            InitializeComponent();
            this.IsEnableLogShowing = false;

            //this.tabControl_input.SelectedTab = this.tabPage3;
            this.IsShowProgressBar = false;

            ExtendInputTabPages = new List<TabPage>(){
                this.tabPage_InputExtend1,
                this.tabPage_InputExtend2,
            };
            ExtendOutputTabPages = new List<TabPage>()
            {
                tabPage_OutputExtendText1,
                tabPage_OutputExtendText2,
                tabPage_OutputExtendTable1,
                tabPage_OutputExtendTable2,
                tabPage_OutputExtendTable3,
                tabPage_OutputExtendTable4,
            };
            OutputTextBoxes = new List<RichTextBoxControl>()
            {
                this.richTextBoxControl_outputExtend1,
                this.richTextBoxControl_outputExtend2,
            };
            RunningFileExtension = "*.*O;*.rnx";

        } 
        private void button_cancel_Click(object sender, EventArgs e)
        {
            OnProcessCommandChanged(ProcessCommandType.Cancel);
            SetRunable(false);
            this.backgroundWorker1.CancelAsync();
        }


        #region 属性
        public event OptionChangedEventHandler OptionChanged;

        /// <summary>
        /// 命令改变
        /// </summary>
        public event ProcessCommandChangedEventHandler ProcessCommandChanged;

        /// <summary>
        /// 并行计算选项
        /// </summary>
        public IParallelConfig ParallelConfig { get { return parallelConfigControl1; } }
        /// <summary>
        /// 处理命令改变了。
        /// </summary>
        /// <param name="type"></param>
        protected virtual void OnProcessCommandChanged(ProcessCommandType type) { if (ProcessCommandChanged != null) { ProcessCommandChanged(type); } }
        /// <summary>
        /// 选项
        /// </summary>
        protected GnssProcessOption Option { get; set; }

        /// <summary>
        /// 卫星系统类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get { return multiGnssSystemSelectControl1.SatelliteTypes; } }

        protected virtual void OnOptionChanged(GnssProcessOption Option) { if (OptionChanged != null) { OptionChanged(Option); } }
         
        /// <summary>
        /// 起始时间
        /// </summary>
        protected DateTime StartTime { get; private set; }
        protected List<RichTextBoxControl> OutputTextBoxes { get; private set; }
        protected List<TabPage> ExtendInputTabPages { get; private set; }
        protected List<TabPage> ExtendOutputTabPages { get; private set; }
        /// <summary>
        /// 是否显示进度条。
        /// </summary>
        public bool IsShowProgressBar { get { return this.progressBarComponent1.Visible; } set { this.progressBarComponent1.Visible = value; } }
        /// <summary>
        /// 是否已经关闭窗口
        /// </summary>
        public bool IsClosed { get; set; }
        /// <summary>
        /// 是否取消计算
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// 在界面显示数据
        /// </summary>
        public bool IsShowData { get { return checkBox_showData.Checked; } }
        /// <summary>
        /// 是否显示信息
        /// </summary>
        public bool IsShowInfo { get { return checkBox1_enableShowInfo.Checked; } }
        /// <summary>
        /// 是否显示信息
        /// </summary>
        public bool IsShowProcessInfo { get { return checkBox1_enableShowInfo.Checked; } }
        /// <summary>
        /// 进度条
        /// </summary>
        protected Geo.Winform.Controls.ProgressBarComponent ProgressBar { get { return progressBarComponent1; } }
        /// <summary>
        /// 设置是否启用详细设置按钮。
        /// </summary>
        /// <param name="trueOrFalse"></param>
        public void SetEnableDetailSettingButton(bool trueOrFalse) { this.button_detailSetting.Visible = trueOrFalse; }
         
        /// <summary>
        /// 运行文件的后缀名
        /// </summary>
        public string RunningFileExtension { get; set; } 
        #endregion

        #region 方法
        public void SetFilePathes(string [] pathes)
        {
            this.fileOpenControl_inputPathes.SetFilePahtes(pathes);
        }
        /// <summary>
        /// 设置是否启用多系统卫星系统。
        /// </summary>
        /// <param name="trueOrFalse"></param>
        protected void SetEnableMultiSysSelection(bool trueOrFalse)
        {
            this.multiGnssSystemSelectControl1.Visible = trueOrFalse;
        }
        /// <summary>
        /// 表格绑定1
        /// </summary>
        /// <param name="table"></param>
        public void BindTableA(ObjectTableStorage table)
        {
            if (!CheckTableSizeForShowing(table)) { return; }

            this.Invoke(new Action(delegate()
            {
                tabPage_OutputExtendTable1.Text = table.Name;
                this.objectTableControl_param.DataBind(table); 
            }));
        }

        /// <summary>
        /// 表格绑定2
        /// </summary>
        /// <param name="table"></param>
        public void BindTableB(ObjectTableStorage table)
        {
            if (!CheckTableSizeForShowing(table)) { return; }

            this.Invoke(new Action(delegate ()
            {
                tabPage_OutputExtendTable2.Text = table.Name;
                this.objectTableControl_rms.DataBind(table);
            }));
        }
        /// <summary>
        /// 表格绑定3
        /// </summary>
        /// <param name="table"></param>
        public void BindTableC(ObjectTableStorage table)
        {
            if (!CheckTableSizeForShowing(table)){ return; } 

            this.Invoke(new Action(delegate ()
            {
                tabPage_OutputExtendTable3.Text = table.Name;
                this.objectTableControl_extesion.DataBind(table);
            }));
        }
        /// <summary>
        /// 表格绑定D
        /// </summary>
        /// <param name="table"></param>
        public void BindTableD(ObjectTableStorage table)
        {
            if (!CheckTableSizeForShowing(table)){ return; } 

            this.Invoke(new Action(delegate ()
            {
                tabPage_OutputExtendTable4.Text = table.Name;
                this.objectTableControl_extesion2.DataBind(table);
            }));
        }
        /// <summary>
        /// 表格大小检查
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private bool CheckTableSizeForShowing(ObjectTableStorage table)
        {
            if (table.ColCount > Setting.MaxTableColCount || table.RowCount > Setting.MaxTableRowCount) { Geo.Utils.FormUtil.ShowWarningMessageBox("界面表格最大显示数为" + Setting.MaxTableColCount + "列，" + Setting.MaxTableRowCount + "行，目前已超出，请直接查看输出文件。"); return false; }
            return true;
        }

        /// <summary>
        /// 设置是否为多文件。
        /// </summary>
        /// <param name="trueOrFalse"></param>
        public void SetIsMultiObsFile(bool trueOrFalse)
        {
            this.fileOpenControl_inputPathes.IsMultiSelect = trueOrFalse;
            if (!trueOrFalse)
            {
                this.fileOpenControl_inputPathes.Top = 5;
                this.fileOpenControl_inputPathes.Dock = DockStyle.Top;
                this.fileOpenControl_inputPathes.Height = 25;

                this.directorySelectionControl1.Top = 5;
                this.directorySelectionControl1.Dock = DockStyle.Top;
                this.directorySelectionControl1.Height = 25;
            }
        }
        /// <summary>
        /// 默认生成方法
        /// </summary>
        /// <returns></returns>
        protected virtual GnssProcessOption CheckOrBuildGnssOption()
        {
            if (Option != null) { return Option; }

            this.Option = CreateOption();
            this.UiToOption();//第一次将界面上的读入
            return Option;
        }

        /// <summary>
        /// 不指定类型的第一次创建
        /// </summary>
        /// <returns></returns>
        protected virtual GnssProcessOption CreateOption()
        {
            return  new GnssProcessOption();
        }

        /// <summary>
        /// 构建选项
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual GnssProcessOption CheckOrBuildGnssOption(GnssSolverType type)
        {
            if (Option != null) { return Option; }

            Option = GnssProcessOptionManager.Instance.Get(type);

            this.UiToOption();//第一次将界面上的读入
            Option.GnssSolverType = type;
            return Option;
        }
        protected virtual GnssProcessOption CheckOrBuildGnssOption(RinexObsFileFormatType type)
        {
            if (Option != null) { return Option; }

            Option =   GnssProcessOptionManager.Instance.Get(type);

            this.UiToOption();//第一次将界面上的读入
            Option.RinexObsFileFormatType = type;
            return Option;
        }
        /// <summary>
        /// 响应界面
        /// </summary>
        protected virtual void DetailSetting()
        { 
            this.CheckOrBuildGnssOption();
            this.UiToOption();

            OptionVizardForm form = new OptionVizardForm(Option);
            form.Init();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Option = form.Option; 
                this.directorySelectionControl1.Path = this.Option.OutputDirectory;

                multiGnssSystemSelectControl1.SetSatelliteTypes(this.Option.SatelliteTypes);
                OptionToUi(Option);

                OnOptionChanged(this.Option);
            }
        }

        protected virtual void OptionToUi(GnssProcessOption Option)
        {
            multiGnssSystemSelectControl1.SetSatelliteTypes( Option.SatelliteTypes);
            //this.fileOpenControl1.FilePathes = Option.ObsFiles.ToArray();
           // this.OutputDirectory= this.Option.OutputDirectory;
        }
        protected virtual void UiToOption()
        {
            this.CheckOrBuildGnssOption();

            Option.SatelliteTypes = multiGnssSystemSelectControl1.SatelliteTypes;
             Option.ObsFiles = new List<string>( this.fileOpenControl_inputPathes.FilePathes );
            this.Option.OutputDirectory = this.OutputDirectory;
        }
        private void PointPositionForm_Load(object sender, EventArgs e)
        {
            this.directorySelectionControl1.Path = Setting.TempDirectory;

            this.fileOpenControl_inputPathes.FilePath = BuildInitPathString();

            checkBox1_enableShowInfo.Checked = Setting.IsShowInfo;
        }
        /// <summary>
        /// 构建初始路径
        /// </summary>
        /// <returns></returns>
        protected virtual  string BuildInitPathString()
        {
            if (Setting.GnsserConfig != null)
            {
                return Setting.GnsserConfig.SampleOFileA + "\r\n" +
                    Setting.GnsserConfig.SampleOFileB;
            }
            return "";
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowNotice(string msg)
        {
            if (this.IsClosed || !this.IsShowInfo) { return; }

            FormUtil.ShowNotice(this.label_notice, msg);
        }


        /// <summary>
        /// 在输出框显示信息。
        /// </summary>
        /// <param name="msg"></param>
        protected override void ShowInfo(string msg)
        {
            if (this.IsClosed || !IsShowInfo) { return; }
            FormUtil.InsertLineWithTimeToTextBox(this.RichTextBoxControl_processInfo, msg);
        }
        /// <summary>
        /// 窗口即将关闭
        /// </summary>
        protected virtual void FormWillClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void LogListeningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormWillClosing(sender, e);

            if (backgroundWorker1.IsBusy && FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No) {   e.Cancel = true;  }
            else  { IsClosed = true; }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroudDoWork();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroudWorkCompleted();
        }
        protected void button_solve_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("后台执行程序正忙！没有空！等忙完再来吧。"); return;
            }

            Init();

            PreRun();

            //Running
            this.backgroundWorker1.RunWorkerAsync();


            SetRunable(true);
        }

        /// <summary>
        /// 初始化工作，如进度条置 0，设置起始时间等。
        /// </summary>
        protected virtual void Init()
        {
            this.ProgressBar.SetCurrentPercessValue(0);
            StartTime = DateTime.Now;
            this.UiToOption();//界面更新
        }

        /// <summary>
        /// 运行前执行。在主线程中,避免其他线程访问出错。
        /// </summary>
        public virtual void PreRun()
        {

        }

        /// <summary>
        /// 运行后执行。在主线程中,避免其他线程访问出错。
        /// </summary>
        public virtual void Complete()
        {

        }


        /// <summary>
        /// 设置是否可以运行
        /// </summary>
        /// <param name="runable"></param>
        protected virtual void SetRunable(bool runable)
        {
            this.IsCancel = !runable;
            this.button_solve.Enabled = !runable;
            if (IsCancel)
            {
                button_solve.Text = "开始计算";
                button_solve.BackColor = Color.Chartreuse;
            }
            else
            {
                button_solve.Text = "正在用力算";
                button_solve.BackColor = Color.OrangeRed;
            }
            this.button_cancel.Enabled = runable;
            if (IsCancel) { backgroundWorker1.CancelAsync(); }
        }

        #endregion

        #region 子类待重写
        /// <summary>
        /// 输入文件类型
        /// </summary>
        protected string InputFileFilter { get { return this.fileOpenControl_inputPathes.Filter; } set { this.fileOpenControl_inputPathes.Filter = value; } }

        protected virtual void BackgroudWorkCompleted()
        {
            Complete();

            SetRunable(false);

            var msg = BuildFinalInfo();
           //  log.Info(msg);
            log.Fatal(msg);

            if (Geo.Utils.FormUtil.ShowYesNoMessageBox(msg + "\r\n是否打开输出目录？") == System.Windows.Forms.DialogResult.Yes)
            {
                Geo.Utils.FileUtil.OpenDirectory(this.directorySelectionControl1.Path);
            }

            this.ProgressBar.Full();
        }
        /// <summary>
        /// 双差
        /// </summary>
        protected virtual void DoubleDifferBackgroudWorkCompleted()
        {
            SetRunable(false);

            var msg = DoubleDifferBuildFinalInfo();
            //  log.Info(msg);
            log.Fatal(msg);

            if (Geo.Utils.FormUtil.ShowYesNoMessageBox(msg + "\r\n是否打开输出目录？") == System.Windows.Forms.DialogResult.Yes)
            {
                Geo.Utils.FileUtil.OpenDirectory(this.directorySelectionControl1.Path);
            }

            this.ProgressBar.Full();
        }
        /// <summary>
        /// 构建双差的最后的提示信息。
        /// </summary>
        /// <returns></returns>
        protected virtual string DoubleDifferBuildFinalInfo()
        {
            int count = TotalPathes.Count;
            var span = DateTime.Now - StartTime;
            var avrPerFile = span.TotalSeconds / (count - 1);
            var msg = "后台执行完成！耗时：" + span.ToString() + "。" + "共 " + span.TotalMinutes.ToString("0.0000") + " 分\r\n";
            msg += "文件数量 " + count + ", 每条基线平均耗时 " + avrPerFile.ToString("0.00") + " 秒\r\n";
            msg += "占用内存 " + Geo.Utils.ProcessUtil.GetProcessUsedMemoryString();
            return msg;
        }


        /// <summary>
        /// 构建最后的提示信息。
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildFinalInfo()
        {
            int count = TotalPathes.Count;
            var span = DateTime.Now - StartTime;
            var avrPerFile = span.TotalSeconds / count;
            var msg = "后台执行完成！耗时：" + span.ToString() + "。" + "共 " + span.TotalMinutes.ToString("0.0000") + " 分\r\n";
            msg += "文件数量 " + count + ", 每个文件平均耗时 " + avrPerFile.ToString("0.00") + " 秒\r\n";
            msg += "占用内存 " + Geo.Utils.ProcessUtil.GetProcessUsedMemoryString();
            return msg;
        }
        /// <summary>
        /// 文件输入管理器
        /// </summary>
        protected InputFileManager InputFileManager { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        protected List<string> TotalPathes { get; set; }
        /// <summary>
        ///输入的原始地址
        /// </summary>
        protected string [] InputRawPathes { get =>( this.fileOpenControl_inputPathes.FilePathes); }
        /// <summary>
        /// 后台运行。
        /// </summary>
        protected virtual void BackgroudDoWork()
        {
            TotalPathes = ParseInputPathes();
            log.Info("解析获取了 " + TotalPathes.Count + " 个文件");
            Run(TotalPathes.ToArray());
        }

        protected List<string> ParseInputPathes()
        {
            return ParseInputPathes(this.fileOpenControl_inputPathes.FilePathes);
        }

        /// <summary>
        /// 解析输入路径
        /// </summary>
        protected virtual List<string> ParseInputPathes(string[] inputPathes)
        {
            this.InputFileManager = new Geo.IO.InputFileManager();
            TotalPathes = (InputFileManager.GetLocalFilePathes(inputPathes, RunningFileExtension));
            return TotalPathes;
        }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get { return this.directorySelectionControl1.Path; } set { directorySelectionControl1.Path = value; } }

        /// <summary>
        ///  运行所有输入
        /// </summary>
        /// <param name="inputPathes"></param> 
        protected virtual void Run(string[] inputPathes)
        {
            if (inputPathes == null && inputPathes.Length == 0)
            {
                return;
            }
            Run(inputPathes[0]);
        }
        /// <summary>
        /// 运行。
        /// </summary>
        /// <param name="inputPath"></param>
        protected virtual void Run(string inputPath)
        {
            if (!File.Exists(inputPath)) { Geo.Utils.FormUtil.ShowFileNotExistBox(inputPath); return; }
        }
        #endregion

        #region 显示和日志控制
        private void checkBox1_enableShowInfo_CheckedChanged(object sender, EventArgs e) { Setting.IsShowInfo = checkBox1_enableShowInfo.Checked; }
        #endregion

        private void button_detailSetting_Click(object sender, EventArgs e)
        {
            DetailSetting();
        }
        /// <summary>
        /// 启用或禁止页面。
        /// </summary>
        /// <param name="inOrOut">输出或输出页面</param>
        /// <param name="index">编号，从0开始，除了基本4个，进出各2个</param>
        /// <param name="enabled">启用否</param>
        /// <param name="title">名称</param>
        public void EnableExtendTabPage(bool inOrOut, int index, bool enabled, string title = "")
        {
            var TabControl = inOrOut ? tabControl_input : tabControl_output;
            var page = inOrOut ? ExtendInputTabPages[index] : ExtendOutputTabPages[index];
            if (String.IsNullOrWhiteSpace(title)) { page.Text = title; }
            page.Parent = enabled ? TabControl : null;
        }
        /// <summary>
        /// 直接设置到输出文本框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="indexOfOuputTextBox"></param>
        public void ShowResult(string text, int indexOfOuputTextBox)
        {
            if (IsShowData)
            OutputTextBoxes[indexOfOuputTextBox].Text = text;
        }
        /// <summary>
        /// 同时指定输入和输出扩展面板的数量
        /// </summary>
        /// <param name="inputCount"></param>
        public void SetExtendTabPageCount(int inputCount, int outputCount) { SetExtendTabPageCount(inputCount, true); SetExtendTabPageCount(outputCount, false); }
        /// <summary>
        /// 设置扩展面板的数量。自动禁用超出指定数量的面板。
        /// </summary>
        /// <param name="enableCount"></param>
        /// <param name="inOrOut"></param>
        public void SetExtendTabPageCount(int enableCount, bool inOrOut)
        {
            var pages = inOrOut ? ExtendInputTabPages : ExtendOutputTabPages;
            int disableCount = pages.Count - enableCount;
            //从后往前开始禁用。
            for (int i = pages.Count - 1, tabCount = 0; i >= 0 && tabCount < disableCount; i--, tabCount++)
            {
                EnableExtendTabPage(inOrOut, i, false);
            }
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            //log.Info("载入 " +  fileOpenControl1.FilePathes.Length+ " 个项目！");//不重复载入
            var path = this.fileOpenControl_inputPathes.FilePath;
            if (Geo.Utils.FileUtil.IsValid(path))
            {
                this.OutputDirectory = Path.Combine(Path.GetDirectoryName(path), Setting.GnsserTempSubDirectoryName);
            }
        }

        private void checkBox_showData_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}