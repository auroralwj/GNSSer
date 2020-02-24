namespace Geo.Draw
{
    partial class TwoDimColorChartForm
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
            Geo.Draw.ColorChartDrawer colorChartDrawer1 = new Geo.Draw.ColorChartDrawer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TwoDimColorChartForm));
            this.chartControl1 = new Geo.Draw.TwoDimColorDrawControl();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            colorChartDrawer1.ChartGraphics = null;
            colorChartDrawer1.ChartSize = new System.Drawing.Size(570, 398);
            colorChartDrawer1.ColorBuilder = null;
            colorChartDrawer1.ContentBorderSize = new System.Drawing.Size(2, 2);
            colorChartDrawer1.ContentBorderWidth = 2;
            colorChartDrawer1.ContentBox = ((Geo.Coordinates.Envelope)(resources.GetObject("colorChartDrawer1.ContentBox")));
            colorChartDrawer1.ContentBoxInScreenCoord = ((Geo.Coordinates.Envelope)(resources.GetObject("colorChartDrawer1.ContentBoxInScreenCoord")));
            colorChartDrawer1.ContentCenter = new System.Drawing.Size(215, 151);
            colorChartDrawer1.ContentOrigin = new System.Drawing.Point(60, 75);
            colorChartDrawer1.ContentSize = new System.Drawing.Size(430, 303);
            colorChartDrawer1.GeoCoords = null;
            colorChartDrawer1.InputToContentCoordConverter = null;
            colorChartDrawer1.LeftDownSpace = new System.Drawing.Size(60, 75);
            colorChartDrawer1.OriginOfLengendColorBar = new System.Drawing.Point(530, 136);
            colorChartDrawer1.RightTopSpace = new System.Drawing.Size(80, 20);
            colorChartDrawer1.SizeOfLegendColorBar = new System.Drawing.Size(12, 182);
            colorChartDrawer1.TableValue = null;
            this.chartControl1.ColorChartDrawer = colorChartDrawer1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.Size = new System.Drawing.Size(570, 398);
            this.chartControl1.TabIndex = 0;
            this.chartControl1.TwoNumeralKeyAndValueDictionary = null;
            this.chartControl1.ValSpanForColor = null;
            // 
            // TwoDimColorChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 398);
            this.Controls.Add(this.chartControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TwoDimColorChartForm";
            this.Text = "图表";
            this.ResumeLayout(false);

        }

        #endregion

        private TwoDimColorDrawControl chartControl1;
    }
}