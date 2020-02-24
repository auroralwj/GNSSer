namespace Geo.WinTools.Images
{
    partial class ScreenImageForm1
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
            this.button_GetScreenPic = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_dir = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_SetFolder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_save = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.button_GetScreen = new System.Windows.Forms.Button();
            this.checkBox_hideMessageBox = new System.Windows.Forms.CheckBox();
            this.button_mouse = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_GetScreenPic
            // 
            this.button_GetScreenPic.Location = new System.Drawing.Point(421, 108);
            this.button_GetScreenPic.Name = "button_GetScreenPic";
            this.button_GetScreenPic.Size = new System.Drawing.Size(75, 23);
            this.button_GetScreenPic.TabIndex = 0;
            this.button_GetScreenPic.Text = "WinAPI抓屏";
            this.button_GetScreenPic.UseVisualStyleBackColor = true;
            this.button_GetScreenPic.Click += new System.EventHandler(this.button_GetScreenPic_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "存储位置:";
            // 
            // textBox_dir
            // 
            this.textBox_dir.Location = new System.Drawing.Point(83, 15);
            this.textBox_dir.Name = "textBox_dir";
            this.textBox_dir.Size = new System.Drawing.Size(263, 21);
            this.textBox_dir.TabIndex = 2;
            this.textBox_dir.Text = "D:\\";
            // 
            // button_SetFolder
            // 
            this.button_SetFolder.Location = new System.Drawing.Point(352, 15);
            this.button_SetFolder.Name = "button_SetFolder";
            this.button_SetFolder.Size = new System.Drawing.Size(36, 23);
            this.button_SetFolder.TabIndex = 3;
            this.button_SetFolder.Text = "...";
            this.button_SetFolder.UseVisualStyleBackColor = true;
            this.button_SetFolder.Click += new System.EventHandler(this.button_SetFolder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_save);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.button_SetFolder);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_name);
            this.groupBox1.Controls.Add(this.textBox_dir);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 78);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // checkBox_save
            // 
            this.checkBox_save.AutoSize = true;
            this.checkBox_save.Checked = true;
            this.checkBox_save.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_save.Location = new System.Drawing.Point(286, 47);
            this.checkBox_save.Name = "checkBox_save";
            this.checkBox_save.Size = new System.Drawing.Size(48, 16);
            this.checkBox_save.TabIndex = 6;
            this.checkBox_save.Text = "存储";
            this.checkBox_save.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "JPG",
            "PNG",
            "GIF",
            "ICO",
            "BMP"});
            this.comboBox1.Location = new System.Drawing.Point(207, 43);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(55, 20);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Text = "JPG";
            this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(166, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "格式:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "存储名称:";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(83, 42);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(77, 21);
            this.textBox_name.TabIndex = 2;
            this.textBox_name.Text = "截屏";
            // 
            // button_GetScreen
            // 
            this.button_GetScreen.Location = new System.Drawing.Point(325, 108);
            this.button_GetScreen.Name = "button_GetScreen";
            this.button_GetScreen.Size = new System.Drawing.Size(75, 23);
            this.button_GetScreen.TabIndex = 5;
            this.button_GetScreen.Text = "C#抓屏";
            this.button_GetScreen.UseVisualStyleBackColor = true;
            this.button_GetScreen.Click += new System.EventHandler(this.button_GetScreen_Click);
            // 
            // checkBox_hideMessageBox
            // 
            this.checkBox_hideMessageBox.AutoSize = true;
            this.checkBox_hideMessageBox.Location = new System.Drawing.Point(12, 119);
            this.checkBox_hideMessageBox.Name = "checkBox_hideMessageBox";
            this.checkBox_hideMessageBox.Size = new System.Drawing.Size(84, 16);
            this.checkBox_hideMessageBox.TabIndex = 6;
            this.checkBox_hideMessageBox.Text = "隐藏提示框";
            this.checkBox_hideMessageBox.UseVisualStyleBackColor = true;
            // 
            // button_mouse
            // 
            this.button_mouse.Location = new System.Drawing.Point(238, 108);
            this.button_mouse.Name = "button_mouse";
            this.button_mouse.Size = new System.Drawing.Size(64, 23);
            this.button_mouse.TabIndex = 7;
            this.button_mouse.Text = "鼠标选取";
            this.button_mouse.UseVisualStyleBackColor = true;
            this.button_mouse.Click += new System.EventHandler(this.button_mouse_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(58, 185);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(364, 168);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // ScreenImageForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 390);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_mouse);
            this.Controls.Add(this.checkBox_hideMessageBox);
            this.Controls.Add(this.button_GetScreen);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_GetScreenPic);
            this.Name = "ScreenImageForm1";
            this.Text = "截屏小工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_GetScreenPic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_dir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_SetFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Button button_GetScreen;
        private System.Windows.Forms.CheckBox checkBox_hideMessageBox;
        private System.Windows.Forms.CheckBox checkBox_save;
        private System.Windows.Forms.Button button_mouse;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}