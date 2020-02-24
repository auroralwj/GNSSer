//2017.09.04, czs, create in hongqing, 数组界面 

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
    /// 数组界面
    /// </summary>
    public partial class NamedArrayControl  : UserControl 
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public NamedArrayControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public double[] GetValue()
        {
            var str = (this.textBox_value.Text).Trim();
            var items = str.Split(new char[] { ',', ' ', '\t' },  StringSplitOptions.RemoveEmptyEntries);
            double[] vals = new double[items.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = Double.Parse(items[i]);
            }
            return vals;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public String [] GetLines()
        {
            return (this.textBox_value.Lines);
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
        public void Init(string title, double[] array)
        {
            SetValue(array);
            this.label_name.Text = title;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="array"></param>
        public void SetValue(double[] array)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in array)
            {
                if (i != 0) { sb.Append(","); }
                sb.Append(item);
                i++;
            }
            this.textBox_value.Text = sb.ToString() + ""; 
        }


    }
}
