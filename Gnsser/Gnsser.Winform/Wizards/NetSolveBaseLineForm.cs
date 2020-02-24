//2018.01.03, czs, edit in hmx, 网解基线


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
    public partial class NetSolveBaseLineForm : Form, IShowLayer
    {
        Log log = new Log(typeof(NetSolveBaseLineForm));
        public event ShowLayerHandler ShowLayer;
        public NetSolveBaseLineForm()
        {
            InitializeComponent();
            this.treeView_netLine.ImageList = ImageManager.Instance.DirectoryImageListSmall;

            this.GetOrCreateOption();
            this.Init();
        }
        public NetSolveBaseLineForm(string projectDirectory)
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
            this.enumRadioControl_obsType.Init<ObsPhaseType>();
            this.EntityToUi();
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
        public string SolverDirectory => Path.Combine(ProjectDirectory, GnssSolverType.网解双差定位.ToString());
        /// <summary>
        /// 工程目录
        /// </summary>
        public string ProjectDirectory { get => directorySelectionControl1_projDir.Path; set => directorySelectionControl1_projDir.Path = value; }
        /// <summary>
        /// 是否已经初始化完毕，可以联动显示
        /// </summary>
        private bool IsInited { get; set; }
        /// <summary>
        /// 中心站网解
        /// </summary>
        CenterSiteBaseLineMangager CenterSiteBaseLineMangager { get; set; }
        /// <summary>
        /// 并行计算选项
        /// </summary>
        public IParallelConfig ParallelConfig { get { return parallelConfigControl1; } }
        #endregion

        #region  基本操作
        public void UiToEntity()
        {
            this.Option.OutputDirectory = ProjectDirectory;
            this.Option.ObsPhaseType = this.enumRadioControl_obsType.GetCurrent<ObsPhaseType>();
        }


        public void EntityToUi()
        {
            //工程目录以界面为准
            //ProjectDirectory = this.Option.OutputDirectory;

            this.enumRadioControl_obsType.SetCurrent<ObsPhaseType>(this.Option.ObsPhaseType);
        }
        private void BaseLineNetSolverForm_Load(object sender, EventArgs e)
        {
            this.namedStringControl_projName.SetValue("Net");
        }

        private void listBox_site_DoubleClick(object sender, EventArgs e)
        {
            this.打开当前文件FToolStripMenuItem_Click(sender, e);
        }
        private void checkBox_enableNet_CheckedChanged(object sender, EventArgs e) { Setting.EnableNet = checkBox_enableNet.Checked; }
        
        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetSelectedSites(); if (sites == null || sites.Count == 0) { return; }

            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("将移除测站 " + Geo.Utils.StringUtil.ToString(sites) + " 及对应基线和计算的结果，移除后可以重新载入,\r\n 确定移除？") == System.Windows.Forms.DialogResult.Yes)
            {
                //移除测站
                foreach (var site in sites)
                {
                    this.bindingSource_site.Remove(site);
                    this.ObsFileManager.Remove(site.SiteName);
                    CenterSiteBaseLineMangager.Remove(site.SiteName);

                    //移除基线 
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
                this.CenterSiteBaseLineMangager.Clear();
                this.BaselineManager.Clear();
                this.bindingSource_site.Clear();
                this.objectTableControl_baselineResult.Clear();
                this.objectTableControl_closureErrors.Clear();
                this.objectTableControl_independentLine.Clear();
                this.objectTableControl_currentLineErrors.Clear();
            }
        }

        private void 导入文件IToolStripMenuItem_Click(object sender, EventArgs e) { if (openFileDialog1_rinexOFile.ShowDialog() == DialogResult.OK) { InpportFiles(openFileDialog1_rinexOFile.FileNames); } }

        private void InpportFiles(string[] filePathes)
        {
            if (filePathes.Length == 0) { return; }
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
                //网解
                CenterSiteBaseLineMangager = ObsFileManager.GenerateCenterSiteBaseLineMangager();
                var lines = ObsFileManager.GenerateAllCenteredObsBaseLines(CenterSiteBaseLineMangager);

                GenerateBaseLineAndAddToCurrentLineManager(lines);
            }

            BindTreeView();
        }

        private void BindTreeView()
        {
            var treeView = this.treeView_netLine;
            treeView.Nodes.Clear();

            if (CenterSiteBaseLineMangager == null) { return; }

            foreach (var kv in CenterSiteBaseLineMangager.KeyValues)
            {
                var site = kv.Key;
                var siteNode = new TreeNode(site)
                {
                    ImageKey = ImageManager.SITE_POINT,// "site",
                    SelectedImageKey = ImageManager.SITE_POINT_SELECTED,//"selectedSite",
                    Tag = this.ObsFileManager.Get(site)
                };
                siteNode.ContextMenuStrip = this.contextMenuStrip_netSite;

                foreach (var line in kv.Value)
                {
                    var lineNode = new TreeNode(line.Name)
                    {
                        ImageKey = ImageManager.NET_POINT,// "line",
                        SelectedImageKey = ImageManager.NET_POINT_SELECTED,//"selectedLine",
                        Tag = this.BaselineManager.GetOrReversed(line)
                    };
                    siteNode.Nodes.Add(lineNode);
                }
                treeView.Nodes.Add(siteNode);
            }
        }

        private void GenerateBaseLineAndAddToCurrentLineManager(List<SiteObsBaseline> lines)
        {
            var path = resultFileNameBuilder.GetBaseLineResultFile();
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


        private void bindingSource_site_CurrentChanged(object sender, EventArgs e)
        {
            if (!IsInited) { return; }
            var site = GetCurrentSite(); if (site == null) { return; }
            ShowObjectAttributeInfo(site);
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

        /// <summary>
        /// 数据绑定显示
        /// </summary>
        private void DataBind()
        {
            IsInited = false;
            this.bindingSource_site.DataSource = ObsFileManager.Values;

            this.label_siteInfo.Text = "测站数量：" + ObsFileManager.Count;
            //重新计算闭合差和显示基线
            BindResultBuildAndSetClosureError();
            //等闭合差计算后再显示

            BindTreeView();

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



        private void Runner_Completed(object sender, EventArgs e)
        {
            BindResultBuildAndSetClosureError();
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
                ObjectTableStorage closureErrorsTable = BaseLineNet.BuildSynchNetTrilateralCheckTResultable("Net", CurrentQualityManager);
                objectTableControl_closureErrors.DataBind(closureErrorsTable);

                //计算闭合差，根据闭合差设置颜色 
                var lines = this.GetAllBaseLines();
                foreach (var line in lines)
                {
                    var quality = CurrentQualityManager.GetBest(line.LineName);
                    if (quality == null || line.EstimatedResult == null) { continue; }

                    line.ResultState = quality.ResultState;
                    line.EstimatedResult.ClosureError = quality.ClosureError.Value.Length;
                }
                //闭合差更新后的值，因此重新提取和绑定
                baseLineTable = BaselineManager.GetBaselineTable();
                this.objectTableControl_baselineResult.DataBind(baseLineTable);

                this.treeView_netLine.Refresh();
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
            var entity = result as IWithEstimatedBaseline;
            if (entity != null)
            {
                var line = entity.GetEstimatedBaseline();
                var lineObj = BaselineManager.Get(line.BaseLineName);

                lineObj.EstimatedResult = line;
            }
            //log.Warn("712 行，未处理网解基线结果绑定。。");
            var netLine = result as IWithEstimatedBaselines;
            if (netLine != null)
            {
                var lines = netLine.GetEstimatedBaselines();
                foreach (var line in lines)
                {
                    var lineObj = BaselineManager.Get(line.BaseLineName);
                    lineObj.EstimatedResult = line;
                }
            }

            log.Info(result.Name + ", " + result.ReceiverTime + "， 即将输出结果文件...");
            var writer = new GnssResultWriter(Option,  this.Option.IsOutputEpochResult,
                Option.IsOutputEpochSatInfo);
            writer.WriteFinal((BaseGnssResult)result);
        }

        #region 数据获取

        public GnssProcessOption GetOrCreateOption()
        {
            if (this.Option == null) { this.Option = GnssProcessOption.GetDefaultSimpleEpochDoubleDifferPositioningOption(); }
            return Option;
        }
        private SiteObsBaseline GetCurrentBaseLine() { return null; }

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
            List<SiteObsBaseline> lst = this.BaselineManager.Values;// 
            return lst;
        }

        #endregion

        #region 结果绘线条图         

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
            if (lines == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请计算后再试！"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(new List<EstimatedBaseline>() { (EstimatedBaseline)lines.EstimatedResult });

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
                if (line == null) { continue; }

                LineString lineString = BuildLineString(line);
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
            var lineString = new LineString(new List<AnyInfo.Geometries.Point>()  {   ptA, ptB  }, baseLine.Name);
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
            if (sites == null || sites.Count == 0) { return; }
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
            var names = new List<string>();
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
            var site = this.ObsFileManager.Get(entity.SiteInfo.SiteName);

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
                    var original = Path.Combine(site.Directory, Path.GetFileName(result));
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
            if (this.backgroundWorker_netSolve.IsBusy && FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No) { e.Cancel = true; }
            //    else { IsClosed = true; }
        }

        private void listBox_site_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView_netLine_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //属性绑定显示 
            if (e.Node != null)
            {
                this.attributeBox1.SetObject(e.Node.Tag, false);
            }


            //绘图显示//基线文本显示
            if ((e.Node.Tag is ObsSiteInfo))
            {
                var baseSite = treeView_netLine.SelectedNode.Tag as ObsSiteInfo;

                //文本显示
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("基准站：" + baseSite.ToString());
                sb.AppendLine();

                var lines = Geo.Utils.TreeNodeUtil.GetCurrentAndSubObjects<SiteObsBaseline>(e.Node);
                foreach (var line in lines)
                {
                    if (line.EstimatedResult != null)
                    {
                        sb.AppendLine(line.EstimatedResult.ToString());
                    }
                }
                this.richTextBoxControl_baselineInfo.Text = sb.ToString();

                //显示残差
                var baseSiteName = baseSite.SiteName;
                ObjectTableStorage table = TryLoadNetSolvePhaseResidualTable(baseSiteName, true);
                if (table != null)
                {
                    this.commonChartControl_currentResidual.DataBind(table);
                    commonChartControl_currentResidual.ShowInfo(table.Name);
                }

                //当前基线参数收敛图 
                table = TryLoadNetSolveConverageTable(baseSiteName, true);
                if (table != null)
                {
                    this.commonChartControl_convergenceA.DataBind(table);
                    commonChartControl_convergenceA.ShowInfo(table.Name);
                }
            }

            if (e.Node.Tag is SiteObsBaseline)
            {
                var currentLine = treeView_netLine.SelectedNode.Tag as SiteObsBaseline;
                var lineName = currentLine.LineName;
                if (CurrentQualityManager != null)
                {
                    //当前基线闭合差
                    var quality = CurrentQualityManager.Get(lineName);
                    if (quality == null) { return; }

                    ObjectTableStorage lineTable = QualityOfTriAngleClosureError.BuildSynchNetTrilateralCheckTResultable(lineName.ToString(), quality);
                    this.objectTableControl_currentLineErrors.DataBind(lineTable);

                    var line = this.BaselineManager.Get(lineName);
                    var reverseLine = this.BaselineManager.Get(lineName.ReverseBaseLine);
                    ObjectTableStorage reverseErrorTable = QualityOfTriAngleClosureError.BuildReverseErrorTable(line, reverseLine);
                    if (reverseErrorTable != null)
                    {
                        objectTableControl_reverseError.DataBind(reverseErrorTable);
                    }
                }
            }
        }
       
        #region  表格加载算法工具细节
        /// <summary>
        /// 尝试加载网解双差的收敛图
        /// </summary>
        /// <param name="baseSiteName"></param>
        /// <returns></returns>
        private ObjectTableStorage TryLoadNetSolveConverageTable(string baseSiteName, bool isFilter)
        {
            ObjectTableStorage table = null;
            var path2 = resultFileNameBuilder.GetNetDoubleDifferBaseLineEpochParamFile(baseSiteName);

            if (File.Exists(path2))
            {
                var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path2), System.IO.Path.GetFileName(path2));
                table = ObjectTableReader.Read(path2);
                //直接返回
                if (!isFilter) { return table; }

                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                var nameBuilder = new NetDoubleDifferPositionParamNameBuilder(this.GetOrCreateOption());
                foreach (var siteName in this.ObsFileManager.Keys)
                {
                    var names = nameBuilder.GetSiteDxyz(siteName);
                    paramNames.AddRange(names);
                }

                table = table.GetTable(table.Name, paramNames);
                table.RemoveRows(0, 20);//默认移除前十
            }

            return table;
        }
        /// <summary>
        /// 尝试加载网解载波残差图
        /// </summary>
        /// <param name="baseSiteName"></param>
        /// <returns></returns>
        private  ObjectTableStorage TryLoadNetSolvePhaseResidualTable(string baseSiteName, bool isFilter)
        {
            ObjectTableStorage phaseObsTable = null;
            var path = this.resultFileNameBuilder.GetNetDoubleDifferBaseLineEpochResidualFile(baseSiteName); 
            if (File.Exists(path))
            {
                var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path), System.IO.Path.GetFileName(path));

                var  table = ObjectTableReader.Read(path);    
                //直接返回
                if (!isFilter) { return table; }

                var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));
                phaseObsTable = table.GetTable(table.Name, titles);

                phaseObsTable.Name = name;
            }
            return phaseObsTable;
        }
        #endregion

        private void button_solveAllNet_Click(object sender, EventArgs e)
        {
            if (backgroundWorker_netSolve.IsBusy)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("我正在忙，请稍等。。。");
                return;
            }
            CurrentCenterSiteToSolve = this.ObsFileManager.Keys;// new List<string>();
            backgroundWorker_netSolve.RunWorkerAsync();
        }
        private void button_solveCurrentNet_Click(object sender, EventArgs e)
        {
            if (backgroundWorker_netSolve.IsBusy)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("我正在忙，请稍等。。。");
                return;
            }
            if(treeView_netLine.SelectedNode == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("选中后再试！");
                return;
            }
            if (treeView_netLine.SelectedNode.Tag != null)
            {
                this.attributeBox1.SetObject(treeView_netLine.SelectedNode.Tag);
            }
            var site = Geo.Utils.TreeNodeUtil.GetCurrentObject<ObsSiteInfo>(treeView_netLine.SelectedNode);
            CurrentCenterSiteToSolve = new List<string> { site.SiteName };
            //NetSolveBaseLine(site.SiteName);

            backgroundWorker_netSolve.RunWorkerAsync();
        }

        private void backgroundWorker_netSolve_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                button_solveCurrentNet.Enabled = false;
                button_solveAllNet.Enabled = false;
            }));

            NetSolveBaseLine();
        }
        #region 网解解算过程
        List<string> CurrentCenterSiteToSolve { get; set; }

        private void NetSolveBaseLine()
        {
            if (false)//并行用一个Option是错误的
            {
                Parallel.ForEach<string>(CurrentCenterSiteToSolve, new Action<string>(m =>
                {
                    NetSolveBaseLine(m);
                }));
            }
            foreach (var m in CurrentCenterSiteToSolve)
            {
                NetSolveBaseLine(m);
            }
        }

        /// <summary>
        /// 解算基线
        /// </summary>
        /// <param name="baseSiteName"></param>
        private void NetSolveBaseLine(string baseSiteName)
        {
            log.Info("即将解算指定的 " + baseSiteName + " 网解。");

            this.GetOrCreateOption();
            this.UiToEntity();

            Option.GnssSolverType = GnssSolverType.网解双差定位;
            Option.IndicatedBaseSiteName = baseSiteName;
            Option.BaseSiteSelectType = BaseSiteSelectType.Indicated;
            Option.IsBaseSiteRequried = true;
            //不同算法输出不同的目录
            Option.OutputDirectory = Path.Combine(Option.OutputDirectory, Option.GnssSolverType.ToString());

            var pathes = this.ObsFileManager.GetSitePathes();

            var runner = new MultiSiteBackGroundRunner(this.Option, pathes.ToArray());
            runner.ParallelConfig = ParallelConfig;
            runner.Completed += Runner_Completed;
            runner.Processed += OneSolver_Processed;
            runner.ProgressViewer = progressBarComponent1;
            runner.Init();
            runner.Run();
        }

        private void backgroundWorker_netSolve_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_solveCurrentNet.Enabled = true;
            button_solveAllNet.Enabled = true;
        }
        #endregion

        private void button_netOption_Click(object sender, EventArgs e)
        {
            if (this.Option == null) { this.Option = GnssProcessOption.GetDefaultNetDoublePostionDifferOption(); }

            UiToEntity();
            Option.GnssSolverType = GnssSolverType.网解双差定位;
            var optionForm = new OptionVizardForm(Option);
            optionForm.Init();
            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Option = optionForm.Option;
                this.EntityToUi();
            }
        }

        private void 查看所有残差图RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //显示残差
            var pathes = this.resultFileNameBuilder.GetAllNetDoubleDifferBaseLineEpochResidualFile();

            int index = 0;
            foreach (var path in pathes)
            {
                if (File.Exists(path))
                {
                    index++;
                    var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path), System.IO.Path.GetFileName(path));

                    var table = ObjectTableReader.Read(path);
                    var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));
                    var phaseObsTable = table.GetTable(table.Name, titles);

                    var form = new CommonChartForm(phaseObsTable) { Text = name };
                    form.Show();
                }
            }
        }

        private void 查看所有收敛图CToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //当前基线参数收敛图
            int index = 0;
            var pathes = resultFileNameBuilder.GetAllNetDoubleDifferBaseLineEpochParamFile();
            foreach (var path in pathes)
            {
                if (File.Exists(path))
                {
                    index++;
                    var name = System.IO.Path.Combine(Geo.Utils.PathUtil.GetSubDirectory(path), System.IO.Path.GetFileName(path));
                    var table = ObjectTableReader.Read(path);
                    var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                    var nameBuilder = new NetDoubleDifferPositionParamNameBuilder(this.GetOrCreateOption());
                    foreach (var siteName in this.ObsFileManager.Keys)
                    {
                        var names = nameBuilder.GetSiteDxyz(siteName);
                        paramNames.AddRange(names);
                    }


                    var phaseObsTable = table.GetTable(table.Name, paramNames);
                    var form = new CommonChartForm(phaseObsTable) { Text = name };
                    form.Show();
                }
            }
        }

        private void treeView_netLine_DrawNode(object sender, DrawTreeNodeEventArgs e)
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

            if (e.Node.Tag is ObsSiteInfo)
            {
                e.DrawDefault = true; //这里用默认颜色，只需要在TreeView失去焦点时选中节点仍然突显  
                return;
            }
            //不可见时，绘制将出现在顶端
            if (!e.Node.IsVisible)
            {
                e.DrawDefault = true;
                return;
            }

            //     log.Error(e.Node.Text + " - " + e.State);
            if ((e.Node.Tag is SiteObsBaseline))
            {
                var item = e.Node.Tag as SiteObsBaseline;

                // 判断是什么类型的标签
                ResultBackGroundStyle backGroundStyle = new ResultBackGroundStyle(item.ResultState); 

                e.Graphics.FillRectangle(backGroundStyle.BackGroundBrush, e.Bounds);
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;
                //文本  
                //e.Graphics.DrawString(e.Node.Text, nodeFont, new SolidBrush(treeView_netLine.ForeColor), e.Bounds, StringFormat.GenericDefault);
                e.Graphics.DrawString(e.Node.Text, nodeFont, new SolidBrush(treeView_netLine.ForeColor), Rectangle.Inflate(e.Bounds, 2, 0));
            }
        }

        private void 全部展开EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView_netLine.ExpandAll();
        }

        private void 全部收起CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView_netLine.CollapseAll();
        }

        private void 展开EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeView_netLine.SelectedNode.Expand();
        }

        private void 收起CToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            this.treeView_netLine.SelectedNode.Collapse();
        }

        public ObsSiteInfo GetCurrentNodeSite()
        {
            return Geo.Utils.TreeNodeUtil.GetCurrentObject<ObsSiteInfo>(this.treeView_netLine.SelectedNode);
        }

        private void 查看残差图SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //显示残差
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolvePhaseResidualTable(baseSiteName, true);
            if (table != null)
            {
                CommonChartForm form = new CommonChartForm(table);
                form.Show();
            }
            else   {    log.Warn("没有找到残差文件！");    }
        }

        private void 查看收敛图LToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolveConverageTable(baseSiteName, true);
            if (table != null)
            {
                CommonChartForm form = new CommonChartForm(table);
                form.Show();
            }
            else { log.Warn("没有找到参数文件！"); }
        }

        private void 查看参数文件PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolveConverageTable(baseSiteName,false);
            if (table != null)
            {
                var form = new TableObjectViewForm(table);
                form.Show();
            }
            else { log.Warn("没有找到参数文件！"); }
        }

        private void 打开残差文件OToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolvePhaseResidualTable(baseSiteName, false);
            if (table != null)
            {
                var form = new TableObjectViewForm(table);
                form.Show();
            }
            else { log.Warn("没有找到残差文件！"); }
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    