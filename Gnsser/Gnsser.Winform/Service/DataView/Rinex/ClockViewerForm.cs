using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using AnyInfo;
using Geo.Times; 

namespace Gnsser.Winform
{
    /// <summary>
    /// 卫星钟差查看器。
    /// </summary>
    public partial class ClockViewerForm : Form, IShowLayer
    {
        public ClockViewerForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;
        ClockFile _clockFile;

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_Path.Text = this.openFileDialog1.FileName;
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            string path = this.textBox_Path.Text;
            if (!File.Exists(path))
            {
                MessageBox.Show("指定文件不存在！"); return;
            }
            ClockFileReader reader = new ClockFileReader(path,false);
            _clockFile = reader.ReadAll();
            if (_clockFile.ClockCount == 0) return;

            this.dateTimePicker_from.Value = _clockFile.First.First().Time.DateTime;
            this.dateTimePicker_to.Value = this.dateTimePicker_from.Value + TimeSpan.FromHours(6);

            this.comboBox_name.DataSource = _clockFile.Names;

            this.bindingSource1.DataSource =  _clockFile.AllItems;//.ClockDics.Values;
        }

        private void button_to_excel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1); }
        DateTime from, to;
        private void button_filter_Click(object sender, EventArgs e)
        {
            if (this.comboBox_name.SelectedItem == null)
            {
                MessageBox.Show("请先读取数据。"); return; ;
            }
            string name = this.comboBox_name.SelectedItem.ToString();
            from = this.dateTimePicker_from.Value;
            to = this.dateTimePicker_to.Value;
            this.bindingSource1.DataSource = _clockFile.GetClockItems(name, new Time( from), new Time( to));//
            //.ClockDics.FindAll(m =>
            //           m.Time.DateTime >= from
            //        && m.Time.DateTime <= to
            //        && m.Name == name);

        }

        private void button_inter_Click(object sender, EventArgs e)
        {
            double interval = double.Parse(this.textBox_interval.Text);
            List<AtomicClock> sortedRecords = this.bindingSource1.DataSource as List<AtomicClock>;
            if (sortedRecords == null)
            {
                MessageBox.Show("请先读取,并删选数据。"); return; ;
            }
            ClockInterpolator interp = new ClockInterpolator(sortedRecords);

            List<AtomicClock> fitedResult = new List<AtomicClock>();
            double cacuCount = (to - from).TotalSeconds / interval;
            for (int xi = 0; xi <= cacuCount; xi++)
            {
                Time gpsTime = sortedRecords[0].Time + interval * xi;
                fitedResult.Add(interp.GetAtomicClock(gpsTime));
            }

            this.bindingSource1.DataSource = fitedResult;

        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.bindingSource1.DataSource != null)
            { 
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                int i = 0;
                foreach (ClockSolnStation item in _clockFile.Header.ClockSolnStations)
                {
                    var pt = new AnyInfo.Geometries.Point(item.GeoCoord, i + "", item.Name);
                    lonlats.Add(pt);
                    i++;
                }
                if (lonlats.Count == 0) { return; }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void ClockViewerForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Text = Setting.GnsserConfig.SampleClkFile;
        }

    }
}
