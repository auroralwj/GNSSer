namespace Geo.Winform.Controls
{
    partial class ParallelConfigControl
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
            this.panel_parallelism = new System.Windows.Forms.Panel();
            this.textBox_parallelism = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_processorCount = new System.Windows.Forms.Label();
            this.checkBox_parallel = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel_parallelism.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_parallelism
            // 
            this.panel_parallelism.Controls.Add(this.textBox_parallelism);
            this.panel_parallelism.Controls.Add(this.label6);
            this.panel_parallelism.Location = new System.Drawing.Point(74, 2);
            this.panel_parallelism.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel_parallelism.Name = "panel_parallelism";
            this.panel_parallelism.Size = new System.Drawing.Size(82, 25);
            this.panel_parallelism.TabIndex = 40;
            // 
            // textBox_parallelism
            // 
            this.textBox_parallelism.Location = new System.Drawing.Point(52, 2);
            this.textBox_parallelism.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_parallelism.Name = "textBox_parallelism";
            this.textBox_parallelism.Size = new System.Drawing.Size(24, 21);
            this.textBox_parallelism.TabIndex = 41;
            this.textBox_parallelism.Text = "4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "本机核数:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 40;
            this.label6.Text = "并行度:";
            // 
            // label_processorCount
            // 
            this.label_processorCount.AutoSize = true;
            this.label_processorCount.Location = new System.Drawing.Point(66, 6);
            this.label_processorCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_processorCount.Name = "label_processorCount";
            this.label_processorCount.Size = new System.Drawing.Size(17, 12);
            this.label_processorCount.TabIndex = 44;
            this.label_processorCount.Text = "12";
            // 
            // checkBox_parallel
            // 
            this.checkBox_parallel.AutoSize = true;
            this.checkBox_parallel.Checked = true;
            this.checkBox_parallel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_parallel.Location = new System.Drawing.Point(2, 6);
            this.checkBox_parallel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_parallel.Name = "checkBox_parallel";
            this.checkBox_parallel.Size = new System.Drawing.Size(72, 16);
            this.checkBox_parallel.TabIndex = 39;
            this.checkBox_parallel.Text = "多核并行";
            this.checkBox_parallel.UseVisualStyleBackColor = true;
            this.checkBox_parallel.CheckedChanged += new System.EventHandler(this.checkBox_parallel_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel_parallelism);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(256, 28);
            this.flowLayoutPanel1.TabIndex = 41;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox_parallel);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(68, 25);
            this.panel1.TabIndex = 41;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label_processorCount);
            this.panel2.Location = new System.Drawing.Point(161, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(88, 23);
            this.panel2.TabIndex = 42;
            // 
            // ParallelConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ParallelConfigControl";
            this.Size = new System.Drawing.Size(256, 28);
            this.panel_parallelism.ResumeLayout(false);
            this.panel_parallelism.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_parallelism;
        private System.Windows.Forms.TextBox textBox_parallelism;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox_parallel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_processorCount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
