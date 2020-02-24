//2018.12.26, czs, create in ryd, 增加时段图绘制
//2019.03.15, czs, edit in hongqing, 修复批量的卫星计算问题，增加批量拟合

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
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
using Geo.IO;
using Geo.Draw;
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 拟合星历服务。
    /// </summary>
    public partial class SingleFileEphemerisServiceForm : Form, Gnsser.Winform.IShowLayer
    {
        Log log = new Log(typeof(SingleFileEphemerisServiceForm));
        public SingleFileEphemerisServiceForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;

         FileEphemerisService dataSourse;
        private void button_read_Click(object sender, EventArgs e)
        {
            //try
            //{
            var path = this.fileOpenControl_eph.FilePath;
            if (!System.IO.File.Exists(path))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件不存在！" + path);
                return;
            }
            dataSourse = EphemerisDataSourceFactory.Create(path);
            this.dataGridView1.DataSource = bindingSource1;
            this.bindingSource1.DataSource = dataSourse.Gets();


            bindingSource_prn.DataSource = dataSourse.Prns;


            arrayCheckBoxControl_prns.Init(dataSourse.Prns);


            //设置时间间隔
            this.timePeriodControl1.SetTimePerid(dataSourse.TimePeriod); 
            //}
            //catch (Exception ex)
            //{
            //    Geo.Utils.FormUtil.HandleException(ex, ex.Message);
            //}
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Path.GetFileName(path) + "，卫星数量 ： " + dataSourse.Prns.Count);

            var types = SatelliteNumber.GetSatTypes(dataSourse.Prns);
            types.Sort();
            foreach (var type in types)
            {
                var prns = dataSourse.GetPrns(type);
                prns.Sort();
                sb.AppendLine(type + " ，共" + prns.Count + " 颗：" + Geo.Utils.EnumerableUtil.ToString(prns, ","));
            }

            log.Info(sb.ToString());
        }

        private void button_toExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1); }

        DateTime TimeFrom { get { return this.timePeriodControl1.TimeFrom; } }
        DateTime TimeTo { get { return this.timePeriodControl1.TimeTo; } }
        double Interval { get { return Double.Parse(this.textBox_interval.Text); } }
        SatelliteNumber Prn { get { return (SatelliteNumber)this.comboBox_prn.SelectedItem; } }
        private void button_show_Click(object sender, EventArgs e)
        {   
            this.bindingSource1.DataSource = dataSourse.Gets(Prn, Time.Parse(TimeFrom), Time.Parse(TimeTo));
        }

        private void button_inter_Click(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = dataSourse.Gets(Prn, Time.Parse(TimeFrom), Time.Parse(TimeTo), Interval);
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && this.bindingSource1.DataSource != null)
            {
                List<Ephemeris> fitedResult = this.bindingSource1.DataSource as List<Ephemeris>;
                List<AnyInfo.Geometries.Point> lonlats = new List< AnyInfo.Geometries.Point >();
                
                foreach (var item in fitedResult)
                {
                    var pt = new AnyInfo.Geometries.Point(item.GeoCoord, item.Time.ToString(), item.Prn.ToString());
                    lonlats.Add(pt);
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void SingleFileEphemerisServiceForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_eph.FilePath = Setting.GnsserConfig.SampleSP3File;
            namedStringControl_coord.SetValue("1000000, 1000000, 1000000");

            this.ColumnTime.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.Time);// "Time"; 
            this.ColumnPrn.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.Prn);// "PRN"; 
            this.ColumnXYZ.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.XYZ);// "Xyz"; 
            this.ColumnGeoCoord.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.GeoCoord);// "GeoCoord";
            this.ColumnXyzDot.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.XyzDot);//= "Xyzdot";
            this.ColumnClockBias.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.ClockBias);//= "ClockBias";
            this.ColumnClockDrift.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.ClockDrift);//= "ClockDrift";
            this.ColumnDriftRate.DataPropertyName = Geo.Utils.ObjectUtil.GetPropertyName<Ephemeris>(m => m.DriftRate);// "DriftRate";
        }

        private void button_multSelect_Click(object sender, EventArgs e)
        {
            var prns = arrayCheckBoxControl_prns.GetSelected<SatelliteNumber>();
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var prn in prns)
            {
                list.AddRange( dataSourse.Gets(prn, Time.Parse(TimeFrom), Time.Parse(TimeTo)));
            }
            this.bindingSource1.DataSource = list;  
        }

        private void button_drawPhaseChart_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<TimePeriod>> timeperiods = new Dictionary<string, List<TimePeriod>>();
            var siteXYZ = XYZ.Parse(namedStringControl_coord.GetValue());
            var angleCut = namedFloatControl_angleCut.GetValue();
            var prns = dataSourse.Prns;
            var from = Time.Parse(TimeFrom);
            var to = Time.Parse(TimeTo);
            var interval = (to - from) / 1000;
            ObjectTableStorage table = new ObjectTableStorage("时段图");
            for (var time = from; time <= to; time = time + interval)
            {
                table.NewRow();
                table.AddItem("Epoch", time);
                foreach (var prn in prns)
                {
                    //  angle
                    var eph = dataSourse.Get(prn, time);
                    var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(eph.XYZ, siteXYZ);
                    if (polar.Elevation < angleCut) { continue; }

                    table.AddItem(prn.ToString(), true);
                }
            }
            EpochChartForm chartForm = new EpochChartForm(table);
            chartForm.Show();
        }

        private void button_coordSet_Click(object sender, EventArgs e)
        {
            CoordSelectForm form = new CoordSelectForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.namedStringControl_coord.SetValue(form.XYZ.ToString());
            }
        }

        private void fileOpenControl_eph_FilePathSetted(object sender, EventArgs e)
        {
            var path = this.fileOpenControl_eph.FilePath;
            if (!System.IO.File.Exists(path))
            {
                return;
            }
            dataSourse = EphemerisDataSourceFactory.Create(path);
            this.timePeriodControl1.SetTimePerid(dataSourse.TimePeriod);

        }

        private void button_interMulti_Click(object sender, EventArgs e)
        {
            var prns = arrayCheckBoxControl_prns.GetSelected<SatelliteNumber>();
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var prn in prns)
            {
                list.AddRange(dataSourse.Gets(prn, Time.Parse(TimeFrom), Time.Parse(TimeTo), Interval));
            }
            this.bindingSource1.DataSource = list;

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
