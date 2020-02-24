namespace Gnsser.Winform.Service.Adjustment
{
    partial class GnssNetworkRobustBayes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GnssNetworkRobustBayes));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_getprioriweeksolutionfile = new System.Windows.Forms.Button();
            this.textBox_PrioriWeekSolutionFilePath = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button_getdaysolutionfilepath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_DailySolutionFilepath = new System.Windows.Forms.TextBox();
            this.textBox_knownpoint = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_getweeksolutionfilepath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_WeekSolutionFilepath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button_Bayes = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_LSAdjustment = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_Robust = new System.Windows.Forms.Button();
            this.button_robustbayes = new System.Windows.Forms.Button();
            this.textBox_eps = new System.Windows.Forms.TextBox();
            this.textBox_k1 = new System.Windows.Forms.TextBox();
            this.textBox_k0 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_getprioriweeksolutionfile);
            this.groupBox1.Controls.Add(this.textBox_PrioriWeekSolutionFilePath);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.button_getdaysolutionfilepath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_DailySolutionFilepath);
            this.groupBox1.Controls.Add(this.textBox_knownpoint);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button_getweeksolutionfilepath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_WeekSolutionFilepath);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1471, 279);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "日/周解文件";
            // 
            // button_getprioriweeksolutionfile
            // 
            this.button_getprioriweeksolutionfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getprioriweeksolutionfile.Location = new System.Drawing.Point(1322, 202);
            this.button_getprioriweeksolutionfile.Name = "button_getprioriweeksolutionfile";
            this.button_getprioriweeksolutionfile.Size = new System.Drawing.Size(66, 23);
            this.button_getprioriweeksolutionfile.TabIndex = 10;
            this.button_getprioriweeksolutionfile.Text = "…";
            this.button_getprioriweeksolutionfile.UseVisualStyleBackColor = true;
            this.button_getprioriweeksolutionfile.Click += new System.EventHandler(this.button_getprioriweeksolutionfile_Click);
            // 
            // textBox_PrioriWeekSolutionFilePath
            // 
            this.textBox_PrioriWeekSolutionFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PrioriWeekSolutionFilePath.Location = new System.Drawing.Point(133, 203);
            this.textBox_PrioriWeekSolutionFilePath.Multiline = true;
            this.textBox_PrioriWeekSolutionFilePath.Name = "textBox_PrioriWeekSolutionFilePath";
            this.textBox_PrioriWeekSolutionFilePath.Size = new System.Drawing.Size(1104, 21);
            this.textBox_PrioriWeekSolutionFilePath.TabIndex = 9;
            this.textBox_PrioriWeekSolutionFilePath.Text = "E:\\VS_Pro\\Test\\Robust_Bayes\\1721\\igs12P1720.snx";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 207);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "先验信息(周解)：";
            // 
            // button_getdaysolutionfilepath
            // 
            this.button_getdaysolutionfilepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getdaysolutionfilepath.Location = new System.Drawing.Point(1322, 110);
            this.button_getdaysolutionfilepath.Name = "button_getdaysolutionfilepath";
            this.button_getdaysolutionfilepath.Size = new System.Drawing.Size(66, 42);
            this.button_getdaysolutionfilepath.TabIndex = 7;
            this.button_getdaysolutionfilepath.Text = "…";
            this.button_getdaysolutionfilepath.UseVisualStyleBackColor = true;
            this.button_getdaysolutionfilepath.Click += new System.EventHandler(this.button_getrijiefilepath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "输入日解文件：";
            // 
            // textBox_DailySolutionFilepath
            // 
            this.textBox_DailySolutionFilepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_DailySolutionFilepath.Location = new System.Drawing.Point(133, 79);
            this.textBox_DailySolutionFilepath.Multiline = true;
            this.textBox_DailySolutionFilepath.Name = "textBox_DailySolutionFilepath";
            this.textBox_DailySolutionFilepath.Size = new System.Drawing.Size(1104, 108);
            this.textBox_DailySolutionFilepath.TabIndex = 5;
            this.textBox_DailySolutionFilepath.Text = resources.GetString("textBox_DailySolutionFilepath.Text");
            // 
            // textBox_knownpoint
            // 
            this.textBox_knownpoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_knownpoint.Location = new System.Drawing.Point(133, 245);
            this.textBox_knownpoint.Name = "textBox_knownpoint";
            this.textBox_knownpoint.Size = new System.Drawing.Size(1104, 21);
            this.textBox_knownpoint.TabIndex = 4;
            this.textBox_knownpoint.Text = "ABMF";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 248);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "已知点：";
            // 
            // button_getweeksolutionfilepath
            // 
            this.button_getweeksolutionfilepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getweeksolutionfilepath.Location = new System.Drawing.Point(1322, 33);
            this.button_getweeksolutionfilepath.Name = "button_getweeksolutionfilepath";
            this.button_getweeksolutionfilepath.Size = new System.Drawing.Size(66, 23);
            this.button_getweeksolutionfilepath.TabIndex = 2;
            this.button_getweeksolutionfilepath.Text = "…";
            this.button_getweeksolutionfilepath.UseVisualStyleBackColor = true;
            this.button_getweeksolutionfilepath.Click += new System.EventHandler(this.button_getfilepath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 36);
            this.label1.TabIndex = 1;
            this.label1.Text = "输入周解文件:\r\n\r\n(坐标真值)";
            // 
            // textBox_WeekSolutionFilepath
            // 
            this.textBox_WeekSolutionFilepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_WeekSolutionFilepath.Location = new System.Drawing.Point(133, 19);
            this.textBox_WeekSolutionFilepath.Multiline = true;
            this.textBox_WeekSolutionFilepath.Name = "textBox_WeekSolutionFilepath";
            this.textBox_WeekSolutionFilepath.Size = new System.Drawing.Size(1104, 46);
            this.textBox_WeekSolutionFilepath.TabIndex = 0;
            this.textBox_WeekSolutionFilepath.Text = "E:\\VS_Pro\\Test\\Robust_Bayes\\1721\\igs13P1721.snx";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            this.openFileDialog2.Multiselect = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Location = new System.Drawing.Point(12, 308);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1471, 99);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "最小二乘平差";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.button_Bayes);
            this.groupBox6.Location = new System.Drawing.Point(732, 20);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(702, 73);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "贝叶斯估计";
            // 
            // button_Bayes
            // 
            this.button_Bayes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Bayes.Location = new System.Drawing.Point(246, 30);
            this.button_Bayes.Name = "button_Bayes";
            this.button_Bayes.Size = new System.Drawing.Size(264, 23);
            this.button_Bayes.TabIndex = 0;
            this.button_Bayes.Text = "Bayes";
            this.button_Bayes.UseVisualStyleBackColor = true;
            this.button_Bayes.Click += new System.EventHandler(this.button_Bayes_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.Controls.Add(this.button_LSAdjustment);
            this.groupBox5.Location = new System.Drawing.Point(38, 20);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(688, 73);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "最小二乘平差";
            // 
            // button_LSAdjustment
            // 
            this.button_LSAdjustment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LSAdjustment.Location = new System.Drawing.Point(210, 30);
            this.button_LSAdjustment.Name = "button_LSAdjustment";
            this.button_LSAdjustment.Size = new System.Drawing.Size(283, 23);
            this.button_LSAdjustment.TabIndex = 0;
            this.button_LSAdjustment.Text = "LS_Adjustment";
            this.button_LSAdjustment.UseVisualStyleBackColor = true;
            this.button_LSAdjustment.Click += new System.EventHandler(this.button_LSAdjustment_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button_Robust);
            this.groupBox3.Controls.Add(this.button_robustbayes);
            this.groupBox3.Controls.Add(this.textBox_eps);
            this.groupBox3.Controls.Add(this.textBox_k1);
            this.groupBox3.Controls.Add(this.textBox_k0);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(12, 427);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1471, 71);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "抗差贝叶斯估计";
            // 
            // button_Robust
            // 
            this.button_Robust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Robust.Location = new System.Drawing.Point(1052, 26);
            this.button_Robust.Name = "button_Robust";
            this.button_Robust.Size = new System.Drawing.Size(114, 23);
            this.button_Robust.TabIndex = 9;
            this.button_Robust.Text = "Robust";
            this.button_Robust.UseVisualStyleBackColor = true;
            this.button_Robust.Click += new System.EventHandler(this.button_Robust_Click);
            // 
            // button_robustbayes
            // 
            this.button_robustbayes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_robustbayes.Location = new System.Drawing.Point(1234, 27);
            this.button_robustbayes.Name = "button_robustbayes";
            this.button_robustbayes.Size = new System.Drawing.Size(119, 23);
            this.button_robustbayes.TabIndex = 7;
            this.button_robustbayes.Text = "Robust_Bayes";
            this.button_robustbayes.UseVisualStyleBackColor = true;
            this.button_robustbayes.Click += new System.EventHandler(this.button_robustbayes_Click);
            // 
            // textBox_eps
            // 
            this.textBox_eps.Location = new System.Drawing.Point(599, 29);
            this.textBox_eps.Name = "textBox_eps";
            this.textBox_eps.Size = new System.Drawing.Size(87, 21);
            this.textBox_eps.TabIndex = 6;
            this.textBox_eps.Text = "0.005";
            // 
            // textBox_k1
            // 
            this.textBox_k1.Location = new System.Drawing.Point(417, 29);
            this.textBox_k1.Name = "textBox_k1";
            this.textBox_k1.Size = new System.Drawing.Size(95, 21);
            this.textBox_k1.TabIndex = 5;
            this.textBox_k1.Text = "4.5";
            // 
            // textBox_k0
            // 
            this.textBox_k0.Location = new System.Drawing.Point(216, 29);
            this.textBox_k0.Name = "textBox_k0";
            this.textBox_k0.Size = new System.Drawing.Size(82, 21);
            this.textBox_k0.TabIndex = 4;
            this.textBox_k0.Text = "1.0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(568, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "eps:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(391, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "k1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "k0:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "抗差因子:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.richTextBox2);
            this.groupBox4.Location = new System.Drawing.Point(12, 513);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1471, 72);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "平差结果输出";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox2.Location = new System.Drawing.Point(6, 14);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(1462, 57);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            this.openFileDialog3.Multiselect = true;
            // 
            // GnssNetworkRobustBayes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1495, 599);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GnssNetworkRobustBayes";
            this.Text = "GnssNetworkRobustBayes";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_WeekSolutionFilepath;
        private System.Windows.Forms.Button button_getweeksolutionfilepath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_DailySolutionFilepath;
        private System.Windows.Forms.TextBox textBox_knownpoint;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_getdaysolutionfilepath;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_LSAdjustment;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_eps;
        private System.Windows.Forms.TextBox textBox_k1;
        private System.Windows.Forms.TextBox textBox_k0;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_robustbayes;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button_Bayes;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button_Robust;
        private System.Windows.Forms.TextBox textBox_PrioriWeekSolutionFilePath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_getprioriweeksolutionfile;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}