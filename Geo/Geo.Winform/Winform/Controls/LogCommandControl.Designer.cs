namespace Geo.Winform.Controls
{
    partial class LogCommandControl
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1_enableShowInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_showError = new System.Windows.Forms.CheckBox();
            this.checkBox_showWarn = new System.Windows.Forms.CheckBox();
            this.checkBox_debugModel = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox1_enableShowInfo);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_showError);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_showWarn);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_debugModel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(287, 61);
            this.flowLayoutPanel1.TabIndex = 45;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // checkBox1_enableShowInfo
            // 
            this.checkBox1_enableShowInfo.AutoSize = true;
            this.checkBox1_enableShowInfo.Checked = true;
            this.checkBox1_enableShowInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1_enableShowInfo.Location = new System.Drawing.Point(3, 3);
            this.checkBox1_enableShowInfo.Name = "checkBox1_enableShowInfo";
            this.checkBox1_enableShowInfo.Size = new System.Drawing.Size(72, 16);
            this.checkBox1_enableShowInfo.TabIndex = 1;
            this.checkBox1_enableShowInfo.Text = "显示信息";
            this.checkBox1_enableShowInfo.UseVisualStyleBackColor = true;
            this.checkBox1_enableShowInfo.CheckedChanged += new System.EventHandler(this.checkBox1_enableShowInfo_CheckedChanged);
            // 
            // checkBox_showError
            // 
            this.checkBox_showError.AutoSize = true;
            this.checkBox_showError.Location = new System.Drawing.Point(81, 3);
            this.checkBox_showError.Name = "checkBox_showError";
            this.checkBox_showError.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showError.TabIndex = 2;
            this.checkBox_showError.Text = "显示错误";
            this.checkBox_showError.UseVisualStyleBackColor = true;
            this.checkBox_showError.CheckedChanged += new System.EventHandler(this.checkBox_showError_CheckedChanged);
            // 
            // checkBox_showWarn
            // 
            this.checkBox_showWarn.AutoSize = true;
            this.checkBox_showWarn.Location = new System.Drawing.Point(159, 3);
            this.checkBox_showWarn.Name = "checkBox_showWarn";
            this.checkBox_showWarn.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showWarn.TabIndex = 2;
            this.checkBox_showWarn.Text = "显示警告";
            this.checkBox_showWarn.UseVisualStyleBackColor = true;
            this.checkBox_showWarn.CheckedChanged += new System.EventHandler(this.checkBox_showWarn_CheckedChanged);
            // 
            // checkBox_debugModel
            // 
            this.checkBox_debugModel.AutoSize = true;
            this.checkBox_debugModel.Location = new System.Drawing.Point(3, 25);
            this.checkBox_debugModel.Name = "checkBox_debugModel";
            this.checkBox_debugModel.Size = new System.Drawing.Size(72, 16);
            this.checkBox_debugModel.TabIndex = 2;
            this.checkBox_debugModel.Text = "启用调试";
            this.checkBox_debugModel.UseVisualStyleBackColor = true;
            this.checkBox_debugModel.CheckedChanged += new System.EventHandler(this.checkBox_debugModel_CheckedChanged);
            // 
            // LogCommandControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "LogCommandControl";
            this.Size = new System.Drawing.Size(287, 61);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        protected System.Windows.Forms.CheckBox checkBox1_enableShowInfo;
        private System.Windows.Forms.CheckBox checkBox_showError;
        private System.Windows.Forms.CheckBox checkBox_showWarn;
        private System.Windows.Forms.CheckBox checkBox_debugModel;
    }
}
