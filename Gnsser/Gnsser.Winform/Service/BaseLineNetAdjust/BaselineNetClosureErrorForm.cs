//2018.11.28, czs, create in hmx, 基线网闭合差计算器
//2018.12.01, czs, edit in hmx, 增加多时段数据
//2018.12.04, czs, edit in hmx, 增加地图显示
//2019.01.12, czs, edit in hmx, 增加最弱文本显示

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
using AnyInfo.Geometries;
using Gnsser.Data;

namespace Gnsser.Winform
{
    public partial class BaselineNetClosureErrorForm : Form, IShowLayer
    {
        Log log = new Log(typeof(BaselineNetClosureErrorForm));
        public event ShowLayerHandler ShowLayer;
        public BaselineNetClosureErrorForm()
        {
            InitializeComponent();
        }
        string[] InputBaselinePathes => this.fileOpenControl_input.FilePathes;
        double periodSpanMinutes => this.namedFloatControl_periodSpanMinutes.GetValue();
        /// <summary>
        /// 同步环闭合差信息
        /// </summary>
        Dictionary<BufferedTimePeriod, TriguilarNetQualitiyManager> AllSychTrilateralQualities { get; set; }
        /// <summary>
        /// 复测较差超限基线
        /// </summary>
        Dictionary<BufferedTimePeriod, RepeatErrorQualityManager> PeriodsRepeatErrorslQualities { get; set; }

        BaseLineNetManager BaseLineNets { get; set; }

        private void button_run_Click(object sender, EventArgs e)
        {
            //手动输入
            double levelFixed = this.namedFloatControl_fixedErrorLevel.GetValue();
            double verticalFixed = this.namedFloatControl_fixedErrorVertical.GetValue();
            double levelCoeef = this.namedFloatControl_levelCoefOfProprotion.GetValue();
            double verticalCoeef = this.namedFloatControl_verticalCoefOfProprotion.GetValue();
            var GnssReveiverNominalAccuracy = new GnssReveiverNominalAccuracy(levelFixed, verticalFixed, levelCoeef, verticalCoeef);

            var periodSpanMinutes = this.namedFloatControl_periodSpanMinutes.GetValue();

            var path = this.fileOpenControl_input.FilePath;
            if (!File.Exists(path)) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！在下无能为力！"); return; }

            var pathes = this.fileOpenControl_input.FilePathes;

            var tables = ObjectTableManager.Read(pathes, ".");
            //合并所有的表格
            var rawTable = tables.Combine();
              BaseLineNets = BaseLineNetManager.Parse(rawTable, periodSpanMinutes);//时段网

            this.AllSychTrilateralQualities = BaseLineNets.BuildTriangularClosureQualies(GnssReveiverNominalAccuracy); 

            //同步环闭合差计算          
            ObjectTableStorage totalSyncErrorTable = BaseLineNets.BuildSyncTrilateralErrorTable(AllSychTrilateralQualities);


            
            //复测基线较差,所有与第一个作差
            var PeriodsRepeatErrors = BaseLineNets.BuildRepeatBaselineError();
            var worst = PeriodsRepeatErrors.GetWorst();
            if (worst != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("复测基线较差最弱边：" + worst.ToString());
                this.richTextBoxControl_textResult.Text += "\r\n" + sb.ToString();
            }

            this.PeriodsRepeatErrorslQualities = BaseLineNets.BuildRepeatBaselingQulities(PeriodsRepeatErrors, GnssReveiverNominalAccuracy);
            //生成所有表格
            ObjectTableStorage repeatErrorTable = BaseLineNets.BuildRepeatBaselingErrorTable(PeriodsRepeatErrorslQualities, GnssReveiverNominalAccuracy); 
             
            this.objectTableControl_syncclosureError.DataBind(totalSyncErrorTable);
            this.objectTableControl_closureErrorOfRepeatBaseline.DataBind(repeatErrorTable);
            this.objectTableControl_rawData.DataBind(rawTable);

            this.tabControl_res.SelectedTab = this.tabPage_allSync;
        }



        private void BaselineNetClosureErrorForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_input.Filter = Setting.BaseLineFileFilter;
            if (!Directory.Exists(Setting.TempDirectory)) { return; }
            var files = Directory.GetFiles(Setting.TempDirectory, "*" + Setting.BaseLineFileExtension);
            if (files != null && files.Length > 0)
            {
                this.fileOpenControl_input.FilePathes = files;
            }

            objectTableControl_syncclosureError.ShowInfo("按照GB/T 18314-2009，三边同步环闭合差应满足：Wx<=√3 /5 σ ");
            this.objectTableControl_closureErrorOfRepeatBaseline.ShowInfo("按照GB/T 18314-2009，B、C级复测基线长度较差应满足：Wx<= 2 √2 σ ");
        }


        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl_input.FilePath;
            if (!File.Exists(path)) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！在下无能为力！"); return; }

