namespace Gnsser.Winform
{
    partial class MultiGnssFileFormateForm
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
            this.rinexObsFileFormatTypeControl1 = new Gnsser.Winform.Controls.RinexObsFileFormatTypeControl();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(599, 217);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rinexObsFileFormatTypeControl1);
            this.panel4.Controls.Add(this.parallelConfigControl1);
            this.panel4.Size = new System.Drawing.Size(584, 82);
            this.panel4.Controls.SetChildIndex(this.parallelConfigControl1, 0);
            this.panel4.Controls.SetChildIndex(this.rinexObsFileFormatTypeControl1, 0);
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
            // rinexObsFileFormatTypeControl1
            // 
            this.rinexObsFileFormatTypeControl1.CurrentdType = Gnsser.RinexObsFileFormatType.单站单历元;
            this.rinexObsFileFormatTypeControl1.Location = new System.Drawing.Point(17, 41);
            this.rinexObsFileFormatTypeControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rinexObsFileFormatTypeControl1.Name = "rinexObsFileFormatTypeControl1";
            this.rinexObsFileFormatTypeControl1.Size = new System.Drawing.Size(553, 41);
            this.rinexObsFileFormatTypeControl1.TabIndex = 44;
            // 
            // MultiGnssFileFormateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 439);
            this.IsShowProgressBar = true;
            this.Name = "MultiGnssFileFormateForm";
            this.Text = "格式化独立测站观测文件";
            this.Load += new System.EventHandler(this.MultiGnssFileFormateForm_Load);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.ParallelConfigControl parallelConfigControl1;
        private Controls.RinexObsFileFormatTypeControl rinexObsFileFormatTypeControl1;
    }
}