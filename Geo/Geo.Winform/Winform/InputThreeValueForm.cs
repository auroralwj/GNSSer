//2018.05.28, czs, create in hmx, 输入两个数据

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
    public partial class InputThreeValueForm : Form
    {
        /// <summary>
        /// 输入两个数据
        /// </summary>
        public InputThreeValueForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据对
        /// </summary>
        public NumerialTriple Numerials
        {
            get
            {
                return new NumerialTriple(FirstValue, SecondValue, this.ThirdValue);
            }
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="titles"></param>
        /// <returns></returns>
        public InputThreeValueForm SetTitle(StringTriple titles)
        {
            return SetTitle(titles.First, titles.Second, titles.Third);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="titles"></param>
        /// <returns></returns>
        public InputThreeValueForm SetValue(NumerialTriple titles)
        {
            return SetValue(titles.First, titles.Second, titles.Third);
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="title1"></param>
        /// <param name="title2"></param>
        /// <returns></returns>
        public InputThreeValueForm SetTitle(string title1, string title2, string title3)
        {
            this.namedFloatControl_val1.Title = title1;
            this.namedFloatControl_val2.Title = title2;
            this.namedFloatControl_val3.Title = title3;
            return this;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public InputThreeValueForm SetValue(double val1, double val2, double val3)
        {
            this.namedFloatControl_val1.Value = val1;
            this.namedFloatControl_val2.Value = val2;
            this.namedFloatControl_val3.Value = val3;
            return this;
        }
        /// <summary>
        /// 第一个值
        /// </summary>
        public double FirstValue { get; set; }
        /// <summary>
        /// 第3个值
        /// </summary>
        public double ThirdValue { get; set; }
        /// <summary>
        /// 第二个值
        /// </summary>
        public double SecondValue { get; set; }

        private void button_ok_Click(object sender, EventArgs e)
        {
            FirstValue = this.namedFloatControl_val1.Value;
            SecondValue = this.namedFloatControl_val2.Value;
            ThirdValue = this.namedFloatControl_val3.Value;

            this.DialogResult = DialogResult.OK;

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
    }
}
