namespace Geo
{
    partial class LectureFeeForm
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
            this.button_run = new System.Windows.Forms.Button();
            this.namedIntControl_times = new Geo.Winform.Controls.NamedIntControl();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.namedIntControl_stardard = new Geo.Winform.Controls.NamedFloatControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.namedFloatControl1_step2Tax = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1_step1Tax = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_step2Fee = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_step1Fee = new Geo.Winform.Controls.NamedFloatControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(504, 102);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 0;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // namedIntControl_times
            // 
            this.namedIntControl_times.Location = new System.Drawing.Point(42, 35);
            this.namedIntControl_times.Name = "namedIntControl_times";
            this.namedIntControl_times.Size = new System.Drawing.Size(117, 23);
            this.namedIntControl_times.TabIndex = 1;
            this.namedIntControl_times.Title = "课次：";
            this.namedIntControl_times.Value = 4;
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl_result.Location = new System.Drawing.Point(13, 133);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(570, 196);
            this.richTextBoxControl_result.TabIndex = 2;
            this.richTextBoxControl_result.Text = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(592, 91);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.namedIntControl_times);
            this.tabPage1.Controls.Add(this.namedIntControl_stardard);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(584, 65);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // namedIntControl_stardard
            // 
            this.namedIntControl_stardard.Location = new System.Drawing.Point(19, 3);
            this.namedIntControl_stardard.Name = "namedIntControl_stardard";
            this.namedIntControl_stardard.Size = new System.Drawing.Size(140, 23);
            this.namedIntControl_stardard.TabIndex = 1;
            this.namedIntControl_stardard.Title = "讲课标准：";
            this.namedIntControl_stardard.Value = 500D;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.namedFloatControl1_step2Tax);
            this.tabPage2.Controls.Add(this.namedFloatControl1_step1Tax);
            this.tabPage2.Controls.Add(this.namedFloatControl_step2Fee);
            this.tabPage2.Controls.Add(this.namedFloatControl_step1Fee);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(584, 65);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "详细设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl1_step2Tax
            // 
            this.namedFloatControl1_step2Tax.Location = new System.Drawing.Point(145, 35);
            this.namedFloatControl1_step2Tax.Name = "namedFloatControl1_step2Tax";
            this.namedFloatControl1_step2Tax.Size = new System.Drawing.Size(114, 23);
            this.namedFloatControl1_step2Tax.TabIndex = 4;
            this.namedFloatControl1_step2Tax.Title = "阶梯2税率：";
            this.namedFloatControl1_step2Tax.Value = 19.05D;
            // 
            // namedFloatControl1_step1Tax
            // 
            this.namedFloatControl1_step1Tax.Location = new System.Drawing.Point(145, 6);
            this.namedFloatControl1_step1Tax.Name = "namedFloatControl1_step1Tax";
            this.namedFloatControl1_step1Tax.Size = new System.Drawing.Size(114, 23);
            this.namedFloatControl1_step1Tax.TabIndex = 3;
            this.namedFloatControl1_step1Tax.Title = "阶梯1税率：";
            this.namedFloatControl1_step1Tax.Value = 25D;
            // 
            // namedFloatControl_step2Fee
            // 
            this.namedFloatControl_step2Fee.Location = new System.Drawing.Point(7, 35);
            this.namedFloatControl_step2Fee.Name = "namedFloatControl_step2Fee";
            this.namedFloatControl_step2Fee.Size = new System.Drawing.Size(114, 23);
            this.namedFloatControl_step2Fee.TabIndex = 2;
            this.namedFloatControl_step2Fee.Title = "阶梯2：";
            this.namedFloatControl_step2Fee.Value = 3360D;
            // 
            // namedFloatControl_step1Fee
            // 
            this.namedFloatControl_step1Fee.Location = new System.Drawing.Point(7, 6);
            this.namedFloatControl_step1Fee.Name = "namedFloatControl_step1Fee";
            this.namedFloatControl_step1Fee.Size = new System.Drawing.Size(114, 23);
            this.namedFloatControl_step1Fee.TabIndex = 1;
            this.namedFloatControl_step1Fee.Title = "阶梯1：";
            this.namedFloatControl_step1Fee.Value = 800D;
            // 
            // LectureFeeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 341);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.richTextBoxControl_result);
            this.Controls.Add(this.button_run);
            this.Name = "LectureFeeForm";
            this.Text = "讲课费计算器";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_run;
        private Winform.Controls.NamedIntControl namedIntControl_times;
        private Winform.Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Winform.Controls.NamedFloatControl namedFloatControl_step2Fee;
        private Winform.Controls.NamedFloatControl namedFloatControl_step1Fee;
        private Winform.Controls.NamedFloatControl namedFloatControl1_step1Tax;
        private Winform.Controls.NamedFloatControl namedFloatControl1_step2Tax;
        private Winform.Controls.NamedFloatControl namedIntControl_stardard;
    }
}