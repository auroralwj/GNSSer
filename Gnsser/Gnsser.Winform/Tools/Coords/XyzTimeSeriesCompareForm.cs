
//2017.03.22， double create in zhengzhou,仿照XyzCompareForm， GNSSer 计算的坐标时间序列结果比较，旨在得出测站随着时间的推移计算的偏差。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms; 
using Gnsser.Data.Sinex; 
using Geo.Utils;
using Geo.Coordinates;
using AnyInfo; 
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service; 
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo;
using Geo.IO;
using Geo.Times;


namespace Gnsser.Winform
{
    public partial class XyzTimeSeriesCompareForm : Form, IShowLayer
    {
        Log log = new Log(typeof(XyzTimeSeriesCompareForm));
        public XyzTimeSeriesCompareForm()
        {
            InitializeComponent();
         //   Geo.Coordinates.NamedXyz
        }

        public event ShowLayerHandler ShowLayer;
   

        private void button_getPath_Click(object sender, EventArgs e) { if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)   this.textBox_PathA.Text = this.openFileDialog1.FileName; }
        private void button_getPathB_Click(object sender, EventArgs e) { if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox_PathB.Text = this.openFileDialog1.FileName; }
        private void button_readB_Click(object sender, EventArgs e) { Read(); }
        public ObjectTableManager TableTextManager { get; set; }
        private void Read()
        {
            string pathA = this.textBox_PathA.Text;

            if (!File.Exists(pathA))
            {
                FormUtil.ShowFileNotExistBox(pathA);
                return;
            }
            string pathB = this.textBox_PathB.Text;            

            List<NamedXyzAndTime> coordsA = NamedXyzParser.GetCoordsAndTime(pathA);
            this.bindingSourceA.DataSource = coordsA;

            List<NamedXyzAndTime> coordsResult = new List<NamedXyzAndTime>();
            List<NamedXyzEnuAndTime> namedXyzEnusAndTime = new List<NamedXyzEnuAndTime>();
            TableTextManager = new ObjectTableManager();
            TableTextManager.OutputDirectory = Path.GetDirectoryName(pathA);// "D:\\Temp\\errorSSR\\";
            var paramTable = TableTextManager.GetOrCreate("CoordinationErrorOfTimeSeries" + Time.UtcNow.DateTime.ToString("yyyy-MM-dd_HH_mm_ss"));//.ToString("yyyy-MM-dd_HH_mm_ss"));

            foreach (var item in coordsA)
            {
                string dayOfWeek = item.dayOfWeek;
                string pathB0=Path.GetFileNameWithoutExtension(pathB);
                string dayOfWeekfileName = null;
                if (pathB0.Substring(0, 3).ToLower() == "gfz" || pathB0.Substring(0, 3).ToLower() == "cod")
                    dayOfWeekfileName = pathB0.Substring(3, 5);
                else dayOfWeekfileName = pathB0.Substring(6, 5);
                string path = pathB.Replace(dayOfWeekfileName, dayOfWeek);
                if (!File.Exists(path))
                {
                    //FormUtil.ShowFileNotExistBox(path);
                    continue; 
                }
                List<NamedXyz> coordsB = NamedXyzParser.GetCoords(path);
                var staXyz = coordsB.Find(m => String.Equals(m.Name, item.Name, StringComparison.CurrentCultureIgnoreCase));
                if (staXyz != null)
                {
                    NamedXyzAndTime NamedXyzAndTime = new NamedXyzAndTime();
                    NamedXyzAndTime.Name = item.Name;
                    NamedXyzAndTime.dayOfWeek = item.dayOfWeek;
                    NamedXyzAndTime.Value = item.Value - staXyz.Value;
                    var enu = NamedXyzEnuAndTime.Get(item.Name, dayOfWeek, NamedXyzAndTime.Value, new XYZ(staXyz.X, staXyz.Y, staXyz.Z));
                    namedXyzEnusAndTime.Add(enu);
                    paramTable.NewRow();
                    paramTable.AddItem("Day", item.dayOfWeek);
                    paramTable.AddItem("Name", item.Name);
                    paramTable.AddItem("dX", enu.X);
                    paramTable.AddItem("dY", enu.Y);
                    paramTable.AddItem("dZ", enu.Z);
                    paramTable.AddItem("dE", enu.E);
                    paramTable.AddItem("dN", enu.N);
                    paramTable.AddItem("dU", enu.U);
                    paramTable.AddItem("length", Math.Sqrt(enu.X*enu.X+enu.Y*enu.Y+enu.Z*enu.Z));
                    paramTable.EndRow(); 
                }
            }

            TableTextManager.WriteAllToFileAndCloseStream();
            if (coordsResult == null) { return; }


            this.bindingSourceC.DataSource = namedXyzEnusAndTime;

            //更进一步，计算偏差RMS
            var table2 = Geo.Utils.DataGridViewUtil.GetDataTable(this.dataGridView1);

            ObjectTableStorage table = new ObjectTableStorage(table2);

            var vector = table.GetAveragesWithStdDev();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("参数\t 平均数\t 均方差");
            foreach (var item in vector)
            {
                sb.AppendLine(item.Key + "\t" +item.Value[0] + "\t" + item.Value[1]);
            }
            var info = sb.ToString();
            MessageBox.Show(info);
            log.Info(info);

            ObjectTableStorage summeryTable = new ObjectTableStorage();
            summeryTable.NewRow();
            summeryTable.AddItem("Name", "Ave");
            foreach (var item in vector)
            {
                summeryTable.AddItem(item.Key, item.Value[0]);
            }
            summeryTable.NewRow();
            summeryTable.AddItem("Name", "Rms");
            foreach (var item in vector)
            {
                summeryTable.AddItem(item.Key, item.Value[1]);
            }
            summeryTable.EndRow();
            Geo.Utils.FileUtil.OpenDirectory(TableTextManager.OutputDirectory);
            this.dataGridView_summery.DataSource = summeryTable.GetDataTable("结果汇总");
        }

