//2016.11.06.00 double create in  zhengzhou，完善相应代码，生成clk文件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Gnsser.Data;
using Geo.Coordinates;
using Geo.Referencing;
using AnyInfo;
using Geo.Algorithm;
using Geo.Times;
using Gnsser.Core;
using System.IO;
using Gnsser.Service;
using Gnsser.Winform.Other;

namespace Gnsser.Winform
{
    public partial class ClockPredictionBasedonClockFileExportForm : Form
    {
        public ClockPredictionBasedonClockFileExportForm()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_nav.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_nav.FileNames;
        }
        ClockFile ClockFile;
        ClockFile ClockFileofPredicted;
        Dictionary<Time , clockbias> ClockBias;
        private class clockbias
        {
            public Dictionary<SatelliteNumber, double> sat;
        }
        private void button_export_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);
            var prns = SatelliteNumber.ParsePRNsBySplliter(textBox_satPrns.Text, new char[] { ',' });
            string[] pathes = this.textBox_Pathes.Lines;
            foreach (var sp3item in pathes)
            {
                ClockFileReader r = new ClockFileReader(sp3item,false);
                ClockFile = r.ReadAll();
                string PredictedClockBiasPath = this.textBox_predictedresultPath.ToString();

                ClockBias = new Dictionary<Time, clockbias>();

                GetPredictedClockBias(PredictedClockBiasPath);
                ClockFileofPredicted = new ClockFile();
                ClockFileofPredicted.Header = ClockFile.Header;

                foreach (var item in ClockFile.Data.Values)
                {
                    foreach (var clock in item)
                        if (clock.ClockType == Data.Rinex.ClockType.Satellite)
                        {
                            clockbias section = ClockBias[clock.Time];
                            clock.ClockBias = section.sat[clock.Prn];
                            ClockFileofPredicted.GetClockItems(clock.Name).Add(clock);
                            
                        }
                        else ClockFileofPredicted.GetClockItems(clock.Name).Add(clock);
                }

                ClockFileofPredicted.Header.Comments.Add("Processd by Gnsser");
                ClockFileofPredicted.Header.Comments.Add("Predicted clock bias replace original precise data");
                var path = Path.Combine(directory, "P" + r.Name + ".clk");
                ClockFileWriter ClockFileWriter = new ClockFileWriter(sp3item, ClockFileofPredicted);
                ClockFileWriter.SaveToFile();
                ClockFile.Clear();
            }
            Geo.Utils.FileUtil.OpenDirectory(directory);
        }

        private void GetPredictedClockBias(string PredictedClockBiasPath)
        {
            bool isEnd = false;
            using (StreamReader sr = new StreamReader(PredictedClockBiasPath))
            {
                string line = sr.ReadLine();
                string[] sat = SinexCoord.SplitByBlank(line);
                while (!isEnd)
                {
                    line = sr.ReadLine();
                    if (line == null || line == "")
                        break;
                    if (line.Length == 0)
                    {
                        isEnd = false;
                    }
                    string[] tmp = SinexCoord.SplitByBlank(line);
                    Time time = Time.Parse(tmp[0]);
                    clockbias cc = new clockbias();
                    for (int i = 0; i < sat.Length; i++)
                        cc.sat.Add(SatelliteNumber.Parse(sat[i + 1]), double.Parse(tmp[i + 1]));
                    ClockBias.Add(time, cc);
                }
            }
        }


        private void GetPath( SatelliteNumber prn,  string PredictedClockBiasPath)
        {
            foreach (var item in this.textBox_predictedresultPath.Lines)
            {
                if (item.Contains(prn.ToString()))
                    PredictedClockBiasPath = item;
                break;
            }
        }

        private void button_predictedresult_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_predictedresult.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_predictedresultPath.Lines = this.openFileDialog_predictedresult.FileNames;
            }
        }
    }
}
