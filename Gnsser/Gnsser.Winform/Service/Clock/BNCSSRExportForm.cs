// 2017.3.14 double creates in zhengzhou

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

namespace Gnsser.Winform
{
    public partial class BNCSSRExportForm :Form
    {
        public BNCSSRExportForm()
        {
            InitializeComponent();
        }

        
        ClockFile ClockFile;
        ClockFile OriginalSSR;
        ClockFile OriginalSSRMinusPreciseClockOutput;
        ClockFile OriginalSSRPlusNavOutput;
        FileEphemerisService ephemeris;
        private class clock
        {
            public List<string> Time=new List<string>();
            public List<double> clk=new List<double> ();
        }
        public static string[] SplitByBlank(string line)
        {
            return line.Split(new char[] { '\t',' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                OriginalSSR = new ClockFile();
                OriginalSSRMinusPreciseClockOutput = new ClockFile();
                OriginalSSRPlusNavOutput = new ClockFile();
                using (StreamReader sr = new StreamReader(SSRsp3Pathes[i]))
                {
                    string line = sr.ReadLine();
                    if (line.Substring(0, 1) == "!")
                        ReadSSRofBNC(ref start0, ref end0,  sr, line);
                    else if (line.Substring(0, 1) == ">")
                    {
                        bool IsNotEnd = true;
                        while (IsNotEnd)
                        {
                            if (line == null || line == "")
                                break;
                            if (line.Length > 10000)
                            {
                                throw new Exception("Line too long");
                            }
                            if (line.Length == 0)
                            {
                                IsNotEnd = false;
                            }
                            if (line.Substring(2, 5) == "CLOCK")
                            {

                                Time time = Time.Parse(line.Substring(8, 21));
                                int satCount = int.Parse(line.Substring(31, 3));
                                for (int index = 0; index < satCount; index++)
                                {
                                    line = sr.ReadLine();
                                    AtomicClock item = new AtomicClock();
                                    item.Prn = SatelliteNumber.TryParse(line.Substring(0, 3));
                                    item.ClockBias = double.Parse(line.Substring(12, 5)) / GnssConst.LIGHT_SPEED;
                                    item.Time = time;
                                    item.ClockType = Data.Rinex.ClockType.Satellite;
                                    item.Name = item.Prn.ToString();
                                    OriginalSSR.GetClockItems(item.Prn).Add(item);
                                }

                                if (time < start0) start0 = time;
                                if (time > end0) end0 = time;
                            }
                            line = sr.ReadLine();

                        }
                    }
                    else
                        ReadSSRofRevisedBNC(ref start0, ref end0, sr, line);
                }               
                OriginalSSR.TimePeriod = new BufferedTimePeriod(start0, end0);
                Dictionary<string, int> SatEpochCount = new Dictionary<string, int>();
                foreach (var item in OriginalSSR.Names)
                {
                    SatEpochCount.Add(item,OriginalSSR.GetClockItems(item).Count); 
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

                ClockFileReader reader = new ClockFileReader(ClockPathes[i], false );
                ClockFile = reader.ReadAll();
                if (ClockFile.ClockCount == 0) return;
                #endregion

                this.bindingSource1.DataSource = OriginalSSR.AllItems;
                this.comboBox_name.DataSource = OriginalSSR.Names;
                
                List<AtomicClock> OriginalSSRDataSource = new List<AtomicClock>();
                List<AtomicClock> ClockFileDataSource = new List<AtomicClock>();

                foreach (var item in OriginalSSR.Names)
                {
                    //OriginalSSRDataSource = OriginalSSR.GetClockItems(key);
                    ClockFileDataSource = ClockFile.GetClockItems(item);
                    if (ClockFileDataSource == null) continue;
                    List<AtomicClock> ErrorResult = new List<AtomicClock>();
                    List<AtomicClock> SSRPlusNavResult = new List<AtomicClock>();
                    
                    foreach (var item1 in ClockFileDataSource)
                    {
                        AtomicClock item2 = new AtomicClock();
                        var clk =  OriginalSSR.GetClockItem(item1.Prn.ToString(),item1.Time);
                        if (item1.ClockBias == 9999999999.0 || clk == null)
                        {
                            item1.ClockBias = 9999999999.0;
                            item2.Time = item1.Time;
                            item2.Prn = item1.Prn;
                            item2.Name = item1.Name;
                            item2.ClockType = item1.ClockType;
                            item2.ClockBias = 9999999999.0;                            
                        }
                        else
                        {
                            var NavItem = ephemeris.Get(item1.Prn, item1.Time);
                            item2.Time = item1.Time;
                            item2.Prn = item1.Prn;
                            item2.Name = item1.Name;
                            item2.ClockType = item1.ClockType;
                            item2.ClockBias = NavItem.ClockBias - NavItem.RelativeCorrection + clk.ClockBias;
                            item1.ClockBias = item2.ClockBias - item1.ClockBias;
                        }
                        SSRPlusNavResult.Add(item2);
                        ErrorResult.Add(item1);
 
                    }
                    OriginalSSRMinusPreciseClockOutput.Add(item, ErrorResult);
                    OriginalSSRPlusNavOutput.Add(item, SSRPlusNavResult);
 
                }
                
                double interval = double.Parse(this.textBox_interval.Text);
                ClockEstimationFrom = this.dateTimePicker_from.Value;
                ClockEstimationTo = this.dateTimePicker_to.Value;
                
               
                OriginalSSR.Header = ClockFile.Header;
                OriginalSSR.Header.SourceName = "SSR" + OriginalSSR.Header.SourceName;
                OriginalSSR.Header.CreationDate = DateTime.Now.ToString();
                OriginalSSR.Header.CreationAgence = "Gnsser";
                OriginalSSR.Header.ANALYSIS_CENTER = "Gnsser";
                OriginalSSR.Header.CreationProgram = "Gnsser";
                OriginalSSR.Header.TYPES_OF_DATA.Clear();
                OriginalSSR.Header.TYPES_OF_DATA.Add("AS");
                OriginalSSR.Header.COUNT_OF_TYPES_OF_DATA = 1;
                OriginalSSR.Header.ClockSolnStations.Clear();
                OriginalSSRMinusPreciseClockOutput.Header = OriginalSSR.Header;
                OriginalSSRPlusNavOutput.Header = OriginalSSR.Header;
                
                var resutlPath = Path.Combine("D:\\Temp\\SSR1\\", OriginalSSR.Header.SourceName);
                var errorResutlPath = Path.Combine("D:\\Temp\\SSR1\\", "error" + OriginalSSR.Header.SourceName);
                
                ClockFileWriter ClockFileWriter = new ClockFileWriter(resutlPath, OriginalSSRPlusNavOutput);
                ClockFileWriter.SaveToFile();
                ClockFileWriter errorClockFileWriter = new ClockFileWriter(errorResutlPath, OriginalSSRMinusPreciseClockOutput);
                errorClockFileWriter.SaveToFile();
                TableTextManager = new ObjectTableManager();


                TableTextManager.OutputDirectory = "D:\\Temp\\SSR1\\";

                var paramTable = TableTextManager.GetOrCreate(OriginalSSRMinusPreciseClockOutput.Name + "errorSSRSat");
                int count = 0;
                SatelliteNumber prnIndex = new SatelliteNumber();
                foreach (var item in OriginalSSRMinusPreciseClockOutput)
                {
                    if (item.Count > count) { count = item.Count; prnIndex = item[0].Prn; }
                }

                var standard = OriginalSSRMinusPreciseClockOutput.GetClockItems(prnIndex);
                double DoubleDiffer = 0;
                foreach (var item in standard)
                {
                    paramTable.NewRow();
                    paramTable.AddItem("Epoch", item.Time);
                    foreach (var item1 in OriginalSSRMinusPreciseClockOutput.Names)
                    {
                        if (item1 == item.Name.ToString()) continue;

                        var ss = OriginalSSRMinusPreciseClockOutput.GetClockItem(item1, item.Time);
                        if (ss == null)
                            continue;
                        if (item.ClockBias == 9999999999.0 || ss.ClockBias == 9999999999.0)
                            DoubleDiffer = 0;
                        else DoubleDiffer = item.ClockBias - ss.ClockBias;
                        paramTable.AddItem(ss.Prn + "-" + item.Prn, DoubleDiffer * 1E9);
                    }
                    paramTable.EndRow();
                }
                TableTextManager.WriteAllToFileAndCloseStream();
            }
            EpochCountTableTextManager.WriteAllToFileAndCloseStream();
            Geo.Utils.FileUtil.OpenDirectory("D:\\Temp\\SSR1\\");
        }

        private void ReadSSRofRevisedBNC(ref Time start0, ref Time end0, StreamReader sr, string line)
        {
            bool IsNotEnd = true;
            while (IsNotEnd)
            {

                if (line == null || line == "")
                    break;
                if (line.Length > 10000)
                {
                    throw new Exception("Line too long");
                }
                if (line.Length == 0)
                {
                    IsNotEnd = false;
                } AtomicClock item = new AtomicClock();

                string[] tmp = SplitByBlank(line);

                item.ClockBias = double.Parse(tmp[3]);
                item.Time = new Time(int.Parse(tmp[0]), double.Parse(tmp[1]));
                item.Prn = SatelliteNumber.TryParse(tmp[2]);


                item.ClockType = Data.Rinex.ClockType.Satellite;
                item.Name = item.Prn.ToString();
                OriginalSSR.GetClockItems(item.Prn).Add(item);
                if (item.Time < start0) start0 = item.Time;
                if (item.Time > end0) end0 = item.Time;
                line = sr.ReadLine();
            }
        }

        private void ReadSSRofBNC(ref Time start0, ref Time end0, StreamReader sr,  string line)
        {
            bool IsNotEnd = true;
            while (IsNotEnd)
            {
                line = sr.ReadLine();
                if (line == null || line == "")
                    break;
                if (line.Length > 10000)
                {
                    throw new Exception("Line too long");
                }
                if (line.Length == 0)
                {
                    IsNotEnd = false;
                }
                if (line.Substring(0, 4) == "1058" || line.Substring(0, 4) == "1064")
                {
                    string[] tmp = SplitByBlank(line);
                    AtomicClock item = new AtomicClock();
                    item.ClockBias = double.Parse(tmp[5])/GnssConst.LIGHT_SPEED;
                    item.Time = new Time(int.Parse(tmp[2]), double.Parse(tmp[3]));
                    item.Prn = SatelliteNumber.TryParse(tmp[4]);
                    item.ClockType = Data.Rinex.ClockType.Satellite;
                    item.Name = item.Prn.ToString();
                    OriginalSSR.GetClockItems(item.Prn).Add(item);
                    if (item.Time < start0) start0 = item.Time;
                    if (item.Time > end0) end0 = item.Time;

                }
            }
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
                this.textBox_SSRsp3Pathes.Text = openFileDialog_result.FileName;
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
