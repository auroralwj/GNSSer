using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Geo.Utils;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;


namespace Gnsser.Winform.Other
{
    public partial class C1P2_Exclude : Form
    {
        public C1P2_Exclude()
        {
            InitializeComponent();
        }
        Data.Rinex.RinexObsFile obsFile;
        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_dir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_exclude_Click(object sender, EventArgs e)
        {
            string sourseDir1 = this.textBox_dir.Text;
            string[] files = Directory.GetFiles(sourseDir1, "*.13O");
            string P1 = "P1";
            progressBar1.Maximum = files.Length;
            progressBar1.Minimum = 1;
            progressBar1.Step = 1;
            progressBar1.Value = progressBar1.Minimum;
            for(int i =0 ;i< files.Length; i++)
            {
                obsFile = new RinexObsFileReader(files[i]).ReadObsFile();
                progressBar1.PerformStep();
                progressBar1.Refresh();
                throw new Exception("to be fiexd");
                //if (!obsFile.Header.ObserTypes.Contains(P1))
                //{
                //    File.Delete(files[time]);
                //    continue;
                //}
                RinexEpochObservation firstobservation = obsFile.GetEpochObservation(obsFile.StartTime);

                //获取GPS卫星                
                List<SatelliteNumber> firstsats = firstobservation.Prns;
                List<SatelliteNumber> gpssats = new List<SatelliteNumber>();
                foreach(var item in firstsats)
                {
                    if(item.SatelliteType == SatelliteType.G)
                    {
                        gpssats.Add(item);
                    }
                }
                
                bool hasp1p2 = false;
                foreach (var prn in gpssats)
                {
                    
                    if(firstobservation[prn][P1].Value != 0)
                    {
                        hasp1p2 = true;
                    }
                }
                if(!hasp1p2)
                {
                    File.Delete(files[i]);
                }
                
            }
        }
    }
}
