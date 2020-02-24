namespace Geo.Draw
{
    partial class TimePeriodChartControl
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制图像CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制图像CToolStripMenuItem,
            this.重置RToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(141, 48);
            // 
            // 复制图像CToolStripMenuItem
            // 
            this.复制图像CToolStripMenuItem.Name = "复制图像CToolStripMenuItem";
            this.复制图像CToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.复制图像CToolStripMenuItem.Text = "复制图像(&C)";
            this.复制图像CToolStripMenuItem.Click += new System.EventHandler(this.复制图像CToolStripMenuItem_Click);
            // 
            // 重置RToolStripMenuItem
            // 
            this.重置RToolStripMenuItem.Name = "重置RToolStripMenuItem";
            this.重置RToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.重置RToolStripMenuItem.Text = "重置(&R)";
            this.重置RToolStripMenuItem.Click += new System.EventHandler(this.重置RToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(531, 352);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.ChartControl_SizeChanged);
            // 
            // TimePeriodChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Name = "TimePeriodChartControl";
            this.Size = new System.Drawing.Size(531, 352);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制图像CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重置RToolStripMenuItem;
    }
}
