using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Gnsser.Data;
using Geo.Coordinates;
using Geo.Referencing;
using AnyInfo;
using Geo.Algorithm;
using Geo.Times; 

namespace Gnsser.Winform
{
    /// <summary>
    /// 拟合星历服务。
    /// </summary>
    public partial class EphemerisServiceForm : Form, Gnsser.Winform.IShowLayer
    {
        public EphemerisServiceForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;

        private void button_getPath_Click(object sender, EventArgs e) { 
            if (this.openFileDialog_nav.ShowDialog() == DialogResult.OK) 
                this.textBox_Path.Lines = openFileDialog_nav.FileNames;
        }
         
        SequentialFileEphemerisService coll;
        private void button_read_Click(object sender, EventArgs e)
        {
            EphemerisDataSourceFactory fac = new EphemerisDataSourceFactory();
            string [] pathes = this.textBox_Path.Lines;
            coll = new SequentialFileEphemerisService(fac, pathes);
            this.bindingSource_prns.DataSource = coll.Prns;
            this.bindingSource1.DataSource =coll.Gets(); 
            //设置时间间隔
            this.dateTimePicker_from.Value = coll.TimePeriod.Start.DateTime;
            this.dateTimePicker_to.Value = coll.TimePeriod.End.DateTime;
        }

        private void button_toExcel_Click(object sender, EventArgs e) { 

            Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1);
        }

        List<Ephemeris> sortedRecords;

        private void button_show_Click(object sender, EventArgs e)
        {
            if (coll == null) { Geo.Utils.FormUtil.ShowErrorMessageBox("读入数据先！"); return; }

            SatelliteNumber PRN = SatelliteNumber.Parse(this.comboBox_prn.SelectedItem.ToString());
            //每次取10个数计算
            sortedRecords = coll.Gets(PRN, new Time(this.dateTimePicker_from.Value), new Time(this.dateTimePicker_to.Value));
   
            if (sortedRecords.Count != 0)
            {
                sortedRecords.Sort();
                ////设置时间间隔
                this.dateTimePicker_from.Value = sortedRecords[0].Time.DateTime;
                this.dateTimePicker_to.Value = sortedRecords[sortedRecords.Count - 1].Time.DateTime;
            }
            this.bindingSource1.DataSource = sortedRecords;
        }

        private void button_inter_Click(object sender, EventArgs e)
        {
            if (sortedRecords!= null &&
                sortedRecords.Count != 0)
            {
                double interval = Double.Parse(this.textBox_interval.Text);
                SatelliteNumber PRN = SatelliteNumber.Parse(this.comboBox_prn.SelectedItem.ToString());
                List<Ephemeris> fitedResult = new List<Ephemeris>();
                double cacuCount = (this.dateTimePicker_to.Value - this.dateTimePicker_from.Value).TotalSeconds / interval;
                for (int xi = 0; xi < cacuCount; xi++)
                {
                    Time gpsTime = sortedRecords[0].Time + interval * xi;
                    fitedResult.Add(coll.Get(PRN, gpsTime));
                }

                this.bindingSource1.DataSource = fitedResult;
            }
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.bindingSource1.DataSource != null)
            {
                List<Ephemeris> fitedResult = this.bindingSource1.DataSource as List<Ephemeris>;
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                foreach (Ephemeris item in fitedResult)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point(item.GeoCoord, item.Prn + " "+ item.Time.ToString()));
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

    }
}
