namespace Geo.Utils
{
    partial class SelectMultiNameForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.okbutton1 = new System.Windows.Forms.Button();
            this.cancelbutton2 = new System.Windows.Forms.Button();
            this.selectAllbutton1 = new System.Windows.Forms.Button();
            this.cancelAllbutton1 = new System.Windows.Forms.Button();
            this.invertSelectbutton1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_filter = new System.Windows.Forms.Button();
            this.button_filterExclude = new System.Windows.Forms.Button();
            this.button_sort = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkedListBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 325);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "请选择";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 17);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(217, 305);
            this.checkedListBox1.TabIndex = 0;
            // 
            // okbutton1
            // 
            this.okbutton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbutton1.Location = new System.Drawing.Point(148, 346);
            this.okbutton1.Name = "okbutton1";
            this.okbutton1.Size = new System.Drawing.Size(75, 38);
            this.okbutton1.TabIndex = 1;
            this.okbutton1.Text = "确定";
            this.okbutton1.UseVisualStyleBackColor = true;
            this.okbutton1.Click += new System.EventHandler(this.okbutton1_Click);
            // 
            // cancelbutton2
            // 
            this.cancelbutton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelbutton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton2.Location = new System.Drawing.Point(241, 346);
            this.cancelbutton2.Name = "cancelbutton2";
            this.cancelbutton2.Size = new System.Drawing.Size(75, 38);
            this.cancelbutton2.TabIndex = 2;
            this.cancelbutton2.Text = "取消";
            this.cancelbutton2.UseVisualStyleBackColor = true;
            // 
            // selectAllbutton1
            // 
            this.selectAllbutton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAllbutton1.Location = new System.Drawing.Point(241, 32);
            this.selectAllbutton1.Name = "selectAllbutton1";
            this.selectAllbutton1.Size = new System.Drawing.Size(75, 39);
            this.selectAllbutton1.TabIndex = 3;
            this.selectAllbutton1.Text = "全选";
            this.selectAllbutton1.UseVisualStyleBackColor = true;
            this.selectAllbutton1.Click += new System.EventHandler(this.selectAllbutton1_Click);
            // 
            // cancelAllbutton1
            // 
            this.cancelAllbutton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelAllbutton1.Location = new System.Drawing.Point(241, 130);
            this.cancelAllbutton1.Name = "cancelAllbutton1";
            this.cancelAllbutton1.Size = new System.Drawing.Size(75, 39);
            this.cancelAllbutton1.TabIndex = 4;
            this.cancelAllbutton1.Text = "全清除";
            this.cancelAllbutton1.UseVisualStyleBackColor = true;
            this.cancelAllbutton1.Click += new System.EventHandler(this.cancelAllbutton1_Click);
            // 
            // invertSelectbutton1
            // 
            this.invertSelectbutton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.invertSelectbutton1.Location = new System.Drawing.Point(241, 81);
            this.invertSelectbutton1.Name = "invertSelectbutton1";
            this.invertSelectbutton1.Size = new System.Drawing.Size(75, 39);
            this.invertSelectbutton1.TabIndex = 5;
            this.invertSelectbutton1.Text = "反选";
            this.invertSelectbutton1.UseVisualStyleBackColor = true;
            this.invertSelectbutton1.Click += new System.EventHandler(this.invertSelectbutton1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(88, 345);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "竖向";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "纸张方向：";
            this.label1.Visible = false;
            // 
            // button_filter
            // 
            this.button_filter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_filter.Location = new System.Drawing.Point(241, 192);
            this.button_filter.Name = "button_filter";
            this.button_filter.Size = new System.Drawing.Size(75, 34);
            this.button_filter.TabIndex = 8;
            this.button_filter.Text = "过滤选择";
            this.button_filter.UseVisualStyleBackColor = true;
            this.button_filter.Click += new System.EventHandler(this.button_filter_Click);
            // 
            // button_filterExclude
            // 
            this.button_filterExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_filterExclude.Location = new System.Drawing.Point(241, 232);
            this.button_filterExclude.Name = "button_filterExclude";
            this.button_filterExclude.Size = new System.Drawing.Size(75, 34);
            this.button_filterExclude.TabIndex = 8;
            this.button_filterExclude.Text = "过滤排除";
            this.button_filterExclude.UseVisualStyleBackColor = true;
            this.button_filterExclude.Click += new System.EventHandler(this.button_filterExclude_Click);
            // 
            // button_sort
            // 
            this.button_sort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_sort.Location = new System.Drawing.Point(241, 272);
            this.button_sort.Name = "button_sort";
            this.button_sort.Size = new System.Drawing.Size(75, 34);
            this.button_sort.TabIndex = 8;
            this.button_sort.Text = "排序";
            this.button_sort.UseVisualStyleBackColor = true;
            this.button_sort.Click += new System.EventHandler(this.button_sort_Click);
            // 
            // SelectMultiNameForm
            // 
            this.AcceptButton = this.okbutton1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelbutton2;
            this.ClientSize = new System.Drawing.Size(338, 396);
            this.Controls.Add(this.button_sort);
            this.Controls.Add(this.button_filterExclude);
            this.Controls.Add(this.button_filter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.invertSelectbutton1);
            this.Controls.Add(this.cancelAllbutton1);
            this.Controls.Add(this.selectAllbutton1);
            this.Controls.Add(this.cancelbutton2);
            this.Controls.Add(this.okbutton1);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectMultiNameForm";
            this.ShowIcon = false;
            this.Text = "请输入或选择";
            this.Load += new System.EventHandler(this.SelectingColForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button okbutton1;
        private System.Windows.Forms.Button cancelbutton2;
        private System.Windows.Forms.Button selectAllbutton1;
        private System.Windows.Forms.Button cancelAllbutton1;
        private System.Windows.Forms.Button invertSelectbutton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_filter;
        private System.Windows.Forms.Button button_filterExclude;
        private System.Windows.Forms.Button button_sort;
    }
}