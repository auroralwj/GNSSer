//2015.10.19, czs, create in hongqing, 操作编辑文件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Api;
using System.IO;
using Geo;

namespace Gnsser.Winform
{

    public partial class OperationInfoEditForm : Form, Geo.Winform.IEntityEditForm<OperationInfo>
    {
        bool isClose = false;
        OperationManager OperationManager = Setting.CurrentOperationManager;
        public OperationInfoEditForm()
        { 
            InitializeComponent();

            this.bindingSource1.DataSource = OperationManager.Keys;
            EntityToUi();
        }

        public OperationInfoEditForm(OperationInfo OperationInfo)
        { 
            InitializeComponent();
            this.Entity = OperationInfo;
            this.bindingSource1.DataSource = OperationManager.Keys;
            EntityToUi();
        }

        /// <summary>
        /// 操作信息
        /// </summary>
        public OperationInfo Entity { get; set; }   

        private void ProjectWorkViewForm_Shown(object sender, EventArgs e)  {  if (isClose) this.Close();  }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.fileOpenControl1.FilePath))
            {
                MessageBox.Show("路径不可为空！");
                return;
            }
            if (String.IsNullOrWhiteSpace(this.textBox_name.Text))
            {
                MessageBox.Show("名称不可为空！");
                return;
            }

            UiToEntity();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void EntityToUi()
        {
            if (Entity == null) { Entity = new OperationInfo(); }
            this.textBox_name.Text = Entity.Name;
            this.comboBox_OpertionName.Text = Entity.OperationName;
            this.fileOpenControl1.FilePath =   Entity.ParamFilePath;
            this.textBox_depends.Text = Entity.DependsString;
        }
        public void UiToEntity()
        {
             if (Entity == null) { Entity = new OperationInfo(); }
             Entity.Name = this.textBox_name.Text;
            Entity.OperationName= this.comboBox_OpertionName.Text;
            Entity.ParamFilePath=  this.fileOpenControl1.FilePath;
            Entity.DependsString =this.textBox_depends.Text;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
         
    }
}
