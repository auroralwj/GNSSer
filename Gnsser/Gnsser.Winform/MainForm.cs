//2012.04.17 14:00, czs, create in zz, GNSS 主界面程序
//2014.12.27, lh, edit, 添加部分 Teqc 模块
//2015.04.26, czs, edit  in namu, 重构了部分功能，代码缩减到 499行。
//2016.07.30, czs, edit in fujian yong'an,  多个版本合并为一个界面
//2017.07.14, czs, edit in hongqing, 菜单分类，代码整合
//2017.10.26, czs, edit in hongqing, 整理日志窗口，提取到单独
//2019.01.17, czs, edit in hmx, 增加大量基线处理相关菜单

using AnyInfo;
using Geo;
using Geo.Utils;
using Geo.Winform;
using Geo.Winform.Demo;
using Geo.Winform.Tools;
using Geo.Winform.Wizards;
using Geo.WinTools;
using Gnsser.Interoperation.Bernese;
using Gnsser.Winform.Service.Adjustment;
using Gnsser.Winform.Suvey;
using Gnsser.Winform.Testing;
using Gnsser.Winform.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    /// <summary>
    /// GNSSer 主界面程序
    /// </summary>
    public partial class MainForm : Form, IMainForm
    {
        #region 构造函数与初始化
        /// <summary>
        /// 主界面程序唯一构造函数。
        /// </summary>
        public MainForm() { InitializeComponent(); }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = ConfigurationManager.AppSettings["Title"];

            toolStripButton_close_Click(null, null);
            this.logListenerControl1.TextChanged += logListenerControl1_TextChanged;

            起始向导页SToolStripMenuItem_Click(this, null);
            switch (Setting.VersionType)
            {
                case Geo.VersionType.Public:
                case VersionType.BaselineNet:
                    this.menuStrip_develop.Visible = false;
                    //this.toolStripButton1单点定位.Visible = false;
                    this.toolStripSeparator26ppp.Visible = false;
                    this.toolStripButton1RtkLib.Visible = false;
                    this.toolStripButton统计分析.Visible = false;
                    this.toolStripButton1RtkLib.Visible = false;
                    this.toolStripButton2网平差.Visible = false;
                    this.toolStripButtonGof分布式计算.Visible = false;
                    this.分析AToolStripMenuItem.Visible = false;
                    this.toolStripButton_intelGnssProcess.Visible = false;
                    this.Text = "GNSSer—GNSS数据处理软件(基线网测试版) v" + Setting.Version + " www.gnsser.com";
                    if (Geo.VersionType.Public == Setting.VersionType)
                    {
                        基线网闭合差计算器CToolStripMenuItem1.Visible = false;
                        同步基线解算CToolStripMenuItem.Visible = false;
                        toolStripButton_multiSiteNetSolve.Visible = false;
                        多站网解NToolStripMenuItem.Visible = false;
                        欢迎WToolStripMenuItem_Click(sender, e);
                    }
                    else
                    {
                    }
                    break;
                case Geo.VersionType.Development:
                    this.menuStrip_public.Visible = false;
                    this.Text = "GNSSer—GNSS数据处理软件(桌面开发版) v" + Setting.Version + " www.gnsser.com";
                    起始页SToolStripMenuItem_Click(sender, e);
                    break;
                case Geo.VersionType.DistributionTesting:
                    this.menuStrip_public.Visible = false;
                    this.Text = "GNSSer—GNSS数据处理软件(桌面内测版) v" + Setting.Version + " www.gnsser.com";
                    break;
                default:
                    break;
            }

        }

        #endregion

        #region 公用工具函数

        #region 打开子窗口
        /// <summary>
        /// 打开子窗口
        /// </summary>
        /// <param name="f"></param>
        public void OpenMidForm(Form f) { this.mdiTab1.OpenMdiChild(f); }
        /// <summary>
        /// 强制打开子窗口，先关闭，再打开。
        /// </summary>
        /// <param name="f"></param>
        public void ForceOpenMidForm(Form f) { ForceOpenMidForm(f.Text, f.GetType()); }
        /// <summary>
        /// 强制打开子窗口，先关闭，再打开。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type"></param>
        public void ForceOpenMidForm(string title, Type type)
        {
            this.mdiTab1.Close(title);
            this.OpenMidForm(title, type);
        }
        /// <summary>
        /// 打开指定类型的窗口。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type"></param>
        public void OpenMidForm(string title, Type type)
        {
            Form form = this.mdiTab1.OpenMidForm(title, type);
            if (form != null)
            {
                if (form is IShowLayer)
                {
                    IShowLayer showLayer = form as IShowLayer;
                    showLayer.ShowLayer += new ShowLayerHandler(form_showLayer);
                }
                if (form is IWithMainForm)
                {
                    IWithMainForm obj = form as IWithMainForm;
                    obj.MainForm = this;
                }
            }
        }
        /// <summary>
        /// 通过名称，激活对话框。
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public bool Activate(string f) { return this.mdiTab1.Activate(f); }
        #endregion

        #region Map
        void form_showLayer(Layer layer)
        {
            if(layer == null) { return; }

            layer.IsShowFeatureName = true;
            this.AddLayerToCurrrentMapOrNewMap(layer);
        }

        //将地图显示出来
        int iTemp = 0;
        private void AddLayerToCurrrentMapOrNewMap(Layer layer, string title = "地图显示")
        {
            if (layer == null) return;
            layer.Name = layer.Name + iTemp++;
            String mapName = "新地图";
            Map map = new Map(mapName);

            map.AddLayer(layer);

            ShowMap(map, title);
        }

        private void ShowMap(Map map, string title = "地图显示")
        {
            Form form = null;
            if (this.mdiTab1.Contains(title))
            {
                form = this.mdiTab1.GetForm(title);
            }
            else
            {
                form = new SingleMapForm(map) { Text = title };
            }
            var mapForm = form as SingleMapForm;
            if (mapForm.Map == null)//地图
            {
                mapForm.SetMap(map);
            }
            else//添加图层
            {
                foreach (var layer in map.Layers)
                {
                    mapForm.Map.AddLayer(layer);
                }
                mapForm.MapsToTreeMenus();
            }

            SetCenterAndRoom(mapForm.Map);

            //SingleMapForm mapForm = new SingleMapForm(map);
            //mapForm.Text = "地图-" + iTemp++;
            OpenMidForm(mapForm);
            mapForm.Refresh();
        }

        private void SetCenterAndRoom(Map map)
        {
            int lastIndex = map.Layers.Count - 1;
            var lastLayer = map.Layers[lastIndex];
            map.CenterLonLat = new Geo.Coordinates.LonLat(lastLayer.Extent.Center.X, lastLayer.Extent.Center.Y);
            map.SetProperMaxZoom(lastLayer.Extent.Width, this.Width);
        }
        private void 浏览地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnyInfo.Map map = new AnyInfo.Map("地图浏览");
            AnyInfo.SingleMapForm form = new AnyInfo.SingleMapForm(map);
            this.mdiTab1.OpenMdiChild(form);
        }

        private void 地图设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnyInfo.WinUI.GoogleTileSettingForm1 form = new AnyInfo.WinUI.GoogleTileSettingForm1(true);
            form.ShowDialog();
        }

        #endregion

        #endregion

        #region 文件
        private void 起始向导页SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("快速访问", typeof(RapidAccessForm)); }


        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e) { Application.Exit(); }
        private void 重启ToolStripMenuItem_Click(object sender, EventArgs e) { Application.Restart(); }
        private void 起始页SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm(new ProjectStartingForm(this)); }
        private void 参数文件管理ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("参数文件管理", typeof(ParamFileMamagerForm)); }

        private void 周跳剔除向导ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardPageCollection WizardPageCollection = new Geo.Winform.Wizards.WizardPageCollection();
            WizardPageCollection.Add(1, new SelectRinexFileWizardPage());
            WizardPageCollection.Add(4, new Page1());
            WizardPageCollection.Add(2, new Page2());
            WizardPageCollection.Add(3, new Page3());

            var host = new WizardForm(WizardPageCollection);
            //   var host = new WizardForm(WizardPageCollection);
            host.Text = "My Wizards";
            // host.WizardCompleted += new WizardHostForm.WizardCompletedEventHandler(host_WizardCompleted);

            host.LoadWizard();
            host.ShowDialog();
        }
        private void 对流层延迟计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("对流层计算", typeof(TropCaculateForm)); }


        #region 工程
        private void 新建工程CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("新建工程", typeof(ProjectCreationForm)); }
        private void 打开当前工程界面CToolStripMenuItem_Click(object sender, EventArgs e) { ForceOpenMidForm(Setting.GnsserConfig.CurrentProject.ProjectName + "工程视图", typeof(ProjectWorkViewForm)); }

        private void 打开工程PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "*.gproj";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Setting.GnsserConfig.OpenAndSetCurrentProject(dlg.FileName);
                ForceOpenMidForm("当前工程视图", typeof(ProjectWorkViewForm));
            }
        }
        #endregion


        #endregion

        #region 数据准备
        private void 测站信息维护SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("基准站信息维护", typeof(SiteInfoManagerForm)); }
        private void 提取测站信息EToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("提取测站信息", typeof(StationInfoExctracterForm)); }

        private void 导航文件下载器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("导航文件下载器", typeof(NaviFileDownloadForm)); }
        private void 选择观测文件SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("关键字文件选择器", typeof(KeyNameFileSelectorForm)); }
        private void 星历下载器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("IGS产品下载器", typeof(IgsProductDownloadForm)); }
        private void 格式化RinexToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("TEQC格式化", typeof(TeqcFormatRinexForm)); }
        private void 压缩解压缩ToolStripMenuItem_Click(object sender, EventArgs e) { new Geo.WinTools.Sys.ZipForm().ShowDialog(); }
        private void 文件下载ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void 批量下载GNSS数据ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("地址生成与下载", typeof(BuildAndDownFileForm)); }

        private void 观测文件下载器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("观测文件下载器", typeof(ObsFileDownloadForm)); }
        private void 解压缩d文件ToolStripMenuItem_Click(object sender, EventArgs e) { new DecompactRinexForm().Show(); }
        private void 文件名大小写转换ToolStripMenuItem_Click(object sender, EventArgs e) { new UpLowFileName().Show(); }
        private void gnss文件地址生成器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("文件地址生成器", typeof(FileUrlGenForm)); }
        private void 批量文件下载ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("批量文件下载", typeof(DownFilesForm)); }
        private void 自动处理ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("数据准备自动处理", typeof(DataPrepareForm)); }
        private void 格式化观测文件ToolStripMenuItem_Click(object sender, EventArgs e) { new OFileFormatForm().Show(); }
        private void 观测文件选择器ToolStripMenuItem_Click(object sender, EventArgs e) { new OFileSelectorForm().Show(); }
        private void 测站分区ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("测站分区", typeof(BlockingForm)); }
        private void 格式化RINEX观测文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("格式化/转换Rinex观测文件", typeof(OFileFormaterForm)); }
        private void 格式化观测文件FToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("格式化单站观测文件", typeof(SingleGnssFileFormateForm)); }
        private void 格式化网解测站观测文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("格式化网解观测文件", typeof(MultiGnssFileFormateForm)); }
        private void 查看IGS宽项VToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("查看Fr-IGS宽项", typeof(WideLaneGpsBiasViewerForm)); }
        private void 更新观测文件坐标ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("更新观测文件坐标", typeof(ObsFileCoordUpdaterForm)); }
        private void 观测文件选择器ToolStripMenuItem_Click_1(object sender, EventArgs e) { OpenMidForm("观测文件选择器", typeof(ObsFileSelectForm)); }
        private void 分析观测文件AToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("观测文件分析器", typeof(ObsDataAnanasisForm)); }
        private void o文件转换为表格文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("O文件转换为表格文件", typeof(OFileToTableForm)); }
        private void 表格文件转换为O文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("表格文件转换为O文件", typeof(TableToOFileForm)); }
        #endregion

        #region 数据查看
        private void 比较坐标文件CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("坐标比较", typeof(XyzCompareForm)); }
        private void 获取SINEX真值ToolStripMenuItem_Click(object sender, EventArgs e) { new Gnsser.Winform.Other.SinexCoord().ShowDialog(); }
        private void 选取SINEX测站ToolStripMenuItem_Click(object sender, EventArgs e) { new Gnsser.Winform.Other.ExtractSinexSite().ShowDialog(); }
        // static int formIndex = 0;

        private void 单文件多表数据查看器VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MultiTableTextOpenVizardForm();
            form.ShowDialog();
        }
        static int formIndex = 0;
        private void 表文件查看器TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Setting.TotalTextTableFileFilter;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;
                foreach (var filePath in files)
                {
#if !DEBUG
                    try
                    {
#endif
                    var reader = new ObjectTableReader(filePath, Encoding.Default);
                    var table = reader.Read();//.GetDataTable();  
                    var fileName = System.IO.Path.GetFileName(filePath);

                    var form = new Geo.Winform.TableObjectViewForm(table) { Text = fileName + "_" + (formIndex++) };
                    form.FilePath = filePath;
                    OpenMidForm(form);
#if !DEBUG
                    }catch(Exception ex)
                    {
                        Geo.Utils.FormUtil.ShowErrorMessageBox("解析失败： " + filePath + ", " + ex.Message);
                    }
                  //  form.Show();
#endif
                }
            }

            if (false)
            {
                TableTextOpenVizardForm form = new TableTextOpenVizardForm();
                form.ShowDialog();
            }
        }

        private void 观测卫星时段查看ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("查看卫星观测时段", typeof(ObsSatViewerForm)); }
        private void 天线文件查看ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("天线文件查看", typeof(AntennaViewForm)); }
        private void 文件名ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Rinex文件名", typeof(RinexFileNameForm)); }
        private void 头文件信息ToolStripMenuItem_Click(object sender, EventArgs e) { { OpenMidForm("头文件信息查看", typeof(ObsFileMetaViewerForm)); } }
        private void O文件查看器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm(" 查看观测内容", typeof(ObsFileViewerForm)); }

        private void 批量观测文件查看MToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("批量观测文件查看", typeof(MultiObsFileViewerForm)); }

        private void 查看观测时段ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("查看文件观测时段", typeof(ViewObsTimePeriodForm)); }
        private void 精密星历查看ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("精密星历查看", typeof(SP3ViewerForm)); }
        int siteMapCount = 1;
        private void 观测站地图分布ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Layer layer = GnssFormFactory.OpenAndShowOFileOnMap("观测站地图分布" + siteMapCount++);
            if (layer != null) { this.AddLayerToCurrrentMapOrNewMap(layer); }
        }
        private void xYZ数据文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("GNSSer数据文件查看器", typeof(GnsserDataFileViewerForm)); }
        private void 查看钟差文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("钟差文件查看器", typeof(ClockViewerForm)); }
        private void 坐标文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Bernese坐标文件查看器", typeof(BerCrdViewForm)); }
        private void 速度文件ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Bernese速度文件查看器", typeof(BerVelViewForm)); }
        private void sinexSToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Sinex文件查看提取器", typeof(SinexViewForm)); }
        private void sinex批量查看ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Sinex批量查看", typeof(MultiSinexViewForm)); }
        private void sinex比较ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Sinex比较", typeof(SinexCompareForm)); }
        private void fCB服务器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("武大FCB服务器", typeof(WhuFcbServiceForm)); }
        private void fCB查看器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("武大FCB文件查看器", typeof(WhuFcbViewerForm)); }
        private void fCB比较器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("武大FCB文件比较器", typeof(WhuFcbComparerForm)); }
        #endregion

        #region 数据分析，预处理
        private void 周跳探测DToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("实时周跳探测", typeof(CycleSlipDetectForm)); }
        private void 探测与修复DToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("探测与修复", typeof(OFileFixingForm)); }
        private void 缓存周跳探测BToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("缓存周跳探测", typeof(BufferedCycleSlipDetectForm)); }
        private void 滑动平均窗口ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("数据平滑", typeof(DataSmoothForm)); }
        private void 载波相位平滑伪距SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("载波相位平滑伪距", typeof(RangeSmootherForm)); }
        private void 粗差探测ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("粗差探测", typeof(GrossDetectingForm)); }
        private void 滑动平均ToolStripMenuItem_Click(object sender, EventArgs e) { new FileFittingForm().ShowDialog(); }
        private void 采样率稀疏ToolStripMenuItem_Click(object sender, EventArgs e) { new TextFilterForm().ShowDialog(); }
        private void 粗差过滤ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("粗差过滤", typeof(ResidualFilterForm)); }
        private void 质量检核ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("质量检核", typeof(QualityCheckForm)); }
        #endregion

        #region 计算服务
        #region 坐标计算
        private void 基线坐标查看ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("向量基本信息", typeof(VectorInfoForm)); }
        private void 本地坐标与地心坐标的转换LToolStripMenuItem_Click(object sender, EventArgs e) { string title = "本地坐标与地心坐标的转换"; OpenMidForm(title, typeof(LocalToGeoCoordForm)); }
        private void 坐标时间序列比较ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("坐标比较时间序列", typeof(XyzTimeSeriesCompareForm)); }
        #endregion

        #region 轨道、钟差计算

        private void dOP计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("DOP计算", typeof(DopForm)); }

        private void rinex导航文件转换ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("导航文件格式转换", typeof(NaviFileConvertForm)); }

        private void 替换igs中的钟差ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("替换igs中的钟差", typeof(IGUEphemerisExportSp3BasedOnIgsForm)); }
        private void 广播星历提取与质量评估ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("广播星历提取与质量分析", typeof(BroadcastEphemerisExportForm)); }
        private void 基于BNC获取实时产品ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("基于BNC获取实时产品", typeof(BNCSSRExportForm)); }
        private void 提取星历ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("提取星历", typeof(EphemerisExportForm)); }
        private void 提取钟差ToolStripMenuItem_Click(object sender, EventArgs e) { new ClockExportForm().ShowDialog(); }

        private void 站星位置计算器SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("站星位置计算器", typeof(SatPolarForm)); }
        private void 卫星高度角EToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("测站卫星高度角计算", typeof(SatEevationSolverForm)); }
        private void 开普勒轨道计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("开普勒轨道计算", typeof(KelplerOrbitForm)); }
        private void 卫星位置计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("卫星位置计算", typeof(SatPosCaculateForm)); }
        private void 单文件服务ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("单文件星历服务", typeof(SingleFileEphemerisServiceForm)); }
        private void 多文件服务ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("多文件星历服务", typeof(EphemerisServiceForm)); }
        private void 星历数据对比ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("星历数据对比", typeof(EphemerisComparingForm)); }
        private void 计算轨道参数ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("计算轨道参数", typeof(OrbitDeterminationForm)); }
        private void 钟差比较ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("钟差比较", typeof(ClockComparingForm)); }
        private void 卫星轨道两行根数ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("卫星轨道两行根数", typeof(TwoLineEleOrbitForm)); }
        private void 轨道计算工具ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("轨道计算工具", typeof(OrbitToolsForm)); }
        #endregion

        #region GNSS计算

        private void toolStripButton_multiSiteNetSolve_Click(object sender, EventArgs e) { OpenMidForm("GNSS网解", typeof(MultiSiteNetSolveForm)); }

        private void toolStripButton1双差_Click(object sender, EventArgs e) { OpenMidForm("基线解算", typeof(TwoSiteSolveForm)); }
        private void GNSS集成计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("GNSS集成计算器", typeof(IntegralGnssFileSolveForm)); }
        private void 单点定位数据流ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("单点定位计算", typeof(SingleGnssFileSolveForm)); }
        private void 单基线解算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("流动站基线解算", typeof(BaseLinePositionForm)); }
        private void 批量流动站基线解算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("批量流动站基线解算", typeof(MultiBaseLinePositionForm)); }
        #endregion

        #region 平差计算
        private void 网平差计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("网平差计算", typeof(NetAdjustment)); }
        private void 合并坐标文件CToolStripMenuItem_Click(object sender, EventArgs e) { new CombinationXyzForm().ShowDialog(); }
        private void 坐标求平均VToolStripMenuItem_Click(object sender, EventArgs e) { new AveragingXyzForm().ShowDialog(); }
        private void 抗差贝叶斯估计ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("抗差贝叶斯估计", typeof(GnssNetworkRobustBayes)); }
        private void gnss网平差ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("网平差", typeof(GnssNetworkAdjustment)); }
        private void 坐标历元归算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("坐标历元归算", typeof(CoordEpochReductionForm)); }
        private void sINEX对比合并ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("对比合并独立Sinex", typeof(MergeSinexForm)); }
        private void gNSS子网联合平差ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("子网整体平差", typeof(SubNetUnionForm)); }
        private void 分区平差ToolStripMenuItem1_Click(object sender, EventArgs e) { OpenMidForm("分区平差", typeof(BlockAdjustForm)); }
        private void ambizapToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Ambizap基线约束平差", typeof(AmbizapForm)); }
        #endregion


        #region 钟差计算
        private void 钟差预报ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("钟差预报", typeof(ClockPredictionBasedonClockFileForm)); }
        private void 钟差预报基于clkToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("IGS生成SP3", typeof(ClockPredictionBasedonSp3Form)); }
        private void sP3文件生成ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("SP3文件生成", typeof(ClockPredictionBasedonSp3ExportForm)); }
        private void clk文件生成ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("CLK文件生成", typeof(ClockPredictionBasedonClockFileExportForm)); }
        private void 钟差估计结果比较ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("钟差估计结果比较", typeof(ClockEstimationResultCompareForm)); }
        private void 实时SSR钟改正合并ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("实时SSR钟改正合并", typeof(RTEphemerisExportSp3Form)); }
        private void 实时钟差sp3生成基于SSR钟改正ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("基于SSR钟改正的实时钟差sp3", typeof(SSRExportForm)); }
        private void 提取数据生成xlsToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("提取IGU数据", typeof(IGUEphemerisExportForm)); }
        private void 生成sp3ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("IGU生成SP3", typeof(IGUEphemerisExportSp3Form)); }
        #endregion

        #region PPP模糊度计算
        private void 计算宽窄项ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("计算宽窄项", typeof(PointPositioningAR)); }
        private void 时段基准卫星选择器SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("时段基准卫星选择器", typeof(SatelliteSelectorForm)); }
        private void mW提取器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("宽巷(MW)产品生成", typeof(MwFractionTableBuilderForm)); }
        private void pPP模糊度计算向导ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("PPP模糊度固定向导", typeof(PPPAmbiguityVizardForm)); }
        private void 单星BSD集成计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("单星BSD集成计算", typeof(MultiPeriodBsdProductSolverForm)); }

        private void bSD窄巷ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("单星BSD集成计算", typeof(MultiPeriodNarrowLaneOfBsdSolverForm)); }

        private void 全历元分段BSD宽巷ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("全历元分段BSD宽巷", typeof(MultiPeriodlWideLaneOfBsdSolverForm)); }

        private void 单星BSD宽巷ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("WideLaneOfBsdSolverForm", typeof(WideLaneOfBsdSolverForm)); }
        #endregion

        #region  其它计算
        private void 度弧度互转ToolStripMenuItem_Click(object sender, EventArgs e) { new DegRadConvertForm().Show(); }
        private void 角度单位转换ToolStripMenuItem_Click(object sender, EventArgs e) { new DegForamatConvertForm().Show(); }
        private void 对流层计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("对流层计算", typeof(BaseStartWizardForm)); }
        private void 计算天线高ToolStripMenuItem_Click(object sender, EventArgs e) { new CaculateAntenaHeightForm().Show(); }
        private void gNSS时间计算器CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("GNSS时间计算", typeof(GnssTimeForm)); }
        private void xYZLBHToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("空间直角与大地坐标系的转换", typeof(GeoXyzConvertForm)); }
        private void 日月计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("日月位置计算", typeof(SunMoonForm)); }

        private void xYZNEUToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("XYZ -> 本地 NEU", typeof(XyzNeuConvertForm)); }
        #endregion
        #endregion

        #region 分布式计算与互操作
        #region 分布式计算
        private void 计算节点管理ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("计算节点管理", typeof(ComputeNodeMgrForm)); }
        private void 测站名称编辑器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("测站名称编辑器", typeof(SiteNameMgrForm)); }
        private void 计算任务编辑器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("计算任务编辑器", typeof(TaskMgrForm)); }
        private void 分布式计算面板ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("主机控制面板", typeof(DistributeControlForm)); }
        private void 终端任务执行器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("终端任务执行器", typeof(BerTaskListenerForm)); }
        private void 文件路径ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("文件路径设置", typeof(PathSettingForm)); }
        private void 终端任务执行器ToolStripMenuItem1_Click(object sender, EventArgs e) { OpenMidForm("终端任务执行器", typeof(TaskClientForm)); }
        #endregion

        #region GNSSer分布式
        private void gPE运行器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("操作流运行器", typeof(OperflowRunnerForm)); }
        private void 参数文件查看器VToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("参数文件查看器", typeof(ParamFileViewerForm)); }
        private void gof分布式计算面板ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Gof分布式计算面板", typeof(GofDistributeControlForm)); }
        private void gof任务编辑器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Gof任务编辑器", typeof(GofTaskMgrForm)); }
        private void gof客户端执行器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Gof客户端执行器", typeof(GofClientRunnerForm)); }
        private void gof计算节点编辑器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Gof计算节点编辑器", typeof(GofComputeNodeMgrForm)); }
        #endregion

        #region 本地调用Bernese
        private void bernese计算面版ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("伯尔尼计算面版", typeof(BerneseRunnerForm)); }
        private void ber自动计算ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("伯尔尼自动计算", typeof(BerControlForm)); }
        private void ber任务解析执行ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("伯尔尼任务解析执行", typeof(TaskRunnerForm)); }
        #endregion

        #region Bernese文件处理
        private void sTA文件生成ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerGenFileForm(BerFileType.STA); }
        private void aBB文件生成ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerGenFileForm(BerFileType.ABB); }
        private void cRD坐标文件生成ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerGenFileForm(BerFileType.CRD); }
        private void vEL速度文件生成ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerGenFileForm(BerFileType.VEL); }
        private void sTA文件合并ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerMergeFileForm(BerFileType.STA); }
        private void aBB文件合并ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerMergeFileForm(BerFileType.ABB); }
        private void cRD坐标文件合并ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerMergeFileForm(BerFileType.CRD); }
        private void vEL速度文件合并ToolStripMenuItem_Click(object sender, EventArgs e) { OpenBerMergeFileForm(BerFileType.VEL); }
        private void OpenBerGenFileForm(BerFileType type) { if (!Activate(type + "文件生成")) OpenMidForm(BerFileFormFactory.CreateBerFileGenForm(type)); }
        private void OpenBerMergeFileForm(BerFileType type) { if (!Activate(type + "文件合并")) OpenMidForm(BerFileFormFactory.CreateBerFileMergeForm(type)); }
        #endregion

        #region TEQC
        private void teqc卫星系统选择ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Teqc卫星系统选择", typeof(SystemSelectionForm)); }
        private void teqc格式转换ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Teqc格式转换", typeof(TrsForm)); }
        private void teqc批处理ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Teqc批处理", typeof(BATForm)); }
        #endregion

        #region rtklib
        private void rtklib调用器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Rtkpost事后定位计算", typeof(RtkpostCallerForm)); }
        private void rtkrcv实时调用ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("Rtkrcv实时定位计算", typeof(RtkrcvCallerForm)); }
        private void rtk配置器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("配置文件编辑器", typeof(ConfigFileEditForm)); }

        private void rtkLib坐标结果查看PToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("RtkLib坐标结果查看", typeof(RtkPositionViewForm)); }
        #endregion

        #endregion

        #region 工具
        private void 进程查看ToolStripMenuItem_Click(object sender, EventArgs e) { new Geo.WinTools.ProcessInfoForm().Show(); }
        private void 时间计算ToolStripMenuItem_Click(object sender, EventArgs e) { new TimeCaculatorForm().Show(); }
        private void 历元计算器ToolStripMenuItem_Click(object sender, EventArgs e) { new GpsTimeForm().Show(); }
        private void 比较文件夹ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("比较文件夹", typeof(Geo.Winform.CheckSameFileForm)); }
        private void 命令行工具ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("命令行工具", typeof(Geo.Winform.Sys.CmdForm)); }
        private void 发送任务ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("发送任务", typeof(Geo.WinTools.TaskSenderForm)); }
        private void 监听任务ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("监听任务", typeof(Geo.WinTools.TaskListenerForm)); }
        private void 内存状态ToolStripMenuItem_Click(object sender, EventArgs e) { FormUtil.ShowMemoryStatusBox(); }
        private void 清除过期日志ToolStripMenuItem_Click(object sender, EventArgs e) { MessageBox.Show(Geo.IO.LogWriter.Instance.TryClearOutDateLogs()); }
        private void 字符转换ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("字符转换", typeof(AnsiCodeForm)); }
        private void 合并DLLToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("DLL合并", typeof(ILmergeForm)); }
        private void fTP下载器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("FTP下载器", typeof(FtpDownloaderForm)); }
        private void cMD运行器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("CMD运行器", typeof(CmdCallerForm)); }
        #region 选项
        private void 调试模式切换ToolStripMenuItem_Click(object sender, EventArgs e) { Setting.IsShowDebug = !Setting.IsShowDebug; 模式设置ToolStripMenuItem.Checked = Setting.IsShowDebug; }
        private void 星历数据源设置ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("星历数据源设置", typeof(Sp3SourceConfigForm)); }
        #endregion
        private void 系统日志ToolStripMenuItem_Click(object sender, EventArgs e) { FileUtil.TryOpenLog(); }
        private void 邮件发送ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("邮件发送", typeof(Geo.WinTools.Net.EmailSenderForm)); }
        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e) { new FileRenameForm().ShowDialog(); }
        #endregion

        #region 子窗口排列
        public void 层叠CToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.Cascade); }
        public void 垂直平铺VToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.TileVertical); }
        public void 水平平铺HToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.TileHorizontal); }
        public void 全部关闭LToolStripMenuItem_Click(object sender, EventArgs e) { foreach (Form childForm in MdiChildren) childForm.Close(); }
        public void 排列图标AToolStripMenuItem_Click(object sender, EventArgs e) { LayoutMdi(MdiLayout.ArrangeIcons); }
        #endregion

        #region 帮助
        private void 帮助文档HToolStripMenuItem_Click(object sender, EventArgs e) { FileUtil.OpenFile(Setting.HelpDocument); }

        private void 检查新版本CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Geo.Utils.NetUtil.IsConnectedToInternet())
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("需要联网才可以检查。");
                return;
            }
            if (!Program.CheckNewVersionNoticeGoVisit())
            {
                MessageBox.Show("当前已经是最新版本！");
            }
        }
        private void 显示启动页ToolStripMenuItem_Click(object sender, EventArgs e) { Geo.Utils.UiUtil.ShowSplash(""); }
        private void 欢迎WToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("使用须知", typeof(NoticeForm)); }
        #endregion


        #region 实验室
        private void 并行矩阵计算测试ToolStripMenuItem_Click(object sender, EventArgs e) { MatrixSpeedTest.CompareArraySpeed(); }

        private void 批量提取ZPDToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "ZPD文件(*.13zpd)|*.13zpd|所有文件(*.*)|*.*";
            openFileDialog.Multiselect = true;
            string[] paths;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                paths = openFileDialog.FileNames;

                string startUpPath = Path.GetDirectoryName(paths[0]);// Application.StartupPath;//可执行程序目录
                string ResultPath = startUpPath + "\\" + "all_zpd.txt";
                //  StreamWriter sw = new StreamWriter("C://pppResult.txt");
                StreamWriter sw = new StreamWriter(ResultPath);


                foreach (var item in paths)
                {

                    StreamReader sr = new StreamReader(item);
                    string read = sr.ReadLine();
                    while (read != null)
                    {
                        read = sr.ReadLine();
                        if (read.Contains("13:001:85500"))
                        {
                            sw.WriteLine(read);
                            break;
                        }
                    }
                    sr.Close();
                }
                sw.Close();

            }
        }

        private void 宽巷FCBsToolStripMenuItem_Click(object sender, EventArgs e) { new Gnsser.Winform.Other.CombinationWL().ShowDialog(); }

        private void 合并共同卫星ToolStripMenuItem_Click(object sender, EventArgs e) { new Gnsser.Winform.Other.CombinationSatNL().ShowDialog(); }

        private void 统计文件大小ToolStripMenuItem_Click(object sender, EventArgs e) { new Gnsser.Winform.Other.FileSize().ShowDialog(); }

        private void 剔除非双频测站ToolStripMenuItem_Click(object sender, EventArgs e) { new Gnsser.Winform.Other.C1P2_Exclude().ShowDialog(); }
        private void 数值格式化ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("数值格式化", typeof(DoubleFormatForm)); }

        private void 矩阵内存测试ToolStripMenuItem_Click(object sender, EventArgs e) { new MatrixMemoForm().Show(); }

        private void 多项式拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] x = new double[20];
            for (int i = 0; i < x.Length; i++) x[i] = i * 1;

            double[] paramArray = new double[] { 0, 1, 2, 3 };
            double[] y = Geo.Algorithm.PolyVal.GetYArray(paramArray, x);

            Geo.Algorithm.LsPolyFit fit = new Geo.Algorithm.LsPolyFit(x, y, 3);
            double[] paralist = fit.FitParameters();
        }

        private void 拉格朗日插值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geo.Algorithm.LagrangeInterplation.Test();
        }

        private void 切比雪夫拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geo.Algorithm.ChebyshevPolyFit.Test();
        }

        private void 参数加权平差ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geo.Algorithm.Adjust.WeightedParamAdjuster.Test();
        }

        private void 分区平差ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool useParalell = false;
            //int core = 4;
            //int blockCount = 8;
            //for (int time = 100; time <= 1000; time += 100)
            //{
            //    Algorithm.Adjust.BlockAdjustment.Test(time, useParalell, core, blockCount);
            //}
            //for (int time = 1000; time < 10000; time += 1000)
            //{
            //    Algorithm.Adjust.BlockAdjustment.Test(time, useParalell, core, blockCount);
            //} 
            //for (int time = 10000; time < 100000; time += 10000)
            //{
            //    Algorithm.Adjust.BlockAdjustment.Test(time, useParalell, core, blockCount);
            //}
            Geo.Algorithm.Adjust.BlockAdjustment.Test(10000, useParalell);
            MessageBox.Show("useParalell =" + useParalell + " 计算完了！呵呵。");
        }

        private void 二进制矩阵读写试验ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.Now;
            int order = 100;
            double[][] matrix = MatrixUtil.CreateRandom(order);
            //double[][] matrix = MatrixUtil.CreateIdentity(order);
            TimeSpan create = DateTime.Now - from;

            from = DateTime.Now;
            string path = @"C:\matrix.bmat";
            MatrixUtil.SaveToBinary(matrix, path);

            //TimeSpan write =  DateTime.Now - from;

            //from = DateTime.Now;
            //double[][] newMatrix = MatrixUtil.FromBinary(path);
            //TimeSpan read = DateTime.Now - from;
            //string msg = ""
            //    + "阶次：" + order + "\r\n"
            //    + "创建：" + create.TotalSeconds +"\r\n"
            //    + "写入：" + write.TotalSeconds + "\r\n"
            //    + "读取：" + read.TotalSeconds + "\r\n";
            //MessageBox.Show(msg
            //    );

            //稀疏矩阵

            Geo.Algorithm.SparseMatrix sM = new Geo.Algorithm.SparseMatrix(matrix);
            path = @"C:\matrix.sbmat";
            sM.ToBinary(path);

            Geo.Algorithm.SparseMatrix s = Geo.Algorithm.SparseMatrix.FromBinary(path);
            double[][] newSmMtirix = s.GetMatrix();
            bool equal = MatrixUtil.IsEqual(newSmMtirix, matrix);


            //MatrixUtil.GetFormatedText(newMatrix);
        }

        private void 文本矩阵读取试验ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.Now;
            int order = 98;
            double[][] matrix = MatrixUtil.CreateRandom(order);
            TimeSpan create = DateTime.Now - from;

            from = DateTime.Now;
            string path = @"C:\matrix.txt";
            MatrixUtil.SaveToText(matrix, path);

            TimeSpan write = DateTime.Now - from;

            from = DateTime.Now;
            double[][] newMatrix = MatrixUtil.ReadFromText(path);
            TimeSpan read = DateTime.Now - from;
            string msg = ""
                + "是否相等：" + MatrixUtil.IsEqual(newMatrix, matrix)
                + "阶次：" + order + "\r\n"
                + "创建：" + create.TotalSeconds + "\r\n"
                + "写入：" + write.TotalSeconds + "\r\n"
                + "读取：" + read.TotalSeconds + "\r\n";
            MessageBox.Show(msg);
        }

        private void kalmanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geo.Algorithm.Adjust.KalmanFilter.Test();
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox("C:\\");
        }
        private void 方阵变换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(@"E:\试验记录\2015.05.17单基线初步试验\Bernese结果\Bernese的DD结果.txt");
            StreamWriter sw = new StreamWriter(@"E:\试验记录\2015.05.17单基线初步试验\Bernese结果\Bernese的DD结果转.txt");

            for (int i = 0; i < 10000; i++)
            {
                string line = sr.ReadLine();
                while (line != null)
                {


                    string[] cha2 = line.Split(' ');

                    string tmp = cha2[0];
                    tmp += '\t';


                    tmp += (cha2[1]);
                    tmp += ('\t');

                    line = sr.ReadLine();
                    cha2 = line.Split(' ');
                    tmp += (cha2[1]);
                    tmp += ('\t');

                    line = sr.ReadLine();
                    cha2 = line.Split(' ');
                    tmp += (cha2[1]);

                    sw.WriteLine(tmp);


                    line = sr.ReadLine();
                }
                if (line == null) break;
            }

            sr.Close();
            sw.Close();


            int rowCol = 4;
            double[][] A = Geo.Utils.MatrixUtil.CreateRandom(rowCol);
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[0].Length; j++)
                {
                    A[i][j] = (i + 1) * 10 + (j + 1);
                }
            }

            File.WriteAllText(@"C:\before.txt", MatrixUtil.GetFormatedText(A));
            MatrixUtil.SymmetricExchange(A, 0, rowCol - 1);

            File.WriteAllText(@"C:\after.txt", MatrixUtil.GetFormatedText(A));

        }
        #endregion

        private void 单点定位向导ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PointPositionVizardForm().ShowDialog();
        }


        #region 信息窗口设置
        private void toolStripButton_close_Click(object sender, EventArgs e)
        {
            logListenerControl1.Height = 25;
        }

        private void 打开消息窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logListenerControl1.Height = this.Height / 5;
        }

        void logListenerControl1_TextChanged(object sender, EventArgs e)
        {
            if (logListenerControl1.Height == 25 && !logListenerControl1.IsFixWindow) { 打开消息窗口ToolStripMenuItem_Click(sender, e); }
        }
        #endregion

        private void 解析GNSSLogger文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Glog文本表格文件(*.txt;*.txt.xls;*.txt;*.xls)|*.glog;*.txt.xls;*.txt;*.xls|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;
                var reader = new ObjectTableManagerReader(path, Encoding.Default);
                reader.IsIntOrFloatFirst = true;
                reader.Spliters = new string[] { ",", "\t" };
                reader.HeaderMarkers = new string[] { "#" };
                var tables = reader.Read();//.GetDataTable();  
                var fileName = System.IO.Path.GetFileName(path);

                foreach (var table in tables)
                {
                    //var form = new Geo.Winform.DataTableViewForm(table) { Text = table.Name };
                    //form.Show();
                    var name = table.Name;
                    OpenMidForm(new TableObjectViewForm(table));
                }
            }
        }

        private void 转换GNSSLoger为RINEX文件CToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Glog文本表格文件(*.txt;*.txt.xls;*.txt;*.xls)|*.glog;*.txt.xls;*.txt;*.xls|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;
                var reader = new ObjectTableManagerReader(path, Encoding.Default);
                reader.IsIntOrFloatFirst = true;
                reader.Spliters = new string[] { "," };
                reader.HeaderMarkers = new string[] { "#" };
                var tables = reader.Read();//.GetDataTable();  
                var fileName = System.IO.Path.GetFileName(path);

                foreach (var table in tables)
                {
                    //var form = new Geo.Winform.DataTableViewForm(table) { Text = table.Name };
                    //form.Show();
                    var name = table.Name;
                    //OpenMidForm(new DataTableViewForm(table) );

                    if (String.Equals(name, "Raw", StringComparison.CurrentCultureIgnoreCase))
                    {
                        AndroidMeasureDecoder docoder = new AndroidMeasureDecoder(table);
                        var opath = Path.Combine(Setting.TempDirectory, Path.GetFileNameWithoutExtension(path) + ".17o");
                        docoder.Run(opath);
                        Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Setting.TempDirectory);
                    }
                }
            }

        }

        private void 窗口转换GNSSLoger为RINEX文件CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMidForm("转换GNSSLoger为RINEX文件", typeof(GLoggerFileConvertForm));
        }

        #region 电离层

        private void 电离层产品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMidForm("电离层产品查看", typeof(IonoViewerForm));
        }

        private void 电离层服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMidForm("电离层服务", typeof(IonoServiceForm));
        }

        private void 电离层卫星服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMidForm("电离层延迟计算", typeof(IonoSatServiceForm));
        }
        private void 站星电离层变化计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMidForm("电离层延迟变化计算", typeof(IonoTrendCaculateForm));
        }
        private void 电离层硬件延迟DToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("电离层硬件延迟", typeof(IonoDcbViewerForm)); }

        private void 电离层DCB计算DToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("电离层DCB计算", typeof(IonoDcbSolveForm)); }

        private void 通用平差器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMidForm("通用平差器", typeof(CommonAdjusterForm));
        }
        #endregion

        private void 版本说明VToolStripMenuItem_Click(object sender, EventArgs e) { FileUtil.OpenFile(Setting.ImprintPath); }

        private void 多项式拟合ToolStripMenuItem_Click_1(object sender, EventArgs e) { OpenMidForm("多项式拟合", typeof(PolyfitForm)); }

        private void dOP绘图DToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("DOP绘图", typeof(DopDrawForm)); }

        private void 观测文件检核OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("啊呀！还没有实现！！！！！！2017.11.06");
        }

        private void 球谐函数SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("球谐函数", typeof(SphericalHarmonicsForm)); }

        private void 配置度盘SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("配置全站仪度盘", typeof(InitConfigAngleTheodoliteForm)); }

        private void 路径替换器RToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("路径替换器", typeof(PathReplaceForm)); }

        private void gNSS计算设置GToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("GNSS选项设置编辑器", typeof(OptionConfigFileEditForm)); }

        private void option文件执行器OToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("GNSSer Opt运行器", typeof(OptionRunnerForm)); }

        private void hTTP访问者HToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void opt网络服务计算WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HttpOptRequesterForm().Show();

        }

        private void 网络服务计算CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HttpUrlPositionerrForm().Show();
        }

        private void 对流层比较ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("对流层比较", typeof(TropProductsCompForm)); }


        private void 星历维护器AToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("星历文件维护器", typeof(Sp3FilemaintainForm)); }

        private void 钟差维护器CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("钟差文件维护器", typeof(ClkFilemaintainForm)); }

        private void 本地IGS产品提取器CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("本地IGS产品提取器", typeof(IgsProducExtractorForm)); }

        private void 电离层球谐函数模型HToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("电离层球谐函数", typeof(IonOfSphericalHarmonicsForm)); }

        private void 伪距平滑精度估算AToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("伪距平滑精度估算", typeof(SmoothRangeAccruceSolverForm)); }

        private void 观测文件拼接CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SingleCombineOFileForm().Show();
        }

        private void 多站观测文件自动拼接MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MultiCombineOFileForm().Show();
        }

        private void 平滑伪距转换SToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("平滑伪距转换", typeof(OFileSmoothRangeFormaterForm)); }

        private void 文本转换为表数据TToolStripMenuItem_Click(object sender, EventArgs e) {   new TextToObjectTableForm().Show();  }

        private void 两表作差DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            { OpenMidForm("两表作差", typeof(DifferOfObjectTableForm)); }
        }

        private void 单位换算CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("单位换算", typeof(UnitConvertForm)); }

        private void 周跳探测与修复CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("钟跳探测与修复", typeof(ClockJumpReviserForm)); }

        private void 观测文件编辑器EToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("观测文件编辑器", typeof(ObsFileEditorForm)); }

        private void 模糊度合成器AToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("模糊度合成器", typeof(AmbiguityCombineerForm)); }
        private void 模糊度文件提取生成BToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("模糊度提取生成", typeof(AmbiguityFileBuilderForm)); }


        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            System.Array array = ((System.Array)e.Data.GetData(DataFormats.FileDrop));//.GetValue(0).ToString();
            if(array == null) { return; }

            List<string> filePaths = new List<string>();
            foreach (object o in array)
            {
                string path = o.ToString();
                if (File.Exists(path))
                    filePaths.Add(path);
            }
            //this.textBox_filepath.Lines = filePaths.ToArray();
        }

        private void 收敛时间计算器CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("收敛时间与精度分析", typeof(ConvergenceTimeCalculatorForm)); }

        private void 同名测站选择器ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("同名测站选择器", typeof(SameSiteSelectForm)); }

        private void 文件提取或移动MToolStripMenuItem_Click(object sender, EventArgs e) { new MoveFileByKeyForm().Show(); }

        private void 共同文件提取器CToolStripMenuItem_Click(object sender, EventArgs e) { new SelectCommonFilesForm().Show(); }

        private void 文件名称提取器NToolStripMenuItem_Click(object sender, EventArgs e) { new FileNameExtractForm().Show(); }

        private void iGS产品提取器EToolStripMenuItem_Click(object sender, EventArgs e) { new IgsProdctExtractorForm().Show(); }

        private void 差分MW数据生成DToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("MW差分产品生成器", typeof(MwDifferTableBuilderForm)); }

        private void 无电离层双差模糊度固定NToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("无电离层双差模糊度固定", typeof(IonoFreeDoubleDifferAmbiFixerForm)); }

        private void 钟差瘦身CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("钟差减肥瘦身", typeof(ClockLoseWeightForm)); }

        private void 基线网闭合差计算器CToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("基线网闭合差计算器", typeof(BaselineNetClosureErrorForm)); }

        private void gNSS网精度查询AToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("GNSS网精度查询", typeof(GnssNetAccuracyForm)); }

        private void 清空临时目录CToolStripMenuItem_Click(object sender, EventArgs e)        {             Setting.CheckOrCleanTempDirectory();        }
        private void 打开临时目录OToolStripMenuItem_Click(object sender, EventArgs e)        {            Geo.Utils.FileUtil.OpenDirectory(Setting.TempDirectory);        }

        private void 基线网平差BToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("基线网平差", typeof(BaselineNetAdjustForm)); }

   
        private void lGO基线文件asc读取RToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("LGO基线文件", typeof(LgoBaselineFileForm)); }

        private void mD5校验ToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("文件MD5校验", typeof(Md5CheckerForm)); }

        private void 时段文件分离器PToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("时段文件分离器", typeof(ObsPeriodDividerForm)); }

        private void 短基线解算面板PToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("同步基线解算", typeof(BaseLineNetSolverForm));  }

        private void 多路径效应MToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("多路径效应分析", typeof(MultiPathAnanasisForm)); }

        private void 多算法同步基线解算MToolStripMenuItem_Click(object sender, EventArgs e){ OpenMidForm("多算法同步基线解算", typeof(AdvancedBaseLineNetSolverForm)); }

        private void 网解同步基线NToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("网解同步基线", typeof(NetSolveBaseLineForm)); }

        private void 查看全站仪平差文件TToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("查看全站仪平差文件", typeof(TotalStaionAdjustFileForm)); }

        private void 高斯坐标转换GToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("高斯坐标转换", typeof(GaussLonlatConvertForm)); }

        private void 平面坐标转换PToolStripMenuItem_Click(object sender, EventArgs e){ OpenMidForm("平面坐标转换", typeof(PlanXyConvertParamEstimatForm)); }

        private void 平面坐标向量查看PToolStripMenuItem_Click(object sender, EventArgs e){ OpenMidForm("平面坐标向量查看", typeof(PlanXyLineViewerForm)); }

        private void 多时段基线解算MToolStripMenuItem_Click(object sender, EventArgs e){ OpenMidForm("多时段基线解算", typeof(MultiPeriodBaseLineSolverForm)); }

        private void xYZ转高斯XY平差PToolStripMenuItem_Click(object sender, EventArgs e)  {   OpenMidForm("XYZ平差到高斯XY", typeof(XyzToGausssXyForm));  }

        private void 单站集成计算SToolStripMenuItem_Click(object sender, EventArgs e) {   OpenMidForm("多时段非差计算", typeof(SingleSitePointPositionSolverForm));  }

        private void xYZ参考框架转换CToolStripMenuItem_Click(object sender, EventArgs e)  {   OpenMidForm("XYZ参考框架转换", typeof(XyzFrameConvertParamEstimatForm));  }

        private void xYZ参考框架文本转换FToolStripMenuItem_Click(object sender, EventArgs e) {   OpenMidForm("XYZ参考框架文本转换", typeof(XyzFrameFileConvertForm));  }

        private void 矩阵文件查看MToolStripMenuItem_Click(object sender, EventArgs e) {   OpenMidForm("矩阵文件查看", typeof(MatrixViewerForm)); }
        private void 矩阵方程EToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("矩阵方程", typeof(MatrixEquationForm)); }

        private void 矩阵方程叠加OToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("矩阵方程叠加", typeof(MatrixEquationComposerForm)); }

        private void 无电离层双差模糊度固定AToolStripMenuItem_Click(object sender, EventArgs e) { OpenMidForm("无电离层模糊度固定", typeof(IonoFreeAmbiguityFixserForm)); }

        private void 多矩阵方程计算MToolStripMenuItem_Click(object sender, EventArgs e){ OpenMidForm("多矩阵方程计算", typeof(MultiMatrixEquationCalculateForm)); }

        private void 大地方位角解算NToolStripMenuItem_Click(object sender, EventArgs e){ OpenMidForm("大地方位角解算", typeof(GeodeticAzimuthForm)); }

        private void 计算讲课费FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new LectureFeeForm().Show();
        }

        private void gIF图制作ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GifMakerForm().Show(); 
        }
    }
}