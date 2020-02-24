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
using Geo.Coordinates;
using Geo.Times; 

namespace Gnsser.Winform
{
    public partial class SP3ViewerForm : Form
    {
        public SP3ViewerForm()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_sp3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_Path.Text = openFileDialog_sp3.FileName;
                button_read_Click( sender,  e);
            }
        }
        List<SP3ViewEntity> entities { get; set; }
        Sp3File sp3;
        private void button_read_Click(object sender, EventArgs e)
        {
            entities = new List<SP3ViewEntity>();
            string path = this.textBox_Path.Text;
            Sp3Reader r = new Sp3Reader(path);
            sp3 = r.ReadAll();

            this.textBox_headInfo.Text = sp3.Header.ToString();

          
            List<SatelliteNumber> Prns = new List<SatelliteNumber>();
            foreach (Sp3Section sec in sp3)
            {
                foreach (var rec in sec)
                {
                    if (!Prns.Contains(rec.Prn)) Prns.Add(rec.Prn);

                    SP3ViewEntity ent = new SP3ViewEntity();
                    ent.Time = sec.Time;

                    ent.PRN = rec.Prn;
                    ent.X = rec.XYZ.X;
                    ent.Y = rec.XYZ.Y;
                    ent.Z = rec.XYZ.Z;

                    GeoCoord coord = CoordTransformer.XyzToGeoCoord(rec.XYZ);
                    ent.Lon = coord.Lon;
                    ent.Lat = coord.Lat;
                    ent.Height = coord.Height;

                    if (rec.XyzDot != null)
                    {
                        ent.Xdot = rec.XyzDot.X;
                        ent.Ydot = rec.XyzDot.Y;
                        ent.Zdot = rec.XyzDot.Z;
                    }
                    ent.ClockRate = rec.ClockDrift;
                    ent.Clock = rec.ClockBias;

                    entities.Add(ent);
                }
            }
            this.bindingSource_prns.DataSource = Prns;
            this.bindingSource1.DataSource = entities;

            ////设置时间间隔
            this.dateTimePicker_from.Value = entities[0].Time.DateTime;
            this.dateTimePicker_to.Value = entities[entities.Count - 1].Time.DateTime;
        }

        private void button_toExcel_Click(object sender, EventArgs e)
        {
            Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1);
        }

        private void button_show_Click(object sender, EventArgs e)
        {
            if (this.bindingSource1.DataSource == null) { Geo.Utils.FormUtil.ShowErrorMessageBox("读入数据先！"); return; }

            SatelliteNumber PRN = SatelliteNumber.Parse(this.comboBox_prn.SelectedItem.ToString());
            //每次取10个数计算
          // List<SP3ViewEntity> colName =  this.bindingSource1.DataSource as List<SP3ViewEntity>;
           var list = entities.FindAll(m =>
                m.PRN.Equals(PRN)
                && m.Time.DateTime >=  this.dateTimePicker_from.Value
                && m.Time.DateTime <= this.dateTimePicker_to.Value);

            if (list.Count != 0)
            {
               // colName.Sort();
                ////设置时间间隔
                this.dateTimePicker_from.Value = list[0].Time.DateTime;
                this.dateTimePicker_to.Value = list[list.Count - 1].Time.DateTime;
            }
            this.bindingSource1.DataSource = list;
        }

        private void SP3ViewerForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Text = Setting.GnsserConfig.SampleSP3File;
        }

        private void button_Convert_Click(object sender, EventArgs e)
        {
            var path = System.IO.Path.Combine(Setting.GnsserConfig.TempDirectory, System.IO.Path.GetFileName(this.textBox_Path.Text));

            System.IO.File.WriteAllText(path, Sp3Writer.BuidSp3V3String(sp3));
            Geo.Utils.FormUtil.ShowOkAndOpenFile(path); 
        }       
    }

    public class SP3ViewEntity : IComparable<SP3ViewEntity>
    {
        public SatelliteNumber PRN { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Lon { get; set; }
        public double Lat { get; set; }
        public double Height { get; set; }

        public double Xdot { get; set; }
        public double Ydot { get; set; }
        public double Zdot { get; set; }

        public double Clock { get; set; }
        public double ClockRate { get; set; }

        /// <summary>
        /// microsec
        /// </summary>
        public Time Time { get; set; }


        public int CompareTo(SP3ViewEntity other)
        {
            return (int)this.Time.CompareTo(other.Time);
        }
    }
}
