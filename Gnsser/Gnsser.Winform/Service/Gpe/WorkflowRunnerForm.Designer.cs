namespace Gnsser.Winform
{
    partial class WorkflowRunnerForm
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
            this.button_runOrStop = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.checkBox1IsShowProcessInfo = new System.Windows.Forms.CheckBox();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.checkBox_showError = new System.Windows.Forms.CheckBox();
            this.checkBox_showWarn = new System.Windows.Forms.CheckBox();
            this.checkBox_debugModel = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_runOrStop
            // 
            this.button_runOrStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_runOrStop.Location = new System.Drawing.Point(487, 106);
            this.button_runOrStop.Name = "button_runOrStop";
            this.button_runOrStop.Size = new System.Drawing.Size(75, 35);
            this.button_runOrStop.TabIndex = 1;
            this.button_runOrStop.Text = "运行/停止";
            this.button_runOrStop.UseVisualStyleBackColor = true;
            this.button_runOrStop.Click += new System.EventHandler(this.button_runOrStop_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // checkBox1IsShowProcessInfo
            // 
            this.checkBox1IsShowProcessInfo.AutoSize = true;
            this.checkBox1IsShowProcessInfo.Checked = true;
            this.checkBox1IsShowProcessInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1IsShowProcessInfo.Location = new System.Drawing.Point(12, 84);
            this.checkBox1IsShowProcessInfo.Name = "checkBox1IsShowProcessInfo";
            this.checkBox1IsShowProcessInfo.Size = new System.Drawing.Size(72, 16);
            this.checkBox1IsShowProcessInfo.TabIndex = 6;
            this.checkBox1IsShowProcessInfo.Text = "显示信息";
            this.checkBox1IsShowProcessInfo.UseVisualStyleBackColor = true;
            this.checkBox1IsShowProcessInfo.CheckedChanged += new System.EventHandler(this.checkBox1IsShowProcessInfo_CheckedChanged);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "操作流文件(GOF)|*.gof|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.IsMultiSelect = true;
            this.fileOpenControl1.LabelName = "GOF文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(12, 11);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(551, 60);
            this.fileOpenControl1.TabIndex = 1;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(12, 107);
            this.progressBarComponent1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(469, 35);
            this.progressBarComponent1.TabIndex = 4;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(12, 149);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(549, 240);
            this.richTextBoxControl1.TabIndex = 3;
            this.richTextBoxControl1.Text = "";
            // 
            // checkBox_showError
            // 
            this.checkBox_showError.AutoSize = true;
            this.checkBox_showError.Location = new System.Drawing.Point(256, 84);
            this.checkBox_showError.Name = "checkBox_showError";
            this.checkBox_showError.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showError.TabIndex = 8;
            this.checkBox_showError.Text = "显示错误";
            this.checkBox_showError.UseVisualStyleBackColor = true;
            this.checkBox_showError.CheckedChanged += new System.EventHandler(this.checkBox_showError_CheckedChanged);
            // 
            // checkBox_showWarn
            // 
            this.checkBox_showWarn.AutoSize = true;
            this.checkBox_showWarn.Location = new System.Drawing.Point(100, 84);
            this.checkBox_showWarn.Name = "checkBox_showWarn";
            this.checkBox_showWarn.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showWarn.TabIndex = 9;
            this.checkBox_showWarn.Text = "显示警告";
            this.checkBox_showWarn.UseVisualStyleBackColor = true;
            this.checkBox_showWarn.CheckedChanged += new System.EventHandler(this.checkBox_showWarn_CheckedChanged);
            // 
            // checkBox_debugModel
            // 
            this.checkBox_debugModel.AutoSize = true;
            this.checkBox_debugModel.Location = new System.Drawing.Point(178, 84);
            this.checkBox_debugModel.Name = "checkBox_debugModel";
            this.checkBox_debugModel.Size = new System.Drawing.Size(72, 16);
            this.checkBox_debugModel.TabIndex = 10;
            this.checkBox_debugModel.Text = "启用调试";
            this.checkBox_debugModel.UseVisualStyleBackColor = true;
            this.checkBox_debugModel.CheckedChanged += new System.EventHandler(this.checkBox_debugModel_CheckedChanged);
            // 
            // WorkflowRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 401);
            this.Controls.Add(this.checkBox_showError);
            this.Controls.Add(this.checkBox_showWarn);
            this.Controls.Add(this.checkBox_debugModel);
            this.Controls.Add(this.checkBox1IsShowProcessInfo);
            this.Controls.Add(this.fileOpenControl1);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.richTextBoxControl1);
            this.Controls.Add(this.button_runOrStop);
            this.Name = "WorkflowRunnerForm";
            this.Text = "操作流运行器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkflowRunnerForm_FormClosing);
            this.Load += new System.EventHandler(this.WorkflowRunnerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_runOrStop;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBox1IsShowProcessInfo;
        private System.Windows.Forms.CheckBox checkBox_showError;
        private System.Windows.Forms.CheckBox checkBox_showWarn;
        private System.Windows.Forms.CheckBox checkBox_debugModel;
    }
}