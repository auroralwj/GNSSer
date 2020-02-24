namespace Geo.Winform.Controls
{
    partial class AdjustVectorRenderControl
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
            this.textBox_differ = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_startParamIndex = new System.Windows.Forms.TextBox();
            this.textBox_paramCount = new System.Windows.Forms.TextBox();
            this.textBox_endEpoch = new System.Windows.Forms.TextBox();
            this.textBox_startEpoch = new System.Windows.Forms.TextBox();
            this.adjustVectorTypeControl1 = new Geo.Winform.Controls.AdjustVectorTypeControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.adjustVectorTypeControl1);
            this.groupBox1.Controls.Add(this.textBox_differ);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.textBox_startParamIndex);
            this.groupBox1.Controls.Add(this.textBox_paramCount);
            this.groupBox1.Controls.Add(this.textBox_endEpoch);
            this.groupBox1.Controls.Add(this.textBox_startEpoch);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(436, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "平差结果绘图选项";
            // 
            // textBox_differ
            // 
            this.textBox_differ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_differ.Location = new System.Drawing.Point(87, 55);
            this.textBox_differ.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_differ.Name = "textBox_differ";
            this.textBox_differ.Size = new System.Drawing.Size(345, 21);
            this.textBox_differ.TabIndex = 38;
            this.textBox_differ.Text = "0, 0, 0, 0, 0, 0, 0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 57);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 37;
            this.label15.Text = "参数中心值：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 86);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "起始参数：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(324, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "个数：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "截止：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 85);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 36;
            this.label14.Text = "起始历元：";
            // 
            // textBox_startParamIndex
            // 
            this.textBox_startParamIndex.Location = new System.Drawing.Point(290, 82);
            this.textBox_startParamIndex.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_startParamIndex.Name = "textBox_startParamIndex";
            this.textBox_startParamIndex.Size = new System.Drawing.Size(30, 21);
            this.textBox_startParamIndex.TabIndex = 35;
            this.textBox_startParamIndex.Text = "0";
            // 
            // textBox_paramCount
            // 
            this.textBox_paramCount.Location = new System.Drawing.Point(369, 82);
            this.textBox_paramCount.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_paramCount.Name = "textBox_paramCount";
            this.textBox_paramCount.Size = new System.Drawing.Size(37, 21);
            this.textBox_paramCount.TabIndex = 35;
            this.textBox_paramCount.Text = "3";
            // 
            // textBox_endEpoch
            // 
            this.textBox_endEpoch.Location = new System.Drawing.Point(169, 82);
            this.textBox_endEpoch.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_endEpoch.Name = "textBox_endEpoch";
            this.textBox_endEpoch.Size = new System.Drawing.Size(48, 21);
            this.textBox_endEpoch.TabIndex = 35;
            this.textBox_endEpoch.Text = "100000";
            // 
            // textBox_startEpoch
            // 
            this.textBox_startEpoch.Location = new System.Drawing.Point(86, 82);
            this.textBox_startEpoch.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_startEpoch.Name = "textBox_startEpoch";
            this.textBox_startEpoch.Size = new System.Drawing.Size(32, 21);
            this.textBox_startEpoch.TabIndex = 35;
            this.textBox_startEpoch.Text = "0";
            // 
            // adjustVectorTypeControl1
            // 
            this.adjustVectorTypeControl1.CurrentdType = Geo.Algorithm.Adjust.AdjustParamVectorType.参数估计值;
            this.adjustVectorTypeControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.adjustVectorTypeControl1.Location = new System.Drawing.Point(2, 16);
            this.adjustVectorTypeControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.adjustVectorTypeControl1.Name = "adjustVectorTypeControl1";
            this.adjustVectorTypeControl1.Size = new System.Drawing.Size(432, 39);
            this.adjustVectorTypeControl1.TabIndex = 39;
            // 
            // AdjustVectorRenderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AdjustVectorRenderControl";
            this.Size = new System.Drawing.Size(436, 108);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_differ;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_endEpoch;
        private System.Windows.Forms.TextBox textBox_startEpoch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_paramCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_startParamIndex;
        private AdjustVectorTypeControl adjustVectorTypeControl1;
    }
}
