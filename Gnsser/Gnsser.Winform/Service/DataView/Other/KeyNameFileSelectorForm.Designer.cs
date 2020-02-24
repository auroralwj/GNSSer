namespace Gnsser.Winform
{
    partial class KeyNameFileSelectorForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_getObsPath = new System.Windows.Forms.Button();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_obsPath = new System.Windows.Forms.TextBox();
            this.button_read = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.textBox_keywords = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_out = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_getObsPath
            // 
            this.button_getObsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getObsPath.Location = new System.Drawing.Point(655, 18);
            this.button_getObsPath.Name = "button_getObsPath";
            this.button_getObsPath.Size = new System.Drawing.Size(51, 73);
            this.button_getObsPath.TabIndex = 0;
            this.button_getObsPath.Text = "...";
            this.button_getObsPath.UseVisualStyleBackColor = true;
            this.button_getObsPath.Click += new System.EventHandler(this.button_getObsPath_Click);
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "所有文件|*.*";
            this.openFileDialog_obs.Multiselect = true;
            this.openFileDialog_obs.Title = "请选择文件（O文件、N文件、Met文件等）";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "文件：";
            // 
            // textBox_obsPath
            // 
            this.textBox_obsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_obsPath.Location = new System.Drawing.Point(91, 18);
            this.textBox_obsPath.Multiline = true;
            this.textBox_obsPath.Name = "textBox_obsPath";
            this.textBox_obsPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_obsPath.Size = new System.Drawing.Size(559, 74);
            this.textBox_obsPath.TabIndex = 2;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(628, 221);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(84, 33);
            this.button_read.TabIndex = 15;
            this.button_read.Text = "选择";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_keywords);
            this.groupBox1.Controls.Add(this.button_getObsPath);
            this.groupBox1.Controls.Add(this.textBox_obsPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 200);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(31, 167);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(589, 22);
            this.directorySelectionControl1.TabIndex = 19;
            // 
            // textBox_keywords
            // 
            this.textBox_keywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_keywords.Location = new System.Drawing.Point(91, 96);
            this.textBox_keywords.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_keywords.Multiline = true;
            this.textBox_keywords.Name = "textBox_keywords";
            this.textBox_keywords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_keywords.Size = new System.Drawing.Size(615, 66);
            this.textBox_keywords.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "包含关键字：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox_out);
            this.groupBox2.Location = new System.Drawing.Point(10, 288);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(714, 178);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // textBox_out
            // 
            this.textBox_out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_out.Location = new System.Drawing.Point(2, 16);
            this.textBox_out.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_out.Multiline = true;
            this.textBox_out.Name = "textBox_out";
            this.textBox_out.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_out.Size = new System.Drawing.Size(710, 160);
            this.textBox_out.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(413, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "说明：根据文件名称，可以选择任何文件，不局限于观测文件，不区分大小写";
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(28, 220);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(594, 34);
            this.progressBarComponent1.TabIndex = 19;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // KeyNameFileSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 477);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_read);
            this.Controls.Add(this.label3);
            this.Name = "KeyNameFileSelectorForm";
            this.Text = "文件关键字选择器";
            this.Load += new System.EventHandler(this.ObsFileMetaViewerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_getObsPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_obsPath;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_out;
        private System.Windows.Forms.TextBox textBox_keywords;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.Label label3;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

