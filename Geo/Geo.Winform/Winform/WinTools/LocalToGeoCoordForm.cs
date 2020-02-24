using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Geo.Winform.Tools
{

    

    public partial class LocalToGeoCoordForm : Form
    {
        public LocalToGeoCoordForm()
        {
            InitializeComponent();
        }

        private void button_neuToXyz_Click(object sender, EventArgs e)
        {
            var siteXYZ = GetSiteCoord();
            StringBuilder sb = new StringBuilder();
            foreach (var item in GetEnus())
            {
                var xyz = CoordTransformer.EnuToXyz(item, siteXYZ);
                sb.AppendLine(xyz.ToString());
            }
            this.richTextBox_ecefXyz.Text = sb.ToString();
        }

        private void button_xyzToNeu_Click(object sender, EventArgs e)
        { 
            var siteXYZ = GetSiteCoord();
            StringBuilder sb = new StringBuilder();
            foreach (var item in GetXyzs())
            {
                var neu = CoordTransformer.XyzToEnu(item, siteXYZ);
                sb.AppendLine(neu.ToString());
            }
            this.richTextBox_neu.Text = sb.ToString();
        }

        public XYZ GetSiteCoord()
        {
            var coordStr = this.textBox_siteCoord.Text;

            if (this.radioButton_geo.Checked)
            {
                var geoCoord = GeoCoord.Parse(coordStr);
                return CoordTransformer.GeoCoordToXyz(geoCoord);
            }

            return XYZ.Parse(coordStr);
        }

        public List<ENU> GetEnus()
        {
            var strs = this.richTextBox_neu.Lines;
            List<ENU> list = new List<ENU>();
            foreach (string item in strs)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                list.Add(ENU.Parse(item));
            }

            return list;
        }
        public List<XYZ> GetXyzs()
        {
            var strs = this.richTextBox_ecefXyz.Lines;
            List<XYZ> list = new List<XYZ>();
            foreach (string item in strs)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                list.Add(XYZ.Parse(item));
            }

            return list;
        }
    }
}
