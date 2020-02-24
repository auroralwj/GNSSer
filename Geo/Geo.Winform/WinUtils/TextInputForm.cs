//2018.08.14, czs, create in hmx, 文本输入

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo
{
    /// <summary>
    /// 文本输入
    /// </summary>
    public partial class TextInputForm : Form
    {
        /// <summary>
        /// 文本输入
        /// </summary>
        public TextInputForm(string initContent = "")
        {
            InitializeComponent();

            this.richTextBoxControl1.Text = initContent;
        }
        /// <summary>
        /// 输入文本
        /// </summary>
        public String TextValue { get; set; }
             

        private void button_ok_Click(object sender, EventArgs e)
        {
            TextValue = richTextBoxControl1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
        }
    }
}
