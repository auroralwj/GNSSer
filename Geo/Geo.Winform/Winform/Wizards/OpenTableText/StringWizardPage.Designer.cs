namespace Geo.Winform.Wizards
{
    partial class StringWizardPage
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
            this.namedStringControl1 = new Geo.Winform.Controls.NamedStringControl();
            this.label_notice = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_notice);
            this.groupBox1.Controls.Add(this.namedStringControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 303);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // namedStringControl1
            // 
            this.namedStringControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl1.Location = new System.Drawing.Point(53, 99);
            this.namedStringControl1.Name = "namedStringControl1";
            this.namedStringControl1.Size = new System.Drawing.Size(382, 23);
            this.namedStringControl1.TabIndex = 0;
            this.namedStringControl1.Title = "名称：";
            // 
            // label_notice
            // 
            this.label_notice.AutoSize = true;
            this.label_notice.Location = new System.Drawing.Point(53, 32);
            this.label_notice.Name = "label_notice";
            this.label_notice.Size = new System.Drawing.Size(89, 12);
            this.label_notice.TabIndex = 1;
            this.label_notice.Text = "提示：请输入。";
            // 
            // StringWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "StringWizardPage";
            this.Size = new System.Drawing.Size(532, 303);
            this.Load += new System.EventHandler(this.SelectFilePageControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.NamedStringControl namedStringControl1;
        private System.Windows.Forms.Label label_notice;
    }
}
