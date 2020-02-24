using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Rinex;

namespace Gnsser.Winform
{
    public partial class OFileFormatForm : Form
    {
        public OFileFormatForm()
        {
            InitializeComponent();
        }
        ObsFileCountManager ObsFileCountManager;
        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_dir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string dirPath = this.textBox_dir.Text;
            string[] files = Directory.GetFiles(dirPath,"*.*O");
           
            if (files.Length == 0)
            {
                files = Directory.GetFiles(dirPath, "*.SMT");

                foreach (var item in files)
                {
                    string path = System.IO.Path.ChangeExtension(item, "**o");
                    byte[] bt = File.ReadAllBytes(item);
                    FileStream fs = File.Create(path);
                    fs.Write(bt, 0, bt.Length);
                    fs.Close();
                }
            }


            files = Directory.GetFiles(dirPath, "*.*O");

            this.progressBarComponent1.InitProcess(files.Length);
            List<string> paths = new List<string>();
            foreach (var item in files)
            {
                if (Gnsser.Data.Rinex.ObsFileFormater.Format(item))
                {

                }

                Data.Rinex.RinexObsFile obsFile = new Gnsser.Data.Rinex.RinexObsFileReader(item).ReadObsFile();

                double per = 0.01;
                string info = ObsFileCountManager.RemoveObserversInfo(ref obsFile, per * 0.01);

                Gnsser.Data.Rinex.ObsFileProcesserChain chain = new Gnsser.Data.Rinex.ObsFileProcesserChain();
                var process = new Gnsser.Data.Rinex.ObsFileCodeFilterProcesser() { MaxPercentage = per };
                chain.AddProcessor(process);
                chain.Revise(ref obsFile);


                info = process.Message;

                string sbb = new Gnsser.Data.Rinex.RinexObsFileWriter().GetRinexString(obsFile, 3.02);

                File.WriteAllText(item, sbb);


                paths.Add(item);


                this.progressBarComponent1.PerformProcessStep();
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in paths)
            {
                sb.AppendLine(item);
            }
            MessageBox.Show("操作完毕！共修改 " + paths.Count + " 个.\r\n" + sb.ToString());
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
