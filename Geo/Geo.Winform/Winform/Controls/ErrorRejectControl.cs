//2016.08.07, czs, create in 福建永安大湖, 粗差探测界面

using System;using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 粗差探测界面
    /// </summary>
    public partial class ErrorRejectControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErrorRejectControl()
        {
            InitializeComponent();
            this.IsEnabled = this.checkBox_enable.Checked; 
        }

        private void checkBox_enable_CheckedChanged(object sender, EventArgs e)
        {
            this.IsEnabled = this.checkBox_enable.Checked; 
        }
        /// <summary>
        /// 是否启用粗差探测。
        /// </summary>
        public bool IsEnabled
        {
            get { return this.textBox_maxLimit.Enabled; }
            set
            {
                textBox_maxLimit.Enabled = value;
                this.checkBox_isRelative.Enabled = value;
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public new string Text { get { return this.groupBox6.Text; } set { this.groupBox6.Text = value; } }
        /// <summary>
        /// 是否相对误差
        /// </summary>
        public bool IsRelative { get { return this.checkBox_isRelative.Enabled; } }
        /// <summary>
        /// 最大限差
        /// </summary>
        public double MaxLimit { get { return Double.Parse(this.textBox_maxLimit.Text); } set { this.textBox_maxLimit.Text = value + ""; } }
 
    }
}
