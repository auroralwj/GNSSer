namespace Gnsser.Winform.Controls
{
    partial class BaseSatSelectingControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox_basePrn = new System.Windows.Forms.ComboBox();
            this.bindingSource_prns = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxIsAssignBasePrn = new System.Windows.Forms.CheckBox();
            this.gnssSystemSelectControl1 = new Gnsser.Winform.Controls.GnssSystemSelectControl();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prns)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_basePrn
            // 
            this.comboBox_basePrn.DataSource = this.bindingSource_prns;
            this.comboBox_basePrn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_basePrn.FormattingEnabled = true;
            this.comboBox_basePrn.Location = new System.Drawing.Point(76, 4);
            this.comboBox_basePrn.Name = "comboBox_basePrn";
            this.comboBox_basePrn.Size = new System.Drawing.Size(69, 20);
            this.comboBox_basePrn.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "基准卫星：";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.gnssSystemSelectControl1);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(270, 87);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // checkBoxIsAssignBasePrn
            // 
            this.checkBoxIsAssignBasePrn.AutoSize = true;
            this.checkBoxIsAssignBasePrn.Checked = true;
            this.checkBoxIsAssignBasePrn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIsAssignBasePrn.Location = new System.Drawing.Point(151, 6);
            this.checkBoxIsAssignBasePrn.Name = "checkBoxIsAssignBasePrn";
            this.checkBoxIsAssignBasePrn.Size = new System.Drawing.Size(96, 16);
            this.checkBoxIsAssignBasePrn.TabIndex = 18;
            this.checkBoxIsAssignBasePrn.Text = "指定基准卫星";
            this.checkBoxIsAssignBasePrn.UseVisualStyleBackColor = true;
            this.checkBoxIsAssignBasePrn.CheckedChanged += new System.EventHandler(this.checkBoxIsAssignBasePrn_CheckedChanged);
            // 
            // gnssSystemSelectControl1
            // 
            this.gnssSystemSelectControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gnssSystemSelectControl1.Location = new System.Drawing.Point(2, 2);
            this.gnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gnssSystemSelectControl1.Name = "gnssSystemSelectControl1";
            this.gnssSystemSelectControl1.Size = new System.Drawing.Size(265, 39);
            this.gnssSystemSelectControl1.TabIndex = 0;
            this.gnssSystemSelectControl1.SatelliteTypeChanged += new Gnsser.SatelliteTypeChangedEventHandler(this.gnssSystemSelectControl1_SatelliteTypeChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.checkBoxIsAssignBasePrn);
            this.panel1.Controls.Add(this.comboBox_basePrn);
            this.panel1.Location = new System.Drawing.Point(3, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 27);
            this.panel1.TabIndex = 16;
            // 
            // BaseSatSelectingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "BaseSatSelectingControl";
            this.Size = new System.Drawing.Size(270, 87);
            this.Load += new System.EventHandler(this.BaseSatSelectingControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prns)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GnssSystemSelectControl gnssSystemSelectControl1;
        private System.Windows.Forms.ComboBox comboBox_basePrn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.BindingSource bindingSource_prns;
        private System.Windows.Forms.CheckBox checkBoxIsAssignBasePrn;
        private System.Windows.Forms.Panel panel1;
    }
}
