namespace Gnsser.Winform
{
    partial class MultiPeriodlWideLaneOfBsdSolverForm
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
            this.checkBox_outputFraction = new System.Windows.Forms.CheckBox();
            this.checkBox_outputInt = new System.Windows.Forms.CheckBox();
            this.checkBox_outputSumery = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_timePeriodCount = new System.Windows.Forms.TextBox();
            this.namedIntControl_minSiteCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_minEpoch = new Geo.Winform.Controls.NamedIntControl();
            this.fileOpenControl_satEleOfBaseSite = new Geo.Winform.Controls.FileOpenControl();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1IsOutputInEachDirectory = new System.Windows.Forms.CheckBox();
            this.checkBoxIsExpandPeriod = new System.Windows.Forms.CheckBox();
            this.namedFloatControl1AngleCut = new Geo.Winform.Controls.NamedFloatControl();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(655, 148);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.namedFloatControl1AngleCut);
            this.panel4.Controls.Add(this.checkBoxIsExpandPeriod);
            this.panel4.Controls.Add(this.checkBox1IsOutputInEachDirectory);
            this.panel4.Controls.Add(this.namedIntControl_minEpoch);
            this.panel4.Controls.Add(this.namedIntControl_minSiteCount);
            this.panel4.Controls.Add(this.textBox_timePeriodCount);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Size = new System.Drawing.Size(649, 123);
            this.panel4.Controls.SetChildIndex(this.groupBox2, 0);
            this.panel4.Controls.SetChildIndex(this.label1, 0);
            this.panel4.Controls.SetChildIndex(this.label2, 0);
            this.panel4.Controls.SetChildIndex(this.textBox_timePeriodCount, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_minSiteCount, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_minEpoch, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox1IsOutputInEachDirectory, 0);
            this.panel4.Controls.SetChildIndex(this.checkBoxIsExpandPeriod, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl1AngleCut, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Filter = "WM平滑文件(*_SmoothedMw.txt.xls)|*_SmoothedMw.txt.xls|WM原始文件|*_MwRaw.txt.xls|O文件|*.*O" +
    "|RINEX压缩文件(*.*D;*.*D.Z)|*.*D.Z;*.*D|所有文件|*.*";
            this.fileOpenControl_inputPathes.LabelName = "MW平滑文件：";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(649, 70);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControl_satEleOfBaseSite);
            this.tabPage1.Size = new System.Drawing.Size(655, 129);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_inputPathes, 0);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_satEleOfBaseSite, 0);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(396, 0);
            // 
            // checkBox_outputFraction
            // 
            this.checkBox_outputFraction.AutoSize = true;
            this.checkBox_outputFraction.Checked = true;
            this.checkBox_outputFraction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_outputFraction.Location = new System.Drawing.Point(13, 20);
            this.checkBox_outputFraction.Name = "checkBox_outputFraction";
            this.checkBox_outputFraction.Size = new System.Drawing.Size(96, 16);
            this.checkBox_outputFraction.TabIndex = 57;
            this.checkBox_outputFraction.Text = "输出小数部分";
            this.checkBox_outputFraction.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputInt
            // 
            this.checkBox_outputInt.AutoSize = true;
            this.checkBox_outputInt.Checked = true;
            this.checkBox_outputInt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_outputInt.Location = new System.Drawing.Point(121, 20);
            this.checkBox_outputInt.Name = "checkBox_outputInt";
            this.checkBox_outputInt.Size = new System.Drawing.Size(96, 16);
            this.checkBox_outputInt.TabIndex = 57;
            this.checkBox_outputInt.Text = "输出整数部分";
            this.checkBox_outputInt.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputSumery
            // 
            this.checkBox_outputSumery.AutoSize = true;
            this.checkBox_outputSumery.Checked = true;
            this.checkBox_outputSumery.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_outputSumery.Location = new System.Drawing.Point(229, 20);
            this.checkBox_outputSumery.Name = "checkBox_outputSumery";
            this.checkBox_outputSumery.Size = new System.Drawing.Size(72, 16);
            this.checkBox_outputSumery.TabIndex = 57;
            this.checkBox_outputSumery.Text = "输出汇总";
            this.checkBox_outputSumery.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_outputFraction);
            this.groupBox2.Controls.Add(this.checkBox_outputInt);
            this.groupBox2.Controls.Add(this.checkBox_outputSumery);
            this.groupBox2.Location = new System.Drawing.Point(110, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 44);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出设置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 61;
            this.label1.Text = "时段分段数：";
            // 
            // textBox_timePeriodCount
            // 
            this.textBox_timePeriodCount.Location = new System.Drawing.Point(231, 50);
            this.textBox_timePeriodCount.Name = "textBox_timePeriodCount";
            this.textBox_timePeriodCount.Size = new System.Drawing.Size(31, 21);
            this.textBox_timePeriodCount.TabIndex = 62;
            this.textBox_timePeriodCount.Text = "8";
            // 
            // namedIntControl_minSiteCount
            // 
            this.namedIntControl_minSiteCount.Location = new System.Drawing.Point(10, 77);
            this.namedIntControl_minSiteCount.Name = "namedIntControl_minSiteCount";
            this.namedIntControl_minSiteCount.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_minSiteCount.TabIndex = 63;
            this.namedIntControl_minSiteCount.Title = "最小测站数：";
            this.namedIntControl_minSiteCount.Value = 5;
            // 
            // namedIntControl_minEpoch
            // 
            this.namedIntControl_minEpoch.Location = new System.Drawing.Point(147, 77);
            this.namedIntControl_minEpoch.Name = "namedIntControl_minEpoch";
            this.namedIntControl_minEpoch.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_minEpoch.TabIndex = 63;
            this.namedIntControl_minEpoch.Title = "最小历元数：";
            this.namedIntControl_minEpoch.Value = 40;
            // 
            // fileOpenControl_satEleOfBaseSite
            // 
            this.fileOpenControl_satEleOfBaseSite.AllowDrop = true;
            this.fileOpenControl_satEleOfBaseSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenControl_satEleOfBaseSite.FilePath = "";
            this.fileOpenControl_satEleOfBaseSite.FilePathes = new string[0];
            this.fileOpenControl_satEleOfBaseSite.Filter = "基准星分段文件|*.txt.xls|星高度角文件|*SatEle.txt.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_satEleOfBaseSite.FirstPath = "";
            this.fileOpenControl_satEleOfBaseSite.IsMultiSelect = false;
            this.fileOpenControl_satEleOfBaseSite.LabelName = "基准星分段或基准站卫星高度角文件：";
            this.fileOpenControl_satEleOfBaseSite.Location = new System.Drawing.Point(3, 73);
            this.fileOpenControl_satEleOfBaseSite.Name = "fileOpenControl_satEleOfBaseSite";
            this.fileOpenControl_satEleOfBaseSite.Size = new System.Drawing.Size(649, 31);
            this.fileOpenControl_satEleOfBaseSite.TabIndex = 61;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(281, 12);
            this.label2.TabIndex = 61;
            this.label2.Text = "未指定时段时，自动用基准测站卫星高度角文件计算";
            // 
            // checkBox1IsOutputInEachDirectory
            // 
            this.checkBox1IsOutputInEachDirectory.AutoSize = true;
            this.checkBox1IsOutputInEachDirectory.Location = new System.Drawing.Point(289, 77);
            this.checkBox1IsOutputInEachDirectory.Name = "checkBox1IsOutputInEachDirectory";
            this.checkBox1IsOutputInEachDirectory.Size = new System.Drawing.Size(132, 16);
            this.checkBox1IsOutputInEachDirectory.TabIndex = 64;
            this.checkBox1IsOutputInEachDirectory.Text = "各时段单独目录输出";
            this.checkBox1IsOutputInEachDirectory.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsExpandPeriod
            // 
            this.checkBoxIsExpandPeriod.AutoSize = true;
            this.checkBoxIsExpandPeriod.Location = new System.Drawing.Point(17, 101);
            this.checkBoxIsExpandPeriod.Name = "checkBoxIsExpandPeriod";
            this.checkBoxIsExpandPeriod.Size = new System.Drawing.Size(228, 16);
            this.checkBoxIsExpandPeriod.TabIndex = 76;
            this.checkBoxIsExpandPeriod.Text = "若选星，是否扩展相同卫星编号的时段";
            this.checkBoxIsExpandPeriod.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl1AngleCut
            // 
            this.namedFloatControl1AngleCut.Location = new System.Drawing.Point(5, 48);
            this.namedFloatControl1AngleCut.Name = "namedFloatControl1AngleCut";
            this.namedFloatControl1AngleCut.Size = new System.Drawing.Size(124, 23);
            this.namedFloatControl1AngleCut.TabIndex = 77;
            this.namedFloatControl1AngleCut.Title = "高度截止角：";
            this.namedFloatControl1AngleCut.Value = 0D;
            // 
            // MultiPeriodlWideLaneOfBsdSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 405);
            this.IsShowProgressBar = true;
            this.Name = "MultiPeriodlWideLaneOfBsdSolverForm";
            this.Text = "全历元分段BSD宽巷计算器";
            this.Load += new System.EventHandler(this.WideLaneSolverForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_outputSumery;
        private System.Windows.Forms.CheckBox checkBox_outputInt;
        private System.Windows.Forms.CheckBox checkBox_outputFraction;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_timePeriodCount;
        private System.Windows.Forms.Label label1;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_minEpoch;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_minSiteCount;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_satEleOfBaseSite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1IsOutputInEachDirectory;
        private System.Windows.Forms.CheckBox checkBoxIsExpandPeriod;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1AngleCut;


    }
}