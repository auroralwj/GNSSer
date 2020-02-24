namespace Gnsser.Winform
{
    partial class SelectedBestBaseline
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_GetCoordSnsPathes = new System.Windows.Forms.Button();
            this.button_GetPppPathes = new System.Windows.Forms.Button();
            this.textBox_pathesOfCoordFile = new System.Windows.Forms.TextBox();
            this.textBox_PathesOfPppFile = new System.Windows.Forms.TextBox();
            this.button_PppBestBaseline = new System.Windows.Forms.Button();
            this.button_ObsFileSelected = new System.Windows.Forms.Button();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_saveTofile = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.button_solve = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SINEX文件(*.snx)|*.snx|OUT文件(*.OUT)|*.*|所有文件(*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "请选择Sinex文件";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.saveFileDialog1.Title = "保存Sinex文件";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_GetCoordSnsPathes);
            this.groupBox1.Controls.Add(this.button_GetPppPathes);
            this.groupBox1.Controls.Add(this.textBox_pathesOfCoordFile);
            this.groupBox1.Controls.Add(this.textBox_PathesOfPppFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(850, 151);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "测站坐标文件：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "PPP的SNX文件：";
            // 
            // button_GetCoordSnsPathes
            // 
            this.button_GetCoordSnsPathes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_GetCoordSnsPathes.Location = new System.Drawing.Point(782, 75);
            this.button_GetCoordSnsPathes.Name = "button_GetCoordSnsPathes";
            this.button_GetCoordSnsPathes.Size = new System.Drawing.Size(50, 23);
            this.button_GetCoordSnsPathes.TabIndex = 16;
            this.button_GetCoordSnsPathes.Text = "...";
            this.button_GetCoordSnsPathes.UseVisualStyleBackColor = true;
            this.button_GetCoordSnsPathes.Click += new System.EventHandler(this.button_GetCoordSnsPathes_Click);
            // 
            // button_GetPppPathes
            // 
            this.button_GetPppPathes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_GetPppPathes.Location = new System.Drawing.Point(782, 12);
            this.button_GetPppPathes.Name = "button_GetPppPathes";
            this.button_GetPppPathes.Size = new System.Drawing.Size(50, 23);
            this.button_GetPppPathes.TabIndex = 16;
            this.button_GetPppPathes.Text = "...";
            this.button_GetPppPathes.UseVisualStyleBackColor = true;
            this.button_GetPppPathes.Click += new System.EventHandler(this.button_GetPppPathes_Click);
            // 
            // textBox_pathesOfCoordFile
            // 
            this.textBox_pathesOfCoordFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pathesOfCoordFile.Location = new System.Drawing.Point(97, 88);
            this.textBox_pathesOfCoordFile.Multiline = true;
            this.textBox_pathesOfCoordFile.Name = "textBox_pathesOfCoordFile";
            this.textBox_pathesOfCoordFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_pathesOfCoordFile.Size = new System.Drawing.Size(666, 57);
            this.textBox_pathesOfCoordFile.TabIndex = 18;
            // 
            // textBox_PathesOfPppFile
            // 
            this.textBox_PathesOfPppFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PathesOfPppFile.Location = new System.Drawing.Point(97, 13);
            this.textBox_PathesOfPppFile.Multiline = true;
            this.textBox_PathesOfPppFile.Name = "textBox_PathesOfPppFile";
            this.textBox_PathesOfPppFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_PathesOfPppFile.Size = new System.Drawing.Size(666, 57);
            this.textBox_PathesOfPppFile.TabIndex = 18;
            // 
            // button_PppBestBaseline
            // 
            this.button_PppBestBaseline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_PppBestBaseline.Location = new System.Drawing.Point(343, 159);
            this.button_PppBestBaseline.Name = "button_PppBestBaseline";
            this.button_PppBestBaseline.Size = new System.Drawing.Size(150, 36);
            this.button_PppBestBaseline.TabIndex = 48;
            this.button_PppBestBaseline.Text = "Ppp选择最优基线(SINEX)";
            this.button_PppBestBaseline.UseVisualStyleBackColor = true;
            this.button_PppBestBaseline.Click += new System.EventHandler(this.button_PppBestBaseline_Click);
            // 
            // button_ObsFileSelected
            // 
            this.button_ObsFileSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ObsFileSelected.Location = new System.Drawing.Point(728, 159);
            this.button_ObsFileSelected.Name = "button_ObsFileSelected";
            this.button_ObsFileSelected.Size = new System.Drawing.Size(116, 36);
            this.button_ObsFileSelected.TabIndex = 47;
            this.button_ObsFileSelected.Text = "O文件选择最优基线";
            this.button_ObsFileSelected.UseVisualStyleBackColor = true;
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_info.Location = new System.Drawing.Point(12, 201);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(850, 291);
            this.textBox_info.TabIndex = 46;
            // 
            // button_saveTofile
            // 
            this.button_saveTofile.Location = new System.Drawing.Point(15, 159);
            this.button_saveTofile.Name = "button_saveTofile";
            this.button_saveTofile.Size = new System.Drawing.Size(92, 23);
            this.button_saveTofile.TabIndex = 45;
            this.button_saveTofile.Text = "导出结果";
            this.button_saveTofile.UseVisualStyleBackColor = true;
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(122, 159);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(91, 23);
            this.button_showOnMap.TabIndex = 44;
            this.button_showOnMap.Text = "地图上显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            // 
            // button_solve
            // 
            this.button_solve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solve.Location = new System.Drawing.Point(534, 159);
            this.button_solve.Name = "button_solve";
            this.button_solve.Size = new System.Drawing.Size(161, 36);
            this.button_solve.TabIndex = 42;
            this.button_solve.Text = "坐标选择最优基线(SINEX)";
            this.button_solve.UseVisualStyleBackColor = true;
            // 
            // SelectedBestBaseline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 504);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_PppBestBaseline);
            this.Controls.Add(this.button_ObsFileSelected);
            this.Controls.Add(this.textBox_info);
            this.Controls.Add(this.button_saveTofile);
            this.Controls.Add(this.button_showOnMap);
            this.Controls.Add(this.button_solve);
            this.Name = "SelectedBestBaseline";
            this.Text = "SelectedBestBaseline";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_GetCoordSnsPathes;
        private System.Windows.Forms.Button button_GetPppPathes;
        private System.Windows.Forms.TextBox textBox_pathesOfCoordFile;
        private System.Windows.Forms.TextBox textBox_PathesOfPppFile;
        private System.Windows.Forms.Button button_PppBestBaseline;
        private System.Windows.Forms.Button button_ObsFileSelected;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Button button_saveTofile;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_solve;
    }
}