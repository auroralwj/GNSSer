//2018.12.12, czs, create in hmx, 基线网解算综合窗口


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
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
using Geo.Winform;
using System.Threading;
using Geo.Draw;
using System.IO;

namespace Gnsser.Winform
{
    public partial class AdvancedBaseLineNetSolverForm : Form, IShowLayer
    {
        Log log = new Log(typeof(BaseLineNetSolverForm));
        public event ShowLayerHandler ShowLayer;
        public AdvancedBaseLineNetSolverForm()
        {
            InitializeComponent();
            this.GetOrCreateOption();
            this.Init();
        }
        public AdvancedBaseLineNetSolverForm(string projectDirectory)
        {
            InitializeComponent();
            this.GetOrCreateOption().OutputDirectory = projectDirectory;

            this.Init();
        }

        private void Init()
        {
            checkBox_enableNet.Checked = Setting.EnableNet;
            this.openFileDialog1_rinexOFile.Filter = Setting.RinexOFileFilter;
            this.enumRadioControl_selectLineType.Init<IndependentLineSelectType>();
            this.enumRadioControl1_GnssSolverType.Init<TwoSiteSolverType>();
            this.enumRadioControl1_GnssSolverType.IsReady = false;
            this.enumRadioControl_obsType.Init<ObsPhaseType>();
            this.EntityToUi();
            this.enumRadioControl1_GnssSolverType.IsReady = true;

            ObsFileManager = new ObsSiteFileManager(TimeSpan.FromSeconds(120));
            BaselineManager = new SiteObsBaselineManager();
        }
        #region 属性
        /// <summary>
        /// 项目名称
        /// </summary>
        string ProjectName => namedStringControl_projName.GetValue();
        /// <summary>
        /// 文件名称生成器
        /// </summary>
        ResultFileNameBuilder resultFileNameBuilder => new ResultFileNameBuilder(SolverDirectory);
        /// <summary>
        /// 测站管理器
        /// </summary>
        ObsSiteFileManager ObsFileManager { get; set; }
        /// <summary>
        /// 基线管理器
        /// </summary>
        SiteObsBaselineManager BaselineManager { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        GnssProcessOption Option { get; set; }
        /// <summary>
        /// 当前计算目录
        /// </summary>
        public string SolverDirectory => Path.Combine(ProjectDirectory, this.GetOrCreateOption().GnssSolverType.ToString());
        /// <summary>
        /// 工程目录
        /// </summary>
        public string ProjectDirectory { get => directorySelectionControl1_projDir.Path; set => directorySelectionControl1_projDir.Path = value; }
        /// <summary>
        /// 是否已经初始化完毕，可以联动显示
        /// </summary>
        private bool IsInited { get; set; }

        /// <summary>
        /// 并行计算选项
        /// </summary>
        public IParallelConfig ParallelConfig { get { return parallelConfigControl1; } }
        #endregion

        #region  基本操作
        public void UiToEntity()
        {
            this.Option.OutputDirectory = ProjectDirectory;
            this.Option .ObsPhaseType = this.enumRadioControl_obsType.GetCurrent<ObsPhaseType>();
            this.Option.GnssSolverType = this.enumRadioControl1_GnssSolverType.GetCurrent<GnssSolverType>();
        }


        public void EntityToUi()
        {
            //工程目录以界面为准
            //ProjectDirectory = this.Option.OutputDirectory;

            this.enumRadioControl_obsType.SetCurrent<ObsPhaseType>(this.Option.ObsPhaseType);
            this.enumRadioControl1_GnssSolverType.SetCurrent(this.Option.GnssSolverType);
        }
        private void BaseLineNetSolverForm_Load(object sender, EventArgs e)
        {
            this.namedStringControl_projName.SetValue("Net");
        }

        private void listBox_vector_DoubleClick(object sender, EventArgs e)
        {
            打开历元参数文件PToolStripMenuItem_Click(sender, e);
        }

        private void listBox_site_DoubleClick(object sender, EventArgs e)
        {
            this.打开当前文件FToolStripMenuItem_Click(sender, e);
        }
        private void checkBox_enableNet_CheckedChanged(object sender, EventArgs e) { Setting.EnableNet = checkBox_enableNet.Checked; }

        private void enumRadioControl1_GnssSolverType_EnumItemSelected(string arg1, bool arg2)
        {
            if (!arg2) { return; }
            var SolverType = (GnssSolverType)Enum.Parse(typeof(GnssSolverType), arg1);
            // var SolverType = GnssSolverTypeHelper.GetGnssSolverType(enumRadioControl1_GnssSolverType.CurrentdType);
            if (Option == null ||
                (Option.GnssSolverType != SolverType
                && Geo.Utils.FormUtil.ShowYesNoMessageBox(" 已选择 “" + arg1 + "”，是否加载默认其设置？当前设置将重置。") == DialogResult.Yes))
            {
                this.Option = GnssProcessOptionManager.Instance[SolverType];
                this.EntityToUi();
            }
        }

        private void listBox_vector_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                Brush mybsh = Brushes.Black;
                Brush backGroundBrush = Brushes.White;
                Brush backGroundBrushSelected = Brushes.DarkGray;
                var currentItem = this.listBox_vector.Items[e.Index];
                var item = currentItem as SiteObsBaseline;
                // 判断是什么类型的标签

                switch (item.ResultState)
                {
                    case ResultState.Unknown:
                        break;
                    case ResultState.Good:
                        backGroundBrush = new SolidBrush(Color.FromArgb(255,20,150,50));// Brushes.DarkGreen;// Brushes.LawnGreen;
                        backGroundBrushSelected = new SolidBrush(Color.FromArgb(100, 150, 200, 150));
                        break;
                    case ResultState.Acceptable:
                        backGroundBrush = new SolidBrush(Color.FromArgb(255, 100, 150, 50));//  Brushes.GreenYellow;
                        backGroundBrushSelected = new SolidBrush(Color.FromArgb(100, 150, 150, 150));
                        break;
                    case ResultState.Warning:
                        backGroundBrush = new SolidBrush(Color.FromArgb(255, 200, 180, 50));// Brushes.YellowGreen;
                        backGroundBrushSelected = new SolidBrush(Color.FromArgb(100, 180, 200, 150));
                        break;
                    case ResultState.Bad:
                        backGroundBrush = new SolidBrush(Color.FromArgb(255, 200, 50, 25));//Brushes.OrangeRed;
                        backGroundBrushSelected = new SolidBrush(Color.FromArgb(100, 255, 200, 150));
                        break;
                    default:
                        break;
                }
                e.Graphics.FillRectangle(backGroundBrush, listBox_vector.GetItemRectangle(e.Index));


                if ((e.State & DrawItemState.Selected)== DrawItemState.Selected)
                { 
                    e.Graphics.FillRectangle(backGroundBrushSelected, listBox_vector.GetItemRectangle(e.Index));
                }
                // 焦点框
                e.DrawFocusRectangle();
                //文本  
                e.Graphics.DrawString(item.LineName.Name, e.Font, new SolidBrush(listBox_vector.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void 标记为好基线OToolStripMenuItem_Click(object sender, EventArgs e) { var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; } line.ResultState = ResultState.Good; this.listBox_vector.Refresh(); }

        private void 标记为坏基线BToolStripMenuItem_Click(object sender, EventArgs e) { var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; } line.ResultState = ResultState.Bad; this.listBox_vector.Refresh(); }


        private void 清除当前标记CToolStripMenuItem_Click(object sender, EventArgs e) { var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; } line.ResultState = ResultState.Unknown; this.listBox_vector.Refresh(); }


