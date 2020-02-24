namespace Geo.Winform
{
    partial class ShortcutCollectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShortcutCollectorForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.程序PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.增加子程序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开主程序位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于本系统AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.程序PToolStripMenuItem,
            this.设置CToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(838, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 程序PToolStripMenuItem
            // 
            this.程序PToolStripMenuItem.Name = "程序PToolStripMenuItem";
            this.程序PToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.程序PToolStripMenuItem.Text = "程序(&P)";
            // 
            // 设置CToolStripMenuItem
            // 
            this.设置CToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.增加子程序ToolStripMenuItem,
            this.数据文件ToolStripMenuItem,
            this.打开主程序位置ToolStripMenuItem});
            this.设置CToolStripMenuItem.Name = "设置CToolStripMenuItem";
            this.设置CToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.设置CToolStripMenuItem.Text = "设置(&C)";
            // 
            // 增加子程序ToolStripMenuItem
            // 
            this.增加子程序ToolStripMenuItem.Name = "增加子程序ToolStripMenuItem";
            this.增加子程序ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.增加子程序ToolStripMenuItem.Text = "增加子程序";
            this.增加子程序ToolStripMenuItem.Click += new System.EventHandler(this.增加子程序ToolStripMenuItem_Click);
            // 
            // 数据文件ToolStripMenuItem
            // 
            this.数据文件ToolStripMenuItem.Name = "数据文件ToolStripMenuItem";
            this.数据文件ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.数据文件ToolStripMenuItem.Text = "数据文件";
            this.数据文件ToolStripMenuItem.Click += new System.EventHandler(this.数据文件ToolStripMenuItem_Click);
            // 
            // 打开主程序位置ToolStripMenuItem
            // 
            this.打开主程序位置ToolStripMenuItem.Name = "打开主程序位置ToolStripMenuItem";
            this.打开主程序位置ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.打开主程序位置ToolStripMenuItem.Text = "打开主程序位置";
            this.打开主程序位置ToolStripMenuItem.Click += new System.EventHandler(this.打开主程序位置ToolStripMenuItem_Click);
            // 
            // 关于本系统AToolStripMenuItem
            // 
            this.关于本系统AToolStripMenuItem.Name = "关于本系统AToolStripMenuItem";
            this.关于本系统AToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(838, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.toolStripButton1.Image = global::Geo.Winform.Properties.Resources.add_16x16;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "添加";
            this.toolStripButton1.Click += new System.EventHandler(this.增加子程序ToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.toolStripButton2.Image = global::Geo.Winform.Properties.Resources.refresh_16x16;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "刷新";
            this.toolStripButton2.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // ShortcutCollectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(838, 484);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ShortcutCollectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "程序库";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.ShortcutCollector_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 程序PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 增加子程序ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于本系统AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开主程序位置ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}

