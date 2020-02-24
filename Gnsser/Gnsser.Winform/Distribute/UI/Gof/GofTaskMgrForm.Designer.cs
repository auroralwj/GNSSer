namespace Gnsser.Winform
{
    partial class GofTaskMgrForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GofTaskMgrForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.增加计算节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改计算节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.删除计算节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新计算节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bindingSource_task = new System.Windows.Forms.BindingSource(this.components);
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.导入OToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.导入Gof文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_task)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入Gof文件ToolStripMenuItem,
            this.toolStripSeparator1,
            this.增加计算节点ToolStripMenuItem,
            this.修改计算节点ToolStripMenuItem,
            this.toolStripSeparator4,
            this.删除计算节点ToolStripMenuItem,
            this.toolStripSeparator3,
            this.刷新计算节点ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(176, 170);
            // 
            // 增加计算节点ToolStripMenuItem
            // 
            this.增加计算节点ToolStripMenuItem.Name = "增加计算节点ToolStripMenuItem";
            this.增加计算节点ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.增加计算节点ToolStripMenuItem.Text = "增加任务";
            this.增加计算节点ToolStripMenuItem.Click += new System.EventHandler(this.增加计算节点ToolStripMenuItem_Click);
            // 
            // 修改计算节点ToolStripMenuItem
            // 
            this.修改计算节点ToolStripMenuItem.Name = "修改计算节点ToolStripMenuItem";
            this.修改计算节点ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.修改计算节点ToolStripMenuItem.Text = "修改任务";
            this.修改计算节点ToolStripMenuItem.Click += new System.EventHandler(this.修改计算节点ToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(165, 6);
            // 
            // 删除计算节点ToolStripMenuItem
            // 
            this.删除计算节点ToolStripMenuItem.Name = "删除计算节点ToolStripMenuItem";
            this.删除计算节点ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.删除计算节点ToolStripMenuItem.Text = "删除任务";
            this.删除计算节点ToolStripMenuItem.Click += new System.EventHandler(this.删除计算节点ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(165, 6);
            // 
            // 刷新计算节点ToolStripMenuItem
            // 
            this.刷新计算节点ToolStripMenuItem.Name = "刷新计算节点ToolStripMenuItem";
            this.刷新计算节点ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.刷新计算节点ToolStripMenuItem.Text = "刷新";
            this.刷新计算节点ToolStripMenuItem.Click += new System.EventHandler(this.刷新计算节点ToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.DataSource = this.bindingSource_task;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(650, 397);
            this.dataGridView1.TabIndex = 3;
            // 
            // bindingSource_task
            // 
            this.bindingSource_task.CurrentChanged += new System.EventHandler(this.bindingSource1_CurrentChanged);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Id";
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Name";
            this.Column2.HeaderText = "名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入OToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(650, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 导入OToolStripButton
            // 
            this.导入OToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.导入OToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("导入OToolStripButton.Image")));
            this.导入OToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导入OToolStripButton.Name = "导入OToolStripButton";
            this.导入OToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.导入OToolStripButton.Text = "导入(&O)";
            this.导入OToolStripButton.Click += new System.EventHandler(this.导入OToolStripButton_Click);
            // 
            // 导入Gof文件ToolStripMenuItem
            // 
            this.导入Gof文件ToolStripMenuItem.Name = "导入Gof文件ToolStripMenuItem";
            this.导入Gof文件ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.导入Gof文件ToolStripMenuItem.Text = "导入Gof文件";
            this.导入Gof文件ToolStripMenuItem.Click += new System.EventHandler(this.导入OToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // GofTaskMgrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 422);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GofTaskMgrForm";
            this.Text = "Gof任务编辑器";
            this.Load += new System.EventHandler(this.SiteNodeMgrForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_task)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource_task;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 增加计算节点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改计算节点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除计算节点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 刷新计算节点ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 导入OToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem 导入Gof文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}