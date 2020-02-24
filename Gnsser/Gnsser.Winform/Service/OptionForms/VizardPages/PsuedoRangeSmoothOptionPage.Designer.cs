namespace Gnsser.Winform
{
    partial class PsuedoRangeSmoothOptionPage
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.namedIntControl_OrderOfDeltaIonoPolyFit = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_IonoFitEpochCount = new Geo.Winform.Controls.NamedIntControl();
            this.enumRadioControl_ionDifferType = new Geo.Winform.EnumRadioControl();
            this.checkBox_IsUseGNSSerSmoothRangeMethod = new System.Windows.Forms.CheckBox();
            this.enumRadioControl_smoothSuposType = new Geo.Winform.EnumRadioControl();
            this.checkBox_IsWeightedPhaseSmoothRange = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSmoothingRange = new System.Windows.Forms.CheckBox();
            this.enumRadioControl_SmoothRangeType = new Geo.Winform.EnumRadioControl();
            this.namedIntControl_bufferSize = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_WindowSizeOfPhaseSmoothRange = new Geo.Winform.Controls.NamedIntControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.fileOpenControl_ionoDelta = new Geo.Winform.Controls.FileOpenControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.checkBox_IsSmoothingRange);
            this.tabPage1.Controls.Add(this.enumRadioControl_SmoothRangeType);
            this.tabPage1.Controls.Add(this.namedIntControl_bufferSize);
            this.tabPage1.Controls.Add(this.namedIntControl_WindowSizeOfPhaseSmoothRange);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(694, 500);
            this.tabPage1.TabIndex = 13;
            this.tabPage1.Text = "伪距平滑";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkBox_IsUseGNSSerSmoothRangeMethod);
            this.groupBox1.Controls.Add(this.enumRadioControl_smoothSuposType);
            this.groupBox1.Controls.Add(this.checkBox_IsWeightedPhaseSmoothRange);
            this.groupBox1.Location = new System.Drawing.Point(6, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(685, 350);
            this.groupBox1.TabIndex = 75;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "载波相位平滑伪距";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.fileOpenControl_ionoDelta);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.enumRadioControl_ionDifferType);
            this.groupBox2.Location = new System.Drawing.Point(-1, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(680, 165);
            this.groupBox2.TabIndex = 75;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电离层变化改正选项(单频)";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.namedIntControl_OrderOfDeltaIonoPolyFit);
            this.groupBox3.Controls.Add(this.namedIntControl_IonoFitEpochCount);
            this.groupBox3.Location = new System.Drawing.Point(445, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(229, 81);
            this.groupBox3.TabIndex = 77;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "电离层变化拟合选项";
            // 
            // namedIntControl_OrderOfDeltaIonoPolyFit
            // 
            this.namedIntControl_OrderOfDeltaIonoPolyFit.Location = new System.Drawing.Point(19, 20);
            this.namedIntControl_OrderOfDeltaIonoPolyFit.Name = "namedIntControl_OrderOfDeltaIonoPolyFit";
            this.namedIntControl_OrderOfDeltaIonoPolyFit.Size = new System.Drawing.Size(120, 23);
            this.namedIntControl_OrderOfDeltaIonoPolyFit.TabIndex = 74;
            this.namedIntControl_OrderOfDeltaIonoPolyFit.Title = "拟合阶次：";
            this.namedIntControl_OrderOfDeltaIonoPolyFit.Value = 1;
            // 
            // namedIntControl_IonoFitEpochCount
            // 
            this.namedIntControl_IonoFitEpochCount.Location = new System.Drawing.Point(6, 50);
            this.namedIntControl_IonoFitEpochCount.Name = "namedIntControl_IonoFitEpochCount";
            this.namedIntControl_IonoFitEpochCount.Size = new System.Drawing.Size(133, 23);
            this.namedIntControl_IonoFitEpochCount.TabIndex = 74;
            this.namedIntControl_IonoFitEpochCount.Title = "拟合历元数：";
            this.namedIntControl_IonoFitEpochCount.Value = 1;
            // 
            // enumRadioControl_ionDifferType
            // 
            this.enumRadioControl_ionDifferType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.enumRadioControl_ionDifferType.ForeColor = System.Drawing.Color.DarkOrange;
            this.enumRadioControl_ionDifferType.Location = new System.Drawing.Point(13, 20);
            this.enumRadioControl_ionDifferType.Name = "enumRadioControl_ionDifferType";
            this.enumRadioControl_ionDifferType.Size = new System.Drawing.Size(359, 100);
            this.enumRadioControl_ionDifferType.TabIndex = 76;
            this.enumRadioControl_ionDifferType.Title = "电离层变化改正类型";
            // 
            // checkBox_IsUseGNSSerSmoothRangeMethod
            // 
            this.checkBox_IsUseGNSSerSmoothRangeMethod.AutoSize = true;
            this.checkBox_IsUseGNSSerSmoothRangeMethod.Location = new System.Drawing.Point(6, 27);
            this.checkBox_IsUseGNSSerSmoothRangeMethod.Name = "checkBox_IsUseGNSSerSmoothRangeMethod";
            this.checkBox_IsUseGNSSerSmoothRangeMethod.Size = new System.Drawing.Size(222, 16);
            this.checkBox_IsUseGNSSerSmoothRangeMethod.TabIndex = 48;
            this.checkBox_IsUseGNSSerSmoothRangeMethod.Text = "GNSSer改进算法，否则原始Hatch滤波";
            this.checkBox_IsUseGNSSerSmoothRangeMethod.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_smoothSuposType
            // 
            this.enumRadioControl_smoothSuposType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_smoothSuposType.Location = new System.Drawing.Point(6, 95);
            this.enumRadioControl_smoothSuposType.Name = "enumRadioControl_smoothSuposType";
            this.enumRadioControl_smoothSuposType.Size = new System.Drawing.Size(678, 53);
            this.enumRadioControl_smoothSuposType.TabIndex = 75;
            this.enumRadioControl_smoothSuposType.Title = "GNSSer 滑动窗口选项(改进算法)";
            // 
            // checkBox_IsWeightedPhaseSmoothRange
            // 
            this.checkBox_IsWeightedPhaseSmoothRange.AutoSize = true;
            this.checkBox_IsWeightedPhaseSmoothRange.Location = new System.Drawing.Point(6, 60);
            this.checkBox_IsWeightedPhaseSmoothRange.Name = "checkBox_IsWeightedPhaseSmoothRange";
            this.checkBox_IsWeightedPhaseSmoothRange.Size = new System.Drawing.Size(174, 16);
            this.checkBox_IsWeightedPhaseSmoothRange.TabIndex = 48;
            this.checkBox_IsWeightedPhaseSmoothRange.Text = "加权非推估(原始Hatch有效)";
            this.checkBox_IsWeightedPhaseSmoothRange.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSmoothingRange
            // 
            this.checkBox_IsSmoothingRange.AutoSize = true;
            this.checkBox_IsSmoothingRange.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkBox_IsSmoothingRange.Location = new System.Drawing.Point(6, 9);
            this.checkBox_IsSmoothingRange.Name = "checkBox_IsSmoothingRange";
            this.checkBox_IsSmoothingRange.Size = new System.Drawing.Size(132, 16);
            this.checkBox_IsSmoothingRange.TabIndex = 48;
            this.checkBox_IsSmoothingRange.Text = "平滑伪距改正总开关";
            this.checkBox_IsSmoothingRange.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_SmoothRangeType
            // 
            this.enumRadioControl_SmoothRangeType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_SmoothRangeType.Location = new System.Drawing.Point(10, 33);
            this.enumRadioControl_SmoothRangeType.Name = "enumRadioControl_SmoothRangeType";
            this.enumRadioControl_SmoothRangeType.Size = new System.Drawing.Size(669, 53);
            this.enumRadioControl_SmoothRangeType.TabIndex = 75;
            this.enumRadioControl_SmoothRangeType.Title = "伪距平滑方法";
            // 
            // namedIntControl_bufferSize
            // 
            this.namedIntControl_bufferSize.Location = new System.Drawing.Point(231, 101);
            this.namedIntControl_bufferSize.Name = "namedIntControl_bufferSize";
            this.namedIntControl_bufferSize.Size = new System.Drawing.Size(224, 23);
            this.namedIntControl_bufferSize.TabIndex = 74;
            this.namedIntControl_bufferSize.Title = "缓存大小(推荐窗口一半)：";
            this.namedIntControl_bufferSize.Value = 0;
            // 
            // namedIntControl_WindowSizeOfPhaseSmoothRange
            // 
            this.namedIntControl_WindowSizeOfPhaseSmoothRange.Location = new System.Drawing.Point(3, 101);
            this.namedIntControl_WindowSizeOfPhaseSmoothRange.Name = "namedIntControl_WindowSizeOfPhaseSmoothRange";
            this.namedIntControl_WindowSizeOfPhaseSmoothRange.Size = new System.Drawing.Size(188, 23);
            this.namedIntControl_WindowSizeOfPhaseSmoothRange.TabIndex = 74;
            this.namedIntControl_WindowSizeOfPhaseSmoothRange.Title = "平滑窗口(权)大小：";
            this.namedIntControl_WindowSizeOfPhaseSmoothRange.Value = 0;
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
            // fileOpenControl_ionoDelta
            // 
            this.fileOpenControl_ionoDelta.AllowDrop = true;
            this.fileOpenControl_ionoDelta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_ionoDelta.FilePath = "";
            this.fileOpenControl_ionoDelta.FilePathes = new string[0];
            this.fileOpenControl_ionoDelta.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_ionoDelta.FirstPath = "";
            this.fileOpenControl_ionoDelta.IsMultiSelect = false;
            this.fileOpenControl_ionoDelta.LabelName = "电离层变化率文件：";
            this.fileOpenControl_ionoDelta.Location = new System.Drawing.Point(7, 137);
            this.fileOpenControl_ionoDelta.Name = "fileOpenControl_ionoDelta";
            this.fileOpenControl_ionoDelta.Size = new System.Drawing.Size(641, 22);
            this.fileOpenControl_ionoDelta.TabIndex = 78;
            // 
            // PsuedoRangeSmoothOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "PsuedoRangeSmoothOptionPage";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_OrderOfDeltaIonoPolyFit;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_IonoFitEpochCount;
        private Geo.Winform.EnumRadioControl enumRadioControl_ionDifferType;
        private System.Windows.Forms.CheckBox checkBox_IsUseGNSSerSmoothRangeMethod;
        private Geo.Winform.EnumRadioControl enumRadioControl_smoothSuposType;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_WindowSizeOfPhaseSmoothRange;
        private System.Windows.Forms.CheckBox checkBox_IsWeightedPhaseSmoothRange;
        private System.Windows.Forms.CheckBox checkBox_IsSmoothingRange;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_bufferSize;
        private Geo.Winform.EnumRadioControl enumRadioControl_SmoothRangeType;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_ionoDelta;
    }
}