namespace Geo.Winform
{
    partial class SimpleSearchForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton_include = new System.Windows.Forms.RadioButton();
            this.radioButton_exclude = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton_fuzzy = new System.Windows.Forms.RadioButton();
            this.radioButton_exact = new System.Windows.Forms.RadioButton();
            this.textBox_word = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ok = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel2);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.textBox_word);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.radioButton_include);
            this.flowLayoutPanel2.Controls.Add(this.radioButton_exclude);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(268, 24);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(128, 25);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // radioButton_include
            // 
            this.radioButton_include.AutoSize = true;
            this.radioButton_include.Checked = true;
            this.radioButton_include.Location = new System.Drawing.Point(3, 3);
            this.radioButton_include.Name = "radioButton_include";
            this.radioButton_include.Size = new System.Drawing.Size(47, 16);
            this.radioButton_include.TabIndex = 0;
            this.radioButton_include.TabStop = true;
            this.radioButton_include.Text = "包含";
            this.radioButton_include.UseVisualStyleBackColor = true;
            // 
            // radioButton_exclude
            // 
            this.radioButton_exclude.AutoSize = true;
            this.radioButton_exclude.Location = new System.Drawing.Point(56, 3);
            this.radioButton_exclude.Name = "radioButton_exclude";
            this.radioButton_exclude.Size = new System.Drawing.Size(59, 16);
            this.radioButton_exclude.TabIndex = 0;
            this.radioButton_exclude.Text = "不包含";
            this.radioButton_exclude.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radioButton_fuzzy);
            this.flowLayoutPanel1.Controls.Add(this.radioButton_exact);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(74, 51);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(322, 25);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // radioButton_fuzzy
            // 
            this.radioButton_fuzzy.AutoSize = true;
            this.radioButton_fuzzy.Checked = true;
            this.radioButton_fuzzy.Location = new System.Drawing.Point(3, 3);
            this.radioButton_fuzzy.Name = "radioButton_fuzzy";
            this.radioButton_fuzzy.Size = new System.Drawing.Size(71, 16);
            this.radioButton_fuzzy.TabIndex = 0;
            this.radioButton_fuzzy.TabStop = true;
            this.radioButton_fuzzy.Text = "模糊匹配";
            this.radioButton_fuzzy.UseVisualStyleBackColor = true;
            // 
            // radioButton_exact
            // 
            this.radioButton_exact.AutoSize = true;
            this.radioButton_exact.Location = new System.Drawing.Point(80, 3);
            this.radioButton_exact.Name = "radioButton_exact";
            this.radioButton_exact.Size = new System.Drawing.Size(71, 16);
            this.radioButton_exact.TabIndex = 0;
            this.radioButton_exact.Text = "精确匹配";
            this.radioButton_exact.UseVisualStyleBackColor = true;
            // 
            // textBox_word
            // 
            this.textBox_word.Location = new System.Drawing.Point(74, 24);
            this.textBox_word.Name = "textBox_word";
            this.textBox_word.Size = new System.Drawing.Size(186, 21);
            this.textBox_word.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "关键字：";
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(349, 114);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 29);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // SimpleSearchForm
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 155);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "SimpleSearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "搜索条件";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_word;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton_fuzzy;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButton_include;
        private System.Windows.Forms.RadioButton radioButton_exclude;
        private System.Windows.Forms.RadioButton radioButton_exact;
    }
}