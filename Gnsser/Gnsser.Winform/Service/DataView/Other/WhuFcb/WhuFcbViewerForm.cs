//2017.06.14, czs, create in hongqing, 武大FCB读取查看器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using AnyInfo;
using Geo.Times;
using Gnsser.Data;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo;
using Geo.Algorithm;

namespace Gnsser.Winform
{
    public partial class WhuFcbViewerForm : Form
    {
        public WhuFcbViewerForm()
        {
            InitializeComponent();
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl1.FilePath;
            FcbFileReader reader = new FcbFileReader(path);
            FcbFile = reader.Read();
            this.bindingSource1.DataSource = FcbFile.FcbInfos;
            

            var wl = FcbFile.Header.WideLaneValue;
            if (wl != null)
            {
                List<FcbValue> newFile = new List<FcbValue>();
                foreach (var item in wl.Data)
                {
                    var newItem = new FcbValue(item.Key, wl.Time, item.Value.Value, item.Value.Rms);
                    newFile.Add(newItem);
                }

                this.bindingSource2.DataSource = newFile;
            }
        }

        FcbFile FcbFile { get; set; }

        private void IgsFcbViewerForm_Load(object sender, EventArgs e)
        {

            bindingSource_prns.DataSource = SatelliteNumber.DefaultGpsPrns;
        }

        private void button_filterPrn_Click(object sender, EventArgs e)
        {
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_prns.Current;


            var data = this.bindingSource1.DataSource as List<FcbValue>;
            if (data == null) { return; }
            this.bindingSource1.DataSource = data.FindAll(m => m.Prn == prn);

        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            Series Series = new System.Windows.Forms.DataVisualization.Charting.Series();

            Series seriesX = new Series(this.bindingSource_prns.Current.ToString());
            seriesX.ChartType = SeriesChartType.Line;// SeriesChartType.Point;
            seriesX.YValueType = ChartValueType.Double;
            seriesX.XValueType = ChartValueType.DateTime;
            seriesX.MarkerSize = 3;
            seriesX.BorderWidth = 2;
            seriesX.ToolTip = "#SERIESNAME: #VALX, #VALY";

            var data = this.bindingSource1.DataSource as List<FcbValue>;
            if (data == null) { return; }
            foreach (var item in data)
            {
                seriesX.Points.AddXY(item.Time.DateTime, item.Value);
            } 

            var form = new Geo.Winform.CommonChartForm(seriesX);
            form.Text = this.bindingSource_prns.Current.ToString() + "_FCB_of_" + Path.GetFileName( this.fileOpenControl1.FilePath);
            form.Show();
        }
    }
}

