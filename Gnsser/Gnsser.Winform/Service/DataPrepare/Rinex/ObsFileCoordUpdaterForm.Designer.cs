namespace Gnsser.Winform
{
    partial class ObsFileCoordUpdaterForm
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
            this.parallelConfigControl1 = new Geo.Winform.Controls.ParallelConfigControl();
            this.fileOpenControl_coordFile = new Geo.Winform.Controls.FileOpenControl();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(599, 171);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.fileOpenControl_coordFile);
            this.panel4.Controls.Add(this.parallelConfigControl1);
            this.panel4.Size = new System.Drawing.Size(593, 120);
            this.panel4.Controls.SetChildIndex(this.parallelConfigControl1, 0);
            this.panel4.Controls.SetChildIndex(this.fileOpenControl_coordFile, 0);
            // 
            // parallelConfigControl1
            // 
            this.parallelConfigControl1.EnableParallel = true;
            this.parallelConfigControl1.Location = new System.Drawing.Point(110, 9);
            this.parallelConfigControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.parallelConfigControl1.Name = "parallelConfigControl1";
            this.parallelConfigControl1.Size = new System.Drawing.Size(254, 26);
            this.parallelConfigControl1.TabIndex = 43;
            // 
            // fileOpenControl_coordFile
            // 
            this.fileOpenControl_coordFile.AllowDrop = true;
            this.fileOpenControl_coordFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_coordFile.FilePath = "";
            this.fileOpenControl_coordFile.FilePathes = new string[0];
            this.fileOpenControl_coordFile.Filter = "SNEX文件；坐标文件|*.snx;*.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_coordFile.IsMultiSelect = false;
            this.fileOpenControl_coordFile.LabelName = "坐标服务文件：";
            this.fileOpenControl_coordFile.Location = new System.Drawing.Point(17, 41);
            this.fileOpenControl_coordFile.Name = "fileOpenControl_coordFile";
            this.fileOpenControl_coordFile.Size = new System.Drawing.Size(470, 22);
            this.fileOpenControl_coordFile.TabIndex = 57;
            // 
            // ObsFileCoordUpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 439);
            this.IsShowProgressBar = true;
            this.Name = "ObsFileCoordUpdaterForm";
            this.Text = "更新观测文件坐标";
            this.Load += new System.EventHandler(this.IntegralGnssFileSolveForm_Load);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.ParallelConfigControl parallelConfigControl1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_coordFile;
    }
}