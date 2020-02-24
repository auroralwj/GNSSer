namespace Gnsser.Winform.Other
{
    partial class CombinationWL
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
            this.textBox_selectPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_selectPath = new System.Windows.Forms.Button();
            this.button_combination = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件夹：";
            // 
            // textBox_selectPath
            // 
            this.textBox_selectPath.Location = new System.Drawing.Point(112, 45);
            this.textBox_selectPath.Name = "textBox_selectPath";
            this.textBox_selectPath.Size = new System.Drawing.Size(255, 21);
            this.textBox_selectPath.TabIndex = 1;
            // 
            // button_selectPath
            // 
            this.button_selectPath.Location = new System.Drawing.Point(386, 45);
            this.button_selectPath.Name = "button_selectPath";
            this.button_selectPath.Size = new System.Drawing.Size(75, 23);
            this.button_selectPath.TabIndex = 2;
            this.button_selectPath.Text = "…";
            this.button_selectPath.UseVisualStyleBackColor = true;
            this.button_selectPath.Click += new System.EventHandler(this.button_selectPath_Click);
            // 
            // button_combination
            // 
            this.button_combination.Location = new System.Drawing.Point(510, 43);
            this.button_combination.Name = "button_combination";
            this.button_combination.Size = new System.Drawing.Size(75, 23);
            this.button_combination.TabIndex = 3;
            this.button_combination.Text = "合并";
            this.button_combination.UseVisualStyleBackColor = true;
            this.button_combination.Click += new System.EventHandler(this.button_combination_Click);
            // 
            // CombinationWL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 119);
            this.Controls.Add(this.button_combination);
            this.Controls.Add(this.button_selectPath);
            this.Controls.Add(this.textBox_selectPath);
            this.Controls.Add(this.label1);
            this.Name = "CombinationWL";
            this.Text = "CombinationWL";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_selectPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_selectPath;
        private System.Windows.Forms.Button button_combination;
    }
}