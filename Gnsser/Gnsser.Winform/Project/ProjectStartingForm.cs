using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Geo.Winform;

namespace Gnsser.Winform
{
    public partial class ProjectStartingForm : Form, IWithMainForm
    {
        public ProjectStartingForm(IMainForm MainForm)
        {
            InitializeComponent();
            Init();

            this.MainForm = MainForm;
        }
        public IMainForm MainForm { get; set; }

        private void Init()
        {
            ClearHistoryPanel();

            var pathes = Setting.GnsserConfig.HistoryProjectPathes;
            foreach (var item in pathes)
            {
                var label = new LinkLabel();
                label.AutoSize = true;
                label.Text = item;
                label.LinkClicked += label_LinkClicked;
                this.AddLinkLabel(label);
            }
        }

        void label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel label = sender as LinkLabel;
            var path = label.Text;
            if(!File.Exists(path)){
                if(Geo.Utils.FormUtil.ShowYesNoMessageBox(  "文件不存在，是否移除该记录？ " + path ) == System.Windows.Forms.DialogResult.Yes){
                    var pathes = Setting.GnsserConfig.HistoryProjectPathes;
                    if(pathes.Remove(path)){
                        Setting.GnsserConfig.HistoryProjectPathes = pathes;
                        Setting.SaveConfigToFile();//.GnsserConfig.SaveCurrentProject();
                        //重新初始化界面
                        Init();
                    }
                } 
            }else{
                Setting.GnsserConfig.CurrentProjectPath = path;
                Setting.GnsserConfig.OpenAndSetCurrentProject(path);
                MainForm.OpenMidForm(Setting.GnsserConfig.CurrentProject.ProjectName + "工程视图", typeof(ProjectWorkViewForm));
           }
        }

        private void linkLabel_createProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProjectCreationForm form = new ProjectCreationForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainForm.ForceOpenMidForm(Setting.GnsserConfig.CurrentProject.ProjectName + "工程视图", typeof(ProjectWorkViewForm));
            }
        }

        private void linkLabel_openProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          // MainForm.OpenMidForm("工程工作视图", typeof(ProjectWorkViewForm));
           OpenFileDialog dlg = new OpenFileDialog();
           dlg.DefaultExt = "*.gproj";
           if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           {
               var path = dlg.FileName;
               Setting.GnsserConfig.OpenAndSetCurrentProject(path);

               MainForm.ForceOpenMidForm("当前工程视图", typeof(ProjectWorkViewForm));
           }
        }

        private void AddLinkLabel(LinkLabel lable)
        {
            this.flowLayoutPanel1.Controls.Add(lable);
        }

        private void ClearHistoryPanel()
        {
            this.flowLayoutPanel1.Controls.Clear();
        }


    }
}
