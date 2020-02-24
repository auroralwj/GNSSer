//2017.07.21, czs, create in hongqing, 日志控制器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.IO;

namespace Geo.Winform.Controls
{
    public partial class LogCommandControl : UserControl
    {
        public LogCommandControl()
        {
            InitializeComponent();
        }
        #region 显示和日志控制
        private void checkBox_debugModel_CheckedChanged(object sender, EventArgs e) { Setting.IsShowDebug = checkBox_debugModel.Checked; }
        private void checkBox_showWarn_CheckedChanged(object sender, EventArgs e) { Setting.IsShowWarning = checkBox_showWarn.Checked; }
        private void checkBox1_enableShowInfo_CheckedChanged(object sender, EventArgs e) { Setting.IsShowInfo = checkBox1_enableShowInfo.Checked; }

        private void checkBox_showError_CheckedChanged(object sender, EventArgs e) { Setting.IsShowError = this.checkBox_showError.Checked; }
         
        private void checkBox_showData_CheckedChanged(object sender, EventArgs e)
        {

        }
          
        #endregion 

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            checkBox_debugModel.Checked =Setting.IsShowDebug;
            checkBox1_enableShowInfo.Checked =Setting.IsShowInfo;
            checkBox_showWarn.Checked =Setting.IsShowWarning;
           this.checkBox_showError.Checked = Setting.IsShowError ;
        }
         
    }
}
