//2017.03.16, double edit in zhengzhou,  修改了输出的文件名，加上了时间段。


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
    public partial class EphemerisExportForm : Form, Gnsser.Winform.IShowLayer
    {
        public EphemerisExportForm()
        {
            InitializeComponent();
        } 
        public event ShowLayerHandler ShowLayer;

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_nav.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_nav.FileNames;
        }

        
         FileEphemerisService singleColl;
         private void button_read_Click(object sender, EventArgs e)
         {
             bool fillWithZero = checkBox1.Checked;
             var intervalSec = int.Parse(textBox_interval.Text) * 60;
             var directory = this.directorySelectionControl1.Path;
             Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

             var prns = SatelliteNumber.ParsePRNsBySplliter(textBox_satPrns.Text, new char[] { ',' });
             Dictionary<SatelliteNumber, StreamWriter> prnWriters = new Dictionary<SatelliteNumber, StreamWriter>();


             string[] pathes = this.textBox_Pathes.Lines;
             string fileName = Path.GetFileName(pathes[0]).Substring(0, 3);
             string firstDayOfWeek = Path.GetFileNameWithoutExtension(pathes[0]).Substring(3, 5);
             string lastDayOfWeek = Path.GetFileNameWithoutExtension(pathes[pathes.Length - 1]).Substring(3, 5);
             string timePeriod = firstDayOfWeek + "-" + lastDayOfWeek;

             string type = EphemerisDataSourceFactory.GetFileEphemerisTypeFromPath(pathes[0]).ToString();
             foreach (var item in prns)
             {
                 var path = Path.Combine(directory, item.ToString() + "-" + fileName+"."+type + timePeriod + ".txt");
                 prnWriters[item] = new StreamWriter(new FileStream(path, FileMode.Create));
             }

             Time start = Time.MaxValue;
             foreach (var path in pathes)
             {
                 singleColl = EphemerisDataSourceFactory.Create(path);
                 Time start0 = singleColl.TimePeriod.Start;
                 Time end0 = singleColl.TimePeriod.End + intervalSec;
                 if (start < start0) start0 = start;
                 foreach (var prn in prns)
                 {
                     var writer = prnWriters[prn];

                     var all = singleColl.Gets(prn, start0, end0);
                     if (all == null) continue;
                     for (Time i = start0; i < end0; i = i + intervalSec)
                     {
                         var find = all.SingleOrDefault(m => m.Time == i);
                         if (find == null)
                         {
                             if (!fillWithZero) continue;

                             find = new Ephemeris() { Time = i, Prn = prn };
                         }
                         writer.WriteLine(find.GetTabValues());
                     }
                 }
                 start = end0;
             }

             foreach (var item in prnWriters)
             {
                 item.Value.Flush();
                 item.Value.Close();
             }

             Geo.Utils.FileUtil.OpenDirectory(directory);
         }
        #region 以前的版本
         //SequentialFileEphemerisService coll;
         //private void button_read_Click(object sender, EventArgs e)
         //{
         //    bool fillWithZero = checkBox1.Checked;
         //    var intervalSec = int.Parse(textBox_interval.Text) * 60;
         //    var directory = this.directorySelectionControl1.Path;
         //    Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

         //    var prns = SatelliteNumber.ParsePRNs(textBox_satPrns.Text, new char[] { ',' });
         //    Dictionary<SatelliteNumber, StreamWriter> prnWriters = new Dictionary<SatelliteNumber, StreamWriter>();


         //    EphemerisDataSourceFactory fac = new EphemerisDataSourceFactory();
         //    string[] pathes = this.textBox_Pathes.Lines;
         //    coll = new SequentialFileEphemerisService(fac, pathes);

         //    int startTime = coll.TimePeriod.Start.GetGpsWeekAndDay();
         //    int endTime = coll.TimePeriod.End.GetGpsWeekAndDay();
         //    string timePeriod = startTime.ToString() + "-" + endTime.ToString();
         //    string fileName = Path.GetFileName(pathes[0]).Substring(0, 3);


         //    foreach (var key in prns)
         //    {
         //        var path = Path.Combine(directory, key.ToString() + "-" + fileName + timePeriod + ".xls");
         //        prnWriters[key] = new StreamWriter(new FileStream(path, FileMode.Create));
         //    }


         //    int allSec = (int)coll.TimePeriod.Span;


         //    Time end = coll.TimePeriod.End + intervalSec;
         //    foreach (var prn in prns)
         //    {
         //        var writer = prnWriters[prn];

         //        var all = coll.Gets(prn, coll.TimePeriod.Start, end);

         //        for (Time i = coll.TimePeriod.Start; i <= end; i = i + intervalSec)
         //        {
         //            var find = all.SingleOrDefault(m => m.Time == i);
         //            if (find == null)
         //            {
         //                if (!fillWithZero) continue;

         //                find = new Ephemeris() { Time = i, Prn = prn };
         //            }
         //            writer.WriteLine(find.GetTabValues());
         //        }
         //    }

         //    foreach (var key in prnWriters)
         //    {
         //        key.Value.Flush();
         //        key.Value.Close();
         //    }

         //    Geo.Utils.FileUtil.OpenDirectory(directory);
         //}
        #endregion
    }

}

