namespace Gnsser.Winform
{
    partial class WideLaneOfBsdSolverForm
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
            this.baseSatSelectingControl1 = new Gnsser.Winform.Controls.BaseSatSelectingControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_outputFraction = new System.Windows.Forms.CheckBox();
            this.checkBox_outputInt = new System.Windows.Forms.CheckBox();
            this.checkBox_outputSumery = new System.Windows.Forms.CheckBox();
            this.namedIntControl_minEpoch = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_minSiteCount = new Geo.Winform.Controls.NamedIntControl();
            this.enabledTimePeriodControl1 = new Geo.Winform.Controls.EnabledTimePeriodControl();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(1013, 148);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.enabledTimePeriodControl1);
            this.panel4.Controls.Add(this.namedIntControl_minEpoch);
            this.panel4.Controls.Add(this.namedIntControl_minSiteCount);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.Add(this.baseSatSelectingControl1);
            this.panel4.Size = new System.Drawing.Size(1007, 123);
            this.panel4.Controls.SetChildIndex(this.baseSatSelectingControl1, 0);
            this.panel4.Controls.SetChildIndex(this.groupBox2, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_minSiteCount, 0);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_minEpoch, 0);
            this.panel4.Controls.SetChildIndex(this.enabledTimePeriodControl1, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Filter = "WM平滑文件(*_SmoothedMw.txt.xls)|*_SmoothedMw.txt.xls|WM原始文件|*_RawMW.txt.xls;*_MwRaw." +
    "txt.xls|O文件|*.*O|RINEX压缩文件(*.*D;*.*D.Z)|*.*D.Z;*.*D|所有文件|*.*";
            this.fileOpenControl_inputPathes.LabelName = "MW平滑文件：";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(942, 101);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(948, 129);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(754, 0);
            // 
            // baseSatSelectingControl1
            // 
            this.baseSatSelectingControl1.EnableBaseSat = false;
            this.baseSatSelectingControl1.Location = new System.Drawing.Point(98, 3);
            this.baseSatSelectingControl1.Name = "baseSatSelectingControl1";
            this.baseSatSelectingControl1.Size = new System.Drawing.Size(279, 85);
            this.baseSatSelectingControl1.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_outputFraction);
            this.groupBox2.Controls.Add(this.checkBox_outputInt);
            this.groupBox2.Controls.Add(this.checkBox_outputSumery);
            this.groupBox2.Location = new System.Drawing.Point(658, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(111, 102);
            this.groupBox2.TabIndex = 60;
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
            this.checkBox_outputInt.Location = new System.Drawing.Point(13, 42);
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
            this.checkBox_outputSumery.Location = new System.Drawing.Point(13, 64);
            this.checkBox_outputSumery.Name = "checkBox_outputSumery";
            this.checkBox_outputSumery.Size = new System.Drawing.Size(72, 16);
            this.checkBox_outputSumery.TabIndex = 57;
            this.checkBox_outputSumery.Text = "输出汇总";
            this.checkBox_outputSumery.UseVisualStyleBackColor = true;
            // 
            // namedIntControl_minEpoch
            // 
            this.namedIntControl_minEpoch.Location = new System.Drawing.Point(154, 83);
            this.namedIntControl_minEpoch.Name = "namedIntControl_minEpoch";
            this.namedIntControl_minEpoch.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_minEpoch.TabIndex = 64;
            this.namedIntControl_minEpoch.Title = "最小历元数：";
            this.namedIntControl_minEpoch.Value = 5;
            // 
            // namedIntControl_minSiteCount
            // 
            this.namedIntControl_minSiteCount.Location = new System.Drawing.Point(17, 83);
            this.namedIntControl_minSiteCount.Name = "namedIntControl_minSiteCount";
            this.namedIntControl_minSiteCount.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_minSiteCount.TabIndex = 65;
            this.namedIntControl_minSiteCount.Title = "最小测站数：";
            this.namedIntControl_minSiteCount.Value = 3;
            // 
            // enabledTimePeriodControl1
            // 
            this.enabledTimePeriodControl1.From = new System.DateTime(2018, 9, 9, 20, 34, 21, 921);
            this.enabledTimePeriodControl1.Location = new System.Drawing.Point(369, 12);
            this.enabledTimePeriodControl1.Name = "enabledTimePeriodControl1";
            this.enabledTimePeriodControl1.Size = new System.Drawing.Size(277, 86);
            this.enabledTimePeriodControl1.TabIndex = 66;
            this.enabledTimePeriodControl1.Title = "从：";
            this.enabledTimePeriodControl1.To = new System.DateTime(2018, 9, 10, 20, 34, 21, 921);
            // 
            // WideLaneOfBsdSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 405);
            this.IsShowProgressBar = true;
            this.Name = "WideLaneOfBsdSolverForm";
            this.Text = "BSD宽巷计算器";
            this.Load += new System.EventHandler(this.WideLaneSolverForm_Load);
            this.panel4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.BaseSatSelectingControl baseSatSelectingControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_outputFraction;
        private System.Windows.Forms.CheckBox checkBox_outputInt;
        private System.Windows.Forms.CheckBox checkBox_outputSumery;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_minEpoch;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_minSiteCount;
        private Geo.Winform.Controls.EnabledTimePeriodControl enabledTimePeriodControl1;


    }
}