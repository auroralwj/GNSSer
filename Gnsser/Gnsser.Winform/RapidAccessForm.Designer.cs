namespace Gnsser.Winform
{
    partial class RapidAccessForm
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
            this.button_pointPosVizard = new System.Windows.Forms.Button();
            this.button_baseLineSolve = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_pointPosVizard
            // 
            this.button_pointPosVizard.Location = new System.Drawing.Point(36, 34);
            this.button_pointPosVizard.Name = "button_pointPosVizard";
            this.button_pointPosVizard.Size = new System.Drawing.Size(102, 63);
            this.button_pointPosVizard.TabIndex = 0;
            this.button_pointPosVizard.Text = "单点定位向导";
            this.button_pointPosVizard.UseVisualStyleBackColor = true;
            this.button_pointPosVizard.Click += new System.EventHandler(this.button_pointPosVizard_Click);
            // 
            // button_baseLineSolve
            // 
            this.button_baseLineSolve.Location = new System.Drawing.Point(173, 34);
            this.button_baseLineSolve.Name = "button_baseLineSolve";
            this.button_baseLineSolve.Size = new System.Drawing.Size(102, 63);
            this.button_baseLineSolve.TabIndex = 0;
            this.button_baseLineSolve.Text = "基线解算向导";
            this.button_baseLineSolve.UseVisualStyleBackColor = true;
            this.button_baseLineSolve.Visible = false;
            this.button_baseLineSolve.Click += new System.EventHandler(this.button_baseLineSolve_Click);
            // 
            // RapidAccessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 423);
            this.Controls.Add(this.button_baseLineSolve);
            this.Controls.Add(this.button_pointPosVizard);
            this.Name = "RapidAccessForm";
            this.Text = "快速访问";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_pointPosVizard;
        private System.Windows.Forms.Button button_baseLineSolve;
    }
}