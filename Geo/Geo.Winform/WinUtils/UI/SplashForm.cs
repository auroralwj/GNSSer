//2015.04.14, czs, edit in namu ， 补齐注释

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Geo.Utils
{
    /// <summary>
    /// 启动窗口
    /// </summary>
    public partial class SplashForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SplashForm()
        {
            InitializeComponent();
            //this.label_title.Text = Winform.Setting.Title;

           // SetAuthString("AnyInfo@163.com @ " + DateTime.Now.Year + " All Rights Reserved");
            SetAuthString("www.gnsser.com gnsser@163.com @ " + DateTime.Now.Year + " All Rights Reserved");
        }
        string title;
        /// <summary>
        /// 显示题目。
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.label_title.Text = value;
            }
        }
        /// <summary>
        /// 设置版权信息
        /// </summary>
        /// <param name="str"></param>
        public void SetAuthString(String str)
        {
            this.label_auth.Text = str;
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            LoadSplashImage();
            SetFontFormat();
            SetPosition();
            SetTimer();
        }

        private void SetFontFormat()
        { 
            this.label_title.Width = this.Size.Width;

            if (label_title.Text == "")
                this.label_title.BackColor = System.Drawing.Color.Transparent; ;

            //label_title
            this.label_title.Top = (int)(this.Height * 0.62);
            this.label_title.Width = this.Width;
            this.label_title.Left = 0;

            if (this.label_title.Text != "")
            {
                float fontSize = this.Width / this.label_title.Text.Length * 0.6f;
                this.label_title.Height = (int)fontSize * 2;
                this.label_title.Font = new System.Drawing.Font("黑体", fontSize, System.Drawing.FontStyle.Bold);//华文行楷
            }            
        }

        private void SetTimer()
        {
            this.timer1.Start();
            this.timer1.Interval = 8000;
        }

        private void LoadSplashImage()
        {
            string SplashPath = Application.StartupPath + @"\Data\Images\Splash.jpg";
            if (!File.Exists(SplashPath))
            {
                MessageBox.Show("启动图像加载失败，请存放到 " + SplashPath);
            }
            else
                using (FileStream f = new FileStream(SplashPath, FileMode.Open))
                {
                    this.BackgroundImage = Bitmap.FromStream(f);
                }
        }

        private void SetPosition()
        {
            //adjust the aboutSize and position of this page.
            var rect = System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize;//.VirtualScreen;
            this.Size = this.BackgroundImage.Size;
            //this.Width = (int)(rect.Width * 0.6);
            //this.Height = (int)(rect.Height * 0.6);
            this.Top = (int)((rect.Height - Size.Height) * 0.5);
            this.Left = (int)((rect.Width - Size.Width) * 0.5);
        }

        private void timer1_Tick(object sender, EventArgs e)        {            this.Close();        }

        private void SplashForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timer1.Stop();
          // System.Threading.Thread.CurrentThread.Abort();
        }

        private void SplashForm_Click(object sender, EventArgs e)        {            this.Close();         }
    }
}