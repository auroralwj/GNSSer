//2019.03.29, czs, create in hmx, 大地问题解算

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
    public partial class GeodeticAzimuthForm : Form
    {
        public GeodeticAzimuthForm()
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
        bool IsOutSplitByTab => checkBox_IsOutSplitByTab.Checked;

        private void button_gaussToLonlat_Click(object sender, EventArgs e)
        {
            var ellipsoid = Ellipsoid;
            var startsLines = this.richTextBoxControl_starts.Lines;
            var endsLines = this.richTextBoxControl_ends.Lines;
            var inputsStarts = new List<XYZ>();
            foreach (string item in startsLines)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                inputsStarts.Add(XYZ.Parse(item));
            }
            var inputsEnds = new List<XYZ>();
            foreach (string item in endsLines)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                inputsEnds.Add(XYZ.Parse(item));
            }
            var results = new List<double>();
            int i = 0;
            foreach (var item in inputsStarts)
            {
                var start = item;
                var end = inputsEnds[i];

               var longlat = Coordinates.GeodeticUtils.BesselAzimuthAngle(start, end, ellipsoid.SemiMajorAxis, ellipsoid.InverseFlattening);
                results.Add(longlat);

                i++;
            }
            var spliter = IsOutSplitByTab? "\t":", ";
            StringBuilder sb = new StringBuilder();
            foreach (var item in results)
            { 
                sb.AppendLine(item.ToString("0.0000000000") + " = " + new DMS(item).ToReadableDms());
            }
            this.richTextBoxControl_lonlat.Text = sb.ToString();
        }

    }
}
