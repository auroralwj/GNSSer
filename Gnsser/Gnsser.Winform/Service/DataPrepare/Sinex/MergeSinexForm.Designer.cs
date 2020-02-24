namespace Gnsser.Winform
{
    partial class MergeSinexForm
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
            this.button_setStaPath = new System.Windows.Forms.Button();
            this.button_readA = new System.Windows.Forms.Button();
            this.textBox_fileA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_A = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox_B = new System.Windows.Forms.TextBox();
            this.textBox_fileB = new System.Windows.Forms.TextBox();
            this.button_read = new System.Windows.Forms.Button();
            this.button_setoldPath = new System.Windows.Forms.Button();
            this.button_merge = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_mergePath = new System.Windows.Forms.TextBox();
            this.button_setMerPath = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.checkBox_show = new System.Windows.Forms.CheckBox();
            this.textBox_C = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_setStaPath);
            this.groupBox1.Controls.Add(this.button_readA);
            this.groupBox1.Controls.Add(this.textBox_fileA);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_A);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 239);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件1";
            // 
            // button_setStaPath
            // 
            this.button_setStaPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setStaPath.Location = new System.Drawing.Point(326, 20);
            this.button_setStaPath.Name = "button_setStaPath";
            this.button_setStaPath.Size = new System.Drawing.Size(47, 23);
            this.button_setStaPath.TabIndex = 2;
            this.button_setStaPath.Text = "...";
            this.button_setStaPath.UseVisualStyleBackColor = true;
            this.button_setStaPath.Click += new System.EventHandler(this.button_setStaPath_Click);
            // 
            // button_readA
            // 
            this.button_readA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readA.Location = new System.Drawing.Point(326, 49);
            this.button_readA.Name = "button_readA";
            this.button_readA.Size = new System.Drawing.Size(47, 26);
            this.button_readA.TabIndex = 1;
            this.button_readA.Text = "读取";
            this.button_readA.UseVisualStyleBackColor = true;
            this.button_readA.Click += new System.EventHandler(this.button_readA_Click);
            // 
            // textBox_fileA
            // 
            this.textBox_fileA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fileA.Location = new System.Drawing.Point(65, 20);
            this.textBox_fileA.Name = "textBox_fileA";
            this.textBox_fileA.Size = new System.Drawing.Size(247, 21);
            this.textBox_fileA.TabIndex = 1;
            this.textBox_fileA.Text = "C:\\EXAMPLE.SNX";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件1：";
            // 
            // textBox_A
            // 
            this.textBox_A.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_A.Location = new System.Drawing.Point(12, 49);
            this.textBox_A.Multiline = true;
            this.textBox_A.Name = "textBox_A";
            this.textBox_A.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_A.Size = new System.Drawing.Size(300, 185);
            this.textBox_A.TabIndex = 1;
            // 
            // textBox_B
            // 
            this.textBox_B.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_B.Location = new System.Drawing.Point(8, 49);
            this.textBox_B.Multiline = true;
            this.textBox_B.Name = "textBox_B";
            this.textBox_B.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_B.Size = new System.Drawing.Size(278, 184);
            this.textBox_B.TabIndex = 1;
            // 
            // textBox_fileB
            // 
            this.textBox_fileB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fileB.Location = new System.Drawing.Point(50, 18);
            this.textBox_fileB.Name = "textBox_fileB";
            this.textBox_fileB.Size = new System.Drawing.Size(236, 21);
            this.textBox_fileB.TabIndex = 1;
            this.textBox_fileB.Text = "C:\\GPSDATA\\EXAMPLE\\STA\\EXAMPLE.SNX";
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(292, 47);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(47, 23);
            this.button_read.TabIndex = 2;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_readB_Click);
            // 
            // button_setoldPath
            // 
            this.button_setoldPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setoldPath.Location = new System.Drawing.Point(292, 18);
            this.button_setoldPath.Name = "button_setoldPath";
            this.button_setoldPath.Size = new System.Drawing.Size(47, 23);
            this.button_setoldPath.TabIndex = 2;
            this.button_setoldPath.Text = "...";
            this.button_setoldPath.UseVisualStyleBackColor = true;
            this.button_setoldPath.Click += new System.EventHandler(this.button_setfileBPath_Click);
            // 
            // button_merge
            // 
            this.button_merge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_merge.Location = new System.Drawing.Point(645, 5);
            this.button_merge.Name = "button_merge";
            this.button_merge.Size = new System.Drawing.Size(75, 32);
            this.button_merge.TabIndex = 2;
            this.button_merge.Text = "合并";
            this.button_merge.UseVisualStyleBackColor = true;
            this.button_merge.Click += new System.EventHandler(this.button_merge_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(85, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "说明：只合并两个文本框的内容。";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "新文件地址：";
            // 
            // textBox_mergePath
            // 
            this.textBox_mergePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_mergePath.Location = new System.Drawing.Point(87, 12);
            this.textBox_mergePath.Name = "textBox_mergePath";
            this.textBox_mergePath.Size = new System.Drawing.Size(499, 21);
            this.textBox_mergePath.TabIndex = 1;
            this.textBox_mergePath.Text = "C:\\MERGED.SNX";
            // 
            // button_setMerPath
            // 
            this.button_setMerPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setMerPath.Location = new System.Drawing.Point(592, 10);
            this.button_setMerPath.Name = "button_setMerPath";
            this.button_setMerPath.Size = new System.Drawing.Size(47, 23);
            this.button_setMerPath.TabIndex = 2;
            this.button_setMerPath.Text = "...";
            this.button_setMerPath.UseVisualStyleBackColor = true;
            this.button_setMerPath.Click += new System.EventHandler(this.button_setMerPath_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_fileB);
            this.groupBox2.Controls.Add(this.button_setoldPath);
            this.groupBox2.Controls.Add(this.button_read);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_B);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(351, 239);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "文件2：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(734, 239);
            this.splitContainer1.SplitterDistance = 379;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.checkBox_show);
            this.splitContainer2.Panel2.Controls.Add(this.textBox_C);
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.textBox_mergePath);
            this.splitContainer2.Panel2.Controls.Add(this.button_setMerPath);
            this.splitContainer2.Panel2.Controls.Add(this.button_merge);
            this.splitContainer2.Panel2.Controls.Add(this.label5);
            this.splitContainer2.Size = new System.Drawing.Size(734, 562);
            this.splitContainer2.SplitterDistance = 239;
            this.splitContainer2.TabIndex = 6;
            // 
            // checkBox_show
            // 
            this.checkBox_show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_show.AutoSize = true;
            this.checkBox_show.Location = new System.Drawing.Point(567, 46);
            this.checkBox_show.Name = "checkBox_show";
            this.checkBox_show.Size = new System.Drawing.Size(96, 16);
            this.checkBox_show.TabIndex = 4;
            this.checkBox_show.Text = "显示合并结果";
            this.checkBox_show.UseVisualStyleBackColor = true;
            // 
            // textBox_C
            // 
            this.textBox_C.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_C.Location = new System.Drawing.Point(3, 68);
            this.textBox_C.Multiline = true;
            this.textBox_C.Name = "textBox_C";
            this.textBox_C.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_C.Size = new System.Drawing.Size(719, 248);
            this.textBox_C.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SNX文件|*.SNX|所有文件|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "SNX文件|*.SNX|所有文件|*.*";
            // 
            // MergeSinexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 562);
            this.Controls.Add(this.splitContainer2);
            this.Name = "MergeSinexForm";
            this.Text = "对比合并独立Sinex";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_readA;
        private System.Windows.Forms.Button button_setStaPath;
        private System.Windows.Forms.TextBox textBox_fileA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox_B;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.TextBox textBox_fileB;
        private System.Windows.Forms.Button button_setoldPath;
        private System.Windows.Forms.TextBox textBox_A;
        private System.Windows.Forms.Button button_merge;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_mergePath;
        private System.Windows.Forms.Button button_setMerPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox textBox_C;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkBox_show;
    }
}