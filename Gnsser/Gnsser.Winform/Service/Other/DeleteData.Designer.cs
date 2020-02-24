namespace Gnsser.Winform.Other
{
    partial class DeleteData
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_minvalue = new System.Windows.Forms.TextBox();
            this.textBox_maxvalue = new System.Windows.Forms.TextBox();
            this.textBox_satNLFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_selectFile = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(323, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max：";
            // 
            // textBox_minvalue
            // 
            this.textBox_minvalue.Location = new System.Drawing.Point(123, 40);
            this.textBox_minvalue.Name = "textBox_minvalue";
            this.textBox_minvalue.Size = new System.Drawing.Size(100, 21);
            this.textBox_minvalue.TabIndex = 2;
            // 
            // textBox_maxvalue
            // 
            this.textBox_maxvalue.Location = new System.Drawing.Point(374, 40);
            this.textBox_maxvalue.Name = "textBox_maxvalue";
            this.textBox_maxvalue.Size = new System.Drawing.Size(100, 21);
            this.textBox_maxvalue.TabIndex = 3;
            // 
            // textBox_satNLFile
            // 
            this.textBox_satNLFile.Location = new System.Drawing.Point(123, 93);
            this.textBox_satNLFile.Name = "textBox_satNLFile";
            this.textBox_satNLFile.Size = new System.Drawing.Size(351, 21);
            this.textBox_satNLFile.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "窄巷文件：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_selectFile
            // 
            this.button_selectFile.Location = new System.Drawing.Point(501, 91);
            this.button_selectFile.Name = "button_selectFile";
            this.button_selectFile.Size = new System.Drawing.Size(75, 23);
            this.button_selectFile.TabIndex = 6;
            this.button_selectFile.Text = "…";
            this.button_selectFile.UseVisualStyleBackColor = true;
            this.button_selectFile.Click += new System.EventHandler(this.button_selectFile_Click);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(595, 91);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(75, 23);
            this.button_delete.TabIndex = 7;
            this.button_delete.Text = "删除";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // DeleteData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 261);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_selectFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_satNLFile);
            this.Controls.Add(this.textBox_maxvalue);
            this.Controls.Add(this.textBox_minvalue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DeleteData";
            this.Text = "DeleteData";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_minvalue;
        private System.Windows.Forms.TextBox textBox_maxvalue;
        private System.Windows.Forms.TextBox textBox_satNLFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_selectFile;
        private System.Windows.Forms.Button button_delete;
    }
}