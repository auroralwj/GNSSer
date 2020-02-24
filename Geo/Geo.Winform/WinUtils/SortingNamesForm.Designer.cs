namespace Geo.Utils
{
    partial class SortingNamesForm
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
            this.button_up = new System.Windows.Forms.Button();
            this.button_last = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.button_top = new System.Windows.Forms.Button();
            this.button_selectAll = new System.Windows.Forms.Button();
            this.button_reverseSelect = new System.Windows.Forms.Button();
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
            this.groupBox1.Size = new System.Drawing.Size(251, 383);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "名称列表";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 17);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(245, 363);
            this.checkedListBox1.TabIndex = 0;
            // 
            // okbutton1
            // 
            this.okbutton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbutton1.Location = new System.Drawing.Point(176, 411);
            this.okbutton1.Name = "okbutton1";
            this.okbutton1.Size = new System.Drawing.Size(75, 31);
            this.okbutton1.TabIndex = 1;
            this.okbutton1.Text = "确定";
            this.okbutton1.UseVisualStyleBackColor = true;
            this.okbutton1.Click += new System.EventHandler(this.okbutton1_Click);
            // 
            // cancelbutton2
            // 
            this.cancelbutton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelbutton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton2.Location = new System.Drawing.Point(269, 411);
            this.cancelbutton2.Name = "cancelbutton2";
            this.cancelbutton2.Size = new System.Drawing.Size(75, 31);
            this.cancelbutton2.TabIndex = 2;
            this.cancelbutton2.Text = "取消";
            this.cancelbutton2.UseVisualStyleBackColor = true;
            // 
            // button_up
            // 
            this.button_up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_up.Location = new System.Drawing.Point(279, 74);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(75, 33);
            this.button_up.TabIndex = 3;
            this.button_up.Text = "往上";
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_last
            // 
            this.button_last.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_last.Location = new System.Drawing.Point(279, 180);
            this.button_last.Name = "button_last";
            this.button_last.Size = new System.Drawing.Size(75, 33);
            this.button_last.TabIndex = 4;
            this.button_last.Text = "最后";
            this.button_last.UseVisualStyleBackColor = true;
            this.button_last.Click += new System.EventHandler(this.button_last_Click);
            // 
            // button_down
            // 
            this.button_down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_down.Location = new System.Drawing.Point(279, 130);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(75, 33);
            this.button_down.TabIndex = 5;
            this.button_down.Text = "往下";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // button_top
            // 
            this.button_top.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_top.Location = new System.Drawing.Point(279, 23);
            this.button_top.Name = "button_top";
            this.button_top.Size = new System.Drawing.Size(75, 33);
            this.button_top.TabIndex = 3;
            this.button_top.Text = "最前";
            this.button_top.UseVisualStyleBackColor = true;
            this.button_top.Click += new System.EventHandler(this.button_top_Click);
            // 
            // button_selectAll
            // 
            this.button_selectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectAll.Location = new System.Drawing.Point(279, 275);
            this.button_selectAll.Name = "button_selectAll";
            this.button_selectAll.Size = new System.Drawing.Size(75, 33);
            this.button_selectAll.TabIndex = 4;
            this.button_selectAll.Text = "全选";
            this.button_selectAll.UseVisualStyleBackColor = true;
            this.button_selectAll.Click += new System.EventHandler(this.button_selectAll_Click);
            // 
            // button_reverseSelect
            // 
            this.button_reverseSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_reverseSelect.Location = new System.Drawing.Point(279, 325);
            this.button_reverseSelect.Name = "button_reverseSelect";
            this.button_reverseSelect.Size = new System.Drawing.Size(75, 33);
            this.button_reverseSelect.TabIndex = 4;
            this.button_reverseSelect.Text = "反选";
            this.button_reverseSelect.UseVisualStyleBackColor = true;
            this.button_reverseSelect.Click += new System.EventHandler(this.button_reverseSelect_Click);
            // 
            // SortingNamesForm
            // 
            this.AcceptButton = this.okbutton1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelbutton2;
            this.ClientSize = new System.Drawing.Size(366, 454);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.button_reverseSelect);
            this.Controls.Add(this.button_selectAll);
            this.Controls.Add(this.button_last);
            this.Controls.Add(this.button_top);
            this.Controls.Add(this.button_up);
            this.Controls.Add(this.cancelbutton2);
            this.Controls.Add(this.okbutton1);
            this.Controls.Add(this.groupBox1);
            this.Name = "SortingNamesForm";
            this.ShowIcon = false;
            this.Text = "请排序";
            this.Load += new System.EventHandler(this.SortingNamesForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button okbutton1;
        private System.Windows.Forms.Button cancelbutton2;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_last;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Button button_top;
        private System.Windows.Forms.Button button_selectAll;
        private System.Windows.Forms.Button button_reverseSelect;
    }
}