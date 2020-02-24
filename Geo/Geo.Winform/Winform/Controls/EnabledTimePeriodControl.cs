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
    public partial class EnabledTimePeriodControl : UserControl
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public EnabledTimePeriodControl()
        {
            InitializeComponent();

            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker2.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now + TimeSpan.FromDays(1);
        } 

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Title { get { return this.label_name.Text; } set { this.label_name.Text = value; } }
        /// <summary>
        /// 最小数值
        /// </summary>
        public DateTime From { get { return this.dateTimePicker1.Value; } set { this.dateTimePicker1.Value = value; } }
       /// <summary>
       /// 最大数值
       /// </summary>
        public DateTime To { get { return this.dateTimePicker2.Value; } set { this.dateTimePicker2.Value = value; } }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public EnabledTimePeriod GetEnabledValue()
        {
            return new EnabledTimePeriod(From, To) { Enabled = this.checkBox_enabled.Checked };
        } 

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="enabledVal"></param>
       public void SetEnabledValue(EnabledTimePeriod enabledVal)
        {
            this.From = enabledVal.Value.Start.DateTime;
            this.To = enabledVal.Value.End.DateTime;
            this.checkBox_enabled.Checked = enabledVal.Enabled;
        }

        private void EnabledTimePeriodControl_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now + TimeSpan.FromDays(1);
        }
    }
}
