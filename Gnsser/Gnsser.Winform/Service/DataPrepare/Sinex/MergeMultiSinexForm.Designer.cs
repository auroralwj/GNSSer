namespace Gnsser.Winform
{
    partial class MergeMultiSinexForm
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

        #region Windows Form Designer generated obsCode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_eraseNonCoord = new System.Windows.Forms.CheckBox();
            this.checkBox_showresult = new System.Windows.Forms.CheckBox();
            this.button_setSavePath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_savepath = new System.Windows.Forms.TextBox();
            this.button_merge = new System.Windows.Forms.Button();
            this.button_setFilePath = new System.Windows.Forms.Button();
            this.textBox_pathes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_eraseNonCoord);
            this.groupBox1.Controls.Add(this.checkBox_showresult);
            this.groupBox1.Controls.Add(this.button_setSavePath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_savepath);
            this.groupBox1.Controls.Add(this.button_merge);
            this.groupBox1.Controls.Add(this.button_setFilePath);
            this.groupBox1.Controls.Add(this.textBox_pathes);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(661, 153);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // checkBox_eraseNonCoord
            // 
            this.checkBox_eraseNonCoord.AutoSize = true;
            this.checkBox_eraseNonCoord.Checked = true;
            this.checkBox_eraseNonCoord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_eraseNonCoord.Location = new System.Drawing.Point(187, 131);
            this.checkBox_eraseNonCoord.Name = "checkBox_eraseNonCoord";
            this.checkBox_eraseNonCoord.Size = new System.Drawing.Size(84, 16);
            this.checkBox_eraseNonCoord.TabIndex = 8;
            this.checkBox_eraseNonCoord.Text = "清除非坐标";
            this.checkBox_eraseNonCoord.UseVisualStyleBackColor = true;
            // 
            // checkBox_showresult
            // 
            this.checkBox_showresult.AutoSize = true;
            this.checkBox_showresult.Location = new System.Drawing.Point(70, 131);
            this.checkBox_showresult.Name = "checkBox_showresult";
            this.checkBox_showresult.Size = new System.Drawing.Size(96, 16);
            this.checkBox_showresult.TabIndex = 7;
            this.checkBox_showresult.Text = "显示合并结果";
            this.checkBox_showresult.UseVisualStyleBackColor = true;
            // 
            // button_setSavePath
            // 
            this.button_setSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setSavePath.Location = new System.Drawing.Point(526, 98);
            this.button_setSavePath.Name = "button_setSavePath";
            this.button_setSavePath.Size = new System.Drawing.Size(46, 23);
            this.button_setSavePath.TabIndex = 6;
            this.button_setSavePath.Text = "...";
            this.button_setSavePath.UseVisualStyleBackColor = true;
            this.button_setSavePath.Click += new System.EventHandler(this.button_setSavePath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "保存路径：";
            // 
            // textBox_savepath
            // 
            this.textBox_savepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_savepath.Location = new System.Drawing.Point(70, 98);
            this.textBox_savepath.Name = "textBox_savepath";
            this.textBox_savepath.Size = new System.Drawing.Size(450, 21);
            this.textBox_savepath.TabIndex = 4;
            this.textBox_savepath.Text = "C:\\MERGED.SNX";
            // 
            // button_merge
            // 
            this.button_merge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_merge.Location = new System.Drawing.Point(594, 110);
            this.button_merge.Name = "button_merge";
            this.button_merge.Size = new System.Drawing.Size(61, 37);
            this.button_merge.TabIndex = 3;
            this.button_merge.Text = "合并";
            this.button_merge.UseVisualStyleBackColor = true;
            this.button_merge.Click += new System.EventHandler(this.button_merge_Click);
            // 
            // button_setFilePath
            // 
            this.button_setFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setFilePath.Location = new System.Drawing.Point(594, 24);
            this.button_setFilePath.Name = "button_setFilePath";
            this.button_setFilePath.Size = new System.Drawing.Size(45, 23);
            this.button_setFilePath.TabIndex = 2;
            this.button_setFilePath.Text = "...";
            this.button_setFilePath.UseVisualStyleBackColor = true;
            this.button_setFilePath.Click += new System.EventHandler(this.button_setFilePath_Click);
            // 
            // textBox_pathes
            // 
            this.textBox_pathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pathes.Location = new System.Drawing.Point(70, 26);
            this.textBox_pathes.Multiline = true;
            this.textBox_pathes.Name = "textBox_pathes";
            this.textBox_pathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_pathes.Size = new System.Drawing.Size(502, 65);
            this.textBox_pathes.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SINEX文件|*.SNX|所有文件|*.*";
            this.openFileDialog1.Multiselect = true;
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.Location = new System.Drawing.Point(12, 172);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(660, 300);
            this.textBox_result.TabIndex = 1;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "SINEX文件|*.SNX|所有文件|*.*";
            // 
            // MergeMultiSinexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 484);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_result);
            this.Name = "MergeMultiSinexForm";
            this.Text = "批量合并独立Sinex";
            this.Load += new System.EventHandler(this.MergeMultiSinexForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_setFilePath;
        private System.Windows.Forms.TextBox textBox_pathes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Button button_merge;
        private System.Windows.Forms.Button button_setSavePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_savepath;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkBox_showresult;
        private System.Windows.Forms.CheckBox checkBox_eraseNonCoord;
    }
}