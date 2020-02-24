namespace Geo.Winform
{
    partial class ColorSelectControl
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_Color = new System.Windows.Forms.Label();
            this.textBox_alfa = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // label_Color
            // 
            this.label_Color.AutoSize = true;
            this.label_Color.BackColor = System.Drawing.Color.Black;
            this.label_Color.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Color.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label_Color.Location = new System.Drawing.Point(39, -1);
            this.label_Color.Margin = new System.Windows.Forms.Padding(3);
            this.label_Color.Name = "label_Color";
            this.label_Color.Padding = new System.Windows.Forms.Padding(6);
            this.label_Color.Size = new System.Drawing.Size(91, 26);
            this.label_Color.TabIndex = 34;
            this.label_Color.Text = "点击设置颜色";
            this.label_Color.Click += new System.EventHandler(this.label_Color_Click);
            // 
            // textBox_alfa
            // 
            this.textBox_alfa.Location = new System.Drawing.Point(196, 3);
            this.textBox_alfa.Name = "textBox_alfa";
            this.textBox_alfa.Size = new System.Drawing.Size(43, 21);
            this.textBox_alfa.TabIndex = 36;
            this.textBox_alfa.Text = "200";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(147, 7);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 37;
            this.label20.Text = "透明度：";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(3, 7);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 12);
            this.label21.TabIndex = 35;
            this.label21.Text = "颜色：";
            // 
            // ColorSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_Color);
            this.Controls.Add(this.textBox_alfa);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label21);
            this.Name = "ColorSelectControl";
            this.Size = new System.Drawing.Size(244, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Color;
        private System.Windows.Forms.TextBox textBox_alfa;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
