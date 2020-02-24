namespace Geo.WinTools
{
    partial class FileSelectorForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_incKeys = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_inDir = new System.Windows.Forms.TextBox();
            this.button_setInDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_saveDir = new System.Windows.Forms.TextBox();
            this.button_setSaveDir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_filePatern = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox_filePatern);
            this.groupBox1.Controls.Add(this.button_setSaveDir);
            this.groupBox1.Controls.Add(this.button_setInDir);
            this.groupBox1.Controls.Add(this.textBox_saveDir);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_inDir);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.textBox_incKeys);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 257);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(334, 276);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 33);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_incKeys
            // 
            this.textBox_incKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_incKeys.Location = new System.Drawing.Point(109, 155);
            this.textBox_incKeys.Multiline = true;
            this.textBox_incKeys.Name = "textBox_incKeys";
            this.textBox_incKeys.Size = new System.Drawing.Size(267, 88);
            this.textBox_incKeys.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 157);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "包含关键字";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "被选文件夹：";
            // 
            // textBox_inDir
            // 
            this.textBox_inDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_inDir.Location = new System.Drawing.Point(109, 22);
            this.textBox_inDir.Name = "textBox_inDir";
            this.textBox_inDir.Size = new System.Drawing.Size(232, 21);
            this.textBox_inDir.TabIndex = 4;
            // 
            // button_setInDir
            // 
            this.button_setInDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setInDir.Location = new System.Drawing.Point(347, 21);
            this.button_setInDir.Name = "button_setInDir";
            this.button_setInDir.Size = new System.Drawing.Size(43, 23);
            this.button_setInDir.TabIndex = 5;
            this.button_setInDir.Text = "...";
            this.button_setInDir.UseVisualStyleBackColor = true;
            this.button_setInDir.Click += new System.EventHandler(this.button_setInDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "存放文件夹：";
            // 
            // textBox_saveDir
            // 
            this.textBox_saveDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_saveDir.Location = new System.Drawing.Point(109, 49);
            this.textBox_saveDir.Name = "textBox_saveDir";
            this.textBox_saveDir.Size = new System.Drawing.Size(232, 21);
            this.textBox_saveDir.TabIndex = 4;
            // 
            // button_setSaveDir
            // 
            this.button_setSaveDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setSaveDir.Location = new System.Drawing.Point(347, 48);
            this.button_setSaveDir.Name = "button_setSaveDir";
            this.button_setSaveDir.Size = new System.Drawing.Size(43, 23);
            this.button_setSaveDir.TabIndex = 5;
            this.button_setSaveDir.Text = "...";
            this.button_setSaveDir.UseVisualStyleBackColor = true;
            this.button_setSaveDir.Click += new System.EventHandler(this.button_setSaveDir_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "文件类型：";
            // 
            // textBox_filePatern
            // 
            this.textBox_filePatern.Location = new System.Drawing.Point(109, 80);
            this.textBox_filePatern.Name = "textBox_filePatern";
            this.textBox_filePatern.Size = new System.Drawing.Size(100, 21);
            this.textBox_filePatern.TabIndex = 6;
            this.textBox_filePatern.Text = "*.*";
            // 
            // FileSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 324);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "FileSelectorForm";
            this.Text = "文件选择器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox_incKeys;
        private System.Windows.Forms.Button button_setInDir;
        private System.Windows.Forms.TextBox textBox_inDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_setSaveDir;
        private System.Windows.Forms.TextBox textBox_saveDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_filePatern;
        private System.Windows.Forms.Label label3;
    }
}