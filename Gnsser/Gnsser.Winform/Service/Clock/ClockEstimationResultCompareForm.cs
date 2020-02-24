// 2016.10.09 double creates in hongqing
//2016.10.10 double edits in hongqing 先拟合再进行比较

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
    public partial class ClockEstimationResultCompareForm :Form
    {
        public ClockEstimationResultCompareForm()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_result.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Text = openFileDialog_result.FileName;
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_clock.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = openFileDialog_clock.FileName;
        }
        ClockFile ClockFile;
        ClockFile ClockFileOfClockEstimation;
        ClockFile ClockFileOfClockEstimationResult;
        private class clock
        {
            public List<string> Time=new List<string>();
            public List<double> clk=new List<double> ();
        }
        public static string[] SplitByBlank(string line)
        {
            return line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private void calculate_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var intervalSec = double.Parse(textBox_interval.Text) * 60;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

            #region 读取钟差估计结果
            Time start0 = Time.MaxValue;
            Time end0 = Time.MinValue;
            bool IsNotEnd = true;
            ClockFileOfClockEstimation = new ClockFile();
            ClockFileOfClockEstimationResult = new ClockFile();
            string[] tmp0 = null;
            using (StreamReader sr = new StreamReader(this.textBox_Pathes.Text))
            {
                string line = sr.ReadLine();
                tmp0 = SplitByBlank(line);
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
                    //string[] tmp = StringUtil.SplitByBlank(line);
                    string[] tmp = SplitByBlank(line);
                    for (int i = 0; i < tmp.Length / 2; i++)
                    {
                        AtomicClock item = new AtomicClock();
                        if (tmp[2 * i + 2] == " ")
                        {

                            item.ClockBias = 9999999999;
                            item.Time = Time.Parse(tmp[0]);
                            item.Prn = SatelliteNumber.TryParse(tmp0[2 * i + 1]);
                            item.ClockType = Data.Rinex.ClockType.Satellite;
                            item.Name = item.Prn.ToString();
                            ClockFileOfClockEstimation.GetClockItems(item.Prn).Add(item);
                            continue;
                        }

                        item.ClockBias = double.Parse(tmp[2 * i + 2]) / 1E9;
                        item.Time = Time.Parse(tmp[0]);
                        //key.Time.Seconds += 1;
                        item.Prn = SatelliteNumber.TryParse(tmp0[2 * i + 1]);
                        item.ClockType = Data.Rinex.ClockType.Satellite;
                        item.Name = item.Prn.ToString();
                        ClockFileOfClockEstimation.GetClockItems(item.Prn).Add(item);
                        if (item.Time < start0) start0 = item.Time;
                        if (item.Time > end0) end0 = item.Time;
                    }
                }
            }

            ClockFileOfClockEstimation.TimePeriod = new BufferedTimePeriod(start0, end0);
            #endregion
            this.bindingSource1.DataSource = ClockFileOfClockEstimation.AllItems;
            this.comboBox_name.DataSource = ClockFileOfClockEstimation.Names;
            #region 读取钟差文件
            string path = this.textBox1.Text;
            ClockFileReader reader = new ClockFileReader(path, false );
            ClockFile = reader.ReadAll();
            if (ClockFile.ClockCount == 0) return;
            #endregion
            #region 两个文件结果做差
            //int allSec = (int)ClockFile.TimePeriod.Span;
            //Time end = ClockFile.TimePeriod.End + intervalSec;
            //Dictionary<string, clock> V1 = new Dictionary<string, clock>();

            //double[] V = new double[32];
            //double[] x = new double[32];
            //clock q = new clock();

            //for (int j = 1; j < tmp0.Length; j++)
            //{
            //    int count = 0;
            //    var a1 = ClockFileOfClockEstimation.GetClockItems(tmp0[j], ClockFileOfClockEstimation.TimePeriod.Start, ClockFileOfClockEstimation.TimePeriod.End);
            //    var a2 = ClockFile.GetClockItems(tmp0[j], ClockFile.TimePeriod.Start, ClockFile.TimePeriod.End);
            //    for (int k = 0, k1 = 0; k < a1.Count; k++)
            //    {
            //        if (a1[k].Time == a2[k1].Time)
            //        {
            //            V[j] = a1[k].ClockBias - a2[k1].ClockBias;
            //            x[j] += V[j] * V[j];
            //            count++;
            //            k++;
            //            k1++;
            //            q.Time.Add(a1[k].Time.ToString());
            //            q.clk.Add(V[j]);
            //        }
            //        else if (a1[k].Time < a2[k1].Time)
            //            k++;
            //        else if (a1[k].Time > a2[k1].Time)
            //            k1++;
            //    }
            //    q.Time.Add("rms");
            //    q.clk.Add(Math.Sqrt(x[j] / count));

            //    V1.Add(tmp0[j], q);
            //}
            #endregion
            double interval = double.Parse(this.textBox_interval.Text);
            ClockEstimationFrom = this.dateTimePicker_from.Value;
            ClockEstimationTo = this.dateTimePicker_to.Value;
            List<AtomicClock> ClockEstimationDataSource = new List<AtomicClock>();
            List<AtomicClock> ClockFileDataSource = new List<AtomicClock>();
            List<AtomicClock> FitedResult = new List<AtomicClock>();
            //foreach (var key in ClockFileOfClockEstimation.Names)
            //{
            //    ClockEstimationDataSource = ClockFileOfClockEstimation.GetClockItems(key, new Time(ClockEstimationFrom), new Time(ClockEstimationTo));
            //    ClockFileDataSource = ClockFile.GetClockItems(key, new Time(ClockFileFrom), new Time(ClockFileTo));
            //    //ClockInterpolator interp = new ClockInterpolator(ClockEstimationDataSource);

            //    ClockEstimationDataSource = ClockFileOfClockEstimation.GetClockItems(key);
            //    ClockFileDataSource = ClockFile.GetClockItems(key);
            //    ClockInterpolator interp = new ClockInterpolator(ClockEstimationDataSource,11);
            //    List<AtomicClock> fitedResult = new List<AtomicClock>();
            //    interval = interval * 60;
            //    double cacuCount = (end0.TickTime - start0.TickTime).TotalSeconds / interval;
            //    for (int xi = 0; xi <= cacuCount-1; xi++)
            //    {
            //        Time gpsTime = ClockFileDataSource[0].Time + interval * xi;
            //        if (gpsTime > interp.MaxAvailableTime) break;
            //        fitedResult.Add(interp.GetAtomicClock(gpsTime));
            //    }
            //    for (int i = 0; i < fitedResult.Count; i++)
            //    {
            //        fitedResult[i].ClockBias -= ClockFileDataSource[i].ClockBias;
            //        FitedResult.Add(fitedResult[i]);
            //    }

            //}

            foreach (var item in ClockFileOfClockEstimation.Names)
            {
                ClockEstimationDataSource = ClockFileOfClockEstimation.GetClockItems(item);
                ClockFileDataSource = ClockFile.GetClockItems(item);
                List<AtomicClock> fitedResult = new List<AtomicClock>();
                foreach (var item1 in ClockEstimationDataSource)
                {
                    var ss = ClockFile.GetClockItem(item, item1.Time);
                    if (ss == null || item1.ClockBias == 9999999999.0)
                        continue;
                    item1.ClockBias -= ss.ClockBias;
                    FitedResult.Add(item1);
                    fitedResult.Add(item1);

                }
                ClockFileOfClockEstimationResult.Add(item, fitedResult);
            }
            ClockFileOfClockEstimation.Header = ClockFile.Header;
            ClockFileOfClockEstimation.Header.SourceName = "CE" + ClockFileOfClockEstimation.Header.SourceName;
            ClockFileOfClockEstimation.Header.CreationDate = DateTime.Now.ToString();
            ClockFileOfClockEstimation.Header.CreationAgence = "Gnsser";
            ClockFileOfClockEstimation.Header.ANALYSIS_CENTER = "Gnsser";
            ClockFileOfClockEstimation.Header.CreationProgram = "Gnsser";
            ClockFileOfClockEstimation.Header.TYPES_OF_DATA.Clear();
            ClockFileOfClockEstimation.Header.TYPES_OF_DATA.Add("AS");
            ClockFileOfClockEstimation.Header.COUNT_OF_TYPES_OF_DATA = 1;
            ClockFileOfClockEstimation.Header.ClockSolnStations.Clear();
            ClockFileOfClockEstimationResult.Header = ClockFileOfClockEstimation.Header;
            var resutlPath = Path.Combine("D:\\Temp\\ClockEstimation1\\", ClockFileOfClockEstimation.Header.SourceName);
            var errorResutlPath = Path.Combine("D:\\Temp\\ClockEstimation1\\", "error"+ClockFileOfClockEstimation.Header.SourceName);
            ClockFileWriter ClockFileWriter = new ClockFileWriter(resutlPath, ClockFileOfClockEstimation);
            ClockFileWriter.SaveToFile();
            ClockFileWriter errorClockFileWriter = new ClockFileWriter(errorResutlPath, ClockFileOfClockEstimationResult);
            errorClockFileWriter.SaveToFile();
            this.bindingSource1.DataSource = FitedResult;
            TableTextManager = new ObjectTableManager();


            TableTextManager.OutputDirectory = "D:\\Temp\\ClockEstimation1\\";

            var paramTable = TableTextManager.GetOrCreate(ClockFileOfClockEstimationResult.Name + "Params" + Time.UtcNow.DateTime.ToString("yyyy-MM-dd_HH_mm_ss"));
            int count = 0;
            SatelliteNumber prnIndex = new SatelliteNumber();
            foreach (var item in ClockFileOfClockEstimationResult)
            {
                if (item.Count > count) { count = item.Count; prnIndex = item[0].Prn; }
            }

            var standard = ClockFileOfClockEstimationResult.GetClockItems(prnIndex);
            double DoubleDiffer = 0;
            foreach (var item in standard)
            {
                paramTable.NewRow();
                paramTable.AddItem("Epoch", item.Time);
                foreach (var item1 in ClockFileOfClockEstimationResult.Names)
                {
                    if (item1 == item.Name.ToString()) continue;

                    var ss = ClockFileOfClockEstimationResult.GetClockItem(item1, item.Time);
                    if (ss == null)
                        continue;
                    DoubleDiffer = item.ClockBias - ss.ClockBias;
                    paramTable.AddItem(ss.Prn + "-" + item.Prn, DoubleDiffer*1E9);
                }
                paramTable.EndRow();
            }
            TableTextManager.WriteAllToFileAndCloseStream();
            Geo.Utils.FileUtil.OpenDirectory("D:\\Temp\\ClockEstimation1\\");
        }
        /// <summary>
        /// 表格输出管理器
        /// </summary>
        public ObjectTableManager TableTextManager { get; set; }
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

        
    }
}
