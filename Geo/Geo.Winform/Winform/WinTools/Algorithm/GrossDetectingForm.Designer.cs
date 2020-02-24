namespace Geo.WinTools
{
    partial class GrossDetectingForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_infiles = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_gross = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.button_run = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_loopDetect = new System.Windows.Forms.CheckBox();
            this.textBox_rms = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_fileProcess = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_infiles);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(245, 471);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // textBox_infiles
            // 
            this.textBox_infiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_infiles.Location = new System.Drawing.Point(3, 20);
            this.textBox_infiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_infiles.Multiline = true;
            this.textBox_infiles.Name = "textBox_infiles";
            this.textBox_infiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_infiles.Size = new System.Drawing.Size(239, 449);
            this.textBox_infiles.TabIndex = 1;
            this.textBox_infiles.Text = "1\r\n2\r\n4\r\n12\r\n2\r\n2\r\n7\r\n8\r\n6\r\n8\r\n45\r\n1\r\n5\r\n7\r\n4";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.splitContainer2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(629, 471);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(4, 22);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer2.Size = new System.Drawing.Size(621, 445);
            this.splitContainer2.SplitterDistance = 322;
            this.splitContainer2.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_gross);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(322, 445);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "粗差";
            // 
            // textBox_gross
            // 
            this.textBox_gross.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_gross.Location = new System.Drawing.Point(3, 21);
            this.textBox_gross.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_gross.Multiline = true;
            this.textBox_gross.Name = "textBox_gross";
            this.textBox_gross.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_gross.Size = new System.Drawing.Size(316, 421);
            this.textBox_gross.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox_result);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(295, 445);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "剔除粗差后的数据";
            // 
            // textBox_result
            // 
            this.textBox_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_result.Location = new System.Drawing.Point(3, 21);
            this.textBox_result.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_result.Size = new System.Drawing.Size(289, 421);
            this.textBox_result.TabIndex = 2;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(653, 19);
            this.button_run.Margin = new System.Windows.Forms.Padding(4);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(100, 44);
            this.button_run.TabIndex = 3;
            this.button_run.Text = "探测";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "观测文件|*.*O|所有文件|*.*";
            this.openFileDialog1.Multiselect = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 64);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(878, 471);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.checkBox_loopDetect);
            this.groupBox3.Controls.Add(this.textBox_rms);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(15, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(612, 46);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "设置";
            // 
            // checkBox_loopDetect
            // 
            this.checkBox_loopDetect.AutoSize = true;
            this.checkBox_loopDetect.Location = new System.Drawing.Point(347, 16);
            this.checkBox_loopDetect.Name = "checkBox_loopDetect";
            this.checkBox_loopDetect.Size = new System.Drawing.Size(89, 19);
            this.checkBox_loopDetect.TabIndex = 2;
            this.checkBox_loopDetect.Text = "循环探测";
            this.checkBox_loopDetect.UseVisualStyleBackColor = true;
            // 
            // textBox_rms
            // 
            this.textBox_rms.Location = new System.Drawing.Point(147, 15);
            this.textBox_rms.Name = "textBox_rms";
            this.textBox_rms.Size = new System.Drawing.Size(162, 25);
            this.textBox_rms.TabIndex = 1;
            this.textBox_rms.Text = "3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "中误差倍数：";
            // 
            // button_fileProcess
            // 
            this.button_fileProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_fileProcess.Location = new System.Drawing.Point(779, 19);
            this.button_fileProcess.Name = "button_fileProcess";
            this.button_fileProcess.Size = new System.Drawing.Size(91, 44);
            this.button_fileProcess.TabIndex = 6;
            this.button_fileProcess.Text = "文件处理";
            this.button_fileProcess.UseVisualStyleBackColor = true;
            this.button_fileProcess.Click += new System.EventHandler(this.button_fileProcess_Click);
            // 
            // GrossDetectingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 547);
            this.Controls.Add(this.button_fileProcess);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button_run);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GrossDetectingForm";
            this.Text = "粗差探测";
            this.Load += new System.EventHandler(this.QualityCheckForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_gross;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox_infiles;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_rms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_loopDetect;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Button button_fileProcess;
    }
}