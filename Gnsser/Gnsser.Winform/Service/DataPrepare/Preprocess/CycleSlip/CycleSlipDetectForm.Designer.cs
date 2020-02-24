namespace Gnsser.Winform
{
    partial class CycleSlipDetectForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gnssSystemSelectControl1 = new Gnsser.Winform.Controls.GnssSystemSelectControl();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.enumRadioControl1 = new Geo.Winform.EnumRadioControl();
            this.button_setting = new System.Windows.Forms.Button();
            this.enumCheckBoxControl1 = new Geo.Winform.EnumCheckBoxControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_startEpoch = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_caculateCount = new System.Windows.Forms.TextBox();
            this.button_draw = new System.Windows.Forms.Button();
            this.checkBox_isreversed = new System.Windows.Forms.CheckBox();
            this.button_process = new System.Windows.Forms.Button();
            this.checkBox_debugModel = new System.Windows.Forms.CheckBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(745, 447);
            this.splitContainer1.SplitterDistance = 164;
            this.splitContainer1.TabIndex = 34;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(745, 164);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gnssSystemSelectControl1);
            this.tabPage1.Controls.Add(this.fileOpenControl1);
            this.tabPage1.Controls.Add(this.directorySelectionControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(564, 138);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文件输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gnssSystemSelectControl1
            // 
            this.gnssSystemSelectControl1.Location = new System.Drawing.Point(15, 71);
            this.gnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gnssSystemSelectControl1.Name = "gnssSystemSelectControl1";
            this.gnssSystemSelectControl1.Size = new System.Drawing.Size(461, 38);
            this.gnssSystemSelectControl1.TabIndex = 7;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "Rinex观测文件|*.*o|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "观测文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(19, 6);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(527, 22);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(19, 34);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(527, 22);
            this.directorySelectionControl1.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.enumRadioControl1);
            this.tabPage2.Controls.Add(this.button_setting);
            this.tabPage2.Controls.Add(this.enumCheckBoxControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(737, 138);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "详细设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl1
            // 
            this.enumRadioControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl1.Location = new System.Drawing.Point(15, 73);
            this.enumRadioControl1.Name = "enumRadioControl1";
            this.enumRadioControl1.Size = new System.Drawing.Size(719, 64);
            this.enumRadioControl1.TabIndex = 5;
            this.enumRadioControl1.Title = "载波类型";
            // 
            // button_setting
            // 
            this.button_setting.Location = new System.Drawing.Point(6, 3);
            this.button_setting.Name = "button_setting";
            this.button_setting.Size = new System.Drawing.Size(74, 51);
            this.button_setting.TabIndex = 8;
            this.button_setting.Text = "设置";
            this.button_setting.UseVisualStyleBackColor = true;
            this.button_setting.Click += new System.EventHandler(this.button_setting_Click);
            // 
            // enumCheckBoxControl1
            // 
            this.enumCheckBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumCheckBoxControl1.Location = new System.Drawing.Point(86, 6);
            this.enumCheckBoxControl1.Name = "enumCheckBoxControl1";
            this.enumCheckBoxControl1.Size = new System.Drawing.Size(643, 61);
            this.enumCheckBoxControl1.TabIndex = 4;
            this.enumCheckBoxControl1.Title = "探测器";
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
            this.splitContainer2.Panel1.Controls.Add(this.progressBarComponent1);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox11);
            this.splitContainer2.Panel1.Controls.Add(this.button_draw);
            this.splitContainer2.Panel1.Controls.Add(this.checkBox_isreversed);
            this.splitContainer2.Panel1.Controls.Add(this.button_process);
            this.splitContainer2.Panel1.Controls.Add(this.checkBox_debugModel);
            this.splitContainer2.Panel1.Controls.Add(this.button_cancel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBoxControl1);
            this.splitContainer2.Size = new System.Drawing.Size(745, 279);
            this.splitContainer2.SplitterDistance = 69;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label11);
            this.groupBox11.Controls.Add(this.textBox_startEpoch);
            this.groupBox11.Controls.Add(this.label10);
            this.groupBox11.Controls.Add(this.textBox_caculateCount);
            this.groupBox11.Location = new System.Drawing.Point(0, 2);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox11.Size = new System.Drawing.Size(149, 64);
            this.groupBox11.TabIndex = 32;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "计算数量";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "起始历元：";
            // 
            // textBox_startEpoch
            // 
            this.textBox_startEpoch.Location = new System.Drawing.Point(87, 15);
            this.textBox_startEpoch.Name = "textBox_startEpoch";
            this.textBox_startEpoch.Size = new System.Drawing.Size(51, 21);
            this.textBox_startEpoch.TabIndex = 18;
            this.textBox_startEpoch.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "计算历元数：";
            // 
            // textBox_caculateCount
            // 
            this.textBox_caculateCount.Location = new System.Drawing.Point(87, 39);
            this.textBox_caculateCount.Name = "textBox_caculateCount";
            this.textBox_caculateCount.Size = new System.Drawing.Size(51, 21);
            this.textBox_caculateCount.TabIndex = 18;
            this.textBox_caculateCount.Text = "10000";
            // 
            // button_draw
            // 
            this.button_draw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_draw.Location = new System.Drawing.Point(579, 39);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(75, 23);
            this.button_draw.TabIndex = 10;
            this.button_draw.Text = "绘图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // checkBox_isreversed
            // 
            this.checkBox_isreversed.AutoSize = true;
            this.checkBox_isreversed.Location = new System.Drawing.Point(175, 39);
            this.checkBox_isreversed.Name = "checkBox_isreversed";
            this.checkBox_isreversed.Size = new System.Drawing.Size(96, 16);
            this.checkBox_isreversed.TabIndex = 33;
            this.checkBox_isreversed.Text = "逆序辅助探测";
            this.checkBox_isreversed.UseVisualStyleBackColor = true;
            // 
            // button_process
            // 
            this.button_process.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_process.Location = new System.Drawing.Point(579, 6);
            this.button_process.Margin = new System.Windows.Forms.Padding(2);
            this.button_process.Name = "button_process";
            this.button_process.Size = new System.Drawing.Size(74, 27);
            this.button_process.TabIndex = 2;
            this.button_process.Text = "探测";
            this.button_process.UseVisualStyleBackColor = true;
            this.button_process.Click += new System.EventHandler(this.button_process_Click);
            // 
            // checkBox_debugModel
            // 
            this.checkBox_debugModel.AutoSize = true;
            this.checkBox_debugModel.Location = new System.Drawing.Point(175, 17);
            this.checkBox_debugModel.Name = "checkBox_debugModel";
            this.checkBox_debugModel.Size = new System.Drawing.Size(72, 16);
            this.checkBox_debugModel.TabIndex = 9;
            this.checkBox_debugModel.Text = "启用调试";
            this.checkBox_debugModel.UseVisualStyleBackColor = true;
            this.checkBox_debugModel.CheckedChanged += new System.EventHandler(this.checkBox_debugModel_CheckedChanged);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(667, 6);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(74, 27);
            this.button_cancel.TabIndex = 8;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(-13, 3);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(1065, 323);
            this.richTextBoxControl1.TabIndex = 3;
            this.richTextBoxControl1.Text = "";
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(287, 21);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(286, 34);
            this.progressBarComponent1.TabIndex = 34;
            // 
            // CycleSlipDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 447);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CycleSlipDetectForm";
            this.Text = "周跳探测";
            this.Load += new System.EventHandler(this.CycleSlipDetectForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_process;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.EnumCheckBoxControl enumCheckBoxControl1;
        private Geo.Winform.EnumRadioControl enumRadioControl1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        protected System.Windows.Forms.Button button_cancel;
        protected System.Windows.Forms.CheckBox checkBox_debugModel;
        private System.Windows.Forms.Button button_draw;
        private Controls.GnssSystemSelectControl gnssSystemSelectControl1;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_startEpoch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_caculateCount;
        protected System.Windows.Forms.Button button_setting;
        private System.Windows.Forms.CheckBox checkBox_isreversed;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
    }
}