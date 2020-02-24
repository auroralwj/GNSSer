//2016.04.26 double creates on the train of xi'an-Zhengzhou 初步完成各种模型，模型的计算性能有待验证
//2016.10.08 double edits in  hongqing
//2016.11.06. double edits in  zhengzhou，更名为ClockPredictionBasedonSp3ExportForm，并完善相应代码，生成sp3文件
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
    public partial class ClockPredictionBasedonSp3ExportForm : Form
    {
        public ClockPredictionBasedonSp3ExportForm()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_sp3.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_sp3.FileNames;
        }
        Sp3File sp3;
        Sp3File sp3ofPredicted;
        Dictionary<Time , Clockbias> ClockBias;
        private class Clockbias
        {
            public Dictionary<SatelliteNumber, double> sat;
        }
        /// <summary>
        /// 输出sp3文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_export_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);
            
            string[] pathes = this.textBox_Pathes.Lines;

            foreach (var sp3item in pathes)
            {
                Sp3Reader r = new Sp3Reader(sp3item);
                sp3 = r.ReadAll();
                string PredictedClockBiasPath = this.textBox_predictedresultPath.ToString();

                ClockBias = new Dictionary<Time, Clockbias>();

                GetPredictedClockBias(PredictedClockBiasPath);
                sp3ofPredicted = new Sp3File();
                sp3ofPredicted.Header = sp3.Header;
                
                foreach (Sp3Section sec in sp3)
                {
                    Sp3Section s = new Sp3Section();
                    Clockbias section = ClockBias[sec.Time];
                    foreach (Ephemeris rec in sec)
                    {
                        s.Time = rec.Time;
                        if (section.sat.ContainsKey(rec.Prn) && section.sat[rec.Prn]!=null)
                            rec.ClockBias = section.sat[rec.Prn];
                        else
                            rec.ClockBias = 999999.999999;
                        
                        s.Add(rec.Prn, rec);
                    }
                    sp3ofPredicted.Add(s);
                }
                sp3ofPredicted.Header.Comments.Add("Processd by Gnsser");
                sp3ofPredicted.Header.Comments.Add("Predicted clock bias replace original precise data");
                var path = Path.Combine(directory, "P"+ r.Name +".sp3");
                Sp3Writer Sp3Writer = new Sp3Writer(path, sp3ofPredicted);
                sp3.Clear();
            }
            Geo.Utils.FileUtil.OpenDirectory(directory);
        }

        /// <summary>
        /// 获取钟差预报结果
        /// </summary>
        /// <param name="PredictedClockBiasPath"></param>
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
                    Clockbias cc = new Clockbias();
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
