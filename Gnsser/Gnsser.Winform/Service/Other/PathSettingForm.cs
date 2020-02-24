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
    public partial class PathSettingForm : Form
    {
        public PathSettingForm()
        {
            InitializeComponent();
            Read();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Save();
            Geo.Utils.FormUtil.ShowOkMessageBox();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_setsitepath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_siteName.Text = this.openFileDialog1.FileName;
        }


        private void button_setTaskPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_task.Text = this.openFileDialog1.FileName;
        }

        private void button_setComputePath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_compuNode.Text = this.openFileDialog1.FileName;
        }

        private void button_reset_Click(object sender, EventArgs e) { Read(); }

        #region IO
        private void Save()
        {
            Setting.GnsserConfig.ComputeNodeFilePath = this.textBox_compuNode.Text;
            Setting.GnsserConfig.SiteFilePath = this.textBox_siteName.Text;
            Setting.GnsserConfig.TaskFilePath = this.textBox_task.Text;

            Setting.GnsserConfig.OutputDirectory = this.textBox_outDir.Text;

            Setting.GnsserConfig.SampleOFile = this.textBox4SampleOFile.Text;
            Setting.GnsserConfig.SampleNFile = this.textBox3SampleNFile.Text;
            Setting.GnsserConfig.SampleSinexFile = this.textBox1SampleSinexFile.Text;
            Setting.GnsserConfig.SampleSP3File = this.textBox2SampleSP3File.Text;

            Setting.GnsserConfig.AntennaFile = this.textBox_antennaFile.Text;
            Setting.GnsserConfig.OceanTideFile = this.textBox_ocean.Text;
            Setting.GnsserConfig.SatExcludeFile = this.textBox_satExclude.Text;
            Setting.GnsserConfig.SatSateFile = this.textBox_satState.Text;
            Setting.GnsserConfig.DcbDirectory = this.textBox_dcbDir.Text;


            Setting.GnsserConfig.BaseDataPath = this.textBox_dataBaseDir.Text;
           Setting.GnsserConfig.IgsProductLocalDirectory = this.textBox_localEphemerisDir.Text;

           Setting.SaveConfigToFile();
        }

        private void Read()
        {
            Setting.LoadConfig();

            this.textBox_compuNode.Text = Setting.GnsserConfig.ComputeNodeFilePath;
            this.textBox_siteName.Text = Setting.GnsserConfig.SiteFilePath;
            this.textBox_task.Text = Setting.GnsserConfig.TaskFilePath;

            this.textBox_outDir.Text = Setting.GnsserConfig.OutputDirectory;

            this.textBox4SampleOFile.Text = Setting.GnsserConfig.SampleOFile;
            this.textBox3SampleNFile.Text = Setting.GnsserConfig.SampleNFile;
            this.textBox1SampleSinexFile.Text = Setting.GnsserConfig.SampleSinexFile;
            this.textBox2SampleSP3File.Text = Setting.GnsserConfig.SampleSP3File;

            this.textBox_antennaFile.Text = Setting.GnsserConfig.AntennaFile;
            this.textBox_ocean.Text = Setting.GnsserConfig.OceanTideFile;
            this.textBox_satExclude.Text = Setting.GnsserConfig.SatExcludeFile;
            this.textBox_satState.Text = Setting.GnsserConfig.SatSateFile;
            this.textBox_dcbDir.Text = Setting.GnsserConfig.DcbDirectory;
            this.textBox_dataBaseDir.Text = Setting.GnsserConfig.BaseDataPath;
            this.textBox_localEphemerisDir.Text = Setting.GnsserConfig.IgsProductLocalDirectory;
        }
        #endregion

        #region sample
        private void button4SampleOFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox4SampleOFile.Text = this.openFileDialog1.FileName;
        }

        private void button3SampleNFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox3SampleNFile.Text = this.openFileDialog1.FileName;
        }

        private void button2SampleSP3File_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox2SampleSP3File.Text = this.openFileDialog1.FileName;
        }
        private void button1SampleSinexFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox1SampleSinexFile.Text = this.openFileDialog1.FileName;
        }
        #endregion

        #region  公共数据源
        private void button_setAntennaFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_ant.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_antennaFile.Text = this.openFileDialog_ant.FileName;
        }

        private void button_ocean_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_ocean.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_ocean.Text = this.openFileDialog_ocean.FileName;
        }

        private void button_satState_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_satState.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_satState.Text = this.openFileDialog_satState.FileName;
        }

        private void button_satExclude_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_excludeSat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_satExclude.Text = this.openFileDialog_excludeSat.FileName;
        }
        #endregion

        private void button_setDcbDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_dcbDir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_setOutDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_outDir.Text = folderBrowserDialog1.SelectedPath;
        } 

        private void button_setDataPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_dataBaseDir.Text = folderBrowserDialog1.SelectedPath;

        }

        private void button_localEphemerisDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_localEphemerisDir.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
