//2019.01.19, czs, 三维坐标转XYZ坐标平差

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using Geo.Coordinates;
using Geo.Referencing;
using Geo;
using Geo.IO; 

namespace Gnsser.Winform
{
    public partial class XyzFrameFileConvertForm : Form, IShowLayer
    {
        Log log = new Log(typeof(XyzFrameFileConvertForm));

        public event ShowLayerHandler ShowLayer;

        public XyzFrameFileConvertForm()
        {
            InitializeComponent();
        }

        private void XyzToGausssXyForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_knownCoord.Filter = Setting.CoordFileFilter;
            this.fileOpenControl_xyz.Filter = Setting.CoordFileFilter;
            this.KnownCoordPath = Setting.GnsserConfig.SiteCoordFile;
        }
        string KnownCoordPath { get => this.fileOpenControl_knownCoord.FilePath; set => this.fileOpenControl_knownCoord.FilePath = value; }
        string InputCoordPath { get => this.fileOpenControl_xyz.FilePath; set => this.fileOpenControl_xyz.FilePath = value; }

        /// <summary>
        /// 是否估计七参数
        /// </summary>
        bool IsEstParams => checkBox1_est7params.Checked;
        /// <summary>
        /// 结果输出目录
        /// </summary>
        public string OutputDirectory => this.directorySelectionControl1.Path;

        private void button_run_Click(object sender, EventArgs e)
        { 
            var inputXyz = new SiteCoordService(new FileOption(InputCoordPath));
            inputXyz.Init();
            log.Info("载入了坐标文件 " + Geo.Utils.StringUtil.ToString(InputCoordPath));

            var table = NamedRmsXyz.BuildTable(inputXyz.NamedRmsXyzes.Values, "待转换三维坐标");
            this.objectTableControl_inputXyz.DataBind(table);

            var allToBeConvert = inputXyz.NamedXyzs;


            //估计 坐标转换的平差参数。旧转新，查找公共测站
            SiteCoordService KnownGaussXyCoords = null;
            if (IsEstParams)
            {
                KnownGaussXyCoords = new SiteCoordService(new FileOption(this.KnownCoordPath));
                KnownGaussXyCoords.Init();
                log.Info("载入了坐标文件 " + Geo.Utils.StringUtil.ToString(KnownCoordPath));
                var table2 = NamedRmsXyz.BuildTable(KnownGaussXyCoords.NamedRmsXyzes.Values, "已知坐标");
                this.objectTableControl_knownCoord.DataBind(table2); 


                log.Info("参数估计");
                EstConvertParams(KnownGaussXyCoords, allToBeConvert);
            }

            //计算转换结果
            var convertParam = this.xyzTransParamControl1.GetValue();

            var planXyConverter = new XyzFrameConverter(convertParam);
            var result = planXyConverter.Convert(allToBeConvert);

            ShowAndOurputResult(result);

            //转换前后残差
            var differ = NamedXyz.Differ(result, allToBeConvert);
            objectTableControl_residuals.DataBind(NamedXyz.BuildTable(differ, "转换前后残差"));


            //平差前与已知坐标比较
            var compareBeforceXysTable = BuildAndSetOrinalCoordCompareTable(KnownGaussXyCoords, allToBeConvert, "平差前与已知坐标比较");
            objectTableControl_compareBeforce.DataBind(compareBeforceXysTable);

            //平差后坐标比较结果  
            var compareTable = BuildAndSetOrinalCoordCompareTable(KnownGaussXyCoords, result, "平差后与已知坐标比较");
            objectTableControl1_adjustCompare.DataBind(compareTable);

        }

        private void ShowAndOurputResult(List<NamedXyz> result)
        {
            ObjectTableStorage resultTable = NamedXyz.BuildTable(result, "结果坐标");
            this.objectTableControl_resultXy.DataBind(resultTable);

            var path = System.IO.Path.Combine(this.OutputDirectory, "XYZ转换结果坐标" + Geo.Times.Time.Now.ToDateAndHourMinitePathString() + Setting.SiteCoordFileExtension);
            ObjectTableWriter.Write(resultTable, path);
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(OutputDirectory);
        }


        //平差前坐标比较结果 
        private ObjectTableStorage BuildAndSetOrinalCoordCompareTable(SiteCoordService KnownGaussXyCoords, List<NamedXyz> allToBeConvert, string name )
        {
            if (KnownGaussXyCoords == null) { return new ObjectTableStorage(); }

            var compareBeforceXys = new List<NamedXyz>();
            foreach (var item in allToBeConvert)
            {
                var known = KnownGaussXyCoords.Get(item.Name);
                if (known == null) { continue; }
                var differ = item.Value - known.Value;
                compareBeforceXys.Add(new NamedXyz(item.Name, differ));
            }
            ObjectTableStorage compareBeforceXysTable = NamedXyz.BuildTable(compareBeforceXys, name);
            return compareBeforceXysTable;
        }

        private void EstConvertParams(SiteCoordService KnownGaussXyCoords, List<NamedXyz> allToBeConvert)
        {
            if(KnownGaussXyCoords == null) { return; }

            var commonOlds = new List<NamedXyz>();
            var commonNews = new List<NamedXyz>();
            foreach (var item in allToBeConvert)
            {
                var known = KnownGaussXyCoords.Get(item.Name);
                if (known == null) { continue; }
                commonOlds.Add(item);
                commonNews.Add(new NamedXyz(item.Name, known.Value));
            }
            var planXyConvertParamEstimator = new XyzFrameConvertParamEstimator(commonOlds, commonNews);
            var param = planXyConvertParamEstimator.Estimate();
            this.xyzTransParamControl1.SetValue(param);


            this.objectTableControl_commonCoordToBeConvert.DataBind(NamedXyz.BuildTable(commonOlds, "待转公共坐标"));
            this.objectTableControl_knownCommonCoord.DataBind(NamedXyz.BuildTable(commonNews, "已知公共坐标"));
            //查看平差结果 
            //      log.Info(planXyConvertParamEstimator.ResultMatrix.ToReadableText());
        }

        private void checkBox1_est7params_CheckedChanged(object sender, EventArgs e)
        {
            fileOpenControl_knownCoord.Enabled = checkBox1_est7params.Checked;
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(InputCoordPath))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请载入数据后再试！"); return;
            }
            var inputXyz = new SiteCoordService(new FileOption(InputCoordPath));
            inputXyz.Init();

            List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();

            foreach (var item in inputXyz.NamedXyzs)
            {
                var xyz = item.Value;
                var geoCoord = CoordTransformer.XyzToGeoCoord(xyz);

                pts.Add(new AnyInfo.Geometries.Point(geoCoord, null, item.Name));
            }

            if (pts.Count == 0) { return; }
            AnyInfo.Layer layer = AnyInfo.LayerFactory.CreatePointLayer(pts);
            ShowLayer(layer);
        }
    }

}
