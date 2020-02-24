namespace Gnsser.Winform
{
    partial class MultiPeriodNarrowLaneOfBsdSolverForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_outputFraction = new System.Windows.Forms.CheckBox();
            this.checkBox_outputInt = new System.Windows.Forms.CheckBox();
            this.checkBox_outputSumery = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_intWideLaneFiles = new Geo.Winform.Controls.FileOpenControl();
            this.namedFloatControl1maxAllowedDiffer = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox1IsOutputInEachDirectory = new System.Windows.Forms.CheckBox();
            this.baseSatSelectingControl1 = new Gnsser.Winform.Controls.BaseSatSelectingControl();
            this.checkBox_calculateAllSat = new System.Windows.Forms.CheckBox();
            this.namedIntControl1MinSiteCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_removeCountOfEachSegment = new Geo.Winform.Controls.NamedIntControl();
            this.namedFloatControl_maxRMSTImes = new Geo.Winform.Controls.NamedFloatControl();
            this.fileOpenControl_fcbPathes = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_combine = new System.Windows.Forms.Button();
            this.namedFloatControl_maxRms = new Geo.Winform.Controls.NamedFloatControl();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(870, 269);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Controls.Add(this.baseSatSelectingControl1);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.namedIntControl_removeCountOfEachSegment);
            this.panel4.Controls.Add(this.namedIntControl1MinSiteCount);
            this.panel4.Controls.Add(this.checkBox_calculateAllSat);
            this.panel4.Controls.Add(this.checkBox1IsOutputInEachDirectory);
            this.panel4.Controls.Add(this.namedFloatControl_maxRMSTImes);
            this.panel4.Controls.Add(this.namedFloatControl_maxRms);
            this.panel4.Controls.Add(this.namedFloatControl1maxAllowedDiffer);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.SetChildIndex(this.groupBox2, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl1maxAllowedDiffer, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl_maxRms, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl_maxRMSTImes, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox1IsOutputInEachDirectory, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_calculateAllSat, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl1MinSiteCount, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_removeCountOfEachSegment, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Filter = "PPP结果文件|*_Params.txt.xls|所有文件|*.*";
            this.fileOpenControl_inputPathes.LabelName = "PPP结果文件：";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(864, 66);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControl_intWideLaneFiles);
            this.tabPage1.Size = new System.Drawing.Size(870, 129);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_inputPathes, 0);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_intWideLaneFiles, 0);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(611, 0);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_outputFraction);
            this.groupBox2.Controls.Add(this.checkBox_outputInt);
            this.groupBox2.Controls.Add(this.checkBox_outputSumery);
            this.groupBox2.Location = new System.Drawing.Point(98, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 44);
            this.groupBox2.TabIndex = 61;
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
            // fileOpenControl_intWideLaneFiles
            // 
            this.fileOpenControl_intWideLaneFiles.AllowDrop = true;
            this.fileOpenControl_intWideLaneFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenControl_intWideLaneFiles.FilePath = "";
            this.fileOpenControl_intWideLaneFiles.FilePathes = new string[0];
            this.fileOpenControl_intWideLaneFiles.Filter = "时段宽巷星间单差整数文件|*.WLInt.txt.xls|星间单差宽项整数文件(表格法)|*_IntOfWL.txt.xls|Excel 文本|*.txt.xls" +
    "|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_intWideLaneFiles.FirstPath = "";
            this.fileOpenControl_intWideLaneFiles.IsMultiSelect = true;
            this.fileOpenControl_intWideLaneFiles.LabelName = "BSD宽巷整数文件：";
            this.fileOpenControl_intWideLaneFiles.Location = new System.Drawing.Point(3, 69);
            this.fileOpenControl_intWideLaneFiles.Name = "fileOpenControl_intWideLaneFiles";
            this.fileOpenControl_intWideLaneFiles.Size = new System.Drawing.Size(864, 35);
            this.fileOpenControl_intWideLaneFiles.TabIndex = 61;
            // 
            // namedFloatControl1maxAllowedDiffer
            // 
            this.namedFloatControl1maxAllowedDiffer.Location = new System.Drawing.Point(9, 62);
            this.namedFloatControl1maxAllowedDiffer.Name = "namedFloatControl1maxAllowedDiffer";
            this.namedFloatControl1maxAllowedDiffer.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl1maxAllowedDiffer.TabIndex = 62;
            this.namedFloatControl1maxAllowedDiffer.Title = "允许最大偏差:";
            this.namedFloatControl1maxAllowedDiffer.Value = 0.25D;
            // 
            // checkBox1IsOutputInEachDirectory
            // 
            this.checkBox1IsOutputInEachDirectory.AutoSize = true;
            this.checkBox1IsOutputInEachDirectory.Location = new System.Drawing.Point(435, 19);
            this.checkBox1IsOutputInEachDirectory.Name = "checkBox1IsOutputInEachDirectory";
            this.checkBox1IsOutputInEachDirectory.Size = new System.Drawing.Size(204, 16);
            this.checkBox1IsOutputInEachDirectory.TabIndex = 65;
            this.checkBox1IsOutputInEachDirectory.Text = "各时段单独目录输出(限于老方法)";
            this.checkBox1IsOutputInEachDirectory.UseVisualStyleBackColor = true;
            // 
            // baseSatSelectingControl1
            // 
            this.baseSatSelectingControl1.EnableBaseSat = false;
            this.baseSatSelectingControl1.Location = new System.Drawing.Point(21, 3);
            this.baseSatSelectingControl1.Name = "baseSatSelectingControl1";
            this.baseSatSelectingControl1.Size = new System.Drawing.Size(602, 43);
            this.baseSatSelectingControl1.TabIndex = 19;
            // 
            // checkBox_calculateAllSat
            // 
            this.checkBox_calculateAllSat.AutoSize = true;
            this.checkBox_calculateAllSat.Location = new System.Drawing.Point(435, 41);
            this.checkBox_calculateAllSat.Name = "checkBox_calculateAllSat";
            this.checkBox_calculateAllSat.Size = new System.Drawing.Size(108, 16);
            this.checkBox_calculateAllSat.TabIndex = 66;
            this.checkBox_calculateAllSat.Text = "计算所有的卫星";
            this.checkBox_calculateAllSat.UseVisualStyleBackColor = true;
            // 
            // namedIntControl1MinSiteCount
            // 
            this.namedIntControl1MinSiteCount.Location = new System.Drawing.Point(327, 95);
            this.namedIntControl1MinSiteCount.Name = "namedIntControl1MinSiteCount";
            this.namedIntControl1MinSiteCount.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl1MinSiteCount.TabIndex = 67;
            this.namedIntControl1MinSiteCount.Title = "最少测站数:";
            this.namedIntControl1MinSiteCount.Value = 10;
            // 
            // namedIntControl_removeCountOfEachSegment
            // 
            this.namedIntControl_removeCountOfEachSegment.Location = new System.Drawing.Point(9, 91);
            this.namedIntControl_removeCountOfEachSegment.Name = "namedIntControl_removeCountOfEachSegment";
            this.namedIntControl_removeCountOfEachSegment.Size = new System.Drawing.Size(306, 23);
            this.namedIntControl_removeCountOfEachSegment.TabIndex = 67;
            this.namedIntControl_removeCountOfEachSegment.Title = "移除PPP浮点解，各时段起始数据数量:";
            this.namedIntControl_removeCountOfEachSegment.Value = 30;
            // 
            // namedFloatControl_maxRMSTImes
            // 
            this.namedFloatControl_maxRMSTImes.Location = new System.Drawing.Point(175, 62);
            this.namedFloatControl_maxRMSTImes.Name = "namedFloatControl_maxRMSTImes";
            this.namedFloatControl_maxRMSTImes.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_maxRMSTImes.TabIndex = 62;
            this.namedFloatControl_maxRMSTImes.Title = "粗差RMS倍数:";
            this.namedFloatControl_maxRMSTImes.Value = 3D;
            // 
            // fileOpenControl_fcbPathes
            // 
            this.fileOpenControl_fcbPathes.AllowDrop = true;
            this.fileOpenControl_fcbPathes.FilePath = "";
            this.fileOpenControl_fcbPathes.FilePathes = new string[0];
            this.fileOpenControl_fcbPathes.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_fcbPathes.FirstPath = "";
            this.fileOpenControl_fcbPathes.IsMultiSelect = true;
            this.fileOpenControl_fcbPathes.LabelName = "文件：";
            this.fileOpenControl_fcbPathes.Location = new System.Drawing.Point(0, 20);
            this.fileOpenControl_fcbPathes.Name = "fileOpenControl_fcbPathes";
            this.fileOpenControl_fcbPathes.Size = new System.Drawing.Size(539, 42);
            this.fileOpenControl_fcbPathes.TabIndex = 20;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_combine);
            this.groupBox3.Controls.Add(this.fileOpenControl_fcbPathes);
            this.groupBox3.Location = new System.Drawing.Point(5, 52);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(631, 68);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "产品汇总合成选项";
            // 
            // button_combine
            // 
            this.button_combine.Location = new System.Drawing.Point(546, 20);
            this.button_combine.Name = "button_combine";
            this.button_combine.Size = new System.Drawing.Size(75, 42);
            this.button_combine.TabIndex = 21;
            this.button_combine.Text = "合成";
            this.button_combine.UseVisualStyleBackColor = true;
            this.button_combine.Click += new System.EventHandler(this.button_combine_Click);
            // 
            // namedFloatControl_maxRms
            // 
            this.namedFloatControl_maxRms.Location = new System.Drawing.Point(327, 62);
            this.namedFloatControl_maxRms.Name = "namedFloatControl_maxRms";
            this.namedFloatControl_maxRms.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_maxRms.TabIndex = 62;
            this.namedFloatControl_maxRms.Title = "最大RMS:";
            this.namedFloatControl_maxRms.Value = 0.25D;
            // 
            // MultiPeriodNarrowLaneOfBsdSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 526);
            this.IsShowProgressBar = true;
            this.Name = "MultiPeriodNarrowLaneOfBsdSolverForm";
            this.Text = "BSD窄巷算器";
            this.Load += new System.EventHandler(this.NarrowLaneOfBsdSolverForm_Load);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_outputFraction;
        private System.Windows.Forms.CheckBox checkBox_outputInt;
        private System.Windows.Forms.CheckBox checkBox_outputSumery;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_intWideLaneFiles;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1maxAllowedDiffer;
        private System.Windows.Forms.CheckBox checkBox1IsOutputInEachDirectory;
        private Controls.BaseSatSelectingControl baseSatSelectingControl1;
        private System.Windows.Forms.CheckBox checkBox_calculateAllSat;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_removeCountOfEachSegment;
        private Geo.Winform.Controls.NamedIntControl namedIntControl1MinSiteCount;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxRMSTImes;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_combine;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_fcbPathes;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxRms;
    }
}