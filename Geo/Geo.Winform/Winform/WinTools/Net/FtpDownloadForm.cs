using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Geo.WinTools.Net
{
    /// <summary>
    /// ftp下载器
    /// </summary>
    public partial class FtpDownloadForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FtpDownloadForm()
        {
            InitializeComponent();
        }

        private void button1_placeToSave_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox2_localPath.Text = this.saveFileDialog1.FileName;
            }
        }

        private void button1_down_Click(object sender, EventArgs e)
        {
            string uri = this.textBox1_uri.Text;
            string filePath = this.textBox_filePath.Text;
            string userName = this.textBox_userName.Text;
            string password = this.textBox_password.Text;
            string urlpath = uri + filePath;
            string savePath = textBox2_localPath.Text;


            Geo.Utils.NetUtil.FtpDownload(urlpath, savePath, userName, password);

             
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(savePath);
        }

  




    }
}
