//2017.09.30, czs, create in hongqing, 单选窗口

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Geo.Utils
{
    /// <summary>
    ///单选窗口
    /// </summary>
    public partial class RadioSelectingForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RadioSelectingForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 以数组初始化
        /// </summary>
        /// <param name="strs"></param>
        public RadioSelectingForm(string[] strs)
        {
            InitializeComponent();
            int i = 0;
            foreach (var item in strs)
            {
                RadioButton btn = new RadioButton();
                btn.AutoSize = true;
                btn.MinimumSize = new System.Drawing.Size(60, 25);
                btn.Text = item;
                btn.CheckedChanged += btn_CheckedChanged;
                if (i == 0) { btn.Checked = true; i++; }
                flowLayoutPanel1.Controls.Add(btn);
            }
        } 

        /// 所选
        /// </summary>
        public string SelectedValue { get; set; }

        void btn_CheckedChanged(object sender, EventArgs e)
        {
            var btn = (RadioButton)sender;
            if (btn.Checked) { label_info.Text = "当前：" + btn.Text; SelectedValue = btn.Text; }
        } 
        private void okbutton1_Click(object sender, EventArgs e)
        {
            bool isSelected = false;
            foreach (var item in flowLayoutPanel1.Controls)
            {
                if (((RadioButton)item).Checked) { isSelected = true; break; }
            }
            if (!isSelected)
            {
                MessageBox.Show("请多少选择一个再走。");
                return;
            }  
            this.DialogResult = DialogResult.OK;
            Close();
        }
          
         

        private void SelectingColForm_Load(object sender, EventArgs e)
        {

        }
    }
}