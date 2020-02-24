namespace Gnsser.Winform
{
    partial class Sp3SourceConfigForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_beidou = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_gps = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_save = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.textBox_glonass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_galieo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_ephemerisDir = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_IgsEphemerisUrlModel = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxurldircory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_beidou
            // 
            this.textBox_beidou.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_beidou.Location = new System.Drawing.Point(101, 202);
            this.textBox_beidou.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_beidou.Name = "textBox_beidou";
            this.textBox_beidou.Size = new System.Drawing.Size(423, 21);
            this.textBox_beidou.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 204);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Beidou:";
            // 
            // textBox_gps
            // 
            this.textBox_gps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_gps.Location = new System.Drawing.Point(101, 226);
            this.textBox_gps.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_gps.Name = "textBox_gps";
            this.textBox_gps.Size = new System.Drawing.Size(423, 21);
            this.textBox_gps.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 229);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "GPS:";
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(12, 10);
            this.button_save.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(63, 20);
            this.button_save.TabIndex = 2;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(88, 10);
            this.button_reset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(63, 20);
            this.button_reset.TabIndex = 2;
            this.button_reset.Text = "重置";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // textBox_glonass
            // 
            this.textBox_glonass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_glonass.Location = new System.Drawing.Point(101, 251);
            this.textBox_glonass.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_glonass.Name = "textBox_glonass";
            this.textBox_glonass.Size = new System.Drawing.Size(423, 21);
            this.textBox_glonass.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 254);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "GLONASS:";
            // 
            // textBox_galieo
            // 
            this.textBox_galieo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_galieo.Location = new System.Drawing.Point(101, 276);
            this.textBox_galieo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_galieo.Name = "textBox_galieo";
            this.textBox_galieo.Size = new System.Drawing.Size(423, 21);
            this.textBox_galieo.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 278);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Galieo:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(99, 309);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(179, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "提示：星历数据源以逗号\",\"分开";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 177);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "本地数据源目录:";
            // 
            // textBox_ephemerisDir
            // 
            this.textBox_ephemerisDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ephemerisDir.Location = new System.Drawing.Point(101, 177);
            this.textBox_ephemerisDir.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_ephemerisDir.Name = "textBox_ephemerisDir";
            this.textBox_ephemerisDir.Size = new System.Drawing.Size(423, 21);
            this.textBox_ephemerisDir.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 45);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "IGS地址模型:";
            // 
            // textBox_IgsEphemerisUrlModel
            // 
            this.textBox_IgsEphemerisUrlModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_IgsEphemerisUrlModel.Location = new System.Drawing.Point(101, 45);
            this.textBox_IgsEphemerisUrlModel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_IgsEphemerisUrlModel.Multiline = true;
            this.textBox_IgsEphemerisUrlModel.Name = "textBox_IgsEphemerisUrlModel";
            this.textBox_IgsEphemerisUrlModel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_IgsEphemerisUrlModel.Size = new System.Drawing.Size(423, 64);
            this.textBox_IgsEphemerisUrlModel.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 113);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "IGS地址目录:";
            // 
            // textBoxurldircory
            // 
            this.textBoxurldircory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxurldircory.Location = new System.Drawing.Point(101, 113);
            this.textBoxurldircory.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxurldircory.Multiline = true;
            this.textBoxurldircory.Name = "textBoxurldircory";
            this.textBoxurldircory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxurldircory.Size = new System.Drawing.Size(423, 64);
            this.textBoxurldircory.TabIndex = 4;
            // 
            // Sp3SourceConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 330);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxurldircory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_galieo);
            this.Controls.Add(this.textBox_glonass);
            this.Controls.Add(this.textBox_gps);
            this.Controls.Add(this.textBox_IgsEphemerisUrlModel);
            this.Controls.Add(this.textBox_ephemerisDir);
            this.Controls.Add(this.textBox_beidou);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Sp3SourceConfigForm";
            this.Text = "精密星历数据源设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_beidou;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_gps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.TextBox textBox_glonass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_galieo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_ephemerisDir;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_IgsEphemerisUrlModel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxurldircory;

    }
}