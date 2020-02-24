namespace Gnsser.Winform
{
    partial class SingleGnssFileFormateForm
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
            this.rinexObsFileFormatTypeControl1 = new Gnsser.Winform.Controls.RinexObsFileFormatTypeControl();
            this.checkBox_IsUseRangeCorrections = new System.Windows.Forms.CheckBox();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(840, 193);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBox_IsUseRangeCorrections);
            this.panel4.Controls.Add(this.rinexObsFileFormatTypeControl1);
            this.panel4.Size = new System.Drawing.Size(834, 123);
            this.panel4.Controls.SetChildIndex(this.rinexObsFileFormatTypeControl1, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_IsUseRangeCorrections, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(834, 101);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(840, 129);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(581, 0);
            // 
            // rinexObsFileFormatTypeControl1
            // 
            this.rinexObsFileFormatTypeControl1.CurrentdType = Gnsser.RinexObsFileFormatType.单站单历元;
            this.rinexObsFileFormatTypeControl1.Location = new System.Drawing.Point(17, 62);
            this.rinexObsFileFormatTypeControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rinexObsFileFormatTypeControl1.Name = "rinexObsFileFormatTypeControl1";
            this.rinexObsFileFormatTypeControl1.Size = new System.Drawing.Size(399, 41);
            this.rinexObsFileFormatTypeControl1.TabIndex = 44;
            // 
            // checkBox_IsUseRangeCorrections
            // 
            this.checkBox_IsUseRangeCorrections.AutoSize = true;
            this.checkBox_IsUseRangeCorrections.Location = new System.Drawing.Point(117, 19);
            this.checkBox_IsUseRangeCorrections.Name = "checkBox_IsUseRangeCorrections";
            this.checkBox_IsUseRangeCorrections.Size = new System.Drawing.Size(132, 16);
            this.checkBox_IsUseRangeCorrections.TabIndex = 0;
            this.checkBox_IsUseRangeCorrections.Text = "是否输出伪距改正数";
            this.checkBox_IsUseRangeCorrections.UseVisualStyleBackColor = true;
            // 
            // SingleGnssFileFormateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 450);
            this.IsShowProgressBar = true;
            this.Name = "SingleGnssFileFormateForm";
            this.Text = "格式化独立测站观测文件";
            this.Load += new System.EventHandler(this.IntegralGnssFileSolveForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.RinexObsFileFormatTypeControl rinexObsFileFormatTypeControl1;
        private System.Windows.Forms.CheckBox checkBox_IsUseRangeCorrections;
    }
}