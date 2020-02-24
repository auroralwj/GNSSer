namespace Gnsser.Winform
{
    partial class ProjectWorkViewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectWorkViewForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.textBox_projDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ProjName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_gotoRun = new System.Windows.Forms.Button();
            this.workOperationControl1 = new Gnsser.Winform.WorkOperationControl();
            this.button_editProject = new System.Windows.Forms.Button();
            this.button_openProjDirectory = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.multiGnssSystemSelectControl1);
            this.groupBox1.Controls.Add(this.timePeriodControl1);
            this.groupBox1.Controls.Add(this.textBox_projDirectory);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_ProjName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(751, 128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工程概要信息";
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiGnssSystemSelectControl1.Enabled = false;
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(587, 12);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(158, 78);
            this.multiGnssSystemSelectControl1.TabIndex = 2;
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Enabled = false;
            this.timePeriodControl1.Title = "会话时段：";
            this.timePeriodControl1.Location = new System.Drawing.Point(19, 60);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(579, 30);
            this.timePeriodControl1.TabIndex = 3;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2015, 10, 13, 15, 45, 28, 222);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm";
            this.timePeriodControl1.TimeTo = new System.DateTime(2015, 10, 13, 15, 45, 28, 227);
            // 
            // textBox_projDirectory
            // 
            this.textBox_projDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_projDirectory.Enabled = false;
            this.textBox_projDirectory.Location = new System.Drawing.Point(108, 96);
            this.textBox_projDirectory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_projDirectory.Name = "textBox_projDirectory";
            this.textBox_projDirectory.Size = new System.Drawing.Size(637, 25);
            this.textBox_projDirectory.TabIndex = 1;
            this.textBox_projDirectory.Text = "projDirectory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "工程目录：";
            // 
            // textBox_ProjName
            // 
            this.textBox_ProjName.Enabled = false;
            this.textBox_ProjName.Location = new System.Drawing.Point(115, 22);
            this.textBox_ProjName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_ProjName.Name = "textBox_ProjName";
            this.textBox_ProjName.Size = new System.Drawing.Size(272, 25);
            this.textBox_ProjName.TabIndex = 1;
            this.textBox_ProjName.Text = "ProjectName";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "工程名称：";
            // 
            // button_gotoRun
            // 
            this.button_gotoRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_gotoRun.Location = new System.Drawing.Point(650, 144);
            this.button_gotoRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_gotoRun.Name = "button_gotoRun";
            this.button_gotoRun.Size = new System.Drawing.Size(112, 38);
            this.button_gotoRun.TabIndex = 4;
            this.button_gotoRun.Text = "去运行";
            this.button_gotoRun.UseVisualStyleBackColor = true;
            this.button_gotoRun.Click += new System.EventHandler(this.button_gotoRun_Click);
            // 
            // workOperationControl1
            // 
            this.workOperationControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workOperationControl1.Location = new System.Drawing.Point(12, 188);
            this.workOperationControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.workOperationControl1.Name = "workOperationControl1";
            this.workOperationControl1.Size = new System.Drawing.Size(750, 333);
            this.workOperationControl1.TabIndex = 1;
            // 
            // button_editProject
            // 
            this.button_editProject.Location = new System.Drawing.Point(13, 145);
            this.button_editProject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_editProject.Name = "button_editProject";
            this.button_editProject.Size = new System.Drawing.Size(123, 38);
            this.button_editProject.TabIndex = 5;
            this.button_editProject.Text = "修改工程信息";
            this.button_editProject.UseVisualStyleBackColor = true;
            this.button_editProject.Click += new System.EventHandler(this.button_editProject_Click);
            // 
            // button_openProjDirectory
            // 
            this.button_openProjDirectory.Location = new System.Drawing.Point(141, 145);
            this.button_openProjDirectory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_openProjDirectory.Name = "button_openProjDirectory";
            this.button_openProjDirectory.Size = new System.Drawing.Size(123, 38);
            this.button_openProjDirectory.TabIndex = 5;
            this.button_openProjDirectory.Text = "打开工程目录";
            this.button_openProjDirectory.UseVisualStyleBackColor = true;
            this.button_openProjDirectory.Click += new System.EventHandler(this.button_openProjDirectory_Click);
            // 
            // ProjectWorkViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 533);
            this.Controls.Add(this.button_openProjDirectory);
            this.Controls.Add(this.button_editProject);
            this.Controls.Add(this.button_gotoRun);
            this.Controls.Add(this.workOperationControl1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ProjectWorkViewForm";
            this.Text = "工程工作视图";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectWorkViewForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_ProjName;
        private System.Windows.Forms.Label label1;
        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private WorkOperationControl workOperationControl1;
        private System.Windows.Forms.Button button_gotoRun;
        private System.Windows.Forms.TextBox textBox_projDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_editProject;
        private System.Windows.Forms.Button button_openProjDirectory;
    }
}