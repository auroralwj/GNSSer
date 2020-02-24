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
using Gnsser;
using Geo.Coordinates;  
using Geo.Referencing;
using Geo.Times;
using AnyInfo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 查看观测时段
    /// </summary>
    public partial class ViewObsTimePeriodForm : Form, IShowLayer
    {
        List<LonLat> lonLats = new List<LonLat>();
       
        public event ShowLayerHandler ShowLayer;

        public ViewObsTimePeriodForm()
        {

            InitializeComponent();

            this.chart1.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.SharpTriangle;
            this.chart1.ChartAreas[0].AxisY.LabelStyle.Format = "G";
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.Title = "时间";
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            this.chart1.ChartAreas[0].AxisX.Interval = 1;
            // Disable offset labels style
            //this.chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
              
            this.chart1.Series.Clear();
        }

        Dictionary<string, string> pathes = new Dictionary<string, string>();

        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string msg = "";
                foreach (string p in openFileDialog1.FileNames)
                {
                    string fileName = Path.GetFileName(p);
                    if (!pathes.ContainsKey(fileName))  pathes.Add(fileName, p);
                    else  msg += fileName + "," + "\r\n";
                }
                if (msg != "")
                {
                    MessageBox.Show("以下文件没有添加成功，原因是已经包含了同名称的文件。\r\n" + msg);
                }
                this.bindingSource1.DataSource = null;
                this.bindingSource1.DataSource = pathes.Keys;
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            DrawLine();
        }

        private void DrawLine()
        {
            this.chart1.ChartAreas[0].AxisY.CustomLabels.Clear();
            lonLats.Clear();

            int i = 0;
            foreach (KeyValuePair<string, string> kv in pathes)
            {
                if (this.chart1.Series.FindByName(kv.Key) != null) continue;

                Gnsser.Data.Rinex.RinexObsFile obsFile = (Data.Rinex.RinexObsFile)new Data.Rinex.RinexObsFileReader(kv.Value).ReadObsFile();
                
                //文件的物理路径所在文件夹路径
                string paths = kv.Value;
                string tmp = paths.Substring(0, paths.LastIndexOf("\\"));
                string orfolder = paths.Substring(0, paths.LastIndexOf("\\") + 1);
                string folder = orfolder.Replace("\\", "\\\\");
          
                if (obsFile.StartTime ==  Time.Default || obsFile.EndTime == Time.Default) continue; 
                if(obsFile.Header.ApproxXyz != null){
                    GeoCoord gCoord = CoordTransformer.XyzToGeoCoord( obsFile.Header.ApproxXyz);
                    lonLats.Add(gCoord);
                }

                DateTime first = obsFile.StartTime.DateTime;
                DateTime last = obsFile.EndTime.DateTime;

                Series series = new Series(kv.Key);
                //series.BorderWidth = 6;
                //series.ShadowOffset = 2;
                series.ChartType = SeriesChartType.RangeBar;
                series.YValueType = ChartValueType.DateTime;
                series.XValueType = ChartValueType.Int32;
                series.ChartArea = "ChartArea1";

                // Set axis labels
               // chart1.ChartAreas[0].AxisY.
                //series.AxisLabel = "Month: #VALX{MMM}\nDay: #VALX{dd}";

               // series.AxisX.IsMarginVisible = true;  

                this.chart1.Series.Add(series);

                //Y轴自定义标签。
                //CustomLabel c = new CustomLabel(xi-1, xi+1, kv.Key, 1, LabelMarkStyle.None);
                //this.chart1.ChartAreas[0].AxisY.CustomLabels.Add(c);

                DataPoint point = new DataPoint();
                point.SetValueXY(i+1, first, last);
                point.AxisLabel = kv.Key;

                series.Points.Add(point);
                i++;
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

        private void button_clear_Click(object sender, EventArgs e)
        {
            this.chart1.Series.Clear();
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key = this.listBox1.SelectedItem.ToString();
            this.bindingSource1.Remove(key);
            this.pathes.Remove(key);
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.bindingSource1.Clear();
            this.pathes.Clear();
        }

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

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && lonLats.Count != 0)
            {
                Layer layer = LayerFactory.CreatePointLayer(lonLats);
                ShowLayer(layer);
            }
        }

        private void button_CreatConfigeFile_Click(object sender, EventArgs e)
        {

        }
       
    }
}
