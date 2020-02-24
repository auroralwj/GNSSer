//2019.01.12, czs, create in hmx, PlanXyLineViewerForm

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Winform;
using Gnsser;

namespace Geo.Winform
{
    public partial class PlanXyLineViewerForm : Form, IShowLayer
    {
        public PlanXyLineViewerForm()
        {
            InitializeComponent();
            fileOpenControl1.Filter = Setting.SiteCoordFileFilter;
        }
        string FilePath => fileOpenControl1.FilePath;
        public event ShowLayerHandler ShowLayer;
        /// <summary>
        /// Y加常数
        /// </summary>
        double YConst => namedFloatControlYConst.GetValue();

        double OrinalLonDeg => this.namedFloatControl_orinalLonDeg.GetValue();

        double AveGeoHeight => this.namedFloatControl_aveGeoHeight.GetValue();
        GaussXyNetLine ResultFile;
        private void button_run_Click(object sender, EventArgs e)
        {
            var talbe = ObjectTableReader.Read(FilePath);
            ResultFile =   GaussXyNetLine.Parse(talbe, OrinalLonDeg, AveGeoHeight, YConst);

            this.bindingSource_startPt.DataSource = ResultFile.Keys;
            this.bindingSource_targetPtA.DataSource = ResultFile.Keys;
            this.bindingSource_targetPtB.DataSource = ResultFile.Keys; 

            var ptTable = ResultFile.GetPointTable();
            objectTableControl_point.DataBind(ptTable);

            var lineTable = ResultFile.GetAllLineTable();

            this.objectTableControl_site.DataBind(talbe);
            this.objectTableControl_vector.DataBind(lineTable);
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && ResultFile != null)
            {
                List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();
                int netIndex = 0;
                List<string> addedNames = new List<string>();
                foreach (var kv in ResultFile.KeyValues)
                {
                    var name = kv.Key;
                    var val = kv.Value; 
                    pts.Add(new AnyInfo.Geometries.Point(val.LonLat, null, name));
                    netIndex++;
                }
                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreatePointLayer(pts);
                ShowLayer(layer);
            }

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

            var startObj = ResultFile[start];
            var targetAObj = ResultFile[targetA];
            var targetBObj = ResultFile[targetB];


            var lineA = new GaussXyVectorLine(startObj, targetAObj);
            var lineB = new GaussXyVectorLine(startObj, targetBObj);
             

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("坐标差北方位角：");

            sb.AppendLine(BuildAngleString(start, targetA, lineA.Azimuth)); 

            var dis = lineA.Length;
            sb.AppendLine("距离(m)：" + dis.ToString("0.00000"));

            sb.AppendLine(BuildAngleString(start, targetB, lineB.Azimuth));
            dis = lineB.Length;
            sb.AppendLine("距离(m)：" + dis.ToString("0.00000"));

            var differ = Math.Abs(lineA.Azimuth - lineB.Azimuth);
            sb.AppendLine("夹角：" + targetA + "-" + start + "-" + targetB + " : " + differ.ToString("0.000000000") + " = " + new DMS(differ));

            if (true)
            {
                var lonlatStart = startObj.LonLat;// Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(startObj, AveGeoHeight, OrinalLonDeg, YConst);
                var lonlatTargetA = targetAObj.LonLat;//Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(targetAObj, AveGeoHeight, OrinalLonDeg, YConst);
                var lonlatTargetB = targetBObj.LonLat;// Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(targetBObj, AveGeoHeight, OrinalLonDeg, YConst);

                sb.AppendLine("经纬度：");
                sb.AppendLine(start + " : " + lonlatStart.ToDmsString() );
                sb.AppendLine(targetA + " : " + lonlatTargetA.ToDmsString() );
                sb.AppendLine(targetB + " : " + lonlatTargetB.ToDmsString() );


                sb.AppendLine("大地方位角："); 

                sb.AppendLine(BuildAngleString(start, targetA, lineA.GeodeticAzimuth));
                sb.AppendLine(BuildAngleString(start, targetB, lineB.GeodeticAzimuth));

                differ = Math.Abs(lineA.GeodeticAzimuth - lineB.GeodeticAzimuth);
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
