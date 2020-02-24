//2016.12.06 double create in hongqing, 将实时数据流以天为单位进行输出
//2017.03.16 double edit in zhengzhou, 修改文件的输出（之前所有结果错误的输出到一个文件中），修改输出文件名

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
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 星历提取服务。
    /// </summary>
    public partial class RTEphemerisExportSp3Form : Form
    {
        public RTEphemerisExportSp3Form()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_sp3.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_sp3.FileNames;
        }
        Sp3File sp3;
        Sp3File sp3Observation;
        /// <summary>
        /// 表格输出管理器
        /// </summary>
        public ObjectTableManager TableTextManager { get; set; }
        private void button_read_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

            string[] pathes = this.textBox_Pathes.Lines;
            int dayOfweekO=0;
            //sp3Observation = new Sp3File();
            TableTextManager = new ObjectTableManager();


            TableTextManager.OutputDirectory = directory;
            
            var SatEpochCountTable = TableTextManager.GetOrCreate("GNSSerSatEpochCount" + Path.GetFileNameWithoutExtension(pathes[0]).Substring(0,12));
            
            #region 按照时间顺序合并
            Time start = Time.MinValue;
            foreach (var pathItem in pathes)
            {
                Sp3Reader r = new Sp3Reader(pathItem);
                sp3 = r.ReadAll();
                var intervalSec = sp3.Header.EpochInterval;

                foreach (Sp3Section sec in sp3)
                {
                    Sp3Section s = new Sp3Section();
                    foreach (var rec in sec) { if (rec.Prn.SatelliteType == SatelliteType.R) rec.Time = rec.Time + 1; s.Time = rec.Time; s.Add(rec.Prn, rec); }
                    if (sp3Observation == null || sp3Observation.Count == 0)
                    {
                        sp3Observation = new Sp3File();
                        sp3Observation.Header = sp3.Header;
                        sp3Observation.Header.StartTime = s.Time;
                        sp3Observation.Header.Comments.Add("Processd by Gnsser");
                        sp3Observation.Header.Comments.Add("Collected from real time SSR clock correction");
                        dayOfweekO = sp3Observation.Header.StartTime.GetGpsWeekAndDay();

                    }
                    if (((s.Time.Hour == 0 && s.Time.Minute == 0 && s.Time.Second == 0)|| start.GetGpsWeekAndDay() != s.Time.GetGpsWeekAndDay()) && sp3Observation.Count != 0 )
                    {

                        var pathObservation = Path.Combine(directory, "Gnsser" + sp3.Name.Substring(0, 12) + dayOfweekO.ToString() + ".sp3");
                        sp3Observation.Header.NumberOfEpochs = sp3Observation.Count;
                        Sp3Writer Sp3WriterObservation = new Sp3Writer(pathObservation, sp3Observation);
                        Sp3WriterObservation.SaveToFile();
                        SatEpochCountTable.NewRow();
                        SatEpochCountTable.AddItem("Day",dayOfweekO);
                        sp3Observation.CheckOrBuildIndexCollection();
                        foreach (var item in sp3Observation.SatEphemerisCollection.Keys)
                        {

                            int a = sp3Observation.SatEphemerisCollection[item].Count;
                            
                            SatEpochCountTable.AddItem( item,a);
                        }
                        SatEpochCountTable.EndRow();

                        sp3Observation.Clear();
                        sp3Observation.SatEphemerisCollection.Clear();
                        sp3Observation.Header.StartTime = s.Time;
                        //sp3Observation.Header.Comments.Add("Processd by Gnsser");
                        //sp3Observation.Header.Comments.Add("Collected from real time SSR clock correction");
                        dayOfweekO = sp3Observation.Header.StartTime.GetGpsWeekAndDay();
                        start = s.Time;
                    }
                    if (!sp3Observation.Contains(s.Time)) 
                        sp3Observation.Add(s);
                }
                
            }
            
            TableTextManager.WriteAllToFileAndCloseStream();

            if (sp3Observation.Count != 0)
            {
                var pathObservation = Path.Combine(directory, "Gnsser" + sp3.Name.Substring(0, 12) + dayOfweekO.ToString() + ".sp3");
                sp3Observation.Header.NumberOfEpochs = sp3Observation.Count;
                Sp3Writer Sp3WriterObservation = new Sp3Writer(pathObservation, sp3Observation);
                Sp3WriterObservation.SaveToFile();
                sp3Observation.Clear();
            }

            #endregion

            Geo.Utils.FileUtil.OpenDirectory(directory);
        }
    }
}

