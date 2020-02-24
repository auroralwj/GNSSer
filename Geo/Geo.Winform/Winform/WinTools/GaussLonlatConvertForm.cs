//2019.01.10, czs, create in hmx, 高斯坐标转换

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo.Coordinates;
using Geo.Referencing;

namespace Geo
{
    public partial class GaussLonlatConvertForm : Form
    {
        public GaussLonlatConvertForm()
        {
            InitializeComponent();
        }
        private void GaussLonlatConvertForm_Load(object sender, EventArgs e)
        {
            enumRadioControl_angleUnit.Init<AngleUnit>();
            enumRadioControl_angleUnit.SetCurrent<AngleUnit>(AngleUnit.Degree);
        }
        AngleUnit AngleUnit => this.enumRadioControl_angleUnit.GetCurrent<AngleUnit>();
        Ellipsoid Ellipsoid => this.ellipsoidSelectControl1.Ellipsoid;
        bool IsWithBeltNum => this.checkBox_isWithBeltNum.Checked;
        int BeltWidth3Or6 => checkBox_is3Belt.Checked ? 3 : 6;
        double OrinalLonDeg => this.namedFloatControl_orinalLonDeg.GetValue();
        double AveGeoHeight => this.namedFloatControl_aveGeoHeight.GetValue();
        bool IsIndicateOriginLon => checkBox_indicated.Checked;
        /// <summary>
        /// Y加常数
        /// </summary>
        double YConst => namedFloatControlYConst.GetValue();
        bool IsOutSplitByTab => checkBox_IsOutSplitByTab.Checked;

        private void button_gaussToLonlat_Click(object sender, EventArgs e)
        {
            var strs = this.richTextBoxControl_gauss.Lines;
            var inputs = new List<XY>();
            foreach (string item in strs)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                inputs.Add(XY.Parse(item));
            } 
            var results = new List<LonLat>();
            foreach (var item in inputs)
            {
                var longlat = Coordinates.CoordTransformer.GaussXyToLonLat(item, AveGeoHeight, BeltWidth3Or6, OrinalLonDeg, YConst, Ellipsoid, AngleUnit);
                results.Add(longlat);
            }
            var spliter = IsOutSplitByTab? "\t":", ";
            StringBuilder sb = new StringBuilder();
            foreach (var item in results)
            { 
                sb.AppendLine(item.ToString("0.0000000000", spliter));
            }
            this.richTextBoxControl_lonlat.Text = sb.ToString();
        }

        private void button_lonlatToGauss_Click(object sender, EventArgs e)
        { 
            var strs = this.richTextBoxControl_lonlat.Lines;
            var inputs = new List<LonLat>();
            foreach (string item in strs)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                inputs.Add(LonLat.Parse(item));
            }
           
            var results = new List<XY>();
            double orinalLonDeg = this.namedFloatControl_orinalLonDeg.GetValue();
            foreach (var item in inputs)
            {
                var longlat = Coordinates.CoordTransformer.LonLatToGaussXy(item, AveGeoHeight, BeltWidth3Or6, ref orinalLonDeg, IsIndicateOriginLon, Ellipsoid,  YConst, AngleUnit, IsWithBeltNum);
                results.Add(longlat);
            }
            this.namedFloatControl_orinalLonDeg.SetValue(orinalLonDeg);
            var spliter = IsOutSplitByTab ? "\t" : ", ";
            StringBuilder sb = new StringBuilder();
            foreach (var item in results)
            {
                sb.AppendLine(item.ToString("0.000000", spliter));
            }
            this.richTextBoxControl_gauss.Text = sb.ToString(); 
        }

    }
}
