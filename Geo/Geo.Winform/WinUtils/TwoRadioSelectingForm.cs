//2018.07.15, czs, create in HMX, ������ѡ����

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
    ///������ѡ����
    /// </summary>
    public partial class TwoRadioSelectingForm : Form
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public TwoRadioSelectingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �������ʼ��
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="str2"></param>
        /// <param name="title1"></param>
        /// <param name="title2"></param>
        public TwoRadioSelectingForm(string[] strs, string [] str2, string title1 = "��ѡ��ĿA", string title2= "��ѡ��ĿA")
        {
            InitializeComponent();
            groupBox1.Text = title1;
            groupBox2.Text = title2;

            InitRadioButtons(strs, flowLayoutPanel1);
            InitRadioButtons(str2, flowLayoutPanel2);
        }

        private void InitRadioButtons(string[] strs, FlowLayoutPanel flowLayoutPanel1)
        {
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

        /// ��ѡ
        /// </summary>
        public StringPair SelectedValue { get; set; }

        void btn_CheckedChanged(object sender, EventArgs e)
        {
            var btn = (RadioButton)sender;
            if (btn.Checked) { label_info.Text = "��ǰ��" + btn.Text;  }
        } 
        private void okbutton1_Click(object sender, EventArgs e)
        {
            bool isSelected = SetValue();
            if (!isSelected)
            {
                MessageBox.Show("�����ѡ��һ�����ߡ�");
                return;
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private bool SetValue()
        {
            SelectedValue = new StringPair();
            bool isSelected = false;
            foreach (var item in flowLayoutPanel1.Controls)
            {
                if (((RadioButton)item).Checked) { isSelected = true; SelectedValue.First = ((RadioButton)item).Text; break; }
            }
            foreach (var item in flowLayoutPanel2.Controls)
            {
                if (((RadioButton)item).Checked) { isSelected = true; SelectedValue.Second = ((RadioButton)item).Text; break; }
            }

            return isSelected;
        }


        private void SelectingColForm_Load(object sender, EventArgs e)
        {

        }
    }
}