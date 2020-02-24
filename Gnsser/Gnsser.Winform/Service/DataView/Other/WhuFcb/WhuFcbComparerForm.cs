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
    public partial class WhuFcbComparerForm : Form
    {
        public WhuFcbComparerForm()
        {
            InitializeComponent();
        }
         
         

        private void IgsFcbViewerForm_Load(object sender, EventArgs e)
        {

            bindingSource_prns.DataSource = SatelliteNumber.DefaultGpsPrns;
        }

        private void button_filterPrn_Click(object sender, EventArgs e)
        {
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_prns.Current; 

            var data = this.bindingSource3.DataSource as List<FcbValue>;
            if (data == null) { return; }
            this.bindingSource3.DataSource = data.FindAll(m => m.Prn == prn);

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

            var data = this.bindingSource3.DataSource as List<FcbValue>;
            if (data == null) { return; }
            foreach (var item in data)
            {
                seriesX.Points.AddXY(item.Time.DateTime, item.Value);
            }


            var form = new Geo.Winform.CommonChartForm(seriesX);
            form.Text = this.bindingSource_prns.Current.ToString() + "_FCB_of_" + Path.GetFileName( this.fileOpenControl1.FilePath);
            form.Show();
        }

        private void button_readAndCompare_Click(object sender, EventArgs e)
        {
            var path1 = this.fileOpenControl1.FilePath;
            FcbFileReader reader = new FcbFileReader(path1);
            FcbFile FcbFile = reader.Read();

            this.bindingSource1.DataSource = FcbFile.FcbInfos;

            var path2 = this.fileOpenControl2.FilePath;
            FcbFileReader reader2 = new FcbFileReader(path2);
            FcbFile FcbFile2 = reader2.Read();

            this.bindingSource2.DataSource = FcbFile2.FcbInfos;

            FcbFile newFile = new FcbFile();
            //foreach (var item in FcbFile)
            //{
            //    var item2 = FcbFile2.GetFcbValue( item.Prn,   item.Time);
            //    if(item2 ==null){continue;}
            //    var newVal = item.Value - item2.Value;
            //    var newRms = item.Rms - item2.Rms;
            //    var newItem = new FcbValue(item.Prn, item.Time,newVal,newRms);
            //    newFile.FcbInfos.Add(newItem);
            //} 

            this.bindingSource3.DataSource = newFile.FcbInfos;

        }
    }
}

