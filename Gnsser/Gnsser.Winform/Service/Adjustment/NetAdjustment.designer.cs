namespace Gnsser.Winform
{
    partial class NetAdjustment
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
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_SaveTofile = new System.Windows.Forms.Button();
            this.button_ShowOnMap = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileOpenControl2_KnownPointName = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl2baselinefiles = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.radioButton_fixedConstraintAdj = new System.Windows.Forms.RadioButton();
            this.radioButton_freeNetAdj = new System.Windows.Forms.RadioButton();
            this.radioButton_minConstraintAdj = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_bayesRobust = new System.Windows.Forms.RadioButton();
            this.radioButton_Robust = new System.Windows.Forms.RadioButton();
            this.button_Calculate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_info.Location = new System.Drawing.Point(18, 386);
            this.textBox_info.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(1200, 146);
            this.textBox_info.TabIndex = 47;
            // 
            // button_SaveTofile
            // 
            this.button_SaveTofile.Location = new System.Drawing.Point(15, 345);
            this.button_SaveTofile.Margin = new System.Windows.Forms.Padding(4);
            this.button_SaveTofile.Name = "button_SaveTofile";
            this.button_SaveTofile.Size = new System.Drawing.Size(138, 34);
            this.button_SaveTofile.TabIndex = 46;
            this.button_SaveTofile.Text = "导出结果";
            this.button_SaveTofile.UseVisualStyleBackColor = true;
            this.button_SaveTofile.Click += new System.EventHandler(this.button_saveTofile_Click);
            // 
            // button_ShowOnMap
            // 
            this.button_ShowOnMap.Location = new System.Drawing.Point(176, 345);
            this.button_ShowOnMap.Margin = new System.Windows.Forms.Padding(4);
            this.button_ShowOnMap.Name = "button_ShowOnMap";
            this.button_ShowOnMap.Size = new System.Drawing.Size(136, 34);
            this.button_ShowOnMap.TabIndex = 45;
            this.button_ShowOnMap.Text = "地图上显示";
            this.button_ShowOnMap.UseVisualStyleBackColor = true;
            this.button_ShowOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileOpenControl2_KnownPointName);
            this.groupBox1.Controls.Add(this.fileOpenControl2baselinefiles);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Location = new System.Drawing.Point(15, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1191, 231);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // fileOpenControl2_KnownPointName
            // 
            this.fileOpenControl2_KnownPointName.AllowDrop = true;
            this.fileOpenControl2_KnownPointName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl2_KnownPointName.FilePath = "";
            this.fileOpenControl2_KnownPointName.FilePathes = new string[0];
            this.fileOpenControl2_KnownPointName.Filter = "Txt文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            this.fileOpenControl2_KnownPointName.IsMultiSelect = false;
            this.fileOpenControl2_KnownPointName.LabelName = "已知点信息：";
            this.fileOpenControl2_KnownPointName.Location = new System.Drawing.Point(20, 35);
            this.fileOpenControl2_KnownPointName.Margin = new System.Windows.Forms.Padding(6);
            this.fileOpenControl2_KnownPointName.Name = "fileOpenControl2_KnownPointName";
            this.fileOpenControl2_KnownPointName.Size = new System.Drawing.Size(1137, 33);
            this.fileOpenControl2_KnownPointName.TabIndex = 54;
            // 
            // fileOpenControl2baselinefiles
            // 
            this.fileOpenControl2baselinefiles.AllowDrop = true;
            this.fileOpenControl2baselinefiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl2baselinefiles.FilePath = "";
            this.fileOpenControl2baselinefiles.FilePathes = new string[0];
            this.fileOpenControl2baselinefiles.Filter = "Xlsx文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            this.fileOpenControl2baselinefiles.IsMultiSelect = true;
            this.fileOpenControl2baselinefiles.LabelName = "基线文件：";
            this.fileOpenControl2baselinefiles.Location = new System.Drawing.Point(40, 131);
            this.fileOpenControl2baselinefiles.Margin = new System.Windows.Forms.Padding(6);
            this.fileOpenControl2baselinefiles.Name = "fileOpenControl2baselinefiles";
            this.fileOpenControl2baselinefiles.Size = new System.Drawing.Size(1118, 86);
            this.fileOpenControl2baselinefiles.TabIndex = 53;
            this.fileOpenControl2baselinefiles.Load += new System.EventHandler(this.fileOpenControl2baselinefiles_Load);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "SINEX文件(*.snx)|*.snx|所有文件(*.*)|*.*";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "已知点坐标：";
            this.fileOpenControl1.Location = new System.Drawing.Point(21, 84);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(6);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(1137, 33);
            this.fileOpenControl1.TabIndex = 53;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.saveFileDialog1.Title = "保存Sinex文件";
            // 
            // radioButton_fixedConstraintAdj
            // 
            this.radioButton_fixedConstraintAdj.AutoSize = true;
            this.radioButton_fixedConstraintAdj.Checked = true;
            this.radioButton_fixedConstraintAdj.Location = new System.Drawing.Point(6, 38);
            this.radioButton_fixedConstraintAdj.Name = "radioButton_fixedConstraintAdj";
            this.radioButton_fixedConstraintAdj.Size = new System.Drawing.Size(159, 22);
            this.radioButton_fixedConstraintAdj.TabIndex = 48;
            this.radioButton_fixedConstraintAdj.TabStop = true;
            this.radioButton_fixedConstraintAdj.Text = "固定点约束平差";
            this.radioButton_fixedConstraintAdj.UseVisualStyleBackColor = true;
            // 
            // radioButton_freeNetAdj
            // 
            this.radioButton_freeNetAdj.AutoSize = true;
            this.radioButton_freeNetAdj.Location = new System.Drawing.Point(354, 34);
            this.radioButton_freeNetAdj.Name = "radioButton_freeNetAdj";
            this.radioButton_freeNetAdj.Size = new System.Drawing.Size(123, 22);
            this.radioButton_freeNetAdj.TabIndex = 49;
            this.radioButton_freeNetAdj.Text = "自由网平差";
            this.radioButton_freeNetAdj.UseVisualStyleBackColor = true;
            // 
            // radioButton_minConstraintAdj
            // 
            this.radioButton_minConstraintAdj.AutoSize = true;
            this.radioButton_minConstraintAdj.Location = new System.Drawing.Point(171, 34);
            this.radioButton_minConstraintAdj.Name = "radioButton_minConstraintAdj";
            this.radioButton_minConstraintAdj.Size = new System.Drawing.Size(177, 22);
            this.radioButton_minConstraintAdj.TabIndex = 50;
            this.radioButton_minConstraintAdj.Text = "最小条件约束平差";
            this.radioButton_minConstraintAdj.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_bayesRobust);
            this.groupBox3.Controls.Add(this.radioButton_Robust);
            this.groupBox3.Controls.Add(this.radioButton_fixedConstraintAdj);
            this.groupBox3.Controls.Add(this.radioButton_minConstraintAdj);
            this.groupBox3.Controls.Add(this.radioButton_freeNetAdj);
            this.groupBox3.Location = new System.Drawing.Point(18, 248);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(855, 63);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "网平差类型选择器";
            // 
            // radioButton_bayesRobust
            // 
            this.radioButton_bayesRobust.AutoSize = true;
            this.radioButton_bayesRobust.Location = new System.Drawing.Point(638, 34);
            this.radioButton_bayesRobust.Name = "radioButton_bayesRobust";
            this.radioButton_bayesRobust.Size = new System.Drawing.Size(195, 22);
            this.radioButton_bayesRobust.TabIndex = 52;
            this.radioButton_bayesRobust.Text = "贝叶斯相关抗差估计";
            this.radioButton_bayesRobust.UseVisualStyleBackColor = true;
            this.radioButton_bayesRobust.Visible = false;
            // 
            // radioButton_Robust
            // 
            this.radioButton_Robust.AutoSize = true;
            this.radioButton_Robust.Location = new System.Drawing.Point(490, 34);
            this.radioButton_Robust.Name = "radioButton_Robust";
            this.radioButton_Robust.Size = new System.Drawing.Size(141, 22);
            this.radioButton_Robust.TabIndex = 51;
            this.radioButton_Robust.Text = "相关抗差估计";
            this.radioButton_Robust.UseVisualStyleBackColor = true;
            this.radioButton_Robust.Visible = false;
            // 
            // button_Calculate
            // 
            this.button_Calculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Calculate.Location = new System.Drawing.Point(1078, 260);
            this.button_Calculate.Name = "button_Calculate";
            this.button_Calculate.Size = new System.Drawing.Size(134, 45);
            this.button_Calculate.TabIndex = 52;
            this.button_Calculate.Text = "计算";
            this.button_Calculate.UseVisualStyleBackColor = true;
            this.button_Calculate.Click += new System.EventHandler(this.button_Calculate_Click);
            // 
            // NetAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 646);
            this.Controls.Add(this.button_Calculate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.textBox_info);
            this.Controls.Add(this.button_SaveTofile);
            this.Controls.Add(this.button_ShowOnMap);
            this.Controls.Add(this.groupBox1);
            this.Name = "NetAdjustment";
            this.Text = "网平差";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Button button_SaveTofile;
        private System.Windows.Forms.Button button_ShowOnMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton radioButton_fixedConstraintAdj;
        private System.Windows.Forms.RadioButton radioButton_freeNetAdj;
        private System.Windows.Forms.RadioButton radioButton_minConstraintAdj;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_bayesRobust;
        private System.Windows.Forms.RadioButton radioButton_Robust;
        private System.Windows.Forms.Button button_Calculate;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl2baselinefiles;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl2_KnownPointName;
    }
}