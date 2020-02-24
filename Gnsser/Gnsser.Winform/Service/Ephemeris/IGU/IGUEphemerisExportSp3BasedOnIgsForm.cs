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
using Geo.Utils;

namespace Gnsser.Winform
{
    /// <summary>
    /// 星历提取服务。
    /// </summary>
    public partial class IGUEphemerisExportSp3BasedOnIgsForm : Form, Gnsser.Winform.IShowLayer
    {
        public IGUEphemerisExportSp3BasedOnIgsForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_IguSp3.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_IguSp3.FileNames;
        }
        private void button_getIgsSp3Path_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_IgsSp3.ShowDialog() == DialogResult.OK)
                this.textBox_IgsSp3Path.Text = openFileDialog_IgsSp3.FileName;
        }
        Sp3File iguSp3;
        Sp3File igsSp3;
        Sp3File sp3Observation;
        Sp3File sp3ofPredicted;
        private void button_read_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var directory = this.directorySelectionControl1.Path;
            string resultDirectory = Path.Combine(directory, "replace clock result");
            Geo.Utils.FileUtil.CheckOrCreateDirectory(resultDirectory);

            string[] pathes = this.textBox_Pathes.Lines;
            string igsSp3Path = this.textBox_IgsSp3Path.Text;
            var st1 = int.Parse(pathes[0].Substring(pathes[0].Length - 6, 2));
            var st2 = int.Parse(pathes[1].Substring(pathes[0].Length - 6, 2));

            #region 按照单个文件进行输出，不合并

            foreach (var pathItem in pathes)
            {
                string dayOfWeek = Path.GetFileNameWithoutExtension(pathItem).Substring(5, 5);
                string dayOfWeekfileName = Path.GetFileNameWithoutExtension(igsSp3Path).Substring(3, 5);
                string path = igsSp3Path.Replace(dayOfWeekfileName, dayOfWeek);
                if (!File.Exists(path))
                {
                    //FormUtil.ShowFileNotExistBox(path);
                    continue;
                }

                Sp3AllReader iguRead = new Sp3AllReader(pathItem);
                iguSp3 = iguRead.ReadAll();
                Sp3AllReader igsRead = new Sp3AllReader(path);
                igsSp3 = igsRead.ReadAll();
                
                var intervalSec = iguSp3.Header.EpochInterval;
                sp3Observation = new Sp3File();
                sp3Observation.Header = igsSp3.Header;
                foreach (Sp3Section sec in igsSp3)
                {
                    var item = iguSp3.Get(sec.Time);
                    Sp3Section s = new Sp3Section();
                    foreach (var rec in sec)
                    {
                        s.Time = rec.Time;
                        if (item != null && item.Contains(rec.Prn))
                            rec.ClockBias = item[rec.Prn].ClockBias;
                        else rec.ClockBias = 0.999999999999;
                        s.Add(rec.Prn, rec);
                    }
                    sp3Observation.Add(s);
                }

                sp3Observation.Header.Comments.Add("Processd by Gnsser");
                sp3Observation.Header.Comments.Add("Choose clock of IGU-P for igs");
                var pathObservation = Path.Combine(resultDirectory, iguSp3.Name);
                if (File.Exists(pathObservation)) File.Delete(pathObservation);
                Sp3Writer Sp3WriterObservation = new Sp3Writer(pathObservation, sp3Observation);
                Sp3WriterObservation.SaveToFile();
                iguSp3.Clear();
            }

            #endregion

            Geo.Utils.FileUtil.OpenDirectory(resultDirectory);
        }


    }

}

