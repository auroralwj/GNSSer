//2017.03.07, czs, create in hongqing, 启动数据范围过滤否

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
    /// 启动数据范围过滤否
    /// </summary>
    public partial class EnabledFloatSpanControl : UserControl
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public EnabledFloatSpanControl()
        {
            InitializeComponent();
        } 

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Title { get { return this.label_name.Text; } set { this.label_name.Text = value; } }
        /// <summary>
        /// 最小数值
        /// </summary>
        public double From { get { return double.Parse(this.textBox_from.Text); } set { (this.textBox_from.Text) = value.ToString(); } }
       /// <summary>
       /// 最大数值
       /// </summary>
        public double To { get { return double.Parse(this.textBox_to.Text); } set { (this.textBox_to.Text) = value.ToString(); } }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public EnableFloatSpan GetEnabledValue()
        {
            var val = double.Parse(this.textBox_from.Text);
            var to = double.Parse(this.textBox_to.Text);
            return new EnableFloatSpan(val, to) { Enabled = this.checkBox_enabled.Checked };
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title"></param>
        /// <param name="val"></param>
       public void Init(string title, double val = 0, double to = 0)
        {
            this.textBox_to.Text = val + "";
            this.textBox_from.Text = to+ ""; 
            this.label_name.Text = title;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="enabledVal"></param>
       public void SetEnabledValue(EnableFloatSpan enabledVal)
       {
            if(enabledVal == null) { return; }

           this.textBox_from.Text = enabledVal.Value.Start + "";
           this.textBox_to.Text = enabledVal.Value.End + "";
            this.checkBox_enabled.Checked = enabledVal.Enabled;
        }

        private void checkBox_enabled_CheckedChanged(object sender, EventArgs e)
       {
           this.textBox_from.Enabled = this.checkBox_enabled.Checked;
           this.textBox_to.Enabled = this.checkBox_enabled.Checked;
        }

    }
}
