using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class OFileSelectorForm : Form
    {
        public OFileSelectorForm()
        {
            InitializeComponent();
        }

        private void button_setODirPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_dir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_setStaPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_selectedPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string dirPath = this.textBox_dir.Text;
            string selectedDir = this.textBox_selectedPath.Text;
            string antstr = this.textBox_ant.Text;
            char[] splitChars = new char[] { ' ', ',', ';', '\n', '\r' };

            string[] ants = antstr.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            string[] files = Directory.GetFiles(dirPath, "*.*O");
            if (!Directory.Exists(selectedDir)) Directory.CreateDirectory(selectedDir);

            int maxSatCount = int.Parse(this.textBox_satCount.Text);

            string[] includedSites = this.textBox_sitesIncluded.Text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            string[] includeReceiver = this.textBox_receivers.Lines;

            this.progressBarComponent1.InitProcess(files.Length);
            List<string> paths = new List<string>();
            foreach (var item in files)
            {
                this.progressBarComponent1.PerformProcessStep();

                bool selected = false;

                string fileName = Path.GetFileNameWithoutExtension(item).ToUpper();
                this.progressBarComponent1.ShowInfo("正在处理：" + item);
                if (this.checkBox_ant.Checked)
                    if (Gnsser.Data.Rinex.ObsFileFormater.UpperAntTypeName(item, ants))
                    {
                        selected = true;
                    }

                if (!selected && checkBox_satCount.Checked)
                {
                    //历元卫星数量大于
                    Gnsser.Data.Rinex.RinexObsFile o = (Data.Rinex.RinexObsFile)new Data.Rinex.RinexObsFileReader(item).ReadObsFile();
                    foreach (var sec in o)
                    {
                        if (sec.Count > maxSatCount)
                        {
                            selected = true;
                        }
                    }
                }
                if (!selected && checkBox_siteIncluded.Checked)
                {
                    if (includedSites.FirstOrDefault<String>(m => fileName.Contains(m.ToUpper())) != null)
                    {
                        selected = true;
                    }
                }

                if (!selected && checkBox_receiverInclude.Checked)
                {
                    Data.Rinex.RinexObsFileHeader h = new Data.Rinex.RinexObsFileReader(item).GetHeader();

                    if (includeReceiver.FirstOrDefault<String>(m => m.Trim() != "" && h.SiteInfo.ReceiverType.ToUpper().Contains(m.ToUpper())) != null)
                    {
                        selected = true;
                    }

                }
                if (selected)
                    if (checkBox_remove.Checked) File.Move(item, Path.Combine(selectedDir, Path.GetFileName(item)));
                    else File.Copy(item, Path.Combine(selectedDir, Path.GetFileName(item)));

            }

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(Path.GetDirectoryName(selectedDir));
        }


        private void button_getNameFromFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = openFileDialog1.FileNames;
                StringBuilder sb = new StringBuilder();
                foreach (var item in files)
                    sb.Append(Path.GetFileName(item).Substring(0, 4).ToUpper() + ",");
                this.textBox_sitesIncluded.Text = sb.ToString();
            }
        }

        private void button_setReceiverFromFile_Click(object sender, EventArgs e)
        {
            string path = this.textBox_receiverPath.Text;
            List<string> list = new List<string>();
            using (StreamReader r = new StreamReader(path))
            {
                string line = null;
                int lineCount = 0;
                while ((line = r.ReadLine()) != null)
                {
                    lineCount++;
                    if (lineCount < 6) continue;//略去头部

                    if (line.Trim() != "" && line.Length > 20)
                    {
                        string receiver = line.Substring(0, 20).Trim();
                        if (receiver != "") list.Add(receiver);
                    }
                }
            }
            this.textBox_receivers.Lines = list.ToArray();
        }

        private void button_setReceiverPath_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_receiverPath.Text = openFileDialog1.FileName;
            }
        }
    }
}
