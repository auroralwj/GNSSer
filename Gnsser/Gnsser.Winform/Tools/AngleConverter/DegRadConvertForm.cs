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
    public partial class DegRadConvertForm : Form
    {
        public DegRadConvertForm()
        {
            InitializeComponent();
        }

        private void button_degreeToRad_Click(object sender, EventArgs e)
        {

            double lat1 = DMS.Parse("35°00'0.22˝").Degrees;
            double lon1 = DMS.Parse("90°00'00.11˝").Degrees;
            double lat2 = DMS.Parse("-30°29'20.960000˝").Degrees;
            double lon2 = DMS.Parse("215°59'04.340000˝").Degrees;


            var angle = GeodeticX.Geodetic.Bessel_BL_A(lat1, lon1, lat2, lon2, 6378245, 298.3);

            var angle2 = Geo.Coordinates.GeodeticUtils.BesselAzimuthAngle(lat1, lon1, lat2, lon2, 6378245, 298.3);



            var degreesLines = textBox_degree.Lines;
            StringBuilder sb = new StringBuilder();
            foreach (var item in degreesLines)
            {
                if (String.IsNullOrWhiteSpace(item)) continue;

                var rad = Geo.Coordinates.AngularConvert.DegToRad(  DMS.Parse(item).Degrees);
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
                var rad = new DMS(a, AngleUnit.Radian).ToString();// DMS.DegreeToDms_s(Geo.Coordinates.AngularConvert.RadToDeg(a));
                sb.AppendLine(rad + "");
            }
            textBox_degree.Text = sb.ToString();
        }
    }
}
