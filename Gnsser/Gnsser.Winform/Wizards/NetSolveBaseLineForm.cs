//2018.01.03, czs, edit in hmx, �������


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
        #region ����
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        string ProjectName => namedStringControl_projName.GetValue();
        /// <summary>
        /// �ļ�����������
        /// </summary>
        ResultFileNameBuilder resultFileNameBuilder => new ResultFileNameBuilder(SolverDirectory);
        /// <summary>
        /// ��վ������
        /// </summary>
        ObsSiteFileManager ObsFileManager { get; set; }
        /// <summary>
        /// ���߹�����
        /// </summary>
        SiteObsBaselineManager BaselineManager { get; set; }
        /// <summary>
        /// ѡ��
        /// </summary>
        GnssProcessOption Option { get; set; }
        /// <summary>
        /// ��ǰ����Ŀ¼
        /// </summary>
        public string SolverDirectory => Path.Combine(ProjectDirectory, GnssSolverType.����˫�λ.ToString());
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public string ProjectDirectory { get => directorySelectionControl1_projDir.Path; set => directorySelectionControl1_projDir.Path = value; }
        /// <summary>
        /// �Ƿ��Ѿ���ʼ����ϣ�����������ʾ
        /// </summary>
        private bool IsInited { get; set; }
        /// <summary>
        /// ����վ����
        /// </summary>
        CenterSiteBaseLineMangager CenterSiteBaseLineMangager { get; set; }
        /// <summary>
        /// ���м���ѡ��
        /// </summary>
        public IParallelConfig ParallelConfig { get { return parallelConfigControl1; } }
        #endregion

        #region  ��������
        public void UiToEntity()
        {
            this.Option.OutputDirectory = ProjectDirectory;
            this.Option.ObsPhaseType = this.enumRadioControl_obsType.GetCurrent<ObsPhaseType>();
        }


        public void EntityToUi()
        {
            //����Ŀ¼�Խ���Ϊ׼
            //ProjectDirectory = this.Option.OutputDirectory;

            this.enumRadioControl_obsType.SetCurrent<ObsPhaseType>(this.Option.ObsPhaseType);
        }
        private void BaseLineNetSolverForm_Load(object sender, EventArgs e)
        {
            this.namedStringControl_projName.SetValue("Net");
        }

        private void listBox_site_DoubleClick(object sender, EventArgs e)
        {
            this.�򿪵�ǰ�ļ�FToolStripMenuItem_Click(sender, e);
        }
        private void checkBox_enableNet_CheckedChanged(object sender, EventArgs e) { Setting.EnableNet = checkBox_enableNet.Checked; }
        
        private void �Ƴ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetSelectedSites(); if (sites == null || sites.Count == 0) { return; }

            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("���Ƴ���վ " + Geo.Utils.StringUtil.ToString(sites) + " ����Ӧ���ߺͼ���Ľ�����Ƴ��������������,\r\n ȷ���Ƴ���") == System.Windows.Forms.DialogResult.Yes)
            {
                //�Ƴ���վ
                foreach (var site in sites)
                {
                    this.bindingSource_site.Remove(site);
                    this.ObsFileManager.Remove(site.SiteName);
                    CenterSiteBaseLineMangager.Remove(site.SiteName);

                    //�Ƴ����� 
                    BaselineManager.Remove(site.SiteName, false);
                }

                DataBind();
            }
        }

        private void ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("��������в�վ�����ߣ���ȷ�����߽���Ѿ�����,\r\n ȷ���Ƴ���") == System.Windows.Forms.DialogResult.Yes)
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

        private void �����ļ�IToolStripMenuItem_Click(object sender, EventArgs e) { if (openFileDialog1_rinexOFile.ShowDialog() == DialogResult.OK) { InpportFiles(openFileDialog1_rinexOFile.FileNames); } }

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
                MessageBox.Show("�����ļ�û����ӳɹ�����Ϊ�Ѿ���������ͬ��վ���Ƶ��ļ������ֶ��Ƴ���վ�����ԡ�\r\n" + msg);
            }

            if (added)
            {
                //����
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
            if (File.Exists(path) && Geo.Utils.FormUtil.ShowYesNoMessageBox("�Ƿ�����ƥ�䵱ǰ���ֵļ�������\r\n " + path) == DialogResult.Yes)
            {
                var tables = ObjectTableManager.Read(new string[] { path }, ".");
                //�ϲ����еı��
                var rawTable = tables.Combine();
                var BaseLineNets = BaseLineNetManager.Parse(rawTable, 20);//ʱ����

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
            //��ӻ��ߣ�����ظ������
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


        private void ������Ŀ¼OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { return; }
            Geo.Utils.FileUtil.OpenDirectory(site.Directory);
        }

        private void �򿪵�ǰ�ļ�FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { return; }
            Geo.Utils.FileUtil.OpenFile(site.FilePath);
        }

        /// <summary>
        /// ���ݰ���ʾ
        /// </summary>
        private void DataBind()
        {
            IsInited = false;
            this.bindingSource_site.DataSource = ObsFileManager.Values;

            this.label_siteInfo.Text = "��վ������" + ObsFileManager.Count;
            //���¼���պϲ����ʾ����
            BindResultBuildAndSetClosureError();
            //�ȱպϲ���������ʾ

            BindTreeView();

            IsInited = true;
        }
        #endregion

        #region ��ʾ 
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
        /// �����ݱ�������պϲ�
        /// </summary>
        private void BindResultBuildAndSetClosureError()
        {
            this.Invoke(new Action(() =>//����Invoke������
            {
                //���߽����ʾ
                var baseLineTable = BaselineManager.GetBaselineTable();
                if (baseLineTable.ColCount == 0 || baseLineTable.RowCount == 0) { return; }

                baseLineTable = BaselineManager.GetBaselineTable();//����ȷ��������ʾһ��
                this.objectTableControl_baselineResult.DataBind(baseLineTable);

                //����Ϊ����
                var BaseLineNets = BaseLineNetManager.Parse(baseLineTable, 200);//ʱ����
                var bigNet = BaseLineNets.FirstKeyValue.Value;

                //�������п��ܵ������Σ�Ȼ����ȡ���磬����պϲ� 
                CurrentQualityManager = bigNet.BuildTriangularClosureQualies(this.GetOrCreateOption().GnssReveiverNominalAccuracy);
                if (CurrentQualityManager.Count == 0) { return; }

                //ͬ�����պϲ������� ����ʾ      
                ObjectTableStorage closureErrorsTable = BaseLineNet.BuildSynchNetTrilateralCheckTResultable("Net", CurrentQualityManager);
                objectTableControl_closureErrors.DataBind(closureErrorsTable);

                //����պϲ���ݱպϲ�������ɫ 
                var lines = this.GetAllBaseLines();
                foreach (var line in lines)
                {
                    var quality = CurrentQualityManager.GetBest(line.LineName);
                    if (quality == null || line.EstimatedResult == null) { continue; }

                    line.ResultState = quality.ResultState;
                    line.EstimatedResult.ClosureError = quality.ClosureError.Value.Length;
                }
                //�պϲ���º��ֵ�����������ȡ�Ͱ�
                baseLineTable = BaselineManager.GetBaselineTable();
                this.objectTableControl_baselineResult.DataBind(baseLineTable);

                this.treeView_netLine.Refresh();
            }));
        }
        /// <summary>
        /// ��ǰ�����αպϲ�����
        /// </summary>
        TriguilarNetQualitiyManager CurrentQualityManager { set; get; }
        /// <summary>
        /// �������һ����վ
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
            //log.Warn("712 �У�δ����������߽���󶨡���");
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

            log.Info(result.Name + ", " + result.ReceiverTime + "�� �����������ļ�...");
            var writer = new GnssResultWriter(Option,  this.Option.IsOutputEpochResult,
                Option.IsOutputEpochSatInfo);
            writer.WriteFinal((BaseGnssResult)result);
        }

        #region ���ݻ�ȡ

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

        #region ���������ͼ         

        private static void TryOpenTableForm(string path)
        {
            if (File.Exists(path))
            {
                var table = ObjectTableReader.Read(path);
                new TableObjectViewForm(table).Show();
            }
        }
        private void �鿴�༭��ѡ��վʱ��EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            site.CheckOrCopyToTempDirectory();
            ObsFileChartEditForm form = new ObsFileChartEditForm(site.FilePath, true, false, false, true);
            form.Show();
        }
        private void �ָ�ԭʼ�ļ�RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("�Ƿ�Ҫ�ָ�" + site.SiteName + "���޸ģ� ") == DialogResult.Yes)
            {
                site.CopyToTempDirectory();
            }
        }
        private void �鿴�༭���в�վʱ��AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            foreach (var site in sites)
            {
                site.CheckOrCopyToTempDirectory();
                ObsFileChartEditForm form = new ObsFileChartEditForm(site.FilePath, true, false, false, true);
                form.Show();
            }
        }

        private void �ָ����в�վԭʼ�ļ�SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("�Ƿ�Ҫ�ָ����в�վ���޸ģ� ") == DialogResult.Yes)
            {
                foreach (var site in sites)
                {
                    site.CopyToTempDirectory();
                }
            }
        }

        private void �鿴���Ǹ߶Ƚ�HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            Thread thread = new Thread(new ParameterizedThreadStart(GnsserFormUtil.OpenSatElevationTableForm));
            thread.Start(site.FilePath);
        }


        bool Check(SiteObsBaseline line, bool mustHasResult = true)
        {
            if (line == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�л������ԣ�"); return false; }
            if (mustHasResult && line.EstimatedResult == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ԣ�" + line.LineName); return false; }
            return true;
        }
        #endregion

        #region  ���

        private void button_runIndependentLine_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ԣ�"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);

            //ע�����ﷵ�ص��ǲ�ͬʱ�εĶ������߼���
            var independentLineNet = BaseLineNet.GetIndependentNet(IndependentLineSelectType);
            objectTableControl_independentLine.Tag = independentLineNet;
            objectTableControl_independentLine.DataBind(independentLineNet.GetLineTable());
        }

        IndependentLineSelectType IndependentLineSelectType => this.enumRadioControl_selectLineType.GetCurrent<IndependentLineSelectType>();


        private void button_saveAllAsGNSSerFile_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ԣ�"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);
            var name = ProjectName + BaseLineNet.BaseLines.First().Name + "_etc_" + this.GetOrCreateOption().ObsPhaseType + "_All";
            SaveGnsserBaselineResult(BaseLineNet, name);
        }

        private void button_saveAllAsLgoAsc_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ԣ�"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);
            var name = ProjectName + BaseLineNet.BaseLines.First().Name + "_etc_" + this.GetOrCreateOption().ObsPhaseType + "_All";
            SaveAsLeoAsc(BaseLineNet, name);
        }


        private void button_saveIndeToLeoAsc_Click(object sender, EventArgs e)
        {
            BaseLineNet BaseLineNet = objectTableControl_independentLine.Tag as BaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("�������ɶ����������ԣ�"); return; }
            var name = ProjectName + BaseLineNet.BaseLines.First().Name + "_etc_" + this.GetOrCreateOption().ObsPhaseType + "_Indpt";
            SaveAsLeoAsc(BaseLineNet, name);
        }

        private void button_saveIndeBaselineFile_Click(object sender, EventArgs e)
        {
            BaseLineNet BaseLineNet = objectTableControl_independentLine.Tag as BaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("�������ɶ����������ԣ�"); return; }
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

        #region ��ͼ��ʾ

        private void button_viewSelectedTriPathes_Click(object sender, EventArgs e)
        {
            var triangles = Geo.Utils.DataGridViewUtil.GetSelectedRows(objectTableControl_closureErrors.DataGridView);
            List<GnssBaseLineName> lineNames = new List<GnssBaseLineName>();
            foreach (var item in triangles)
            {
                var pathString = item.Cells["�պ�·��"].Value.ToString();
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
            if (lines == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ԣ�"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(new List<EstimatedBaseline>() { (EstimatedBaseline)lines.EstimatedResult });

            ShowLinesOnMap(BaseLineNet);
        }
        private void button_showAllLineeOnMap_Click(object sender, EventArgs e)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ԣ�"); return; }
            BaseLineNet BaseLineNet = new BaseLineNet(lines);

            ShowLinesOnMap(BaseLineNet);
        }
        private void button_showIndeLineOnMap_Click(object sender, EventArgs e)
        {
            BaseLineNet BaseLineNet = objectTableControl_independentLine.Tag as BaseLineNet;
            if (BaseLineNet == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("�������ɶ����������ԣ�"); return; }

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

            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "����");
            ShowLayer(layer);
        }
        private void ShowLinesOnMap(List<GnssBaseLineName> lineNames)
        {
            var lines = GetAllLines();
            if (lines == null || lines.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ԣ�"); return; }
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

            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "������");
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

        private void ��ͼ��ʾ��վSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = GetAllSites();
            if (sites == null || sites.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("���������ݺ����ԣ�"); return; }
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

        private void �鿴���в�վʱ��ͼPToolStripMenuItem_Click(object sender, EventArgs e)
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
            EpochChartForm chartForm = new EpochChartForm(timeperiods, "��վʱ��ͼ");
            chartForm.Show();
        }
        #endregion
        #region  ��ʽ����վ
        public ObsFileConvertOption ObsFileConvertOption { get; set; }
        public ObsFileConvertOption GetOrInitObsFileFormatOption()
        {
            if (ObsFileConvertOption == null) { ObsFileConvertOption = new ObsFileConvertOption(); }
            return ObsFileConvertOption;
        }
        private void ��ʽ����ǰ��վFToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ������һ���ø�ʽ����ǰ��վTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { return; }
            RunFormateSitesTask(new List<ObsSiteInfo>() { site });
        }

        private void ��ʽ�����в�վAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }

            this.ObsFileConvertOption = GetOrInitObsFileFormatOption();
            var form = new ObsFileConvertOptionForm(ObsFileConvertOption);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ObsFileConvertOption = form.Option;

                RunFormateSitesTask(sites);
            }
        }
        private void ������һ���ø�ʽ���в�վMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("�Ƿ�Ҫ������һ���ø�ʽ�����й۲��ļ�����ʽ���󣬽����浽��ʱĿ¼�� ") == DialogResult.Yes)
            {
                RunFormateSitesTask(sites);
            }
        }

        public void RunFormateSitesTask(List<ObsSiteInfo> sites)
        {
            //�����߳���ִ�б���
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
                    log.Warn("����ʱ�ļ�·����һ�£���ʽ�����Ϊ: " + ObsFileFormater.FirstOutputPath + ",  ��ʱ·��Ϊ: " + site.TempFilePath);
                }
                else
                {
                    log.Info(site.SiteName + " ���, ��ʽ������� " + ObsFileFormater.FirstOutputPath);
                }
            }

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(sites[0].TempDirectory, "��ʽ����ϣ��Ƿ����ʱĿ¼�������Ҫ�滻Դ�ļ������ֶ�������");
        }

        #endregion
        
        #region  PPP ����
        private void �鿴��ǰ��վPPP����ͼVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }

            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
            var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
            GnsserFormUtil.ShowChartForm(path, paramNames);
        }

        private void �򿪵�ǰ��վPPP���RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
            TryOpenTableForm(path);

        }
        private void �鿴���в�վPPP����ͼAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            var names = new List<string>();
            foreach (var site in sites)
            {
                var path = Path.Combine(ProjectDirectory, resultFileNameBuilder.BuildEpochParamFileName(site.SiteObsInfo.FileInfo.FileName));
                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                GnsserFormUtil.ShowChartForm(path, paramNames);
            }

        }

        private void pPP���㲢���µ�ǰͷ�ļ�PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }

            var pathes = new string[] { site.FilePath };
            var t1 = new System.Threading.Tasks.Task(() => RunPpp(pathes));
            t1.Start();
        }
        private void �����ļ�PPP������ͷ�ļ�MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sites = this.GetAllSites(); if (sites == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            var names = new List<string>();
            foreach (var site in sites)
            {
                names.Add(site.FilePath);
            }
            //�����߳���ִ�б���
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
            log.Info("���³ɹ��� ����� �� " + site.TempFilePath);

            log.Info(entity.Name + ", " + entity.ReceiverTime + "�� ���������ļ�");
            var writer = new GnssResultWriter(Solver.Option,  Solver.Option.IsOutputEpochResult,
                Solver.Option.IsOutputEpochSatInfo);

            writer.WriteFinal(entity);
        }

        private void PppRunner_Completed(object sender, EventArgs e)
        {
            var site = GetCurrentSite(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("��ѡ�в�վ���ԣ�"); return; }
            var temptempDir = Path.Combine(site.TempDirectory, "Temp");
            Geo.Utils.FileUtil.TryDeleteFileOrDirectory(temptempDir);
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("PPP������ϣ���ʱ�ļ������Ѹ��£��Ƿ�����ʱ�ļ��滻Դ�ļ���") == DialogResult.Yes)
            {
                var pathes = Directory.GetFiles(site.TempDirectory);
                foreach (var result in pathes)
                {
                    var original = Path.Combine(site.Directory, Path.GetFileName(result));
                    Geo.Utils.FileUtil.MoveFile(result, original, true);
                    log.Info("���滻 �� " + original);
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
            if (this.backgroundWorker_netSolve.IsBusy && FormUtil.ShowYesNoMessageBox("����������̨û��������ϡ��Ƿ�һ��Ҫ�˳���") == DialogResult.No) { e.Cancel = true; }
            //    else { IsClosed = true; }
        }

        private void listBox_site_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView_netLine_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //���԰���ʾ 
            if (e.Node != null)
            {
                this.attributeBox1.SetObject(e.Node.Tag, false);
            }


            //��ͼ��ʾ//�����ı���ʾ
            if ((e.Node.Tag is ObsSiteInfo))
            {
                var baseSite = treeView_netLine.SelectedNode.Tag as ObsSiteInfo;

                //�ı���ʾ
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("��׼վ��" + baseSite.ToString());
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

                //��ʾ�в�
                var baseSiteName = baseSite.SiteName;
                ObjectTableStorage table = TryLoadNetSolvePhaseResidualTable(baseSiteName, true);
                if (table != null)
                {
                    this.commonChartControl_currentResidual.DataBind(table);
                    commonChartControl_currentResidual.ShowInfo(table.Name);
                }

                //��ǰ���߲�������ͼ 
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
                    //��ǰ���߱պϲ�
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
       
        #region  �������㷨����ϸ��
        /// <summary>
        /// ���Լ�������˫�������ͼ
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
                //ֱ�ӷ���
                if (!isFilter) { return table; }

                var paramNames = new List<string>() { ParamNames.Epoch, ParamNames.De, ParamNames.Dn, ParamNames.Du };
                var nameBuilder = new NetDoubleDifferPositionParamNameBuilder(this.GetOrCreateOption());
                foreach (var siteName in this.ObsFileManager.Keys)
                {
                    var names = nameBuilder.GetSiteDxyz(siteName);
                    paramNames.AddRange(names);
                }

                table = table.GetTable(table.Name, paramNames);
                table.RemoveRows(0, 20);//Ĭ���Ƴ�ǰʮ
            }

            return table;
        }
        /// <summary>
        /// ���Լ��������ز��в�ͼ
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
                //ֱ�ӷ���
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
                Geo.Utils.FormUtil.ShowWarningMessageBox("������æ�����Եȡ�����");
                return;
            }
            CurrentCenterSiteToSolve = this.ObsFileManager.Keys;// new List<string>();
            backgroundWorker_netSolve.RunWorkerAsync();
        }
        private void button_solveCurrentNet_Click(object sender, EventArgs e)
        {
            if (backgroundWorker_netSolve.IsBusy)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("������æ�����Եȡ�����");
                return;
            }
            if(treeView_netLine.SelectedNode == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("ѡ�к����ԣ�");
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
        #region ����������
        List<string> CurrentCenterSiteToSolve { get; set; }

        private void NetSolveBaseLine()
        {
            if (false)//������һ��Option�Ǵ����
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
        /// �������
        /// </summary>
        /// <param name="baseSiteName"></param>
        private void NetSolveBaseLine(string baseSiteName)
        {
            log.Info("��������ָ���� " + baseSiteName + " ���⡣");

            this.GetOrCreateOption();
            this.UiToEntity();

            Option.GnssSolverType = GnssSolverType.����˫�λ;
            Option.IndicatedBaseSiteName = baseSiteName;
            Option.BaseSiteSelectType = BaseSiteSelectType.Indicated;
            Option.IsBaseSiteRequried = true;
            //��ͬ�㷨�����ͬ��Ŀ¼
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
            Option.GnssSolverType = GnssSolverType.����˫�λ;
            var optionForm = new OptionVizardForm(Option);
            optionForm.Init();
            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Option = optionForm.Option;
                this.EntityToUi();
            }
        }

        private void �鿴���вв�ͼRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //��ʾ�в�
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

        private void �鿴��������ͼCToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //��ǰ���߲�������ͼ
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
                e.DrawDefault = true; //������Ĭ����ɫ��ֻ��Ҫ��TreeViewʧȥ����ʱѡ�нڵ���Ȼͻ��  
                return;
            }
            //���ɼ�ʱ�����ƽ������ڶ���
            if (!e.Node.IsVisible)
            {
                e.DrawDefault = true;
                return;
            }

            //     log.Error(e.Node.Text + " - " + e.State);
            if ((e.Node.Tag is SiteObsBaseline))
            {
                var item = e.Node.Tag as SiteObsBaseline;

                // �ж���ʲô���͵ı�ǩ
                ResultBackGroundStyle backGroundStyle = new ResultBackGroundStyle(item.ResultState); 

                e.Graphics.FillRectangle(backGroundStyle.BackGroundBrush, e.Bounds);
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;
                //�ı�  
                //e.Graphics.DrawString(e.Node.Text, nodeFont, new SolidBrush(treeView_netLine.ForeColor), e.Bounds, StringFormat.GenericDefault);
                e.Graphics.DrawString(e.Node.Text, nodeFont, new SolidBrush(treeView_netLine.ForeColor), Rectangle.Inflate(e.Bounds, 2, 0));
            }
        }

        private void ȫ��չ��EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView_netLine.ExpandAll();
        }

        private void ȫ������CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView_netLine.CollapseAll();
        }

        private void չ��EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeView_netLine.SelectedNode.Expand();
        }

        private void ����CToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            this.treeView_netLine.SelectedNode.Collapse();
        }

        public ObsSiteInfo GetCurrentNodeSite()
        {
            return Geo.Utils.TreeNodeUtil.GetCurrentObject<ObsSiteInfo>(this.treeView_netLine.SelectedNode);
        }

        private void �鿴�в�ͼSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //��ʾ�в�
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolvePhaseResidualTable(baseSiteName, true);
            if (table != null)
            {
                CommonChartForm form = new CommonChartForm(table);
                form.Show();
            }
            else   {    log.Warn("û���ҵ��в��ļ���");    }
        }

        private void �鿴����ͼLToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolveConverageTable(baseSiteName, true);
            if (table != null)
            {
                CommonChartForm form = new CommonChartForm(table);
                form.Show();
            }
            else { log.Warn("û���ҵ������ļ���"); }
        }

        private void �鿴�����ļ�PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolveConverageTable(baseSiteName,false);
            if (table != null)
            {
                var form = new TableObjectViewForm(table);
                form.Show();
            }
            else { log.Warn("û���ҵ������ļ���"); }
        }

        private void �򿪲в��ļ�OToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var baseSiteName = GetCurrentNodeSite().SiteName;
            ObjectTableStorage table = TryLoadNetSolvePhaseResidualTable(baseSiteName, false);
            if (table != null)
            {
                var form = new TableObjectViewForm(table);
                form.Show();
            }
            else { log.Warn("û���ҵ��в��ļ���"); }
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    