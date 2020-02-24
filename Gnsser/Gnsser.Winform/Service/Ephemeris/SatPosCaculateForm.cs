using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting.Data;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser.Data.Rinex;
using System.IO;
using Geo.Times; 
using Geo.Coordinates;
using Gnsser;
using Gnsser.Data;
using Geo.Referencing;
using AnyInfo;

namespace Gnsser
{
    public delegate void ShowLayerHandler(Layer layer);

    public partial class SatPosCaculateForm : Form, Gnsser.Winform.IShowLayer
    {
        List<LonLat> lonLats = new List<LonLat>();
        public event ShowLayerHandler ShowLayer;
        FileEphemerisService navFile;
        SatInfoCaculator sathelper { get; set; }

        public SatPosCaculateForm()
        {
            InitializeComponent();

            this.dateTimePicker_from.Value = DateTime.Now.Date;
            this.dateTimePicker_to.Value = dateTimePicker_from.Value + TimeSpan.FromDays(1);

            this.chart_satRanges.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.SharpTriangle;
            this.chart_satRanges.ChartAreas[0].AxisY.LabelStyle.Format = "G";
            this.chart_satRanges.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart_satRanges.ChartAreas[0].AxisY.IsMarginVisible = false;
            this.chart_satRanges.ChartAreas[0].AxisY.Title = "时间";
            this.chart_satRanges.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            //this.chart1.ChartAreas[0].AxisX.Interval = 1;
            this.chart_satRanges.ChartAreas[0].AxisX.IsMarginVisible = false;
            // Disable offset labels style
            //this.chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;


            this.chart_satCounts.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.SharpTriangle;
            this.chart_satCounts.ChartAreas[0].AxisX.LabelStyle.Format = "G";
            this.chart_satCounts.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //this.chart3.ChartAreas[0].AxisX.IsMarginVisible = false;
            this.chart_satCounts.ChartAreas[0].AxisX.Title = "时间";
            this.chart_satCounts.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            //this.chart3.ChartAreas[0].AxisY.Interval = 1;
            this.chart_satCounts.ChartAreas[0].AxisY.IsMarginVisible = true;

        } 

