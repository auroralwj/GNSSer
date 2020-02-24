namespace Gnsser.Winform.Controls
{
    partial class GnssSystemSelectControl
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton_beidou = new System.Windows.Forms.RadioButton();
            this.radioButton_gps = new System.Windows.Forms.RadioButton();
            this.radioButton_galileo = new System.Windows.Forms.RadioButton();
            this.radioButton_glonass = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(93, 98);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "系统";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radioButton_beidou);
            this.flowLayoutPanel1.Controls.Add(this.radioButton_gps);
            this.flowLayoutPanel1.Controls.Add(this.radioButton_galileo);
            this.flowLayoutPanel1.Controls.Add(this.radioButton_glonass);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 16);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(89, 80);
            this.flowLayoutPanel1.TabIndex = 39;
            // 
            // radioButton_beidou
            // 
            this.radioButton_beidou.AutoSize = true;
            this.radioButton_beidou.Checked = true;
            this.radioButton_beidou.Location = new System.Drawing.Point(2, 2);
            this.radioButton_beidou.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_beidou.Name = "radioButton_beidou";
            this.radioButton_beidou.Size = new System.Drawing.Size(59, 16);
            this.radioButton_beidou.TabIndex = 41;
            this.radioButton_beidou.TabStop = true;
            this.radioButton_beidou.Text = "Beidou";
            this.radioButton_beidou.UseVisualStyleBackColor = true;
            this.radioButton_beidou.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton_gps
            // 
            this.radioButton_gps.AutoSize = true;
            this.radioButton_gps.Location = new System.Drawing.Point(2, 22);
            this.radioButton_gps.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_gps.Name = "radioButton_gps";
            this.radioButton_gps.Size = new System.Drawing.Size(41, 16);
            this.radioButton_gps.TabIndex = 42;
            this.radioButton_gps.Text = "GPS";
            this.radioButton_gps.UseVisualStyleBackColor = true;
            this.radioButton_gps.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton_galileo
            // 
            this.radioButton_galileo.AutoSize = true;
            this.radioButton_galileo.Location = new System.Drawing.Point(2, 42);
            this.radioButton_galileo.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_galileo.Name = "radioButton_galileo";
            this.radioButton_galileo.Size = new System.Drawing.Size(65, 16);
            this.radioButton_galileo.TabIndex = 43;
            this.radioButton_galileo.Text = "Galileo";
            this.radioButton_galileo.UseVisualStyleBackColor = true;
            this.radioButton_galileo.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton_glonass
            // 
            this.radioButton_glonass.AutoSize = true;
            this.radioButton_glonass.Location = new System.Drawing.Point(2, 62);
            this.radioButton_glonass.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_glonass.Name = "radioButton_glonass";
            this.radioButton_glonass.Size = new System.Drawing.Size(65, 16);
            this.radioButton_glonass.TabIndex = 44;
            this.radioButton_glonass.Text = "GLONASS";
            this.radioButton_glonass.UseVisualStyleBackColor = true;
            this.radioButton_glonass.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // GnssSystemSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GnssSystemSelectControl";
            this.Size = new System.Drawing.Size(93, 98);
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton_beidou;
        private System.Windows.Forms.RadioButton radioButton_gps;
        private System.Windows.Forms.RadioButton radioButton_galileo;
        private System.Windows.Forms.RadioButton radioButton_glonass;
    }
}
