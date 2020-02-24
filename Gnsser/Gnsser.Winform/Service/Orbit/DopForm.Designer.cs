namespace Gnsser.Winform
{
    partial class DopForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.fileOpenControl_prnWeight = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_ephe = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.enabledFloatControl1maxDop = new Geo.Winform.Controls.EnabledFloatControl();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.checkBox1IsSimpleModel = new System.Windows.Forms.CheckBox();
            this.timeLoopControl1 = new Geo.Winform.Controls.TimeLoopControl();
            this.namedFloatControl_cutOffAnlgle = new Geo.Winform.Controls.NamedFloatControl();
            this.geoGridLoopControl1 = new Geo.Winform.Controls.GeoGridLoopControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.button_cancel = new System.Windows.Forms.Button();
            this.logCommandControl1 = new Geo.Winform.Controls.LogCommandControl();
            this.button_run = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(872, 270);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBoxControl_result);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(864, 244);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_result.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(858, 238);
            this.richTextBoxControl_result.TabIndex = 3;
            this.richTextBoxControl_result.Text = "";
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
            this.splitContainer1.Size = new System.Drawing.Size(872, 464);
            this.splitContainer1.SplitterDistance = 190;
            this.splitContainer1.TabIndex = 3;
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
            this.splitContainer2.Panel2.Controls.Add(this.button_cancel);
            this.splitContainer2.Panel2.Controls.Add(this.logCommandControl1);
            this.splitContainer2.Panel2.Controls.Add(this.button_run);
            this.splitContainer2.Size = new System.Drawing.Size(872, 190);
            this.splitContainer2.SplitterDistance = 129;
            this.splitContainer2.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(872, 129);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.directorySelectionControl1);
            this.tabPage3.Controls.Add(this.fileOpenControl_prnWeight);
            this.tabPage3.Controls.Add(this.fileOpenControl_ephe);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(864, 103);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "输入";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(3, 47);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(858, 24);
            this.directorySelectionControl1.TabIndex = 1;
            // 
            // fileOpenControl_prnWeight
            // 
            this.fileOpenControl_prnWeight.AllowDrop = true;
            this.fileOpenControl_prnWeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_prnWeight.FilePath = "";
            this.fileOpenControl_prnWeight.FilePathes = new string[0];
            this.fileOpenControl_prnWeight.Filter = "卫星权值文件|*.SatWeight;*.PrnP|所有文件|*.*";
            this.fileOpenControl_prnWeight.FirstPath = "";
            this.fileOpenControl_prnWeight.IsMultiSelect = false;
            this.fileOpenControl_prnWeight.LabelName = "卫星权值文件：";
            this.fileOpenControl_prnWeight.Location = new System.Drawing.Point(3, 25);
            this.fileOpenControl_prnWeight.Name = "fileOpenControl_prnWeight";
            this.fileOpenControl_prnWeight.Size = new System.Drawing.Size(858, 22);
            this.fileOpenControl_prnWeight.TabIndex = 6;
            // 
            // fileOpenControl_ephe
            // 
            this.fileOpenControl_ephe.AllowDrop = true;
            this.fileOpenControl_ephe.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_ephe.FilePath = "";
            this.fileOpenControl_ephe.FilePathes = new string[0];
            this.fileOpenControl_ephe.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3;*.*P|所有文件|*.*";
            this.fileOpenControl_ephe.FirstPath = "";
            this.fileOpenControl_ephe.IsMultiSelect = false;
            this.fileOpenControl_ephe.LabelName = "星历文件：";
            this.fileOpenControl_ephe.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl_ephe.Name = "fileOpenControl_ephe";
            this.fileOpenControl_ephe.Size = new System.Drawing.Size(858, 22);
            this.fileOpenControl_ephe.TabIndex = 0;
            this.fileOpenControl_ephe.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.enabledFloatControl1maxDop);
            this.tabPage5.Controls.Add(this.multiGnssSystemSelectControl1);
            this.tabPage5.Controls.Add(this.checkBox1IsSimpleModel);
            this.tabPage5.Controls.Add(this.timeLoopControl1);
            this.tabPage5.Controls.Add(this.namedFloatControl_cutOffAnlgle);
            this.tabPage5.Controls.Add(this.geoGridLoopControl1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(864, 103);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "高级设置";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // enabledFloatControl1maxDop
            // 
            this.enabledFloatControl1maxDop.Location = new System.Drawing.Point(557, 73);
            this.enabledFloatControl1maxDop.Name = "enabledFloatControl1maxDop";
            this.enabledFloatControl1maxDop.Size = new System.Drawing.Size(199, 23);
            this.enabledFloatControl1maxDop.TabIndex = 9;
            this.enabledFloatControl1maxDop.Title = "DOP最大值限定：";
            this.enabledFloatControl1maxDop.Value = 0.1D;
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(553, 26);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(275, 39);
            this.multiGnssSystemSelectControl1.TabIndex = 7;
            // 
            // checkBox1IsSimpleModel
            // 
            this.checkBox1IsSimpleModel.AutoSize = true;
            this.checkBox1IsSimpleModel.Location = new System.Drawing.Point(723, 6);
            this.checkBox1IsSimpleModel.Name = "checkBox1IsSimpleModel";
            this.checkBox1IsSimpleModel.Size = new System.Drawing.Size(72, 16);
            this.checkBox1IsSimpleModel.TabIndex = 8;
            this.checkBox1IsSimpleModel.Text = "简略输出";
            this.checkBox1IsSimpleModel.UseVisualStyleBackColor = true;
            // 
            // timeLoopControl1
            // 
            this.timeLoopControl1.Location = new System.Drawing.Point(3, 3);
            this.timeLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timeLoopControl1.Name = "timeLoopControl1";
            this.timeLoopControl1.Size = new System.Drawing.Size(561, 30);
            this.timeLoopControl1.TabIndex = 1;
            // 
            // namedFloatControl_cutOffAnlgle
            // 
            this.namedFloatControl_cutOffAnlgle.Location = new System.Drawing.Point(569, 6);
            this.namedFloatControl_cutOffAnlgle.Name = "namedFloatControl_cutOffAnlgle";
            this.namedFloatControl_cutOffAnlgle.Size = new System.Drawing.Size(139, 23);
            this.namedFloatControl_cutOffAnlgle.TabIndex = 5;
            this.namedFloatControl_cutOffAnlgle.Title = "高度截止角(度)：";
            this.namedFloatControl_cutOffAnlgle.Value = 10D;
            // 
            // geoGridLoopControl1
            // 
            this.geoGridLoopControl1.Location = new System.Drawing.Point(7, 34);
            this.geoGridLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.geoGridLoopControl1.Name = "geoGridLoopControl1";
            this.geoGridLoopControl1.Size = new System.Drawing.Size(520, 57);
            this.geoGridLoopControl1.TabIndex = 0;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(182, 4);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(525, 34);
            this.progressBarComponent1.TabIndex = 4;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(794, 3);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(66, 30);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // logCommandControl1
            // 
            this.logCommandControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.logCommandControl1.Location = new System.Drawing.Point(0, 0);
            this.logCommandControl1.Name = "logCommandControl1";
            this.logCommandControl1.Size = new System.Drawing.Size(176, 57);
            this.logCommandControl1.TabIndex = 2;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(713, 2);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 30);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "权值文件需包括列：Epoch，PRN，Weight，其中Epoch分辨率为1天";
            // 
            // DopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 464);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DopForm";
            this.Text = "DOP计算";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DopForm_FormClosing);
            this.Load += new System.EventHandler(this.DopForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_ephe;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private Geo.Winform.Controls.LogCommandControl logCommandControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_cutOffAnlgle;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TabPage tabPage5;
        private Geo.Winform.Controls.GeoGridLoopControl geoGridLoopControl1;
        private Geo.Winform.Controls.TimeLoopControl timeLoopControl1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_prnWeight;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.Windows.Forms.CheckBox checkBox1IsSimpleModel;
        private Geo.Winform.Controls.EnabledFloatControl enabledFloatControl1maxDop;
        private System.Windows.Forms.Label label1;
    }
}