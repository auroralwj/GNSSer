using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace Geo.WinTools.Net
{
    /// <summary>
    /// 获取网页源代码
    /// </summary>
    public partial class GetWebPageSourceForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetWebPageSourceForm()
        {
            InitializeComponent();
        }

        private void button_Go_Click(object sender, EventArgs e)
        {
            string uri = this.textBox_IpPath.Text;
            WebClient client = new WebClient();
            byte[] bytes = client.DownloadData(uri);
            this.textBox1.Text = Encoding.UTF8.GetString(bytes);
        }
    }
}