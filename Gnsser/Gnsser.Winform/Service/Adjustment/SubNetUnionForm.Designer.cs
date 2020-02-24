namespace Gnsser.Winform
{
    partial class SubNetUnionForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_getPath = new System.Windows.Forms.Button();
            this.textBox_Path = new System.Windows.Forms.TextBox();
            this.button_read = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.button_saveTofile = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_getPath);
            this.groupBox1.Controls.Add(this.textBox_Path);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(995, 108);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "文件：";
            // 
            // button_getPath
            // 
            this.button_getPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getPath.Location = new System.Drawing.Point(905, 16);
            this.button_getPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_getPath.Name = "button_getPath";
            this.button_getPath.Size = new System.Drawing.Size(65, 55);
            this.button_getPath.TabIndex = 16;
            this.button_getPath.Text = "...";
            this.button_getPath.UseVisualStyleBackColor = true;
            this.button_getPath.Click += new System.EventHandler(this.button_getPath_Click);
            // 
            // textBox_Path
            // 
            this.textBox_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Path.Location = new System.Drawing.Point(105, 16);
            this.textBox_Path.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_Path.Multiline = true;
            this.textBox_Path.Name = "textBox_Path";
            this.textBox_Path.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Path.Size = new System.Drawing.Size(772, 83);
            this.textBox_Path.TabIndex = 18;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(906, 129);
            this.button_read.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(105, 45);
            this.button_read.TabIndex = 19;
            this.button_read.Text = "平差计算";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SINEX文件(*.snx)|*.snx|所有文件(*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "请选择Sinex文件";
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(159, 130);
            this.button_showOnMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(121, 29);
            this.button_showOnMap.TabIndex = 36;
            this.button_showOnMap.Text = "地图上显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // button_saveTofile
            // 
            this.button_saveTofile.Location = new System.Drawing.Point(16, 130);
            this.button_saveTofile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_saveTofile.Name = "button_saveTofile";
            this.button_saveTofile.Size = new System.Drawing.Size(123, 29);
            this.button_saveTofile.TabIndex = 38;
            this.button_saveTofile.Text = "导出SINEX文本";
            this.button_saveTofile.UseVisualStyleBackColor = true;
            this.button_saveTofile.Click += new System.EventHandler(this.button_saveTofile_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "SINEX文件|*.SNX|所有文件|*.*";
            this.saveFileDialog1.Title = "保存Sinex文件";
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_info.Location = new System.Drawing.Point(5, 182);
            this.textBox_info.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(1003, 382);
            this.textBox_info.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 162);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 40;
            this.label2.Text = "SINEX结果：";
            // 
            // SubNetUnionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 580);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_info);
            this.Controls.Add(this.button_saveTofile);
            this.Controls.Add(this.button_showOnMap);
            this.Controls.Add(this.button_read);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SubNetUnionForm";
            this.Text = "子网联合平差";
            this.Load += new System.EventHandler(this.SubNetUnionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_getPath;
        private System.Windows.Forms.TextBox textBox_Path;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_saveTofile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Label label2;
    }
}