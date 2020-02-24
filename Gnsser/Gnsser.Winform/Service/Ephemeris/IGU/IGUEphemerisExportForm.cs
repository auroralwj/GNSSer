//2016.11.15 double, add in zhengzhou, 根据IGU文件生成xls，分别输出IGU-O和IGU-P

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
    public partial class IGUEphemerisExportForm : Form, Gnsser.Winform.IShowLayer
    {
        public IGUEphemerisExportForm()
        {
            InitializeComponent();
        }
        public event ShowLayerHandler ShowLayer;

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_clk.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_clk.FileNames;
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var intervalSec = int.Parse(textBox_interval.Text) * 60;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

            var prns = SatelliteNumber.ParsePRNsBySplliter(textBox_satPrns.Text, new char[] { ',' });
            Dictionary<SatelliteNumber, StreamWriter> prnWriters = new Dictionary<SatelliteNumber, StreamWriter>();
            Dictionary<SatelliteNumber, StreamWriter> prnWritersOfPredicted = new Dictionary<SatelliteNumber, StreamWriter>();
            foreach (var item in prns)
            {
                var path = Path.Combine(directory, item.ToString() + "IGU-O" + ".xls");
                var pathOfPredicted = Path.Combine(directory, item.ToString() + "IGU-P" + ".xls");
                prnWriters[item] = new StreamWriter(new FileStream(path, FileMode.Create));
                prnWritersOfPredicted[item] = new StreamWriter(new FileStream(pathOfPredicted, FileMode.Create));
            }


            FileEphemerisService coll;
            string[] pathes = this.textBox_Pathes.Lines;
            var st1 = int.Parse(pathes[0].Substring(pathes[0].Length - 6, 2));
            var st2 = int.Parse(pathes[1].Substring(pathes[0].Length - 6, 2));
            #region 不进行合并，即没有重合部分
            if (st2 - st1 == 0)
            {
                foreach (var pathItem in pathes)
                {
                    coll = EphemerisDataSourceFactory.Create(pathItem);
                    Time end1 = coll.TimePeriod.Start + 24 * 3600 - intervalSec;
                    Time end = coll.TimePeriod.End;
                    foreach (var prn in prns)
                    {
                        var writer = prnWriters[prn];
                        var writer1 = prnWritersOfPredicted[prn];
                        var all = coll.Gets(prn, coll.TimePeriod.Start, end1);
                        var allOfPredicted = coll.Gets(prn, end1 + intervalSec, end);
                        for (Time i = coll.TimePeriod.Start; i <= end1; i = i + intervalSec)
                        {
                            var find = all.SingleOrDefault(m => m.Time == i);
                            if (find == null)
                            {
                                if (!fillWithZero) continue;

                                find = new Ephemeris() { Time = i, Prn = prn };
                            }
                            writer.WriteLine(find.GetTabValues());
                        }
                        for (Time i = end1 + intervalSec; i <= end; i = i + intervalSec)
                        {
                            var find = allOfPredicted.SingleOrDefault(m => m.Time == i);
                            if (find == null)
                            {
                                if (!fillWithZero) continue;

                                find = new Ephemeris() { Time = i, Prn = prn };
                            }
                            writer1.WriteLine(find.GetTabValues());
                        }
                    }
                }
            }

            #endregion
            #region 合并，即每个文件选取最新的6小时或12小时、18小时等
            else
            {
                foreach (var pathItem in pathes)
                {
                    coll = EphemerisDataSourceFactory.Create(pathItem);
                    Time end1 = coll.TimePeriod.Start + (st2 - st1) * 3600 - intervalSec;
                    Time end = coll.TimePeriod.Start + (st2 - st1 + 24) * 3600 - intervalSec;
                    Time startOfPredicted = coll.TimePeriod.Start + 24 * 3600 - intervalSec;
                    foreach (var prn in prns)
                    {
                        var writer = prnWriters[prn];
                        var writer1 = prnWritersOfPredicted[prn];
                        var all = coll.Gets(prn, coll.TimePeriod.Start, end1);
                        var allOfPredicted = coll.Gets(prn, startOfPredicted, end);
                        for (Time i = coll.TimePeriod.Start; i <= end1; i = i + intervalSec)
                        {
                            var find = all.SingleOrDefault(m => m.Time == i);
                            if (find == null)
                            {
                                if (!fillWithZero) continue;

                                find = new Ephemeris() { Time = i, Prn = prn };
                            }
                            writer.WriteLine(find.GetTabValues());
                        }
                        for (Time i = startOfPredicted; i <= end; i = i + intervalSec)
                        {
                            var find = allOfPredicted.SingleOrDefault(m => m.Time == i);
                            if (find == null)
                            {
                                if (!fillWithZero) continue;

                                find = new Ephemeris() { Time = i, Prn = prn };
                            }
                            writer1.WriteLine(find.GetTabValues());
                        }
                    }
                }
            }
            #endregion
            foreach (var item in prnWriters)
            {
                item.Value.Flush();
                item.Value.Close();
            }
            foreach (var item in prnWritersOfPredicted)
            {
                item.Value.Flush();
                item.Value.Close();
            }

            Geo.Utils.FileUtil.OpenDirectory(directory);
        }
    }
}

