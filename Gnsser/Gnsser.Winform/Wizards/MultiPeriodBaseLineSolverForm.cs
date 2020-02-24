//2019.01.13, czs, create in hmx, 多时段基线网解算综合窗口
//2019.01.17, czs, create in hmx Gymnasium, 修正发现的问题


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

namespace Gnsser.Winform
{
    /// <summary>
    /// 多时段基线网解算综合窗口
    /// </summary>
    public partial class MultiPeriodBaseLineSolverForm : Form, IShowLayer
    {
        Log log = new Log(typeof(MultiPeriodBaseLineSolverForm));
        public event ShowLayerHandler ShowLayer;
        string ResultState = "ResultState";

        public MultiPeriodBaseLineSolverForm()
        {
            InitializeComponent();
            this.objectTableControl_allClosureErrors.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint;
            this.objectTableControl_currentLineSyncClosureErrors.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint;
            this.objectTableControl_allRepeatError.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint;
            this.objectTableControl_currentRepeatError.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint;
            this.objectTableControl_twoPeriodAsybn.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint;
            this.objectTableControl_triPeiodasyncClosure.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint;

            this.Init();
        }

        #region 颜色底色更新
        private void ClosureError_DataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var table = ((DataGridView)sender);
            if (table.Columns.Contains(ResultState))
            {
                var row = table.Rows[e.RowIndex];
                var cell = row.Cells[ResultState];
                UpdateStateBackColor(row, cell);
            }
        }

        private static void UpdateStateBackColor(DataGridViewRow row, DataGridViewCell cell)
        {
            var val = cell.Value.ToString();
            ResultState ResultState = Geo.Utils.EnumUtil.Parse<ResultState>(val);
            ResultBackGroundStyle backGroundStyle = new ResultBackGroundStyle(ResultState);
            row.DefaultCellStyle.BackColor = backGroundStyle.BackGroundBrush.Color;
            //row.DefaultCellStyle.SelectionBackColor = backGroundStyle.BackGroundBrushSelected.Color;
        }
        #endregion

        private void MultiPeriodBaseLineSolverForm_Load(object sender, EventArgs e)
        {
            this.enumRadioControl_BaseLineSelectionType.Init<BaseLineSelectionType>();
            checkBox_enableNet.Checked = Setting.EnableNet;
            namedStringControl_projName.SetValue("Net");
        }
        private void Init()
        {
            checkBox_enableNet.Checked = Setting.EnableNet;
            this.openFileDialog1_rinexOFile.Filter = Setting.RinexOFileFilter;
            this.enumRadioControl_obsType.Init<ObsPhaseType>();
            this.enumRadioControl_autoBaseLinSolveType.Init<AutoBaseLinSolveType>();
            this.enumRadioControl_selectLineType.Init<IndependentLineSelectType>();

            multiSolverOptionControl1.Init<TwoSiteSolverType>();
            multiSolverOptionControl1.SetTitle("单一计算或短基线算法");

            Manager = new MultiPeriodSiteLineManager(MinCommonSpan);
            AutoBaseLineSolver = new AutoBaseLineSolver();
            AutoBaseLineSolver.ProgressViewer = progressBarComponent1;
            AutoBaseLineSolver.Completed += AutoBaseLineSolver_Completed;

            this.EntityToUi();
        }

        private void AutoBaseLineSolver_Completed()
        {
            log.Info("当前计算完毕!");
            //Geo.Utils.FormUtil.ShowWarningMessageBox("当前计算完毕！");
             
            this.Invoke(new Action(() => this.DataBind()));
        }
        #region 属性
        /// <summary>
        ///核心管理器
        /// </summary>
        MultiPeriodSiteLineManager Manager { get; set; }
        /// <summary>
        /// 基线生成类型
        /// </summary>
        BaseLineSelectionType BaseLineSelectionType => this.enumRadioControl_BaseLineSelectionType.GetCurrent<BaseLineSelectionType>();
        /// <summary>
        /// 自动更新目录
        /// </summary>
        bool IsAutoUpdateProjDirectory => checkBox_autoUpdateProjDir.Checked;
        /// <summary>
        /// PPP更新源文件坐标
        /// </summary>
        bool IsReplaceApproxCoordWhenPPP => checkBox_replaceApproxCoordWhenPPP.Checked;
        /// <summary>
        /// 共同时段
        /// </summary>
        TimeSpan MinCommonSpan => TimeSpan.FromMinutes(this.namedFloatControl_periodSpanMinutes.GetValue());
        /// <summary>
        /// 工程目录
        /// </summary>
        public string ProjectDirectory { get => directorySelectionControl1_projDir.Path; set => directorySelectionControl1_projDir.Path = value; }
        /// <summary>
        /// 独立基线选择方法
        /// </summary>
        IndependentLineSelectType IndependentLineSelectType => this.enumRadioControl_selectLineType.GetCurrent<IndependentLineSelectType>();

