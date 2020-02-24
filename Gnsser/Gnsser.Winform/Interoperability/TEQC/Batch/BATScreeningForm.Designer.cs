namespace Gnsser.Winform
{
    partial class BATScreeningForm
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

        #region Windows Form Designer generated obsCode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_withoutpercent = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox_invert = new System.Windows.Forms.CheckBox();
            this.textBox_o_slps = new System.Windows.Forms.TextBox();
            this.textBox_mp2 = new System.Windows.Forms.TextBox();
            this.textBox_mp1 = new System.Windows.Forms.TextBox();
            this.textBox_percent = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(330, 245);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 32);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(231, 245);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 32);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_withoutpercent);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.checkBox_invert);
            this.groupBox1.Controls.Add(this.textBox_o_slps);
            this.groupBox1.Controls.Add(this.textBox_mp2);
            this.groupBox1.Controls.Add(this.textBox_mp1);
            this.groupBox1.Controls.Add(this.textBox_percent);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(31, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(374, 218);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "筛选信息";
            // 
            // checkBox_withoutpercent
            // 
            this.checkBox_withoutpercent.AutoSize = true;
            this.checkBox_withoutpercent.Location = new System.Drawing.Point(65, 159);
            this.checkBox_withoutpercent.Name = "checkBox_withoutpercent";
            this.checkBox_withoutpercent.Size = new System.Drawing.Size(96, 16);
            this.checkBox_withoutpercent.TabIndex = 10;
            this.checkBox_withoutpercent.Text = "不考虑采集率";
            this.checkBox_withoutpercent.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(293, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "筛选说明：默认选项为“或”关系，反选即筛选剩余项";
            // 
            // checkBox_invert
            // 
            this.checkBox_invert.AutoSize = true;
            this.checkBox_invert.Location = new System.Drawing.Point(183, 159);
            this.checkBox_invert.Name = "checkBox_invert";
            this.checkBox_invert.Size = new System.Drawing.Size(72, 16);
            this.checkBox_invert.TabIndex = 8;
            this.checkBox_invert.Text = "整体反选";
            this.checkBox_invert.UseVisualStyleBackColor = true;
            // 
            // textBox_o_slps
            // 
            this.textBox_o_slps.Location = new System.Drawing.Point(183, 121);
            this.textBox_o_slps.Name = "textBox_o_slps";
            this.textBox_o_slps.Size = new System.Drawing.Size(123, 21);
            this.textBox_o_slps.TabIndex = 7;
            this.textBox_o_slps.Text = "200";
            // 
            // textBox_mp2
            // 
            this.textBox_mp2.Location = new System.Drawing.Point(183, 91);
            this.textBox_mp2.Name = "textBox_mp2";
            this.textBox_mp2.Size = new System.Drawing.Size(123, 21);
            this.textBox_mp2.TabIndex = 6;
            this.textBox_mp2.Text = "0.75";
            // 
            // textBox_mp1
            // 
            this.textBox_mp1.Location = new System.Drawing.Point(183, 61);
            this.textBox_mp1.Name = "textBox_mp1";
            this.textBox_mp1.Size = new System.Drawing.Size(123, 21);
            this.textBox_mp1.TabIndex = 5;
            this.textBox_mp1.Text = "0.5";
            // 
            // textBox_percent
            // 
            this.textBox_percent.Location = new System.Drawing.Point(183, 30);
            this.textBox_percent.Name = "textBox_percent";
            this.textBox_percent.Size = new System.Drawing.Size(123, 21);
            this.textBox_percent.TabIndex = 4;
            this.textBox_percent.Text = "95";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "周跳比（O/slps）：小于";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "L2多路径（Mp2）：大于";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "L1多路径（Mp1）：大于";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据采集率（%）：小于";
            // 
            // BATScreeningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 297);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "BATScreeningForm";
            this.Text = "BATScreeningForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_withoutpercent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox_invert;
        private System.Windows.Forms.TextBox textBox_o_slps;
        private System.Windows.Forms.TextBox textBox_mp2;
        private System.Windows.Forms.TextBox textBox_mp1;
        private System.Windows.Forms.TextBox textBox_percent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}