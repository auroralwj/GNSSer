//2015.10.19, czs, create in hongqing, 操作新增文件

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
    public partial class OperationInfoCreationForm : Form, Geo.Winform.IEntityEditForm<OperationInfo>, IBaseDirecory
    {
        OperationManager OperationManager = Setting.CurrentOperationManager;
        public OperationInfoCreationForm()
        { 
            InitializeComponent();

            this.bindingSource1.DataSource = OperationManager.Keys;
            EntityToUi();
        }

        public OperationInfoCreationForm(OperationInfo OperationInfo)
        { 
            InitializeComponent();
            this.Entity = OperationInfo;
            this.bindingSource1.DataSource = OperationManager.Keys;
            EntityToUi();
        }
        /// <summary>
        /// 参数根目录
        /// </summary>
        public string BaseDirectory { get; set; }
        /// <summary>
        /// 操作信息
        /// </summary>
        public OperationInfo Entity { get; set; }
        /// <summary>
        /// 后缀名称
        /// </summary>
        public string Extension = ".param";
        
        private void button_ok_Click(object sender, EventArgs e)
        {
            var name = this.textBox_fileName.Text;
            if (String.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("请输入参数文件名称！");
                return;
            }
             name = this.textBox_name.Text;
            if (String.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("请输入任务名称！");
                return;
            }
            UiToEntity();

            Geo.Utils.FileUtil.CheckOrCreateFile(this.Entity.ParamFilePath, "# Created By Gnsser " + Geo.Utils.DateTimeUtil.GetDateTimePathStringNow() + "\r\n") ;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void EntityToUi()
        {
            if (Entity == null) { Entity = new OperationInfo(); }
            this.textBox_name.Text = Entity.Name;
            this.comboBox_OpertionName.Text = Entity.OperationName;
            this.textBox_fileName.Text = Entity.ParamFilePath;
            this.textBox_depends.Text = Entity.DependsString;
        }

        public void UiToEntity()
        {
            if (Entity == null) { Entity = new OperationInfo(); }
            Entity.Name = this.textBox_name.Text.Trim();
            Entity.OperationName= this.comboBox_OpertionName.Text;
            Entity.DependsString =this.textBox_depends.Text;
            var name =  this.textBox_fileName.Text;
          
            //check
            if(!name.ToLower().Contains(Extension)){
                var operation = OperationManager.Get(Entity.OperationName);
                name += "." +operation.ParamType.Name +  Extension;
            }

            name =  Geo.Utils.PathUtil.GetAbsPath(name, BaseDirectory);

            Entity.ParamFilePath = name;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        } 
         
    }
}
