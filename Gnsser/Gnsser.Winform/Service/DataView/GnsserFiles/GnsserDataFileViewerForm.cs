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
using Geo.IO;
using Gnsser.Api;

namespace Gnsser.Winform
{
    /// <summary>
    /// 卫星钟差查看器。
    /// </summary>
    public partial class GnsserDataFileViewerForm : Form, IShowLayer
    {
        public GnsserDataFileViewerForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer; 

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

            var extention = Path.GetExtension(path).ToLower();
            switch (extention)
            {
                case ".xyz": 
                    this.dataGridView1.DataSource = new IdXyzReader(path).ReadAll();
                    break;
                case ".lbh":
                    this.dataGridView1.DataSource = new IdGeoCoordReader(path).ReadAll();
                    break;
                case ".floatAmbiguities":
                    this.dataGridView1.DataSource = new VectorNameReader(path).ReadAll();
                    break;
                default:
                    MessageBox.Show("暂不支持！");
                    break;
            }
        }
        private void button_selectedRead_Click(object sender, EventArgs e)
        {
            string path = this.textBox_Path.Text;
            if (!File.Exists(path))
            {
                MessageBox.Show("指定文件不存在！"); return;
            }

            int startIndex = int.Parse(this.textBox_startIndex.Text);
            int count = int.Parse(this.textBox_count.Text); 

            var extention = Path.GetExtension(path).ToLower();
            switch (extention)
            {
                case ".xyz":
                    this.dataGridView1.DataSource = new IdXyzReader(path).Read(startIndex, count);
                    break;
                case ".lbh":
                    this.dataGridView1.DataSource = new IdGeoCoordReader(path).Read(startIndex, count);
                    break;
                case ".floatAmbiguities":
                    this.dataGridView1.DataSource = new VectorNameReader(path).Read(startIndex, count);
                    break;
                default:
                    MessageBox.Show("暂不支持！");
                    break;
            }
        }

        DateTime from, to;

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            //if (ShowLayer != null && this.bindingSource1.DataSource != null)
            //{ 
            //    List<LonLat> lonlats = new List<LonLat>();
            //    foreach (ClockSolnStation path in _clockFile.Header.ClockSolnStations)
            //    {
            //        lonlats.Add(path.GeoCoord);
            //    }
            //    Layer layer = LayerFactory.CreatePointLayer(lonlats);
            //    ShowLayer(layer);
            //}
        }

        private void ClockViewerForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Text = Setting.GnsserConfig.SampleXyzFile;
        }
    }
}
