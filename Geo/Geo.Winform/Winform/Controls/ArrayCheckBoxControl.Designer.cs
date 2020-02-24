namespace Geo.Winform
{
    partial class ArrayCheckBoxControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.全选AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部清除CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.反选RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选择关键字SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.清除关键字KToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(240, 59);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "枚举项目";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全选AToolStripMenuItem,
            this.全部清除CToolStripMenuItem,
            this.反选RToolStripMenuItem,
            this.toolStripSeparator1,
            this.选择关键字SToolStripMenuItem,
            this.清除关键字KToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 142);
            // 
            // 全选AToolStripMenuItem
            // 
            this.全选AToolStripMenuItem.Name = "全选AToolStripMenuItem";
            this.全选AToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.全选AToolStripMenuItem.Text = "全选(&A)";
            this.全选AToolStripMenuItem.Click += new System.EventHandler(this.全选AToolStripMenuItem_Click);
            // 
            // 全部清除CToolStripMenuItem
            // 
            this.全部清除CToolStripMenuItem.Name = "全部清除CToolStripMenuItem";
            this.全部清除CToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.全部清除CToolStripMenuItem.Text = "全部清除(&C)";
            this.全部清除CToolStripMenuItem.Click += new System.EventHandler(this.全部清除CToolStripMenuItem_Click);
            // 
            // 反选RToolStripMenuItem
            // 
            this.反选RToolStripMenuItem.Name = "反选RToolStripMenuItem";
            this.反选RToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.反选RToolStripMenuItem.Text = "反选(&R)";
            this.反选RToolStripMenuItem.Click += new System.EventHandler(this.反选RToolStripMenuItem_Click);
            // 
            // 选择关键字SToolStripMenuItem
            // 
            this.选择关键字SToolStripMenuItem.Name = "选择关键字SToolStripMenuItem";
            this.选择关键字SToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.选择关键字SToolStripMenuItem.Text = "选择关键字(&S)";
            this.选择关键字SToolStripMenuItem.Click += new System.EventHandler(this.选择关键字SToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // 清除关键字KToolStripMenuItem
            // 
            this.清除关键字KToolStripMenuItem.Name = "清除关键字KToolStripMenuItem";
            this.清除关键字KToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.清除关键字KToolStripMenuItem.Text = "清除关键字(&K)";
            this.清除关键字KToolStripMenuItem.Click += new System.EventHandler(this.清除关键字KToolStripMenuItem_Click);
            // 
            // ArrayCheckBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.groupBox1);
            this.Name = "ArrayCheckBoxControl";
            this.Size = new System.Drawing.Size(246, 79);
            this.Load += new System.EventHandler(this.EnumRadioControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 全选AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部清除CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 反选RToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 选择关键字SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除关键字KToolStripMenuItem;
    }
}
