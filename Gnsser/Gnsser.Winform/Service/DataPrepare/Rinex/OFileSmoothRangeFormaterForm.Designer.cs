namespace Gnsser.Winform
{
    partial class OFileSmoothRangeFormaterForm
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
            this.tabPage0.Size = new System.Drawing.Size(740, 243);
            // 
            // panel4
            // 
            this.panel4.Size = new System.Drawing.Size(734, 123);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(734, 101);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(740, 129);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(481, 0);
            // 
            // OFileSmoothRangeFormaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 500);
            this.IsShowProgressBar = true;
            this.Name = "OFileSmoothRangeFormaterForm";
            this.Text = "格式化独立测站观测文件";
            this.Load += new System.EventHandler(this.IntegralGnssFileSolveForm_Load);
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}