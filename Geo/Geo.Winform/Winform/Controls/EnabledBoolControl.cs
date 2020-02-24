//2016.11.27, czs, create in hongqing, 启用浮点数界面

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 启用布尔值界面
    /// </summary>
    public partial class EnabledBoolControl : UserControl
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public EnabledBoolControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public EnableBool GetEnabledValue()
        {
            var val = (this.checkBox_value.Checked);
            return new EnableBool(val) { Enabled = this.checkBox_enabled.Checked };
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title"></param>
        /// <param name="val"></param>
        public void Init(string title, bool val = false)
        {
            this.checkBox_value.Checked = val;
            this.checkBox_enabled.Text = title;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Title { get { return this.checkBox_enabled.Text; } set { this.checkBox_enabled.Text = value; } }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="enabledVal"></param>
        public void SetEnabledValue(EnableBool enabledVal)
        {
            this.checkBox_value.Checked = enabledVal.Value;
            this.checkBox_enabled.Checked = enabledVal.Enabled;
        }

        private void checkBox_enabled_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox_value.Enabled = this.checkBox_enabled.Checked;
        }

    }
}
