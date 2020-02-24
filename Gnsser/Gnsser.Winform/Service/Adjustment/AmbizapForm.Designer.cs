namespace Gnsser.Winform
{
    partial class AmbizapForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_setBaseLinePathes = new System.Windows.Forms.Button();
            this.button_setPppPathes = new System.Windows.Forms.Button();
            this.textBox_pathesOfBaseLine = new System.Windows.Forms.TextBox();
            this.textBox_PathesOfPPP = new System.Windows.Forms.TextBox();
            this.button_solve = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.button_saveTofile = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_outSolve = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_setBaseLinePathes);
            this.groupBox1.Controls.Add(this.button_setPppPathes);
            this.groupBox1.Controls.Add(this.textBox_pathesOfBaseLine);
            this.groupBox1.Controls.Add(this.textBox_PathesOfPPP);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(961, 189);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 100);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "基线解算结果：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "单点定位结果：";
            // 
            // button_setBaseLinePathes
            // 
            this.button_setBaseLinePathes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setBaseLinePathes.Location = new System.Drawing.Point(871, 94);
            this.button_setBaseLinePathes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_setBaseLinePathes.Name = "button_setBaseLinePathes";
            this.button_setBaseLinePathes.Size = new System.Drawing.Size(67, 29);
            this.button_setBaseLinePathes.TabIndex = 16;
            this.button_setBaseLinePathes.Text = "...";
            this.button_setBaseLinePathes.UseVisualStyleBackColor = true;
            this.button_setBaseLinePathes.Click += new System.EventHandler(this.button_setBaseLinePathes_Click);
            // 
            // button_setPppPathes
            // 
            this.button_setPppPathes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setPppPathes.Location = new System.Drawing.Point(871, 15);
            this.button_setPppPathes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_setPppPathes.Name = "button_setPppPathes";
            this.button_setPppPathes.Size = new System.Drawing.Size(67, 29);
            this.button_setPppPathes.TabIndex = 16;
            this.button_setPppPathes.Text = "...";
            this.button_setPppPathes.UseVisualStyleBackColor = true;
            this.button_setPppPathes.Click += new System.EventHandler(this.button_setPppPathes_Click);
            // 
            // textBox_pathesOfBaseLine
            // 
            this.textBox_pathesOfBaseLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pathesOfBaseLine.Location = new System.Drawing.Point(129, 110);
            this.textBox_pathesOfBaseLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_pathesOfBaseLine.Multiline = true;
            this.textBox_pathesOfBaseLine.Name = "textBox_pathesOfBaseLine";
            this.textBox_pathesOfBaseLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_pathesOfBaseLine.Size = new System.Drawing.Size(715, 70);
            this.textBox_pathesOfBaseLine.TabIndex = 18;
            // 
            // textBox_PathesOfPPP
            // 
            this.textBox_PathesOfPPP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PathesOfPPP.Location = new System.Drawing.Point(129, 16);
            this.textBox_PathesOfPPP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_PathesOfPPP.Multiline = true;
            this.textBox_PathesOfPPP.Name = "textBox_PathesOfPPP";
            this.textBox_PathesOfPPP.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_PathesOfPPP.Size = new System.Drawing.Size(715, 70);
            this.textBox_PathesOfPPP.TabIndex = 18;
            // 
            // button_solve
            // 
            this.button_solve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solve.Location = new System.Drawing.Point(647, 212);
            this.button_solve.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_solve.Name = "button_solve";
            this.button_solve.Size = new System.Drawing.Size(132, 45);
            this.button_solve.TabIndex = 19;
            this.button_solve.Text = "平差(SINEX)";
            this.button_solve.UseVisualStyleBackColor = true;
            this.button_solve.Click += new System.EventHandler(this.button_read_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SINEX文件(*.snx)|*.snx|OUT文件(*.OUT)|*.*|所有文件(*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "请选择Sinex文件";
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(159, 211);
            this.button_showOnMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(121, 29);
            this.button_showOnMap.TabIndex = 36;
            this.button_showOnMap.Text = "地图上显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // button_saveTofile
            // 
            this.button_saveTofile.Location = new System.Drawing.Point(16, 211);
            this.button_saveTofile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_saveTofile.Name = "button_saveTofile";
            this.button_saveTofile.Size = new System.Drawing.Size(123, 29);
            this.button_saveTofile.TabIndex = 38;
            this.button_saveTofile.Text = "导出结果";
            this.button_saveTofile.UseVisualStyleBackColor = true;
            this.button_saveTofile.Click += new System.EventHandler(this.button_saveTofile_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.saveFileDialog1.Title = "保存Sinex文件";
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_info.Location = new System.Drawing.Point(16, 278);
            this.textBox_info.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(959, 286);
            this.textBox_info.TabIndex = 39;
            // 
            // button_outSolve
            // 
            this.button_outSolve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_outSolve.Location = new System.Drawing.Point(822, 212);
            this.button_outSolve.Margin = new System.Windows.Forms.Padding(4);
            this.button_outSolve.Name = "button_outSolve";
            this.button_outSolve.Size = new System.Drawing.Size(132, 45);
            this.button_outSolve.TabIndex = 40;
            this.button_outSolve.Text = "平差(OUT)";
            this.button_outSolve.UseVisualStyleBackColor = true;
            this.button_outSolve.Click += new System.EventHandler(this.button_outSolve_Click);
            // 
            // AmbizapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 580);
            this.Controls.Add(this.button_outSolve);
            this.Controls.Add(this.textBox_info);
            this.Controls.Add(this.button_saveTofile);
            this.Controls.Add(this.button_showOnMap);
            this.Controls.Add(this.button_solve);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AmbizapForm";
            this.Text = "Ambizap基线约束平差";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_setPppPathes;
        private System.Windows.Forms.TextBox textBox_PathesOfPPP;
        private System.Windows.Forms.Button button_solve;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_saveTofile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_setBaseLinePathes;
        private System.Windows.Forms.TextBox textBox_pathesOfBaseLine;
        private System.Windows.Forms.Button button_outSolve;
    }
}