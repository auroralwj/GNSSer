namespace Gnsser.Winform
{
    partial class CaculateAntenaHeightForm
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
            this.textBox_antdiameter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_slantheight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.textBox_out = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_phaseHeight = new Geo.Winform.Controls.NamedFloatControl();
            this.label3 = new System.Windows.Forms.Label();
            this.namedIntControl_fractionCount = new Geo.Winform.Controls.NamedIntControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_antdiameter
            // 
            this.textBox_antdiameter.Location = new System.Drawing.Point(126, 24);
            this.textBox_antdiameter.Name = "textBox_antdiameter";
            this.textBox_antdiameter.Size = new System.Drawing.Size(139, 21);
            this.textBox_antdiameter.TabIndex = 0;
            this.textBox_antdiameter.Text = "0.33962";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "天线直径（米）：";
            // 
            // textBox_slantheight
            // 
            this.textBox_slantheight.Location = new System.Drawing.Point(126, 51);
            this.textBox_slantheight.Name = "textBox_slantheight";
            this.textBox_slantheight.Size = new System.Drawing.Size(139, 21);
            this.textBox_slantheight.TabIndex = 0;
            this.textBox_slantheight.Text = "1.471";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "斜高（米）：";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(436, 105);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 28);
            this.button_run.TabIndex = 2;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // textBox_out
            // 
            this.textBox_out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_out.Location = new System.Drawing.Point(14, 139);
            this.textBox_out.Multiline = true;
            this.textBox_out.Name = "textBox_out";
            this.textBox_out.Size = new System.Drawing.Size(497, 117);
            this.textBox_out.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.namedIntControl_fractionCount);
            this.groupBox1.Controls.Add(this.namedFloatControl_phaseHeight);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_antdiameter);
            this.groupBox1.Controls.Add(this.textBox_slantheight);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(14, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(497, 87);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息（长度单位：m）";
            // 
            // namedFloatControl_phaseHeight
            // 
            this.namedFloatControl_phaseHeight.Location = new System.Drawing.Point(281, 24);
            this.namedFloatControl_phaseHeight.Name = "namedFloatControl_phaseHeight";
            this.namedFloatControl_phaseHeight.Size = new System.Drawing.Size(180, 23);
            this.namedFloatControl_phaseHeight.TabIndex = 2;
            this.namedFloatControl_phaseHeight.Title = "相位中心高度：";
            this.namedFloatControl_phaseHeight.Value = 0.00891D;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "说明：采用勾股定理计算天线高";
            // 
            // namedIntControl_fractionCount
            // 
            this.namedIntControl_fractionCount.Location = new System.Drawing.Point(281, 53);
            this.namedIntControl_fractionCount.Name = "namedIntControl_fractionCount";
            this.namedIntControl_fractionCount.Size = new System.Drawing.Size(133, 23);
            this.namedIntControl_fractionCount.TabIndex = 3;
            this.namedIntControl_fractionCount.Title = "保留小数位：";
            this.namedIntControl_fractionCount.Value = 4;
            // 
            // CaculateAntenaHeightForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 268);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_out);
            this.Controls.Add(this.button_run);
            this.Name = "CaculateAntenaHeightForm";
            this.Text = "计算天线高";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_antdiameter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_slantheight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TextBox textBox_out;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_phaseHeight;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_fractionCount;
    }
}