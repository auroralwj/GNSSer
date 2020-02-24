namespace Gnsser.Winform.Suvey
{
    partial class InitConfigAngleTheodoliteForm
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
            this.namedIntControl_round = new Geo.Winform.Controls.NamedIntControl();
            this.radioButton_j2 = new System.Windows.Forms.RadioButton();
            this.radioButton_j1 = new System.Windows.Forms.RadioButton();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(493, 26);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 33);
            this.button_run.TabIndex = 0;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // namedIntControl_round
            // 
            this.namedIntControl_round.Location = new System.Drawing.Point(168, 20);
            this.namedIntControl_round.Name = "namedIntControl_round";
            this.namedIntControl_round.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_round.TabIndex = 1;
            this.namedIntControl_round.Title = "测回数量：";
            this.namedIntControl_round.Value = 3;
            // 
            // radioButton_j2
            // 
            this.radioButton_j2.AutoSize = true;
            this.radioButton_j2.Checked = true;
            this.radioButton_j2.Location = new System.Drawing.Point(71, 20);
            this.radioButton_j2.Name = "radioButton_j2";
            this.radioButton_j2.Size = new System.Drawing.Size(35, 16);
            this.radioButton_j2.TabIndex = 2;
            this.radioButton_j2.TabStop = true;
            this.radioButton_j2.Text = "J2";
            this.radioButton_j2.UseVisualStyleBackColor = true;
            // 
            // radioButton_j1
            // 
            this.radioButton_j1.AutoSize = true;
            this.radioButton_j1.Location = new System.Drawing.Point(18, 20);
            this.radioButton_j1.Name = "radioButton_j1";
            this.radioButton_j1.Size = new System.Drawing.Size(35, 16);
            this.radioButton_j1.TabIndex = 3;
            this.radioButton_j1.Text = "J1";
            this.radioButton_j1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(12, 66);
            this.richTextBoxControl1.MaxAppendLineCount = 10000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(563, 236);
            this.richTextBoxControl1.TabIndex = 4;
            this.richTextBoxControl1.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_j1);
            this.groupBox1.Controls.Add(this.radioButton_j2);
            this.groupBox1.Location = new System.Drawing.Point(2, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(137, 53);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "经纬度类型";
            // 
            // SettingInitAngleTheodoliteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 314);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBoxControl1);
            this.Controls.Add(this.namedIntControl_round);
            this.Controls.Add(this.button_run);
            this.Name = "SettingInitAngleTheodoliteForm";
            this.Text = "配置度盘";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_run;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_round;
        private System.Windows.Forms.RadioButton radioButton_j2;
        private System.Windows.Forms.RadioButton radioButton_j1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}