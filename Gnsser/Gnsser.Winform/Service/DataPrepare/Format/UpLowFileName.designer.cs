namespace Gnsser.Winform
{
    partial class UpLowFileName
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_up = new System.Windows.Forms.RadioButton();
            this.radioButton_low = new System.Windows.Forms.RadioButton();
            this.button_setPath = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_go = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButton_up);
            this.groupBox1.Controls.Add(this.radioButton_low);
            this.groupBox1.Controls.Add(this.button_setPath);
            this.groupBox1.Controls.Add(this.textBox_path);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(524, 85);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // radioButton_up
            // 
            this.radioButton_up.AutoSize = true;
            this.radioButton_up.Location = new System.Drawing.Point(200, 49);
            this.radioButton_up.Name = "radioButton_up";
            this.radioButton_up.Size = new System.Drawing.Size(95, 16);
            this.radioButton_up.TabIndex = 3;
            this.radioButton_up.TabStop = true;
            this.radioButton_up.Text = "名称改为大写";
            this.radioButton_up.UseVisualStyleBackColor = true;
            // 
            // radioButton_low
            // 
            this.radioButton_low.AutoSize = true;
            this.radioButton_low.Location = new System.Drawing.Point(70, 49);
            this.radioButton_low.Name = "radioButton_low";
            this.radioButton_low.Size = new System.Drawing.Size(95, 16);
            this.radioButton_low.TabIndex = 3;
            this.radioButton_low.TabStop = true;
            this.radioButton_low.Text = "名称改为小写";
            this.radioButton_low.UseVisualStyleBackColor = true;
            // 
            // button_setPath
            // 
            this.button_setPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setPath.Location = new System.Drawing.Point(424, 20);
            this.button_setPath.Name = "button_setPath";
            this.button_setPath.Size = new System.Drawing.Size(75, 23);
            this.button_setPath.TabIndex = 2;
            this.button_setPath.Text = "...";
            this.button_setPath.UseVisualStyleBackColor = true;
            this.button_setPath.Click += new System.EventHandler(this.button_setPath_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_path.Location = new System.Drawing.Point(70, 21);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(348, 21);
            this.textBox_path.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件夹：";
            // 
            // button_go
            // 
            this.button_go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_go.Location = new System.Drawing.Point(437, 107);
            this.button_go.Name = "button_go";
            this.button_go.Size = new System.Drawing.Size(75, 23);
            this.button_go.TabIndex = 1;
            this.button_go.Text = "修改";
            this.button_go.UseVisualStyleBackColor = true;
            this.button_go.Click += new System.EventHandler(this.button_go_Click);
            // 
            // BaseStartWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 140);
            this.Controls.Add(this.button_go);
            this.Controls.Add(this.groupBox1);
            this.Name = "BaseStartWizardForm";
            this.Text = "文件名";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_up;
        private System.Windows.Forms.RadioButton radioButton_low;
        private System.Windows.Forms.Button button_setPath;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_go;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

