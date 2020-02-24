namespace Gnsser.Winform
{
    partial class TwoSiteSolveForm
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
            this.button_drawPhaseResidual = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            // 
            // tabPage0
            // 
            this.tabPage0.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.tabPage0.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.tabPage0.Size = new System.Drawing.Size(653, 182);
            // 
            // panel3
            // 
            this.panel3.Margin = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(768, 119);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_drawPhaseResidual);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(768, 119);
            this.panel2.Controls.SetChildIndex(this.paramVectorRenderControl1, 0);
            this.panel2.Controls.SetChildIndex(this.button_drawPhaseResidual, 0);
            // 
            // panel4
            // 
            this.panel4.Size = new System.Drawing.Size(768, 119);
            // 
            // fileOpenControl_inputPathes
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.LabelName = "同步观测文件：";
            this.fileOpenControl_inputPathes.Location = new System.Drawing.Point(5, 5);
            this.fileOpenControl_inputPathes.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(643, 93);
            // 
            // tabPage1
            // 
            this.tabPage1.Margin = new System.Windows.Forms.Padding(5);
            this.tabPage1.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage1.Size = new System.Drawing.Size(653, 125);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(352, 0);
            this.panel_buttons.Margin = new System.Windows.Forms.Padding(5);
            // 
            // panel5
            // 
            this.panel5.Size = new System.Drawing.Size(527, 24);
            // 
            // button_drawPhaseResidual
            // 
            this.button_drawPhaseResidual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawPhaseResidual.Location = new System.Drawing.Point(659, 86);
            this.button_drawPhaseResidual.Name = "button_drawPhaseResidual";
            this.button_drawPhaseResidual.Size = new System.Drawing.Size(90, 23);
            this.button_drawPhaseResidual.TabIndex = 1;
            this.button_drawPhaseResidual.Text = "绘制载波残差";
            this.button_drawPhaseResidual.UseVisualStyleBackColor = true;
            this.button_drawPhaseResidual.Click += new System.EventHandler(this.button_drawPhaseResidual_Click);
            // 
            // TwoSiteSolveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 439);
            this.IsShowProgressBar = true;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TwoSiteSolveForm";
            this.OutputDirectory = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\Common7\\IDE\\Temp";
            this.Text = "单站解算器";
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.panel_buttons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_drawPhaseResidual;
    }
}