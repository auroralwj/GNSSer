using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Gnsser.Interoperation.Bernese;
using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;

namespace Gnsser.Winform
{
    public partial class BerCrdViewForm : Form, IShowLayer
    {
        public BerCrdViewForm()
        {
            InitializeComponent();
        }

        public event ShowLayerHandler ShowLayer;
        CrdFile file;
        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_Path.Text = this.openFileDialog1.FileName;
                    Read();
            }

        }

        private void button_read_Click(object sender, EventArgs e)
        {
            Read();
        }

        private void Read()
        {
            string path = this.textBox_Path.Text;
            if (!File.Exists(path))
            {
                FormUtil.ShowFileNotExistBox(path);
                return;
            }
            file = CrdFile.Read(path);

            this.textBox_info.Text = "基准：" + file.Datum + "\r\n" + "历元：" + file.Epoch;
            this.bindingSource1.DataSource = file.Items;
        }

        private void button_toExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1); }


        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && file != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                foreach (CrdItem item in file.Items)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point( item.GeoCoord, item.StationName));
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void button_saveTofile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file.Save(saveFileDialog1.FileName);
                FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            }
        }

        private void button_merge_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    CrdFile fileB = CrdFile.Read(this.openFileDialog1.FileName);
                    file.AddRange(fileB);
                    this.bindingSource1.DataSource = file.Items;
                    this.bindingSource1.ResetBindings(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

    }
}
