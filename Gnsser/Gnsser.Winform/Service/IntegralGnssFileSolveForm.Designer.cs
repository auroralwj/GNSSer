namespace Gnsser.Winform
{
    partial class IntegralGnssFileSolveForm
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
            this.checkBox_disposeWhenEnd = new System.Windows.Forms.CheckBox();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.paramVectorRenderControl1.Size = new System.Drawing.Size(993, 117);
            // 
            // tabPage0
            // 
            this.tabPage0.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.tabPage0.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.tabPage0.Size = new System.Drawing.Size(1028, 345);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBox_disposeWhenEnd);
            this.panel3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel3.Size = new System.Drawing.Size(993, 117);
            this.panel3.Controls.SetChildIndex(this.checkBox_disposeWhenEnd, 0);
            // 
            // panel2
            // 
            this.panel2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel2.Size = new System.Drawing.Size(993, 117);
            // 
            // panel4
            // 
            this.panel4.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panel4.Size = new System.Drawing.Size(1020, 117);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Location = new System.Drawing.Point(7, 6);
            this.fileOpenControl_inputPathes.Margin = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(1014, 85);
            // 
            // tabPage1
            // 
            this.tabPage1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tabPage1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tabPage1.Size = new System.Drawing.Size(1028, 125);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(680, 0);
            this.panel_buttons.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            // 
            // checkBox_disposeWhenEnd
            // 
            this.checkBox_disposeWhenEnd.AutoSize = true;
            this.checkBox_disposeWhenEnd.Location = new System.Drawing.Point(7, 48);
            this.checkBox_disposeWhenEnd.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_disposeWhenEnd.Name = "checkBox_disposeWhenEnd";
            this.checkBox_disposeWhenEnd.Size = new System.Drawing.Size(194, 19);
            this.checkBox_disposeWhenEnd.TabIndex = 55;
            this.checkBox_disposeWhenEnd.Text = "计算完毕后立即释放资源";
            this.checkBox_disposeWhenEnd.UseVisualStyleBackColor = true;
            // 
            // IntegralGnssFileSolveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 606);
            this.IsShowProgressBar = true;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "IntegralGnssFileSolveForm";
            this.Text = "多站整体解算";
            this.Load += new System.EventHandler(this.IntegralGnssFileSolveForm_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.panel_buttons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox_disposeWhenEnd;
    }
}