using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.WinTools
{
    /// <summary>
    /// ASII码查看器
    /// </summary>
    public partial class AnsiCodeForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AnsiCodeForm()
        {
            InitializeComponent(); 
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            string timeStr = this.textBox_code.Text;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in timeStr)
            {
                if (i != 0) sb.Append(",");
                sb.Append(item + ":" + Convert.ToInt32(item));
                i++;
            }
            this.textBox_result.Text = sb.ToString();
        }

        private void AnsiCodeForm_Load(object sender, EventArgs e)
        {

        }

        private void button_showAll_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            string txt = "";
            for (byte i = 0; i < 255; i++)
            {
                if (i != 0)
                {
                  //  sb.Append(","); 
                    txt += ",";
                }
                string text = i + ":" + (char)i;// Convert.ToChar(i).ToString();
                txt += text;

              //  sb.Append(text);
            }
            this.textBox_result.Text = txt;// sb.ToString();
        }
    }
}
