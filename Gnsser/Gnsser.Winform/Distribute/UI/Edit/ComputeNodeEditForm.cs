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
    public partial class ComputeNodeEditForm : Form
    {   
        public ComputeNodeEditForm()
        {
            InitializeComponent();

            Init();
        }
        public ComputeNodeEditForm(BaseComputeNode ComputeNode)
        {
            InitializeComponent();
            Init();
            this.ComputeNode = ComputeNode;
        }

        private void Init()
        {
            //this.bindingSource1.DataSource = TaskMgr.GetTasks();
        }

        BaseComputeNode computeNode;
        public BaseComputeNode ComputeNode
        {
            get { return computeNode; }
            set
            {
                computeNode = value;
                this.textBox_name.Text = value.Name;
                this.textBox_number.Text = value.Id.ToString();
                this.textBox_ip.Text = value.Ip;
                this.textBox_port.Text = value.Port+""; 
                this.checkBox_enabled.Checked = value.Enabled;
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (ComputeNode == null)
            {
                computeNode = new BaseComputeNode();
            }
            try
            {
                ComputeNode.Name = this.textBox_name.Text.Trim();
                ComputeNode.Ip = this.textBox_ip.Text.Trim();
                ComputeNode.Port = int.Parse(this.textBox_port.Text.Trim());
                ComputeNode.Id = int.Parse(this.textBox_number.Text); 
                ComputeNode.Enabled = this.checkBox_enabled.Checked;
               
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
