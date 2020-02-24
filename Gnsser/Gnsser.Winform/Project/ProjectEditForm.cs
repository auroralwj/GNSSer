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

    public partial class ProjectEditForm : Form, IWithMainForm
    {
        public ProjectEditForm()
        {
            InitializeComponent();
            this.button_saveOrCreate.Text = "创建";

            projectInfoControl1.EntityToUi(); // 初始创建一个。
        }

        public ProjectEditForm(GnsserProject Project)
        {
            InitializeComponent();
            this.button_saveOrCreate.Text = "修改";

            this.Project = Project;
            projectInfoControl1.EntityToUi();
        }

        public IMainForm MainForm { get; set; }

        /// <summary>
        /// 工程项目
        /// </summary>
        public GnsserProject Project
        {
            get { return projectInfoControl1.Entity; }
            set { projectInfoControl1.Entity = value; }
        }

        private void button_saveOrCreate_Click(object sender, EventArgs e)
        {
            projectInfoControl1.SetAsCurrentProjectAndSaveToFile();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
