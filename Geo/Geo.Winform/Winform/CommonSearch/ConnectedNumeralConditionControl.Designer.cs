namespace Geo.Winform
{
    partial class ConnectedNumeralConditionControl
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBox_conect = new System.Windows.Forms.ComboBox();
            this.numeralConditionControl1 = new Geo.Winform.NumeralConditionControl();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.comboBox_conect);
            this.flowLayoutPanel1.Controls.Add(this.numeralConditionControl1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(554, 33);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // comboBox_conect
            // 
            this.comboBox_conect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_conect.FormattingEnabled = true;
            this.comboBox_conect.Items.AddRange(new object[] {
            "且",
            "或"});
            this.comboBox_conect.Location = new System.Drawing.Point(3, 3);
            this.comboBox_conect.Name = "comboBox_conect";
            this.comboBox_conect.Size = new System.Drawing.Size(61, 20);
            this.comboBox_conect.TabIndex = 0;
            // 
            // numeralConditionControl1
            // 
            this.numeralConditionControl1.Location = new System.Drawing.Point(70, 3);
            this.numeralConditionControl1.Name = "numeralConditionControl1";
            this.numeralConditionControl1.Size = new System.Drawing.Size(473, 27);
            this.numeralConditionControl1.TabIndex = 1;
            // 
            // ConnectedNumeralConditionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ConnectedNumeralConditionControl";
            this.Size = new System.Drawing.Size(554, 33);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBox_conect;
        private NumeralConditionControl numeralConditionControl1;
    }
}
