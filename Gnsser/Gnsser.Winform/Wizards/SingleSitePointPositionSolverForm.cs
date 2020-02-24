//2019.01.18, czs, create in hmx, PPP计算新面板


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
    /// PPP计算新面板
    /// </summary>
    public partial class SingleSitePointPositionSolverForm : Form, IShowLayer
    {
        Log log = new Log(typeof(MultiPeriodBaseLineSolverForm));
        public event ShowLayerHandler ShowLayer;
        string ResultState = "ResultState";

        public SingleSitePointPositionSolverForm()
        {
            InitializeComponent(); 
            this.objectTableControl_currentResidual.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint; 
            this.objectTableControl_currentRepeatError.DataGridView.RowPrePaint += ClosureError_DataGridView_RowPrePaint;

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

        private void Init()
        {
            checkBox_enableNet.Checked = Setting.EnableNet;
            this.openFileDialog1_rinexOFile.Filter = Setting.RinexOFileFilter;
            this.enumRadioControl_obsType.Init<ObsPhaseType>();
            multiSolverOptionControl1.Init<SingleSiteGnssSolverType>();
            multiSolverOptionControl1.SetTitle("算法");

            Manager = new MultiPeriodSiteLineManager(MinCommonSpan);

            this.EntityToUi();
        }
         
        #region 属性
        /// <summary>
        ///核心管理器
        /// </summary>
        MultiPeriodSiteLineManager Manager { get; set; }
        /// <summary>
        /// 共同时段
        /// </summary>
        TimeSpan MinCommonSpan => TimeSpan.FromMinutes(this.namedFloatControl_periodSpanMinutes.GetValue());
        /// <summary>
        /// 工程目录
        /// </summary>
        public string ProjectDirectory { get => directorySelectionControl1_projDir.Path; set => directorySelectionControl1_projDir.Path = value; }
        
        /// <summary>
        /// 是否已经初始化完毕，可以联动显示
        /// </summary>
        private bool IsInited { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        GnssProcessOption Option => multiSolverOptionControl1.GetCurrentOption();
        /// <summary>
        /// 算法设置
        /// </summary>
        Dictionary<GnssSolverType, GnssProcessOption> Options => multiSolverOptionControl1.Options;
        /// <summary>
        /// PPP更新源文件坐标
        /// </summary>
        bool IsReplaceApproxCoordWhenPPP => checkBox_replaceApproxCoordWhenPPP.Checked;
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
        /// 时段树形目录下选择的测站信息
        /// </summary>
        /// <returns></returns>
        private ObsSiteInfo GetCurrentPeriodObsSiteInfo() { return Geo.Utils.TreeNodeUtil.GetCurrentObject<ObsSiteInfo>(this.treeView_periods.SelectedNode); } 
        /// <summary>
        /// 当前测站解算任务
        /// </summary>
        List<ObsSiteInfo> CurrentSitesToBeSolve { get; set; }
        private ObsSiteInfo GetCurrentListSite() { return this.bindingSource_allLines.Current as ObsSiteInfo; }
        private List<ObsSiteInfo> GetSelectedListSites()
        {
            var result = new List<ObsSiteInfo>();
            foreach (var item in this.listBox_vector.SelectedItems)
            {
                var obj = item as ObsSiteInfo;
                result.Add(obj);
            }
            return result;
        }
        /// <summary>
        /// 当前测站名称
        /// </summary>
        /// <returns></returns>
        private string GetCurrentSite() { return Geo.Utils.TreeNodeUtil.GetCurrentObject<string>(this.treeView_sites.SelectedNode); }

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
        private List<ObsSiteInfo> GetAllSites() { return this.Manager.SiteManager.GetAllSites(); }
   
        /// <summary>
        /// 项目名称
        /// </summary>
        string ProjectName => namedStringControl_projName.GetValue();
        #region 显示 
         
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
            Manager.SiteManager.MinEpochTimeSpan = MinCommonSpan;//更新
            var oks = this.Manager.Add(filePathes);

            if (oks.Count != filePathes.Length)
            {
                var failedCount = filePathes.Length - oks.Count;
                MessageBox.Show(failedCount + " 个文件没有添加成功，因为在相同时段已经包含了相同测站名称的文件或观测时段太短，请手动移除该站后再试。\r\n");
            }

            if (oks.Count > 0) //添加成功了
            {

                if (Geo.Utils.FormUtil.ShowYesNoMessageBox(
                        "是否根据当前设置，查找并匹配当前工程已经计算的结果？"
                        + "类型：" + this.GnssSolverType + " ") == DialogResult.Yes)
                {
                    CheckOrSetEstResult(oks);
                }


                BuildAndSetError();
                //绑定显示
                DataBind();
            }
        }
        //检查并设置工程目录和名称
        private void CheckOrSetProjectDirectoryAndName(string[] filePathes)
        {
            var dir = Path.GetDirectoryName(filePathes[0]);
            var projName = Path.GetFileName(dir);
            if (projName != ProjectName && ProjectDirectory != Path.Combine(dir, Setting.GnsserProjectDirectoryName))
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否将当前工程名称替换为 “" + projName
                   + "”，输出目录为 " + dir) == DialogResult.Yes)
                {
                    this.ProjectDirectory = Path.Combine(dir, Setting.GnsserProjectDirectoryName);
                    Geo.Utils.FileUtil.CheckOrCreateDirectory(ProjectDirectory);
                    this.namedStringControl_projName.SetValue(projName);
                }
            }
        }  /// <summary>
           /// 检查，并附加计算结果。
           /// </summary>
           /// <param name="newAddedLines"></param>
        private void CheckOrSetEstResult(List<ObsSiteInfo> newAddedLines)
        {
            //建立一个辅助时段基线管理器
            var siteObsBaselines = new MultiPeriodObsFileManager(newAddedLines, this.MinCommonSpan);

            //分时段处理
            foreach (var kv in siteObsBaselines.KeyValues)
            {
                var netPeriod = kv.Key;
                var mangager = kv.Value;
                 

                string path = "";
                var solverDir = this.GetCurrentSolverDirectory(netPeriod);//默认目录
                var pathes = Directory.GetFiles(solverDir, "*" + Setting.SiteCoordFileExtension);
                if (pathes.Length > 0)
                {
                    path = pathes[0];
                } 

                if ((File.Exists(path)  ))
                {
                    List<string> pathList = new List<string>();
                    if (File.Exists(path))
                    {
                        pathList.Add(path);
                    } 
                    var tables = ObjectTableManager.Read(pathList.ToArray(), ".");
                    //合并所有的表格
                    var rawTable = tables.Combine();
                    EstimatedSiteManager estimatedSites = EstimatedSiteManager.Parse(rawTable);

                    var sites = kv.Value;
                    foreach (var item in sites)
                    {
                        item.EstimatedSite = estimatedSites.Get(item.SiteName);
                    }  
                }
            }
        }

        #endregion

        #region 数据绑定显示
        /// <summary>
        /// 数据绑定显示
        /// </summary>
        private void DataBind()
        {
            IsInited = false;
            ////重新计算闭合差和显示基线
            //BindResultBuildAndSetClosureError();
            //等闭合差计算后再显示
            BindSitesToTree();

            BindPeriodTree();

            BindLineList();

            BindResultTable();

            IsInited = true;
        }

        private void BindResultTable()
        {
            ObjectTableStorage table = this.Manager.SiteManager.GetSiteCoordResultTable();
            this.objectTableControl_positionLineResult.DataBind(table);
        }
        #region 绑定细节
        private void BindLineList()
        {
            this.bindingSource_allLines.DataSource = Manager.SiteManager.GetAllSites();
            this.label_listAll.Text = "数量：" + Manager.SiteManager.SiteCount;
        }

        private void BindPeriodTree()
        {
            TreeNode topNode = new TreeNode("时段");
            var periods = this.Manager.SiteManager.TimePeriods;

            foreach (var period in periods)
            {
                string periodName = BuidPeriodName(period);
                var node = new TreeNode(periodName) { Tag = period };
                topNode.Nodes.Add(node);

                var items = this.Manager.SiteManager.Get(period);
               
                foreach (var item in items)
                {
                    var name = Geo.Utils.StringUtil.FillSpaceRight( item.SiteName.ToString(), 8) + " " + BuidPeriodName(item.TimePeriod);

                    var subNode = new TreeNode(name) { Tag = item };
                    node.Nodes.Add(subNode);
                }
            }
            treeView_periods.Nodes.Clear();
            treeView_periods.Nodes.Add(topNode);
            topNode.Expand();

            this.label_periods.Text = "数量：" + periods.Count;
        }
         

        private void BindSitesToTree()
        {
            TreeNode topNode = new TreeNode("测站");
            foreach (var siteName in Manager.SiteManager.SiteNames)
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
            label_siteInfo.Text = "时段：" + Manager.SiteManager.PeriodCount + ",测站：" + Manager.SiteManager.SiteCount + ",文件：" + Manager.SiteManager.FileCount;
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

            DataBind();
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
         
        private static void TryOpenTableForm(string path)
        {
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                new TableObjectViewForm(table).Show();
            }
        }
         

        private void 查看编辑所选测站时段EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = this.GetCurrentObsSiteInfo(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            site.CheckOrCopyToTempDirectory();
            ObsFileChartEditForm form = new ObsFileChartEditForm(site.FilePath, true, false, false, true);
            form.Show();
        }

        private void 查看编辑所有测站时段AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }
            foreach (var site in sites)
            {
                site.CheckOrCopyToTempDirectory();
                ObsFileChartEditForm form = new ObsFileChartEditForm(site.FilePath, true, false, false, true);
                form.Show();
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
         
        private void button_saveAllAsGNSSerFile_Click(object sender, EventArgs e)
        {
            ObjectTableStorage table = this.objectTableControl_positionLineResult.TableObjectStorage;
          //  var lines = this.Manager.SiteManager.GetAllSites();
           // if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
           if(table == null || table.IsEmpty)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return;
            }
            string name = BuildLineFileNameNoExtension(table.RowCount, "_All");
            var outpath = this.resultFileNameBuilder.BuildSiteCoordResulPath(name);


           // this.Manager.SiteManager.GetSiteCoordResultTable();
            ObjectTableWriter.Write(table, outpath);
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(ProjectDirectory);
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
                    BuildAndSetError();
                    DataBind();
                }
            }
            else
            {
                var site = GetCurrentObsSiteInfo(); if (site == null) { return; }

                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("将移除测站文件 " + site.FileName + " 及对应基线和计算的结果，移除后可以重新载入,\r\n 确定移除？") == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Manager.Remove(site);
                    BuildAndSetError();
                    DataBind();
                }
            }

        }
         
        #region 树形展开与收缩        
        private void 展开收缩时段EToolStripMenuItem_Click(object sender, EventArgs e) { Geo.Utils.TreeNodeUtil.TriggerExpandAll(treeView_periods.TopNode); }
        private void 展开收缩测站EToolStripMenuItem_Click(object sender, EventArgs e) { Geo.Utils.TreeNodeUtil.TriggerExpandAll(treeView_sites.TopNode); }
 
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
                var item = currentItem as ObsSiteInfo;
                // 判断是什么类型的标签
                ResultBackGroundStyle backGroundStyle = new ResultBackGroundStyle( Gnsser.GnssGradeType.Unknown);
                e.Graphics.FillRectangle(backGroundStyle.BackGroundBrush, listBox_vector.GetItemRectangle(e.Index));
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(backGroundStyle.BackGroundBrushSelected, listBox_vector.GetItemRectangle(e.Index));
                }
                // 焦点框
                e.DrawFocusRectangle();
                //文本  
                var name = Geo.Utils.StringUtil.FillSpaceRight(item.SiteName, 14) + " " + item.TimePeriod.ToDefualtPathString();
                e.Graphics.DrawString(name, e.Font, new SolidBrush(listBox_vector.ForeColor), e.Bounds, StringFormat.GenericDefault);
            }

        }
        private void treeView_periods_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowCurrentTreeNodeAttribute(treeView_periods);
            var currentLine = Geo.Utils.TreeNodeUtil.GetCurrentObject<ObsSiteInfo>(e.Node);
            DataBindCurrentPppSite(currentLine);
        } 

        private void listBox_vector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsInited) { return; }
            if (listBox_vector.SelectedItem == null) { return; }
            ShowObjectAttributeInfo(listBox_vector.SelectedItem);

            if (currentVectorIndex == listBox_vector.SelectedIndex) { return; }

            currentVectorIndex = listBox_vector.SelectedIndex;
             
            var current = this.GetCurrentListSite();

            DataBindCurrentPppSite(current);
        }


        /// <summary>
        /// 绑定数据表，并计算闭合差
        /// </summary>
        private void BuildAndSetError()
        {
            this.Invoke(new Action(() =>//采用Invoke将出错
            {
                if (this.Manager.BaseLineManager.Count == 0) { return; }

                //同步环闭合差
                var syncTriguilars = this.Manager.BaseLineManager.GetOrCreatePeriodSyncTriguilarNetQualitiyManager(this.Option.GnssReveiverNominalAccuracy, true);
                var syncCloureTable = syncTriguilars.GetObjectTable(); 

                //计算闭合差，根据闭合差设置颜色   
                this.listBox_vector.Refresh();

                //基线结果显示  同步环闭合差更新后才绑定
                var baseLineTable = this.Manager.BaseLineManager.GetBaselineTable();
                this.objectTableControl_positionLineResult.DataBind(baseLineTable);

                //复测基线较差
                var RepeatErrorOfBaseLineManager = this.Manager.BaseLineManager.GetOrCreateRepeatErrorOfBaseLineManager(this.Option.GnssReveiverNominalAccuracy, true);
                var repeatErrorTable = RepeatErrorOfBaseLineManager.GetObjectTable(); 
            }));
        }
        #region 显示当前 
         
        private void DataBindCurrentPppSite(ObsSiteInfo currentSite)
        {
            if (currentSite == null) { return; }

            //当前 残差图
            var path = "";
            var solverDiretory =  GetCurrentSolverDirectory(currentSite.NetPeriod);//this.GetOrCreatePppOption().OutputDirectory;//

            path = Path.Combine(solverDiretory, resultFileNameBuilder.BuildEpochResidualFileName(currentSite.SiteObsInfo.FileInfo.FileName));
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));

                var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path, 2), System.IO.Path.GetFileName(path));
                var phaseObsTable = table.GetTable(name, titles);
                this.commonChartControl_currentResidual.DataBind(phaseObsTable);
                //绘制全图
                this.objectTableControl_currentResidual.DataBind(table);
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
        /// <param name="site"></param>
        /// <returns></returns>
        public string GetCurrentSolverDirectory(ObsSiteInfo site)
        {
            this.Option.OutputDirectory = this.ProjectDirectory;
            var gnssSolverType = this.Option.GnssSolverType;
            return Option.GetSolverDirectory(site.NetPeriod, gnssSolverType);
        }

        private void ShowCurrentTreeNodeAttribute(TreeView treeView_periods)
        {
            if (!IsInited) { return; }
            if (treeView_periods.SelectedNode == null) { return; }
            ShowObjectAttributeInfo(treeView_periods.SelectedNode.Tag);
        }
        #endregion

        private void MultiPeriodBaseLineSolverForm_Load(object sender, EventArgs e)
        {
            checkBox_enableNet.Checked = Setting.EnableNet;
            namedStringControl_projName.SetValue("Net");
        }

        #region 基线解算

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        #endregion
        private void checkBox_enableNet_CheckedChanged(object sender, EventArgs e) { Setting.EnableNet = checkBox_enableNet.Checked; }

        private void 移除所选时段RToolStripMenuItem_Click(object sender, EventArgs e)
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
                    this.BuildAndSetError();
                    this.DataBind();
                }
            }
            if (lineObj != null)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否删除基线 " + lineObj + " " + lineObj.TimePeriod + " ？") == DialogResult.Yes)
                {
                    this.Manager.Remove(lineObj);
                    this.BuildAndSetError();
                    this.DataBind();
                }
            }
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
        GnssProcessOption GetOrCreatePppOption()
        { 
            var pppOption = this.multiSolverOptionControl1.GetCurrentOption();
            pppOption.OutputDirectory = this.ProjectDirectory;// Path.Combine(this.ProjectDirectory, pppOption.GnssSolverType.ToString());
     
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

            this.Invoke(new Action(() => this.DataBind()));
           // this.Invoke(new Action(delegate () { this.DataBind(); })); 
        }
        #endregion
         

        private void button_solveCurrentLine_Click(object sender, EventArgs e)
        {
            var sites = this.GetSelectedListSites();

            if (sites == null || sites.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有选择测站！"); return; }

            //在主线程中执行报错
            var t1 = new System.Threading.Tasks.Task(() => RunPpp(sites));
            t1.Start();
        }

        private void button_runAllBaseLine_Click(object sender, EventArgs e)
        {
            var sites = this.Manager.SiteManager.GetAllSites();
            if (sites == null || sites.Count ==0) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！"); return; }

            //在主线程中执行报错
            var t1 = new System.Threading.Tasks.Task(() => RunPpp(sites));
            t1.Start();

        }
    }

}
