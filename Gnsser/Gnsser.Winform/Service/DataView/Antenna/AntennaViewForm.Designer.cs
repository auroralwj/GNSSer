namespace Gnsser.Winform
{
    partial class AntennaViewForm
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
            this.openFileDialog_atx = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_selectFiles = new System.Windows.Forms.Button();
            this.textBox_fileNames = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_output = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_atx
            // 
            this.openFileDialog_atx.Filter = "天线文件|*.atx|所有文件|*.*";
            this.openFileDialog_atx.Multiselect = true;
            this.openFileDialog_atx.Title = "请选择O文件";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_selectFiles);
            this.groupBox1.Controls.Add(this.textBox_fileNames);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(917, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // button_selectFiles
            // 
            this.button_selectFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectFiles.Location = new System.Drawing.Point(832, 25);
            this.button_selectFiles.Margin = new System.Windows.Forms.Padding(4);
            this.button_selectFiles.Name = "button_selectFiles";
            this.button_selectFiles.Size = new System.Drawing.Size(81, 27);
            this.button_selectFiles.TabIndex = 2;
            this.button_selectFiles.Text = "选择文件";
            this.button_selectFiles.UseVisualStyleBackColor = true;
            this.button_selectFiles.Click += new System.EventHandler(this.button_selectFiles_Click);
            // 
            // textBox_fileNames
            // 
            this.textBox_fileNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fileNames.Location = new System.Drawing.Point(88, 25);
            this.textBox_fileNames.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_fileNames.Multiline = true;
            this.textBox_fileNames.Name = "textBox_fileNames";
            this.textBox_fileNames.Size = new System.Drawing.Size(731, 27);
            this.textBox_fileNames.TabIndex = 1;
            this.textBox_fileNames.Text = ".\\Data\\igs08.atx";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件名：";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(833, 103);
            this.button_run.Margin = new System.Windows.Forms.Padding(4);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(100, 38);
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
            this.groupBox3.Location = new System.Drawing.Point(20, 149);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(913, 512);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "输出";
            // 
            // textBox_output
            // 
            this.textBox_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_output.Location = new System.Drawing.Point(4, 22);
            this.textBox_output.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_output.Multiline = true;
            this.textBox_output.Name = "textBox_output";
            this.textBox_output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_output.Size = new System.Drawing.Size(905, 486);
            this.textBox_output.TabIndex = 0;
            // 
            // AntennaViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 676);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AntennaViewForm";
            this.Text = "天线查看";
            this.Load += new System.EventHandler(this.AntennaViewForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_atx;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_selectFiles;
        private System.Windows.Forms.TextBox textBox_fileNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_output;
    }
}

