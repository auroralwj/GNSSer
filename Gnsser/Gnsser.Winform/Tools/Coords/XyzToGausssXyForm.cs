//2019.01.16, czs, 三维坐标转高斯平面坐标平差

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
    public partial class XyzToGausssXyForm : Form
    {
        Log log = new Log(typeof(XyzToGausssXyForm));
        public XyzToGausssXyForm()
        {
            InitializeComponent();
        }

        private void XyzToGausssXyForm_Load(object sender, EventArgs e)
        {
            enumRadioControl_angleUnit.Init<AngleUnit>();
            enumRadioControl_angleUnit.SetCurrent<AngleUnit>(AngleUnit.Degree);

            //  KnownCoordPath = Setting.GnsserConfig.SiteCoordFile;
            this.fileOpenControl_gaussPlainXy.Filter = Setting.CoordFileFilter;
            this.fileOpenControl_xyz.Filter = Setting.CoordFileFilter;
        }
        string KnownGaussXyCoordPath { get => this.fileOpenControl_gaussPlainXy.FilePath; set => this.fileOpenControl_gaussPlainXy.FilePath = value; }
        AngleUnit AngleUnit => this.enumRadioControl_angleUnit.GetCurrent<AngleUnit>();
        Ellipsoid Ellipsoid => this.ellipsoidSelectControl1.Ellipsoid;
        bool IsWithBeltNum => this.checkBox_isWithBeltNum.Checked;
        int BeltWidth3Or6 => checkBox_is3Belt.Checked ? 3 : 6;
        double OrinalLonDeg => this.namedFloatControl_orinalLonDeg.GetValue();
        double AveGeoHeight => this.namedFloatControl_aveGeoHeight.GetValue();
        bool IsIndicateOriginLon => checkBox_indicated.Checked;
        /// <summary>
        /// Y加常数
        /// </summary>
        double YConst => namedFloatControlYConst.GetValue(); 

        private void button_run_Click(object sender, EventArgs e)
        {
            var inputXyzPath = this.fileOpenControl_xyz.FilePath;
            var knownGaussXyPath = this.fileOpenControl_gaussPlainXy.FilePath;

            //var inputXyz = TableObjectReader.Read(inputXyzPath);
            //var knownGaussXy = TableObjectReader.Read(knownGaussXyPath);

            var KnownGaussXyCoords = new SiteCoordService(new FileOption(this.KnownGaussXyCoordPath));
            KnownGaussXyCoords.Init();
            log.Info("载入了坐标文件 " + Geo.Utils.StringUtil.ToString(KnownGaussXyCoordPath));

            var inputXyz = new SiteCoordService(new FileOption(inputXyzPath));
            inputXyz.Init();
            log.Info("载入了坐标文件 " + Geo.Utils.StringUtil.ToString(inputXyzPath));

            var table = NamedRmsXyz.BuildTable(inputXyz.NamedRmsXyzes.Values, "待转换三维坐标");
            this.objectTableControl_inputXyz.DataBind(table);
            var table2 = NamedRmsXyz.BuildTable(KnownGaussXyCoords.NamedRmsXyzes.Values, "已知高斯坐标");
            this.objectTableControl_knownGaussXy.DataBind(table2); 

            //首先，将空间直角坐标转换为大地坐标
            var ellipsoid = Ellipsoid;
            Dictionary<string, GeoCoord> geoCoords = new Dictionary<string, GeoCoord>();
            foreach (var item in inputXyz.NamedRmsXyzes)
            {
                var name = item.Name;
                geoCoords[name] = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(item.Value.Value, ellipsoid, AngleUnit.Degree);
            }

            //转换为高斯坐标
            var convertedXy = new List<NamedXy>();
            double orinalLonDeg = OrinalLonDeg;
            foreach (var item in geoCoords)
            {
                var geoCoord = item.Value;
                var xy = Geo.Coordinates.CoordTransformer.LonLatToGaussXy(geoCoord, AveGeoHeight, BeltWidth3Or6, ref orinalLonDeg, IsIndicateOriginLon, Ellipsoid, YConst, AngleUnit, IsWithBeltNum);
                convertedXy.Add(new NamedXy(item.Key, xy));
            }
            this.namedFloatControl_orinalLonDeg.SetValue(orinalLonDeg);
            //显示
            ObjectTableStorage convertedXyTable = NamedXy.BuildTable(convertedXy,"转换后的高斯坐标");
            objectTableControl1_convertGsussXy.DataBind(convertedXyTable);


            //估计平面坐标转换的平差参数。旧转新，查找公共测站
            var commonOlds = new List<NamedXy>();
            var commonNews = new List<NamedXy>();
            foreach (var item in convertedXy)
            {
                var known = KnownGaussXyCoords.Get(item.Name);
                if (known == null) { continue; }
                commonOlds.Add(new NamedXy(item.Name, item.Value));
                commonNews.Add(new NamedXy(item.Name, known.Value));
            }
            var planXyConvertParamEstimator = new PlainXyConvertParamEstimator(commonOlds, commonNews);
            var param = planXyConvertParamEstimator.Estimate();
            this.plainXyTransParamControl1.SetValue(param);

            //查看平差结果 
            log.Info(planXyConvertParamEstimator.ResultMatrix.ToReadableText());


            //计算转换结果
            var convertParam = this.plainXyTransParamControl1.GetValue();

            PlanXyConverter planXyConverter = new PlanXyConverter(convertParam);
            var result = planXyConverter.Convert(convertedXy);

            ObjectTableStorage resultTable = NamedXy.BuildTable(result, "结果高斯坐标");
            this.objectTableControl_resultXy.DataBind(resultTable);

            var path = System.IO.Path.Combine(this.OutputDirectory, "结果高斯坐标" +  Geo.Times.Time.Now.ToDateAndHourMinitePathString() + Setting.SiteCoordFileExtension);
            ObjectTableWriter.Write(resultTable,path);
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(OutputDirectory);


            //平差后高斯坐标比较结果 
            List<NamedXy> compareXys = new List<NamedXy>();
            foreach (var item in result)
            {
                var known = KnownGaussXyCoords.Get(item.Name);
                if (known == null) { continue; }
                var differ = item.Value - known.Value;
                compareXys.Add(new NamedXy(item.Name, differ));
            }
            ObjectTableStorage compareTable = NamedXy.BuildTable(compareXys, "平差后高斯坐标比较结果");
            objectTableControl1_adjustCompare.DataBind(compareTable);

            //平差前高斯坐标比较结果 
            List<NamedXy> compareBeforceXys = new List<NamedXy>();
            foreach (var item in convertedXy)
            {
                var known = KnownGaussXyCoords.Get(item.Name);
                if (known == null) { continue; }
                var differ = item.Value - known.Value;
                compareBeforceXys.Add(new NamedXy(item.Name, differ));
            }
            ObjectTableStorage compareBeforceXysTable = NamedXy.BuildTable(compareBeforceXys, "平差前高斯坐标比较结果");
            objectTableControl_compareBeforce.DataBind(compareBeforceXysTable);
            
        }

        public string OutputDirectory => this.directorySelectionControl1.Path;
    }

}
