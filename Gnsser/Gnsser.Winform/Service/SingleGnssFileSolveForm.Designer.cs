namespace Gnsser.Winform
{
    partial class SingleGnssFileSolveForm
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
            this.button_drawDop = new System.Windows.Forms.Button();
            this.button_darwResiduals = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Size = new System.Drawing.Size(647, 119);
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
            this.panel2.Controls.Add(this.button_darwResiduals);
            this.panel2.Controls.Add(this.button_drawDop);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(647, 119);
            this.panel2.Controls.SetChildIndex(this.paramVectorRenderControl1, 0);
            this.panel2.Controls.SetChildIndex(this.button_drawDop, 0);
            this.panel2.Controls.SetChildIndex(this.button_darwResiduals, 0);
            // 
            // panel4
            // 
            this.panel4.Size = new System.Drawing.Size(768, 119);
            // 
            // fileOpenControl_inputPathes
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(645, 95);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(653, 125);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(352, 0);
            // 
            // panel5
            // 
            this.panel5.Size = new System.Drawing.Size(527, 24);
            // 
            // button_drawDop
            // 
            this.button_drawDop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawDop.Location = new System.Drawing.Point(472, 93);
            this.button_drawDop.Name = "button_drawDop";
            this.button_drawDop.Size = new System.Drawing.Size(75, 23);
            this.button_drawDop.TabIndex = 1;
            this.button_drawDop.Text = "绘制DOP值";
            this.button_drawDop.UseVisualStyleBackColor = true;
            this.button_drawDop.Click += new System.EventHandler(this.button_drawDop_Click);
            // 
            // button_darwResiduals
            // 
            this.button_darwResiduals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_darwResiduals.Location = new System.Drawing.Point(561, 93);
            this.button_darwResiduals.Name = "button_darwResiduals";
            this.button_darwResiduals.Size = new System.Drawing.Size(85, 23);
            this.button_darwResiduals.TabIndex = 2;
            this.button_darwResiduals.Text = "绘制载波残差";
            this.button_darwResiduals.UseVisualStyleBackColor = true;
            this.button_darwResiduals.Click += new System.EventHandler(this.button_darwResiduals_Click);
            // 
            // SingleGnssFileSolveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 439);
            this.IsShowProgressBar = true;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SingleGnssFileSolveForm";
            this.Text = "单站解算器";
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.panel_buttons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_drawDop;
        private System.Windows.Forms.Button button_darwResiduals;
    }
}