        string FilePath => fileOpenControl_eph.FilePath;
        private void Init()
        {
            navFile = EphemerisDataSourceFactory.Create(FilePath);
            this.bindingSource_satsPrn.DataSource = navFile.Prns;

            sathelper = new SatInfoCaculator(
                navFile,
                StationPos);

            this.dateTimePicker_from.Value = navFile.TimePeriod.Start.DateTime;
            this.dateTimePicker_to.Value = navFile.TimePeriod.End.DateTime;
            this.dateTimePicker_moment.Value = this.dateTimePicker_from.Value; 

        }
        private void button_getLonLatFromObsFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog_obs.ShowDialog() == DialogResult.OK)
            {
                RinexObsFileHeader h = new Data.Rinex.RinexObsFileReader(openFileDialog_obs.FileName).GetHeader();
                this.textBox_stationCoord.Text = CoordTransformer.XyzToGeoCoord(h.ApproxXyz, Ellipsoid.WGS84).ToString();

                Init(); 

                this.sathelper.StationPos = StationPos;
            }
        }
        public bool CheckOrInit()
        {
            try
            {
                if (sathelper == null)
                {
                    Init();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void button_cacuAllInstantSatInfos_Click(object sender, EventArgs e)
        {
            if (!CheckOrInit()) { return; }

            sathelper.EleAngle = this.EleAngle;
            List<StationSatInfo> infos = sathelper.GetAllInstantSatInfos( Time);
            this.bindingSource_instantMultiSatInfo.DataSource = infos;

            this.label_satInfo.Text = "卫星数量：" + infos.Count;
        } 

        private List<StationSatInfo> GetSatInfosOnTime()
        {
            sathelper.EleAngle = this.EleAngle;
            return sathelper.GetAllInstantSatInfos( Time);
        }

        public XYZ StationPos
        {
            get
            {
                GeoCoord geoCoord = GeoCoord.Parse(this.textBox_stationCoord.Text);
                return Geo.Coordinates.CoordTransformer.GeoCoordToXyz(geoCoord, Ellipsoid.WGS84);
            }
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {

            if (ShowLayer != null && sathelper!=null  && this.sathelper.LonLats.Count != 0)
            {
                List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();
                foreach (var item in this.sathelper.LonLats)
                {
                    pts.Add(new AnyInfo.Geometries.Point(item[0], item[1], item.Tag.ToString()));
                }
                Layer layer = LayerFactory.CreatePointLayer(pts);
                ShowLayer(layer);
            }
        }
        /// <summary>
        /// 时段单星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="east"></param>
        private void button_caculate_Click(object sender, EventArgs e)
        {
            if (!CheckOrInit()) { return; }
            SatelliteNumber rec = (SatelliteNumber)this.bindingSource_satsPrn.Current;
            sathelper.EleAngle = this.EleAngle;
            sathelper.SetPeriod(From, To, SpanMinute);
            this.bindingSource1.DataSource = sathelper.GetPeriodSatInfos(rec);
        }

        #region
        private void button4_cacuAppearRange_Click(object sender, EventArgs e)
        {
            if (!CheckOrInit()) { return; }

            this.chart_satRanges.Series.Clear();
            sathelper.EleAngle = this.EleAngle;
            sathelper.SetPeriod(From, To, SpanMinute);
            List<VisibilityOfSat> sats = sathelper.GetPeriodSatAppearTimes();

            int i = 0;
            foreach (VisibilityOfSat sat in sats)
            {
                if (this.chart_satRanges.Series.FindByName(sat.PRN.ToString()) != null) continue;

                Series series = new Series(sat.PRN.ToString());
                series.ChartType = SeriesChartType.RangeBar;
                series.YValueType = ChartValueType.DateTime;
                series.XValueType = ChartValueType.Int32;
                series["PixelPointWidth"] = "80";
                //Keep in mind that PointWidth defines the aboutSize relative to the available space between the satData points. 
                //By setting this property to 1.0 satData points of the chart with coefficient single series will have no spacing between the points.
                //Setting property to 0.5 will provide 50% gap.
                //series["PointWidth"] = "0.9";

                foreach (BufferedTimePeriod d in sat.VisibleTimes)
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(i + 1, d.Start.DateTime, d.End.DateTime);
                    point.AxisLabel = sat.PRN.ToString();
                    series.Points.Add(point);
                }
                this.chart_satRanges.Series.Add(series);

                i++;
            }
        }
      
        #endregion 

        private void button1_radar_Click(object sender, EventArgs e)
        {
            if (!CheckOrInit()) { return; }
            this.chart_radar.Series.Clear();

            List<StationSatInfo> infos = GetSatInfosOnTime();
            Series series = new Series("卫星");

            series.ChartType = SeriesChartType.Polar;
            series.YValueType = ChartValueType.Double;
            series.XValueType = ChartValueType.Double;
            series["PolarDrawingStyle"] = "Marker";
            series.MarkerSize = 10;
            series.MarkerStyle = MarkerStyle.Circle;
            foreach (StationSatInfo info in infos)
            {
                DataPoint point = new DataPoint(info.Azimuth, info.ElevatAngle);
                point.Label = info.PRN.ToString();
                series.Points.Add(point);
            }
            this.chart_radar.Series.Add(series);
        }

        int EleAngle { get { return int.Parse(this.textBox1_eveAngle.Text); } }
        public DateTime From { get { return this.dateTimePicker_from.Value; } }
        public DateTime To { get { return this.dateTimePicker_to.Value; } }
        public DateTime Time { get { return this.dateTimePicker_moment.Value; } }
        double SpanMinute { get { return double.Parse(this.textBox_timespanMinute.Text); } }
        int Count { get { return (int)Math.Round((To - From).TotalMinutes / SpanMinute); } }

        private void button_polarSpan_Click(object sender, EventArgs e)
        {
            if (!CheckOrInit()) { return; }
            this.chart_radar.Series.Clear();
            sathelper.EleAngle = this.EleAngle;
            sathelper.SetPeriod(From, To, SpanMinute);
            Dictionary<SatelliteNumber,List<StationSatInfo>> infos = sathelper.GetPeriodAllSatInfos();

            foreach(KeyValuePair<SatelliteNumber, List<StationSatInfo>> kv in infos){              
                Series series = new Series(  kv.Key.ToString());
                series.ChartType = SeriesChartType.Polar;
                series.YValueType = ChartValueType.Double;
                series.XValueType = ChartValueType.Double;
                series["PolarDrawingStyle"] = "Line";
             //   series.MarkerSize = 5;
               // series.MarkerStyle = MarkerStyle.Circle;
                this.chart_radar.Series.Add(series);
                foreach(StationSatInfo info in kv.Value){
                     DataPoint point = new DataPoint(info.Azimuth, info.ElevatAngle);
                      
                     series.Points.Add(point);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        { 
            CopyChart(this.chart_radar); 
        }
        private void button1_copyChart1Pic_Click(object sender, EventArgs e)
        { 
            CopyChart(this.chart_satRanges);
        }

        private void button_copyChart3_Click(object sender, EventArgs e)
        {
            CopyChart(this.chart_satCounts); 
        }

        private static void CopyChart(Chart c)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                c.SaveImage(stream, ChartImageFormat.Png);

                Bitmap image = new Bitmap(stream);
                Clipboard.SetImage(image);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "获取图像失败，请重试！"); }
        }

        private void button_cacuViewCount_Click(object sender, EventArgs e)
        {
            if (!CheckOrInit()) { return; }
            this.chart_satCounts.Series.Clear();

            sathelper.EleAngle = this.EleAngle;
            sathelper.SetPeriod(From, To, SpanMinute);
            Dictionary<DateTime, Int32> infos = sathelper.GetPeriodSatAppearCounts();

            Series series = new Series("卫星可见数/个");
            series.ChartType = SeriesChartType.Line;//.StepLine;
            series.YValueType = ChartValueType.Int32;
            series.XValueType = ChartValueType.DateTime;
            series.BorderWidth = 3;  
            this.chart_satCounts.Series.Add(series);

            foreach (KeyValuePair<DateTime, Int32> kv in infos)
            { 
                series.Points.AddXY(kv.Key, kv.Value);
            }
        }

        private void button_setSationCoord_Click(object sender, EventArgs e)
        {
            if (!CheckOrInit()) { return; }
            this.sathelper.StationPos = StationPos;
        }

        private void button_exportSatHeight_Click(object sender, EventArgs e)
        {

            if (!CheckOrInit()) { return; }
            Time from = new Time(this.dateTimePicker_from.Value);
            Time  to = new Time(this.dateTimePicker_to.Value);
            StringBuilder sb = new StringBuilder(); 
            sb.Append("Time");
            sb.Append("\t");
            int i = 0;
            foreach (var item in this.navFile.Prns)
            {
                if(i !=0) sb.Append("\t");
                sb.Append(item);
                i++;
            }
            sb.AppendLine();

            for (Time time = from; time < to; time = time + TimeSpan.FromMinutes(10))
            {
                sb.Append(time.ToString());
                sb.Append("\t");
                i = 0;
                foreach (var item in this.navFile.Prns)
                {
                    if (i != 0) sb.Append("\t");
                    var satPos = navFile.Get(item, time);
                   // GeoCoord coord = CoordTransformer.XyzToGeoCoord(satPos.XYZ);
                    Polar p = CoordTransformer.XyzToGeoPolar(satPos.XYZ, StationPos);
                   sb.Append(p.Elevation);
                   //  sb.Append(satPos.XYZ.Radius());
                    i++;
                }
                sb.AppendLine(); 
            }

            string path = "D:\\satElevation.xls";
            File.WriteAllText(path, sb.ToString());
            Geo.Utils.FormUtil.ShowOkAndOpenFile(path);
        }

        private void SatPosCaculateForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_eph.FilePath = Gnsser.Winform.Setting.GnsserConfig.SampleNFile;
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            Init();
        }
    }
}
