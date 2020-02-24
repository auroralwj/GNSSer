namespace Geo.Winform.Controls
{
    partial class EnabledBoolControl
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
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.checkBox_value = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBox_enabled.Location = new System.Drawing.Point(294, 0);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 23);
            this.checkBox_enabled.TabIndex = 0;
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            this.checkBox_enabled.CheckedChanged += new System.EventHandler(this.checkBox_enabled_CheckedChanged);
            // 
            // checkBox_value
            // 
            this.checkBox_value.AutoSize = true;
            this.checkBox_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_value.Location = new System.Drawing.Point(0, 0);
            this.checkBox_value.Name = "checkBox_value";
            this.checkBox_value.Size = new System.Drawing.Size(294, 23);
            this.checkBox_value.TabIndex = 2;
            this.checkBox_value.Text = "名称";
            this.checkBox_value.UseVisualStyleBackColor = true;
            // 
            // EnabledBoolControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_value);
            this.Controls.Add(this.checkBox_enabled);
            this.Name = "EnabledBoolControl";
            this.Size = new System.Drawing.Size(342, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.CheckBox checkBox_value;
    }
}
