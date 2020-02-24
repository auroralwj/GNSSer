namespace Gnsser.Winform
{
    partial class IgsProducExtractorForm
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
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_run = new System.Windows.Forms.Button();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.checkBox_moveOrCopy = new System.Windows.Forms.CheckBox();
            this.namedStringControl1_extension = new Geo.Winform.Controls.NamedStringControl();
            this.namedStringControl_nameOfIgs = new Geo.Winform.Controls.NamedStringControl();
            this.checkBox_onlyOne = new System.Windows.Forms.CheckBox();
            this.checkBox_override = new System.Windows.Forms.CheckBox();
            this.timePeriodControl1 = new Gnsser.Winform.Controls.TimePeriodUserControl();
            this.SuspendLayout();
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "钟差文件(*.clk_30s;*.clk_05s;*.clk)|*.clk_30s;*.clk_05s;*.clk|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = true;
            this.fileOpenControl1.LabelName = "源文件夹：";
            this.fileOpenControl1.Location = new System.Drawing.Point(37, 12);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(732, 51);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(684, 221);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 48);
            this.button_run.TabIndex = 5;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(53, 221);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(584, 34);
            this.progressBarComponent1.TabIndex = 7;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "目标文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(24, 69);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(745, 22);
            this.directorySelectionControl1.TabIndex = 8;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // checkBox_moveOrCopy
            // 
            this.checkBox_moveOrCopy.AutoSize = true;
            this.checkBox_moveOrCopy.Location = new System.Drawing.Point(520, 152);
            this.checkBox_moveOrCopy.Name = "checkBox_moveOrCopy";
            this.checkBox_moveOrCopy.Size = new System.Drawing.Size(84, 16);
            this.checkBox_moveOrCopy.TabIndex = 11;
            this.checkBox_moveOrCopy.Text = "剪切或复制";
            this.checkBox_moveOrCopy.UseVisualStyleBackColor = true;
            // 
            // namedStringControl1_extension
            // 
            this.namedStringControl1_extension.Location = new System.Drawing.Point(24, 145);
            this.namedStringControl1_extension.Name = "namedStringControl1_extension";
            this.namedStringControl1_extension.Size = new System.Drawing.Size(332, 23);
            this.namedStringControl1_extension.TabIndex = 12;
            this.namedStringControl1_extension.Title = "文件后缀名：";
            // 
            // namedStringControl_nameOfIgs
            // 
            this.namedStringControl_nameOfIgs.Location = new System.Drawing.Point(12, 174);
            this.namedStringControl_nameOfIgs.Name = "namedStringControl_nameOfIgs";
            this.namedStringControl_nameOfIgs.Size = new System.Drawing.Size(332, 23);
            this.namedStringControl_nameOfIgs.TabIndex = 12;
            this.namedStringControl_nameOfIgs.Title = "IGS中心代码：";
            // 
            // checkBox_onlyOne
            // 
            this.checkBox_onlyOne.AutoSize = true;
            this.checkBox_onlyOne.Checked = true;
            this.checkBox_onlyOne.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_onlyOne.Location = new System.Drawing.Point(379, 181);
            this.checkBox_onlyOne.Name = "checkBox_onlyOne";
            this.checkBox_onlyOne.Size = new System.Drawing.Size(168, 16);
            this.checkBox_onlyOne.TabIndex = 11;
            this.checkBox_onlyOne.Text = "一个文件有一个数据源即可";
            this.checkBox_onlyOne.UseVisualStyleBackColor = true;
            // 
            // checkBox_override
            // 
            this.checkBox_override.AutoSize = true;
            this.checkBox_override.Location = new System.Drawing.Point(592, 113);
            this.checkBox_override.Name = "checkBox_override";
            this.checkBox_override.Size = new System.Drawing.Size(72, 16);
            this.checkBox_override.TabIndex = 11;
            this.checkBox_override.Text = "是否覆盖";
            this.checkBox_override.UseVisualStyleBackColor = true;
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(53, 97);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(492, 32);
            this.timePeriodControl1.TabIndex = 14;
            // 
            // IgsProducExtractorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.timePeriodControl1);
            this.Controls.Add(this.namedStringControl_nameOfIgs);
            this.Controls.Add(this.namedStringControl1_extension);
            this.Controls.Add(this.checkBox_onlyOne);
            this.Controls.Add(this.checkBox_override);
            this.Controls.Add(this.checkBox_moveOrCopy);
            this.Controls.Add(this.directorySelectionControl1);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.fileOpenControl1);
            this.Name = "IgsProducExtractorForm";
            this.Text = "IGS产品提取器";
            this.Load += new System.EventHandler(this.FilemaintainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_run;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBox_moveOrCopy;
        private Geo.Winform.Controls.NamedStringControl namedStringControl1_extension;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_nameOfIgs;
        private System.Windows.Forms.CheckBox checkBox_onlyOne;
        private System.Windows.Forms.CheckBox checkBox_override;
        private Controls.TimePeriodUserControl timePeriodControl1;
    }
}