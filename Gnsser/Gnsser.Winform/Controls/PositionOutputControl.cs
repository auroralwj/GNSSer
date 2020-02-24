using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Winform.Controls
{
    public partial class PositionOutputControl : UserControl
    {
        public PositionOutputControl()
        {
            InitializeComponent();
            this.Enabled = true;
            if (Setting.VersionType != Geo.VersionType.Development)
            {
                this.Enabled = false;
            }
            checkBox_outputResult.Checked = true;
        }
        /// <summary>
        /// 是否输出结果
        /// </summary>
        public bool IsOutputResult { get { return checkBox_outputResult.Checked; } set { checkBox_outputResult.Checked = value; } }
        /// <summary>
        /// 是否输出历元结果
        /// </summary>
        public bool IsOutputEpochInfo { get { return checkBox_outputEpochInfo.Checked; } set { checkBox_outputEpochInfo.Checked = value; } }
        /// <summary>
        /// 是否输出卫星信息
        /// </summary>
        public bool IsOutputSatInfo { get { return this.checkBox_outputSiteSat.Checked; } set { checkBox_outputSiteSat.Checked = value; } }
        /// <summary>
        /// 是否输出平差信息
        /// </summary>
        public bool IsOutputAdjust { get { return this.checkBox_outputAdjust.Checked; } set { checkBox_outputAdjust.Checked = value; } }
    }
}
