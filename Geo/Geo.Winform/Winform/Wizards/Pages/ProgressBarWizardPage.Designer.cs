namespace Geo.Winform.Wizards
{
    partial class ProgressBarWizardPage
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
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.SuspendLayout();
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(27, 87);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(468, 34);
            this.progressBarComponent1.TabIndex = 0;
            // 
            // ProgressBarWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBarComponent1);
            this.Name = "ProgressBarWizardPage";
            this.Size = new System.Drawing.Size(532, 303);
            this.Load += new System.EventHandler(this.SelectFilePageControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ProgressBarComponent progressBarComponent1;

    }
}
