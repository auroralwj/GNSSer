namespace Gnsser.Winform
{
    partial class FtpDownloaderForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxControl_urls = new Geo.Winform.Controls.RichTextBoxControl();
            this.label1 = new System.Windows.Forms.Label();
            this.button_down = new System.Windows.Forms.Button();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.richTextBoxControl_urls);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 107);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // richTextBoxControl_urls
            // 
            this.richTextBoxControl_urls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl_urls.Location = new System.Drawing.Point(66, 21);
            this.richTextBoxControl_urls.Name = "richTextBoxControl_urls";
            this.richTextBoxControl_urls.Size = new System.Drawing.Size(469, 66);
            this.richTextBoxControl_urls.TabIndex = 2;
            this.richTextBoxControl_urls.Text = "ftp://127.0.0.1/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地址:";
            // 
            // button_down
            // 
            this.button_down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_down.Location = new System.Drawing.Point(479, 130);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(75, 23);
            this.button_down.TabIndex = 1;
            this.button_down.Text = "下载";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(12, 159);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(542, 114);
            this.richTextBoxControl1.TabIndex = 2;
            this.richTextBoxControl1.Text = "";
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "本地目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(13, 131);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(437, 22);
            this.directorySelectionControl1.TabIndex = 3;
            // 
            // FtpDownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 285);
            this.Controls.Add(this.directorySelectionControl1);
            this.Controls.Add(this.richTextBoxControl1);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.groupBox1);
            this.Name = "FtpDownloaderForm";
            this.Text = "FtpDownloaderForm";
            this.Load += new System.EventHandler(this.FtpDownloaderForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_down;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_urls;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
    }
}