        List<NamedXyz> Compared { get; set; }
    
        private void button_showOnMap_Click(object sender, EventArgs e)
        { 
            List<NamedXyz> coordsB = this.bindingSourceA.DataSource as List<NamedXyz>; 

            if (ShowLayer != null && Compared != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>(); 
                int i = 1;
                foreach (var g in coordsB)
                {
                    var find = Compared.Find(m => String.Equals(m.Name, g.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (find == null) continue;

                    var geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(g.Value);
                    var name = find.Name + ","  + find.Value.ToString();
                    lonlats.Add(new AnyInfo.Geometries.Point(geoCoord,  (i++) + "", name));
                } 
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
            else
            {
                MessageBox.Show("请先读取！");
            }
        }

        private void button_draw_Click(object sender, EventArgs e)
        {        
            if (Compared != null)
            {
                DrawDifferLine(Compared);
            }
            else
            {
                MessageBox.Show("请先读取！");
            }
        }

        public void DrawDifferLine(List<NamedXyz> differs)
        { 

            if (differs == null || differs.Count == 0) return;
            int index = 0; 
            //X
            Series seriesX = new Series("X");
            seriesX.ChartType = SeriesChartType.Column;

            foreach (var item in differs.ToArray())
            {
                index++;
                seriesX.Points.Add(new DataPoint(index, item.Value.X));

            }
            //Y
            index = 0;
            Series seriesY = new Series("Y");
            seriesY.ChartType = SeriesChartType.Column;
            foreach (var item in differs.ToArray())
            {
                index++;
                seriesY.Points.Add(new DataPoint(index, item.Value.Y ));

            }
            //Z
            index = 0;
            Series seriesZ = new Series("Z");
            seriesZ.ChartType = SeriesChartType.Column;
            foreach (var item in differs.ToArray())
            {
                index++;
                seriesZ.Points.Add(new DataPoint(index, item.Value.Z ));
            }

            Geo.Winform.CommonChartForm form = new Geo.Winform.CommonChartForm(seriesX, seriesY, seriesZ);
            form.Text = "XYZ Error";
            form.Show();
        }

        private void dataGridViewA_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
