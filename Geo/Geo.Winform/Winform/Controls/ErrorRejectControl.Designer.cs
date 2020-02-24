namespace Geo.Winform.Controls
{
    partial class ErrorRejectControl
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_enable = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_maxLimit = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox_isRelative = new System.Windows.Forms.CheckBox();
            this.groupBox6.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.flowLayoutPanel1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(234, 76);
            this.groupBox6.TabIndex = 32;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "粗差剔除";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox_enable);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_isRelative);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(230, 58);
            this.flowLayoutPanel1.TabIndex = 31;
            // 
            // checkBox_enable
            // 
            this.checkBox_enable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enable.Location = new System.Drawing.Point(2, 2);
            this.checkBox_enable.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_enable.Name = "checkBox_enable";
            this.checkBox_enable.Size = new System.Drawing.Size(96, 22);
            this.checkBox_enable.TabIndex = 30;
            this.checkBox_enable.Text = "启用粗差剔除";
            this.checkBox_enable.UseVisualStyleBackColor = true;
            this.checkBox_enable.CheckedChanged += new System.EventHandler(this.checkBox_enable_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox_maxLimit);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Location = new System.Drawing.Point(3, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 22);
            this.panel1.TabIndex = 32;
            // 
            // textBox_maxLimit
            // 
            this.textBox_maxLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_maxLimit.Location = new System.Drawing.Point(46, 1);
            this.textBox_maxLimit.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_maxLimit.Name = "textBox_maxLimit";
            this.textBox_maxLimit.Size = new System.Drawing.Size(92, 21);
            this.textBox_maxLimit.TabIndex = 23;
            this.textBox_maxLimit.Text = "0.3";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "限差：";
            // 
            // checkBox_isRelative
            // 
            this.checkBox_isRelative.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_isRelative.Checked = true;
            this.checkBox_isRelative.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isRelative.Location = new System.Drawing.Point(148, 28);
            this.checkBox_isRelative.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_isRelative.Name = "checkBox_isRelative";
            this.checkBox_isRelative.Size = new System.Drawing.Size(72, 22);
            this.checkBox_isRelative.TabIndex = 30;
            this.checkBox_isRelative.Text = "相对误差";
            this.checkBox_isRelative.UseVisualStyleBackColor = true;
            this.checkBox_isRelative.CheckedChanged += new System.EventHandler(this.checkBox_enable_CheckedChanged);
            // 
            // ErrorRejectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ErrorRejectControl";
            this.Size = new System.Drawing.Size(234, 76);
            this.groupBox6.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_enable;
        private System.Windows.Forms.TextBox textBox_maxLimit;
        private System.Windows.Forms.CheckBox checkBox_isRelative;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}
