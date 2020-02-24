namespace Geo.Winform.Tools
{
    partial class LocalToGeoCoordForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_geo = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton_xyz = new System.Windows.Forms.RadioButton();
            this.textBox_siteCoord = new System.Windows.Forms.TextBox();
            this.richTextBox_neu = new System.Windows.Forms.RichTextBox();
            this.richTextBox_ecefXyz = new System.Windows.Forms.RichTextBox();
            this.button_neuToXyz = new System.Windows.Forms.Button();
            this.button_xyzToNeu = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
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
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButton_geo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton_xyz);
            this.groupBox1.Controls.Add(this.textBox_siteCoord);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(615, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // radioButton_geo
            // 
            this.radioButton_geo.AutoSize = true;
            this.radioButton_geo.Location = new System.Drawing.Point(493, 25);
            this.radioButton_geo.Name = "radioButton_geo";
            this.radioButton_geo.Size = new System.Drawing.Size(71, 16);
            this.radioButton_geo.TabIndex = 8;
            this.radioButton_geo.TabStop = true;
            this.radioButton_geo.Text = "大地坐标";
            this.radioButton_geo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "测站地心坐标：";
            // 
            // radioButton_xyz
            // 
            this.radioButton_xyz.AutoSize = true;
            this.radioButton_xyz.Checked = true;
            this.radioButton_xyz.Location = new System.Drawing.Point(391, 25);
            this.radioButton_xyz.Name = "radioButton_xyz";
            this.radioButton_xyz.Size = new System.Drawing.Size(95, 16);
            this.radioButton_xyz.TabIndex = 7;
            this.radioButton_xyz.TabStop = true;
            this.radioButton_xyz.Text = "空间直角坐标";
            this.radioButton_xyz.UseVisualStyleBackColor = true;
            // 
            // textBox_siteCoord
            // 
            this.textBox_siteCoord.Location = new System.Drawing.Point(113, 23);
            this.textBox_siteCoord.Name = "textBox_siteCoord";
            this.textBox_siteCoord.Size = new System.Drawing.Size(258, 21);
            this.textBox_siteCoord.TabIndex = 4;
            this.textBox_siteCoord.Text = "1000000,1000000,1000000";
            // 
            // richTextBox_neu
            // 
            this.richTextBox_neu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_neu.Location = new System.Drawing.Point(0, 12);
            this.richTextBox_neu.Name = "richTextBox_neu";
            this.richTextBox_neu.Size = new System.Drawing.Size(205, 405);
            this.richTextBox_neu.TabIndex = 1;
            this.richTextBox_neu.Text = "100,100,100";
            // 
            // richTextBox_ecefXyz
            // 
            this.richTextBox_ecefXyz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_ecefXyz.Location = new System.Drawing.Point(0, 12);
            this.richTextBox_ecefXyz.Name = "richTextBox_ecefXyz";
            this.richTextBox_ecefXyz.Size = new System.Drawing.Size(268, 405);
            this.richTextBox_ecefXyz.TabIndex = 2;
            this.richTextBox_ecefXyz.Text = "";
            // 
            // button_neuToXyz
            // 
            this.button_neuToXyz.Location = new System.Drawing.Point(27, 37);
            this.button_neuToXyz.Name = "button_neuToXyz";
            this.button_neuToXyz.Size = new System.Drawing.Size(75, 23);
            this.button_neuToXyz.TabIndex = 3;
            this.button_neuToXyz.Text = "->";
            this.button_neuToXyz.UseVisualStyleBackColor = true;
            this.button_neuToXyz.Click += new System.EventHandler(this.button_neuToXyz_Click);
            // 
            // button_xyzToNeu
            // 
            this.button_xyzToNeu.Location = new System.Drawing.Point(27, 97);
            this.button_xyzToNeu.Name = "button_xyzToNeu";
            this.button_xyzToNeu.Size = new System.Drawing.Size(75, 23);
            this.button_xyzToNeu.TabIndex = 6;
            this.button_xyzToNeu.Text = "<-";
            this.button_xyzToNeu.UseVisualStyleBackColor = true;
            this.button_xyzToNeu.Click += new System.EventHandler(this.button_xyzToNeu_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "站心坐标ENU：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "地心地固空间直角坐标：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 72);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox_neu);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(617, 417);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 8;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button_neuToXyz);
            this.splitContainer2.Panel1.Controls.Add(this.button_xyzToNeu);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox_ecefXyz);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Size = new System.Drawing.Size(408, 417);
            this.splitContainer2.SplitterDistance = 136;
            this.splitContainer2.TabIndex = 0;
            // 
            // LocalToGeoCoordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 500);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Name = "LocalToGeoCoordForm";
            this.Text = "站心坐标与地心坐标的转换";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
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
        private System.Windows.Forms.RadioButton radioButton_geo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton_xyz;
        private System.Windows.Forms.TextBox textBox_siteCoord;
        private System.Windows.Forms.RichTextBox richTextBox_neu;
        private System.Windows.Forms.RichTextBox richTextBox_ecefXyz;
        private System.Windows.Forms.Button button_neuToXyz;
        private System.Windows.Forms.Button button_xyzToNeu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}