        /// <summary>
        /// 是否已经初始化完毕，可以联动显示
        /// </summary>
        private bool IsInited { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        GnssProcessOption Option => multiSolverOptionControl1.GetCurrentOption();
        /// <summary>
        /// 最长短基线
        /// </summary>
        double MaxShortLineLength => namedFloatControl_longShortDividLength.GetValue();
        /// <summary>
        /// 算法设置
        /// </summary>
        Dictionary<GnssSolverType, GnssProcessOption> Options => multiSolverOptionControl1.Options;
        /// <summary>
        /// 自动基线类型
        /// </summary>
        AutoBaseLinSolveType AutoBaseLinSolveType => enumRadioControl_autoBaseLinSolveType.GetCurrent<AutoBaseLinSolveType>();
        /// <summary>
        /// 基线类型
        /// </summary>
        GnssSolverType GnssSolverType => multiSolverOptionControl1.GnssSolverType;
        /// <summary>
        /// 并行计算选项
        /// </summary>
        public IParallelConfig ParallelConfig { get { return parallelConfigControl1; } }
        /// <summary>
        /// 树形目录下选择的测站信息
        /// </summary>
        /// <returns></returns>
        private ObsSiteInfo GetCurrentObsSiteInfo() { return Geo.Utils.TreeNodeUtil.GetCurrentObject<ObsSiteInfo>(this.treeView_sites.SelectedNode); }
        /// <summary>
        /// 当前测站名称
        /// </summary>
        /// <returns></returns>
        private string GetCurrentSite() { return Geo.Utils.TreeNodeUtil.GetCurrentObject<string>(this.treeView_sites.SelectedNode); }

        #region 显示 
        int currentVectorIndex = -1;
        private void ShowObjectAttributeInfo<T>(T obj)
        {
            if (!IsInited || obj == null) { return; }
            attributeBox1.SetObject<T>(obj, false);
        }

        /// <summary>
        /// 文件名称生成器,此处目录没有使用。
        /// </summary>
        ResultFileNameBuilder resultFileNameBuilder => new ResultFileNameBuilder(this.ProjectDirectory);
        private List<EstimatedBaseline> GetAllEstLines() { return this.Manager.BaseLineManager.GetAllEstLines(); }
        private List<ObsSiteInfo> GetAllSites() { return this.Manager.SiteManager.GetAllSites(); }
        private List<SiteObsBaseline> GetAllBaseLines() { return this.Manager.BaseLineManager.GetAllBaseLines(); }
        /// <summary>
        /// 自动基线解算器
        /// </summary>
        AutoBaseLineSolver AutoBaseLineSolver { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        string ProjectName => namedStringControl_projName.GetValue();
        /// <summary>
        /// 当前基线解算任务
        /// </summary>
        List<SiteObsBaseline> CurrentBaselinesToBeSolve { get; set; }
        private SiteObsBaseline GetCurrentBaseLine() { return this.bindingSource_allLines.Current as SiteObsBaseline; }
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
        #endregion

        #region 对象显示绑定
        public void UiToEntity()
        {
            foreach (var opt in Options.Values)
            {
                opt.OutputDirectory = ProjectDirectory;
                //Path.Combine(ProjectDirectory, opt.GnssSolverType.ToString());
                opt.ObsPhaseType = this.enumRadioControl_obsType.GetCurrent<ObsPhaseType>();
            }
        }

        public void EntityToUi()
        {
            //工程目录以界面为准
            //ProjectDirectory = this.Option.OutputDirectory;

            //    this.enumRadioControl_obsType.SetCurrent<ObsPhaseType>(this.Option.ObsPhaseType);
            //        this.enumRadioControl1_GnssSolverType.SetCurrent(this.Option.GnssSolverType);
        }
        #endregion

        #region 文件导入
        private void 导入文件IToolStripMenuItem_Click(object sender, EventArgs e) { if (openFileDialog1_rinexOFile.ShowDialog() == DialogResult.OK) { InpportFiles(openFileDialog1_rinexOFile.FileNames); } }

        private void InpportFiles(string[] filePathes)
        {
            if (filePathes.Length == 0) { return; }
            CheckOrSetProjectDirectoryAndName(filePathes);

            //尝试添加，并返回成功的
            List<SiteObsBaseline> newAddedBaseLines = null;
            Manager.SiteManager.MinEpochTimeSpan = MinCommonSpan;//更新
            if (BaseLineSelectionType == BaseLineSelectionType.全基线)
            {
                var oks = this.Manager.Add(filePathes, out newAddedBaseLines);
                if (oks.Count != filePathes.Length)
                {
                    var failedCount = filePathes.Length - oks.Count;
                    MessageBox.Show(failedCount + " 个文件没有添加成功，因为在相同时段已经包含了相同测站名称的文件或观测时段太短，请手动移除该站后再试。\r\n");
                }
            }
            else //重新生成
            {
                this.Manager.Add(filePathes);
                newAddedBaseLines = this.Manager.RebuildBaseLineManager(BaseLineSelectionType, this.Option.CenterSiteName, this.Option.BaseLineFilePath);
            }

            TrySetBaseLineValues(newAddedBaseLines);
            //绑定显示
            DataBind(true);
        }
        //检查并设置工程目录和名称
        private void CheckOrSetProjectDirectoryAndName(string[] filePathes)
        {
            var dir = Path.GetDirectoryName(filePathes[0]);
            var projName = Path.GetFileName(dir);
            if (projName != ProjectName && ProjectDirectory != Path.Combine(dir, Setting.GnsserProjectDirectoryName))
            {
                if (IsAutoUpdateProjDirectory)
                {
                    this.ProjectDirectory = Path.Combine(dir, Setting.GnsserProjectDirectoryName);      
                    Geo.Utils.FileUtil.CheckOrCreateDirectory(ProjectDirectory);
                    this.namedStringControl_projName.SetValue(projName);
                    log.Info("当前工程名称已替换为 “" + projName    + "”，输出目录为 " + dir);
                }
            }
        }

        private void 按照设置重新生成基线GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RebuildBaseLinesAndTrySetValues();
            //绑定显示
            DataBind(true);
        }

        private void RebuildBaseLinesAndTrySetValues()
        {
            var newAddedBaseLines = this.Manager.RebuildBaseLineManager(BaseLineSelectionType, this.Option.CenterSiteName, this.Option.BaseLineFilePath);
            TrySetBaseLineValues(newAddedBaseLines);
        }
        /// <summary>
        /// 尝试设置计算值
        /// </summary>
        /// <param name="newAddedBaseLines"></param>
        private void TrySetBaseLineValues(List<SiteObsBaseline> newAddedBaseLines)
        {
            if (newAddedBaseLines.Count > 0) //添加成功了
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox(
                        "是否根据当前设置，查找并匹配当前工程已经计算的结果？" +
                        " 自动解算策略：" + this.AutoBaseLinSolveType
                        + ", 单一计算或短基线类型：" + this.GnssSolverType + " ") == DialogResult.Yes)
                {
                    CheckOrSetEstResult(newAddedBaseLines);
                }
            }
        }

        /// <summary>
        /// 检查，并附加计算结果。
        /// </summary>
        /// <param name="newAddedLines"></param>
        private void CheckOrSetEstResult(List<SiteObsBaseline> newAddedLines)
        {
            //建立一个辅助时段基线管理器
            MutliPeriodBaseLineManager siteObsBaselines = new MutliPeriodBaseLineManager(newAddedLines, this.MinCommonSpan);            

            //分时段处理
            foreach (var kv in siteObsBaselines.KeyValues)
            {
                var netPeriod = kv.Key;
                var mangager = kv.Value;
                CheckOrSetEstResult(newAddedLines, netPeriod);
            }
        }
        /// <summary>
        /// 检查，并附加计算结果。
        /// </summary>
        /// <param name="newAddedLines"></param>
        /// <param name="netPeriod"></param>
        private void CheckOrSetEstResult(List<SiteObsBaseline> newAddedLines, TimePeriod netPeriod)
        {
            var netPer = this.Manager.BaseLineManager.GetPeriodKey(netPeriod);//获取网名称

            string path = "";
            var solverDir = this.GetCurrentSolverDirectory(netPer);//默认目录
            var pathes = Directory.GetFiles(solverDir, "*" + Setting.BaseLineFileExtension);
            if (pathes.Length > 0)
            {
                path = pathes[0];
            }
            //若没有默认 无电离层双差 ，且 按基线长度采用不同算法
            string pathLong = "";
            if (this.GnssSolverType != GnssSolverType.无电离层双差 && this.AutoBaseLinSolveType == AutoBaseLinSolveType.按基线长度采用不同算法)
            {
                solverDir = this.Option.GetSolverDirectory(netPeriod, GnssSolverType.无电离层双差);//默认目录
                pathes = Directory.GetFiles(solverDir, "*" + Setting.BaseLineFileExtension);
                if (pathes.Length > 0)
                {
                    pathLong = pathes[0];
                }
            }

            if ((File.Exists(path) || File.Exists(pathLong)))
            {
                List<string> pathList = new List<string>();
                if (File.Exists(path))
                {
                    pathList.Add(path);
                }
                if (File.Exists(pathLong))
                {
                    pathList.Add(pathLong);
                }
                var tables = ObjectTableManager.Read(pathList.ToArray(), ".");
                //合并所有的表格
                var rawTable = tables.Combine();
                var BaseLineNets = BaseLineNetManager.Parse(rawTable, MinCommonSpan.TotalMinutes);//时段网

                var currentPeriodLines = SiteObsBaseline.GetLinesInPeriod(newAddedLines, netPeriod, this.MinCommonSpan);

                foreach (var line in currentPeriodLines)
                {
                    var est = BaseLineNets.GetFirst(line.LineName);
                    if (est != null)
                    {
                        line.EstimatedResult = est;
                    }
                }
            }
        }

        #endregion
        /// <summary>
        /// 解算后是否及时异步环
        /// </summary>
        bool IsSolveAsyncClosure => checkBox_solveAsyncClosure.Checked;
        #region 数据绑定显示
        /// <summary>
        /// 数据绑定显示
        /// </summary>
        private void DataBind(bool bindNavTree = false)
        {
            IsInited = false;

            BuildAndSetClosureError();

            ////重新计算闭合差和显示基线
            //BindResultBuildAndSetClosureError();
            //等闭合差计算后再显示
            if (bindNavTree)
            {
                BindLeftNavMenu();
            }

            IsInited = true;
        }

        /// <summary>
        /// 绑定左边树形目录，列表菜单
        /// </summary>
        private void BindLeftNavMenu()
        {
            BindSitesToTree();

            BindLineToTree();

            BindPeriodTree();

            BindLineList();
        }

        #region 绑定细节
        private void BindLineList()
        {
            var lineobjs = Manager.BaseLineManager.AllLineObjs;
            lineobjs.Sort();
            this.bindingSource_allLines.DataSource = lineobjs;
            this.label_listAll.Text =  "异名基线：" + Manager.BaseLineManager.CountOfDifferLineName
                + ",总基线：" + Manager.BaseLineManager.TotalLineCount; 
        }

        private void BindPeriodTree()
        {
            TreeNode topNode = new TreeNode("时段");
            var periods = this.Manager.BaseLineManager.TimePeriods;
            periods.Sort();
            foreach (var period in periods)
            {
                string periodName = BuidPeriodName(period);
                var node = new TreeNode(periodName) { Tag = period };
                topNode.Nodes.Add(node);

                var lines = this.Manager.BaseLineManager.Get(period);
               
                foreach (var line in lines)
                {
                    var name = Geo.Utils.StringUtil.FillSpaceRight( line.ToString(), 13) + " " + BuidPeriodName(line.TimePeriod);

                    var subNode = new TreeNode(name) { Tag = line };
                    node.Nodes.Add(subNode);
                }
            }
            treeView_periods.Nodes.Clear();
            treeView_periods.Nodes.Add(topNode);
            topNode.Expand();

            this.label_periods.Text = "时段：" + periods.Count
                + ",异名基线数：" + Manager.BaseLineManager.CountOfDifferLineName
                + ",总基线数：" + Manager.BaseLineManager.TotalLineCount; 
        }


        private void BindLineToTree()
        {
            TreeNode topNode = new TreeNode("基线");
            var lineNames = Manager.BaseLineManager.LineNames;
            lineNames.Sort();

            foreach (var lineName in lineNames)
            {
                var node = new TreeNode(lineName.ToString()) { Tag = lineName };
                topNode.Nodes.Add(node);

                var lines = Manager.BaseLineManager.GetLines(lineName);
        //        lines.Sort();
               foreach (var line in lines)
                {
                    var name = BuidPeriodName(line.TimePeriod);

                    var subNode = new TreeNode(name) { Tag = line };
                    node.Nodes.Add(subNode);
                }
            }
            treeView_lines.Nodes.Clear();
            treeView_lines.Nodes.Add(topNode);
            topNode.Expand();
            this.label_lineInfo.Text = "时段：" + Manager.BaseLineManager.Count 
                + ",异名基线：" + Manager.BaseLineManager.CountOfDifferLineName
                + ",总基线：" + Manager.BaseLineManager.TotalLineCount; ;
        }

        private void BindSitesToTree()
        {
            TreeNode topNode = new TreeNode("测站");
            var sites = Manager.SiteManager.SiteNames;
            sites.Sort();
            foreach (var siteName in sites)
            {
                var node = new TreeNode(siteName) { Tag = siteName };
                topNode.Nodes.Add(node);

                var periods = Manager.SiteManager.GetSites(siteName);
                periods.Sort();
                foreach (var site in periods)
                {
                    var name = Geo.Utils.StringUtil.FillSpaceRight( site.FileName, 10) + " " + BuidPeriodName(site.TimePeriod);
                    var siteN = new TreeNode(name) { Tag = site };
                    node.Nodes.Add(siteN);
                }
            }
            treeView_sites.Nodes.Clear();
            treeView_sites.Nodes.Add(topNode);
            topNode.Expand();
            label_siteInfo.Text = "时段：" + Manager.SiteManager.PeriodCount
                + ",测站：" + Manager.SiteManager.SiteCount + ",文件：" + Manager.SiteManager.FileCount;
        }
        private static string BuidPeriodName(TimePeriod period)
        {
            return period.ToDefualtPathString();
        }
        #endregion
        #endregion

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView_sites.Nodes.Clear();
            Manager.Clear();

            DataBind(true);
        }

        #endregion

        #region 测站 基线基本 操作
        private void 打开当前文件FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentObsSiteInfo(); if (site == null) { return; }
            Geo.Utils.FileUtil.OpenFile(site.FilePath);
        }

        private void 打开所在目录OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentObsSiteInfo(); if (site == null) { return; }
            Geo.Utils.FileUtil.OpenDirectory(site.Directory);
        }

        private void 查看载波残差LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochResidualFileName(line.LineName.Name));
            GnsserFormUtil.ShowPhaseChartForm(path);
        }
        bool Check(SiteObsBaseline line, bool mustHasResult = true)
        {
            if (line == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中基线再试！"); return false; }
            if (mustHasResult && line.EstimatedResult == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！" + line.LineName); return false; }
            return true;
        }

        private void 查看历元收敛图EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
            var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
            GnsserFormUtil.ShowChartForm(path, paramNames);
        }

        private void 查看历元参数RMS图EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamRmsFileName(line.LineName.Name));
            var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.Dx, ParamNames.Dy, ParamNames.Dz };
            GnsserFormUtil.ShowChartForm(path, paramNames);
        }

        private void 设置后绘产出图SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochResidualFileName(line.LineName.Name));
            GnsserFormUtil.SetThenShowChartForm(path);
        }
        private void 设置后绘历元参数收敛图DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
            GnsserFormUtil.SetThenShowChartForm(path);
        }

        private void 设置后绘制RMS图RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (!Check(line)) { return; }

            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamRmsFileName(line.LineName.Name));
            GnsserFormUtil.SetThenShowChartForm(path);
        }

        private void 查看所有基线残差图AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lines = this.GetAllBaseLines();
            var first = lines.FirstOrDefault();
            if (first == null || first.EstimatedResult == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！" + first.LineName); return; }

            foreach (var line in lines)
            {
                var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochResidualFileName(line.LineName.Name));
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
                var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
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
                var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                GnsserFormUtil.ShowChartForm(path, paramNames);
            }
        }

        private void 打开历元参数文件PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; }
            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
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

        private void 打开残差文件RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; }
            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(line.LineName.Name));
            TryOpenTableForm(path);
        }

        private void 打开RMS文件RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (!Check(line)) { return; }
            var path = Path.Combine(this.Option.GetSolverDirectory(line.NetPeriod), resultFileNameBuilder.BuildEpochParamRmsFileName(line.LineName.Name));
            TryOpenTableForm(path);
        }

        private void 查看编辑基线测站时段EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (line == null) { return; }

            EditBaseLineObsFile(line); 
        }

        private void 查看基线时段图PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine();
            if (line == null) { return; }
            List<ObsSiteInfo> sites = new List<ObsSiteInfo>() { line.Start, line.End };
            ShowSiteObsPhases(sites);
        }

        private void 查看编辑所选测站时段EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = this.GetCurrentObsSiteInfo(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            site.CheckOrCopyToTempDirectory();
            EditObsFile(site);
        }


        private void 查看编辑所有测站时段AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            foreach (var site in sites)
            {
                site.CheckOrCopyToTempDirectory(); 
                EditObsFile(site);
            }
        }

        private void 恢复原始文件RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentObsSiteInfo(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否要恢复" + site.SiteName + "的修改？ ") == DialogResult.Yes)
            {
                site.CopyToTempDirectory();
            }
        }

        private void 查看卫星高度角HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentObsSiteInfo(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            Thread thread = new Thread(new ParameterizedThreadStart(GnsserFormUtil.OpenSatElevationTableForm));
            thread.Start(site.FilePath);
        }
        private void button_showCurentLine_Click(object sender, EventArgs e)
        {
            var lines = this.GetCurrentBaseLine();
            if (lines == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(new List<EstimatedBaseline>() { (EstimatedBaseline)lines.EstimatedResult });

            ShowLinesOnMap(BaseLineNet);
        }
        private void 地图显示测站SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.Manager.SiteManager.GetAllSites();
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
            var sites = this.Manager.SiteManager.GetAllSites();
            ShowSiteObsPhases(sites);
        }
        public static void ShowSiteObsPhases(List<ObsSiteInfo> sites)
        {
            var timeperiods = new Dictionary<string, TimePeriod>();
            foreach (var item in sites) { if (File.Exists(item.FilePath)) { timeperiods[item.FileName] = RinexObsFileReader.ReadPeriod(item.FilePath); } }
            EpochChartForm chartForm = new EpochChartForm(timeperiods, "测站时段图");
            chartForm.Show();
        }

        private void button_showAllLineeOnMap_Click_1(object sender, EventArgs e)
        {
            var lines = GetAllEstLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);

            ShowLinesOnMap(BaseLineNet);
        }
        private void ShowLinesOnMap(BaseLineNet BaseLineNet)
        {
            var lines = BaseLineNet.Values;
            ShowLinesOnMap(lines);
        }

        private void ShowLinesOnMap(List<EstimatedBaseline> lines)
        {
            List<LineString> lineStrings = new List<LineString>();
            foreach (var line in lines)
            {
                LineString lineString = BuildLineString(line);
                lineStrings.Add(lineString);
            }
            if (lineStrings.Count == 0) { return; }

            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "基线");
            ShowLayer(layer);
        }
        private void ShowLinesOnMap(List<SiteObsBaseline> lines)
        {
            List<LineString> lineStrings = new List<LineString>();
            foreach (var line in lines)
            {
                LineString lineString = BuildLineString(line);
                lineStrings.Add(lineString);
            }
            if (lineStrings.Count == 0) { return; }

            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "基线");
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
        private LineString BuildLineString(SiteObsBaseline baseLine)
        {
            var name = baseLine.LineName.RovName;
            var startGeoCoord = CoordTransformer.XyzToGeoCoord(baseLine.Start.SiteObsInfo.ApproxXyz);
            var ptA = new AnyInfo.Geometries.Point(startGeoCoord, null, name);
            name = baseLine.LineName.RefName;
            var geoCoord = CoordTransformer.XyzToGeoCoord(baseLine.End.SiteObsInfo.ApproxXyz);
            var ptB = new AnyInfo.Geometries.Point(geoCoord, null, name);
            var lineString = new LineString(new List<AnyInfo.Geometries.Point>()
                        {
                            ptA, ptB
                        }, baseLine.LineName.Name);
            return lineString;
        }
        private void button_saveAllAsGNSSerFile_Click(object sender, EventArgs e)
        {
            var lines = GetAllEstLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            string name = BuildLineFileNameNoExtension(lines.Count, "_All");
            SaveGnsserBaselineResult(lines, name);
        }

        private void button_saveAllAsLgoAsc_Click(object sender, EventArgs e)
        {
            var lines = GetAllBaseLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            
            string name = BuildLineFileNameNoExtension(lines.Count, "_All");
            SaveAsLeoAsc(lines, name);
        }
        //输出产品名称
        private string BuildLineFileNameNoExtension(int count, string append = "_All")
        {
            var name = ProjectName + "_" + count + "_" + this.Option.GnssSolverType;
            if (this.Option.GnssSolverType.ToString().Contains("单频"))
            {
                name += "_" + this.Option.ObsPhaseType;
            }
            name += append;
            return name;
        }
        private void SaveGnsserBaselineResult(MultiPeriodBaseLineNet BaseLineNet, string name)
        {
            ObjectTableStorage table = BaseLineNet.GetLineTable();
            var outpath = this.resultFileNameBuilder.BuildBaseLineResulPath(name);

            ObjectTableWriter.Write(table, outpath);
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(ProjectDirectory);
        }
        private void SaveGnsserBaselineResult(List<EstimatedBaseline> BaseLineNet, string name)
        {
            ObjectTableStorage table = EstimatedBaseline.GetLineTable(BaseLineNet, "基线名称");
            var outpath = this.resultFileNameBuilder.BuildBaseLineResulPath(name);

            ObjectTableWriter.Write(table, outpath);
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(ProjectDirectory);
        }

        private void SaveAsLeoAsc(List<SiteObsBaseline> BaseLineNet, string name)
        {
            var managers = this.Manager.BaseLineManager
                .ExtractPeriodLines(BaseLineNet).GetOrBuildPeriodBaseLineNet(true);
            SaveAsLeoAsc(managers, name);
        }
        private void SaveAsLeoAsc(MultiPeriodBaseLineNet BaseLineNet, string name)
        {
            BaseLineFileConverter converter = new BaseLineFileConverter();
            List<EpochLgoAscBaseLine> files = new List<EpochLgoAscBaseLine>();
            foreach (var item in BaseLineNet)
            {
                var outnet = converter.Build(item);
                files.Add(outnet);
            }
            var outpath = Path.Combine(this.ProjectDirectory, name + Setting.BaseLineFileOfLgoExtension);

            LgoAscBaseLineFileWriter writer = new LgoAscBaseLineFileWriter(outpath);
            writer.Write(files);
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(ProjectDirectory);
        }


        private void button_viewSelectedTriPathes_Click(object sender, EventArgs e)
        {
            var triangles = Geo.Utils.DataGridViewUtil.GetSelectedRows(objectTableControl_allClosureErrors.DataGridView);
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
        private void ShowLinesOnMap(List<GnssBaseLineName> lineNames)
        {
            var lines = GetAllEstLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);

            List<LineString> lineStrings = new List<LineString>();
            foreach (var lineName in lineNames)
            {
                var line = BaseLineNet.GetOrReversed(lineName);
                if (line == null) { continue; }

                LineString lineString = BuildLineString(line);
                lineStrings.Add(lineString);
            }
            if (lineStrings.Count == 0) { return; }

            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "三角网");
            ShowLayer(layer);
        }

        private void button_showIndeLineOnMap_Click(object sender, EventArgs e)
        {
            var BaseLineNet = objectTableControl_independentLine.Tag as MultiPeriodBaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先生成独立基线再试！"); return; }

            ShowLinesOnMap(BaseLineNet.GetAllLines());
        }

        private void button_saveIndeBaselineFile_Click(object sender, EventArgs e)
        {
            var BaseLineNet = objectTableControl_independentLine.Tag as MultiPeriodBaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先生成独立基线再试！"); return; }
            string name = BuildLineFileNameNoExtension(BaseLineNet.Count, "_Indpt");
            SaveGnsserBaselineResult(BaseLineNet, name);
        }

        private void button_saveIndeToLeoAsc_Click(object sender, EventArgs e)
        {
            var BaseLineNet = objectTableControl_independentLine.Tag as MultiPeriodBaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先生成独立基线再试！"); return; }
            string name = BuildLineFileNameNoExtension(BaseLineNet.Count, "_Indpt");
            SaveAsLeoAsc(BaseLineNet, name);
        }
        private void button_runIndependentLine_Click(object sender, EventArgs e)
        {
            var nets = this.Manager.BaseLineManager.BuidIndependentNets(IndependentLineSelectType);

            if (nets == null || nets.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }

            objectTableControl_independentLine.Tag = nets;
            objectTableControl_independentLine.DataBind(nets.GetLineTable());
        }

        #endregion


        private void treeView_sites_MouseDoubleClick(object sender, MouseEventArgs e) { 打开当前文件FToolStripMenuItem_Click(sender, e); }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var siteName = GetCurrentSite();
            if (siteName != null)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("将移除测站 " + siteName + " 下的所有文件， 及对应基线和计算的结果，移除后可以重新载入,\r\n 确定移除？") == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Manager.Remove(siteName); 
                    DataBind(true);
                }
            }
            else
            {
                var site = GetCurrentObsSiteInfo(); if (site == null) { return; }

                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("将移除测站文件 " + site.FileName + " 及对应基线和计算的结果，移除后可以重新载入,\r\n 确定移除？") == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Manager.Remove(site); 
                    DataBind(true);
                }
            }

        }

        private void 移除所选基线RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetSelectedLines();
            this.Manager.BaseLineManager.Remove(line); 
            this.DataBind(true);
        }
        #region 树形展开与收缩        
        private void 展开收缩时段EToolStripMenuItem_Click(object sender, EventArgs e) { Geo.Utils.TreeNodeUtil.TriggerExpandAll(treeView_periods.TopNode); }
        private void 展开收缩测站EToolStripMenuItem_Click(object sender, EventArgs e) { Geo.Utils.TreeNodeUtil.TriggerExpandAll(treeView_sites.TopNode); }
        private void 展开收缩所有基线EToolStripMenuItem_Click(object sender, EventArgs e) { Geo.Utils.TreeNodeUtil.TriggerExpandAll(treeView_lines.TopNode); }
        #endregion

        #region 显示当前属性
        private ObsSiteInfo currentShowing { get; set; }
        private void treeView_sites_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowCurrentTreeNodeAttribute(treeView_sites);

            var site = this.GetCurrentObsSiteInfo();
            if (currentShowing == null || currentShowing != site)
            {
                this.DataBindCurrentPppSite(site);
                currentShowing = site;
            }
        }


        private void listBox_vector_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();

                var currentItem = this.listBox_vector.Items[e.Index];
                var item = currentItem as SiteObsBaseline;
                // 判断是什么类型的标签
                ResultBackGroundStyle backGroundStyle = new ResultBackGroundStyle(item.ResultState);
                e.Graphics.FillRectangle(backGroundStyle.BackGroundBrush, listBox_vector.GetItemRectangle(e.Index));
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(backGroundStyle.BackGroundBrushSelected, listBox_vector.GetItemRectangle(e.Index));
                }
                // 焦点框
                e.DrawFocusRectangle();
                //文本  
                var name = Geo.Utils.StringUtil.FillSpaceRight(item.LineName.Name, 14) + " " + item.TimePeriod.ToDefualtPathString();
                e.Graphics.DrawString(name, e.Font, new SolidBrush(listBox_vector.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }

        }
        private void treeView_periods_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowCurrentTreeNodeAttribute(treeView_periods);
            var currentLine = Geo.Utils.TreeNodeUtil.GetCurrentObject<SiteObsBaseline>(e.Node);
            DataBindCurrentLine(currentLine);
        }
        private void treeView_lines_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowCurrentTreeNodeAttribute(treeView_lines);
            var currentLine = Geo.Utils.TreeNodeUtil.GetCurrentObject<SiteObsBaseline>(e.Node);
            DataBindCurrentLine(currentLine);
        }


        private void listBox_vector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsInited) { return; }
            if (listBox_vector.SelectedItem == null) { return; }
            ShowObjectAttributeInfo(listBox_vector.SelectedItem);

            if (currentVectorIndex == listBox_vector.SelectedIndex) { return; }

            currentVectorIndex = listBox_vector.SelectedIndex;

            var currentLine = this.GetCurrentBaseLine();

            DataBindCurrentLine(currentLine);
        }


        /// <summary>
        /// 绑定数据表，并计算闭合差
        /// </summary>
        private void BuildAndSetClosureError()
        {

            if (this.Manager.BaseLineManager.Count == 0) { return; }

            //同步环闭合差
            var syncTriguilars = this.Manager.BaseLineManager.GetOrCreatePeriodSyncTriguilarNetQualitiyManager(this.Option.GnssReveiverNominalAccuracy, true);
            var syncCloureTable = syncTriguilars.GetObjectTable();
            objectTableControl_allClosureErrors.DataBind(syncCloureTable);

            //计算闭合差，根据闭合差设置颜色  
            foreach (var periodLines in this.Manager.BaseLineManager.KeyValues)
            {
                var period = periodLines.Key;
                var currentSyncPeriodManager = syncTriguilars[period];

                foreach (var lineKv in periodLines.Value.KeyValues)
                {
                    var line = lineKv.Value;
                    var quality = currentSyncPeriodManager.GetBest(line.LineName);
                    if (quality == null || line.EstimatedResult == null) { continue; }

                    line.ResultState = quality.ResultState;
                    line.EstimatedResult.ClosureError = quality.ClosureError.Value.Length;
                }
            }
            this.listBox_vector.Refresh();

            //基线结果显示  同步环闭合差更新后才绑定
            var baseLineTable = this.Manager.BaseLineManager.GetBaselineTable();
            this.objectTableControl_baselineResult.DataBind(baseLineTable);

            //复测基线较差
            var RepeatErrorOfBaseLineManager = this.Manager.BaseLineManager.GetOrCreateRepeatErrorOfBaseLineManager(this.Option.GnssReveiverNominalAccuracy, true);
            var repeatErrorTable = RepeatErrorOfBaseLineManager.GetObjectTable();
            this.objectTableControl_allRepeatError.DataBind(repeatErrorTable);

            //是否解算异步环
            if (IsSolveAsyncClosure)
            {
                var start = DateTime.Now;

                //3时段异步环闭合差
                var triPeriodAsyncClousureErrorOfBaseLineManager = this.Manager.BuildAsyncTriangularClosureQualies(this.Option.GnssReveiverNominalAccuracy, 3);
                var TriAsyncClousureTable = triPeriodAsyncClousureErrorOfBaseLineManager.GetObjectTable();
                this.objectTableControl_triPeiodasyncClosure.DataBind(TriAsyncClousureTable);
                //2时段异步环闭合差
                var twoPeriodAsyncClousureErrorOfBaseLineManager = this.Manager.BuildAsyncTriangularClosureQualies(this.Option.GnssReveiverNominalAccuracy, 2);
                var twoPeriodAsyncClousureTable = twoPeriodAsyncClousureErrorOfBaseLineManager.GetObjectTable();
                this.objectTableControl_twoPeriodAsybn.DataBind(twoPeriodAsyncClousureTable);

                var span = DateTime.Now - start;
                log.Info("异步环闭合差搜索共耗时： " + span.TotalMinutes.ToString("0.000") + " 分钟");
            }
        }
        #region 显示当前 

        private void DataBindCurrentLine(SiteObsBaseline currentLine)
        {
            if (currentLine == null) { return; }

            //当前基线残差图
            var path = "";
            var solverDiretory = GetCurrentSolverDirectory(currentLine);// GetCurrentSolverDirectory(currentLine.NetPeriod);
            path = Path.Combine(solverDiretory, resultFileNameBuilder.BuildEpochResidualFileName(currentLine.LineName.Name));
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));

                var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path, 2), System.IO.Path.GetFileName(path));
                var phaseObsTable = table.GetTable(name, titles);
                this.commonChartControl_currentResidual.DataBind(phaseObsTable);
            }
            else
            {
                this.commonChartControl_currentResidual.Clear();
            }

            //当前基线参数收敛图
            path = Path.Combine(solverDiretory, resultFileNameBuilder.BuildEpochParamFileName(currentLine.LineName.Name));

            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path, 2), System.IO.Path.GetFileName(path));
                var phaseObsTable = table.GetTable(name, paramNames);
                phaseObsTable.RemoveRows(0, StartCountOfResidualChart);//默认移除前40

                var phaseTable = phaseObsTable.GetTableAllColMinusLastValid();
                commonChartControl_currentParamConvergence.DataBind(phaseTable);
            }
            else
            {
                this.commonChartControl_currentParamConvergence.Clear();
            }

            if (currentLine == null || currentLine.EstimatedResult == null)
            {
                richTextBoxControl_currentLine.Text = currentLine + "没有结果";
                objectTableControl_currentLineSyncClosureErrors.Clear();
                objectTableControl_currentRepeatError.Clear();
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine(currentLine.EstimatedResult.ToReadableText());
                //反向 
                sb.AppendLine("-----------反向---------");
                sb.AppendLine(currentLine.EstimatedResult.ReversedBaseline.ToReadableText());

                richTextBoxControl_currentLine.Text = sb.ToString();


                //当前同步环闭合差显示
                var CurrentQualityManager = this.Manager.BaseLineManager.GetTriguilarNetQualitiyManager(currentLine.NetPeriod, this.Option.GnssReveiverNominalAccuracy, true);

                if (CurrentQualityManager == null) { return; }
                var quality = CurrentQualityManager.Get(currentLine.LineName);
                if (quality == null) { return; }

                ObjectTableStorage lineTable = QualityOfTriAngleClosureError.BuildSynchNetTrilateralCheckTResultable(currentLine.LineName.ToString(), quality);
                this.objectTableControl_currentLineSyncClosureErrors.DataBind(lineTable);

                //复测基线较差
                var RepeatErrorOfBaseLineManager = this.Manager.BaseLineManager.GetOrCreateRepeatErrorOfBaseLineManager(this.Option.GnssReveiverNominalAccuracy, true);
                if (RepeatErrorOfBaseLineManager == null) { return; }
                var repeatQuality = RepeatErrorOfBaseLineManager.Get(currentLine);
                if (repeatQuality == null) { return; }

                ObjectTableStorage lineRepeatTable = repeatQuality.GetObjectTable();
                this.objectTableControl_currentRepeatError.DataBind(lineRepeatTable);
            }
        }

        private void DataBindCurrentPppSite(ObsSiteInfo currentSite)
        {
            if (currentSite == null) { return; }

            //当前 残差图
            var path = "";
            var solverDiretory = GetOrCreatePppOption().GetSolverDirectory(currentSite.NetPeriod);// GetCurrentSolverDirectory(currentLine.NetPeriod);

            path = Path.Combine(solverDiretory, resultFileNameBuilder.BuildEpochResidualFileName(currentSite.SiteObsInfo.FileInfo.FileName));
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));

                var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path, 2), System.IO.Path.GetFileName(path));
                var phaseObsTable = table.GetTable(name, titles);
                this.commonChartControl_currentResidual.DataBind(phaseObsTable);

                this.objectTableControl_currentLineSyncClosureErrors.DataBind(table);
            }

            //当前 参数收敛图 
            path = Path.Combine(solverDiretory, resultFileNameBuilder.BuildEpochParamFileName(currentSite.SiteObsInfo.FileInfo.FileName));
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path, 2), System.IO.Path.GetFileName(path));
                var phaseObsTable = table.GetTable(name, paramNames);
                phaseObsTable.RemoveRows(0, StartCountOfResidualChart);//默认移除前40

                var phaseTable = phaseObsTable.GetTableAllColMinusLastValid();
                commonChartControl_currentParamConvergence.DataBind(phaseTable);


                this.objectTableControl_currentRepeatError.DataBind(table);
            }
             
            richTextBoxControl_currentLine.Text = currentSite.ToReadableText();  
        }
        #endregion

        /// <summary>
        /// 起始历元
        /// </summary>
        int StartCountOfResidualChart => namedIntControl_removeFirstEpochCount.GetValue();
        /// <summary>
        /// 当前算法窗口目录
        /// </summary>
        /// <param name="netPeriod"></param>
        /// <returns></returns>
        public string GetCurrentSolverDirectory(TimePeriod netPeriod)
        {
            this.Option.OutputDirectory = this.ProjectDirectory;
            return Option.GetSolverDirectory(netPeriod);
        }
        /// <summary>
        /// 当前算法窗口目录,支持 按基线长度采用不同算法
        /// </summary>
        /// <param name="baseLine"></param>
        /// <returns></returns>
        public string GetCurrentSolverDirectory(SiteObsBaseline baseLine)
        {
            this.Option.OutputDirectory = this.ProjectDirectory;
            var gnssSolverType = this.Option.GnssSolverType;
            if (this.AutoBaseLinSolveType == AutoBaseLinSolveType.按基线长度采用不同算法 && baseLine.GetLength() > MaxShortLineLength)
            {
                gnssSolverType = GnssSolverType.无电离层双差;
            }
            if(baseLine.EstimatedResult != null)
            {
                gnssSolverType = baseLine.EstimatedResult.GnssSolverType;
            }

            return Option.GetSolverDirectory(baseLine.NetPeriod, gnssSolverType);
        }

        private void ShowCurrentTreeNodeAttribute(TreeView treeView_periods)
        {
            if (!IsInited) { return; }
            if (treeView_periods.SelectedNode == null) { return; }
            ShowObjectAttributeInfo(treeView_periods.SelectedNode.Tag);
        }
        #endregion


        private void 解算所选基线SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentBaselinesToBeSolve = GetSelectedLines();
            if (CurrentBaselinesToBeSolve.Count == 0) { return; }
            SolveBaseLines(CurrentBaselinesToBeSolve);
        }

        private void 解算所有基线AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentBaselinesToBeSolve = Manager.BaseLineManager.AllLineObjs;
            if (CurrentBaselinesToBeSolve.Count == 0) { return; }
            SolveBaseLines(CurrentBaselinesToBeSolve);
        }
        #region 基线解算

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var lines = this.Manager.BaseLineManager.ExtractPeriodLines(CurrentBaselinesToBeSolve);
            AutoBaseLineSolver.Solve(lines);

        }
        private void SolveBaseLines(List<SiteObsBaseline> toSolves)
        {
            if (backgroundWorker1.IsBusy)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("正在计算中.....请稍等");
            }
            else
            { 
                UiToEntity();
                AutoBaseLineSolver.Init(AutoBaseLinSolveType, GnssSolverType, Options, MaxShortLineLength);
                AutoBaseLineSolver.ParallelConfig = ParallelConfig;

                this.CurrentBaselinesToBeSolve = toSolves;

                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        #endregion
        private void checkBox_enableNet_CheckedChanged(object sender, EventArgs e) { Setting.EnableNet = checkBox_enableNet.Checked; }

        private void 移除所选时段基线RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.treeView_periods.SelectedNode;
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择节点后再试！"); return; }
            var timePeiod = Geo.Utils.TreeNodeUtil.GetCurrentObject<TimePeriod>(node);
            var lineObj = Geo.Utils.TreeNodeUtil.GetCurrentObject<SiteObsBaseline>(node, true); 
            if (timePeiod != null)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否删除 " + timePeiod + " 所有的基线？") == DialogResult.Yes)
                {
                    this.Manager.Remove(timePeiod); 
                    this.DataBind(true);
                }
            }
            if (lineObj != null)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否删除基线 " + lineObj + " " + lineObj.TimePeriod + " ？") == DialogResult.Yes)
                {
                    this.Manager.Remove(lineObj); 
                    this.DataBind(true);
                }
            }
        }

        private void 删除所选基线DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.treeView_lines.SelectedNode;
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择基线节点后再试！"); return; }

            var lineName = Geo.Utils.TreeNodeUtil.GetCurrentObject<GnssBaseLineName>(node, true);
            var lineObj = Geo.Utils.TreeNodeUtil.GetCurrentObject<SiteObsBaseline>(node, true);

            if (lineName == null && lineObj == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择基线节点后再试！"); return; }

            if (lineName != null)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否删除 " + lineName + " 所有时段的基线？") == DialogResult.Yes)
                {
                    this.Manager.Remove(lineName);
                }
            }

            if (lineObj != null)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否删除基线 " + lineObj + " " + lineObj.TimePeriod + " ？") == DialogResult.Yes)
                {
                    this.Manager.Remove(lineObj);
                }
            } 
            this.DataBind(true);
        }

        private void 解算所选树形基线SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.treeView_lines.SelectedNode;
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择基线节点后再试！"); return; }

            var lineObjs = Geo.Utils.TreeNodeUtil.GetCurrentSubObjects<SiteObsBaseline>(node);
            if (lineObjs == null || lineObjs.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择基线节点后再试！"); return; }

            this.SolveBaseLines(lineObjs);
            this.DataBind(true);

        }

        private void 解算所选时段基线SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.treeView_periods.SelectedNode;
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择基线节点后再试！"); return; }

            var lineObjs = Geo.Utils.TreeNodeUtil.GetCurrentSubObjects<SiteObsBaseline>(node);
            if (lineObjs == null || lineObjs.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择基线节点后再试！"); return; }

            this.SolveBaseLines(lineObjs);
            this.DataBind();
        }

        private void treeView_lines_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            UpdateLineTreeBackGround((TreeView)sender, e);
        }


        private void treeView_periods_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            UpdateLineTreeBackGround((TreeView)sender, e);
        }
        /// <summary>
        /// 更新树形基线背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLineTreeBackGround(TreeView sender, DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusBounds = e.Node.Bounds;
                    focusBounds.Size = new Size(focusBounds.Width - 1,
                    focusBounds.Height - 1);
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }

            //不可见或非基线时，绘制将出现在顶端
            if (!e.Node.IsVisible || !(e.Node.Tag is SiteObsBaseline))
            {
                e.DrawDefault = true;
                return;
            }

            //     log.Error(e.Node.Text + " - " + e.State);
            if ((e.Node.Tag is SiteObsBaseline))
            {
                var line = e.Node.Tag as SiteObsBaseline;
                ResultBackGroundStyle backGroundStyle = new ResultBackGroundStyle(line.ResultState);
                if (sender.SelectedNode == e.Node)
                {
                    e.Graphics.FillRectangle(backGroundStyle.BackGroundBrushSelected, e.Bounds); 
                }
                else
                {
                    e.Graphics.FillRectangle(backGroundStyle.BackGroundBrush, e.Bounds);
                }
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = sender.Font;
                //文本  
                //e.Graphics.DrawString(e.Node.Text, nodeFont, new SolidBrush(sender.ForeColor), e.Bounds, StringFormat.GenericDefault);
                e.Graphics.DrawString(e.Node.Text, nodeFont, new SolidBrush(sender.ForeColor), Rectangle.Inflate(e.Bounds, 2, 0));
              //  e.Graphics.DrawRectangle(Pens.AliceBlue, e.Bounds);
            }
        }
        #region PPP更新头坐标
        GnssProcessOption PppOption { get; set; }
        GnssProcessOption GetOrCreatePppOption()
        {
            var pppOption = PppOption != null ? PppOption : GnssProcessOption.GetDefaultIonoFreePppOption();
            pppOption.OutputDirectory = this.ProjectDirectory;// Path.Combine(, pppOption.GnssSolverType.ToString());
            PppOption = pppOption;
            return pppOption;
        }
        private void pPP计算并更新所选头文件SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var siteName = GetCurrentSite();
            if (siteName != null)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("确定采用PPP更新测站 " + siteName + " 下的所有文件？") == System.Windows.Forms.DialogResult.Yes)
                {
                    var sites = this.Manager.SiteManager.GetSites(siteName);
                    if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中有文件的测站再试！"); return; }

                    //在主线程中执行报错
                    var t1 = new System.Threading.Tasks.Task(() => RunPpp(sites));
                    t1.Start();
                }
            }
            else
            {
                var site = GetCurrentObsSiteInfo(); if (site == null) { return; }

                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("确定采用PPP更新文件 " + site.FileName + "？") == System.Windows.Forms.DialogResult.Yes)
                {
                    var sites = new List<ObsSiteInfo>() { site };
                    //在主线程中执行报错
                    var t1 = new System.Threading.Tasks.Task(() => RunPpp(sites));
                    t1.Start();
                }
            }

        }

        private void 所有文件PPP并更新头文件MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }

            //在主线程中执行报错
            var t1 = new System.Threading.Tasks.Task(() => RunPpp(sites));
            t1.Start();

        }
        private void 查看当前测站PPP收敛图VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentObsSiteInfo(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }

            var path = Path.Combine(GetOrCreatePppOption().GetSolverDirectory(site.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
            var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
            GnsserFormUtil.ShowChartForm(path, paramNames);
        }

        private void 打开当前测站PPP结果RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentObsSiteInfo(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            var path = Path.Combine(GetOrCreatePppOption().GetSolverDirectory(site.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
            TryOpenTableForm(path);

        }
        private void 查看所有测站PPP收敛图AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            var names = new List<string>();
            foreach (var site in sites)
            {
                var path = Path.Combine(GetOrCreatePppOption().GetSolverDirectory(site.NetPeriod), resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                GnsserFormUtil.ShowChartForm(path, paramNames);
            }

        }
        private void RunPpp(List<ObsSiteInfo> sites)
        {
            if (sites.Count == 0) { log.Warn("没有文件！"); return; }
            MultiPeriodObsFileManager obsSiteInfos = this.Manager.SiteManager.ExtractPeriodSites(sites);

            Geo.Utils.FileUtil.CheckOrCreateDirectory(GetOrCreatePppOption().OutputDirectory);
            PppRinexFileApproxUpdater runner = new PppRinexFileApproxUpdater(GetOrCreatePppOption(), IsReplaceApproxCoordWhenPPP);

            runner.ProgressViewer = this.progressBarComponent1;

            runner.Update(obsSiteInfos);
        }
        #endregion

        private void 查看已移除基线BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Geo.Utils.FormUtil.ShowWarningMessageBox("尚未实现，请稍等。");
            var form = new ObjectSelectingForm<SiteObsBaseline>(this.Manager.BaseLineManager.RemovedLines);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var news = form.SelectedObjects;
                if(news.Count == 0) { return; }
                foreach (var item in news)
                {
                    this.Manager.BaseLineManager.Add(item);
                }
                 
                this.DataBind(true);
            }
        }

        private void 地图查看所选时段基线MToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var node = this.treeView_periods.SelectedNode;
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择时段节点后再试！"); return; }

            var lineObjs = Geo.Utils.TreeNodeUtil.GetCurrentSubObjects<SiteObsBaseline>(node);
            if (lineObjs == null || lineObjs.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择时段节点后再试！"); return; }
             
            ShowLinesOnMap(lineObjs);
        }

        private void 树形查看编辑基线测站时段EToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var node = this.treeView_periods.SelectedNode;
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择节点后再试！"); return; } 
            var lineObj = Geo.Utils.TreeNodeUtil.GetCurrentObject<SiteObsBaseline>(node, false);            
            if (lineObj != null)
            {
                if (lineObj == null) { return; }
                EditBaseLineObsFile(lineObj);
            }
        }
        #region 编辑文件
        private static void EditBaseLineObsFile(SiteObsBaseline lineObj)
        {
            lineObj.Start.CheckOrCopyToTempDirectory();
            EditObsFile(lineObj.Start);
            EditObsFile(lineObj.End); 
        }
        private static void EditObsFile(ObsSiteInfo site)
        {
            ObsFileChartEditForm form = new ObsFileChartEditForm(site.FilePath, true, false, false, true);
            form.Text = site.FileName + ", " + site.SiteName;
            form.Show();
        }
        #endregion

        private void 时段刷新背景RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeView_periods.Refresh();
        }

        private void 刷新背景RToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.treeView_lines.Refresh();
        }

        private void 打开时段目录OToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var node = this.treeView_periods.SelectedNode;
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择节点后再试！"); return; }
            var lineObj = Geo.Utils.TreeNodeUtil.GetCurrentObject<SiteObsBaseline>(node, false);
            Geo.Utils.FileUtil.OpenDirectory( Path.GetDirectoryName( this.Option.GetSolverDirectory(lineObj.NetPeriod)));
        }

        private void 打开基线结果目录RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.treeView_periods.SelectedNode;
            OpenDirectoryOfTree(node);
        }
        private void 打开基线结果目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.treeView_lines.SelectedNode;
            OpenDirectoryOfTree(node);
        }

        private void OpenDirectoryOfTree(TreeNode node)
        {
            if (node == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选择节点后再试！"); return; }
            var lineObj = Geo.Utils.TreeNodeUtil.GetCurrentObject<SiteObsBaseline>(node, false);
            Geo.Utils.FileUtil.OpenDirectory((this.Option.GetSolverDirectory(lineObj.NetPeriod)));
        }

        private void 打开基线结果目录OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var line = this.GetCurrentBaseLine(); if (null == (line)) { return; }
            Geo.Utils.FileUtil.OpenDirectory((this.Option.GetSolverDirectory(line.NetPeriod)));
        }

        private void MultiSolverOptionControl1_Load(object sender, EventArgs e)
        {

        }

        private void MultiSolverOptionControl1_OptionSetted(GnssProcessOption obj)
        {
            enumRadioControl_BaseLineSelectionType.SetCurrent(Option.BaseLineSelectionType);
            fileOpenControl_baselineFile.FilePath = Option.BaseLineFilePath;
        }

        private void MultiSolverOptionControl1_OptionSetting(GnssProcessOption obj)
        {
            Option.BaseLineSelectionType = enumRadioControl_BaseLineSelectionType.GetCurrent<BaseLineSelectionType>();
            Option.BaseLineFilePath = fileOpenControl_baselineFile.FilePath;
        }
    }
}
