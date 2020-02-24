namespace Geo.Winform
{
    partial class FileOpenForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_read = new System.Windows.Forms.Button();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Location = new System.Drawing.Point(4, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 61);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " 信息";
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(383, 84);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 29);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(302, 84);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 29);
            this.button_read.TabIndex = 2;
            this.button_read.Text = "确定";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(8, 20);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(438, 22);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // FileOpenForm
            // 
            this.AcceptButton = this.button_read;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(470, 121);
            this.Controls.Add(this.button_read);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.groupBox1);
            this.Name = "FileOpenForm";
            this.Text = "打开文件";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_read;
    }
}