namespace Gnsser.Winform
{
    partial class ProjectInfoControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectInfoControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.textBox_projFilePath = new System.Windows.Forms.TextBox();
            this.textBox_ProjName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.directoryProjectDirectory = new Geo.Winform.Controls.DirectorySelectionControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.directoryRevisedObsDirectory = new Geo.Winform.Controls.DirectorySelectionControl();
            this.directorySelectionControlParam = new Geo.Winform.Controls.DirectorySelectionControl();
            this.directorySelectionControl1Script = new Geo.Winform.Controls.DirectorySelectionControl();
            this.directoryOutputDirectory = new Geo.Winform.Controls.DirectorySelectionControl();
            this.directoryMiddleDirectory = new Geo.Winform.Controls.DirectorySelectionControl();
            this.directoryCommonDirectory = new Geo.Winform.Controls.DirectorySelectionControl();
            this.directoryObservationDirectory = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.multiGnssSystemSelectControl1);
            this.groupBox1.Controls.Add(this.timePeriodControl1);
            this.groupBox1.Controls.Add(this.textBox_projFilePath);
            this.groupBox1.Controls.Add(this.textBox_ProjName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.directoryProjectDirectory);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(828, 485);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "工程文件：";
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(105, 170);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
             this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(347, 47);
            this.multiGnssSystemSelectControl1.TabIndex = 2;
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Title = "会话时段：";
            this.timePeriodControl1.Location = new System.Drawing.Point(22, 242);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(579, 30);
            this.timePeriodControl1.TabIndex = 3;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2015, 10, 13, 15, 45, 28, 222);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm";
            this.timePeriodControl1.TimeTo = new System.DateTime(2015, 10, 13, 15, 45, 28, 227);
            // 
            // textBox_projFilePath
            // 
            this.textBox_projFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_projFilePath.Enabled = false;
            this.textBox_projFilePath.Location = new System.Drawing.Point(105, 123);
            this.textBox_projFilePath.Name = "textBox_projFilePath";
            this.textBox_projFilePath.Size = new System.Drawing.Size(674, 25);
            this.textBox_projFilePath.TabIndex = 1;
            this.textBox_projFilePath.TextChanged += new System.EventHandler(this.textBox_ProjName_TextChanged);
            // 
            // textBox_ProjName
            // 
            this.textBox_ProjName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ProjName.Location = new System.Drawing.Point(105, 34);
            this.textBox_ProjName.Name = "textBox_ProjName";
            this.textBox_ProjName.Size = new System.Drawing.Size(674, 25);
            this.textBox_ProjName.TabIndex = 1;
            this.textBox_ProjName.TextChanged += new System.EventHandler(this.textBox_ProjName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "工程名称：";
            // 
            // directoryProjectDirectory
            // 
            this.directoryProjectDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryProjectDirectory.LabelName = "工程目录：";
            this.directoryProjectDirectory.Location = new System.Drawing.Point(21, 74);
            this.directoryProjectDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directoryProjectDirectory.Name = "directoryProjectDirectory";
            this.directoryProjectDirectory.Path = "";
            this.directoryProjectDirectory.Size = new System.Drawing.Size(758, 28);
            this.directoryProjectDirectory.TabIndex = 3;
            this.directoryProjectDirectory.DirectoryChanged +=  (this.directoryProjectDirectory_DirectoryChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.directoryRevisedObsDirectory);
            this.groupBox2.Controls.Add(this.directorySelectionControlParam);
            this.groupBox2.Controls.Add(this.directorySelectionControl1Script);
            this.groupBox2.Controls.Add(this.directoryOutputDirectory);
            this.groupBox2.Controls.Add(this.directoryMiddleDirectory);
            this.groupBox2.Controls.Add(this.directoryCommonDirectory);
            this.groupBox2.Controls.Add(this.directoryObservationDirectory);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(828, 485);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "路径信息";
            // 
            // directoryRevisedObsDirectory
            // 
            this.directoryRevisedObsDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryRevisedObsDirectory.LabelName = "预处理目录：";
            this.directoryRevisedObsDirectory.Location = new System.Drawing.Point(38, 146);
            this.directoryRevisedObsDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directoryRevisedObsDirectory.Name = "directoryRevisedObsDirectory";
            this.directoryRevisedObsDirectory.Path = "";
            this.directoryRevisedObsDirectory.Size = new System.Drawing.Size(773, 28);
            this.directoryRevisedObsDirectory.TabIndex = 3;
            // 
            // directorySelectionControlParam
            // 
            this.directorySelectionControlParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControlParam.LabelName = "参数目录：";
            this.directorySelectionControlParam.Location = new System.Drawing.Point(53, 254);
            this.directorySelectionControlParam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directorySelectionControlParam.Name = "directorySelectionControlParam";
            this.directorySelectionControlParam.Path = "";
            this.directorySelectionControlParam.Size = new System.Drawing.Size(758, 28);
            this.directorySelectionControlParam.TabIndex = 3;
            // 
            // directorySelectionControl1Script
            // 
            this.directorySelectionControl1Script.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1Script.LabelName = "脚本目录：";
            this.directorySelectionControl1Script.Location = new System.Drawing.Point(53, 218);
            this.directorySelectionControl1Script.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directorySelectionControl1Script.Name = "directorySelectionControl1Script";
            this.directorySelectionControl1Script.Path = "";
            this.directorySelectionControl1Script.Size = new System.Drawing.Size(758, 28);
            this.directorySelectionControl1Script.TabIndex = 3;
            // 
            // directoryOutputDirectory
            // 
            this.directoryOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryOutputDirectory.LabelName = "输出目录：";
            this.directoryOutputDirectory.Location = new System.Drawing.Point(53, 182);
            this.directoryOutputDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directoryOutputDirectory.Name = "directoryOutputDirectory";
            this.directoryOutputDirectory.Path = "";
            this.directoryOutputDirectory.Size = new System.Drawing.Size(758, 28);
            this.directoryOutputDirectory.TabIndex = 3;
            // 
            // directoryMiddleDirectory
            // 
            this.directoryMiddleDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryMiddleDirectory.LabelName = "中间目录：";
            this.directoryMiddleDirectory.Location = new System.Drawing.Point(53, 110);
            this.directoryMiddleDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directoryMiddleDirectory.Name = "directoryMiddleDirectory";
            this.directoryMiddleDirectory.Path = "";
            this.directoryMiddleDirectory.Size = new System.Drawing.Size(758, 28);
            this.directoryMiddleDirectory.TabIndex = 3;
            // 
            // directoryCommonDirectory
            // 
            this.directoryCommonDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryCommonDirectory.LabelName = "公共目录：";
            this.directoryCommonDirectory.Location = new System.Drawing.Point(53, 74);
            this.directoryCommonDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directoryCommonDirectory.Name = "directoryCommonDirectory";
            this.directoryCommonDirectory.Path = "";
            this.directoryCommonDirectory.Size = new System.Drawing.Size(758, 28);
            this.directoryCommonDirectory.TabIndex = 3;
            // 
            // directoryObservationDirectory
            // 
            this.directoryObservationDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryObservationDirectory.LabelName = "原始观测目录：";
            this.directoryObservationDirectory.Location = new System.Drawing.Point(23, 38);
            this.directoryObservationDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directoryObservationDirectory.Name = "directoryObservationDirectory";
            this.directoryObservationDirectory.Path = "";
            this.directoryObservationDirectory.Size = new System.Drawing.Size(788, 28);
            this.directoryObservationDirectory.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(842, 520);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(834, 491);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(834, 491);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "路径信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ProjectInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ProjectInfoControl";
            this.Size = new System.Drawing.Size(842, 520);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_ProjName;
        private System.Windows.Forms.Label label1;
        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private Geo.Winform.Controls.DirectorySelectionControl directoryProjectDirectory;
        private Geo.Winform.Controls.DirectorySelectionControl directoryObservationDirectory;
        private Geo.Winform.Controls.DirectorySelectionControl directoryCommonDirectory;
        private Geo.Winform.Controls.DirectorySelectionControl directoryMiddleDirectory;
        private Geo.Winform.Controls.DirectorySelectionControl directoryOutputDirectory;
        private Geo.Winform.Controls.DirectorySelectionControl directoryRevisedObsDirectory;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_projFilePath;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControlParam;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1Script;
    }
}