        private void 清除所有好标记CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetAllBaseLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            foreach (var line in lines)
            {
                if (line.ResultState == ResultState.Good) { line.ResultState = ResultState.Unknown; }
            }
            this.listBox_vector.Refresh();
        }

        private void 清除所有坏标记CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetAllBaseLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            foreach (var line in lines)
            {
                if (line.ResultState == ResultState.Bad) { line.ResultState = ResultState.Unknown; }
            }

            this.listBox_vector.Refresh();
        }


        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetSelectedSites(); if (sites == null || sites.Count ==0) { return; }

            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("将移除测站 " + Geo.Utils.StringUtil.ToString(sites) + " 及对应基线和计算的结果，移除后可以重新载入,\r\n 确定移除？") == System.Windows.Forms.DialogResult.Yes)
            {
                //移除测站
                foreach (var site in sites)
                {
                    this.bindingSource_site.Remove(site);
                    this.ObsFileManager.Remove(site.SiteName);

                    //移除基线
                    var lines = this.BaselineManager.Get(site.SiteName);
                    foreach (var item in lines) { this.bindingSource_vector.Remove(item); }
                    BaselineManager.Remove(site.SiteName, false);
                }

                DataBind();
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("将清空所有测站及基线，请确保基线结果已经保存,\r\n 确定移除？") == System.Windows.Forms.DialogResult.Yes)
            {
                this.ObsFileManager.Clear();
                this.BaselineManager.Clear();
                this.bindingSource_site.Clear();
                this.bindingSource_vector.Clear();
                this.objectTableControl_baselineResult.Clear();
                this.objectTableControl_closureErrors.Clear();
                this.objectTableControl_independentLine.Clear();
                this.objectTableControl_currentLineErrors.Clear();
            }
        }

        private void 导入文件IToolStripMenuItem_Click(object sender, EventArgs e) { if (openFileDialog1_rinexOFile.ShowDialog() == DialogResult.OK) { InpportFiles(openFileDialog1_rinexOFile.FileNames); } }

        private void InpportFiles(string[] filePathes)
        {
            if(filePathes.Length == 0) { return; }
            var dir = Path.GetDirectoryName(filePathes[0]);
            this.ProjectDirectory = Path.Combine(dir, Setting.GnsserProjectDirectoryName);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(ProjectDirectory);
            var projName = Path.GetFileName(dir);
            this.namedStringControl_projName.SetValue(projName);

            StringBuilder msg = new StringBuilder();
            bool added = false;
            foreach (string path in filePathes)
            {
                var info = new ObsSiteInfo(path);
                if (!ObsFileManager.Contains(info.SiteName)) { ObsFileManager.Add(info.SiteName, info); added = true; }
                else { msg.AppendLine(info.FilePath); }
            }
            if (msg.Length > 0)
            {
                MessageBox.Show("以下文件没有添加成功，因为已经包含了相同测站名称的文件，请手动移除该站后再试。\r\n" + msg);
            }

            if (added)
            {
                var lines = ObsFileManager.GenerateObsBaseLines(this.GetOrCreateOption().BaseLineSelectionType, this.Option.CenterSiteName, this.Option.BaseLineFilePath);

                GenerateBaseLineAndAddToCurrentLineManager(lines);
            }
        }

        private void GenerateBaseLineAndAddToCurrentLineManager(List<SiteObsBaseline> lines)
        {
            var path =  resultFileNameBuilder.GetBaseLineResultFile();
            if (File.Exists(path) && Geo.Utils.FormUtil.ShowYesNoMessageBox("是否载入匹配当前发现的计算结果？\r\n " + path) == DialogResult.Yes)
            {
                var tables = ObjectTableManager.Read(new string[] { path }, ".");
                //合并所有的表格
                var rawTable = tables.Combine();
                var BaseLineNets = BaseLineNetManager.Parse(rawTable, 20);//时段网

                foreach (var line in lines)
                {
                    var est = BaseLineNets.First.Get(line.LineName);
                    if (est != null) { line.EstimatedResult = est; }
                }
            }
            else
            {
                // Geo.Utils.FileUtil.TryClearDirectory(ProjectDirectory);
            }
            //添加基线，如果重复则忽略
            if (BaselineManager.Add(lines, false) > 0)
            {
                DataBind();
            }
        }

        private void 查看已移除基线BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ObjectSelectingForm<GnssBaseLineName>(BaselineManager.RemovedLines.Keys.ToList());
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var news = form.SelectedObjects;
                foreach (var item in news)
                {
                    var obj = BaselineManager.RemovedLines[item];
                    BaselineManager.Add(obj);
                }

                this.DataBind();
            }
        }

        private void 移除此基线RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetSelectedLines(); if (lines == null || lines.Count ==0) { return; }

            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("将移除基线 " + Geo.Utils.StringUtil.ToString(lines) + " 及对应的计算结果，移除后可以重新添加,\r\n 确定移除？") == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (var line in lines)
                {
                    //判断是否独立基线
                    if (this.BaselineManager.IsIndependent(line.LineName))
                    {
                        if (Geo.Utils.FormUtil.ShowYesNoMessageBox(line.LineName + " 是一条独立基线，移除后，对应测站无法参与计算，是否仍然移除？") != DialogResult.Yes)
                        {
                            continue;
                        }
                    }

                    //移除基线
                    var lineObj = this.BaselineManager.Get(line.LineName);
                    if (lineObj != null) { this.bindingSource_vector.Remove(lineObj); }
                    BaselineManager.Remove(lineObj.LineName);
                }

                DataBind();
            }
        }
        private void bindingSource_site_CurrentChanged(object sender, EventArgs e)
        {
            if (!IsInited) { return; }
            var site = GetCurrentSite(); if (site == null) { return; }
            ShowObjectAttributeInfo(site);
        }

        private void bindingSource_vector_CurrentChanged(object sender, EventArgs e)
        {
            if (!IsInited) { return; }

            SiteObsBaseline line = GetCurrentBaseLine(); if (line == null) { return; }
            ShowObjectAttributeInfo(line);
        }

        private void 打开所在目录OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { return; }
            Geo.Utils.FileUtil.OpenDirectory(site.Directory);
        }

        private void 打开当前文件FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { return; }
            Geo.Utils.FileUtil.OpenFile(site.FilePath);
        }

        private void button_optionSet_Click(object sender, EventArgs e)
        {
            SolverSetting();
        }

        private void SolverSetting()
        {
            if (this.Option == null) { this.Option = GnssProcessOption.GetDefaultSimpleEpochDoubleDifferPositioningOption(); }
            UiToEntity();
            var optionForm = new OptionVizardForm(Option);
            optionForm.Init();
            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Option = optionForm.Option;
                this.EntityToUi();
            }
        }

        /// <summary>
        /// 数据绑定显示
        /// </summary>
        private void DataBind()
        {
            IsInited = false;
            this.bindingSource_site.DataSource = ObsFileManager.Values;
            this.bindingSource_vector.DataSource = BaselineManager.Values;

            this.label_siteInfo.Text = "测站数量："  + ObsFileManager.Count;
            this.label_lineInfo.Text = "基线数量："  + BaselineManager.Count;
            //重新计算闭合差和显示基线
            BindResultBuildAndSetClosureError();
            //等闭合差计算后再显示

            IsInited = true;
        }
        #endregion

        #region 显示 
        private void ShowObjectAttributeInfo<T>(T obj)
        {
            if (!IsInited) { return; }
            attributeBox1.SetObject<T>(obj, false);
        }
        #endregion

        #region 基线解算

        private void 解算此基线SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteObsBaseline line = GetCurrentBaseLine();
            if (line == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择基线！"); return; }
            this.RunSolveBaseLine(new List<SiteObsBaseline>() { line });
         // SolveBaseLine(new List<SiteObsBaseline>() { line });
        }
         
        private void 计算包含指定测站的基线IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ObjectSelectingForm<ObsSiteInfo>(this.ObsFileManager.Values);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var news = form.SelectedObjects;
                List<SiteObsBaseline> lines = new List<SiteObsBaseline>();
                foreach (var item in news)
                {
                    var obj = BaselineManager.Get(item.SiteName);
                    lines.AddRange(obj);
                }
                this.RunSolveBaseLine(lines);
                //在主线程中执行报错
                //var t1 = new System.Threading.Tasks.Task(() => SolveBaseLine(lines.Distinct().ToList()));
                //t1.Start(); 
            }

        }

        private void 解算所选基线SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetSelectedLines(); if (lines == null || lines.Count == 0) { return; }

            this.RunSolveBaseLine(lines); 
        }
        private void 选择基线后解算MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ObjectSelectingForm<GnssBaseLineName>(BaselineManager.Keys);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var news = form.SelectedObjects;
                List<SiteObsBaseline> lines = new List<SiteObsBaseline>();
                foreach (var item in news)
                {
                    var obj = BaselineManager.Get(item);
                    lines.Add(obj);
                }
                this.RunSolveBaseLine(lines);
                //在主线程中执行报错
                //var t1 = new System.Threading.Tasks.Task(() => SolveBaseLine(lines));
                //t1.Start(); 
            }
        }
        private void 解算所有基线AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = GetAllBaseLines();
            if (lines == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请载入数据！"); return; } 
            this.RunSolveBaseLine(lines);
        }

        private void 解算所有坏基线DToolStripMenuItem_Click(object sender, EventArgs e) {
            var lines = GetAllBaseLines();
            if (lines == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请载入数据！"); return; }

            List<SiteObsBaseline> toSolves = new List<SiteObsBaseline>();
            foreach (var item in lines)
            {
                if (item.ResultState == ResultState.Bad) { toSolves.Add(item); }
            }
            this.RunSolveBaseLine(toSolves);
        }

        private void 解算所有非好基线DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = GetAllBaseLines();
            if (lines == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请载入数据！"); return; }

            List<SiteObsBaseline> toSolves = new List<SiteObsBaseline>();
            foreach (var item in lines)
            {
                if (item.ResultState != ResultState.Good) { toSolves.Add(item); }
            }
            this.RunSolveBaseLine(toSolves);
        }

        private void button_runAllBaseLine_Click(object sender, EventArgs e)
        {
            解算所有基线AToolStripMenuItem_Click(null, null);
        }
        /// <summary>
        /// 当前解算的
        /// </summary>
        public List<SiteObsBaseline> CurrentBaselinesToBeSolve { get; set; }
       
        /// <summary>
        /// 解算所有非好基线
        /// </summary>
        private void RunSolveBaseLine(List<SiteObsBaseline> toSolves)
        {
            if (backgroundWorker.IsBusy)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("正在计算中.....请稍等");
            }
            else
            {
                button_runAllBaseLine.Enabled = false;
                this.button_solveCurrentLine.Enabled = false;
                 
                this.CurrentBaselinesToBeSolve = toSolves;
                this.backgroundWorker.RunWorkerAsync();
            }
        }
        /// <summary>
        /// 解算基线
        /// </summary>
        /// <param name="sitebaseLines"></param>
        private void SolveBaseLine(List<SiteObsBaseline> sitebaseLines)
        {
            if (sitebaseLines == null || sitebaseLines.Count == 0) { log.Warn("没有计算数据！"); return; }
            log.Info("即将解算指定的 " + sitebaseLines.Count + " 条基线。");
            List<GnssBaseLineName> baseLines = new List<GnssBaseLineName>() { };
            List<string> pathes = new List<string>();
            foreach (var item in sitebaseLines)
            {
                pathes.Add(item.Start.FilePath);
                pathes.Add(item.End.FilePath);

                //更新最新路径，当修改后存储在Temp目录中的需要更新地址
                item.LineName.RefFilePath = item.Start.FilePath;
                item.LineName.RovFilePath = item.End.FilePath;

                baseLines.Add(item.LineName);
            }
            this.GetOrCreateOption();
            this.UiToEntity();

            //不同算法输出不同的目录
            Option.OutputDirectory = Path.Combine(Option.OutputDirectory, Option.GnssSolverType.ToString());

            TwoSiteBackGroundRunner runner = new TwoSiteBackGroundRunner(this.Option, pathes.Distinct().ToArray(), baseLines);
            runner.ParallelConfig = ParallelConfig;
            runner.Completed += Runner_Completed;
            runner.Processed += OneSolver_Processed;
            runner.ProgressViewer = progressBarComponent1;
            runner.Init();
            runner.Run();
        }

        private void Runner_Completed(object sender, EventArgs e)
        {
            BindResultBuildAndSetClosureError();
            currentVectorIndex = -1;//重置当前所选
            this.Invoke(new Action(()=> { listBox_vector_SelectedIndexChanged(null, null); }));           
        }
        /// <summary>
        /// 绑定数据表，并计算闭合差
        /// </summary>
        private void BindResultBuildAndSetClosureError()
        {
            this.Invoke(new Action(() =>//采用Invoke将出错
            {
                //基线结果显示
                var baseLineTable = BaselineManager.GetBaselineTable();
                if (baseLineTable.ColCount == 0 || baseLineTable.RowCount == 0) { return; }

                baseLineTable = BaselineManager.GetBaselineTable();//首先确保可以显示一个
                this.objectTableControl_baselineResult.DataBind(baseLineTable);

                //解析为网络
                var BaseLineNets = BaseLineNetManager.Parse(baseLineTable, 200);//时段网
                var bigNet = BaseLineNets.FirstKeyValue.Value;

                //生成所有可能的三角形，然后提取网络，计算闭合差 
                CurrentQualityManager = bigNet.BuildTriangularClosureQualies(this.GetOrCreateOption().GnssReveiverNominalAccuracy);
                if (CurrentQualityManager.Count == 0) { return; }

                //同步环闭合差表格生成 和显示      
                ObjectTableStorage closureErrorsTable = BaseLineNet.BuildSynchNetTrilateralCheckTResultable( "Net", CurrentQualityManager);                    
                objectTableControl_closureErrors.DataBind(closureErrorsTable);

                //计算闭合差，根据闭合差设置颜色 
                var lines = this.GetAllBaseLines();
                foreach (var line in lines)
                {
                    var quality = CurrentQualityManager.GetBest(line.LineName);
                    if (quality == null) { continue; }

                    line.ResultState = quality.ResultState;
                    line.EstimatedResult.ClosureError = quality.ClosureError.Value.Length;
                }
                this.listBox_vector.Refresh();
                //闭合差更新后的值，因此重新提取和绑定
                baseLineTable = BaselineManager.GetBaselineTable();
                this.objectTableControl_baselineResult.DataBind(baseLineTable);
            }));
        }
        /// <summary>
        /// 当前三角形闭合差质量
        /// </summary>
        TriguilarNetQualitiyManager CurrentQualityManager { set; get; }
        /// <summary>
        /// 计算完毕一个测站
        /// </summary>
        /// <param name="Solver"></param>
        void OneSolver_Processed(IntegralGnssFileSolver Solver)
        {
            var result = Solver.CurrentGnssResult;
            var entity = result as IWithEstimatedBaseline; if (entity == null) { return; }
            var line = entity.GetEstimatedBaseline();
            var lineObj = BaselineManager.Get(line.BaseLineName);

            lineObj.EstimatedResult = line;
            log.Info(result.Name + ", " + result.ReceiverTime + "， 即将输出结果文件...");
            var writer = new GnssResultWriter(Option,  this.Option.IsOutputEpochResult,
                Option.IsOutputEpochSatInfo);
            writer.WriteFinal((BaseGnssResult)result);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SolveBaseLine(this.CurrentBaselinesToBeSolve);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_runAllBaseLine.Enabled = true; button_solveCurrentLine.Enabled = true;
        }
        #endregion

        #region 数据获取

        public GnssProcessOption GetOrCreateOption()
        {
            if (this.Option == null) { this.Option = GnssProcessOption.GetDefaultSimpleEpochDoubleDifferPositioningOption(); }
            return Option;
        }
        private SiteObsBaseline GetCurrentBaseLine() { return this.bindingSource_vector.Current as SiteObsBaseline; }
        private List<SiteObsBaseline> GetSelectedLines()
        {
            var result = new List<SiteObsBaseline>();
            foreach (var item in this.listBox_vector.SelectedItems)
            {
                var obj = item as SiteObsBaseline;
                result.Add(obj);
            }
            return result;
        }
        private ObsSiteInfo GetCurrentSite() { return this.bindingSource_site.Current as ObsSiteInfo; }
        private List<ObsSiteInfo> GetSelectedSites()
        {
            List<ObsSiteInfo> result = new List<ObsSiteInfo>();
            foreach (var item in this.listBox_site.SelectedItems)
            {
                var obj = item as ObsSiteInfo;
                result.Add(obj);
            }
            return result;
        }
        private List<ObsSiteInfo> GetAllSites() { return this.bindingSource_site.DataSource as List<ObsSiteInfo>; }

        private List<EstimatedBaseline> GetAllLines()
        {
            List<SiteObsBaseline> lst = GetAllBaseLines();
            var result = new List<EstimatedBaseline>();
            foreach (var item in lst)
            {
                var est = item.EstimatedResult;
                if (est == null) { continue; }

                result.Add((EstimatedBaseline)(est));
            }
            return result;
        }
        private List<SiteObsBaseline> GetAllBaseLines()
        {
            List<SiteObsBaseline> lst = this.bindingSource_vector.DataSource as List<SiteObsBaseline>;
            return lst;
        }

        #endregion

        #region 结果绘线条图

        private void 打开残差文件RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; }
            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochResidualFileName(line.LineName.Name));
            TryOpenTableForm(path);
        }

        private void 打开历元参数文件PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; }
            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
            TryOpenTableForm(path);
        }


        private void 打开RMS文件RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; }
            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamRmsFileName(line.LineName.Name));
            TryOpenTableForm(path);
        }
        private static void TryOpenTableForm(string path)
        {
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                new TableObjectViewForm(table).Show();
            }
        }
        private void 查看编辑基线测站时段EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; }
            line.Start.CheckOrCopyToTempDirectory();
            ObsFileChartEditForm form = new ObsFileChartEditForm(line.Start.FilePath, true, false, false, true);
            form.Show();
            line.End.CheckOrCopyToTempDirectory();
            ObsFileChartEditForm form1 = new ObsFileChartEditForm(line.End.FilePath, true, false, false, true);
            form1.Show();

        }
        private void 查看编辑所选测站时段EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            site.CheckOrCopyToTempDirectory();
            ObsFileChartEditForm form = new ObsFileChartEditForm(site.FilePath, true, false, false, true);
            form.Show();
        }
        private void 恢复原始文件RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否要恢复" + site.SiteName + "的修改？ ") == DialogResult.Yes)
            {
                site.CopyToTempDirectory();
            }
        }
        private void 查看编辑所有测站时段AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            foreach (var site in sites)
            {
                site.CheckOrCopyToTempDirectory();
                ObsFileChartEditForm form = new ObsFileChartEditForm(site.FilePath, true, false, false, true);
                form.Show();
            }
        }

        private void 恢复所有测站原始文件SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否要恢复所有测站的修改？ ") == DialogResult.Yes)
            {
                foreach (var site in sites)
                {
                    site.CopyToTempDirectory();
                }
            }
        }

        private void 查看卫星高度角HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            Thread thread = new Thread(new ParameterizedThreadStart(GnsserFormUtil.OpenSatElevationTableForm));
            thread.Start(site.FilePath);
            //OpenSatElevationTable(site);
            // new System.Threading.Thread((System.Threading.ThreadStart)delegate { Application.Run(new Form2); }).Start(); 
        }


        private void 查看载波残差LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochResidualFileName(line.LineName.Name));
            GnsserFormUtil.ShowPhaseChartForm(path);
        }


        private void 查看历元收敛图EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
            var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
            GnsserFormUtil.ShowChartForm(path, paramNames);
        }


        private void 查看历元参数RMS图EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamRmsFileName(line.LineName.Name));
            var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.Dx, ParamNames.Dy, ParamNames.Dz };
            GnsserFormUtil.ShowChartForm(path, paramNames);
        }

        private void 设置后绘产出图SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochResidualFileName(line.LineName.Name));
            GnsserFormUtil.SetThenShowChartForm(path);
        }


        private void 设置后绘历元参数收敛图DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
            GnsserFormUtil.SetThenShowChartForm(path);
        }

        private void 设置后绘制RMS图RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamRmsFileName(line.LineName.Name));
            GnsserFormUtil.SetThenShowChartForm(path);
        }
        private void 查看所有基线残差图AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetAllBaseLines();
            var first = lines.FirstOrDefault();
            if (first == null || first.EstimatedResult == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！" + first.LineName); return; }

            foreach (var line in lines)
            {
                var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochResidualFileName(line.LineName.Name));
                GnsserFormUtil.ShowPhaseChartForm(path);
            }
        }
        private void 设置后绘所有历元参数收敛图SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetAllBaseLines();
            var first = lines.FirstOrDefault();
            if (first == null || first.EstimatedResult == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！" + first.LineName); return; }

            List<string> pathes = new List<string>();
            foreach (var line in lines)
            {
                var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
                pathes.Add(path);
            }
            GnsserFormUtil.SetThenShowChartForm(pathes.ToArray());
        }

        private void 查看所有历元收敛图AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetAllBaseLines();
            var first = lines.FirstOrDefault();
            if (first == null || first.EstimatedResult == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！" + first.LineName); return; }

            foreach (var line in lines)
            {
                var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                GnsserFormUtil.ShowChartForm(path, paramNames);
            }
        }
        bool Check(SiteObsBaseline line, bool mustHasResult = true)
        {
            if (line == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中基线再试！"); return false; }
            if (mustHasResult && line.EstimatedResult == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！" + line.LineName); return false; }
            return true;
        }
        #endregion

        #region  输出

        private void button_runIndependentLine_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);

            //注意这里返回的是不同时段的独立基线集合
            var independentLineNet = BaseLineNet.GetIndependentNet(IndependentLineSelectType);
            objectTableControl_independentLine.Tag = independentLineNet;
            objectTableControl_independentLine.DataBind(independentLineNet.GetLineTable());
        }

        IndependentLineSelectType IndependentLineSelectType => this.enumRadioControl_selectLineType.GetCurrent<IndependentLineSelectType>();


        private void button_saveAllAsGNSSerFile_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);
            var name = ProjectName + BaseLineNet.BaseLines.First().Name + "_etc_" + this.GetOrCreateOption().ObsPhaseType + "_All";
            SaveGnsserBaselineResult(BaseLineNet, name);
        }

        private void button_saveAllAsLgoAsc_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);
            var name = ProjectName + BaseLineNet.BaseLines.First().Name + "_etc_" + this.GetOrCreateOption().ObsPhaseType + "_All";
            SaveAsLeoAsc(BaseLineNet, name);
        }


        private void button_saveIndeToLeoAsc_Click(object sender, EventArgs e)
        {
            BaseLineNet BaseLineNet = objectTableControl_independentLine.Tag as BaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先生成独立基线再试！"); return; }
            var name = ProjectName + BaseLineNet.BaseLines.First().Name + "_etc_" + this.GetOrCreateOption().ObsPhaseType + "_Indpt";
            SaveAsLeoAsc(BaseLineNet, name);
        }

        private void button_saveIndeBaselineFile_Click(object sender, EventArgs e)
        {
            BaseLineNet BaseLineNet = objectTableControl_independentLine.Tag as BaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先生成独立基线再试！"); return; }
            var name = ProjectName + BaseLineNet.BaseLines.First().Name + "_etc_" + this.GetOrCreateOption().ObsPhaseType + "_Indpt";
            SaveGnsserBaselineResult(BaseLineNet, name);
        }
        private void SaveAsLeoAsc(BaseLineNet BaseLineNet, string name)
        {
            BaseLineFileConverter converter = new BaseLineFileConverter();
            var outnet = converter.Build(BaseLineNet);
            var outpath = Path.Combine(ProjectDirectory, name + Setting.BaseLineFileOfLgoExtension);
            LgoAscBaseLineFileWriter writer = new LgoAscBaseLineFileWriter(outpath);
            writer.Write(outnet);
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(SolverDirectory);
        }

        private void SaveGnsserBaselineResult(BaseLineNet BaseLineNet, string name)
        {
            ObjectTableStorage table = BaseLineNet.GetLineTable();
            var outpath = this.resultFileNameBuilder.BuildBaseLineResulPath(name);

            ObjectTableWriter.Write(table, outpath);
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(SolverDirectory);
        }
        #endregion

        #region 地图显示

        private void button_viewSelectedTriPathes_Click(object sender, EventArgs e)
        {
            var triangles = Geo.Utils.DataGridViewUtil.GetSelectedRows(objectTableControl_closureErrors.DataGridView);
            List<GnssBaseLineName> lineNames = new List<GnssBaseLineName>();
            foreach (var item in triangles)
            {
                var pathString = item.Cells["闭合路径"].Value.ToString();
                var pathes = Geo.Utils.StringUtil.Split(pathString, new char[] { ',', ' ' });
                foreach (var path in pathes)
                {
                    var lineName = new GnssBaseLineName(path);
                    lineNames.Add(lineName);
                }
            }

            ShowLinesOnMap(lineNames);
        }
        private void button_showCurentLine_Click(object sender, EventArgs e)
        {
            var lines = this.GetCurrentBaseLine();
            if (lines == null ) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(new List<EstimatedBaseline>() { (EstimatedBaseline)lines.EstimatedResult } );

            ShowLinesOnMap(BaseLineNet);
        }
        private void button_showAllLineeOnMap_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);

            ShowLinesOnMap(BaseLineNet);
        }
        private void button_showIndeLineOnMap_Click(object sender, EventArgs e)
        {
            BaseLineNet BaseLineNet = objectTableControl_independentLine.Tag as BaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先生成独立基线再试！"); return; }

            ShowLinesOnMap(BaseLineNet);
        }


        private void ShowLinesOnMap(BaseLineNet BaseLineNet)
        {
            List<LineString> lineStrings = new List<LineString>();
            foreach (var line in BaseLineNet)
            {
                LineString lineString = BuildLineString(line);
                lineStrings.Add(lineString);
            }
            if (lineStrings.Count == 0) { return; }

            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "基线");
            ShowLayer(layer);
        }
        private void ShowLinesOnMap(List<GnssBaseLineName> lineNames)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);

            List<LineString> lineStrings = new List<LineString>();
            foreach (var lineName in lineNames)
            {
                var line = BaseLineNet.GetOrReversed(lineName);
                if(line == null) { continue; }

                LineString lineString =  BuildLineString(line);
                lineStrings.Add(lineString);
            }
            if (lineStrings.Count == 0) { return; }

            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "三角网");
            ShowLayer(layer);
        }

        private LineString BuildLineString(EstimatedBaseline baseLine)
        {
            var name = baseLine.BaseLineName.RovName;
            var ptA = new AnyInfo.Geometries.Point(baseLine.EstimatedGeoCoordOfRov, null, name);
            name = baseLine.BaseLineName.RefName;
            var geoCoord = CoordTransformer.XyzToGeoCoord(baseLine.ApproxXyzOfRef);
            var ptB = new AnyInfo.Geometries.Point(geoCoord, null, name);
            var lineString = new LineString(new List<AnyInfo.Geometries.Point>()
                        {
                            ptA, ptB
                        }, baseLine.Name);
            return lineString;
        }

        private void 地图显示测站SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = GetAllSites();
            if (sites == null || sites.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请载入数据后再试！"); return; }
            List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();

            foreach (var item in sites)
            {
                var xyz = item.SiteObsInfo.ApproxXyz;
                var geoCoord = CoordTransformer.XyzToGeoCoord(xyz);

                pts.Add(new AnyInfo.Geometries.Point(geoCoord, null, item.SiteName));
            }

            if (pts.Count == 0) { return; }
            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreatePointLayer(pts);
            ShowLayer(layer);
        }

        private void 查看所有测站时段图PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.ObsFileManager.Values;
            ShowSiteObsPhases(sites);
        }

        private void 查看基线时段图PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }
            List<ObsSiteInfo> sites = new List<ObsSiteInfo>() { line.Start, line.End };
            ShowSiteObsPhases(sites);
        }
        public static void ShowSiteObsPhases(List<ObsSiteInfo> sites)
        {
            Dictionary<string, TimePeriod> timeperiods = new Dictionary<string, TimePeriod>();
            foreach (var item in sites)
            {
                if (File.Exists(item.FilePath))
                {
                    timeperiods[item.SiteName] = RinexObsFileReader.ReadPeriod(item.FilePath);
                }
            }
            EpochChartForm chartForm = new EpochChartForm(timeperiods, "测站时段图");
            chartForm.Show();
        }
        #endregion
        #region  格式化测站
        public ObsFileConvertOption ObsFileConvertOption { get; set; }
        public ObsFileConvertOption GetOrInitObsFileFormatOption()
        {
            if (ObsFileConvertOption == null) { ObsFileConvertOption = new ObsFileConvertOption(); }
            return ObsFileConvertOption;
        }
        private void 格式化当前测站FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { return; }

            this.ObsFileConvertOption = GetOrInitObsFileFormatOption();
            var form = new ObsFileConvertOptionForm(ObsFileConvertOption);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ObsFileConvertOption = form.Option;
                RunFormateSitesTask(new List<ObsSiteInfo>() { site });
            }
        }

        private void 采用上一设置格式化当前测站TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { return; }
            RunFormateSitesTask(new List<ObsSiteInfo>() { site });
        }

        private void 格式化所有测站AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
      
            this.ObsFileConvertOption = GetOrInitObsFileFormatOption();
            var form = new ObsFileConvertOptionForm(ObsFileConvertOption);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ObsFileConvertOption = form.Option;

                RunFormateSitesTask(sites);
            }
        }
        private void 采用上一设置格式所有测站MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否要采用上一设置格式化所有观测文件？格式化后，将保存到临时目录中 ") == DialogResult.Yes)
            {
                RunFormateSitesTask(sites);
            }
        }

        public void RunFormateSitesTask(List<ObsSiteInfo> sites)
        {
            //在主线程中执行报错
            var t1 = new System.Threading.Tasks.Task(() => FormateSites(sites));
            t1.Start(); 
        }

        private void FormateSites(List<ObsSiteInfo> sites)
        {
            if(sites == null || sites.Count == 0) { return; }
            this.ObsFileConvertOption = GetOrInitObsFileFormatOption();
            foreach (var site in sites)
            {
                ObsFileConvertOption opt = GetOrInitObsFileFormatOption();
                opt.OutputDirectory = site.Directory;
                string subDir = "Temp";
                ObsFileFormater ObsFileFormater = new ObsFileFormater(opt, site.OriginalPath);
                ObsFileFormater.IsOverrite = true;
                ObsFileFormater.SubDirectory = subDir;
                ObsFileFormater.Init();
                ObsFileFormater.Run();

                if (site.TempFilePath != ObsFileFormater.FirstOutputPath)
                {
                    log.Warn("与临时文件路径不一致，格式化输出为: " + ObsFileFormater.FirstOutputPath + ",  临时路径为: " + site.TempFilePath);
                }
                else
                {
                    log.Info(site.SiteName + " 完成, 格式化输出到 " + ObsFileFormater.FirstOutputPath);
                }
            }

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(sites[0].TempDirectory, "格式化完毕，是否打开临时目录？如果需要替换源文件，请手动操作。");
        }

        #endregion
        int currentVectorIndex = -1;
        private void listBox_vector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentVectorIndex == listBox_vector.SelectedIndex) { return; }

            currentVectorIndex = listBox_vector.SelectedIndex;

            if (CurrentQualityManager == null) { return; }
            var lines = this.GetAllBaseLines();
            var currentLine = this.GetCurrentBaseLine();
            if (currentLine == null || currentLine.EstimatedResult == null) { return; }

            var quality = CurrentQualityManager.Get(currentLine.LineName);
            if (quality == null) { return; }

            ObjectTableStorage lineTable = QualityOfTriAngleClosureError.BuildSynchNetTrilateralCheckTResultable(currentLine.LineName.ToString(), quality);
            this.objectTableControl_currentLineErrors.DataBind(lineTable);



            //列表所有双差算法
            List<string> pathes = resultFileNameBuilder.GetAllTwoSiteSolverResidualFileName(currentLine.LineName.Name);
            int index = 0;
            //文本显示 

            var resultPathes = resultFileNameBuilder.GetAllBaseLineResultFile();
            StringBuilder sb = new StringBuilder();
            foreach (var path in resultPathes)
            {
                var tables = ObjectTableManager.Read(new string[] { path }, ".");
                //合并所有的表格
                var rawTable = tables.Combine();
                var BaseLineNets = BaseLineNetManager.Parse(rawTable, 20);//时段网

                if(BaseLineNets .Count == 0) { continue; }

                var net = BaseLineNets.First;
                var line = net.GetOrReversed(currentLine.LineName);

                if (line != null)
                {
                    sb.AppendLine(path);
                    sb.AppendLine(line.ToString());
                }
                 
            } 
            richTextBoxControl_baselineInfo.Text = sb.ToString(); 


            index = 0;
            pathes = resultFileNameBuilder.GetAllTwoSiteSolverResidualFileName(currentLine.LineName.Name);
            foreach (var path in pathes)
            {
                if (File.Exists(path))
                {
                    index++;
                    var name= System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path), System.IO.Path.GetFileName(path)); 
                    
                    var table = ObjectTableReader.Read(path);
                    var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));
                    var phaseObsTable = table.GetTable(table.Name, titles);
                    if(index == 1)
                    { 
                       this.commonChartControl_currentResidual.DataBind(phaseObsTable);
                        commonChartControl_currentResidual.ShowInfo(name);

                    }
                    if (index == 2)
                    { 
                        this.commonChartControl_ResidualB.DataBind(phaseObsTable);
                        commonChartControl_ResidualB.ShowInfo(name);
                    }
                    if (index == 3)
                    { 
                        this.commonChartControl_ResidualC.DataBind(phaseObsTable);
                        commonChartControl_ResidualC.ShowInfo(name);
                    }
                    if (index == 4)
                    { 
                        this.commonChartControl_ResidualD.DataBind(phaseObsTable);
                        commonChartControl_ResidualD.ShowInfo(name);
                    }
                }
            }

            //当前基线参数收敛图
            index = 0;
            pathes = resultFileNameBuilder.GetAllTwoSiteSolverEpochParamFileName(currentLine.LineName.Name);
            foreach (var path in pathes)
            {
                if (File.Exists(path))
                {
                    index++;
                    var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path), System.IO.Path.GetFileName(path));
                    var table = ObjectTableReader.Read(path);
                    var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                    var phaseObsTable = table.GetTable(table.Name, paramNames);
                    phaseObsTable.RemoveRows(0, 20);//默认移除前十

                    if (index == 1)
                    {
                        commonChartControl_convergenceA.DataBind(phaseObsTable);
                        commonChartControl_convergenceA.ShowInfo(name);
                    }
                    if (index == 2)
                    {
                        this.commonChartControl_convergenceB.DataBind(phaseObsTable);
                        commonChartControl_convergenceB.ShowInfo(name);
                    }
                    if (index == 3)
                    {
                        this.commonChartControl_convergenceC.DataBind(phaseObsTable);
                        commonChartControl_convergenceC.ShowInfo(name);
                    } 
                    if (index == 4)
                    {
                        this.commonChartControl_convergenceD.DataBind(phaseObsTable);
                        commonChartControl_convergenceD.ShowInfo(name);
                    } 
                }
            }

        }   

        private void listBox_vector_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = e.ItemHeight + 2;
        }
        #region  PPP 计算
        private void 查看当前测站PPP收敛图VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
            var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
            GnsserFormUtil.ShowChartForm(path, paramNames);
        }

        private void 打开当前测站PPP结果RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
            TryOpenTableForm(path);

        }
        private void 查看所有测站PPP收敛图AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            var names = new List<string>();
            foreach (var site in sites)
            {
                var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                GnsserFormUtil.ShowChartForm(path, paramNames);
            }

        }

        private void pPP计算并更新当前头文件PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            
            var pathes = new string[] { site.FilePath };
            var t1 = new System.Threading.Tasks.Task(() => RunPpp(pathes));
            t1.Start();
        }
        private void 所有文件PPP并更新头文件MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            var names = new List< string>();
            foreach (var site in sites)
            {
                names.Add(site.FilePath);
            }
            //在主线程中执行报错
            var t1 = new System.Threading.Tasks.Task(() => RunPpp(names.ToArray()));
            t1.Start(); 
        }

        private void PppRunner_Processed(SingleSiteGnssSolveStreamer Solver)
        {
            var entity = Solver.CurrentGnssResult as SingleSiteGnssResult; if (entity == null) { return; }
            var xyz = entity.EstimatedXyz;
            var site =  this.ObsFileManager.Get(entity.SiteInfo.SiteName);

            site.SiteObsInfo.ApproxXyz = xyz;

            var temptempDir = Path.Combine(site.TempDirectory, "Temp");
            Geo.Utils.FileUtil.CheckOrCreateDirectory(temptempDir);

            var outPath = Path.Combine(temptempDir, site.SiteObsInfo.FileInfo.FileName);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic[RinexHeaderLabel.APPROX_POSITION_XYZ] = RinexObsFileWriter.BuildApproxXyzLine(xyz);
            var replacer = new LineFileReplacer(site.FilePath, outPath, dic);
            replacer.EndMarkers.Add(RinexHeaderLabel.END_OF_HEADER);
            //replacer.AddingLines.Add(RinexObsFileWriter.BuildGnsserCommentLines());
            replacer.AddingLines.Add(RinexObsFileWriter.BuildCommentLine("Approx XYZ updated with GNSSer PPP " + Geo.Utils.DateTimeUtil.GetFormatedDateTimeNow()));
            replacer.Run();

            Geo.Utils.FileUtil.MoveFile(outPath, site.TempFilePath, true);
            log.Info("更新成功！ 输出到 ： " + site.TempFilePath);

            log.Info(entity.Name + ", " + entity.ReceiverTime + "， 输出到结果文件");
            var writer = new GnssResultWriter(Solver.Option,  Solver.Option.IsOutputEpochResult,
                Solver.Option.IsOutputEpochSatInfo);

            writer.WriteFinal(entity);
        }

        private void PppRunner_Completed(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            var temptempDir = Path.Combine(site.TempDirectory, "Temp");
            Geo.Utils.FileUtil.TryDeleteFileOrDirectory(temptempDir);
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("PPP计算完毕，临时文件坐标已更新，是否用临时文件替换源文件？") == DialogResult.Yes)
            {
                var pathes = Directory.GetFiles(site.TempDirectory);
                foreach (var result in pathes)
                {
                    var original = Path.Combine( site.Directory, Path.GetFileName(result));
                    Geo.Utils.FileUtil.MoveFile(result, original, true);
                    log.Info("已替换 ： " + original);
                }
            }
        }


        private void RunPpp(string[] pathes)
        {
            var option = GnssProcessOption.GetDefaultIonoFreePppOption();
            option.OutputDirectory = ProjectDirectory;
            var pppRunner = new PointPositionBackGroundRunner(option, pathes);
            pppRunner.ParallelConfig = ParallelConfig;
            pppRunner.ProgressViewer = this.progressBarComponent1;
            pppRunner.Processed += PppRunner_Processed;
            pppRunner.Completed += PppRunner_Completed;
            pppRunner.Init();
            pppRunner.Run();
        }
        #endregion

        private void objectTableControl_baselineResult_Load(object sender, EventArgs e)
        {

        }

        private void BaseLineNetSolverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
          //  FormWillClosing(sender, e);

            if (backgroundWorker.IsBusy && FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No) { e.Cancel = true; }
        //    else { IsClosed = true; }
        }

        private void 重新构建基线IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseLineSelectionType selectionType = BaseLineSelectionType.全基线;
            if (!Geo.Utils.FormUtil.ShowAndSelectEnumRadioForm<BaseLineSelectionType>(out selectionType, this.GetOrCreateOption().BaseLineSelectionType)) { return; }


            this.GetOrCreateOption().BaseLineSelectionType = selectionType;

            var lines = ObsFileManager.GenerateObsBaseLines(this.GetOrCreateOption().BaseLineSelectionType, this.Option.CenterSiteName, this.Option.BaseLineFilePath);

            this.BaselineManager.Clear();
            this.GenerateBaseLineAndAddToCurrentLineManager(lines);
        }

        private void listBox_site_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }


}
