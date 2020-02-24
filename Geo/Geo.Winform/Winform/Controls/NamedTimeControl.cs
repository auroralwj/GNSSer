//2017.07.25, czs, create in hongqing, 字符串界面 

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
    /// 字符串界面
    /// </summary>
    public partial class NamedTimeControl : UserControl
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public NamedTimeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public  DateTime GetValue()
        {
           return (this.dateTimePicker1.Value); 
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Title { get { return this.label_name.Text; } set { this.label_name.Text = value; } }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title"></param>
        /// <param name="val"></param>
        public void Init(string title, DateTime val)
        {
            this.dateTimePicker1.Value = val;
            this.label_name.Text = title;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="enabledVal"></param>
        public void SetValue(DateTime enabledVal)
        {
            this.dateTimePicker1.Value = enabledVal ; 
        }


    }
}
