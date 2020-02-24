//2018.09.06, czs, edit in hmx, 增加表格试图

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Winform;
using Geo;
using Geo.IO;
using Geo.Times;
using System.IO;
using Gnsser;
using Gnsser.Service;

namespace Gnsser.Winform
{
    /// <summary>
    /// IGS 宽项查看器。
    /// </summary>
    public partial class WideLaneGpsBiasViewerForm : Form
    {
        Log log = new Log( typeof(WideLaneGpsBiasViewerForm));
        public WideLaneGpsBiasViewerForm()
        {
            InitializeComponent();
        }
        WideLaneBiasItem Current { get; set; }
        private void button_view_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControlOpath.FilePath;
            WideLaneBiasItemReader reader = new WideLaneBiasItemReader(path);
            var time = dateTimePicker1.Value;
            var destTime = new Time(time.Date);
            foreach (var item in reader)
            {
                if (item.Time.Date == new Time(time.Date))
                {
                    Current = item;
                    this.richTextBoxControl1.Text = item.ToLines();
                    log.Info("查到到了产品 " + item.Time.Date);
                    break;
                }
            }
        }

        private void WideLaneGpsBiasViewerForm_Load(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = new DateTime(2013, 1, 1);
            timePeriodControl1.SetTimePerid( new TimePeriod(DateTime.Now - TimeSpan.FromDays(360), DateTime.Now));
            this.textBox_wsbUrl.Text = "ftp://ftpsedr.cls.fr/pub/igsac/Wide_lane_GPS_satellite_biais.wsb";
            this.fileOpenControlOpath.FilePath = Path.Combine(GnsserConfig.BaseDirectory, "Data\\GNSS\\Common\\Wide_lane_GPS_satellite_biais.wsb");
        }

        private void button_differ_Click(object sender, EventArgs e)
        {
            var basePrn =  baseSatSelectingControl1.SelectedPrn;
            if (basePrn.SatelliteType != SatelliteType.G)
            {
                MessageBox.Show("请选择GPS系统！目前似乎还不支持其它系统，如果支持了请尝试高版本先，若还没有请 Email To： gnsser@163.com");
                return;
            }

            if(Current == null){
                MessageBox.Show("请先选择合适的日期并加载！注意：数据更新日期具有一定的延迟性。");
                return;
            }
            var dic = Current.GetMwDiffer(basePrn);
            this.richTextBoxControl1.Text = Geo.Utils.DictionaryUtil.ToStringLines<SatelliteNumber, double>(dic);
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            Geo.Utils.FormUtil.ShowWaittingForm();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new Action(delegate()
            {
                this.button_down.Enabled = false;
            }));            

            var url = this.textBox_wsbUrl.Text;
            var name = Path.GetFileName(url);
            var dir = Gnsser.Setting.GnsserConfig.BaseDataPath;

            Geo.Utils.NetUtil.Download(url, Path.Combine(dir, name));
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(dir);

            this.Invoke(new Action(delegate()
            {
                this.button_down.Enabled = true;
            }));             
        }

        private void WideLaneGpsBiasViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
              if(  Geo.Utils.FormUtil.ShowYesNoMessageBox("尚在下载 WSB 文件，是否仍然坚决地退出？")
                   != System.Windows.Forms.DialogResult.Yes)
              {
                  e.Cancel = true;
              }
            }
        }
        WideLaneBiasService WideLaneBiasService { get; set; }
        private void button_periodView_Click(object sender, EventArgs e)
        {
            var period = this.timePeriodControl1.TimePeriod;

            var path = this.fileOpenControlOpath.FilePath;
            WideLaneBiasService = new WideLaneBiasService(path); 

            ObjectTableStorage table = new ObjectTableStorage("批量查看结果");
            for (var time = period.Start.Date; time <= period.End; time += TimeSpan.FromDays(1))
            {
                var data = WideLaneBiasService.Get(time);
                if (data == null) { continue; }

                table.NewRow();
                table.AddItem("Epoch", time.Date + TimeSpan.FromHours(12));
                foreach (var kv in data.Data.Data)
                {
                    table.AddItem(kv.Key, kv.Value);
                }  
            }
            objectTableControl1.DataBind(table);
        }

        private void Looper_Looping(Time obj)
        {
            throw new NotImplementedException();
        }

        private void button_periodDiffer_Click(object sender, EventArgs e)
        {

        }

        private void button_convertToGnsserFcb_Click(object sender, EventArgs e)
        {
            var basePrn = baseSatSelectingControl1.SelectedPrn;
            if (basePrn.SatelliteType != SatelliteType.G)
            {
                MessageBox.Show("请选择GPS系统！目前似乎还不支持其它系统，如果支持了请尝试高版本先，若还没有请 Email To： gnsser@163.com");
                return;
            }
            var toPath = Path.Combine(Setting.TempDirectory, "FcbOfDcb" + Gnsser.Setting.FcbExtension);
            FcbOfUpdWriter writer = new FcbOfUpdWriter(toPath);
            var period = this.timePeriodControl1.TimePeriod;

            var path = this.fileOpenControlOpath.FilePath;
            WideLaneBiasService = new WideLaneBiasService(path);
             
            for (var time = period.Start.Date; time <= period.End; time += TimeSpan.FromDays(1))
            {
                var data = WideLaneBiasService.Get(time);
                if (data == null) { continue; }
                FcbOfUpd fcb = new FcbOfUpd(basePrn, data);
                writer.Write(fcb);
            }
            writer.Dispose();

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Setting.TempDirectory);
        }
    }
}
