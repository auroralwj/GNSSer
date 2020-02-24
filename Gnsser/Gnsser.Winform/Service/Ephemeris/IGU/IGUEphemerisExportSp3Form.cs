//2016.11.15 double, add in zhengzhou, 根据IGU文件生成sp3，分别输出IGU-O和IGU-P

using System;
using Gnsser.Times;
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

namespace Gnsser.Winform
{
    /// <summary>
    /// 星历提取服务。
    /// </summary>
    public partial class IGUEphemerisExportSp3Form : Form, Gnsser.Winform.IShowLayer
    {
        public IGUEphemerisExportSp3Form()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_sp3.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_sp3.FileNames;
        }
        Sp3File sp3;
        Sp3File sp3P;
        Sp3File sp3Observation;
        Sp3File sp3ofPredicted;
        private void button_read_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

            string[] pathes = this.textBox_Pathes.Lines;
            
            var st1 = int.Parse(pathes[0].Substring(pathes[0].Length-6, 2));
            var st2 = int.Parse(pathes[1].Substring(pathes[0].Length - 6, 2));
            #region 按照多个文件进行合并输出
            if (st2 - st1 != 0)
            {
                sp3ofPredicted = new Sp3File(); sp3Observation = new Sp3File(); int indexOfFile = 1; int dayOfweekO = 0; int dayOfweekP = 0;
                foreach (var pathItem in pathes)
                {                    
                    Sp3AllReader r = new Sp3AllReader(pathItem);
                    Sp3AllReader rP = new Sp3AllReader(pathItem);
                    sp3 = r.ReadAll();
                    sp3P = rP.ReadAll();
                    var intervalSec = sp3.Header.EpochInterval;
                    
                    Time end1 = sp3.TimePeriod.Start + (st2 - st1) * 3600 - intervalSec;
                    Time end = sp3.TimePeriod.Start + (st2 - st1 + 24) * 3600 - intervalSec;
                    Time startOfPredicted = sp3.TimePeriod.Start + 24 * 3600 - intervalSec;
                    foreach (Sp3Section sec in sp3)
                    {
                        Sp3Section s = new Sp3Section();
                        foreach (var rec in sec){ s.Time = rec.Time;
                            s.Add(rec.Prn, rec);}                        
                        if (sec.Time <=end1) sp3Observation.Add(s);
                        else if (sec.Time <= end && sec.Time > startOfPredicted) sp3ofPredicted.Add(s);
                    }
                    if (indexOfFile % (24 / (st2 - st1)) == 1)
                    {
                        sp3Observation.Header = sp3.Header;
                        sp3ofPredicted.Header = sp3P.Header;
                        sp3ofPredicted.Header.StartTime = sp3P.Header.StartTime + 24 * 3600;
                        dayOfweekO=sp3Observation.Header.StartTime.GetGpsWeekAndDay();
                        if (dayOfweekO % 10 == 6)
                        { 
                            dayOfweekP = dayOfweekO + 4;
                            sp3ofPredicted.Header.GPSWeek += 1;
                        }
                        else dayOfweekP = dayOfweekO + 1;
                    }
                    if (indexOfFile % (24 / (st2 - st1)) == 0)
                    {
                        sp3Observation.Header.Comments.Add("Processd by Gnsser");
                        sp3Observation.Header.Comments.Add("Choose IGU-O from igu");
                        sp3ofPredicted.Header.Comments.Add("Processd by Gnsser");
                        sp3ofPredicted.Header.Comments.Add("Choose IGU-P from igu");
                        var pathObservation = Path.Combine(directory, "IGU-O" + dayOfweekO.ToString()+".sp3");
                        var pathPredicted = Path.Combine(directory, "IGU-P" + dayOfweekP.ToString() + ".sp3");
                        sp3Observation.Header.NumberOfEpochs = sp3Observation.Count;
                        sp3ofPredicted.Header.NumberOfEpochs = sp3ofPredicted.Count;
                        Sp3Writer Sp3WriterObservation = new Sp3Writer(pathObservation, sp3Observation);
                        Sp3Writer Sp3WriterPredicted = new Sp3Writer(pathPredicted, sp3ofPredicted);
                        Sp3WriterObservation.SaveToFile();
                        Sp3WriterPredicted.SaveToFile();
                        sp3Observation.Clear();
                        sp3ofPredicted.Clear();
                    }
                    sp3.Clear();
                    indexOfFile++;
                }
            }


            #endregion
            #region 按照单个文件进行输出，不合并
            else
            {
                foreach (var pathItem in pathes)
                {
                    Sp3Reader r = new Sp3Reader(pathItem);
                    sp3 = r.ReadAll();
                    var intervalSec = sp3.Header.EpochInterval;
                    sp3Observation = new Sp3File();
                    sp3Observation.Header = sp3.Header;
                    sp3ofPredicted = new Sp3File();
                    sp3ofPredicted.Header = sp3.Header;
                    Time end1 = sp3.TimePeriod.Start + 24 * 3600 - intervalSec;
                    Time end = sp3.TimePeriod.End;
                    foreach (Sp3Section sec in sp3)
                    {
                        Sp3Section s = new Sp3Section();

                        foreach (var rec in sec)
                        {
                            s.Time = rec.Time;
                            s.Add(rec.Prn, rec);
                        }
                        if (sec.Time < end1) sp3Observation.Add(s);
                        else sp3ofPredicted.Add(s);
                    }
                    sp3Observation.Header.Comments.Add("Processd by Gnsser");
                    sp3Observation.Header.Comments.Add("Choose IGU-O from igu");
                    sp3ofPredicted.Header.Comments.Add("Processd by Gnsser");
                    sp3ofPredicted.Header.Comments.Add("Choose IGU-P from igu");
                    var pathObservation = Path.Combine(directory, "IGU-O-Basedon" + r.Name);
                    var pathPredicted = Path.Combine(directory, "IGU-P-Basedon" + r.Name);
                    Sp3Writer Sp3WriterObservation = new Sp3Writer(pathObservation, sp3Observation);
                    Sp3Writer Sp3WriterPredicted = new Sp3Writer(pathPredicted, sp3ofPredicted);
                    Sp3WriterObservation.SaveToFile();
                    Sp3WriterPredicted.SaveToFile();
                    sp3.Clear();
                }
            }
            #endregion

            Geo.Utils.FileUtil.OpenDirectory(directory);
        }
    }

}

