//2017.07.21, czs, create in hongqing, 数据范围

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
    /// 数据范围过滤
    /// </summary>
    public partial class FloatSpanControl : UserControl
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public FloatSpanControl()
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
        public NumerialSegment GetValue()
        {
            var val = double.Parse(this.textBox_from.Text);
            var to = double.Parse(this.textBox_to.Text);
            return new NumerialSegment(val, to);
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
       public void SetValue(NumerialSegment enabledVal)
       {
           this.textBox_from.Text = enabledVal.Start + "";
           this.textBox_to.Text = enabledVal.End + ""; 
        }
         

    }
}
