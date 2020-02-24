using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Utils
{
    /// <summary>
    /// 一个可以输入内容的窗口。
    /// </summary>
    public partial class InputLinesForm : Form
    {
        /// <summary>
        /// 用户输入的值
        /// </summary>
        public string[]  Lines { get; set; } 

        #region 构造函数们
        /// <summary>
        /// 构造函数
        /// </summary>
        public InputLinesForm()
        {
            InitializeComponent();
        } 
         
        #endregion

        private void button_ok_Click(object sender, EventArgs e)
        {
            string val = this.textBox_value.Text.Trim();
            if (val == "")//基础检测
            {
                FormUtil.ShowNotEmptyMessageBox();
                return;
            }
            else
            {
                Lines = this.textBox_value.Lines;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Close();
        }
    }
}
