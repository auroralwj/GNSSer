//2015.10.13, czs, create in hongqing, 工程配置文件
//2015.10.21, czs, edit in hongqing, 修改为用户控件

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
    public partial class ProjectInfoControl : UserControl, IWithMainForm, Geo.Winform.IEntityEditForm<GnsserProject>
    {
        public ProjectInfoControl()
        {
            InitializeComponent();
            this.Entity = new GnsserProject(); // 初始创建一个。
        }

        public ProjectInfoControl(GnsserProject Project)
        {
            InitializeComponent(); 

            this.Entity = Project; 
        }

        public IMainForm MainForm { get; set; }
        GnsserProject project;

        /// <summary>
        /// 工程项目
        /// </summary>
        public GnsserProject Entity { get { return project; } set { project = value; this.EntityToUi(); } } 

        void directoryProjectDirectory_DirectoryChanged(string val)
        {
            if (Entity.ProjectDirectory != val)
            {
                this.Entity.ProjectDirectory = val;
                UpdateGprojFilePath();
            }
        }
        private void textBox_ProjName_TextChanged(object sender, EventArgs e)
        {
            if ( !String.IsNullOrWhiteSpace(this.textBox_ProjName.Text) && Entity.ProjectName != this.textBox_ProjName.Text.Trim())
            {
                this.Entity.ProjectName = this.textBox_ProjName.Text.Trim();
                UpdateGprojFilePath();
            }
        }

        private void UpdateGprojFilePath()
        {
            this.textBox_projFilePath.Text = this.Entity.ProjectFilePath;            
            this.EntityToUi();
        }

        /// <summary>
        /// 设置为当前工程，根据工程路径，保存工程文件设置，同时保存到历史记录中。
        /// </summary>
        public void SetAsCurrentProjectAndSaveToFile()
        {
            UiToEntity();

            //Save To File
              var path = Entity.ProjectFilePath;
              if (String.IsNullOrWhiteSpace(path))
              {
                  MessageBox.Show("保存路径不可为空！");
                  return;
              }

              Entity.CheckOrCreateProjectDirectories();

              Setting.GnsserConfig.SetAsCurrentProjectAndSaveToFile(Entity); 
        }

        #region UI Entity 转换
        public void UiToEntity()
        {
            if (Entity == null) { Entity = new GnsserProject(); }

            this.Entity.ProjectName = this.textBox_ProjName.Text.Trim();

            this.Text = Entity.ProjectName + "的信息";

            this.Entity.Session = new Geo.Times.BufferedTimePeriod(this.timePeriodControl1.TimeFrom, this.timePeriodControl1.TimeTo);
            this.Entity.SatelliteTypes = this.multiGnssSystemSelectControl1.SatelliteTypes;
            this.Entity.ProjectDirectory = this.directoryProjectDirectory.Path;
            this.Entity.ObservationDirectory = this.directoryObservationDirectory.Path;
            this.Entity.CommonDirectory = this.directoryCommonDirectory.Path;
            this.Entity.MiddleDirectory = this.directoryMiddleDirectory.Path;
            this.Entity.OutputDirectory = this.directoryOutputDirectory.Path;
            this.Entity.RevisedObsDirectory = this.directoryRevisedObsDirectory.Path;
            this.Entity.ParamDirectory = this.directorySelectionControlParam.Path;
            this.Entity.ScriptDirectory = this.directorySelectionControl1Script.Path;
        }

        public void EntityToUi()
        {
            if (Entity == null) { Entity = new GnsserProject(); }

            this.Text = Entity.ProjectName + "的信息";
            this.textBox_projFilePath.Text = Entity.ProjectFilePath;

          
            this.textBox_ProjName.Text = this.Entity.ProjectName;
            this.timePeriodControl1.TimeFrom = this.Entity.Session.Start.DateTime;
            this.timePeriodControl1.TimeTo = this.Entity.Session.End.DateTime; 

            this.multiGnssSystemSelectControl1.SetSatelliteTypes(this.Entity.SatelliteTypes);
            this.directoryProjectDirectory.Path =  this.Entity.ProjectDirectory;
            this.directoryObservationDirectory.Path = this.Entity.ObservationDirectory;
            this.directoryCommonDirectory.Path = this.Entity.CommonDirectory;
            this.directoryMiddleDirectory.Path = this.Entity.MiddleDirectory;
            this.directoryOutputDirectory.Path = this.Entity.OutputDirectory;
            this.directoryRevisedObsDirectory.Path = this.Entity.RevisedObsDirectory;
            this.directorySelectionControlParam.Path =this.Entity.ParamDirectory ;
            this.directorySelectionControl1Script.Path = this.Entity.ScriptDirectory;
        }
        #endregion

    }
}
