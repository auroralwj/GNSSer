//2018.07.09, czs, create in hmx, 输入两个数据

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo.Winform
{
    /// <summary>
    /// 输入两个数据
    /// </summary>
    public partial class InputTwoStringForm : Form
    {
        /// <summary>
        /// 输入两个数据
        /// </summary>
        public InputTwoStringForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据对
        /// </summary>
        public StringPair NumerialPair
        {
            get
            {
                return new StringPair(FirstValue, SecondValue);
            }
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="titles"></param>
        /// <returns></returns>
        public InputTwoStringForm SetTitle(StringPair titles)
        {
            return SetTitle(titles.First, titles.Second);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="titles"></param>
        /// <returns></returns>
        public InputTwoStringForm SetValue(StringPair titles)
        {
            return SetValue(titles.First, titles.Second);
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="title1"></param>
        /// <param name="title2"></param>
        /// <returns></returns>
        public InputTwoStringForm SetTitle(string title1, string title2)
        {
            this.namedFloatControl_val1.Title = title1;
            this.namedFloatControl_val2.Title = title2;
            return this;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public InputTwoStringForm SetValue(string val1, string val2)
        {
            this.namedFloatControl_val1.SetValue( val1);
            this.namedFloatControl_val2.SetValue( val2);
            return this;
        }
        /// <summary>
        /// 第一个值
        /// </summary>
        public string FirstValue { get; set; }
        /// <summary>
        /// 第二个值
        /// </summary>
        public string SecondValue { get; set; }

        private void button_ok_Click(object sender, EventArgs e)
        {
            FirstValue = this.namedFloatControl_val1.GetValue();
            SecondValue = this.namedFloatControl_val2.GetValue();

            this.DialogResult = DialogResult.OK;

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
    }
}
