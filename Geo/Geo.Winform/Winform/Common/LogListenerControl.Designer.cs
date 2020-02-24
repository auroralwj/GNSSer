namespace Geo.Winform
{
    partial class LogListenerControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.checkBox_fixWindow = new System.Windows.Forms.CheckBox();
            this.label_info = new System.Windows.Forms.Label();
            this.enumCheckBoxControl1 = new Geo.Winform.EnumCheckBoxControl();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_expand = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_shrink = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_up = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_down = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox_keyword = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1search = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label_info);
            this.splitContainer1.Panel1.Controls.Add(this.enumCheckBoxControl1);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxControl1);
            this.splitContainer1.Size = new System.Drawing.Size(600, 224);
            this.splitContainer1.SplitterDistance = 151;
            this.splitContainer1.TabIndex = 5;
            // 
            // checkBox_fixWindow
            // 
            this.checkBox_fixWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_fixWindow.AutoSize = true;
            this.checkBox_fixWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBox_fixWindow.Location = new System.Drawing.Point(525, 3);
            this.checkBox_fixWindow.Name = "checkBox_fixWindow";
            this.checkBox_fixWindow.Size = new System.Drawing.Size(72, 16);
            this.checkBox_fixWindow.TabIndex = 6;
            this.checkBox_fixWindow.Text = "固定窗口";
            this.checkBox_fixWindow.UseVisualStyleBackColor = false;
            // 
            // label_info
            // 
            this.label_info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(3, 201);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(29, 12);
            this.label_info.TabIndex = 5;
            this.label_info.Text = "Info";
            // 
            // enumCheckBoxControl1
            // 
            this.enumCheckBoxControl1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.enumCheckBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumCheckBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.enumCheckBoxControl1.Name = "enumCheckBoxControl1";
            this.enumCheckBoxControl1.Size = new System.Drawing.Size(151, 224);
            this.enumCheckBoxControl1.TabIndex = 4;
            this.enumCheckBoxControl1.Title = "输出选项";
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxControl1.MaxAppendLineCount = 10000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(445, 224);
            this.richTextBoxControl1.TabIndex = 1;
            this.richTextBoxControl1.Text = "";
            this.richTextBoxControl1.TextChanged += new System.EventHandler(this.richTextBoxControl1_TextChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_expand,
            this.toolStripLabel_shrink,
            this.toolStripLabel_up,
            this.toolStripLabel_down,
            this.toolStripComboBox1,
            this.toolStripLabel1,
            this.toolStripTextBox_keyword,
            this.toolStripButton1search});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(600, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.MouseHover += new System.EventHandler(this.toolStrip1_MouseHover);
            this.toolStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStrip1_MouseMove);
            this.toolStrip1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolStrip1_MouseUp);
            // 
            // toolStripLabel_expand
            // 
            this.toolStripLabel_expand.Name = "toolStripLabel_expand";
            this.toolStripLabel_expand.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel_expand.Text = "展开";
            this.toolStripLabel_expand.Click += new System.EventHandler(this.toolStripLabel_expand_Click);
            // 
            // toolStripLabel_shrink
            // 
            this.toolStripLabel_shrink.Name = "toolStripLabel_shrink";
            this.toolStripLabel_shrink.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel_shrink.Text = "收缩";
            this.toolStripLabel_shrink.Click += new System.EventHandler(this.toolStripLabel_shrink_Click);
            // 
            // toolStripLabel_up
            // 
            this.toolStripLabel_up.Name = "toolStripLabel_up";
            this.toolStripLabel_up.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel_up.Text = "增加";
            this.toolStripLabel_up.Click += new System.EventHandler(this.toolStripLabel_up_Click);
            // 
            // toolStripLabel_down
            // 
            this.toolStripLabel_down.Name = "toolStripLabel_down";
            this.toolStripLabel_down.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel_down.Text = "减小";
            this.toolStripLabel_down.Click += new System.EventHandler(this.toolStripLabel_down_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "所有",
            "警告",
            "错误",
            "信息",
            "关键"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(81, 25);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel1.Text = "关键字：";
            // 
            // toolStripTextBox_keyword
            // 
            this.toolStripTextBox_keyword.Name = "toolStripTextBox_keyword";
            this.toolStripTextBox_keyword.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripButton1search
            // 
            this.toolStripButton1search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.toolStripButton1search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1search.Image = global::Geo.Properties.Resources.search_16x16;
            this.toolStripButton1search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1search.Name = "toolStripButton1search";
            this.toolStripButton1search.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1search.Text = "搜索";
            this.toolStripButton1search.Click += new System.EventHandler(this.toolStripLabel1_search_Click);
            // 
            // LogListenerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_fixWindow);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "LogListenerControl";
            this.Size = new System.Drawing.Size(600, 249);
            this.Load += new System.EventHandler(this.LogListenerControl_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LogListenerControl_MouseUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.RichTextBoxControl richTextBoxControl1;
        private EnumCheckBoxControl enumCheckBoxControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox_keyword;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_down;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_up;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_shrink;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_expand;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.ToolStripButton toolStripButton1search;
        private System.Windows.Forms.CheckBox checkBox_fixWindow;
    }
}
