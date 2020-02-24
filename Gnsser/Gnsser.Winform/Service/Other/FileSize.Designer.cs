namespace Gnsser.Winform.Other
{
    partial class FileSize
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_dir = new System.Windows.Forms.TextBox();
            this.button_select = new System.Windows.Forms.Button();
            this.button_calsize = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件夹：";
            // 
            // textBox_dir
            // 
            this.textBox_dir.Location = new System.Drawing.Point(73, 23);
            this.textBox_dir.Name = "textBox_dir";
            this.textBox_dir.Size = new System.Drawing.Size(274, 21);
            this.textBox_dir.TabIndex = 1;
            // 
            // button_select
            // 
            this.button_select.Location = new System.Drawing.Point(371, 21);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(75, 23);
            this.button_select.TabIndex = 2;
            this.button_select.Text = "……";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // button_calsize
            // 
            this.button_calsize.Location = new System.Drawing.Point(461, 21);
            this.button_calsize.Name = "button_calsize";
            this.button_calsize.Size = new System.Drawing.Size(75, 23);
            this.button_calsize.TabIndex = 3;
            this.button_calsize.Text = "统计";
            this.button_calsize.UseVisualStyleBackColor = true;
            this.button_calsize.Click += new System.EventHandler(this.button_calsize_Click);
            // 
            // FileSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 78);
            this.Controls.Add(this.button_calsize);
            this.Controls.Add(this.button_select);
            this.Controls.Add(this.textBox_dir);
            this.Controls.Add(this.label1);
            this.Name = "FileSize";
            this.Text = "FileSize";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_dir;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.Button button_calsize;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}