namespace Gnsser.Winform
{
    partial class ParalleledFileForm
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
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(651, 182);
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(645, 123);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(645, 123);
            // 
            // panel4
            // 
            this.panel4.Size = new System.Drawing.Size(645, 123);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(645, 101);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(651, 129);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(392, 0);
            // 
            // ParalleledFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 439);
            this.IsShowProgressBar = true;
            this.Name = "ParalleledFileForm";
            this.Text = "并行观测文件执行器";
            this.Load += new System.EventHandler(this.IntegralGnssFileSolveForm_Load);
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}