            if (BaseLineNets != null)
            {
                BaseLineNets = LoadBaseLineNets();
            }

            if (ShowLayer != null && BaseLineNets != null)
            {
                List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();
                int netIndex = 0;
                List<string> addedNames = new List<string>();
                foreach (var kv in BaseLineNets.KeyValues)
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
                if (pts.Count == 0) { return; }
                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreatePointLayer(pts);
                ShowLayer(layer);
            }
        }

        private BaseLineNetManager LoadBaseLineNets()
        {
            var tables = ObjectTableManager.Read(InputBaselinePathes, ".");
            var table = tables.Combine();       //合并所有的表格
            var BaseLineNet = BaseLineNetManager.Parse(table, periodSpanMinutes);//时段网
            return BaseLineNet;
        }

        private void button_showLines_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl_input.FilePath;
            if (!File.Exists(path)) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！在下无能为力！"); return; }

            if (BaseLineNets != null)
            {
                BaseLineNets = LoadBaseLineNets();
            }

            if (ShowLayer != null && BaseLineNets != null)
            { 
                int netIndex = 0;
                List<LineString> lineStrings = new List<LineString>();
                foreach (var kv in BaseLineNets.KeyValues)
                {
                    foreach (var line in kv.Value)
                    {
                        LineString lineString = BuildLineString(netIndex, line);
                        lineStrings.Add(lineString);
                    }
                    netIndex++;
                }
                if(lineStrings.Count == 0) { return; }

                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "基线");
                ShowLayer(layer);
            }
        }

        private void button_showBadLines_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl_input.FilePath;
            if (!File.Exists(path)) { Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！在下无能为力！"); return; }
     
            if (AllSychTrilateralQualities == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先计算后再试！"); return;
            }
            if (BaseLineNets != null)
            {
                BaseLineNets = LoadBaseLineNets();
            }
             
            if (ShowLayer != null && AllSychTrilateralQualities != null)
            {
                int netIndex = 0;
                List<LineString> lineStrings = new List<LineString>();
                foreach (var kv in AllSychTrilateralQualities)
                {
                    foreach (var line in kv.Value.KeyValues)
                    {
                        var lineName = line.Key;
                        bool isbad = false;
                        var item = line.Value;
                        {
                            if (!item.IsAllOk)
                            {
                                isbad = true;
                                break;
                            }
                        }

                        if (!isbad)
                        {
                            continue;
                        }

                        LineString lineString = BuildLineString(netIndex, lineName.GetBaseLineNames()[0]);


                        lineStrings.Add(lineString);
                    }
                    netIndex++;
                }
                if (lineStrings.Count == 0) { return; }

                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreateLineStringLayer(lineStrings, "基线", Color.OrangeRed, 5);
                ShowLayer(layer);
            }
        }
         
        private LineString BuildLineString(int netIndex, GnssBaseLineName lineName)
        {
            var baseLine = BaseLineNets.GetBaseLineOrReversed(lineName);
            return BuildLineString(netIndex, baseLine);
        }

        private  LineString BuildLineString(int netIndex, EstimatedBaseline baseLine)
        {
            var name = netIndex + "-" + baseLine.BaseLineName.RovName;
            var ptA = new AnyInfo.Geometries.Point(baseLine.EstimatedGeoCoordOfRov, null, name);
            name = netIndex + "-" + baseLine.BaseLineName.RefName;
            var geoCoord = CoordTransformer.XyzToGeoCoord(baseLine.ApproxXyzOfRef);
            var ptB = new AnyInfo.Geometries.Point(geoCoord, null, name);
            var lineString = new LineString(new List<AnyInfo.Geometries.Point>()
                        {
                            ptA, ptB
                        }, netIndex + "-" + baseLine.Name);
            return lineString;
        }

        private void button_exportLgoasc_Click(object sender, EventArgs e)
        {
            if(BaseLineNets == null || BaseLineNets.Count == 0) { return; }
            BaseLineFileConverter converter = new BaseLineFileConverter();
            int i = 0;
            foreach (var item in BaseLineNets)
            {
                var outnet = converter.Build(item);

                var outpath = Path.Combine(Setting.TempDirectory, i +"-" + outnet.BaseLines.First().Key.Name + "_etc" + outnet.BaseLines.Count + Setting.BaseLineFileOfLgoExtension);
                LgoAscBaseLineFileWriter writer = new LgoAscBaseLineFileWriter(outpath);
                writer.Write(outnet);
                i++;
            }

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(Setting.TempDirectory);
        }
    }
}
