using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform;

namespace Gnsser.Winform
{
    public partial class FtpDownloaderForm : LogListenerForm
    {
        public FtpDownloaderForm()
        {
            InitializeComponent();
        }

        protected override void ShowInfo(string info)
        {
            string time = Geo.Utils.DateTimeUtil.GetTimeStringWithMiniSecondNow();
            Geo.Utils.FormUtil.InsertLineToTextBox(richTextBoxControl1,time + "\t" + info);         
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            var urls = this.richTextBoxControl_urls.Lines;
            var local = this.directorySelectionControl1.Path;
            foreach (var item in urls)
            {
                var info = "正在下载 " + item;
                Geo.Utils.FormUtil.InsertLineToTextBox(richTextBoxControl1, info);

              var collection =  Geo.Utils.NetUtil.DownloadFtpDirecotryOrFile(item, "*.*" ,local);

                StringBuilder sb = new StringBuilder();
                
                foreach (var file in collection)
                {
                    sb.AppendLine(file);
                }

                ShowInfo("已下载： " + collection.Count + "  个文件 \r\n" + sb.ToString());
            }


        }

        private void FtpDownloaderForm_Load(object sender, EventArgs e)
        {
            this.directorySelectionControl1.Path = "D:\\Temp\\";
        }
    }
}
