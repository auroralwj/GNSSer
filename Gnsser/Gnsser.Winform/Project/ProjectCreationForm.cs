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

    public partial class ProjectCreationForm : Form, IWithMainForm
    {
        public ProjectCreationForm()
        {
            InitializeComponent();
            this.ObjectToUi(); // 初始创建一个。
        }
        public ProjectCreationForm(GnsserProject Project)
        {
            InitializeComponent(); 

            this.Project = Project;
            this.ObjectToUi(); 
        }
        public IMainForm MainForm { get; set; } 
        /// <summary>
        /// 工程项目
        /// </summary>
        public GnsserProject Project { get; set; } 

        void directoryProjectDirectory_StringValueChanged(string val)
        {
            if (Project.ProjectDirectory != val)
            {
                this.Project.ProjectDirectory = val;
               // this.Project.UpdateWithProjectDirectory();

                UpdateGprojFilePath();
            }
        }
        private void textBox_ProjName_TextChanged(object sender, EventArgs e)
        {
            if ( !String.IsNullOrWhiteSpace(this.textBox_ProjName.Text) && Project.ProjectName != this.textBox_ProjName.Text.Trim())
            {
                this.Project.ProjectName = this.textBox_ProjName.Text.Trim();
                UpdateGprojFilePath();
            }
        }

        private void UpdateGprojFilePath()
        {
            this.textBox_projFilePath.Text = this.Project.ProjectFilePath;
            
          //  Project.InitialDirectories();

            this.ObjectToUi();
        }


        private void button_create_Click(object sender, EventArgs e)
        {
            UiToObject();

            //Save To File
              var path = Project.ProjectFilePath;// this.fileOpenControl1.FilePath;
              if (String.IsNullOrWhiteSpace(path))
              {
                  MessageBox.Show("保存路径不可为空！");
                  return;
              }

              Project.CheckOrCreateProjectDirectories();

              Setting.GnsserConfig.SetAsCurrentProjectAndSaveToFile(Project); 

              this.DialogResult = System.Windows.Forms.DialogResult.OK; 
              this.Close(); 
        }
        /// <summary>
        /// 是否变化
        /// </summary>
        private void ReadAndShow()
        {
            var path = Project.ProjectFilePath;
            Geo.IO.ConfigReader reader = new Geo.IO.ConfigReader(path);
            var config = reader.Read();

            Project = new GnsserProject(config);

            ObjectToUi();
        }

        public void UiToObject()
        {
            if (Project == null) { Project = new GnsserProject(); }

            this.Project.ProjectName = this.textBox_ProjName.Text.Trim();

            this.Text = Project.ProjectName + "的信息";

            this.Project.Session = new Geo.Times.BufferedTimePeriod(this.timePeriodControl1.TimeFrom, this.timePeriodControl1.TimeTo);
            this.Project.SatelliteTypes = this.multiGnssSystemSelectControl1.SatelliteTypes;
            this.Project.ProjectDirectory = this.directoryProjectDirectory.Path;
            this.Project.ObservationDirectory = this.directoryObservationDirectory.Path;
            this.Project.CommonDirectory = this.directoryCommonDirectory.Path;
            this.Project.MiddleDirectory = this.directoryMiddleDirectory.Path;
            this.Project.OutputDirectory = this.directoryOutputDirectory.Path;
            this.Project.RevisedObsDirectory = this.directoryRevisedObsDirectory.Path;
            this.Project.ParamDirectory = this.directorySelectionControlParam.Path;
            this.Project.ScriptDirectory = this.directorySelectionControl1Script.Path;

        }

        public void ObjectToUi()
        {
            if (Project == null) { Project = new GnsserProject(); }

            this.Text = Project.ProjectName + "的信息";
            this.textBox_projFilePath.Text = Project.ProjectFilePath;

          
            this.textBox_ProjName.Text = this.Project.ProjectName;
            this.timePeriodControl1.TimeFrom = this.Project.Session.Start.DateTime;
            this.timePeriodControl1.TimeTo = this.Project.Session.End.DateTime; 

            this.multiGnssSystemSelectControl1.SetSatelliteTypes( this.Project.SatelliteTypes );
            this.directoryProjectDirectory.Path =  this.Project.ProjectDirectory;
            this.directoryObservationDirectory.Path = this.Project.ObservationDirectory;
            this.directoryCommonDirectory.Path = this.Project.CommonDirectory;
            this.directoryMiddleDirectory.Path = this.Project.MiddleDirectory;
            this.directoryOutputDirectory.Path = this.Project.OutputDirectory;
            this.directoryRevisedObsDirectory.Path = this.Project.RevisedObsDirectory;
            this.directorySelectionControlParam.Path =this.Project.ParamDirectory ;
            this.directorySelectionControl1Script.Path = this.Project.ScriptDirectory;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void directoryProjectDirectory_Load(object sender, EventArgs e)
        {

        }

    }
}
