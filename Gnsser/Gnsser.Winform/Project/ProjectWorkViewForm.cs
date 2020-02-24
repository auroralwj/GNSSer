//2015.10.13, czs, create in hongqing, 工程配置文件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform;

namespace Gnsser.Winform
{

    public partial class ProjectWorkViewForm : Form, IWithMainForm
    { 
        public ProjectWorkViewForm()
        { 
            InitializeComponent(); 

            Init();
        } 
        public ProjectWorkViewForm(string path )
        {
            InitializeComponent();
            Init(); 
        }


        public IMainForm MainForm { get { return this.workOperationControl1.MainForm; } set { this.workOperationControl1.MainForm = value; } }

        private void Init(  )
        {
            this.Entity = Setting.GnsserConfig.CurrentProject;
            this.workOperationControl1.SetWorkflow(this.Entity.Workflow);
            this.EntityToUi();
        }
        /// <summary>
        /// 工程项目
        /// </summary>
        public GnsserProject Entity { get; set; }  

        public void EntityToUi()
        {
            if (Entity == null) { Entity = new GnsserProject(); }

            this.Text = Entity.ProjectName + "的信息";

            this.textBox_ProjName.Text = this.Entity.ProjectName;
            this.timePeriodControl1.TimeFrom = this.Entity.Session.Start.DateTime;
            this.timePeriodControl1.TimeTo = this.Entity.Session.End.DateTime;
            this.textBox_projDirectory.Text = this.Entity.ProjectDirectory;
            this.multiGnssSystemSelectControl1.SetSatelliteTypes( this.Entity.SatelliteTypes); 
        }
         
        private void button_gotoRun_Click(object sender, EventArgs e)
        {
            CheckAndSaveChange();

            var gofes = Setting.GnsserConfig.CurrentProject.GetAbsGofFilePathes();//.Workflow.GofFileNames.ToArray();

            GotoGpeForm(gofes);
        }

        private void GotoGpeForm(string[] gofes)
        {
            var form = new WorkflowRunnerForm(gofes);
            form.Text = Setting.GnsserConfig.CurrentProject.ProjectName + "工作流运行器";
            this.MainForm.OpenMidForm(form);
        }
        
        private void ProjectWorkViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckAndSaveChange();
        }

        #region  方法 

        private void CheckAndSaveChange()
        {
            //保存当前项目信息
            Setting.GnsserConfig.SaveCurrentProject();

            if (!this.workOperationControl1.IsChangedSaved)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否保存更改？") == System.Windows.Forms.DialogResult.Yes)
                {
                    this.workOperationControl1.SaveChanges();
                }
            }
        }
        #endregion

        private void button_editProject_Click(object sender, EventArgs e)
        {
            var form = new ProjectEditForm(Entity);
            form.Text = Setting.GnsserConfig.CurrentProject.ProjectName+  "信息修改器";

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Init();
            }
        }

        private void button_openProjDirectory_Click(object sender, EventArgs e)
        {
            Geo.Utils.FileUtil.OpenDirectory(Entity.ProjectDirectory);
        }

    }
}
