using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Gnsser.Winform
{
    public partial class DegForamatConvertForm : Form
    {
        public DegForamatConvertForm()
        {
            InitializeComponent();
        }

        private void button_degreeToRad_Click(object sender, EventArgs e)
        {
            var degreesLines = textBox_degree.Lines;
            StringBuilder sb = new StringBuilder();
            foreach (var item in degreesLines)
            {
                if (String.IsNullOrWhiteSpace(item)) continue;

                var rad = DMS.Parse(item).Degrees;
                sb.AppendLine(rad + "");
            }

            textBox_rad.Text = sb.ToString();
        }

        private void button_radToDegree_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var radsLines = textBox_rad.Lines;

            foreach (var item in radsLines)
            {
                if (String.IsNullOrWhiteSpace(item)) continue;

                var a = double.Parse(item);
                var rad = new DMS(a, AngleUnit.Degree).ToString();
                sb.AppendLine(rad + "");
            }
            textBox_degree.Text = sb.ToString();
        }
    }
}