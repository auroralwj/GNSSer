namespace Geo.Utils
{
    partial class OperationTypeSelectForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.enumRadioControl1 = new Geo.Winform.EnumRadioControl();
            this.okbutton1 = new System.Windows.Forms.Button();
            this.cancelbutton2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // enumRadioControl1
            // 
            this.enumRadioControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl1.Location = new System.Drawing.Point(12, 12);
            this.enumRadioControl1.Name = "enumRadioControl1";
            this.enumRadioControl1.Size = new System.Drawing.Size(319, 91);
            this.enumRadioControl1.TabIndex = 3;
            this.enumRadioControl1.Title = "操作";
            // 
            // okbutton1
            // 
            this.okbutton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbutton1.Location = new System.Drawing.Point(171, 115);
            this.okbutton1.Name = "okbutton1";
            this.okbutton1.Size = new System.Drawing.Size(75, 29);
            this.okbutton1.TabIndex = 1;
            this.okbutton1.Text = "确定";
            this.okbutton1.UseVisualStyleBackColor = true;
            this.okbutton1.Click += new System.EventHandler(this.okbutton1_Click);
            // 
            // cancelbutton2
            // 
            this.cancelbutton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelbutton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton2.Location = new System.Drawing.Point(256, 115);
            this.cancelbutton2.Name = "cancelbutton2";
            this.cancelbutton2.Size = new System.Drawing.Size(75, 29);
            this.cancelbutton2.TabIndex = 2;
            this.cancelbutton2.Text = "取消";
            this.cancelbutton2.UseVisualStyleBackColor = true;
            // 
            // OperationTypeSelectForm
            // 
            this.AcceptButton = this.okbutton1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelbutton2;
            this.ClientSize = new System.Drawing.Size(346, 151);
            this.Controls.Add(this.enumRadioControl1);
            this.Controls.Add(this.cancelbutton2);
            this.Controls.Add(this.okbutton1);
            this.Name = "OperationTypeSelectForm";
            this.ShowIcon = false;
            this.Text = "请选择";
            this.Load += new System.EventHandler(this.SelectingColForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button okbutton1;
        private System.Windows.Forms.Button cancelbutton2;
        private Winform.EnumRadioControl enumRadioControl1;
    }
}