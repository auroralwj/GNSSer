//2014.12.04, czs, create in jinxingliaomao shuangliao jilin, 星历生成下载器
//2017.06.13, czs, edit in hongqing, 增加二位数年和年内周的表达。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using Gnsser.Times;
using System.Windows.Forms;
using Geo.Times;
using Gnsser.Data;

namespace Gnsser.Winform
{
    public partial class IgsProductDownloadForm : Form
    {
        public IgsProductDownloadForm()
        {
            InitializeComponent();

            var datasource = Enum.GetNames(typeof(IgsProductType));

            bindingSource_productTypes.DataSource = datasource;
        }

        static bool IsCancel = false;
        public string SaveDir { get { return this.directorySelectionControl1.Path; } }

        string[] FileUrls
        {
            get
            {
                string[] urls = null;
                this.Invoke(new Action(delegate()
               {
                   urls = this.richTextBoxControl_allUrls.Lines;
               }));
                return urls;
            }
        }
        string[] UrlDirectories { get { return textBox_pathDirs.Lines; } set { textBox_pathDirs.Lines = value; } }
     
        string[] SourceNames
        {
            get
            {
                return textBox_sourcenames.Text.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        string[] UrlModels { get { return textBox_model.Lines; } set { textBox_model.Lines = value; } }

        #region 人工交互
        private void button_buildPathes_Click(object sender, EventArgs e)
        {
            var type = (IgsProductType)Enum.Parse(typeof(IgsProductType), this.bindingSource_productTypes.Current.ToString());
            int step = int.Parse(this.textBox_stepHour.Text);
            string[] siteNames = namedStringControl_siteNames.GetValue().Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);


            var EphemerisPathBuilder = new IgsProductUrlPathBuilder(
                UrlDirectories,UrlModels, SourceNames,
                this.timePeriodUserControl1.TimePeriod.Start.DateTime, this.timePeriodUserControl1.TimePeriod.End.DateTime, type, step * 3600);
            EphemerisPathBuilder.SiteNames = new List<string>(siteNames);


            this.richTextBoxControl_allUrls.Lines =  EphemerisPathBuilder.Build();
            ShowInfo("地址生成成功");
            MessageBox.Show("地址生成成功！共 " + FileUrls.Length + " 条地址");
        }
        private void button_cancel_Click(object sender, EventArgs e)
        {
            ShowInfo("即将停止下载!");
            IsCancel = true;
            backgroundWorker1.CancelAsync();
        }

        private void button_download_Click(object sender, EventArgs e)
        {
            if (FileUrls.Length == 0)
            {
                MessageBox.Show("请先生成地址，或将地址粘贴到下面“全部地址”文本框中！");
                return;
            }

            IsCancel = false;
            this.progressBarComponent1.InitProcess( FileUrls.Length); 

            this.button_download.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }
        #endregion
        

        private void ShowInfo(string url)
        {
            this.Invoke(new Action(delegate()
            {
                this.textBox_result.Text = DateTime.Now + ":" + url + "\r\n" + this.textBox_result.Text;
                this.textBox_result.Update();
            }));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!Directory.Exists(SaveDir)) Directory.CreateDirectory(SaveDir);

                List<string> failed = new List<string>();
                int okCount = 0;
                foreach (string url in FileUrls)
                {
                    string info = "下载成功 ";
                    if (IsCancel || backgroundWorker1.CancellationPending)
                    {
                        ShowInfo("下载终止！");
                        break;
                    }
                    if (!Geo.Utils.NetUtil.FtpDownload(url, Path.Combine(SaveDir, Path.GetFileName(url))))
                    {
                        failed.Add(url);
                        info = "下载失败！ ";
                    }
                    else
                    {
                        okCount++;
                    }
                    ShowInfo(info + url);

                    this.Invoke(new Action(PerformStep));
                }
                //输出失败路径
                StringBuilder sb = new StringBuilder();
                foreach (string fail in failed)
                {
                    sb.AppendLine(fail);
                }
                if (sb.Length > 0)
                {
                    this.Invoke(new Action(delegate()
                    {
                        this.richTextBoxControl_failedUrls.Text = sb.ToString();
                    }));
                    ShowInfo("失败的：\r\n" + sb.ToString());
                }

                String msg = "下载完成。共" + FileUrls.Length + "，成功下载 " + okCount + " \r\n下载失败 " + failed.Count + " 个。\r\n";

                msg += "\r\n是否打开目录？";
                Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(SaveDir, msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" %>_<% 出错啦！" + ex.Message);
            }
        }

