namespace Gnsser.Winform
{
    partial class AccuracyEvaluationOptionPage
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_stream = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.namedIntControl_labelCharCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedFloatControl_maxAllowConvergTime = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_maxDiffer = new Geo.Winform.Controls.NamedFloatControl();
            this.namedIntControl_epochCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedFloatControl1MaxAllowedDifferAfterConvergence = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_maxAllowedRms = new Geo.Winform.Controls.NamedFloatControl();
            this.enabledStringControl_analysisParam = new Geo.Winform.Controls.EnabledStringControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage_stream.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_stream);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(671, 473);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_stream
            // 
            this.tabPage_stream.Controls.Add(this.groupBox7);
            this.tabPage_stream.Location = new System.Drawing.Point(4, 22);
            this.tabPage_stream.Name = "tabPage_stream";
            this.tabPage_stream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_stream.Size = new System.Drawing.Size(663, 447);
            this.tabPage_stream.TabIndex = 12;
            this.tabPage_stream.Text = "静态精度评估";
            this.tabPage_stream.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.namedIntControl_labelCharCount);
            this.groupBox7.Controls.Add(this.namedFloatControl_maxAllowConvergTime);
            this.groupBox7.Controls.Add(this.namedFloatControl_maxDiffer);
            this.groupBox7.Controls.Add(this.namedIntControl_epochCount);
            this.groupBox7.Controls.Add(this.namedFloatControl1MaxAllowedDifferAfterConvergence);
            this.groupBox7.Controls.Add(this.namedFloatControl_maxAllowedRms);
            this.groupBox7.Controls.Add(this.enabledStringControl_analysisParam);
            this.groupBox7.Location = new System.Drawing.Point(5, 14);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox7.Size = new System.Drawing.Size(595, 348);
            this.groupBox7.TabIndex = 76;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "历元结果分析";
            // 
            // namedIntControl_labelCharCount
            // 
            this.namedIntControl_labelCharCount.Location = new System.Drawing.Point(29, 55);
            this.namedIntControl_labelCharCount.Margin = new System.Windows.Forms.Padding(4);
            this.namedIntControl_labelCharCount.Name = "namedIntControl_labelCharCount";
            this.namedIntControl_labelCharCount.Size = new System.Drawing.Size(165, 23);
            this.namedIntControl_labelCharCount.TabIndex = 73;
            this.namedIntControl_labelCharCount.Title = "显示字符数量：";
            this.namedIntControl_labelCharCount.Value = 4;
            // 
            // namedFloatControl_maxAllowConvergTime
            // 
            this.namedFloatControl_maxAllowConvergTime.Location = new System.Drawing.Point(26, 117);
            this.namedFloatControl_maxAllowConvergTime.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_maxAllowConvergTime.Name = "namedFloatControl_maxAllowConvergTime";
            this.namedFloatControl_maxAllowConvergTime.Size = new System.Drawing.Size(197, 23);
            this.namedFloatControl_maxAllowConvergTime.TabIndex = 75;
            this.namedFloatControl_maxAllowConvergTime.Title = "最大允许收敛时间(分)：";
            this.namedFloatControl_maxAllowConvergTime.Value = 240D;
            // 
            // namedFloatControl_maxDiffer
            // 
            this.namedFloatControl_maxDiffer.Location = new System.Drawing.Point(77, 148);
            this.namedFloatControl_maxDiffer.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_maxDiffer.Name = "namedFloatControl_maxDiffer";
            this.namedFloatControl_maxDiffer.Size = new System.Drawing.Size(146, 23);
            this.namedFloatControl_maxDiffer.TabIndex = 76;
            this.namedFloatControl_maxDiffer.Title = "最大允许偏差：";
            this.namedFloatControl_maxDiffer.Value = 0.1D;
            // 
            // namedIntControl_epochCount
            // 
            this.namedIntControl_epochCount.Location = new System.Drawing.Point(29, 86);
            this.namedIntControl_epochCount.Margin = new System.Windows.Forms.Padding(4);
            this.namedIntControl_epochCount.Name = "namedIntControl_epochCount";
            this.namedIntControl_epochCount.Size = new System.Drawing.Size(165, 23);
            this.namedIntControl_epochCount.TabIndex = 74;
            this.namedIntControl_epochCount.Title = "比较历元数量：";
            this.namedIntControl_epochCount.Value = 20;
            // 
            // namedFloatControl1MaxAllowedDifferAfterConvergence
            // 
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Location = new System.Drawing.Point(40, 210);
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Name = "namedFloatControl1MaxAllowedDifferAfterConvergence";
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Size = new System.Drawing.Size(183, 23);
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.TabIndex = 77;
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Title = "收敛后允许最大偏差：";
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Value = 0.25D;
            // 
            // namedFloatControl_maxAllowedRms
            // 
            this.namedFloatControl_maxAllowedRms.Location = new System.Drawing.Point(83, 179);
            this.namedFloatControl_maxAllowedRms.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_maxAllowedRms.Name = "namedFloatControl_maxAllowedRms";
            this.namedFloatControl_maxAllowedRms.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_maxAllowedRms.TabIndex = 78;
            this.namedFloatControl_maxAllowedRms.Title = "最大允许RMS：";
            this.namedFloatControl_maxAllowedRms.Value = 0.05D;
            // 
            // enabledStringControl_analysisParam
            // 
            this.enabledStringControl_analysisParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enabledStringControl_analysisParam.Location = new System.Drawing.Point(17, 24);
            this.enabledStringControl_analysisParam.Name = "enabledStringControl_analysisParam";
            this.enabledStringControl_analysisParam.Size = new System.Drawing.Size(558, 23);
            this.enabledStringControl_analysisParam.TabIndex = 72;
            this.enabledStringControl_analysisParam.Title = "待分析参数名称：";
            // 
            // openFileDialog_nav
            // 
            this.openFileDialog_nav.FileName = "星历文件";
            this.openFileDialog_nav.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件,压缩文件（*.*O;*.*D;*.*D.Z;|*.*o;*.*D.Z;*.*D|所有文件|*.*";
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "钟差文件";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk;*.clk_30s|所有文件|*.*";
            // 
            // AccuracyEvaluationOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AccuracyEvaluationOptionPage";
            this.Size = new System.Drawing.Size(671, 473);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_stream.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_stream;
        private System.Windows.Forms.GroupBox groupBox7;
        private Geo.Winform.Controls.EnabledStringControl enabledStringControl_analysisParam;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_labelCharCount;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxAllowConvergTime;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxDiffer;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_epochCount;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1MaxAllowedDifferAfterConvergence;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxAllowedRms;
    }
}