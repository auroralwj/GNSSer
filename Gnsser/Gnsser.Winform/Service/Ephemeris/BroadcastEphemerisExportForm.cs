//2017.3.18 double, create in zhengzhou, 根据广播星历文件生成txt，进行质量分析

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
    public partial class BroadcastEphemerisExportForm : Form, Gnsser.Winform.IShowLayer
    {
        public BroadcastEphemerisExportForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_clk.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_clk.FileNames;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (this.openFileDialog_Broadcast.ShowDialog() == DialogResult.OK)
                this.textBox_Broadcast.Lines = openFileDialog_Broadcast.FileNames;
        }
        private void button_read_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

            var prns = SatelliteNumber.ParsePRNsBySplliter(textBox_satPrns.Text, new char[] { ',' });
            Dictionary<SatelliteNumber, StreamWriter> prnWritersOfBroadcast = new Dictionary<SatelliteNumber, StreamWriter>();
            Dictionary<SatelliteNumber, StreamWriter> prnWritersOfBroadcastError = new Dictionary<SatelliteNumber, StreamWriter>();
            
            FileEphemerisService coll;
            FileEphemerisService coll0;
            ClockService collOfPrecise;
            
            string[] pathes = this.textBox_Broadcast.Lines;
            string[] pathesOfPrecise = this.textBox_Pathes.Lines;
            string firstDayOfWeek = GetDayOfWeekFromBroadcastFileName(pathes[0]);
            string lastDayOfWeek = GetDayOfWeekFromBroadcastFileName(pathes[pathes.Length-1]);
            string filename = Path.GetFileNameWithoutExtension(pathesOfPrecise[0]).Substring(0, 3) + GetFileEphemerisTypeFromPath(pathesOfPrecise[0]).ToString(); ;
            foreach (var item in prns)
            {
                var pathOfBroadcast = Path.Combine(directory, item.ToString() + "Broadcast" + "-" + firstDayOfWeek + "-" + lastDayOfWeek + ".txt");
                var pathOfBroadcastError = Path.Combine(directory, item.ToString() + "BroadcastError" + "-" + filename + firstDayOfWeek + "-" + lastDayOfWeek + ".txt");
                prnWritersOfBroadcast[item] = new StreamWriter(new FileStream(pathOfBroadcast, FileMode.Create));
                prnWritersOfBroadcastError[item] = new StreamWriter(new FileStream(pathOfBroadcastError, FileMode.Create));
            }

            List<string> listPath=new List<string> ();
            for(int i=0;i<pathesOfPrecise.Length;i++)
            {
                listPath.Add(pathesOfPrecise[i]);
            }
            
            #region 
            coll0 = EphemerisDataSourceFactory.Create(pathes[0]);
            foreach (var pathItem in pathes)
            {
                coll = EphemerisDataSourceFactory.Create(pathItem);
                string dayOfWeek = GetDayOfWeekFromBroadcastFileName(pathItem);
                for (int index = 0; index < listPath.Count; index++) 
                {
                    if (listPath[index].Contains(dayOfWeek))
                    {
                        collOfPrecise = new ClockService(listPath[index]);
                        Time end = collOfPrecise.TimePeriod.End;
                        foreach (var prn in prns)
                        {
                            var writerOfBroadcast = prnWritersOfBroadcast[prn];
                            var writerOfBroadcastError = prnWritersOfBroadcastError[prn];
                            var allOfBroadcast = coll.Gets(prn, coll.TimePeriod.Start, end);
                            var allOfBroadcast0 = coll0.Gets(prn, coll0.TimePeriod.Start, coll0.TimePeriod.End);
                            var allOfPrecise = collOfPrecise.GetClocks(prn);
                            if (allOfBroadcast == null)// || allOfPrecise == null || allOfPrecise.Count==0)
                                continue;
                            allOfBroadcast.Sort();
                            allOfBroadcast0.Sort();
                            if (prn.SatelliteType == SatelliteType.E)
                            { }
                            int interval = 0;
                            Time start0 = collOfPrecise.TimePeriod.Start;
                            switch (prn.SatelliteType)
                            {
                                case SatelliteType.E: interval = 600; break;
                                case SatelliteType.C: interval = 3600; break;
                                case SatelliteType.G: interval = 7200; break;
                                case SatelliteType.R: interval = 1800; start0 = collOfPrecise.TimePeriod.Start + 900; break;
                                case SatelliteType.J: interval = 896; break;
                                default: break;
                            }
                            for (Time i = start0; i <= end; i += interval)
                            {
                                var findOfPrecise = allOfPrecise.SingleOrDefault(m => m.Time == i);
                                var findOfBroadcast = allOfBroadcast.SingleOrDefault(m => m.Time == i);
                                if (findOfBroadcast == null)
                                    findOfBroadcast = allOfBroadcast0.SingleOrDefault(m => m.Time == i);
                                if (findOfPrecise == null || findOfBroadcast == null)
                                {
                                    findOfPrecise = new AtomicClock() { Time = i, Prn = prn, ClockBias = 9999999999.0 };

                                    if (findOfBroadcast == null) findOfBroadcast = new Ephemeris() { Time = i, Prn = prn, ClockBias = 9999999999.0 };
                                }
                                else findOfPrecise.ClockBias = findOfPrecise.ClockBias - findOfBroadcast.ClockBias;

                                if (findOfBroadcast.ClockBias == 9999999999.0 && prn.SatelliteType == SatelliteType.G )
                                {
                                    findOfBroadcast = allOfBroadcast.FirstOrDefault(m => Math.Abs((m.Time - i) * 1.0) < 60);
                                    if (findOfBroadcast == null) findOfBroadcast = allOfBroadcast0.FirstOrDefault(m => Math.Abs((m.Time - i) * 1.0) < 60);
                                    if (findOfBroadcast == null) findOfBroadcast = new Ephemeris() { Time = i, Prn = prn, ClockBias = 9999999999.0 }; 
                                }
                                writerOfBroadcast.WriteLine(findOfBroadcast.GetSimpleTabValues());
                                writerOfBroadcastError.WriteLine(findOfPrecise.GetTabValues());
                            }


                            //for (Time i = collOfPrecise.TimePeriod.Start; i <= end1; i = i + intervalSec)
                            //{
                            //    var findOfBroadcast = allOfBroadcast.SingleOrDefault(m => m.Time == i);
                            //    var findOfPrecise = allOfBroadcast.SingleOrDefault(m => m.Time == i);
                            //    if (findOfBroadcast == null || findOfPrecise == null)
                            //    {
                            //        if (!fillWithZero) continue;

                            //        findOfBroadcast = new Ephemeris() { Time = i, Prn = prn };
                            //    }
                            //    writerOfBroadcast.WriteLine(findOfBroadcast.GetTabValues());
                            //    findOfPrecise.ClockBias = findOfPrecise.ClockBias - findOfBroadcast.ClockBias;
                            //    writerOfBroadcastError.WriteLine(findOfPrecise.GetTabValues());
                            //}
                        }
                        coll0 = EphemerisDataSourceFactory.Create(pathItem);
                        listPath.RemoveAt(index);
                        break;
                    }
                }                
            }


            #endregion

            foreach (var item in prnWritersOfBroadcast)
            {
                item.Value.Flush();
                item.Value.Close();
            }
            foreach (var item in prnWritersOfBroadcastError)
            {
                item.Value.Flush();
                item.Value.Close();
            }

            Geo.Utils.FileUtil.OpenDirectory(directory);
        }

        private static string GetDayOfWeekFromBroadcastFileName(string pathItem)
        {
            string filename = Path.GetFileName(pathItem);
            string year = "20" + filename.Substring(9, 2);
            string day = filename.Substring(4, 3);
            string yearAndDay = year + day;
            Time time = Time.ParseYearDayString(yearAndDay);
            string dayOfWeek = time.GetGpsWeekAndDay().ToString();
            return dayOfWeek;
        }
        private static string GetFileEphemerisTypeFromPath(string filePath)
        {
            string type = "UnKnown";
            char lastChar = filePath.ToUpper()[filePath.Length - 1];
            switch (lastChar)
            {
                case 'P':
                case 'N':
                    type = ".GpsNFile";
                    break;
                case 'R':
                case 'C':
                    type = ".Compass";
                    break;
                case 'G':
                    type = ".Glonass";
                    break;
                case '3':
                    type = ".sp3";
                    break;
                case 'K': 
                    //type = ".clk";
                    //break;
                case'S':
                    type = ".clk";
                    break;
                default: break;
            }
            return type;
        }
        
    }
}

