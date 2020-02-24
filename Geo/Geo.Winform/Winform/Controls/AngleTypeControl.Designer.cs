namespace Geo.Winform.Controls
{
    partial class AngleTypeControl
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
            this.checkBox_isDeg = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton_dms_s = new System.Windows.Forms.RadioButton();
            this.radioButton_degree = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_isDeg
            // 
            this.checkBox_isDeg.AutoSize = true;
            this.checkBox_isDeg.Checked = true;
            this.checkBox_isDeg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isDeg.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox_isDeg.Location = new System.Drawing.Point(0, 0);
            this.checkBox_isDeg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_isDeg.Name = "checkBox_isDeg";
            this.checkBox_isDeg.Size = new System.Drawing.Size(105, 25);
            this.checkBox_isDeg.TabIndex = 0;
            this.checkBox_isDeg.Text = "角度[弧度]";
            this.checkBox_isDeg.UseVisualStyleBackColor = true;
            this.checkBox_isDeg.CheckedChanged += new System.EventHandler(this.checkBox_isRad_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton_dms_s);
            this.panel1.Controls.Add(this.radioButton_degree);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(105, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 25);
            this.panel1.TabIndex = 2;
            // 
            // radioButton_dms_s
            // 
            this.radioButton_dms_s.AutoSize = true;
            this.radioButton_dms_s.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButton_dms_s.Location = new System.Drawing.Point(73, 0);
            this.radioButton_dms_s.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_dms_s.Name = "radioButton_dms_s";
            this.radioButton_dms_s.Size = new System.Drawing.Size(96, 25);
            this.radioButton_dms_s.TabIndex = 3;
            this.radioButton_dms_s.TabStop = true;
            this.radioButton_dms_s.Text = "度分秒.秒";
            this.radioButton_dms_s.UseVisualStyleBackColor = true;
            // 
            // radioButton_degree
            // 
            this.radioButton_degree.AutoSize = true;
            this.radioButton_degree.Checked = true;
            this.radioButton_degree.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButton_degree.Location = new System.Drawing.Point(0, 0);
            this.radioButton_degree.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_degree.Name = "radioButton_degree";
            this.radioButton_degree.Size = new System.Drawing.Size(73, 25);
            this.radioButton_degree.TabIndex = 2;
            this.radioButton_degree.TabStop = true;
            this.radioButton_degree.Text = "度小数";
            this.radioButton_degree.UseVisualStyleBackColor = true;
            // 
            // AngleTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBox_isDeg);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AngleTypeControl";
            this.Size = new System.Drawing.Size(277, 25);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_isDeg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_dms_s;
        private System.Windows.Forms.RadioButton radioButton_degree;
    }
}
