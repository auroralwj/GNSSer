using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Geo.Winform.Tools
{
    public partial class BarCodeGeneratorForm : Form //: Geodesy.Winform.Common.HasProjsListForm  
    {
        //输出
        List<String> barCodes = new List<string>();
        //样式控制
        bool expanded = false;

        public BarCodeGeneratorForm()
        {
            InitializeComponent();
        }

        private void button_view_Click(object sender, EventArgs e)
        {
            if (expanded)
            {
                this.Height = 216;
                this.button_view.Text = "展开 >> ";              
            }
            else
            {
                this.Height = 216 *2;
                this.button_view.Text = " << 收起";
            }
            expanded = !expanded;
        }

        private void button_gen_Click(object sender, EventArgs e)
        {
            string title;
            string char1;
            string char2;
            string char3;
            int scope1Step;
            int scope1Start;
            int scope1End;
            string scope1Format;
            int scope2Step;
            int scope2Start;
            int scope2End;
            string scope2Format;
            GetValue(out title, out char1, out char2, out char3, out scope1Step, out scope1Start, out scope1End, out scope1Format, out scope2Step, out scope2Start, out scope2End, out scope2Format);

            Gen(title, char1, char2, char3, scope1Step, scope1Start, scope1End, scope1Format, scope2Step, scope2Start, scope2End, scope2Format);

        }

        private void GetValue(out string title, out string char1, out string char2, out string char3, out int scope1Step, out int scope1Start, out int scope1End, out string scope1Format, out int scope2Step, out int scope2Start, out int scope2End, out string scope2Format)
        {
            title = this.textBox_unitBarCode.Text.Trim();
            char1 = this.textBox_char1.Text.Trim();
            char2 = this.textBox_char2.Text.Trim();
            char3 = this.textBox_char3.Text.Trim();

            string scope1StartStr = this.textBox_scope1Start.Text.Trim();
            string scope1EndStr = this.textBox_scope1End.Text.Trim();

            scope1Step = int.Parse(this.textBox_scope1Step.Text);
            int scope1Len = (int)(Math.Max(scope1StartStr.Length, scope1EndStr.Length));
            scope1Start = int.Parse(scope1StartStr);
            scope1End = int.Parse(scope1EndStr);
            scope1Format = GetZeros(scope1Len);

            string scope2StartStr = this.textBox_scope2Start.Text.Trim();
            string scope2EndStr = this.textBox_scope2End.Text.Trim();

            scope2Step = int.Parse(this.textBox_scope2Step.Text);
            int scope2Len = (int)(Math.Max(scope2StartStr.Length, scope2EndStr.Length));
            scope2Start = int.Parse(scope2StartStr);
            scope2End = int.Parse(scope2EndStr);
            scope2Format = GetZeros(scope2Len);
        }

        private void Gen(string title, string char1, string char2, string char3, int scope1Step, int scope1Start, int scope1End, string scope1Format, int scope2Step, int scope2Start, int scope2End, string scope2Format)
        {

            int count = (scope2End - scope2Start) * (scope1End - scope1Start);
            if (count > 10000)
            {
                if (MessageBox.Show(" 此操作将生成 " + count + " 个编号，计算期间可能会影响计算机运行速度。是否取消？", "提示",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    return;
                }
            }

            barCodes.Clear();
            StringBuilder sb = new StringBuilder();
            for (int i = scope1Start; i <= scope1End; i += scope1Step)
            {
                for (int j = scope2Start; j <= scope2End; j += scope2Step)
                {
                    string barCode = title + char1 + i.ToString(scope1Format) +
                        char2 + j.ToString(scope2Format) + char3;
                    sb.AppendLine(barCode);
                    barCodes.Add(barCode);
                }
            }

            this.textBox_results.Text = sb.ToString();
        }


        private static string GetZeros(int scope1Len)
        {
            string scope1Format = "";
            for (int i = 0; i < scope1Len; i++)
            {
                scope1Format += "0";
            }
            return scope1Format;
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(this.saveFileDialog1.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                foreach(string line in barCodes){
                    sw.WriteLine(line + ",") ;
                }

                sw.Close();
                fs.Close();
                MessageBox.Show( "保存完毕! "+ this.saveFileDialog1.FileName);
            }

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (this.BarCodes.Count == 0)
            {
                if (MessageBox.Show("还没有生成条形码，确定退出？", "提示", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Close();
                }
                else return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox_unitBarCode_TextChanged(object sender, EventArgs e)
        {
            string title;
            string char1;
            string char2;
            string char3;
            int scope1Step;
            int scope1Start;
            int scope1End;
            string scope1Format;
            int scope2Step;
            int scope2Start;
            int scope2End;
            string scope2Format;
            GetValue(out title, out char1, out char2, out char3, out scope1Step, out scope1Start, out scope1End, out scope1Format, out scope2Step, out scope2Start, out scope2End, out scope2Format);

            string barCode = title + char1 + scope1Start.ToString(scope1Format) +
                         char2 + scope2Start.ToString(scope2Format) + char3;
            this.label_barCodeInfo.Text = "编号示例：" + barCode;
        }

        public List<String> BarCodes
        {
            get { return barCodes; }
            set { barCodes = value; }
        }
    }
}