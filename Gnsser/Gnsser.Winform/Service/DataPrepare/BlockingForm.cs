using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class BlockingForm : Form
    {
        public BlockingForm()
        {
            InitializeComponent();
        }

        private void button1_save_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void button_reset_Click(object sender, EventArgs e) { Read(); }
        private void BlockingForm_Load(object sender, EventArgs e) { Read(); }

        private void button_filterCommonSite_Click(object sender, EventArgs e)
        {
            //公共
            string [] commons = this.textBox_commonSite.Text.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>(commons);
            string [] strs =  this.textBox_blockSite.Lines;
            for (int i = 0; i < strs.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                string[] items = strs[i].Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in items)
                {
                    if (list.Contains(item)) continue;
                    else
                    {
                        sb.Append(item);
                        sb.Append(",");
                    }
                }
                strs[i] = sb.ToString();
                strs[i] = strs[i].Remove(strs[i].Length - 1); ;
            }
            this.textBox_blockSite.Lines =strs ; 
        }

        private void Read()
        {
            this.textBox_blockSite.Lines = Setting.GnsserConfig.BlockSite.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            this.textBox_commonSite.Text = Setting.GnsserConfig.BlockCommonSite;
        }
        private void Save()
        {
            Setting.GnsserConfig.BlockCommonSite = this.textBox_commonSite.Text;
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.textBox_blockSite.Lines)
            {
                sb.Append(item + ";");
            }
            Setting.GnsserConfig.BlockSite = sb.ToString();
            Setting.SaveConfigToFile();
        }


    }
}
