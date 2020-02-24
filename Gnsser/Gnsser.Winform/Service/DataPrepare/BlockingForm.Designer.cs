namespace Gnsser.Winform
{
    partial class BlockingForm
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
            this.button1_save = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_commonSite = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_blockSite = new System.Windows.Forms.TextBox();
            this.button_reset = new System.Windows.Forms.Button();
            this.button_filterCommonSite = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1_save
            // 
            this.button1_save.Location = new System.Drawing.Point(13, 13);
            this.button1_save.Name = "button1_save";
            this.button1_save.Size = new System.Drawing.Size(75, 23);
            this.button1_save.TabIndex = 0;
            this.button1_save.Text = "保存";
            this.button1_save.UseVisualStyleBackColor = true;
            this.button1_save.Click += new System.EventHandler(this.button1_save_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(13, 42);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_commonSite);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button_filterCommonSite);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_blockSite);
            this.splitContainer1.Size = new System.Drawing.Size(579, 372);
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "公共测站：";
            // 
            // textBox_commonSite
            // 
            this.textBox_commonSite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_commonSite.Location = new System.Drawing.Point(3, 27);
            this.textBox_commonSite.Multiline = true;
            this.textBox_commonSite.Name = "textBox_commonSite";
            this.textBox_commonSite.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_commonSite.Size = new System.Drawing.Size(573, 156);
            this.textBox_commonSite.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "区内参数，一行为一个分区，逗号隔开";
            // 
            // textBox_blockSite
            // 
            this.textBox_blockSite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_blockSite.Location = new System.Drawing.Point(6, 24);
            this.textBox_blockSite.Multiline = true;
            this.textBox_blockSite.Name = "textBox_blockSite";
            this.textBox_blockSite.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_blockSite.Size = new System.Drawing.Size(573, 155);
            this.textBox_blockSite.TabIndex = 0;
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(105, 12);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 23);
            this.button_reset.TabIndex = 2;
            this.button_reset.Text = "重置";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // button_filterCommonSite
            // 
            this.button_filterCommonSite.Location = new System.Drawing.Point(234, -1);
            this.button_filterCommonSite.Name = "button_filterCommonSite";
            this.button_filterCommonSite.Size = new System.Drawing.Size(103, 23);
            this.button_filterCommonSite.TabIndex = 3;
            this.button_filterCommonSite.Text = "过滤公共测站";
            this.button_filterCommonSite.UseVisualStyleBackColor = true;
            this.button_filterCommonSite.Click += new System.EventHandler(this.button_filterCommonSite_Click);
            // 
            // BlockingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 426);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1_save);
            this.Name = "BlockingForm";
            this.Text = "BlockingForm";
            this.Load += new System.EventHandler(this.BlockingForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1_save;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox_commonSite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_blockSite;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Button button_filterCommonSite;
    }
}