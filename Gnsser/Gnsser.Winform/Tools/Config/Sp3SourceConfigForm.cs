//2014.12.04, czs, create in jinxingliangmao, 星历数据源设置

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class Sp3SourceConfigForm : Form
    {
        public Sp3SourceConfigForm()
        {
            InitializeComponent();
            LoadSetting();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            Save();
            Geo.Utils.FormUtil.ShowOkMessageBox();
        }

        private void Save()
        { 

            Setting.GnsserConfig.IgsProductUrlDirectories = this.textBoxurldircory.Lines;
            Setting.GnsserConfig.IgsProductUrlModels = this.textBox_IgsEphemerisUrlModel.Lines;
            Setting.GnsserConfig.IgsProductLocalDirectory = this.textBox_ephemerisDir.Text.Trim();
            Setting.GnsserConfig.BeidouEphemerisSource = this.textBox_beidou.Text.Trim();
            Setting.GnsserConfig.GpsEphemerisSource = this.textBox_gps.Text.Trim();
            Setting.GnsserConfig.GalieoEphemerisSource = this.textBox_galieo.Text.Trim();
            Setting.GnsserConfig.GlonassEphemerisSource = this.textBox_glonass.Text.Trim();
            Setting.SaveConfigToFile();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            LoadSetting();
        }

        private void LoadSetting()
        {
            this.textBoxurldircory.Lines = Setting.GnsserConfig.IgsProductUrlDirectories;
            this.textBox_IgsEphemerisUrlModel.Lines = Setting.GnsserConfig.IgsProductUrlModels;
            this.textBox_ephemerisDir.Text = Setting.GnsserConfig.IgsProductLocalDirectory.Trim();
            this.textBox_beidou.Text = Setting.GnsserConfig.BeidouEphemerisSource.Trim();
            this.textBox_gps.Text = Setting.GnsserConfig.GpsEphemerisSource.Trim();
            this.textBox_galieo.Text = Setting.GnsserConfig.GalieoEphemerisSource.Trim();
            this.textBox_glonass.Text = Setting.GnsserConfig.GlonassEphemerisSource.Trim();
        }
    }
}
