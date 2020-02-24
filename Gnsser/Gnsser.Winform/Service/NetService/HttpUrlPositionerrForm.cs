using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Security;
using Geo.Utils;

namespace Gnsser.Winform
{
    public partial class HttpUrlPositionerrForm : LogListenerForm
    {
        public HttpUrlPositionerrForm()
        {
            InitializeComponent();
        }

        protected override void ShowInfo(string info)
        {
            string time = Geo.Utils.DateTimeUtil.GetTimeStringWithMiniSecondNow();
            Geo.Utils.FormUtil.InsertLineToTextBox(richTextBoxControl1, time + "\t" + info);
        }


        private void FtpDownloaderForm_Load(object sender, EventArgs e)
        {
        }

        private void button_visit_Click(object sender, EventArgs e)
        {
            try
            {
                var url = this.textBox_url.Text;
                var obsFiles = this.richTextBoxControl_obsUrls.Lines;

                if (obsFiles.Length == 0)
                {
                    MessageBox.Show("请输入配置文件再试！");
                    return;
                }
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in obsFiles)
                {
                    if (item.Contains(@":\"))
                    {
                        MessageBox.Show("请采用Internet的 FTP 或 网址，不要采用本地地址！O文件，D文件或O.Z都可以。\n" + item);
                        return;
                    }
                    if(i!= 0)
                    {
                        sb.Append(";");
                    }

                    sb.Append(item);
                    i++;
                }

                ShowInfo("即将发出计算服务请求，请耐心等待。。。。");

                Encoding encoding = Encoding.GetEncoding("utf-8");
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("obsUrls", sb.ToString());
                HttpWebResponse response = HttpWebRequestUtil.CreatePostHttpResponse(url, parameters, null, null, encoding, null);
                string cookieString = response.Headers["Set-Cookie"];

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                string srcString = reader.ReadToEnd();
                //返回值赋值
                reader.Close();

                ShowInfo("计算完毕，返回结果。");

                ShowInfo(srcString);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
         
    }
}