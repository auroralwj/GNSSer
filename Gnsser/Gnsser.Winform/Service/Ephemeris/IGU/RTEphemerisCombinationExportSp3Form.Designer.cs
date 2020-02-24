namespace Gnsser.Winform
{
    partial class RTEphemerisCombinationExportSp3Form
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
            this.openFileDialog_SSRsp3 = new System.Windows.Forms.OpenFileDialog();
            this.button_export = new System.Windows.Forms.Button();
            this.textBox_SSRsp3Pathes = new System.Windows.Forms.TextBox();
            this.button_getSSRsp3Path = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_getNavPath = new System.Windows.Forms.Button();
            this.textBox_NavPathes = new System.Windows.Forms.TextBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.openFileDialog_Nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_SSRsp3
            // 
            this.openFileDialog_SSRsp3.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            this.openFileDialog_SSRsp3.Multiselect = true;
            this.openFileDialog_SSRsp3.Title = "请选择文件";
            // 
            // button_export
            // 
            this.button_export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_export.Location = new System.Drawing.Point(607, 292);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(75, 23);
            this.button_export.TabIndex = 19;
            this.button_export.Text = "导出";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_read_Click);
            // 
            // textBox_SSRsp3Pathes
            // 
            this.textBox_SSRsp3Pathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SSRsp3Pathes.Location = new System.Drawing.Point(97, 13);
            this.textBox_SSRsp3Pathes.Multiline = true;
            this.textBox_SSRsp3Pathes.Name = "textBox_SSRsp3Pathes";
            this.textBox_SSRsp3Pathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_SSRsp3Pathes.Size = new System.Drawing.Size(488, 45);
            this.textBox_SSRsp3Pathes.TabIndex = 18;
            // 
            // button_getSSRsp3Path
            // 
            this.button_getSSRsp3Path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getSSRsp3Path.Location = new System.Drawing.Point(601, 11);
            this.button_getSSRsp3Path.Name = "button_getSSRsp3Path";
            this.button_getSSRsp3Path.Size = new System.Drawing.Size(50, 47);
            this.button_getSSRsp3Path.TabIndex = 16;
            this.button_getSSRsp3Path.Text = "...";
            this.button_getSSRsp3Path.UseVisualStyleBackColor = true;
            this.button_getSSRsp3Path.Click += new System.EventHandler(this.button_getSSRsp3Path_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "SSRsp3文件：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button_getNavPath);
            this.groupBox1.Controls.Add(this.textBox_NavPathes);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_getSSRsp3Path);
            this.groupBox1.Controls.Add(this.textBox_SSRsp3Pathes);
            this.groupBox1.Location = new System.Drawing.Point(22, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(666, 273);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "广播星历文件：";
            // 
            // button_getNavPath
            // 
            this.button_getNavPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getNavPath.Location = new System.Drawing.Point(601, 84);
            this.button_getNavPath.Name = "button_getNavPath";
            this.button_getNavPath.Size = new System.Drawing.Size(50, 47);
            this.button_getNavPath.TabIndex = 22;
            this.button_getNavPath.Text = "...";
            this.button_getNavPath.UseVisualStyleBackColor = true;
            this.button_getNavPath.Click += new System.EventHandler(this.button_getNavPath_Click);
            // 
            // textBox_NavPathes
            // 
            this.textBox_NavPathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_NavPathes.Location = new System.Drawing.Point(97, 86);
            this.textBox_NavPathes.Multiline = true;
            this.textBox_NavPathes.Name = "textBox_NavPathes";
            this.textBox_NavPathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_NavPathes.Size = new System.Drawing.Size(488, 45);
            this.textBox_NavPathes.TabIndex = 24;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(28, 219);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(632, 22);
            this.directorySelectionControl1.TabIndex = 21;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(512, 64);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(126, 16);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "若无数据，以0填充";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // openFileDialog_Nav
            // 
            this.openFileDialog_Nav.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            this.openFileDialog_Nav.Multiselect = true;
            this.openFileDialog_Nav.Title = "请选择文件";
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "openFileDialog2";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk|所有文件|*.*";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(80, 154);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(505, 45);
            this.textBox1.TabIndex = 40;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(601, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 47);
            this.button1.TabIndex = 38;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 39;
            this.label5.Text = "钟差文件：";
            // 
            // RTEphemerisCombinationExportSp3Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 352);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_export);
            this.Name = "RTEphemerisCombinationExportSp3Form";
            this.Text = "星历文件导出";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_SSRsp3;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.TextBox textBox_SSRsp3Pathes;
        private System.Windows.Forms.Button button_getSSRsp3Path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_getNavPath;
        private System.Windows.Forms.TextBox textBox_NavPathes;
        private System.Windows.Forms.OpenFileDialog openFileDialog_Nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
    }
}