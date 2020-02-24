//2016.12.06 double create in hongqing, 基于实时数据流钟改正输出得到实时钟差文件,SSRsp3中的clockbias已经除以光速了

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
    public partial class RTEphemerisCombinationExportSp3Form : Form
    {
        public RTEphemerisCombinationExportSp3Form()
        {
            InitializeComponent();
        }

        private void button_getSSRsp3Path_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_SSRsp3.ShowDialog() == DialogResult.OK)
                this.textBox_SSRsp3Pathes.Lines = openFileDialog_SSRsp3.FileNames;
        }
        private void button_getNavPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_Nav.ShowDialog() == DialogResult.OK)
                this.textBox_NavPathes.Lines = openFileDialog_Nav.FileNames;
        }
        Sp3File SSRsp3;
        FileEphemerisService ephemeris;
        ClockFile ClockFile;
        Sp3File ErrorSSRsp3;
        ClockFile SSRMinusPreciseClockOutput;
        ClockFile SSRPlusNavOutput;
        /// <summary>
        /// 表格输出管理器
        /// </summary>
        public ObjectTableManager TableTextManager { get; set; }
        private void button_read_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
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

            #region 按照时间顺序合并
            for (int i = 0; i < fileCount; i++)
            {
                ErrorSSRsp3 = new Sp3File();
                SSRMinusPreciseClockOutput = new ClockFile();
                SSRPlusNavOutput = new ClockFile();
                Sp3Reader SSRsp3Reader = new Sp3Reader(SSRsp3Pathes[i]);
                ParamNavFileReader reader = new ParamNavFileReader(NavPathes[i]);
                SSRsp3 = SSRsp3Reader.ReadAll();
                ephemeris = new SingleParamNavFileEphService(reader.ReadGnssNavFlie());

                #region 读取钟差文件
                ClockFileReader ClockReader = new ClockFileReader(ClockPathes[i],false);
                ClockFile = ClockReader.ReadAll();
                if (ClockFile.ClockCount == 0) return;
                #endregion
                #region 以sp3的格式进行比较，发现比较麻烦
                //foreach (Sp3Section sec in SSRsp3)
                //{
                //    Sp3Section ErrorResult = new Sp3Section();
                //    Time t = sec.Time;
                //    foreach (Sp3Record rec in sec)
                //    {
                //        Sp3Record error = new Sp3Record();
                //        if (!ephemeris.Prns.Contains(rec.Prn))
                //        {
                //            rec.ClockBias = 999999.999999 * 1e6;
                //            error.Time = rec.Time;
                //            error.Prn = rec.Prn;
                //            error.ClockBias = 999999.999999 * 1e6;
                //            ErrorResult.Add(error.Prn, error);
                //            continue;
                //        }
                //        var key = ephemeris.Get(rec.Prn, t);
                //        rec.ClockBias = key.ClockBias - key.RelativeTime + rec.ClockBias;
                //        var clock=ClockFile.GetClockItem(rec.Prn.ToString(), t);
                //        error.Time = rec.Time;
                //        error.Prn = rec.Prn;
                //        error.ClockBias = clock.ClockBias - rec.ClockBias;
                //        ErrorResult.Add(error.Prn,error);                        
                //    }
                //    ErrorSSRsp3.Add(ErrorResult);
                //}
                #endregion
                #region 以clock文件格式进行比较
                List<AtomicClock> ClockFileDataSource = new List<AtomicClock>();
                foreach (var item in SSRsp3.Prns)
                {
                    ClockFileDataSource = ClockFile.GetClockItems(item);
                    List<AtomicClock> ErrorResult = new List<AtomicClock>();
                    List<AtomicClock> SSRPlusNavResult = new List<AtomicClock>();
                    foreach (var item1 in ClockFileDataSource)
                    {
                        AtomicClock item2 = new AtomicClock();
                        var clk = SSRsp3.Get(item1.Time);

                        if (item1.ClockBias == 9999999999.0 || clk == null || !clk.Contains(item1.Prn))
                        {
                            if (clk != null)
                            { }
                            item1.ClockBias = 9999999999.0;
                            item2.Time = item1.Time;
                            item2.Prn = item1.Prn;
                            item2.Name = item1.Name;
                            item2.ClockType = item1.ClockType;
                            item2.ClockBias = 9999999999.0;
                        }
                        //else if (!clk.Contains(item1.Prn))
                        //{
                        //    item1.ClockBias = 9999999999.0;
                        //    item2.Time = item1.Time;
                        //    item2.Prn = item1.Prn;
                        //    item2.Name = item1.Name;
                        //    item2.ClockType = item1.ClockType;
                        //    item2.ClockBias = 9999999999.0;
                        //}
                        else
                        {
                            var NavItem = ephemeris.Get(item1.Prn, item1.Time);
                            item2.Time = item1.Time;
                            item2.Prn = item1.Prn;
                            item2.Name = item1.Name;
                            item2.ClockType = item1.ClockType;
                            item2.ClockBias = NavItem.ClockBias - NavItem.RelativeCorrection + clk.Data[item1.Prn].ClockBias;
                            item1.ClockBias = item2.ClockBias - item1.ClockBias;
                        }
                        SSRPlusNavResult.Add(item2);
                        ErrorResult.Add(item1);
                    }
                    SSRMinusPreciseClockOutput.Add(item.ToString(), ErrorResult);
                    SSRPlusNavOutput.Add(item.ToString(), SSRPlusNavResult);
                }
                #endregion
                SSRMinusPreciseClockOutput.Header = ClockFile.Header;
                SSRMinusPreciseClockOutput.Header.SourceName = "errorSSR" + ClockFile.Header.SourceName;
                SSRMinusPreciseClockOutput.Header.CreationDate = DateTime.Now.ToString();
                SSRMinusPreciseClockOutput.Header.CreationAgence = "Gnsser";
                SSRMinusPreciseClockOutput.Header.ANALYSIS_CENTER = "Gnsser";
                SSRMinusPreciseClockOutput.Header.CreationProgram = "Gnsser";
                SSRMinusPreciseClockOutput.Header.TYPES_OF_DATA.Clear();
                SSRMinusPreciseClockOutput.Header.TYPES_OF_DATA.Add("AS");
                SSRMinusPreciseClockOutput.Header.COUNT_OF_TYPES_OF_DATA = 1;
                SSRMinusPreciseClockOutput.Header.ClockSolnStations.Clear();
                SSRPlusNavOutput.Header = SSRMinusPreciseClockOutput.Header;
                if (!System.IO.Directory.Exists(@"D:\Temp\errorSSR\"))
                {
                    System.IO.Directory.CreateDirectory(@"D:\Temp\errorSSR\");
                }
                var errorResutlPath = Path.Combine("D:\\Temp\\errorSSR\\", "errorSSR" + ClockFile.Header.SourceName);
                ClockFileWriter errorClockFileWriter = new ClockFileWriter(errorResutlPath, SSRMinusPreciseClockOutput);
                errorClockFileWriter.SaveToFile();
                
                var SSRPlusNavResutlPath = Path.Combine("D:\\Temp\\errorSSR\\", "SSR+Nav" + ClockFile.Header.SourceName);
                ClockFileWriter SSRPlusNavWriter = new ClockFileWriter(SSRPlusNavResutlPath, SSRPlusNavOutput);
                SSRPlusNavWriter.SaveToFile();

                TableTextManager = new ObjectTableManager();


                TableTextManager.OutputDirectory = "D:\\Temp\\errorSSR\\";

                var paramTable = TableTextManager.GetOrCreate(SSRMinusPreciseClockOutput.Name + "errorSSRSat");
                int count = 1000000000;
                SatelliteNumber prnIndex = new SatelliteNumber();
                foreach (var item in SSRMinusPreciseClockOutput)
                {
                    int countEx=0;
                    foreach (var item1 in item)
                    {
                        if (item1.ClockBias == 9999999999.0)
                            countEx++;
                    }
                    if (countEx < count) { count = countEx; prnIndex = item[0].Prn; }
                }

                var standard = SSRMinusPreciseClockOutput.GetClockItems(prnIndex);
                double DoubleDiffer = 0;
                foreach (var item in standard)
                {
                    paramTable.NewRow();
                    paramTable.AddItem("Epoch", item.Time);
                    foreach (var item1 in SSRMinusPreciseClockOutput.Names)
                    {
                        if (item1 == item.Name.ToString()) continue;

                        var ss = SSRMinusPreciseClockOutput.GetClockItem(item1, item.Time);
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

                int dayOfWeek = SSRsp3.Header.StartTime.GetGpsWeekAndDay();
                var path = Path.Combine("D:\\Temp\\errorSSR\\",  "RT" +SSRsp3.Name );//+ dayOfWeek.ToString() + ".sp3");
                Sp3Writer Sp3Writer = new Sp3Writer(path, SSRsp3);
                Sp3Writer.SaveToFile();
                SSRsp3.Clear();
                //var errorPath = Path.Combine("D:\\Temp\\errorSSR\\", "error" + SSRsp3Pathes[i].Substring(0, 5) + "RT" + dayOfWeek.ToString() + ".sp3");
                //Sp3Writer errorSp3Writer = new Sp3Writer(path, ErrorSSRsp3);
                //errorSp3Writer.SaveToFile();
                //ErrorSSRsp3.Clear();
            }

            #endregion
            Geo.Utils.FileUtil.OpenDirectory("D:\\Temp\\errorSSR\\");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_clock.ShowDialog() == DialogResult.OK)
                this.textBox1.Lines = openFileDialog_clock.FileNames;
        }
    }
}

