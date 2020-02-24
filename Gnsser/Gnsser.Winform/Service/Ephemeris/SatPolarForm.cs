//2018.08.26, czs, create in HMX, 绘图工具，为论文而生

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting.Data;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser.Data.Rinex;
using System.IO;
using Geo.Times;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Data;
using Geo.Referencing;
using AnyInfo;
using Geo;
using Gnsser.Service;
using Geo.Winform;

namespace Gnsser.Winform
{
    public partial class SatPolarForm : Form
    {
        public SatPolarForm()
        {
            InitializeComponent();
        }
        ObjectTableStorage TableObject { get; set; }
        IEphemerisService EphemerisService { get; set; }
        List<SatelliteNumber> Prns { get; set; }
        double cutOff { get; set; }
        XYZ siteXyz { get; set; }

        private void button_run_Click(object sender, EventArgs e)
        {
            EphemerisService = GlobalNavEphemerisService.Instance;
            var prnStrs = namedStringControl_prn.GetValue();
            Prns = SatelliteNumber.ParsePRNsBySplliter(prnStrs);
            cutOff = namedFloatControl1AngleCut.GetValue();
            siteXyz = XYZ.Parse(this.namedStringControl_coord.GetValue());

            TableObject = new ObjectTableStorage();


            Geo.TimeLooper looper = timeLoopControl1.GetTimeLooper();
            looper.Looping += Looper_Looping;
            looper.Completed += Looper_Completed;

            looper.Run();
            looper.Complete();
             

            var indexName = TableObject.GetIndexColName();

            var chart = chart1.ChartAreas[0];
            chart.BorderDashStyle = ChartDashStyle.Dash;
            chart.BorderWidth = 1; 
            //chart.AxisY.LineWidth = 5;
            chart.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            var seriesDic = new BaseDictionary<SatelliteNumber, Series>("卫星雷达图", 
                new Func<SatelliteNumber, Series>(key=>
                {
                    Series series = new Series(key.ToString());
                    series.ChartType = SeriesChartType.Polar;
                    series.YValueType = ChartValueType.Double;
                    series.XValueType = ChartValueType.Double;
                    //series["PolarDrawingStyle"] = "Line";
                    //series["PolarDrawingStyle"] = "Line";
                    series.MarkerSize = 5;
                    series.MarkerStyle = MarkerStyle.Circle;

                    series.MarkerSize = 5;
                    series.BorderWidth = 5;

                    return series;

                }));

            foreach ( var row in TableObject.BufferedValues)
            {
                var rowData = new BaseDictionary<SatelliteNumber, DataPoint>("", m=> new DataPoint());
                foreach (var item in row)
                {
                    if(item.Key == indexName) { continue; }
                    var prn = SatelliteNumber.Parse(item.Key);

                    var valName = item.Key;
                    if (valName.Contains(Elevation))
                    {
                        rowData.GetOrCreate(prn).SetValueY(item.Value);
                    }
                    if (valName.Contains(Azimuth))
                    {
                        rowData.GetOrCreate(prn).XValue =Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                    }
                }
                 
                foreach (var kv in rowData.Data)
                {
                    seriesDic.GetOrCreate(kv.Key) .Points.Add(kv.Value);
                }
            }

            chart1.Series.Clear();
            foreach (var item in seriesDic)
            {
                chart1.Series.Add(item);
            }

            CommonChartForm form = new CommonChartForm();
            var char1 = form.Chart.ChartAreas[0];
            char1.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            int defaulFontSize = 14;
            form.Chart.Series.Clear();
            var chart0 = form.Chart.ChartAreas[0];
            chart0.AxisX.IsLabelAutoFit = true;
            chart0.AxisX.LabelAutoFitMinFontSize = defaulFontSize;
            chart0.AxisY.IsLabelAutoFit = true;
            chart0.AxisY.LabelAutoFitMinFontSize = defaulFontSize;
            chart0.AxisX.LabelStyle.Font = new Font(FontSettingOption.DefaultFontFamily, defaulFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            chart0.AxisY.LabelStyle.Font = new Font(FontSettingOption.DefaultFontFamily, defaulFontSize, FontStyle.Regular, GraphicsUnit.Pixel); 
            form.Chart.Legends[0].AutoFitMinFontSize = defaulFontSize;

            foreach (var item in seriesDic)
            {
                form.Chart.Series.Add(item);
            }
            form.Show();

        }
        const string Elevation = "Elevation";
        const string Azimuth = "Azimuth";

        private void Looper_Completed(object sender, EventArgs e)
        {
            this.objectTableControl1.DataBind(TableObject);
        }

        private void Looper_Looping(Geo.Times.Time time)
        {

            TableObject.NewRow();

            TableObject.AddItem("Epoch", time);

            string prefix = "";
            int satCount = Prns.Count;
            foreach (var prn in Prns)
            {

                var eph = EphemerisService.Get(prn, time);
                if (eph == null)
                {
                    continue;
                }

                var satXyz = eph.XYZ;

                var polar = CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                if (cutOff > polar.Elevation)
                {
                    continue;
                }


                prefix = prn.ToString() + "_"; 
                TableObject.AddItem(prefix + "Elevation", polar.Elevation);
                TableObject.AddItem(prefix + "Azimuth", polar.Azimuth);
            }
        }

        private void button_coordSet_Click(object sender, EventArgs e)
        {
            CoordSelectForm form = new CoordSelectForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.namedStringControl_coord.SetValue(form.XYZ.ToString());
            }
        }

