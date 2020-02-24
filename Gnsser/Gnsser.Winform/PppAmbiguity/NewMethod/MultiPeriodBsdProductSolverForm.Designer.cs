namespace Gnsser.Winform
{
    partial class MultiPeriodBsdProductSolverForm
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
            this.textBox_angleCut = new System.Windows.Forms.TextBox();
            this.fileOpenControl_pppResults = new Geo.Winform.Controls.FileOpenControl();
            this.namedIntControl_minEpoch = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_minSiteCount = new Geo.Winform.Controls.NamedIntControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_outputFraction = new System.Windows.Forms.CheckBox();
            this.checkBox_outputInt = new System.Windows.Forms.CheckBox();
            this.checkBox_outputSumery = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_satEleOfBaseSite = new Geo.Winform.Controls.FileOpenControl();
            this.textBox_timePeriodCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.fileOpenControl_nav = new Geo.Winform.Controls.FileOpenControl();
            this.namedFloatControl1maxAllowedDiffer = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBoxIsExpandPeriod = new System.Windows.Forms.CheckBox();
            this.namedFloatControl1AngleCut = new Geo.Winform.Controls.NamedFloatControl();
            this.namedIntControl_emptyRowCount = new Geo.Winform.Controls.NamedIntControl();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(661, 137);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.namedIntControl_emptyRowCount);
            this.panel4.Controls.Add(this.namedFloatControl1AngleCut);
            this.panel4.Controls.Add(this.checkBoxIsExpandPeriod);
            this.panel4.Controls.Add(this.namedFloatControl1maxAllowedDiffer);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.textBox_interval);
            this.panel4.Controls.Add(this.textBox_timePeriodCount);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.Add(this.namedIntControl_minEpoch);
            this.panel4.Controls.Add(this.namedIntControl_minSiteCount);
            this.panel4.Controls.Add(this.textBox_angleCut);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Size = new System.Drawing.Size(655, 120);
            this.panel4.Controls.SetChildIndex(this.label1, 0);
            this.panel4.Controls.SetChildIndex(this.textBox_angleCut, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_minSiteCount, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_minEpoch, 0);
            this.panel4.Controls.SetChildIndex(this.groupBox2, 0);
            this.panel4.Controls.SetChildIndex(this.label3, 0);
            this.panel4.Controls.SetChildIndex(this.label2, 0);
            this.panel4.Controls.SetChildIndex(this.textBox_timePeriodCount, 0);
            this.panel4.Controls.SetChildIndex(this.textBox_interval, 0);
            this.panel4.Controls.SetChildIndex(this.label4, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl1maxAllowedDiffer, 0);
            this.panel4.Controls.SetChildIndex(this.checkBoxIsExpandPeriod, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl1AngleCut, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_emptyRowCount, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.LabelName = "观测文件：";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(655, 34);
            this.fileOpenControl_inputPathes.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControl_satEleOfBaseSite);
            this.tabPage1.Controls.Add(this.fileOpenControl_nav);
            this.tabPage1.Controls.Add(this.fileOpenControl_pppResults);
            this.tabPage1.Size = new System.Drawing.Size(661, 126);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_inputPathes, 0);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_pppResults, 0);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_nav, 0);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_satEleOfBaseSite, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(401, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 62;
            this.label1.Text = "高度截止角:";
            // 
            // textBox_angleCut
            // 
            this.textBox_angleCut.Location = new System.Drawing.Point(478, 14);
            this.textBox_angleCut.Name = "textBox_angleCut";
            this.textBox_angleCut.Size = new System.Drawing.Size(51, 21);
            this.textBox_angleCut.TabIndex = 63;
            this.textBox_angleCut.Text = "20";
            // 
            // fileOpenControl_pppResults
            // 
            this.fileOpenControl_pppResults.AllowDrop = true;
            this.fileOpenControl_pppResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_pppResults.FilePath = "";
            this.fileOpenControl_pppResults.FilePathes = new string[0];
            this.fileOpenControl_pppResults.Filter = "PPP结果文件|*_Params.txt.xls|所有文件|*.*";
            this.fileOpenControl_pppResults.FirstPath = "";
            this.fileOpenControl_pppResults.IsMultiSelect = true;
            this.fileOpenControl_pppResults.LabelName = "PPP结果文件：";
            this.fileOpenControl_pppResults.Location = new System.Drawing.Point(3, 37);
            this.fileOpenControl_pppResults.Name = "fileOpenControl_pppResults";
            this.fileOpenControl_pppResults.Size = new System.Drawing.Size(655, 25);
            this.fileOpenControl_pppResults.TabIndex = 62;
            // 
            // namedIntControl_minEpoch
            // 
            this.namedIntControl_minEpoch.Location = new System.Drawing.Point(143, 72);
            this.namedIntControl_minEpoch.Name = "namedIntControl_minEpoch";
            this.namedIntControl_minEpoch.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_minEpoch.TabIndex = 66;
            this.namedIntControl_minEpoch.Title = "最小历元数：";
            this.namedIntControl_minEpoch.Value = 5;
            // 
            // namedIntControl_minSiteCount
            // 
            this.namedIntControl_minSiteCount.Location = new System.Drawing.Point(18, 72);
            this.namedIntControl_minSiteCount.Name = "namedIntControl_minSiteCount";
            this.namedIntControl_minSiteCount.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_minSiteCount.TabIndex = 67;
            this.namedIntControl_minSiteCount.Title = "最小测站数：";
            this.namedIntControl_minSiteCount.Value = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_outputFraction);
            this.groupBox2.Controls.Add(this.checkBox_outputInt);
            this.groupBox2.Controls.Add(this.checkBox_outputSumery);
            this.groupBox2.Location = new System.Drawing.Point(113, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 44);
            this.groupBox2.TabIndex = 68;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出设置";
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
            // fileOpenControl_satEleOfBaseSite
            // 
            this.fileOpenControl_satEleOfBaseSite.AllowDrop = true;
            this.fileOpenControl_satEleOfBaseSite.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_satEleOfBaseSite.FilePath = "";
            this.fileOpenControl_satEleOfBaseSite.FilePathes = new string[0];
            this.fileOpenControl_satEleOfBaseSite.Filter = "基准星分段文件|*.txt.xls|星高度角文件|*SatEle.txt.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_satEleOfBaseSite.FirstPath = "";
            this.fileOpenControl_satEleOfBaseSite.IsMultiSelect = false;
            this.fileOpenControl_satEleOfBaseSite.LabelName = "分段基准星或基准站卫星高度角文件(非必须)：";
            this.fileOpenControl_satEleOfBaseSite.Location = new System.Drawing.Point(3, 82);
            this.fileOpenControl_satEleOfBaseSite.Name = "fileOpenControl_satEleOfBaseSite";
            this.fileOpenControl_satEleOfBaseSite.Size = new System.Drawing.Size(655, 18);
            this.fileOpenControl_satEleOfBaseSite.TabIndex = 63;
            // 
            // textBox_timePeriodCount
            // 
            this.textBox_timePeriodCount.Location = new System.Drawing.Point(204, 46);
            this.textBox_timePeriodCount.Name = "textBox_timePeriodCount";
            this.textBox_timePeriodCount.Size = new System.Drawing.Size(58, 21);
            this.textBox_timePeriodCount.TabIndex = 71;
            this.textBox_timePeriodCount.Text = "8";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(281, 12);
            this.label2.TabIndex = 69;
            this.label2.Text = "未指定时段时，自动用基准测站卫星高度角文件计算";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 70;
            this.label3.Text = "时段分段数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(271, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 73;
            this.label4.Text = "采样间隔：";
            // 
            // textBox_interval
            // 
            this.textBox_interval.Location = new System.Drawing.Point(342, 72);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(49, 21);
            this.textBox_interval.TabIndex = 72;
            this.textBox_interval.Text = "30";
            // 
            // fileOpenControl_nav
            // 
            this.fileOpenControl_nav.AllowDrop = true;
            this.fileOpenControl_nav.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_nav.FilePath = "";
            this.fileOpenControl_nav.FilePathes = new string[0];
            this.fileOpenControl_nav.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_nav.FirstPath = "";
            this.fileOpenControl_nav.IsMultiSelect = false;
            this.fileOpenControl_nav.LabelName = "星历(非必须，推荐导航文件，更快)：";
            this.fileOpenControl_nav.Location = new System.Drawing.Point(3, 62);
            this.fileOpenControl_nav.Name = "fileOpenControl_nav";
            this.fileOpenControl_nav.Size = new System.Drawing.Size(655, 20);
            this.fileOpenControl_nav.TabIndex = 64;
            this.fileOpenControl_nav.FilePathSetted += new System.EventHandler(this.fileOpenControl_nav_FilePathSetted);
            // 
            // namedFloatControl1maxAllowedDiffer
            // 
            this.namedFloatControl1maxAllowedDiffer.Location = new System.Drawing.Point(407, 72);
            this.namedFloatControl1maxAllowedDiffer.Name = "namedFloatControl1maxAllowedDiffer";
            this.namedFloatControl1maxAllowedDiffer.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl1maxAllowedDiffer.TabIndex = 74;
            this.namedFloatControl1maxAllowedDiffer.Title = "允许最大偏差:";
            this.namedFloatControl1maxAllowedDiffer.Value = 0.25D;
            // 
            // checkBoxIsExpandPeriod
            // 
            this.checkBoxIsExpandPeriod.AutoSize = true;
            this.checkBoxIsExpandPeriod.Location = new System.Drawing.Point(21, 101);
            this.checkBoxIsExpandPeriod.Name = "checkBoxIsExpandPeriod";
            this.checkBoxIsExpandPeriod.Size = new System.Drawing.Size(228, 16);
            this.checkBoxIsExpandPeriod.TabIndex = 75;
            this.checkBoxIsExpandPeriod.Text = "若选星，是否扩展相同卫星编号的时段";
            this.checkBoxIsExpandPeriod.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl1AngleCut
            // 
            this.namedFloatControl1AngleCut.Location = new System.Drawing.Point(5, 49);
            this.namedFloatControl1AngleCut.Name = "namedFloatControl1AngleCut";
            this.namedFloatControl1AngleCut.Size = new System.Drawing.Size(110, 23);
            this.namedFloatControl1AngleCut.TabIndex = 78;
            this.namedFloatControl1AngleCut.Title = "高度截止角：";
            this.namedFloatControl1AngleCut.Value = 0D;
            // 
            // namedIntControl_emptyRowCount
            // 
            this.namedIntControl_emptyRowCount.Location = new System.Drawing.Point(273, 101);
            this.namedIntControl_emptyRowCount.Name = "namedIntControl_emptyRowCount";
            this.namedIntControl_emptyRowCount.Size = new System.Drawing.Size(165, 23);
            this.namedIntControl_emptyRowCount.TabIndex = 79;
            this.namedIntControl_emptyRowCount.Title = "清除MW滤波前面数据：";
            this.namedIntControl_emptyRowCount.Value = 20;
            // 
            // MultiPeriodBsdProductSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 405);
            this.IsShowProgressBar = true;
            this.Name = "MultiPeriodBsdProductSolverForm";
            this.Text = "BSD集成计算器";
            this.Load += new System.EventHandler(this.WideLaneSolverForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_angleCut;
        private System.Windows.Forms.Label label1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_pppResults;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_minEpoch;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_minSiteCount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_outputFraction;
        private System.Windows.Forms.CheckBox checkBox_outputInt;
        private System.Windows.Forms.CheckBox checkBox_outputSumery;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_satEleOfBaseSite;
        private System.Windows.Forms.TextBox textBox_timePeriodCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_interval;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_nav;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1maxAllowedDiffer;
        private System.Windows.Forms.CheckBox checkBoxIsExpandPeriod;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1AngleCut;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_emptyRowCount;


    }
}