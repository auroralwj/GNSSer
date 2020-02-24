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
    public partial class ClockComparingForm : Form, Gnsser.Winform.IShowLayer
    {
         
        public event ShowLayerHandler ShowLayer;  

        public ClockComparingForm()
        {
            InitializeComponent();
        } 
        ClockFile _clockFile;

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                this.textBox_Path.Text = this.openFileDialog1.FileName; 
        }

        private void button_setSp3Path_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_sp3Path.Text = this.openFileDialog1.FileName; 
        }   
        
        SingleSp3FileEphService sp3File;

        private void button_read_Click(object sender, EventArgs e)
        {
            string clkPath = this.textBox_Path.Text;
            string sp3Path = this.textBox_sp3Path.Text;
            if (!File.Exists(clkPath) || !File.Exists(sp3Path))
            {
                MessageBox.Show("指定文件不存在！"); return;
            }

            ClockFileReader reader = new ClockFileReader(clkPath,false);
            _clockFile = reader.ReadAll();
            if (_clockFile.ClockCount == 0) return;

             
            sp3File = new SingleSp3FileEphService(sp3Path);

            this.dateTimePicker_from.Value = _clockFile.First.First().Time.DateTime;
            this.dateTimePicker_to.Value = this.dateTimePicker_from.Value + TimeSpan.FromHours(6);

            List<String> prns = new List<string>();
            foreach (SatelliteNumber prn in _clockFile.Header.PrnList) prns.Add(prn.ToString());
            this.comboBox_name.DataSource = prns;

            this.bindingSource_clk.DataSource =  _clockFile.AllItems;
            this.bindingSource_sp3.DataSource = sp3File.Gets();
        }

        private void button_to_excel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView_clk); }
        DateTime from, to;
        List<AtomicClock> Clocks;
        List<Ephemeris> Ephemerides;
        private void button_filter_Click(object sender, EventArgs e)
        {
            if (this.comboBox_name.SelectedItem == null)
            {
                MessageBox.Show("请先读取数据。"); return; ;
            }
            string name = this.comboBox_name.SelectedItem.ToString();
            from = this.dateTimePicker_from.Value;
            to = this.dateTimePicker_to.Value;
            Clocks = _clockFile.GetClockItems(name, Time.Parse(from), Time.Parse(to));
            this.bindingSource_clk.DataSource = Clocks;

            this.Ephemerides = sp3File.Gets(SatelliteNumber.Parse(name), Time.Parse(from), Time.Parse(to));

            this.bindingSource_sp3.DataSource = Ephemerides;

            ShowComparing();
        }

        private void ShowComparing()
        {
            //比较
            List<ClockLite> compares = new List<ClockLite>();

            foreach (var item in Clocks)
            {
                Ephemeris eph = Ephemerides.Find(m => m.Time.Equals(item.Time));
                if (eph == null) continue;

                ClockLite c = new ClockLite()
                {
                    Prn = item.Prn == null ? SatelliteNumber.Parse( item.Name) : item.Prn,
                    GpsTime = item.Time,
                    Offset = item.ClockBias - eph.ClockBias,
                    Drift = item.ClockDrift - eph.ClockDrift
                };
                c.Distance = c.Offset * GnssConst.LIGHT_SPEED;
                compares.Add(c);
            }

            this.bindingSource_comparing.DataSource = compares;
        }

        public class ClockLite
        {
            public SatelliteNumber Prn { get; set; }
            public Time GpsTime { get; set; }

            public double Offset { get; set; }
            public double Drift { get; set; }
            public double Distance { get; set; }
        }


        private void button_inter_Click(object sender, EventArgs e)
        {
            double interval = double.Parse(this.textBox_interval.Text);
            List<AtomicClock> sortedRecords = this.bindingSource_clk.DataSource as List<AtomicClock>;
            if (sortedRecords == null)
            {
                MessageBox.Show("请先读取,并删选数据。"); return; ;
            }

        //    Data.ClockDataSource datasource = new Data.ClockDataSource(_clockFile);

            ClockInterpolator interp = new ClockInterpolator(sortedRecords,2);

            this.Clocks = interp.GetAtomicClocks( Time.Parse(from), Time.Parse(to), interval);

            this.bindingSource_clk.DataSource = this.Clocks;
            string name = this.comboBox_name.SelectedItem.ToString();
            SatelliteNumber prn = SatelliteNumber.Parse(name);

            this.Ephemerides = sp3File.Gets(SatelliteNumber.Parse(name), Time.Parse(from), Time.Parse(to), interval);                
            this.bindingSource_sp3.DataSource = Ephemerides;

            ShowComparing();
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.bindingSource_clk.DataSource != null)
            { 
                List<LonLat> lonlats = new List<LonLat>();
                foreach (ClockSolnStation item in _clockFile.Header.ClockSolnStations)
                {
                    lonlats.Add(item.GeoCoord);
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void ClockViewerForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Text = @"./Data/GNSS/Rinex/2002.05.23_143_11674_V2TwoSite/igs11674.clk";
            this.textBox_sp3Path.Text = @"./Data/GNSS/Rinex/2002.05.23_143_11674_V2TwoSite/igs11674.sp3";
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
