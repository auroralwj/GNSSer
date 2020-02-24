namespace Gnsser.Winform.Controls
{
    partial class MultiSolverOptionControl
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
            this.enumRadioControl_solverType = new Geo.Winform.EnumRadioControl();
            this.button_optSetting = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // enumRadioControl_solverType
            // 
            this.enumRadioControl_solverType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumRadioControl_solverType.IsReady = false;
            this.enumRadioControl_solverType.Location = new System.Drawing.Point(0, 0);
            this.enumRadioControl_solverType.Name = "enumRadioControl_solverType";
            this.enumRadioControl_solverType.Size = new System.Drawing.Size(402, 89);
            this.enumRadioControl_solverType.TabIndex = 0;
            this.enumRadioControl_solverType.Title = "GNSS 算法";
            // 
            // button_optSetting
            // 
            this.button_optSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_optSetting.Location = new System.Drawing.Point(324, 60);
            this.button_optSetting.Name = "button_optSetting";
            this.button_optSetting.Size = new System.Drawing.Size(75, 26);
            this.button_optSetting.TabIndex = 1;
            this.button_optSetting.Text = "设置";
            this.button_optSetting.UseVisualStyleBackColor = true;
            this.button_optSetting.Click += new System.EventHandler(this.button_optSetting_Click);
            // 
            // button_reset
            // 
            this.button_reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_reset.Location = new System.Drawing.Point(243, 60);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 26);
            this.button_reset.TabIndex = 1;
            this.button_reset.Text = "重置";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // MultiSolverOptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.button_optSetting);
            this.Controls.Add(this.enumRadioControl_solverType);
            this.Name = "MultiSolverOptionControl";
            this.Size = new System.Drawing.Size(402, 89);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.EnumRadioControl enumRadioControl_solverType;
        private System.Windows.Forms.Button button_optSetting;
        private System.Windows.Forms.Button button_reset;
    }
}
