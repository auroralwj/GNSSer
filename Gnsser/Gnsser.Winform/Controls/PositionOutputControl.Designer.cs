namespace Gnsser.Winform.Controls
{
    partial class PositionOutputControl
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
            this.checkBox_outputResult = new System.Windows.Forms.CheckBox();
            this.checkBox_outputEpochInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_outputSiteSat = new System.Windows.Forms.CheckBox();
            this.checkBox_outputAdjust = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox_outputResult);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_outputEpochInfo);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_outputSiteSat);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_outputAdjust);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(509, 20);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // checkBox_outputResult
            // 
            this.checkBox_outputResult.AutoSize = true;
            this.checkBox_outputResult.Location = new System.Drawing.Point(3, 3);
            this.checkBox_outputResult.Name = "checkBox_outputResult";
            this.checkBox_outputResult.Size = new System.Drawing.Size(96, 16);
            this.checkBox_outputResult.TabIndex = 0;
            this.checkBox_outputResult.Text = "输出结果文件";
            this.checkBox_outputResult.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputEpochInfo
            // 
            this.checkBox_outputEpochInfo.AutoSize = true;
            this.checkBox_outputEpochInfo.Location = new System.Drawing.Point(105, 3);
            this.checkBox_outputEpochInfo.Name = "checkBox_outputEpochInfo";
            this.checkBox_outputEpochInfo.Size = new System.Drawing.Size(120, 16);
            this.checkBox_outputEpochInfo.TabIndex = 1;
            this.checkBox_outputEpochInfo.Text = "输出历元信息文件";
            this.checkBox_outputEpochInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputSiteSat
            // 
            this.checkBox_outputSiteSat.AutoSize = true;
            this.checkBox_outputSiteSat.Location = new System.Drawing.Point(231, 3);
            this.checkBox_outputSiteSat.Name = "checkBox_outputSiteSat";
            this.checkBox_outputSiteSat.Size = new System.Drawing.Size(126, 16);
            this.checkBox_outputSiteSat.TabIndex = 2;
            this.checkBox_outputSiteSat.Text = "输出测站-卫星文件";
            this.checkBox_outputSiteSat.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputAdjust
            // 
            this.checkBox_outputAdjust.AutoSize = true;
            this.checkBox_outputAdjust.Location = new System.Drawing.Point(363, 3);
            this.checkBox_outputAdjust.Name = "checkBox_outputAdjust";
            this.checkBox_outputAdjust.Size = new System.Drawing.Size(96, 16);
            this.checkBox_outputAdjust.TabIndex = 3;
            this.checkBox_outputAdjust.Text = "输出平差文件";
            this.checkBox_outputAdjust.UseVisualStyleBackColor = true;
            // 
            // PositionOutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "PositionOutputControl";
            this.Size = new System.Drawing.Size(509, 20);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox_outputResult;
        private System.Windows.Forms.CheckBox checkBox_outputEpochInfo;
        private System.Windows.Forms.CheckBox checkBox_outputSiteSat;
        private System.Windows.Forms.CheckBox checkBox_outputAdjust;
    }
}
