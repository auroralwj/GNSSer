//2019.01.10, czs, edit in hmx, 简化表达

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Common;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Times;


namespace Geo.WinTools
{
    /// <summary>
    /// 坐标转换
    /// </summary>
    public partial class GeoXyzConvertForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GeoXyzConvertForm()
        {
            InitializeComponent();
        }
        private void GeoXyzConvertForm_Load(object sender, EventArgs e)
        {
            enumRadioControl_angleUnit.Init<AngleUnit>();
            enumRadioControl_angleUnit.SetCurrent<AngleUnit>(AngleUnit.Degree);
        }
        AngleUnit AngleUnit => this.enumRadioControl_angleUnit.GetCurrent<AngleUnit>();
        Ellipsoid Ellipsoid => this.ellipsoidSelectControl1.Ellipsoid;

        bool IsOutSplitByTab => checkBox_IsOutSplitByTab.Checked;
        private void button_xyzTogeo_Click(object sender, EventArgs e)
        {
            try
            {
                string splliter = "\t";

                AngleUnit unit = AngleUnit;
                Geo.Referencing.Ellipsoid ellipsoid = Ellipsoid;

                List<XYZ> xyzs = new List<XYZ>();
                foreach (var item in this.textBox_xyz.Lines)
                {
                    if (item == "") continue;
                    xyzs.Add(XYZ.Parse(item));
                }
                StringBuilder sb = new StringBuilder();
                var spliter = IsOutSplitByTab ? "\t" : ", ";
                foreach (var item in xyzs)
                {
                    GeoCoord geeCoord = CoordTransformer.XyzToGeoCoord(item, ellipsoid, unit);
                    sb.AppendLine(geeCoord.ToString("0.0000000000", spliter));
                }
                this.textBox_geo.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_geoToxyz_Click(object sender, EventArgs e)
        {
            try
            {
                AngleUnit unit = AngleUnit;
                Geo.Referencing.Ellipsoid ellipsoid = Ellipsoid;

                List<GeoCoord> sources = new List<GeoCoord>();
                foreach (var item in this.textBox_geo.Lines)
                {
                    if (item == "") { continue; }

                    string[] strs = item.Split(new char[] { ',', '\t', ' ', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    DMS lon = DMS.Parse(strs[0], unit);
                    DMS lat = DMS.Parse(strs[1], unit);
                    double height = Double.Parse(strs[2]);

                    sources.Add(new GeoCoord(lon.Degrees, lat.Degrees, height, AngleUnit.Degree));
                }
                StringBuilder sb = new StringBuilder();

                var spliter = IsOutSplitByTab ? "\t" : ", ";
                foreach (var item in sources)
                {
                    XYZ xYZ = CoordTransformer.GeoCoordToXyz(item, ellipsoid);
                    sb.AppendLine(xYZ.ToString("0.0000000", spliter));
                }
                this.textBox_xyz.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
