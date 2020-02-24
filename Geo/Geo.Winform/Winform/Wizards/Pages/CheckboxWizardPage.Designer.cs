namespace Geo.Winform.Wizards
{
    partial class CheckboxWizardPage
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.enumCheckBoxControl1 = new Geo.Winform.EnumCheckBoxControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.enumCheckBoxControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 303);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件输入";
            // 
            // enumCheckBoxControl1
            // 
            this.enumCheckBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumCheckBoxControl1.Location = new System.Drawing.Point(3, 17);
            this.enumCheckBoxControl1.Name = "enumCheckBoxControl1";
            this.enumCheckBoxControl1.Size = new System.Drawing.Size(526, 283);
            this.enumCheckBoxControl1.TabIndex = 0;
            this.enumCheckBoxControl1.Title = "选项";
            // 
            // CheckboxWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "CheckboxWizardPage";
            this.Size = new System.Drawing.Size(532, 303);
            this.Load += new System.EventHandler(this.SelectFilePageControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private EnumCheckBoxControl enumCheckBoxControl1;
    }
}
