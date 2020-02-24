namespace Geo
{
    partial class FontSettingForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.colorSelectControl1 = new Geo.Winform.ColorSelectControl();
            this.comboBox_fontType = new System.Windows.Forms.ComboBox();
            this.textBox_fontSize = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.enumRadioControl_FontStyle = new Geo.Winform.EnumRadioControl();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(362, 169);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(281, 169);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(450, 164);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.colorSelectControl1);
            this.tabPage1.Controls.Add(this.comboBox_fontType);
            this.tabPage1.Controls.Add(this.textBox_fontSize);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 138);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // colorSelectControl1
            // 
            this.colorSelectControl1.Location = new System.Drawing.Point(10, 6);
            this.colorSelectControl1.Name = "colorSelectControl1";
            this.colorSelectControl1.Size = new System.Drawing.Size(244, 28);
            this.colorSelectControl1.TabIndex = 30;
            // 
            // comboBox_fontType
            // 
            this.comboBox_fontType.FormattingEnabled = true;
            this.comboBox_fontType.Items.AddRange(new object[] {
            "微软雅黑",
            "宋体",
            "黑体",
            "Times New Roman",
            "楷体"});
            this.comboBox_fontType.Location = new System.Drawing.Point(79, 89);
            this.comboBox_fontType.Name = "comboBox_fontType";
            this.comboBox_fontType.Size = new System.Drawing.Size(100, 20);
            this.comboBox_fontType.TabIndex = 29;
            this.comboBox_fontType.Text = "宋体";
            // 
            // textBox_fontSize
            // 
            this.textBox_fontSize.Location = new System.Drawing.Point(79, 51);
            this.textBox_fontSize.Name = "textBox_fontSize";
            this.textBox_fontSize.Size = new System.Drawing.Size(100, 21);
            this.textBox_fontSize.TabIndex = 28;
            this.textBox_fontSize.Text = "10";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(182, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 25;
            this.label11.Text = "像素";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(31, 93);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 26;
            this.label13.Text = "字体：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 55);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 27;
            this.label12.Text = "字体大小：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.enumRadioControl_FontStyle);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(442, 138);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "格式";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_FontStyle
            // 
            this.enumRadioControl_FontStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumRadioControl_FontStyle.Location = new System.Drawing.Point(3, 3);
            this.enumRadioControl_FontStyle.Name = "enumRadioControl_FontStyle";
            this.enumRadioControl_FontStyle.Size = new System.Drawing.Size(436, 132);
            this.enumRadioControl_FontStyle.TabIndex = 0;
            this.enumRadioControl_FontStyle.Title = "选项";
            // 
            // FontSettingForm
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(449, 204);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Name = "FontSettingForm";
            this.Text = "字体样式设置";
            this.Load += new System.EventHandler(this.FontSettingForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox comboBox_fontType;
        private System.Windows.Forms.TextBox textBox_fontSize;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage2;
        private Winform.EnumRadioControl enumRadioControl_FontStyle;
        private Winform.ColorSelectControl colorSelectControl1;
    }
}