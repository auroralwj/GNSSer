//2015.01.07, czs, create in namu, 加权坐标输入

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
    public partial class RmsedXyzControl : UserControl
    {
        public RmsedXyzControl()
        {
            InitializeComponent();
            this.IsEnabled = this.checkBox_enable.Checked; 
        }

        private void checkBox_enable_CheckedChanged(object sender, EventArgs e)
        {
            this.IsEnabled = this.checkBox_enable.Checked; 
        }
        /// <summary>
        /// 是否启用坐标。
        /// </summary>
        public bool IsEnabled
        {
            get { return this.textBox_xyz.Enabled; }
            set
            {
                textBox_xyz.Enabled = value;
                textBox_rms.Enabled = value;
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public new string Text { get { return this.groupBox6.Text; } set { this.groupBox6.Text = value; } }
        /// <summary>
        /// 加权坐标
        /// </summary>
        public RmsedXYZ RmsedXyz { get { return new RmsedXYZ(Xyz, Rms); } }
        public void SetRmsedXyz(RmsedXYZ value) {  Xyz = value.Value; Rms = value.Rms; } 

        /// <summary>
        /// 坐标
        /// </summary>
        protected XYZ Xyz { get { return XYZ.Parse(this.textBox_xyz.Text); } set { this.textBox_xyz.Text = value + ""; } }
        /// <summary>
        /// 均方根
        /// </summary>
        protected XYZ Rms { get { return XYZ.Parse(this.textBox_rms.Text); } set { this.textBox_rms.Text = value + ""; } }
    }
}
