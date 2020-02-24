namespace Geo.Winform.Controls
{
    partial class RmsedXyzControl
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox_enable = new System.Windows.Forms.CheckBox();
            this.textBox_rms = new System.Windows.Forms.TextBox();
            this.textBox_xyz = new System.Windows.Forms.TextBox();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.checkBox_enable);
            this.groupBox6.Controls.Add(this.textBox_rms);
            this.groupBox6.Controls.Add(this.textBox_xyz);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox6.Size = new System.Drawing.Size(498, 82);
            this.groupBox6.TabIndex = 32;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "坐标";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(11, 51);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 15);
            this.label17.TabIndex = 5;
            this.label17.Text = "RMS:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 23);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 15);
            this.label10.TabIndex = 5;
            this.label10.Text = "XYZ:";
            // 
            // checkBox_enable
            // 
            this.checkBox_enable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enable.AutoSize = true;
            this.checkBox_enable.Location = new System.Drawing.Point(400, 22);
            this.checkBox_enable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_enable.Name = "checkBox_enable";
            this.checkBox_enable.Size = new System.Drawing.Size(89, 19);
            this.checkBox_enable.TabIndex = 30;
            this.checkBox_enable.Text = "启用坐标";
            this.checkBox_enable.UseVisualStyleBackColor = true;
            this.checkBox_enable.CheckedChanged += new System.EventHandler(this.checkBox_enable_CheckedChanged);
            // 
            // textBox_rms
            // 
            this.textBox_rms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_rms.Location = new System.Drawing.Point(57, 48);
            this.textBox_rms.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_rms.Name = "textBox_rms";
            this.textBox_rms.Size = new System.Drawing.Size(334, 25);
            this.textBox_rms.TabIndex = 23;
            this.textBox_rms.Text = "10, 10, 10";
            // 
            // textBox_xyz
            // 
            this.textBox_xyz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_xyz.Location = new System.Drawing.Point(57, 20);
            this.textBox_xyz.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_xyz.Name = "textBox_xyz";
            this.textBox_xyz.Size = new System.Drawing.Size(334, 25);
            this.textBox_xyz.TabIndex = 23;
            this.textBox_xyz.Text = "0, 0, 0";
            // 
            // RmsedXyzControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);
            this.Name = "RmsedXyzControl";
            this.Size = new System.Drawing.Size(498, 82);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_enable;
        private System.Windows.Forms.TextBox textBox_rms;
        private System.Windows.Forms.TextBox textBox_xyz;
    }
}
