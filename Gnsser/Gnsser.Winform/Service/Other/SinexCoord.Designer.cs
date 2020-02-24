namespace Gnsser.Winform.Other
{
    partial class SinexCoord
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_sinexFile = new System.Windows.Forms.TextBox();
            this.button_extractcoords = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_getfilepath = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_uotputpath = new System.Windows.Forms.TextBox();
            this.button_outputDirec = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_GnsserFile = new System.Windows.Forms.TextBox();
            this.button_GnsserResult = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.button_CommonPoint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "SINEX文件:";
            // 
            // textBox_sinexFile
            // 
            this.textBox_sinexFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sinexFile.Location = new System.Drawing.Point(97, 26);
            this.textBox_sinexFile.Name = "textBox_sinexFile";
            this.textBox_sinexFile.Size = new System.Drawing.Size(275, 21);
            this.textBox_sinexFile.TabIndex = 1;
            // 
            // button_extractcoords
            // 
            this.button_extractcoords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_extractcoords.Location = new System.Drawing.Point(465, 50);
            this.button_extractcoords.Name = "button_extractcoords";
            this.button_extractcoords.Size = new System.Drawing.Size(75, 23);
            this.button_extractcoords.TabIndex = 2;
            this.button_extractcoords.Text = "提取坐标";
            this.button_extractcoords.UseVisualStyleBackColor = true;
            this.button_extractcoords.Click += new System.EventHandler(this.button_extractcoords_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "SINEX文件|*.*SNX;*.*snx|所有文件|*.*";
            // 
            // button_getfilepath
            // 
            this.button_getfilepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getfilepath.Location = new System.Drawing.Point(385, 26);
            this.button_getfilepath.Name = "button_getfilepath";
            this.button_getfilepath.Size = new System.Drawing.Size(75, 23);
            this.button_getfilepath.TabIndex = 3;
            this.button_getfilepath.Text = "…";
            this.button_getfilepath.UseVisualStyleBackColor = true;
            this.button_getfilepath.Click += new System.EventHandler(this.button_getfilepath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "输出目录:";
            // 
            // textBox_uotputpath
            // 
            this.textBox_uotputpath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_uotputpath.Location = new System.Drawing.Point(97, 78);
            this.textBox_uotputpath.Name = "textBox_uotputpath";
            this.textBox_uotputpath.Size = new System.Drawing.Size(275, 21);
            this.textBox_uotputpath.TabIndex = 5;
            // 
            // button_outputDirec
            // 
            this.button_outputDirec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_outputDirec.Location = new System.Drawing.Point(385, 76);
            this.button_outputDirec.Name = "button_outputDirec";
            this.button_outputDirec.Size = new System.Drawing.Size(75, 23);
            this.button_outputDirec.TabIndex = 6;
            this.button_outputDirec.Text = "…";
            this.button_outputDirec.UseVisualStyleBackColor = true;
            this.button_outputDirec.Click += new System.EventHandler(this.button_outputDirec_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Gnsser结果:";
            // 
            // textBox_GnsserFile
            // 
            this.textBox_GnsserFile.Location = new System.Drawing.Point(97, 127);
            this.textBox_GnsserFile.Name = "textBox_GnsserFile";
            this.textBox_GnsserFile.Size = new System.Drawing.Size(275, 21);
            this.textBox_GnsserFile.TabIndex = 8;
            // 
            // button_GnsserResult
            // 
            this.button_GnsserResult.Location = new System.Drawing.Point(385, 127);
            this.button_GnsserResult.Name = "button_GnsserResult";
            this.button_GnsserResult.Size = new System.Drawing.Size(75, 23);
            this.button_GnsserResult.TabIndex = 9;
            this.button_GnsserResult.Text = "…";
            this.button_GnsserResult.UseVisualStyleBackColor = true;
            this.button_GnsserResult.Click += new System.EventHandler(this.button_GnsserResult_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            this.openFileDialog2.Filter = "Gnsser结果|*.*txt|所有文件|*.*";
            // 
            // button_CommonPoint
            // 
            this.button_CommonPoint.Location = new System.Drawing.Point(466, 127);
            this.button_CommonPoint.Name = "button_CommonPoint";
            this.button_CommonPoint.Size = new System.Drawing.Size(75, 23);
            this.button_CommonPoint.TabIndex = 10;
            this.button_CommonPoint.Text = "共同点坐标";
            this.button_CommonPoint.UseVisualStyleBackColor = true;
            this.button_CommonPoint.Click += new System.EventHandler(this.button_CommonPoint_Click);
            // 
            // SinexCoord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 174);
            this.Controls.Add(this.button_CommonPoint);
            this.Controls.Add(this.button_GnsserResult);
            this.Controls.Add(this.textBox_GnsserFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_outputDirec);
            this.Controls.Add(this.textBox_uotputpath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_getfilepath);
            this.Controls.Add(this.button_extractcoords);
            this.Controls.Add(this.textBox_sinexFile);
            this.Controls.Add(this.label1);
            this.Name = "SinexCoord";
            this.Text = "SinexCoord";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_sinexFile;
        private System.Windows.Forms.Button button_extractcoords;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_getfilepath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_uotputpath;
        private System.Windows.Forms.Button button_outputDirec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_GnsserFile;
        private System.Windows.Forms.Button button_GnsserResult;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button button_CommonPoint;
    }
}