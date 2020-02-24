namespace Gnsser.Ntrip.WinForms
{
    partial class LocalDataReceiverForm
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileOpenControl_nav = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_saveRawData = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.paramVectorRenderControl1 = new Geo.Winform.Controls.ParamVectorRenderControl();
            this.button_drawDxyz = new System.Windows.Forms.Button();
            this.singleSiteGnssSolverTypeSelectionControl1 = new Gnsser.Winform.SingleSiteGnssSolverTypeSelectionControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.label_info = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(749, 376);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 90;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.progressBarComponent1);
            this.splitContainer2.Panel2.Controls.Add(this.label_info);
            this.splitContainer2.Panel2.Controls.Add(this.button_stop);
            this.splitContainer2.Panel2.Controls.Add(this.button_start);
            this.splitContainer2.Size = new System.Drawing.Size(749, 199);
            this.splitContainer2.SplitterDistance = 157;
            this.splitContainer2.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(749, 157);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(597, 131);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileOpenControl_nav);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Controls.Add(this.checkBox_saveRawData);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(591, 125);
            this.groupBox1.TabIndex = 89;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通用设置";
            // 
            // fileOpenControl_nav
            // 
            this.fileOpenControl_nav.AllowDrop = true;
            this.fileOpenControl_nav.FilePath = "";
            this.fileOpenControl_nav.FilePathes = new string[0];
            this.fileOpenControl_nav.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_nav.FirstPath = "";
            this.fileOpenControl_nav.IsMultiSelect = false;
            this.fileOpenControl_nav.LabelName = "星历(若无将等待下载)：";
            this.fileOpenControl_nav.Location = new System.Drawing.Point(2, 76);
            this.fileOpenControl_nav.Name = "fileOpenControl_nav";
            this.fileOpenControl_nav.Size = new System.Drawing.Size(460, 22);
            this.fileOpenControl_nav.TabIndex = 88;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "二进制RTCM3.0(*.rtcm3)|*.Rtcm3|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "本地Rtcm文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(26, 48);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(440, 22);
            this.fileOpenControl1.TabIndex = 85;
            // 
            // checkBox_saveRawData
            // 
            this.checkBox_saveRawData.AutoSize = true;
            this.checkBox_saveRawData.Checked = true;
            this.checkBox_saveRawData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_saveRawData.Location = new System.Drawing.Point(27, 20);
            this.checkBox_saveRawData.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_saveRawData.Name = "checkBox_saveRawData";
            this.checkBox_saveRawData.Size = new System.Drawing.Size(96, 16);
            this.checkBox_saveRawData.TabIndex = 80;
            this.checkBox_saveRawData.Text = "保存原始数据";
            this.checkBox_saveRawData.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "本地目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(142, 20);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\Temp2";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\Temp2"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(369, 22);
            this.directorySelectionControl1.TabIndex = 85;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.paramVectorRenderControl1);
            this.tabPage2.Controls.Add(this.button_drawDxyz);
            this.tabPage2.Controls.Add(this.singleSiteGnssSolverTypeSelectionControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(741, 131);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "定位设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paramVectorRenderControl1.Location = new System.Drawing.Point(3, 3);
            this.paramVectorRenderControl1.Margin = new System.Windows.Forms.Padding(2);
            this.paramVectorRenderControl1.Name = "paramVectorRenderControl1";
            this.paramVectorRenderControl1.Size = new System.Drawing.Size(735, 79); 
            this.paramVectorRenderControl1.TabIndex = 15;
            // 
            // button_drawDxyz
            // 
            this.button_drawDxyz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawDxyz.Location = new System.Drawing.Point(-163, 12);
            this.button_drawDxyz.Name = "button_drawDxyz";
            this.button_drawDxyz.Size = new System.Drawing.Size(86, 30);
            this.button_drawDxyz.TabIndex = 14;
            this.button_drawDxyz.Text = "绘坐标偏差图";
            this.button_drawDxyz.UseVisualStyleBackColor = true;
            this.button_drawDxyz.Click += new System.EventHandler(this.button_drawDxyz_Click);
            // 
            // singleSiteGnssSolverTypeSelectionControl1
            // 
            this.singleSiteGnssSolverTypeSelectionControl1.CurrentdType = Gnsser.SingleSiteGnssSolverType.无电离层组合PPP;
            this.singleSiteGnssSolverTypeSelectionControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.singleSiteGnssSolverTypeSelectionControl1.Location = new System.Drawing.Point(3, 89);
            this.singleSiteGnssSolverTypeSelectionControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.singleSiteGnssSolverTypeSelectionControl1.Name = "singleSiteGnssSolverTypeSelectionControl1";
            this.singleSiteGnssSolverTypeSelectionControl1.Size = new System.Drawing.Size(735, 39);
            this.singleSiteGnssSolverTypeSelectionControl1.TabIndex = 13;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(233, 3);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(342, 34);
            this.progressBarComponent1.TabIndex = 2;
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(7, 7);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(29, 12);
            this.label_info.TabIndex = 1;
            this.label_info.Text = "信息";
            // 
            // button_stop
            // 
            this.button_stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_stop.Location = new System.Drawing.Point(662, 5);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 32);
            this.button_stop.TabIndex = 0;
            this.button_stop.Text = "停止";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Location = new System.Drawing.Point(581, 4);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 32);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "启动";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(749, 173);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(741, 147);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "操作信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(735, 141);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // LocalDataReceiverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 376);
            this.Controls.Add(this.splitContainer1);
            this.Name = "LocalDataReceiverForm";
            this.Text = "本地Ntrip数据接收器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LocalDataReceiverForm_FormClosing);
            this.Load += new System.EventHandler(this.DataReceiverForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_saveRawData;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Label label_info;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_nav;
        private System.Windows.Forms.TabPage tabPage2;
        private Winform.SingleSiteGnssSolverTypeSelectionControl singleSiteGnssSolverTypeSelectionControl1;
        private System.Windows.Forms.Button button_drawDxyz;
        private Geo.Winform.Controls.ParamVectorRenderControl paramVectorRenderControl1;
    }
}