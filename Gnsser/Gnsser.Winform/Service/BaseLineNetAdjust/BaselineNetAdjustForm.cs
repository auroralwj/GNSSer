//2018.12.02, czs, create in hmx, 增加多时段数据

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Geo.IO;
using System.IO;
using Geo.Coordinates;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using AnyInfo.Geometries;

namespace Gnsser.Winform
{
    public partial class BaselineNetAdjustForm : Form, IShowLayer
    {
        Log log = new Log(typeof(BaselineNetAdjustForm));

        public event ShowLayerHandler ShowLayer;

        public BaselineNetAdjustForm()
        {
            isInited = false;
            InitializeComponent();
            this.enumRadioControl_adjustType.Init<BaselineNetAdjustType>();
            this.enumRadioControl_selectLineType.Init<IndependentLineSelectType>();
            KnownCoordPath = Setting.GnsserConfig.SiteCoordFile;
            this.fileOpenControl_knownCoord.Filter = Setting.CoordFileFilter;
        }
        /// <summary>
        /// 所有基线表，原始表
        /// </summary>
        ObjectTableStorage TotalLineTable { get; set; }
        /// <summary>
        /// 所有原始基线的管理器
        /// </summary>
        BaseLineNetManager BaseLineNetManager { get; set; }
        /// <summary>
        ///用于平差的基线。
        /// </summary>
        BaseLineNetManager independentLineNet { get; set; }
        BaselineNetAdjustType AdjustType => this.enumRadioControl_adjustType.GetCurrent<BaselineNetAdjustType>();
        string[] BaseLinePathes => this.fileOpenControl_baseline.FilePathes;
        string KnownCoordPath { get=> this.fileOpenControl_knownCoord.FilePath;  set => this.fileOpenControl_knownCoord.FilePath = value; }
        IndependentLineSelectType IndependentLineSelectType => this.enumRadioControl_selectLineType.GetCurrent<IndependentLineSelectType>();
        //是否选择独立基线
        bool IsSelectIndependentLine => checkBox_isAllLinIndependentOre.Checked;
        SiteCoordService SiteCoordService { get; set; }
        string OutputDirectory => this.directorySelectionControl1.Path;
        private void button_run_Click(object sender, EventArgs e)
        {
            if (BaseLinePathes.Length == 0 || !File.Exists(BaseLinePathes[0])) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！在下无能为力！"); return; }
 
            //再次加载
            LoadFiles(); 

            var fixedSites = arrayCheckBoxControl_fixedSites.GetSelected<string>();
            this.independentLineNet = BaseLineNetManager;
            if (fixedSites.Count !=0 && BaseLineNetManager.Contains(fixedSites[0]))
            {
                LoadAndInitSites();
                log.Info("重新更新了已知站点。");
            }
            if (IsSelectIndependentLine)   //注意这里返回的是不同时段的独立基线集合
            { 
                this.independentLineNet = BaseLineNetManager.GetIndependentLines(IndependentLineSelectType);
            } 

            var siteNames = BaseLineNetManager.GetSiteNames();

            BaseAdjustMatrixBuilder matrixBuilder = BuildAdjustMatrix(independentLineNet.GetBaseLines(), siteNames, fixedSites);

            ParamAdjuster paramAdjuster = new ParamAdjuster();

            var adustResult = paramAdjuster.Run(new AdjustObsMatrix(matrixBuilder));
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("StdDev : " + adustResult.StdDev.ToString("G6"));
           // sb.AppendLine("RMS : " + adustResult.GetRmsedTableRow );
            log.Fatal(sb.ToString());
            BaselineNetResult result = new BaselineNetResult(BaseLineNetManager, siteNames, adustResult);

            ObjectTableStorage resultTable = result.BuildObjectTable();

            //log.Info(adustResult.ToReadableText());

            this.objectTableControl_netAdjustResult.DataBind(resultTable);
            objectTableControl_independentLine.DataBind(independentLineNet.GetPhaseTable());

            //基线改正表对象
            var vectorCorrectionTable = result.GetVectorCorrectionTable();
            objectTableControl_vectorCorrection.DataBind(vectorCorrectionTable);

            //显示
            //tabControl_result.SelectedTab = this.tabPage_netAdjustResult;


            //写入输出
            var path = Path.Combine(OutputDirectory, "网平差结果" + BaseLinePathes.Length + "Files" + Setting.SiteCoordFileExtension);
            ObjectTableWriter.Write(resultTable, path);

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(path),"执行完毕，是否打开所在目录？" + path);
        }

        private BaseAdjustMatrixBuilder BuildAdjustMatrix(List<EstimatedBaseline> independentLineNet, 
            List<string> totalSites, List<string> fixedSites)
        {
            BaseAdjustMatrixBuilder matrixBuilder = null;
            switch (AdjustType)
            {
                case BaselineNetAdjustType.秩亏自由网平差:
                    matrixBuilder = new FreeRankDeficiencyBaselineNetMatrixBuilder(independentLineNet, totalSites, fixedSites);
                    break;
                case BaselineNetAdjustType.固定基准站约束平差:
                    matrixBuilder = new SiteFixedBaselineNetMatrixBuilder(independentLineNet, totalSites, fixedSites);
                    break;                  
                default:
                    matrixBuilder = new FreeRankDeficiencyBaselineNetMatrixBuilder(independentLineNet, totalSites, fixedSites);
                    break;
            }
            matrixBuilder.Build();
            return matrixBuilder;
        }

        bool isInited = false;

        private void BaselineNetClosureErrorForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_baseline.Filter = Setting.BaseLineFileFilter;
            if (!Directory.Exists(Setting.TempDirectory)) { return; }
            var files = Directory.GetFiles(Setting.TempDirectory, "*" + Setting.BaseLineFileExtension);
            if (files != null && files.Length > 0)
            {
                this.fileOpenControl_baseline.FilePathes = files;
            }
            enumRadioControl_adjustType_EnumItemSelected("",true);
            checkBox_isAllLinIndependentOre_CheckedChanged(sender, e);
            isInited = true;
        }

        private void enumRadioControl_adjustType_EnumItemSelected(string arg1, bool arg2)
        {
            arrayCheckBoxControl_fixedSites.Visible = (AdjustType == BaselineNetAdjustType.固定基准站约束平差);
            button_loadSItes.Visible = arrayCheckBoxControl_fixedSites.Visible;
        }         

        private void button_loadSites_Click(object sender, EventArgs e)
        {
            LoadAndInitSites();
        }

        private void InitKownSites()
        {
            List<string> knownSites = new List<string>();
            var siteNames = BaseLineNetManager.GetSiteNames();

            if (SiteCoordService != null)
            {
                foreach (var name in siteNames)
                {
                    var coord = SiteCoordService.Get(name);
                    if (coord != null)
                    {
                        knownSites.Add(name);
                        this.BaseLineNetManager.SetSiteCoord(name, coord.Value);
                       log.Info("基线网设置了已知坐标 " + name + " " + coord.Value);
                    }

                }
            }
            if (knownSites.Count == 0)
            {
                log.Warn("在已知坐标文件中，没有找到测站名称，默认以头文件坐标替代，请手动选择！");
                knownSites = siteNames;
            }
            arrayCheckBoxControl_fixedSites.Init<string>(knownSites);
        }

        private void LoadAndInitSites()
        {
            try
            {
                if (BaseLinePathes.Length == 0 || !File.Exists(BaseLinePathes[0])) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！在下无能为力！"); return; }

                LoadFiles();
                InitKownSites();
            }catch(Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox( "解析失败！ " + ex.Message);
            }
        }

        private void LoadFiles()
        {
            var periodSpanMinutes = namedFloatControl_periodSpanMinutes.GetValue();
            if(BaseLinePathes.Length == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请设置所有文件后再试！"); return; }

            var tables = ObjectTableManager.Read(BaseLinePathes, ".");
            log.Info("载入了基线文件 " + Geo.Utils.StringUtil.ToString(BaseLinePathes));
            //合并所有的表格
            TotalLineTable = tables.Combine();
            BaseLineNetManager = BaseLineNetManager.Parse(TotalLineTable, periodSpanMinutes);//时段网 
            BaseLineNetManager.Init();

            if (File.Exists(KnownCoordPath))
            {
                SiteCoordService = new SiteCoordService(new FileOption(this.KnownCoordPath));
                log.Info("载入了坐标文件 " + Geo.Utils.StringUtil.ToString(KnownCoordPath));
            }


            this.objectTableControl_rawData.DataBind(TotalLineTable);
        }
         

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && BaseLineNetManager != null)
            {
                List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();
                int netIndex = 0;
                List<string> addedNames = new List<string>();
                foreach (var kv in BaseLineNetManager.KeyValues)
                {
                    foreach (var line in kv.Value)
                    {
                        var name = netIndex + "-" + line.BaseLineName.RovName;
                        if (!addedNames.Contains(name))
                        {
                            pts.Add(new AnyInfo.Geometries.Point(line.EstimatedGeoCoordOfRov, null, name));
                            addedNames.Add(name);
                        }
                        name = netIndex + "-" + line.BaseLineName.RefName;
                        if (!addedNames.Contains(name))
                        {
                            var geoCoord = CoordTransformer.XyzToGeoCoord(line.ApproxXyzOfRef);
                            pts.Add(new AnyInfo.Geometries.Point(geoCoord, null, name));
                            addedNames.Add(name);
                        }
                    }
                    netIndex++;
                }
                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreatePointLayer(pts);
                ShowLayer(layer);
            }
        }

        private void button_showIndependentLine_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && independentLineNet != null)
            {
                int netIndex = 0;
                List<LineString> lineStrings = new List<LineString>();
                foreach (var kv in independentLineNet.KeyValues)
                {
                    foreach (var line in kv.Value)
                    {
                        var name = netIndex + "-" + line.BaseLineName.RovName;
                        var ptA = new AnyInfo.Geometries.Point(line.EstimatedGeoCoordOfRov, null, name);

                        name = netIndex + "-" + line.BaseLineName.RefName;
                        var geoCoord = CoordTransformer.XyzToGeoCoord(line.ApproxXyzOfRef);
                        var ptB = new AnyInfo.Geometries.Point(geoCoord, null, name);

                        var lineString = new LineString(new List<AnyInfo.Geometries.Point>()
                        {
                            ptA, ptB
                        }, netIndex + "-" + line.Name);
                        lineStrings.Add(lineString);
                    }
                    netIndex++;
                }
                if (lineStrings.Count == 0) { return; }

                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "基线");
                ShowLayer(layer);
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            this.LoadFiles();
        }

        private void checkBox_isAllLinIndependentOre_CheckedChanged(object sender, EventArgs e)
        {
            this.enumRadioControl_selectLineType.Visible = checkBox_isAllLinIndependentOre.Checked;
        }

        private void fileOpenControl_baseline_FilePathSetted(object sender, EventArgs e)
        {
            if (isInited)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("载入了新文件，是否立即读取？") == DialogResult.Yes)
                {
                    this.LoadFiles();
                }
                var dir = Path.GetDirectoryName(BaseLinePathes[0]);
                if (!String.Equals(dir, this.OutputDirectory, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否将输出目录设置为 " + dir + "？") == DialogResult.Yes)
                    {
                        this.directorySelectionControl1.Path = dir;
                    }
                }
            }
        }
    }
}
