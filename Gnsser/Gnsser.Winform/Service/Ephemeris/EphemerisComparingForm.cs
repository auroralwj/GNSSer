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
    /// 星历数据对比。
    /// </summary>
    public partial class EphemerisComparingForm : Form, Gnsser.Winform.IShowLayer
    {
        public EphemerisComparingForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;

        private void button_getPath_Click(object sender, EventArgs e) { if (this.openFileDialog_nav.ShowDialog() == DialogResult.OK)    this.textBox_pathA.Text = openFileDialog_nav.FileName; }
        private void button_getPathB_Click(object sender, EventArgs e) { if (this.openFileDialog_nav.ShowDialog() == DialogResult.OK) this.textBox_pathB.Text = openFileDialog_nav.FileName; }

        FileEphemerisService ephemerisA;
        FileEphemerisService ephemerisB;
        private void button_read_Click(object sender, EventArgs e)
        {
            try
            {
                ephemerisA = EphemerisDataSourceFactory.Create(this.textBox_pathA.Text, FileEphemerisType.Unkown, false);
                ephemerisB = EphemerisDataSourceFactory.Create(this.textBox_pathB.Text, FileEphemerisType.Unkown, false);
                List<SatelliteNumber> commonPrns = ephemerisA.Prns.FindAll(m => ephemerisB.Prns.Contains(m));

                if (commonPrns.Count == 0)
                {
                    Geo.Utils.FormUtil.ShowWarningMessageBox("没有共同卫星，无法比较！");
                }

                this.bindingSource_prns.DataSource = commonPrns;
                this.bindingSource1.DataSource = ephemerisA.Gets();
                this.bindingSource2.DataSource = ephemerisB.Gets();
                //设置时间间隔
                this.dateTimePicker_from.Value = ephemerisA.TimePeriod.Start.DateTime > ephemerisB.TimePeriod.Start.DateTime ? ephemerisA.TimePeriod.Start.DateTime : ephemerisB.TimePeriod.Start.DateTime;
                this.dateTimePicker_to.Value = ephemerisA.TimePeriod.End.DateTime < ephemerisB.TimePeriod.End.DateTime ? ephemerisA.TimePeriod.End.DateTime : ephemerisB.TimePeriod.End.DateTime;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button_toExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1); }

        List<Ephemeris> sortedRecordsA;
        List<Ephemeris> sortedRecordsB;
        List<Ephemeris> sortedRecordsC = new List<Ephemeris>();

        private void button_show_Click(object sender, EventArgs e)
        {
            if (ephemerisA == null) { Geo.Utils.FormUtil.ShowErrorMessageBox("读入数据 A 先！"); return; }
            if (ephemerisB == null) { Geo.Utils.FormUtil.ShowErrorMessageBox("读入数据 B 先！"); return; }

            if(this.comboBox_prn.SelectedItem == null){
                Geo.Utils.FormUtil.ShowWarningMessageBox("没有共同卫星，无法比较！");
                return;
            }

            SatelliteNumber PRN = SatelliteNumber.Parse(this.comboBox_prn.SelectedItem.ToString());
            //每次取10个数计算
            sortedRecordsA = ephemerisA.Gets(PRN);
            sortedRecordsB = ephemerisB.Gets(PRN);//, new Time(this.TimeFrom.Value), new Time(this.TimeTo.Value));

            this.bindingSource1.DataSource = sortedRecordsA;
            this.bindingSource2.DataSource = sortedRecordsB;
            this.bindingSource3.DataSource = GetDifferEphemeirs(sortedRecordsA, sortedRecordsB);
        }


        private void button_inter_Click(object sender, EventArgs e)
        {
            if (sortedRecordsA != null && sortedRecordsA.Count != 0 && sortedRecordsB != null && sortedRecordsB.Count != 0)
            {
                double interval = Double.Parse(this.textBox_interval.Text);
                SatelliteNumber PRN = SatelliteNumber.Parse(this.comboBox_prn.SelectedItem.ToString());
                List<Ephemeris> fitedResultA = new List<Ephemeris>();
                List<Ephemeris> fitedResultB = new List<Ephemeris>();
                Time start = new Time(this.dateTimePicker_from.Value);


                if (ephemerisA is Geo.Algorithm.IInterpolationCaller)
                {
                    Geo.Algorithm.IInterpolationCaller a = ephemerisA as Geo.Algorithm.IInterpolationCaller;
                    a.Order = int.Parse(this.textBox_A_count.Text);
                    if (this.radioButton_A_cheb.Checked)
                        a.InterpolationType = InterpolationType.ChebyshevPolyFit;
                    else a.InterpolationType = InterpolationType.LagrangeInterplation;
                }
                if (ephemerisB is Geo.Algorithm.IInterpolationCaller)
                {
                    Geo.Algorithm.IInterpolationCaller b = ephemerisB as Geo.Algorithm.IInterpolationCaller;
                    b.Order = int.Parse(this.textBox_B_count.Text);
                    if (this.radioButton_B_cheb.Checked)
                        b.InterpolationType = InterpolationType.ChebyshevPolyFit;
                    else b.InterpolationType = InterpolationType.LagrangeInterplation;
                }

                double cacuCount = (this.dateTimePicker_to.Value - this.dateTimePicker_from.Value).TotalSeconds / interval;
                for (int xi = 0; xi < cacuCount; xi++)
                {
                    Time gpsTime = start + interval * xi;
                    Ephemeris eA = ephemerisA.Get(PRN, gpsTime);
                    Ephemeris eB = ephemerisB.Get(PRN, gpsTime);
                    fitedResultA.Add(eA);
                    fitedResultB.Add(eB);
                }
                this.bindingSource1.DataSource = fitedResultA;
                this.bindingSource2.DataSource = fitedResultB;
                this.bindingSource3.DataSource = GetDifferEphemeirs(fitedResultA, fitedResultB);
            }
        }

        private List<Ephemeris> GetDifferEphemeirs(List<Ephemeris> ephsA, List<Ephemeris> ephsB)
        {
            List<Ephemeris> sortedRecordsC = new List<Ephemeris>();
            List<Ephemeris> sortedRecords = ephsA.FindAll(m => ephsB.Exists(n => n.Prn.Equals(m.Prn) && n.Time.Equals(m.Time)));
            foreach (var item in sortedRecords)
            {
                Ephemeris epheA = ephsA.Find(n => n.Prn.Equals(item.Prn) && n.Time.Equals(item.Time));
                Ephemeris epheB = ephsB.Find(n => n.Prn.Equals(item.Prn) && n.Time.Equals(item.Time));
                sortedRecordsC.Add(new Ephemeris()
                {
                    Prn = item.Prn,
                    Time = item.Time,
                    ClockBias = epheA.ClockBias - epheB.ClockBias,
                    ClockDrift = epheA.ClockDrift - epheB.ClockDrift,
                    RelativeCorrection = epheA.RelativeCorrection - epheB.RelativeCorrection,
                    XYZ = epheA.XYZ - epheB.XYZ,
                    XyzDot = epheA.XyzDot - epheB.XyzDot, 
                });
            }
            return sortedRecordsC;
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.bindingSource1.DataSource != null)
            {
                List<Ephemeris> fitedResult = this.bindingSource1.DataSource as List<Ephemeris>;
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                foreach (Ephemeris item in fitedResult)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point(item.GeoCoord, item.Prn + " " + item.Time.ToString()));
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void button_BtoExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView2); }
        private void button_CtoExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView3); }

        private void EphemerisComparingForm_Load(object sender, EventArgs e)
        {

        }

        private void button_darwLines_Click(object sender, EventArgs e)
        {
            if (this.bindingSource3.DataSource != null)
            {
              var list = this.bindingSource3.DataSource as List<Ephemeris>;

              Geo.ObjectTableStorage table = new Geo.ObjectTableStorage("比较结果");
              foreach (var item in list)
              {
                  table.NewRow();

                  table.AddItem("Epoch", item.Time.DateTime);
                  table.AddItem("Dx", item.XYZ.X);
                  table.AddItem("Dy", item.XYZ.Y);
                  table.AddItem("Dz", item.XYZ.Z);
                  table.AddItem("Dc", item.ClockBias);


                  table.EndRow();
              }

              this.paramVectorRenderControl1.SetTableTextStorage(table);
              this.paramVectorRenderControl1.DrawParamLines();
            }





        }

    }
}
