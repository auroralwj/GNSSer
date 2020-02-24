namespace Gnsser.Winform
{
    partial class ObsSatViewerForm
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

        #region Windows Form Designer generated obsCode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCode editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "5,7,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "5,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(4D, "7,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, "1,3,0,0");
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_read = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_copyPic = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_readFile = new System.Windows.Forms.Button();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.checkBox_sortPrn = new System.Windows.Forms.CheckBox();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Rinex 观测文件|*.*o";
            this.openFileDialog1.Title = "请选择O文件";
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(632, 47);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(103, 26);
            this.button_read.TabIndex = 5;
            this.button_read.Text = "分析载入数据";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(13, 99);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(75, 23);
            this.button_clear.TabIndex = 8;
            this.button_clear.Text = "清除图表";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_copyPic
            // 
            this.button_copyPic.Location = new System.Drawing.Point(94, 99);
            this.button_copyPic.Name = "button_copyPic";
            this.button_copyPic.Size = new System.Drawing.Size(75, 23);
            this.button_copyPic.TabIndex = 10;
            this.button_copyPic.Text = "复制图像";
            this.button_copyPic.UseVisualStyleBackColor = true;
            this.button_copyPic.Click += new System.EventHandler(this.button_copyPic_Click);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(17, 128);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeBar;
            series1.Legend = "Legend1";
            series1.LegendText = "FileName";
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.YValuesPerPoint = 4;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(718, 421);
            this.chart1.TabIndex = 6;
            this.chart1.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "文件观测时段";
            this.chart1.Titles.Add(title1);
            // 
            // button_readFile
            // 
            this.button_readFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readFile.Location = new System.Drawing.Point(500, 47);
            this.button_readFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_readFile.Name = "button_readFile";
            this.button_readFile.Size = new System.Drawing.Size(111, 26);
            this.button_readFile.TabIndex = 13;
            this.button_readFile.Text = "载入数据并分析";
            this.button_readFile.UseVisualStyleBackColor = true;
            this.button_readFile.Click += new System.EventHandler(this.button_readFile_Click);
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(11, 47);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(292, 44);
            this.multiGnssSystemSelectControl1.TabIndex = 14;
            // 
            // checkBox_sortPrn
            // 
            this.checkBox_sortPrn.AutoSize = true;
            this.checkBox_sortPrn.Location = new System.Drawing.Point(324, 47);
            this.checkBox_sortPrn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_sortPrn.Name = "checkBox_sortPrn";
            this.checkBox_sortPrn.Size = new System.Drawing.Size(72, 16);
            this.checkBox_sortPrn.TabIndex = 15;
            this.checkBox_sortPrn.Text = "卫星排序";
            this.checkBox_sortPrn.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "Rinex 观测文件|*.*o|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "RINEX O文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(11, 12);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(723, 30);
            this.fileOpenControl1.TabIndex = 16;
            // 
            // ObsSatViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 559);
            this.Controls.Add(this.fileOpenControl1);
            this.Controls.Add(this.checkBox_sortPrn);
            this.Controls.Add(this.multiGnssSystemSelectControl1);
            this.Controls.Add(this.button_readFile);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.button_copyPic);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_read);
            this.Name = "ObsSatViewerForm";
            this.Text = "观测卫星时段查看";
            this.Load += new System.EventHandler(this.ObsSatViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_copyPic;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button_readFile;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.Windows.Forms.CheckBox checkBox_sortPrn;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
    }
}