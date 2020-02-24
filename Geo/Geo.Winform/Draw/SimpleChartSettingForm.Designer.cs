namespace Geo
{
    partial class SimpleChartSettingForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.enabledStringControl_xTitle = new Geo.Winform.Controls.EnabledStringControl();
            this.enabledStringControl_yTitle = new Geo.Winform.Controls.EnabledStringControl();
            this.label_fontOfTitleXY = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.enabledStringControl_xAxieFormat = new Geo.Winform.Controls.EnabledStringControl();
            this.enabledStringControl_aixesYFormat = new Geo.Winform.Controls.EnabledStringControl();
            this.checkBox_YIsStartedFromZero = new System.Windows.Forms.CheckBox();
            this.checkBox_XIsStartedFromZero = new System.Windows.Forms.CheckBox();
            this.checkBox_isShowYGrid = new System.Windows.Forms.CheckBox();
            this.enabledFloatSpanControl_xValueSpan = new Geo.Winform.Controls.EnabledFloatSpanControl();
            this.checkBox_showGridOfX = new System.Windows.Forms.CheckBox();
            this.enabledFloatSpanControl_YValuespan = new Geo.Winform.Controls.EnabledFloatSpanControl();
            this.enabledFloatControl_yInterval = new Geo.Winform.Controls.EnabledFloatControl();
            this.enabledFloatControl_xInterval = new Geo.Winform.Controls.EnabledFloatControl();
            this.button_Apply = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(331, 405);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 26);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(250, 405);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 26);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 400);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(411, 374);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "坐标轴设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.enabledStringControl_xTitle);
            this.groupBox7.Controls.Add(this.enabledStringControl_yTitle);
            this.groupBox7.Controls.Add(this.label_fontOfTitleXY);
            this.groupBox7.Location = new System.Drawing.Point(20, 10);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(380, 105);
            this.groupBox7.TabIndex = 37;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "坐标轴名称";
            this.groupBox7.Enter += new System.EventHandler(this.groupBox7_Enter);
            // 
            // enabledStringControl_xTitle
            // 
            this.enabledStringControl_xTitle.Location = new System.Drawing.Point(5, 20);
            this.enabledStringControl_xTitle.Name = "enabledStringControl_xTitle";
            this.enabledStringControl_xTitle.Size = new System.Drawing.Size(319, 23);
            this.enabledStringControl_xTitle.TabIndex = 0;
            this.enabledStringControl_xTitle.Title = "X轴名称：";
            // 
            // enabledStringControl_yTitle
            // 
            this.enabledStringControl_yTitle.Location = new System.Drawing.Point(5, 48);
            this.enabledStringControl_yTitle.Name = "enabledStringControl_yTitle";
            this.enabledStringControl_yTitle.Size = new System.Drawing.Size(319, 23);
            this.enabledStringControl_yTitle.TabIndex = 0;
            this.enabledStringControl_yTitle.Title = "Y轴名称：";
            // 
            // label_fontOfTitleXY
            // 
            this.label_fontOfTitleXY.AutoSize = true;
            this.label_fontOfTitleXY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_fontOfTitleXY.Location = new System.Drawing.Point(69, 79);
            this.label_fontOfTitleXY.Name = "label_fontOfTitleXY";
            this.label_fontOfTitleXY.Size = new System.Drawing.Size(133, 14);
            this.label_fontOfTitleXY.TabIndex = 1;
            this.label_fontOfTitleXY.Text = "XY 轴标题的字体和颜色";
            this.label_fontOfTitleXY.Click += new System.EventHandler(this.label_fontOfTitle_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.enabledStringControl_xAxieFormat);
            this.groupBox4.Controls.Add(this.enabledStringControl_aixesYFormat);
            this.groupBox4.Controls.Add(this.checkBox_YIsStartedFromZero);
            this.groupBox4.Controls.Add(this.checkBox_XIsStartedFromZero);
            this.groupBox4.Controls.Add(this.checkBox_isShowYGrid);
            this.groupBox4.Controls.Add(this.enabledFloatSpanControl_xValueSpan);
            this.groupBox4.Controls.Add(this.checkBox_showGridOfX);
            this.groupBox4.Controls.Add(this.enabledFloatSpanControl_YValuespan);
            this.groupBox4.Controls.Add(this.enabledFloatControl_yInterval);
            this.groupBox4.Controls.Add(this.enabledFloatControl_xInterval);
            this.groupBox4.Location = new System.Drawing.Point(19, 115);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(381, 244);
            this.groupBox4.TabIndex = 36;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "坐标范围";
            // 
            // enabledStringControl_xAxieFormat
            // 
            this.enabledStringControl_xAxieFormat.Location = new System.Drawing.Point(4, 144);
            this.enabledStringControl_xAxieFormat.Name = "enabledStringControl_xAxieFormat";
            this.enabledStringControl_xAxieFormat.Size = new System.Drawing.Size(319, 23);
            this.enabledStringControl_xAxieFormat.TabIndex = 0;
            this.enabledStringControl_xAxieFormat.Title = "X轴文本格式：";
            // 
            // enabledStringControl_aixesYFormat
            // 
            this.enabledStringControl_aixesYFormat.Location = new System.Drawing.Point(4, 172);
            this.enabledStringControl_aixesYFormat.Name = "enabledStringControl_aixesYFormat";
            this.enabledStringControl_aixesYFormat.Size = new System.Drawing.Size(319, 23);
            this.enabledStringControl_aixesYFormat.TabIndex = 0;
            this.enabledStringControl_aixesYFormat.Title = "Y轴文本格式：";
            // 
            // checkBox_YIsStartedFromZero
            // 
            this.checkBox_YIsStartedFromZero.AutoSize = true;
            this.checkBox_YIsStartedFromZero.Location = new System.Drawing.Point(94, 216);
            this.checkBox_YIsStartedFromZero.Name = "checkBox_YIsStartedFromZero";
            this.checkBox_YIsStartedFromZero.Size = new System.Drawing.Size(78, 16);
            this.checkBox_YIsStartedFromZero.TabIndex = 5;
            this.checkBox_YIsStartedFromZero.Text = "Y 轴从0起";
            this.checkBox_YIsStartedFromZero.UseVisualStyleBackColor = true;
            // 
            // checkBox_XIsStartedFromZero
            // 
            this.checkBox_XIsStartedFromZero.AutoSize = true;
            this.checkBox_XIsStartedFromZero.Location = new System.Drawing.Point(8, 215);
            this.checkBox_XIsStartedFromZero.Name = "checkBox_XIsStartedFromZero";
            this.checkBox_XIsStartedFromZero.Size = new System.Drawing.Size(78, 16);
            this.checkBox_XIsStartedFromZero.TabIndex = 5;
            this.checkBox_XIsStartedFromZero.Text = "X 轴从0起";
            this.checkBox_XIsStartedFromZero.UseVisualStyleBackColor = true;
            // 
            // checkBox_isShowYGrid
            // 
            this.checkBox_isShowYGrid.AutoSize = true;
            this.checkBox_isShowYGrid.Location = new System.Drawing.Point(251, 216);
            this.checkBox_isShowYGrid.Name = "checkBox_isShowYGrid";
            this.checkBox_isShowYGrid.Size = new System.Drawing.Size(72, 16);
            this.checkBox_isShowYGrid.TabIndex = 0;
            this.checkBox_isShowYGrid.Text = "Y 轴格网";
            this.checkBox_isShowYGrid.UseVisualStyleBackColor = true;
            // 
            // enabledFloatSpanControl_xValueSpan
            // 
            this.enabledFloatSpanControl_xValueSpan.From = 0.1D;
            this.enabledFloatSpanControl_xValueSpan.Location = new System.Drawing.Point(13, 20);
            this.enabledFloatSpanControl_xValueSpan.Name = "enabledFloatSpanControl_xValueSpan";
            this.enabledFloatSpanControl_xValueSpan.Size = new System.Drawing.Size(307, 24);
            this.enabledFloatSpanControl_xValueSpan.TabIndex = 3;
            this.enabledFloatSpanControl_xValueSpan.Title = "X轴 范围：";
            this.enabledFloatSpanControl_xValueSpan.To = 0.1D;
            // 
            // checkBox_showGridOfX
            // 
            this.checkBox_showGridOfX.AutoSize = true;
            this.checkBox_showGridOfX.Location = new System.Drawing.Point(177, 215);
            this.checkBox_showGridOfX.Name = "checkBox_showGridOfX";
            this.checkBox_showGridOfX.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showGridOfX.TabIndex = 0;
            this.checkBox_showGridOfX.Text = "X 轴格网";
            this.checkBox_showGridOfX.UseVisualStyleBackColor = true;
            // 
            // enabledFloatSpanControl_YValuespan
            // 
            this.enabledFloatSpanControl_YValuespan.From = 0.1D;
            this.enabledFloatSpanControl_YValuespan.Location = new System.Drawing.Point(13, 85);
            this.enabledFloatSpanControl_YValuespan.Name = "enabledFloatSpanControl_YValuespan";
            this.enabledFloatSpanControl_YValuespan.Size = new System.Drawing.Size(307, 24);
            this.enabledFloatSpanControl_YValuespan.TabIndex = 3;
            this.enabledFloatSpanControl_YValuespan.Title = "Y轴 范围：";
            this.enabledFloatSpanControl_YValuespan.To = 0.1D;
            // 
            // enabledFloatControl_yInterval
            // 
            this.enabledFloatControl_yInterval.Location = new System.Drawing.Point(13, 115);
            this.enabledFloatControl_yInterval.Name = "enabledFloatControl_yInterval";
            this.enabledFloatControl_yInterval.Size = new System.Drawing.Size(187, 23);
            this.enabledFloatControl_yInterval.TabIndex = 4;
            this.enabledFloatControl_yInterval.Title = "Y轴 间距：";
            this.enabledFloatControl_yInterval.Value = 0.1D;
            // 
            // enabledFloatControl_xInterval
            // 
            this.enabledFloatControl_xInterval.Location = new System.Drawing.Point(13, 50);
            this.enabledFloatControl_xInterval.Name = "enabledFloatControl_xInterval";
            this.enabledFloatControl_xInterval.Size = new System.Drawing.Size(187, 23);
            this.enabledFloatControl_xInterval.TabIndex = 4;
            this.enabledFloatControl_xInterval.Title = "X轴 间距：";
            this.enabledFloatControl_xInterval.Value = 0.1D;
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(148, 405);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(75, 26);
            this.button_Apply.TabIndex = 0;
            this.button_Apply.Text = "应用";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // SimpleChartSettingForm
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(418, 440);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Name = "SimpleChartSettingForm";
            this.Text = "绘图设置";
            this.Load += new System.EventHandler(this.ChartSettingForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Winform.Controls.EnabledStringControl enabledStringControl_yTitle;
        private Winform.Controls.EnabledStringControl enabledStringControl_xTitle;
        private System.Windows.Forms.Label label_fontOfTitleXY;
        private System.Windows.Forms.CheckBox checkBox_showGridOfX;
        private Winform.Controls.EnabledFloatSpanControl enabledFloatSpanControl_YValuespan;
        private Winform.Controls.EnabledFloatControl enabledFloatControl_yInterval;
        private System.Windows.Forms.CheckBox checkBox_isShowYGrid;
        private Winform.Controls.EnabledFloatControl enabledFloatControl_xInterval;
        private Winform.Controls.EnabledFloatSpanControl enabledFloatSpanControl_xValueSpan;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox_YIsStartedFromZero;
        private System.Windows.Forms.CheckBox checkBox_XIsStartedFromZero;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private Winform.Controls.EnabledStringControl enabledStringControl_xAxieFormat;
        private Winform.Controls.EnabledStringControl enabledStringControl_aixesYFormat;
    }
}