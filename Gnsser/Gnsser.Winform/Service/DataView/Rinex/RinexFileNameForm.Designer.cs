namespace Gnsser.Winform
{
    partial class RinexFileNameForm
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
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_selectFiles = new System.Windows.Forms.Button();
            this.textBox_fileNames = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_shownameInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_checkName = new System.Windows.Forms.CheckBox();
            this.button_run = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_output = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件|*.*O|所有文件|*.*";
            this.openFileDialog_obs.Multiselect = true;
            this.openFileDialog_obs.Title = "请选择O文件";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_selectFiles);
            this.groupBox1.Controls.Add(this.textBox_fileNames);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(688, 131);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // button_selectFiles
            // 
            this.button_selectFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectFiles.Location = new System.Drawing.Point(621, 27);
            this.button_selectFiles.Name = "button_selectFiles";
            this.button_selectFiles.Size = new System.Drawing.Size(61, 84);
            this.button_selectFiles.TabIndex = 2;
            this.button_selectFiles.Text = "选择文件";
            this.button_selectFiles.UseVisualStyleBackColor = true;
            this.button_selectFiles.Click += new System.EventHandler(this.button_selectFiles_Click);
            // 
            // textBox_fileNames
            // 
            this.textBox_fileNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fileNames.Location = new System.Drawing.Point(66, 20);
            this.textBox_fileNames.Multiline = true;
            this.textBox_fileNames.Name = "textBox_fileNames";
            this.textBox_fileNames.Size = new System.Drawing.Size(549, 104);
            this.textBox_fileNames.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件名：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBox_shownameInfo);
            this.groupBox2.Controls.Add(this.checkBox_checkName);
            this.groupBox2.Controls.Add(this.button_run);
            this.groupBox2.Location = new System.Drawing.Point(15, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(685, 76);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // checkBox_shownameInfo
            // 
            this.checkBox_shownameInfo.AutoSize = true;
            this.checkBox_shownameInfo.Checked = true;
            this.checkBox_shownameInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_shownameInfo.Location = new System.Drawing.Point(201, 20);
            this.checkBox_shownameInfo.Name = "checkBox_shownameInfo";
            this.checkBox_shownameInfo.Size = new System.Drawing.Size(96, 16);
            this.checkBox_shownameInfo.TabIndex = 2;
            this.checkBox_shownameInfo.Text = "显示名称信息";
            this.checkBox_shownameInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_checkName
            // 
            this.checkBox_checkName.AutoSize = true;
            this.checkBox_checkName.Checked = true;
            this.checkBox_checkName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_checkName.Location = new System.Drawing.Point(63, 20);
            this.checkBox_checkName.Name = "checkBox_checkName";
            this.checkBox_checkName.Size = new System.Drawing.Size(132, 16);
            this.checkBox_checkName.TabIndex = 1;
            this.checkBox_checkName.Text = "显示不符合规范文件";
            this.checkBox_checkName.UseVisualStyleBackColor = true;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(595, 20);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 39);
            this.button_run.TabIndex = 0;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox_output);
            this.groupBox3.Location = new System.Drawing.Point(15, 236);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(685, 293);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "输出";
            // 
            // textBox_output
            // 
            this.textBox_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_output.Location = new System.Drawing.Point(3, 17);
            this.textBox_output.Multiline = true;
            this.textBox_output.Name = "textBox_output";
            this.textBox_output.Size = new System.Drawing.Size(679, 273);
            this.textBox_output.TabIndex = 0;
            // 
            // RinexFileNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 541);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "RinexFileNameForm";
            this.Text = "Rinex文件名";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_selectFiles;
        private System.Windows.Forms.TextBox textBox_fileNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.CheckBox checkBox_shownameInfo;
        private System.Windows.Forms.CheckBox checkBox_checkName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_output;
    }
}

