namespace Gnsser.Winform
{
    partial class ObsFileMetaViewerForm
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
            this.checkBox_source = new System.Windows.Forms.CheckBox();
            this.checkBox_Gnsser = new System.Windows.Forms.CheckBox();
            this.checkBox_teqc = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_out = new System.Windows.Forms.TextBox();
            this.button_saveAs = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_getObsPath
            // 
            this.button_getObsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getObsPath.Location = new System.Drawing.Point(781, 22);
            this.button_getObsPath.Margin = new System.Windows.Forms.Padding(4);
            this.button_getObsPath.Name = "button_getObsPath";
            this.button_getObsPath.Size = new System.Drawing.Size(68, 91);
            this.button_getObsPath.TabIndex = 0;
            this.button_getObsPath.Text = "...";
            this.button_getObsPath.UseVisualStyleBackColor = true;
            this.button_getObsPath.Click += new System.EventHandler(this.button_getObsPath_Click);
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件|*.*O|星历文件|*.*N|所有文件|*.*";
            this.openFileDialog_obs.Multiselect = true;
            this.openFileDialog_obs.Title = "请选择文件（O文件、N文件、Met文件等）";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "观测数据文件：";
            // 
            // textBox_obsPath
            // 
            this.textBox_obsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_obsPath.Location = new System.Drawing.Point(144, 22);
            this.textBox_obsPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_obsPath.Multiline = true;
            this.textBox_obsPath.Name = "textBox_obsPath";
            this.textBox_obsPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_obsPath.Size = new System.Drawing.Size(629, 91);
            this.textBox_obsPath.TabIndex = 2;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(761, 164);
            this.button_read.Margin = new System.Windows.Forms.Padding(4);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(112, 41);
            this.button_read.TabIndex = 15;
            this.button_read.Text = "提取查看";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_source);
            this.groupBox1.Controls.Add(this.checkBox_Gnsser);
            this.groupBox1.Controls.Add(this.checkBox_teqc);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_getObsPath);
            this.groupBox1.Controls.Add(this.textBox_obsPath);
            this.groupBox1.Location = new System.Drawing.Point(16, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(857, 146);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // checkBox_source
            // 
            this.checkBox_source.AutoSize = true;
            this.checkBox_source.Location = new System.Drawing.Point(451, 120);
            this.checkBox_source.Name = "checkBox_source";
            this.checkBox_source.Size = new System.Drawing.Size(74, 19);
            this.checkBox_source.TabIndex = 18;
            this.checkBox_source.Text = "原文件";
            this.checkBox_source.UseVisualStyleBackColor = true;
            // 
            // checkBox_Gnsser
            // 
            this.checkBox_Gnsser.AutoSize = true;
            this.checkBox_Gnsser.Checked = true;
            this.checkBox_Gnsser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Gnsser.Location = new System.Drawing.Point(144, 120);
            this.checkBox_Gnsser.Name = "checkBox_Gnsser";
            this.checkBox_Gnsser.Size = new System.Drawing.Size(137, 19);
            this.checkBox_Gnsser.TabIndex = 17;
            this.checkBox_Gnsser.Text = "Gnsser提取结果";
            this.checkBox_Gnsser.UseVisualStyleBackColor = true;
            // 
            // checkBox_teqc
            // 
            this.checkBox_teqc.AutoSize = true;
            this.checkBox_teqc.Location = new System.Drawing.Point(313, 120);
            this.checkBox_teqc.Name = "checkBox_teqc";
            this.checkBox_teqc.Size = new System.Drawing.Size(121, 19);
            this.checkBox_teqc.TabIndex = 16;
            this.checkBox_teqc.Text = "TEQC提取结果";
            this.checkBox_teqc.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button_saveAs);
            this.groupBox2.Controls.Add(this.textBox_out);
            this.groupBox2.Location = new System.Drawing.Point(13, 212);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(860, 386);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // textBox_out
            // 
            this.textBox_out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_out.Location = new System.Drawing.Point(3, 53);
            this.textBox_out.Multiline = true;
            this.textBox_out.Name = "textBox_out";
            this.textBox_out.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_out.Size = new System.Drawing.Size(854, 366);
            this.textBox_out.TabIndex = 0;
            // 
            // button_saveAs
            // 
            this.button_saveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveAs.Location = new System.Drawing.Point(748, 13);
            this.button_saveAs.Name = "button_saveAs";
            this.button_saveAs.Size = new System.Drawing.Size(104, 34);
            this.button_saveAs.TabIndex = 1;
            this.button_saveAs.Text = "结果另存为";
            this.button_saveAs.UseVisualStyleBackColor = true;
            this.button_saveAs.Click += new System.EventHandler(this.button_saveAs_Click);
            // 
            // ObsFileMetaViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 656);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_read);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ObsFileMetaViewerForm";
            this.Text = "头文件信息查看";
            this.Load += new System.EventHandler(this.ObsFileMetaViewerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.CheckBox checkBox_teqc;
        private System.Windows.Forms.CheckBox checkBox_source;
        private System.Windows.Forms.CheckBox checkBox_Gnsser;
        private System.Windows.Forms.Button button_saveAs;
    }
}

