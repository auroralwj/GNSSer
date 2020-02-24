namespace Gnsser.Winform
{
    partial class GnssNetworkAdjustment
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
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_SaveTofile = new System.Windows.Forms.Button();
            this.button_ShowOnMap = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_eps = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_k1 = new System.Windows.Forms.TextBox();
            this.textBox_k0 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_readAdjust = new System.Windows.Forms.Button();
            this.button_LSAdjust = new System.Windows.Forms.Button();
            this.textBox_KnowPoints = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_getFilePath = new System.Windows.Forms.Button();
            this.textBox_Path = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button_FreeCoGPS = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_info.Location = new System.Drawing.Point(12, 231);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(812, 125);
            this.textBox_info.TabIndex = 47;
            // 
            // button_SaveTofile
            // 
            this.button_SaveTofile.Location = new System.Drawing.Point(10, 199);
            this.button_SaveTofile.Name = "button_SaveTofile";
            this.button_SaveTofile.Size = new System.Drawing.Size(92, 23);
            this.button_SaveTofile.TabIndex = 46;
            this.button_SaveTofile.Text = "导出结果";
            this.button_SaveTofile.UseVisualStyleBackColor = true;
            this.button_SaveTofile.Click += new System.EventHandler(this.button_saveTofile_Click);
            // 
            // button_ShowOnMap
            // 
            this.button_ShowOnMap.Location = new System.Drawing.Point(117, 199);
            this.button_ShowOnMap.Name = "button_ShowOnMap";
            this.button_ShowOnMap.Size = new System.Drawing.Size(91, 23);
            this.button_ShowOnMap.TabIndex = 45;
            this.button_ShowOnMap.Text = "地图上显示";
            this.button_ShowOnMap.UseVisualStyleBackColor = true;
            this.button_ShowOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_FreeCoGPS);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button_LSAdjust);
            this.groupBox1.Controls.Add(this.textBox_KnowPoints);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_getFilePath);
            this.groupBox1.Controls.Add(this.textBox_Path);
            this.groupBox1.Location = new System.Drawing.Point(10, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(805, 186);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.textBox_eps);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox_k1);
            this.groupBox2.Controls.Add(this.textBox_k0);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.button_readAdjust);
            this.groupBox2.Location = new System.Drawing.Point(3, 119);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(799, 61);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Robust";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(625, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 36);
            this.button1.TabIndex = 29;
            this.button1.Text = "模拟抗差";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_eps
            // 
            this.textBox_eps.Location = new System.Drawing.Point(491, 23);
            this.textBox_eps.Name = "textBox_eps";
            this.textBox_eps.Size = new System.Drawing.Size(109, 21);
            this.textBox_eps.TabIndex = 28;
            this.textBox_eps.Text = "0.001";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(447, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 27;
            this.label6.Text = "eps：";
            // 
            // textBox_k1
            // 
            this.textBox_k1.Location = new System.Drawing.Point(299, 23);
            this.textBox_k1.Name = "textBox_k1";
            this.textBox_k1.Size = new System.Drawing.Size(109, 21);
            this.textBox_k1.TabIndex = 26;
            this.textBox_k1.Text = "4.5";
            // 
            // textBox_k0
            // 
            this.textBox_k0.Location = new System.Drawing.Point(125, 22);
            this.textBox_k0.Name = "textBox_k0";
            this.textBox_k0.Size = new System.Drawing.Size(110, 21);
            this.textBox_k0.TabIndex = 25;
            this.textBox_k0.Text = "1.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(255, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "k1：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(91, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "k0：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "抗差因子：";
            // 
            // button_readAdjust
            // 
            this.button_readAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readAdjust.Location = new System.Drawing.Point(718, 11);
            this.button_readAdjust.Name = "button_readAdjust";
            this.button_readAdjust.Size = new System.Drawing.Size(78, 36);
            this.button_readAdjust.TabIndex = 19;
            this.button_readAdjust.Text = "抗差估计";
            this.button_readAdjust.UseVisualStyleBackColor = true;
            this.button_readAdjust.Click += new System.EventHandler(this.button_readAdjust_Click);
            // 
            // button_LSAdjust
            // 
            this.button_LSAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LSAdjust.Location = new System.Drawing.Point(721, 50);
            this.button_LSAdjust.Name = "button_LSAdjust";
            this.button_LSAdjust.Size = new System.Drawing.Size(78, 36);
            this.button_LSAdjust.TabIndex = 27;
            this.button_LSAdjust.Text = "LS估计";
            this.button_LSAdjust.UseVisualStyleBackColor = true;
            this.button_LSAdjust.Click += new System.EventHandler(this.button_LSAdjust_Click);
            // 
            // textBox_KnowPoints
            // 
            this.textBox_KnowPoints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_KnowPoints.Location = new System.Drawing.Point(96, 78);
            this.textBox_KnowPoints.Name = "textBox_KnowPoints";
            this.textBox_KnowPoints.Size = new System.Drawing.Size(574, 21);
            this.textBox_KnowPoints.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "已知点信息：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "文件：";
            // 
            // button_getFilePath
            // 
            this.button_getFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getFilePath.Location = new System.Drawing.Point(737, 21);
            this.button_getFilePath.Name = "button_getFilePath";
            this.button_getFilePath.Size = new System.Drawing.Size(50, 23);
            this.button_getFilePath.TabIndex = 16;
            this.button_getFilePath.Text = "...";
            this.button_getFilePath.UseVisualStyleBackColor = true;
            this.button_getFilePath.Click += new System.EventHandler(this.button_getPath_Click);
            // 
            // textBox_Path
            // 
            this.textBox_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Path.Location = new System.Drawing.Point(97, 13);
            this.textBox_Path.Multiline = true;
            this.textBox_Path.Name = "textBox_Path";
            this.textBox_Path.Size = new System.Drawing.Size(621, 46);
            this.textBox_Path.TabIndex = 18;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SINEX文件(*.snx)|*.snx|所有文件(*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "请选择Sinex文件";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.saveFileDialog1.Title = "保存Sinex文件";
            // 
            // button_FreeCoGPS
            // 
            this.button_FreeCoGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_FreeCoGPS.Location = new System.Drawing.Point(721, 88);
            this.button_FreeCoGPS.Name = "button_FreeCoGPS";
            this.button_FreeCoGPS.Size = new System.Drawing.Size(78, 36);
            this.button_FreeCoGPS.TabIndex = 29;
            this.button_FreeCoGPS.Text = "自由网平差";
            this.button_FreeCoGPS.UseVisualStyleBackColor = true;
            this.button_FreeCoGPS.Click += new System.EventHandler(this.button_FreeCoGPS_Click);
            // 
            // GnssNetworkAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 431);
            this.Controls.Add(this.textBox_info);
            this.Controls.Add(this.button_SaveTofile);
            this.Controls.Add(this.button_ShowOnMap);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "GnssNetworkAdjustment";
            this.Text = "网平差";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Button button_SaveTofile;
        private System.Windows.Forms.Button button_ShowOnMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_k1;
        private System.Windows.Forms.TextBox textBox_k0;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_KnowPoints;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_getFilePath;
        private System.Windows.Forms.TextBox textBox_Path;
        private System.Windows.Forms.Button button_readAdjust;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button_LSAdjust;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_eps;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_FreeCoGPS;
    }
}