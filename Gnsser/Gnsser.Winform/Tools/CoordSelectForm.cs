//2018.06.05, czs, create in hmx, 坐标选择器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gnsser.Data;
using Gnsser.Service;
using Geo.Utils;
using Gnsser;
using Geo.Coordinates;
using Geo.Times;
using Geo;
using Geo.IO;
using Geo.Draw;
using Gnsser.Data.Rinex;
using Gnsser.Services;


namespace Gnsser.Winform
{
    public partial class CoordSelectForm : Form
    {
        public CoordSelectForm()
        {
            InitializeComponent();
        }

        public GeoCoord GeoCoord { get; internal set; }
        public XYZ XYZ { get; internal set; }

        private void button_ok_Click(object sender, EventArgs e)
        {
            var coordStr = namedStringControl_coord.GetValue();

            this.XYZ = XYZ.Parse(coordStr);
            this.DialogResult = DialogResult.OK;

        }

        private void CoordSelectForm_Load(object sender, EventArgs e)
        {
            SetXyz(XYZ.Zero);
            fileOpenControl_extractFromOFile.Filter = Setting.RinexOFileFilter; 
            this.bindingSource_coord.DataSource = IgsSiteCoordService.Instance.SiteNames;

        }

        private void SetXyz(XYZ XYZ)
        {
            namedStringControl_coord.SetValue(XYZ.ToString());
        }

        private void fileOpenControl_extractFromOFile_FilePathSetted(object sender, EventArgs e)
        {
            SetCoordFromObsFile();
        }

        private void SetCoordFromObsFile()
        {
            var path = fileOpenControl_extractFromOFile.FilePath;

            RinexObsFileReader reader = new RinexObsFileReader(path, false);
            var header = reader.GetHeader();
            if (header.ApproxXyz != null)
            {
                SetXyz(header.ApproxXyz);
            }
            else
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("很抱歉，所选文件不包含坐标！");
            }
        }

        private void comboBox_coordSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var site = comboBox_coordSource.SelectedValue.ToString();
            var coord  = IgsSiteCoordService.Instance.Get(site);
            label_siteInfo.Text = "已选择：" + site;
            SetXyz(coord.Value );

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button_setCoordFromFile_Click(object sender, EventArgs e)
        {
            SetCoordFromObsFile();
        }
    }
}
