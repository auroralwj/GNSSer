namespace Gnsser.Winform
{
    partial class ClockJumpReviserForm
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
            this.checkBox_repaire = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_clockJumpFilePath = new Geo.Winform.Controls.FileOpenControl();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(728, 233);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.fileOpenControl_clockJumpFilePath);
            this.panel4.Controls.Add(this.checkBox_repaire);
            this.panel4.Size = new System.Drawing.Size(722, 123);
            this.panel4.Controls.SetChildIndex(this.checkBox_repaire, 0);
            this.panel4.Controls.SetChildIndex(this.fileOpenControl_clockJumpFilePath, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(722, 101);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(728, 129);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(469, 0);
            // 
            // checkBox_repaire
            // 
            this.checkBox_repaire.AutoSize = true;
            this.checkBox_repaire.Checked = true;
            this.checkBox_repaire.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_repaire.Location = new System.Drawing.Point(157, 18);
            this.checkBox_repaire.Name = "checkBox_repaire";
            this.checkBox_repaire.Size = new System.Drawing.Size(72, 16);
            this.checkBox_repaire.TabIndex = 58;
            this.checkBox_repaire.Text = "修复钟跳";
            this.checkBox_repaire.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_clockJumpFilePath
            // 
            this.fileOpenControl_clockJumpFilePath.AllowDrop = true;
            this.fileOpenControl_clockJumpFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_clockJumpFilePath.FilePath = "";
            this.fileOpenControl_clockJumpFilePath.FilePathes = new string[0];
            this.fileOpenControl_clockJumpFilePath.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_clockJumpFilePath.FirstPath = "";
            this.fileOpenControl_clockJumpFilePath.IsMultiSelect = false;
            this.fileOpenControl_clockJumpFilePath.LabelName = "外置钟跳文件：";
            this.fileOpenControl_clockJumpFilePath.Location = new System.Drawing.Point(19, 51);
            this.fileOpenControl_clockJumpFilePath.Name = "fileOpenControl_clockJumpFilePath";
            this.fileOpenControl_clockJumpFilePath.Size = new System.Drawing.Size(457, 22);
            this.fileOpenControl_clockJumpFilePath.TabIndex = 59;
            // 
            // ClockJumpReviserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 490);
            this.IsShowProgressBar = true;
            this.Name = "ClockJumpReviserForm";
            this.Text = "钟跳探测与修复";
            this.Load += new System.EventHandler(this.IntegralGnssFileSolveForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_repaire;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_clockJumpFilePath;
    }
}