        private void PerformStep()
        {
            this.progressBarComponent1.PerformProcessStep();
            this.Update();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button_download.Enabled = true;
            ShowInfo("已经停止下载!");
            //    MessageBox.Show("已经停止下载!");
        }


        private void DownFilesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("确认关闭，关闭后将取消下载文件。")
                == System.Windows.Forms.DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
                }
                else
                    e.Cancel = true;
        }
         
        private void EphemerisDownloadForm_Load(object sender, EventArgs e) {
            this.UrlDirectories = Setting.GnsserConfig.IgsProductUrlDirectories;
            this.UrlModels = Setting.GnsserConfig.IgsProductUrlModels; 
            directorySelectionControl1.Path = Setting.TempDirectory;
        }

        private void richTextBoxControl_allUrls_TextChanged(object sender, EventArgs e) { ShowInfo("下载区共 " + FileUrls.Length + " 条地址"); }
          
        private void radioButton1IGS周解模板_CheckedChanged(object sender, EventArgs e)
        {
            textBox_model.Text = "{UrlDirectory}/{Week}/{SourceName}{SubYear}P{WeekOfYear}.{ProductType}.Z";
            this.textBox_stepHour.Text = (7 * 24) + "";
            textBox_sourcenames.Text = "igs,wum,gbm,qzf,tum,com";

        }

        private void radioButton2IGS日解模板_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox_stepHour.Text = 24 + "";
            textBox_model.Text = "{UrlDirectory}/{Week}/{SourceName}{Week}{DayOfWeek}.{ProductType}.Z";
            textBox_sourcenames.Text = "igs,wum,gbm,qzf,tum,com";

        }

        private void radioButton3IGMAS小时产品_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox_stepHour.Text = 6 + "";
            textBox_model.Text = "http://124.205.50.178/Product/TreePage/downItem/?fid=/products/products/{BdsWeek}/isu{BdsWeek}{DayOfWeek}_{Hour}.{ProductType}.Z"; // "{UrlDirectory}/{BdsWeek}/{SourceName}{Week}{DayOfWeek}.{ProductType}.Z";
            textBox_sourcenames.Text = "isc,isr,isu";

        }

        private void radioButton4IGMAS日解产品_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox_stepHour.Text = 24 + "";
            textBox_model.Text = "http://124.205.50.178/Product/TreePage/downItem/?fid=/products/products/{BdsWeek}/isu{BdsWeek}{DayOfWeek}.{ProductType}.Z"; // "{UrlDirectory}/{BdsWeek}/{SourceName}{Week}{DayOfWeek}.{ProductType}.Z";
            textBox_sourcenames.Text = "isc,isr,isu";

        }

        private void button_checkLib_Click(object sender, EventArgs e)
        {
            var allLines = this.richTextBoxControl_allUrls.Lines;
            Dictionary<string, string> allNames = new Dictionary<string, string>();
            foreach (var line in allLines)
            {
                var fileName = Path.GetFileName(line);
                allNames[fileName] = line;
            }

            var igsLocalDires = Setting.GnsserConfig.IgsProductLocalDirectories;

            List<string> notIncludes = new List<string>();
            foreach (var item in allNames)
            {
                bool isContains = false;
                foreach (var igsLocalDir in igsLocalDires)
                {
                    var localPath = Path.Combine(igsLocalDir, item.Key);
                    var localPath2 = Path.Combine(igsLocalDir, item.Key.TrimEnd('Z', '.'));


                    if (Geo.Utils.FileUtil.IsValid(localPath) 
                        || Geo.Utils.FileUtil.IsValid(localPath2))
                    {
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                {
                    notIncludes.Add(item.Value);
                }
            }
            var msg = "库存中未包含 " + notIncludes.Count + " 个, 生成地址已被替换为库存中未包含地址。";
            Geo.Utils.FormUtil.ShowOkMessageBox(msg);
            ShowInfo(msg);

            this.richTextBoxControl_allUrls.Lines = notIncludes.ToArray();

        }
         
    }
}
