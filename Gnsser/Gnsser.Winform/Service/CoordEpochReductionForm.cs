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
using Geo.Times; 
using Gnsser.Interoperation.Bernese;

using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;

namespace Gnsser.Winform
{
    public partial class CoordEpochReductionForm : Form, Gnsser.Winform.IShowLayer
    {
        public CoordEpochReductionForm()
        {
            InitializeComponent();
        }

        public event ShowLayerHandler ShowLayer;
        CrdFile oldCrdFile;
        CrdFile newCrdFile;
        VelFile velFile;
        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_crdPath.Text = this.openFileDialog1.FileName;
            }
        }
        private void button_setVelPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_velPath.Text = this.openFileDialog2.FileName;
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            Read();
        }

        private void Read()
        {
            string crdPath = this.textBox_crdPath.Text;
            string velPath = this.textBox_velPath.Text;
            if (!File.Exists(crdPath) || !File.Exists(velPath))
            {
                FormUtil.ShowFileNotExistBox(crdPath + "\r\n" + velPath);
                return;
            }
            oldCrdFile = CrdFile.Read(crdPath);
            velFile = VelFile.Read(velPath);
            Bind();
        }

        private void Bind()
        {
            this.textBox_info.Text = "基准：" + oldCrdFile.Datum + "\r\n" + "历元：" + oldCrdFile.Epoch;
            this.bindingSource_raw.DataSource = oldCrdFile.Items;

            this.bindingSource_vel.DataSource = velFile.Items;

        }

        private void button_toExcel_Click(object sender, EventArgs e) {
            DataGridView view = this.dataGridView1;
            if(this.tabControl1.SelectedTab == this.tabPage_old)
                view = this.dataGridView1;
            if (this.tabControl1.SelectedTab == this.tabPage_new)
                view = this.dataGridView2;
            if (this.tabControl1.SelectedTab == this.tabPage_vel)
                view = this.dataGridView4;
            Geo.Utils.ReportUtil.SaveToExcel(view);
        }


        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && oldCrdFile != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                foreach (CrdItem item in oldCrdFile.Items)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point(item.GeoCoord, item.StationName));
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void button_saveTofile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                newCrdFile.Save(saveFileDialog1.FileName);
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
                    oldCrdFile.AddRange(fileB);
                    this.bindingSource_raw.DataSource = oldCrdFile.Items;
                    this.bindingSource_raw.ResetBindings(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }


        private void button_calculate_Click(object sender, EventArgs e)
        {
            Time toEphoch = new Time(dateTimePicker_toEpoch.Value);
            TimeSpan span = toEphoch.DateTime - oldCrdFile.Epoch.DateTime;
            double differYear = span.TotalDays / 365.0;

            newCrdFile = new CrdFile()
            {
                DateString = DateTime.Now.ToString(),
                Datum = oldCrdFile.Datum,
                Epoch = toEphoch,
                Items = new List<CrdItem>(),
                Label = "Gnsser"                
            };
            foreach (var crdItem in oldCrdFile.Items)
            {
                VelItem vItem = velFile.Items.Find(m => m.StationName == crdItem.StationName);
                if (vItem == null)
                {
                    if (checkBox_ignoreError.Checked) continue;

                    if (Geo.Utils.FormUtil.ShowYesNoMessageBox("没有 " + crdItem.StationName + " 的速度信息。是否终止计算？")
                        == System.Windows.Forms.DialogResult.Yes)
                        break;
                    continue;
                }

                CrdItem newItem = crdItem.Clone();
                XYZ xyz = CrdItem.GetEpochReduction(vItem.Vxyz, differYear);
                newItem.Xyz += xyz;
                newCrdFile.Items.Add(newItem);
            }
            this.bindingSource_ehpched.DataSource = newCrdFile.Items;
        }


    }
}
