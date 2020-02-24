// 2017.05.06 double creates in hongqing

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Geo.Times;
using System.IO;
using Geo;
using Geo.Coordinates;

namespace Gnsser.Winform
{
    public partial class SSRExportForm :Form
    {
        public SSRExportForm()
        {
            InitializeComponent();
        }

        
        ClockFile ClockFile{get;set;}
        Sp3File OriginalSSRSp3 { get; set; }
        Sp3File OriginalSSRSp3MinusPreciseClockOutput { get; set; }
        Sp3File OriginalSSRSp3PlusNavOutput { get; set; }
        FileEphemerisService ephemeris { get; set; }
        private class clock
        {
            public List<string> Time=new List<string>();
            public List<double> clk=new List<double> ();
        }
       
        private void calculate_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var intervalSec = double.Parse(textBox_interval.Text) * 60;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);
            string[] SSRsp3Pathes = this.textBox_SSRsp3Pathes.Lines;
            string[] NavPathes = this.textBox_NavPathes.Lines;
            string[] ClockPathes = this.textBox1.Lines;
            if (SSRsp3Pathes.Length != NavPathes.Length)
            {
                return;
            }
            int fileCount = SSRsp3Pathes.Length;

            if (!System.IO.Directory.Exists(@"D:\Temp\SSR1\"))
            { System.IO.Directory.CreateDirectory(@"D:\Temp\SSR1\"); }
            EpochCountTableTextManager = new ObjectTableManager();


            EpochCountTableTextManager.OutputDirectory = "D:\\Temp\\SSR1\\";

            var SatEpochCountTable = EpochCountTableTextManager.GetOrCreate("BNCSatEpochCount");// + OriginalSSR.Header.Name);
            SatEpochCountTable.NewRow();

            for (int i = 0; i < fileCount; i++)
            {
                #region 读取SSR产品
                Time start0 = Time.MaxValue;
                Time end0 = Time.MinValue;
                OriginalSSRSp3 = new Sp3File();
                OriginalSSRSp3MinusPreciseClockOutput = new Sp3File();
                OriginalSSRSp3PlusNavOutput = new Sp3File();
                Sp3Reader r = new Sp3Reader(SSRsp3Pathes[i]);
                OriginalSSRSp3 = r.ReadAll();
                OriginalSSRSp3.CheckOrBuildIndexCollection();

                Dictionary<string, int> SatEpochCount = new Dictionary<string, int>();
                foreach (var item in OriginalSSRSp3.Prns)
                {
                    SatEpochCount.Add(item.ToString(), OriginalSSRSp3.SatEphemerisCollection[item].Count);
                }
                SatEpochCountTable.AddItem("Day", start0.GetGpsWeekAndDay());
                foreach (var item in SatEpochCount)
                {
                    SatEpochCountTable.AddItem(item.Key, item.Value);
                }
                SatEpochCountTable.EndRow();

                #endregion

                #region 读取广播星历
                ParamNavFileReader NavReader = new ParamNavFileReader(NavPathes[i]);
                ephemeris = new SingleParamNavFileEphService(NavReader.ReadGnssNavFlie());
                #endregion
                #region 读取钟差文件

                //ClockFileReader reader = new ClockFileReader(ClockPathes[i]);
                //ClockFile = reader.ReadAll();
                //if (ClockFile.ClockCount == 0) return;
                #endregion
                OriginalSSRSp3.CheckOrBuildIndexCollection();
                for (Time time = OriginalSSRSp3.TimePeriod.Start; time <= OriginalSSRSp3.TimePeriod.End; time += 1)
                {
                    Sp3Section Sp3Section = new Sp3Section();
                    Sp3Section.Time = time;
                    foreach (var prn in OriginalSSRSp3.Prns)
                    {
                        var ss1 = OriginalSSRSp3.SatEphemerisCollection[prn].Values.FindLast(b => b.Time <= time);

                        if (!ephemeris.Prns.Contains(prn)) continue;
                        var ss = ephemeris.Get(prn, time);
                        XYZ eA = ss.XyzDot / ss.XyzDot.Length;
                        XYZ eC = ss.XYZ.Cross(ss.XyzDot) / (ss.XYZ.Cross(ss.XyzDot)).Length;
                        XYZ eR = eA.Cross(eC) / (eA.Cross(eC)).Length;

                        XYZ deltaO = ss1.XYZ + ss1.XyzDot * (time.Seconds - ss1.Time.Seconds);
                        double x = eA.X * deltaO.X + eA.X * deltaO.Y + eC.X * deltaO.Z;
                        double y = eA.Y * deltaO.X + eA.Y * deltaO.Y + eC.Y * deltaO.Z;
                        double z = eA.Z * deltaO.X + eA.Z * deltaO.Y + eC.Z * deltaO.Z;


                        Ephemeris Sp3Record = new Ephemeris();
                        Sp3Record.Prn = prn;
                        Sp3Record.XYZ = ss.XYZ-new XYZ(x, y, z);
                        if (prn.SatelliteType == SatelliteType.R) Sp3Record.ClockBias = ss.ClockBias + ss1.ClockBias ;
                        else Sp3Record.ClockBias = ss.ClockBias-ss.RelativeCorrection + ss1.ClockBias;
                        Sp3Section.Add(prn, Sp3Record);
                    }
                    OriginalSSRSp3PlusNavOutput.Add(Sp3Section);
                }
                var resultPath = Path.Combine("D:\\Temp\\SSR1\\", OriginalSSRSp3.Name);

                Sp3Writer ClockFileWriter = new Sp3Writer(resultPath, OriginalSSRSp3PlusNavOutput);
                ClockFileWriter.SaveToFile();
            }

                
            //    List<AtomicClock> OriginalSSRDataSource = new List<AtomicClock>();
            //    List<AtomicClock> ClockFileDataSource = new List<AtomicClock>();

            //    foreach (var key in OriginalSSRSp3.Prns)
            //    {
            //        //OriginalSSRDataSource = OriginalSSR.GetClockItems(key);
            //        ClockFileDataSource = ClockFile.GetClockItems(key);
            //        if (ClockFileDataSource == null) continue;
            //        List<AtomicClock> ErrorResult = new List<AtomicClock>();
            //        List<AtomicClock> SSRPlusNavResult = new List<AtomicClock>();
                    
            //        foreach (var item1 in ClockFileDataSource)
            //        {
            //            AtomicClock item2 = new AtomicClock();
            //            var clk =  OriginalSSRSp3.GetClockItem(item1.Prn.ToString(),item1.Time);
            //            if (item1.ClockBias == 9999999999.0 || clk == null)
            //            {
            //                item1.ClockBias = 9999999999.0;
            //                item2.Time = item1.Time;
            //                item2.Prn = item1.Prn;
            //                item2.Name = item1.Name;
            //                item2.ClockType = item1.ClockType;
            //                item2.ClockBias = 9999999999.0;                            
            //            }
            //            else
            //            {
            //                var NavItem = ephemeris.Get(item1.Prn, item1.Time);
            //                item2.Time = item1.Time;
            //                item2.Prn = item1.Prn;
            //                item2.Name = item1.Name;
            //                item2.ClockType = item1.ClockType;
            //                item2.ClockBias = NavItem.ClockBias - NavItem.RelativeTime + clk.ClockBias;
            //                item1.ClockBias = item2.ClockBias - item1.ClockBias;
            //            }
            //            SSRPlusNavResult.Add(item2);
            //            ErrorResult.Add(item1);
 
            //        }
            //        OriginalSSRSp3MinusPreciseClockOutput.Add(key, ErrorResult);
            //        OriginalSSRSp3PlusNavOutput.Add(key, SSRPlusNavResult);
 
            //    }
                
            //    double interval = double.Parse(this.textBox_interval.Text);
            //    ClockEstimationFrom = this.dateTimePicker_from.Value;
            //    ClockEstimationTo = this.dateTimePicker_to.Value;
                
               
            //    OriginalSSRSp3.Header = new Sp3Header ();
            //    OriginalSSRSp3.Header.AgencyName="Gnsser";
            //    OriginalSSRSp3.Header.EndTime=
            //    .Name = "SSR" + OriginalSSRSp3.Header.Name;
            //    OriginalSSRSp3.Header.CreationDate = DateTime.Now.ToString();
            //    OriginalSSRSp3.Header.CreationAgence = "Gnsser";
            //    OriginalSSRSp3.Header.ANALYSIS_CENTER = "Gnsser";
            //    OriginalSSRSp3.Header.CreationProgram = "Gnsser";
            //    OriginalSSRSp3MinusPreciseClockOutput.Header = OriginalSSRSp3.Header;
            //    OriginalSSRSp3PlusNavOutput.Header = OriginalSSRSp3.Header;
                
            //    var resutlPath = Path.Combine("D:\\Temp\\SSR1\\", OriginalSSRSp3.Header.Name);
            //    var errorResutlPath = Path.Combine("D:\\Temp\\SSR1\\", "error" + OriginalSSRSp3.Header.Name);
                
            //    ClockFileWriter ClockFileWriter = new ClockFileWriter(resutlPath, OriginalSSRSp3PlusNavOutput);
            //    ClockFileWriter.SaveToFile();
            //    ClockFileWriter errorClockFileWriter = new ClockFileWriter(errorResutlPath, OriginalSSRSp3MinusPreciseClockOutput);
            //    errorClockFileWriter.SaveToFile();
            //    TableTextManager = new TableObjectManager();


            //    TableTextManager.OutputDirectory = "D:\\Temp\\SSR1\\";

            //    var paramTable = TableTextManager.GetOrCreate(OriginalSSRSp3MinusPreciseClockOutput.Name + "errorSSRSat");
            //    int count = 0;
            //    SatelliteNumber prnIndex = new SatelliteNumber();
            //    foreach (var key in OriginalSSRSp3MinusPreciseClockOutput)
            //    {
            //        if (key.Count > count) { count = key.Count; prnIndex = key[0].Prn; }
            //    }

            //    var standard = OriginalSSRSp3MinusPreciseClockOutput.GetClockItems(prnIndex);
            //    double DoubleDiffer = 0;
            //    foreach (var key in standard)
            //    {
            //        paramTable.NewRow();
            //        paramTable.AddItem("Epoch", key.Time);
            //        foreach (var item1 in OriginalSSRSp3MinusPreciseClockOutput.Names)
            //        {
            //            if (item1 == key.Name.ToString()) continue;

            //            var ss = OriginalSSRSp3MinusPreciseClockOutput.GetClockItem(item1, key.Time);
            //            if (ss == null)
            //                continue;
            //            if (key.ClockBias == 9999999999.0 || ss.ClockBias == 9999999999.0)
            //                DoubleDiffer = 0;
            //            else DoubleDiffer = key.ClockBias - ss.ClockBias;
            //            paramTable.AddItem(ss.Prn + "-" + key.Prn, DoubleDiffer * 1E9);
            //        }
            //        paramTable.EndRow();
            //    }
            //    TableTextManager.WriteAllToFileAndCloseStream();
            //}
            //EpochCountTableTextManager.WriteAllToFileAndCloseStream();
            Geo.Utils.FileUtil.OpenDirectory("D:\\Temp\\SSR1\\");
        }
        
        /// <summary>
        /// 表格输出管理器
        /// </summary>
        public ObjectTableManager TableTextManager { get; set; }
        /// <summary>
        /// 表格输出管理器
        /// </summary>
        public ObjectTableManager EpochCountTableTextManager { get; set; }
        /// <summary>
        /// 将结果保存到excel中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_to_excel_Click(object sender, EventArgs e)
        {
            Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1);
        }
        DateTime ClockEstimationFrom, ClockEstimationTo;
        DateTime ClockFileFrom, ClockFileTo;
        private void button_filter_Click(object sender, EventArgs e)
        {
            if (this.comboBox_name.SelectedItem == null)
            {
                MessageBox.Show("请先读取数据。"); return; ;
            }
            string name = this.comboBox_name.SelectedItem.ToString();
            ClockEstimationFrom = this.dateTimePicker_from.Value;
            ClockEstimationTo = this.dateTimePicker_to.Value;
            this.bindingSource1.DataSource = ClockFile.GetClockItems(name, new Time(ClockEstimationFrom), new Time(ClockEstimationTo));
        }

        private void button_inter_Click(object sender, EventArgs e)
        {
            double interval = double.Parse(this.textBox_interval.Text);
            List<AtomicClock> sortedRecords = this.bindingSource1.DataSource as List<AtomicClock>;
            if (sortedRecords == null)
            {
                MessageBox.Show("请先读取,并删选数据。"); return; ;
            }
            ClockInterpolator interp = new ClockInterpolator(sortedRecords);

            List<AtomicClock> fitedResult = new List<AtomicClock>();
            double cacuCount = (ClockFileTo - ClockFileFrom).TotalSeconds / interval;
            for (int xi = 0; xi <= cacuCount; xi++)
            {
                Time gpsTime = sortedRecords[0].Time + interval * xi;
                fitedResult.Add(interp.GetAtomicClock(gpsTime));
            }

            this.bindingSource1.DataSource = fitedResult;
        }
        #region 获取指定路径文件
        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_result.ShowDialog() == DialogResult.OK)
                this.textBox_SSRsp3Pathes.Lines = openFileDialog_result.FileNames;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_clock.ShowDialog() == DialogResult.OK)
                this.textBox1.Lines = openFileDialog_clock.FileNames;
                //this.textBox1.Text = openFileDialog_clock.FileName;
        }
        private void button_getNavPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_Nav.ShowDialog() == DialogResult.OK)
                this.textBox_NavPathes.Lines = openFileDialog_Nav.FileNames;
        }
        #endregion

    }
}
