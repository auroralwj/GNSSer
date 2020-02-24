namespace Gnsser.Winform
{
    partial class TropCaculateForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_run = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.radioButton_neill = new System.Windows.Forms.RadioButton();
            this.radioButton_gmf = new System.Windows.Forms.RadioButton();
            this.radioButton_VMF = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_geoCoord = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.textBox_geoCoord);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton_VMF);
            this.groupBox1.Controls.Add(this.radioButton_gmf);
            this.groupBox1.Controls.Add(this.radioButton_neill);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(412, 118);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(92, 32);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.richTextBoxControl1);
            this.groupBox2.Location = new System.Drawing.Point(12, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(492, 137);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 17);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(486, 117);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // radioButton_neill
            // 
            this.radioButton_neill.AutoSize = true;
            this.radioButton_neill.Location = new System.Drawing.Point(36, 78);
            this.radioButton_neill.Name = "radioButton_neill";
            this.radioButton_neill.Size = new System.Drawing.Size(77, 16);
            this.radioButton_neill.TabIndex = 0;
            this.radioButton_neill.TabStop = true;
            this.radioButton_neill.Text = "Neill模型";
            this.radioButton_neill.UseVisualStyleBackColor = true;
            // 
            // radioButton_gmf
            // 
            this.radioButton_gmf.AutoSize = true;
            this.radioButton_gmf.Location = new System.Drawing.Point(122, 78);
            this.radioButton_gmf.Name = "radioButton_gmf";
            this.radioButton_gmf.Size = new System.Drawing.Size(71, 16);
            this.radioButton_gmf.TabIndex = 0;
            this.radioButton_gmf.TabStop = true;
            this.radioButton_gmf.Text = "GMF 模型";
            this.radioButton_gmf.UseVisualStyleBackColor = true;
            // 
            // radioButton_VMF
            // 
            this.radioButton_VMF.AutoSize = true;
            this.radioButton_VMF.Location = new System.Drawing.Point(199, 78);
            this.radioButton_VMF.Name = "radioButton_VMF";
            this.radioButton_VMF.Size = new System.Drawing.Size(71, 16);
            this.radioButton_VMF.TabIndex = 0;
            this.radioButton_VMF.TabStop = true;
            this.radioButton_VMF.Text = "VMF 模型";
            this.radioButton_VMF.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "大地坐标：";
            // 
            // textBox_geoCoord
            // 
            this.textBox_geoCoord.Location = new System.Drawing.Point(119, 18);
            this.textBox_geoCoord.Name = "textBox_geoCoord";
            this.textBox_geoCoord.Size = new System.Drawing.Size(224, 21);
            this.textBox_geoCoord.TabIndex = 2;
            this.textBox_geoCoord.Text = "(120.00, 30.0, 100.0)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = " 日期（年积日）：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(120, 42);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // TropCaculateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 312);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TropCaculateForm";
            this.Text = "对流层改正计算器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.GroupBox groupBox2;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.RadioButton radioButton_gmf;
        private System.Windows.Forms.RadioButton radioButton_neill;
        private System.Windows.Forms.RadioButton radioButton_VMF;
        private System.Windows.Forms.TextBox textBox_geoCoord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
    }
}