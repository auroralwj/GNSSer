//2019.01.09, czs, create in hmx, 全站仪器结果

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
using Gnsser.Data;

namespace Gnsser.Winform
{
    public partial class TotalStaionAdjustFileForm : Form, IShowLayer
    {
        Log log = new Log(typeof(TotalStaionAdjustFileForm));

        public event ShowLayerHandler ShowLayer;

        public TotalStaionAdjustFileForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Y加常数
        /// </summary>
        double YConst => namedFloatControlYConst.GetValue();

        double OrinalLonDeg => this.namedFloatControl_orinalLonDeg.GetValue();

        double AveGeoHeight => this.namedFloatControl_aveGeoHeight.GetValue();
        private void BaselineNetClosureErrorForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_file.Filter = "全站仪平差结果|*.ou2";// Setting.BaseLineFileFilterOfLgo;
            if (!Directory.Exists(Setting.TempDirectory)) { return; }
            var files = Directory.GetFiles(Setting.TempDirectory, "*.ou2");
            if (files != null && files.Length > 0)
            {
                this.fileOpenControl_file.FilePathes = files;
            }
        }
         

        private void fileOpenControl_baseline_FilePathSetted(object sender, EventArgs e)
        {
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && ResultFile != null)
            {
                List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();
                int netIndex = 0;
                List<string> addedNames = new List<string>();
                foreach (var kv in ResultFile.ApproxCoords.KeyValues)
                {
                    var name = kv.Key;
                    var val = kv.Value;
                    double centerLon = 0;
                    var lonlat = Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(val, AveGeoHeight, OrinalLonDeg, YConst);//15,6

                    pts.Add(new AnyInfo.Geometries.Point(lonlat, null, name)); 
                    netIndex++;
                }
                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreatePointLayer(pts);
                ShowLayer(layer);
            }
        }
        TotalStationResultFile ResultFile;
        private void button_read_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl_file.FilePath;
            TotalStationResultReader reader = new TotalStationResultReader(path);
            ResultFile = reader.Read() ;
              

            this.objectTableControl_approxCoord.DataBind(ResultFile.GetApproxTable());
            this.objectTableControl_directionResult.DataBind(ResultFile.GetDirectionTable());
            this.objectTableControl_distanceResult.DataBind(ResultFile.GetDistanceTable());
            this.objectTableControl_adjustCoord.DataBind(ResultFile.GetCoordTable());
            this.objectTableControl_combined.DataBind(ResultFile.GetCombinedTable());


            this.bindingSource_startPt.DataSource = this.ResultFile.AdjustCoordResults.Keys;
            this.bindingSource_targetPtA.DataSource = this.ResultFile.AdjustCoordResults.Keys;
            this.bindingSource_targetPtB.DataSource = this.ResultFile.AdjustCoordResults.Keys;
        }

        private void button_solveAzimuth_Click(object sender, EventArgs e)
        {
            if (ResultFile == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取数据再试！");
                return;
            }
            var start = this.bindingSource_startPt.Current.ToString();
            var targetA = this.bindingSource_targetPtA.Current.ToString();
            var targetB = this.bindingSource_targetPtB.Current.ToString();

            var startObj = ResultFile.AdjustCoordResults[start];
            var targetAObj = ResultFile.AdjustCoordResults[targetA];
            var targetBObj = ResultFile.AdjustCoordResults[targetB];

            var az1 = Geo.Coordinates.CoordTransformer.GetAzimuthAngleOfLeftHandXy(startObj.XY, targetAObj.XY);
            var az2 = Geo.Coordinates.CoordTransformer.GetAzimuthAngleOfLeftHandXy(startObj.XY, targetBObj.XY);
             
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("坐标差北方位角：");
            
            sb.AppendLine(BuildAngleString(start, targetA, az1));

            var dis = (startObj.XY - targetAObj.XY).Radius();
            sb.AppendLine("距离(m)：" + dis.ToString("0.00000"));

            sb.AppendLine(BuildAngleString(start, targetB, az2));
            dis = (startObj.XY - targetBObj.XY).Radius();
            sb.AppendLine("距离(m)：" + dis.ToString("0.00000"));

            var differ = Math.Abs(az1 - az2);
            sb.AppendLine("夹角：" + targetA + "-" + start + "-" + targetB + " : " + differ.ToString("0.000000000") + " = " + new DMS(differ));

            if (true)
            {
                var lonlatStart = Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(startObj.XY, AveGeoHeight, OrinalLonDeg, YConst);
                var lonlatTargetA = Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(targetAObj.XY, AveGeoHeight, OrinalLonDeg, YConst);
                var lonlatTargetB = Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(targetBObj.XY, AveGeoHeight, OrinalLonDeg, YConst);

                sb.AppendLine("经纬度：");
                sb.AppendLine(start + " : " + new DMS(lonlatStart.Lon, AngleUnit.Degree) + ", " + new DMS(lonlatStart.Lat, AngleUnit.Degree));
                sb.AppendLine(targetA + " : " + new DMS(lonlatTargetA.Lon, AngleUnit.Degree) + ", " + new DMS(lonlatTargetA.Lat, AngleUnit.Degree));
                sb.AppendLine(targetB + " : " + new DMS(lonlatTargetB.Lon, AngleUnit.Degree) + ", " + new DMS(lonlatTargetB.Lat, AngleUnit.Degree));


                sb.AppendLine("大地方位角：");
                var azimuth1 = Geo.Coordinates.GeodeticUtils.BesselAzimuthAngle(lonlatStart, lonlatTargetA);
                var azimuth2 = Geo.Coordinates.GeodeticUtils.BesselAzimuthAngle(lonlatStart, lonlatTargetB);

                sb.AppendLine(BuildAngleString(start, targetA, azimuth1));
                sb.AppendLine(BuildAngleString(start, targetB, azimuth2));

                differ = Math.Abs(azimuth1 - azimuth2);
                sb.AppendLine("夹角：" + targetA + "-" + start + "-" + targetB + " : " + differ.ToString("0.000000000") + " = " + new DMS(differ));
            }
            this.richTextBoxControl_result.Text += sb.ToString();
        }

        public string BuildAngleString(string from, string to, double deg)
        { 
            var reverAz1 = deg + 180;
            if (reverAz1 > 360) { reverAz1 = reverAz1 - 360; }
            return from + "→" + to + " : " + deg.ToString("0.000000000") + " = " + new DMS(deg) +
                "，反向：" + reverAz1.ToString("0.000000000") + " = " + new DMS(reverAz1);
        }
    }
}
