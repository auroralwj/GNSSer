using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace Geo.Utils
{
    /// <summary>
    /// 提示等待窗口
    /// </summary>
    public partial class WaitingForm : Form
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WaitingForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 带提示信息的构造函数
        /// </summary>
        /// <param name="info"></param>
        public WaitingForm(string info)
        {
            InitializeComponent();
            this.Info = info;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="second"></param>
        public WaitingForm(string info, double second)
        {
            InitializeComponent();
            this.Info = info;
            this.interval = (int)(second * 1000);
        }
        int interval = 15000;
        private string info;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Info
        {
            get { return info; }
            set { info = value;
            if (value != null)
            {
                this.label_info.Text = info;
            }
            }
        }
        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplashForm_Load(object sender, EventArgs e)
        {
            this.timer1.Start();
            this.timer1.Interval = interval;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
            
        }
        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SplashForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timer1.Stop();
        }
        /// <summary>
        /// 点击窗口时间，然后关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SplashForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}