        private void SatPolarForm_Load(object sender, EventArgs e)
        {
            this.namedStringControl_coord.SetValue("-2604187.7098  4479033.8061  3707506.4372");
            var gpsPrns = Geo.Utils.StringUtil.ToString(SatelliteNumber.GpsPrns, ",");

            namedStringControl_prn.SetValue(gpsPrns);
        }
    }

    /// <summary>
    /// 站星位置计算器
    /// </summary>
    public class SatPolarCalculator
    {
        public SatPolarCalculator(XYZ siteXyz, List<SatelliteNumber> Prns, TimePeriod timePeriod, double stepSecond=30, double cutOff= 5)
        {
            this.siteXyz = siteXyz;
            this.Prns = Prns;
            this.cutOff = cutOff;
            EphemerisService = GlobalNavEphemerisService.Instance;
            looper = new TimeLooper(timePeriod, stepSecond);

            looper.Looping += Looper_Looping;
            looper.Completed += Looper_Completed;

        }
        ObjectTableStorage TableObject { get; set; }
        IEphemerisService EphemerisService { get; set; }
        List<SatelliteNumber> Prns { get; set; }
        double cutOff { get; set; }
        XYZ siteXyz { get; set; }
        Geo.TimeLooper looper { get; set; }

        public ObjectTableStorage BuildTable(  )
        {
            TableObject = new ObjectTableStorage();
            looper.Run();
            looper.Complete();
            return TableObject; 
        }

        public static void ShowChartForm(ObjectTableStorage TableObject)
        {
            var indexName = TableObject.GetIndexColName();
            var seriesDic = new BaseDictionary<SatelliteNumber, Series>("卫星雷达图",
                new Func<SatelliteNumber, Series>(key =>
                {
                    Series series = new Series(key.ToString());
                    series.ChartType = SeriesChartType.Polar;
                    series.YValueType = ChartValueType.Double;
                    series.XValueType = ChartValueType.Double;
                    //series["PolarDrawingStyle"] = "Line";
                    //series["PolarDrawingStyle"] = "Line";
                    series.MarkerSize = 5;
                    series.MarkerStyle = MarkerStyle.Circle;

                    series.MarkerSize = 5;
                    series.BorderWidth = 5;

                    return series;

                }));

            foreach (var row in TableObject.BufferedValues)
            {
                var rowData = new BaseDictionary<SatelliteNumber, DataPoint>("", m => new DataPoint());
                foreach (var item in row)
                {
                    if (item.Key == indexName) { continue; }
                    var prn = SatelliteNumber.Parse(item.Key);

                    var valName = item.Key;
                    if (valName.Contains(Elevation))
                    {
                        rowData.GetOrCreate(prn).SetValueY(item.Value);
                    }
                    if (valName.Contains(Azimuth))
                    {
                        rowData.GetOrCreate(prn).XValue = Geo.Utils.ObjectUtil.GetNumeral(item.Value);
                    }
                }

                foreach (var kv in rowData.Data)
                {
                    seriesDic.GetOrCreate(kv.Key).Points.Add(kv.Value);
                }
            }

            CommonChartForm form = new CommonChartForm();
            form.Text = TableObject.Name;
            var char1 = form.Chart.ChartAreas[0];
            char1.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            int defaulFontSize = 14;
            form.Chart.Series.Clear();
            var chart0 = form.Chart.ChartAreas[0];
            chart0.AxisX.IsLabelAutoFit = true;
            chart0.AxisX.LabelAutoFitMinFontSize = defaulFontSize;
            chart0.AxisY.IsLabelAutoFit = true;
            chart0.AxisY.LabelAutoFitMinFontSize = defaulFontSize;
            chart0.AxisX.LabelStyle.Font = new Font(FontSettingOption.DefaultFontFamily, defaulFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            chart0.AxisY.LabelStyle.Font = new Font(FontSettingOption.DefaultFontFamily, defaulFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            form.Chart.Legends[0].AutoFitMinFontSize = defaulFontSize;

            foreach (var item in seriesDic)
            {
                form.Chart.Series.Add(item);
            }
            form.Show();
        }

        const string Elevation = "Elevation";
        const string Azimuth = "Azimuth";

        private void Looper_Completed(object sender, EventArgs e)
        {
        }

        private void Looper_Looping(Geo.Times.Time time)
        {
            TableObject.NewRow();
            TableObject.AddItem("Epoch", time);
            string prefix = "";
            int satCount = Prns.Count;
            foreach (var prn in Prns)
            {
                var eph = EphemerisService.Get(prn, time);
                if (eph == null)
                {
                    continue;
                }
                var satXyz = eph.XYZ;
                var polar = CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                if (cutOff > polar.Elevation)
                {
                    continue;
                }
                prefix = prn.ToString() + "_";
                TableObject.AddItem(prefix + "Elevation", polar.Elevation);
                TableObject.AddItem(prefix + "Azimuth", polar.Azimuth);
            }
        }       

    }

}
