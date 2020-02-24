namespace Gnsser.Winform
{
    partial class SatelliteOptionPage
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
            this.tabPage_sat = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.enabledStringControl_RemoveSats = new Geo.Winform.Controls.EnabledStringControl();
            this.textBox_MinSuccesiveEphemerisCount = new System.Windows.Forms.TextBox();
            this.enabledStringControl_IndicatedPrn = new Geo.Winform.Controls.EnabledStringControl();
            this.checkBox_IsSwitchWhenEphemerisNull = new System.Windows.Forms.CheckBox();
            this.checkBox_IsDisableEclipsedSat = new System.Windows.Forms.CheckBox();
            this.textBox_angleCut = new System.Windows.Forms.TextBox();
            this.checkBox_IsExcludeMalfunctioningSat = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_IsRemoveOrDisableNotPassedSat = new System.Windows.Forms.CheckBox();
            this.checkBox_IsEphemerisRequired = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.namedIntControl_ephInterOrder = new Geo.Winform.Controls.NamedIntControl();
            this.checkBox_IsConnectIgsDailyProduct = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage_sat.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_sat);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_sat
            // 
            this.tabPage_sat.Controls.Add(this.groupBox12);
            this.tabPage_sat.Location = new System.Drawing.Point(4, 22);
            this.tabPage_sat.Name = "tabPage_sat";
            this.tabPage_sat.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_sat.Size = new System.Drawing.Size(694, 500);
            this.tabPage_sat.TabIndex = 6;
            this.tabPage_sat.Text = "卫星";
            this.tabPage_sat.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.namedIntControl_ephInterOrder);
            this.groupBox12.Controls.Add(this.enabledStringControl_RemoveSats);
            this.groupBox12.Controls.Add(this.textBox_MinSuccesiveEphemerisCount);
            this.groupBox12.Controls.Add(this.enabledStringControl_IndicatedPrn);
            this.groupBox12.Controls.Add(this.checkBox_IsConnectIgsDailyProduct);
            this.groupBox12.Controls.Add(this.checkBox_IsSwitchWhenEphemerisNull);
            this.groupBox12.Controls.Add(this.checkBox_IsDisableEclipsedSat);
            this.groupBox12.Controls.Add(this.textBox_angleCut);
            this.groupBox12.Controls.Add(this.checkBox_IsExcludeMalfunctioningSat);
            this.groupBox12.Controls.Add(this.label1);
            this.groupBox12.Controls.Add(this.checkBox_IsRemoveOrDisableNotPassedSat);
            this.groupBox12.Controls.Add(this.checkBox_IsEphemerisRequired);
            this.groupBox12.Controls.Add(this.label20);
            this.groupBox12.Location = new System.Drawing.Point(3, 6);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(685, 336);
            this.groupBox12.TabIndex = 47;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "星历设置";
            // 
            // enabledStringControl_RemoveSats
            // 
            this.enabledStringControl_RemoveSats.Location = new System.Drawing.Point(32, 274);
            this.enabledStringControl_RemoveSats.Margin = new System.Windows.Forms.Padding(4);
            this.enabledStringControl_RemoveSats.Name = "enabledStringControl_RemoveSats";
            this.enabledStringControl_RemoveSats.Size = new System.Drawing.Size(459, 23);
            this.enabledStringControl_RemoveSats.TabIndex = 50;
            this.enabledStringControl_RemoveSats.Title = "移除卫星(以“,”间隔)：";
            // 
            // textBox_MinSuccesiveEphemerisCount
            // 
            this.textBox_MinSuccesiveEphemerisCount.Location = new System.Drawing.Point(174, 183);
            this.textBox_MinSuccesiveEphemerisCount.Name = "textBox_MinSuccesiveEphemerisCount";
            this.textBox_MinSuccesiveEphemerisCount.Size = new System.Drawing.Size(72, 21);
            this.textBox_MinSuccesiveEphemerisCount.TabIndex = 48;
            this.textBox_MinSuccesiveEphemerisCount.Text = "8";
            // 
            // enabledStringControl_IndicatedPrn
            // 
            this.enabledStringControl_IndicatedPrn.Location = new System.Drawing.Point(85, 241);
            this.enabledStringControl_IndicatedPrn.Margin = new System.Windows.Forms.Padding(4);
            this.enabledStringControl_IndicatedPrn.Name = "enabledStringControl_IndicatedPrn";
            this.enabledStringControl_IndicatedPrn.Size = new System.Drawing.Size(208, 23);
            this.enabledStringControl_IndicatedPrn.TabIndex = 48;
            this.enabledStringControl_IndicatedPrn.Title = "指定基准卫星：";
            // 
            // checkBox_IsSwitchWhenEphemerisNull
            // 
            this.checkBox_IsSwitchWhenEphemerisNull.AutoSize = true;
            this.checkBox_IsSwitchWhenEphemerisNull.Location = new System.Drawing.Point(23, 127);
            this.checkBox_IsSwitchWhenEphemerisNull.Name = "checkBox_IsSwitchWhenEphemerisNull";
            this.checkBox_IsSwitchWhenEphemerisNull.Size = new System.Drawing.Size(204, 16);
            this.checkBox_IsSwitchWhenEphemerisNull.TabIndex = 47;
            this.checkBox_IsSwitchWhenEphemerisNull.Text = "当得到NULL后是否切换星历数据源";
            this.checkBox_IsSwitchWhenEphemerisNull.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsDisableEclipsedSat
            // 
            this.checkBox_IsDisableEclipsedSat.AutoSize = true;
            this.checkBox_IsDisableEclipsedSat.Location = new System.Drawing.Point(23, 51);
            this.checkBox_IsDisableEclipsedSat.Name = "checkBox_IsDisableEclipsedSat";
            this.checkBox_IsDisableEclipsedSat.Size = new System.Drawing.Size(156, 16);
            this.checkBox_IsDisableEclipsedSat.TabIndex = 0;
            this.checkBox_IsDisableEclipsedSat.Text = "禁用太阳阴影影响的卫星";
            this.checkBox_IsDisableEclipsedSat.UseVisualStyleBackColor = true;
            // 
            // textBox_angleCut
            // 
            this.textBox_angleCut.Location = new System.Drawing.Point(174, 210);
            this.textBox_angleCut.Name = "textBox_angleCut";
            this.textBox_angleCut.Size = new System.Drawing.Size(72, 21);
            this.textBox_angleCut.TabIndex = 2;
            this.textBox_angleCut.Text = "15.0";
            // 
            // checkBox_IsExcludeMalfunctioningSat
            // 
            this.checkBox_IsExcludeMalfunctioningSat.AutoSize = true;
            this.checkBox_IsExcludeMalfunctioningSat.Location = new System.Drawing.Point(23, 101);
            this.checkBox_IsExcludeMalfunctioningSat.Name = "checkBox_IsExcludeMalfunctioningSat";
            this.checkBox_IsExcludeMalfunctioningSat.Size = new System.Drawing.Size(204, 16);
            this.checkBox_IsExcludeMalfunctioningSat.TabIndex = 46;
            this.checkBox_IsExcludeMalfunctioningSat.Text = "是否移除故障卫星(外部文件指定)";
            this.checkBox_IsExcludeMalfunctioningSat.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "高度截止角(度)：";
            // 
            // checkBox_IsRemoveOrDisableNotPassedSat
            // 
            this.checkBox_IsRemoveOrDisableNotPassedSat.AutoSize = true;
            this.checkBox_IsRemoveOrDisableNotPassedSat.Location = new System.Drawing.Point(23, 76);
            this.checkBox_IsRemoveOrDisableNotPassedSat.Name = "checkBox_IsRemoveOrDisableNotPassedSat";
            this.checkBox_IsRemoveOrDisableNotPassedSat.Size = new System.Drawing.Size(228, 16);
            this.checkBox_IsRemoveOrDisableNotPassedSat.TabIndex = 46;
            this.checkBox_IsRemoveOrDisableNotPassedSat.Text = "删除(True)或不启用未通过检核的卫星";
            this.checkBox_IsRemoveOrDisableNotPassedSat.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsEphemerisRequired
            // 
            this.checkBox_IsEphemerisRequired.AutoSize = true;
            this.checkBox_IsEphemerisRequired.Location = new System.Drawing.Point(23, 29);
            this.checkBox_IsEphemerisRequired.Name = "checkBox_IsEphemerisRequired";
            this.checkBox_IsEphemerisRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsEphemerisRequired.TabIndex = 46;
            this.checkBox_IsEphemerisRequired.Text = "是否需要星历";
            this.checkBox_IsEphemerisRequired.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(43, 186);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(125, 12);
            this.label20.TabIndex = 1;
            this.label20.Text = "星历拟合最小连续数：";
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
            // namedIntControl_ephInterOrder
            // 
            this.namedIntControl_ephInterOrder.Location = new System.Drawing.Point(273, 183);
            this.namedIntControl_ephInterOrder.Name = "namedIntControl_ephInterOrder";
            this.namedIntControl_ephInterOrder.Size = new System.Drawing.Size(166, 23);
            this.namedIntControl_ephInterOrder.TabIndex = 51;
            this.namedIntControl_ephInterOrder.Title = "星历插值阶次：";
            this.namedIntControl_ephInterOrder.Value = 0;
            // 
            // checkBox_IsConnectIgsDailyProduct
            // 
            this.checkBox_IsConnectIgsDailyProduct.AutoSize = true;
            this.checkBox_IsConnectIgsDailyProduct.Location = new System.Drawing.Point(23, 149);
            this.checkBox_IsConnectIgsDailyProduct.Name = "checkBox_IsConnectIgsDailyProduct";
            this.checkBox_IsConnectIgsDailyProduct.Size = new System.Drawing.Size(378, 16);
            this.checkBox_IsConnectIgsDailyProduct.TabIndex = 47;
            this.checkBox_IsConnectIgsDailyProduct.Text = "拼接IGS产品（可以提供隔日连续计算，但是星历框架可能不一致）";
            this.checkBox_IsConnectIgsDailyProduct.UseVisualStyleBackColor = true;
            // 
            // SatelliteOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SatelliteOptionPage";
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_sat.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox textBox_angleCut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_IsEphemerisRequired;
        private System.Windows.Forms.CheckBox checkBox_IsRemoveOrDisableNotPassedSat;
        private System.Windows.Forms.CheckBox checkBox_IsExcludeMalfunctioningSat;
        private System.Windows.Forms.CheckBox checkBox_IsDisableEclipsedSat;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.TabPage tabPage_sat;
        private System.Windows.Forms.CheckBox checkBox_IsSwitchWhenEphemerisNull;
        private System.Windows.Forms.TextBox textBox_MinSuccesiveEphemerisCount;
        private System.Windows.Forms.Label label20;
        private Geo.Winform.Controls.EnabledStringControl enabledStringControl_IndicatedPrn;
        private Geo.Winform.Controls.EnabledStringControl enabledStringControl_RemoveSats;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_ephInterOrder;
        private System.Windows.Forms.CheckBox checkBox_IsConnectIgsDailyProduct;
    }
}