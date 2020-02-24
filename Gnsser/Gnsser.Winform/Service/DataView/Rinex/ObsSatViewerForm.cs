using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting.Data;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Gnsser;
using Geo.Coordinates;  
using Geo.Referencing;
using AnyInfo;
using Geo.Times; 

namespace Gnsser.Winform
{
    public partial class ObsSatViewerForm : Form
    {
        public ObsSatViewerForm()
        {
            InitializeComponent();

            this.chart1.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            this.chart1.ChartAreas[0].AxisY.LabelStyle.Format = "G";
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.Title = "时间";
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            this.chart1.ChartAreas[0].AxisX.Title = "PRN";
            this.chart1.ChartAreas[0].AxisX.Interval = 1;
            // Disable offset labels style
            this.chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;

            this.chart1.Series.Clear();
        }
        string OutputDirectory { get { return Setting.GnsserConfig.TempDirectory; } }
        RinexFileObsDataSource ObsDataSource { get; set; }
 
        private void button_draw_Click(object sender, EventArgs e)
        {
            if (ObsDataAnalyst == null) MessageBox.Show("数据为空！请先读取数据。");
            DrawLine();
            SaveAndOpenFile();
        }

        private void SaveAndOpenFile()
        {
            string fileName = Path.GetFileNameWithoutExtension( FilePath);
            string path = Path.Combine(OutputDirectory,  fileName + "_SatPeriod.txt");
            ObsDataAnalyst.SatVisibleMaker.SaveSatPeriodText(path,checkBox_sortPrn.Checked);
            string selector = ObsDataAnalyst.SatelliteSelector.GetFormatedString();
            Geo.Utils.FileUtil.OpenFile(path);
        } 
        private void DrawLine()
        {
            DrawChart();
             string fileName = Path.GetFileNameWithoutExtension( FilePath);
             string path = Path.Combine(OutputDirectory, fileName + "_卫星出现率.txt");
            File.WriteAllText(path, ObsDataAnalyst.SatVisibleMaker.ToFormatedString());
        }

        private void DrawChart()
        {
            this.chart1.Series.Clear();

            // Set axis labels
            // chart1.ChartAreas[0].AxisY.
            //series.AxisLabel = "Month: #VALX{MMM}\nDay: #VALX{dd}";
            // series.AxisX.IsMarginVisible = true;   
            List<SatelliteNumber> prns = new List<SatelliteNumber>(ObsDataAnalyst.SatVisibleMaker.Prns);
            prns.Sort();
            int i = 0;
            foreach (var sat in prns)
            {
                Series series = this.chart1.Series.FindByName(sat.ToString());

                if (series == null)
                {
                    series = new Series(sat.ToString());
                }

                series.ChartType = SeriesChartType.RangeBar;
                series.XValueType = ChartValueType.Int32;
                series.YValueType = ChartValueType.DateTimeOffset;
                series.BorderWidth = 6;
                series["PixelPointWidth"] = "80";
                //Keep in mind that PointWidth defines the aboutSize relative to the available space between the satData points. 
                //By setting this property to 1.0 satData points of the chart with coefficient single series will have no spacing between the points.
                //Setting property to 0.5 will provide 50% gap.
                series["PointWidth"] = "0.9";

                var periods = ObsDataAnalyst.SatVisibleMaker.GetPeriods(sat);
                foreach (BufferedTimePeriod d in periods)
                {
                    DataPoint startPoint = new DataPoint();
                    startPoint.SetValueXY(sat.PRN, d.Start.DateTime,d.End.DateTime);
                    startPoint.AxisLabel = sat.ToString();
                    series.Points.Add(startPoint);

                    //DataPoint endPoint = new DataPoint();
                    //endPoint.SetValueXY( sat.PRN,d.End.DateTime);
                    //endPoint.AxisLabel = sat.ToString();

                    //series.Points.Add(endPoint);

                    i++;

                }
                this.chart1.Series.Add(series);
            }
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            Series series = new Series("Series1", 2010);
            series.XAxisType = AxisType.Secondary;
            // Initialize an array of doubles
            double[] yval = { 2, 6, 4, 5, 3 };
            series.ChartType = SeriesChartType.Line;

            // Initialize an array of strings
            string[] xval = { "Peter", "Andrew", "Julie", "Mary", "Dave" };

            // BindMain the double array to the y axis points of the Default satData series
            series.Points.DataBindXY(xval, yval);

            this.chart1.Series.Add(series);

            //this.chart1.Series.Add(series);
            //for (int xi = 0; xi < 40; xi++)
            //    series.Points.Add(new DataPoint(xi * 2, Math.Sin(xi) * 10));
        }

        private void button_clear_Click(object sender, EventArgs e) { this.chart1.Series.Clear(); }

        private void button_copyPic_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                this.chart1.SaveImage(stream, ChartImageFormat.Png);

                Bitmap image = new Bitmap(stream);
                Clipboard.SetImage(image);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "获取图像失败，请重试！"); }
        }

        private void ObsSatViewerForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl1.FilePath = Setting.GnsserConfig.SampleOFileA;
        }
        String FilePath { get { return this.fileOpenControl1.FilePath; } }

        private void button_readFile_Click(object sender, EventArgs e)
        {
            if (!Geo.Utils.FormUtil.CheckExistOrShowWarningForm(FilePath)) {  return;  }
            //加载文件数据
            ObsDataSource = new RinexFileObsDataSource(FilePath, true);
            ObsDataAnalyst = new ObsDataAnalyst(ObsDataSource, multiGnssSystemSelectControl1.SatelliteTypes); 

            DrawLine();
            SaveAndOpenFile();
        }
        ObsDataAnalyst ObsDataAnalyst { get; set; }
    }
}
