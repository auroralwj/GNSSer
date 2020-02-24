namespace Gnsser.Winform
{
    partial class GnssCaculatorOptionPage
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
            this.tabPage_caculator = new System.Windows.Forms.TabPage();
            this.enumRadioControl1StepOfRecursive = new Geo.Winform.EnumRadioControl();
            this.button_loadDefault = new System.Windows.Forms.Button();
            this.enumRadioControl1AdjustmentType = new Geo.Winform.EnumRadioControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_cacuWithKalmanFilter = new System.Windows.Forms.RadioButton();
            this.radioButton_cacu_noFilter = new System.Windows.Forms.RadioButton();
            this.gnssSolverSelectionControl1 = new Gnsser.Winform.GnssSolverSelectionControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.checkBox_enableAdjust = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage_caculator.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_caculator);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(653, 507);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_caculator
            // 
            this.tabPage_caculator.Controls.Add(this.checkBox_enableAdjust);
            this.tabPage_caculator.Controls.Add(this.enumRadioControl1StepOfRecursive);
            this.tabPage_caculator.Controls.Add(this.button_loadDefault);
            this.tabPage_caculator.Controls.Add(this.enumRadioControl1AdjustmentType);
            this.tabPage_caculator.Controls.Add(this.groupBox2);
            this.tabPage_caculator.Controls.Add(this.gnssSolverSelectionControl1);
            this.tabPage_caculator.Location = new System.Drawing.Point(4, 22);
            this.tabPage_caculator.Name = "tabPage_caculator";
            this.tabPage_caculator.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_caculator.Size = new System.Drawing.Size(645, 481);
            this.tabPage_caculator.TabIndex = 4;
            this.tabPage_caculator.Text = "计算器";
            this.tabPage_caculator.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl1StepOfRecursive
            // 
            this.enumRadioControl1StepOfRecursive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl1StepOfRecursive.IsReady = false;
            this.enumRadioControl1StepOfRecursive.Location = new System.Drawing.Point(13, 395);
            this.enumRadioControl1StepOfRecursive.Name = "enumRadioControl1StepOfRecursive";
            this.enumRadioControl1StepOfRecursive.Size = new System.Drawing.Size(622, 79);
            this.enumRadioControl1StepOfRecursive.TabIndex = 37;
            this.enumRadioControl1StepOfRecursive.Title = "递归最小二乘选项";
            // 
            // button_loadDefault
            // 
            this.button_loadDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_loadDefault.Location = new System.Drawing.Point(437, 132);
            this.button_loadDefault.Name = "button_loadDefault";
            this.button_loadDefault.Size = new System.Drawing.Size(188, 40);
            this.button_loadDefault.TabIndex = 35;
            this.button_loadDefault.Text = "加载计算器对应的默认配置";
            this.button_loadDefault.UseVisualStyleBackColor = true;
            this.button_loadDefault.Click += new System.EventHandler(this.button_loadDefault_Click);
            // 
            // enumRadioControl1AdjustmentType
            // 
            this.enumRadioControl1AdjustmentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl1AdjustmentType.IsReady = false;
            this.enumRadioControl1AdjustmentType.Location = new System.Drawing.Point(100, 29);
            this.enumRadioControl1AdjustmentType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl1AdjustmentType.Name = "enumRadioControl1AdjustmentType";
            this.enumRadioControl1AdjustmentType.Size = new System.Drawing.Size(536, 124);
            this.enumRadioControl1AdjustmentType.TabIndex = 36;
            this.enumRadioControl1AdjustmentType.Title = "平差器选项";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_cacuWithKalmanFilter);
            this.groupBox2.Controls.Add(this.radioButton_cacu_noFilter);
            this.groupBox2.Location = new System.Drawing.Point(7, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(87, 76);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "计算方式";
            // 
            // radioButton_cacuWithKalmanFilter
            // 
            this.radioButton_cacuWithKalmanFilter.AutoSize = true;
            this.radioButton_cacuWithKalmanFilter.Location = new System.Drawing.Point(6, 22);
            this.radioButton_cacuWithKalmanFilter.Name = "radioButton_cacuWithKalmanFilter";
            this.radioButton_cacuWithKalmanFilter.Size = new System.Drawing.Size(71, 16);
            this.radioButton_cacuWithKalmanFilter.TabIndex = 0;
            this.radioButton_cacuWithKalmanFilter.TabStop = true;
            this.radioButton_cacuWithKalmanFilter.Text = "滤波计算";
            this.radioButton_cacuWithKalmanFilter.UseVisualStyleBackColor = true;
            // 
            // radioButton_cacu_noFilter
            // 
            this.radioButton_cacu_noFilter.AutoSize = true;
            this.radioButton_cacu_noFilter.Location = new System.Drawing.Point(6, 45);
            this.radioButton_cacu_noFilter.Name = "radioButton_cacu_noFilter";
            this.radioButton_cacu_noFilter.Size = new System.Drawing.Size(71, 16);
            this.radioButton_cacu_noFilter.TabIndex = 0;
            this.radioButton_cacu_noFilter.TabStop = true;
            this.radioButton_cacu_noFilter.Text = "独立计算";
            this.radioButton_cacu_noFilter.UseVisualStyleBackColor = true;
            // 
            // gnssSolverSelectionControl1
            // 
            this.gnssSolverSelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gnssSolverSelectionControl1.CurrentdType = Gnsser.GnssSolverType.钟差网解;
            this.gnssSolverSelectionControl1.Location = new System.Drawing.Point(5, 159);
            this.gnssSolverSelectionControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gnssSolverSelectionControl1.Name = "gnssSolverSelectionControl1";
            this.gnssSolverSelectionControl1.Size = new System.Drawing.Size(629, 224);
            this.gnssSolverSelectionControl1.TabIndex = 33;
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
            // checkBox_enableAdjust
            // 
            this.checkBox_enableAdjust.AutoSize = true;
            this.checkBox_enableAdjust.ForeColor = System.Drawing.Color.Red;
            this.checkBox_enableAdjust.Location = new System.Drawing.Point(100, 6);
            this.checkBox_enableAdjust.Name = "checkBox_enableAdjust";
            this.checkBox_enableAdjust.Size = new System.Drawing.Size(72, 16);
            this.checkBox_enableAdjust.TabIndex = 38;
            this.checkBox_enableAdjust.Text = "平差计算";
            this.checkBox_enableAdjust.UseVisualStyleBackColor = true;
            this.checkBox_enableAdjust.CheckedChanged += new System.EventHandler(this.checkBox_enableAdjust_CheckedChanged);
            // 
            // GnssCaculatorOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "GnssCaculatorOptionPage";
            this.Size = new System.Drawing.Size(653, 507);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_caculator.ResumeLayout(false);
            this.tabPage_caculator.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_caculator;
        private System.Windows.Forms.RadioButton radioButton_cacu_noFilter;
        private System.Windows.Forms.RadioButton radioButton_cacuWithKalmanFilter;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private GnssSolverSelectionControl gnssSolverSelectionControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_loadDefault;
        private Geo.Winform.EnumRadioControl enumRadioControl1AdjustmentType;
        private Geo.Winform.EnumRadioControl enumRadioControl1StepOfRecursive;
        private System.Windows.Forms.CheckBox checkBox_enableAdjust;
    }
}