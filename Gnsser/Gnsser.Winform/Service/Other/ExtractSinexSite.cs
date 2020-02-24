using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Gnsser.Data.Sinex;
using Geo.Utils;

namespace Gnsser.Winform.Other
{
    public partial class ExtractSinexSite : Form
    {
        public ExtractSinexSite()
        {
            InitializeComponent();
        }
        List<string> stations = new List<string>();
        List<string> stationfiles = new List<string>();
        private void button_SinexFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_SinexFile.Text = this.openFileDialog1.FileName;
            }
        }

        private void button_ObsFilePath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                this.textBox_ObsFilePath.Text = this.folderBrowserDialog1.SelectedPath;
            string sourseDir1 = this.textBox_ObsFilePath.Text;
            string[] files = Directory.GetFiles(sourseDir1, "*.12o");
            for (int i = 0; i < files.Length; i++)
            {
                string tmp = files[i].Substring(files[i].Length - 12, 4);
                tmp = tmp.ToUpper();
                stations.Add(tmp);
                stationfiles.Add(files[i]);
            }
        }

        private void button_extract_Click(object sender, EventArgs e)
        {
            SinexFile sinexfile;
            sinexfile = SinexReader.Read(textBox_SinexFile.Text);
            List<string> sites = sinexfile.GetSiteCods();

            progressBar1.Maximum = stations.Count;
            progressBar1.Minimum = 1;
            progressBar1.Step = 1;
            progressBar1.Value = progressBar1.Minimum;


            for(int i = 0; i< stations.Count; i++)
            {
                progressBar1.PerformStep();
                progressBar1.Refresh();
                if(sites.Contains(stations[i]))
                {
                    continue;
                }
                else
                {
                    File.Delete(stationfiles[i]);
                }                
            }
            FormUtil.ShowIfOpenDirMessageBox(this.textBox_ObsFilePath.Text);
        }
    }
}
