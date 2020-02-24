namespace Geo.WinTools
{
    partial class PolyfitForm
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
            this.textBox_number = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_out = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button_draw = new System.Windows.Forms.Button();
            this.textBox_order = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_nextY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_errorTimes = new System.Windows.Forms.TextBox();
            this.button_differOnce = new System.Windows.Forms.Button();
            this.button_extractLasttoNext = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_number);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 424);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // textBox_number
            // 
            this.textBox_number.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_number.Location = new System.Drawing.Point(3, 17);
            this.textBox_number.Multiline = true;
            this.textBox_number.Name = "textBox_number";
            this.textBox_number.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_number.Size = new System.Drawing.Size(253, 404);
            this.textBox_number.TabIndex = 1;
            this.textBox_number.Text = "1\r\n2\r\n4\r\n8\r\n10\r\n14\r\n16\r\n23\r\n44\r\n88";
            // 
            // button_ok
            // 
            this.button_ok.BackColor = System.Drawing.Color.OrangeRed;
            this.button_ok.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button_ok.Location = new System.Drawing.Point(11, 265);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 48);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "计算";
            this.button_ok.UseVisualStyleBackColor = false;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_out
            // 
            this.textBox_out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_out.Location = new System.Drawing.Point(3, 17);
            this.textBox_out.Multiline = true;
            this.textBox_out.Name = "textBox_out";
            this.textBox_out.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_out.Size = new System.Drawing.Size(216, 404);
            this.textBox_out.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_out);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 424);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(602, 424);
            this.splitContainer1.SplitterDistance = 259;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button_draw);
            this.splitContainer2.Panel1.Controls.Add(this.textBox_nextY);
            this.splitContainer2.Panel1.Controls.Add(this.textBox_errorTimes);
            this.splitContainer2.Panel1.Controls.Add(this.textBox_order);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.button_extractLasttoNext);
            this.splitContainer2.Panel1.Controls.Add(this.button_differOnce);
            this.splitContainer2.Panel1.Controls.Add(this.button_ok);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(339, 424);
            this.splitContainer2.SplitterDistance = 113;
            this.splitContainer2.TabIndex = 0;
            // 
            // button_draw
            // 
            this.button_draw.Location = new System.Drawing.Point(11, 319);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(75, 37);
            this.button_draw.TabIndex = 4;
            this.button_draw.Text = "绘图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // textBox_order
            // 
            this.textBox_order.Location = new System.Drawing.Point(11, 36);
            this.textBox_order.Name = "textBox_order";
            this.textBox_order.Size = new System.Drawing.Size(60, 21);
            this.textBox_order.TabIndex = 3;
            this.textBox_order.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "拟合阶次：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "下一个值：";
            // 
            // textBox_nextY
            // 
            this.textBox_nextY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_nextY.Location = new System.Drawing.Point(11, 170);
            this.textBox_nextY.Name = "textBox_nextY";
            this.textBox_nextY.Size = new System.Drawing.Size(75, 21);
            this.textBox_nextY.TabIndex = 3;
            this.textBox_nextY.Text = "100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "限差倍数：";
            // 
            // textBox_errorTimes
            // 
            this.textBox_errorTimes.Location = new System.Drawing.Point(11, 221);
            this.textBox_errorTimes.Name = "textBox_errorTimes";
            this.textBox_errorTimes.Size = new System.Drawing.Size(60, 21);
            this.textBox_errorTimes.TabIndex = 3;
            this.textBox_errorTimes.Text = "3";
            // 
            // button_differOnce
            // 
            this.button_differOnce.Location = new System.Drawing.Point(11, 63);
            this.button_differOnce.Name = "button_differOnce";
            this.button_differOnce.Size = new System.Drawing.Size(75, 26);
            this.button_differOnce.TabIndex = 1;
            this.button_differOnce.Text = "差分一次";
            this.button_differOnce.UseVisualStyleBackColor = true;
            this.button_differOnce.Click += new System.EventHandler(this.button_differOnce_Click);
            // 
            // button_extractLasttoNext
            // 
            this.button_extractLasttoNext.Location = new System.Drawing.Point(11, 100);
            this.button_extractLasttoNext.Name = "button_extractLasttoNext";
            this.button_extractLasttoNext.Size = new System.Drawing.Size(75, 51);
            this.button_extractLasttoNext.TabIndex = 1;
            this.button_extractLasttoNext.Text = "提取最后作为下一值";
            this.button_extractLasttoNext.UseVisualStyleBackColor = true;
            this.button_extractLasttoNext.Click += new System.EventHandler(this.button_extractLasttoNext_Click);
            // 
            // PolyfitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 424);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PolyfitForm";
            this.Text = "多项式拟合";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_number;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_out;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox textBox_order;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.TextBox textBox_nextY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_errorTimes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_differOnce;
        private System.Windows.Forms.Button button_extractLasttoNext;
    }
}