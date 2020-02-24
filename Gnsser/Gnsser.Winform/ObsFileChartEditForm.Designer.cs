using Geo.Draw;

namespace Gnsser.Winform
{
    partial class ObsFileChartEditForm
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
            this.chartControl1 = new Geo.Draw.EpochChartControl();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.MaxEpoch = 0;
            this.chartControl1.MinEpoch = 0;
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.Origin = new System.Drawing.Point(50, 70);
            this.chartControl1.SatCount = 0;
            this.chartControl1.Size = new System.Drawing.Size(570, 409);
            this.chartControl1.TabIndex = 0;
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 409);
            this.Controls.Add(this.chartControl1);
            this.Name = "ChartForm";
            this.Text = "图表";
            this.ResumeLayout(false);

        }

        #endregion

        private EpochChartControl chartControl1;
    }
}