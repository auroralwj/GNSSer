
using System.Windows.Forms.DataVisualization.Charting;

namespace Geo.Winform
{
    partial class CommonChartForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonChartForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制绘图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动图例LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.新窗口打开绘图NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.绘图另存为SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.隐藏打开按钮面板VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonZoomInY = new System.Windows.Forms.Button();
            this.buttonZoomOutY = new System.Windows.Forms.Button();
            this.buttonZoomXIn = new System.Windows.Forms.Button();
            this.buttonZoomXOut = new System.Windows.Forms.Button();
            this.buttonDouwnY = new System.Windows.Forms.Button();
            this.button_upX = new System.Windows.Forms.Button();
            this.buttonUpY = new System.Windows.Forms.Button();
            this.button_downX = new System.Windows.Forms.Button();
            this.button_resetY = new System.Windows.Forms.Button();
            this.button_resetX = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_setting = new System.Windows.Forms.Button();
            this.button_openNewTableWihFormat = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.button_simpleSet = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制绘图ToolStripMenuItem,
            this.移动图例LToolStripMenuItem,
            this.toolStripSeparator2,
            this.新窗口打开绘图NToolStripMenuItem,
            this.toolStripSeparator1,
            this.绘图另存为SToolStripMenuItem,
            this.toolStripSeparator3,
            this.隐藏打开按钮面板VToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(194, 132);
            // 
            // 复制绘图ToolStripMenuItem
            // 
            this.复制绘图ToolStripMenuItem.Name = "复制绘图ToolStripMenuItem";
            this.复制绘图ToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.复制绘图ToolStripMenuItem.Text = "复制绘图(&C)";
            this.复制绘图ToolStripMenuItem.Click += new System.EventHandler(this.复制绘图ToolStripMenuItem_Click);
            // 
            // 移动图例LToolStripMenuItem
            // 
            this.移动图例LToolStripMenuItem.Name = "移动图例LToolStripMenuItem";
            this.移动图例LToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.移动图例LToolStripMenuItem.Text = "移动图例(&L)";
            this.移动图例LToolStripMenuItem.Click += new System.EventHandler(this.移动图例LToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(190, 6);
            // 
            // 新窗口打开绘图NToolStripMenuItem
            // 
            this.新窗口打开绘图NToolStripMenuItem.Name = "新窗口打开绘图NToolStripMenuItem";
            this.新窗口打开绘图NToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.新窗口打开绘图NToolStripMenuItem.Text = "新窗口打开绘图(&N)";
            this.新窗口打开绘图NToolStripMenuItem.Click += new System.EventHandler(this.新窗口打开绘图NToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(190, 6);
            // 
            // 绘图另存为SToolStripMenuItem
            // 
            this.绘图另存为SToolStripMenuItem.Name = "绘图另存为SToolStripMenuItem";
            this.绘图另存为SToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.绘图另存为SToolStripMenuItem.Text = "绘图另存为(&S)";
            this.绘图另存为SToolStripMenuItem.Click += new System.EventHandler(this.绘图另存为SToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(190, 6);
            // 
            // 隐藏打开按钮面板VToolStripMenuItem
            // 
            this.隐藏打开按钮面板VToolStripMenuItem.Name = "隐藏打开按钮面板VToolStripMenuItem";
            this.隐藏打开按钮面板VToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.隐藏打开按钮面板VToolStripMenuItem.Text = "隐藏/打开按钮面板(&V)";
            this.隐藏打开按钮面板VToolStripMenuItem.Click += new System.EventHandler(this.隐藏打开按钮面板VToolStripMenuItem_Click);
            // 
            // buttonZoomInY
            // 
            this.buttonZoomInY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomInY.Location = new System.Drawing.Point(370, 27);
            this.buttonZoomInY.Name = "buttonZoomInY";
            this.buttonZoomInY.Size = new System.Drawing.Size(33, 23);
            this.buttonZoomInY.TabIndex = 1;
            this.buttonZoomInY.Text = "Y+";
            this.buttonZoomInY.UseVisualStyleBackColor = true;
            this.buttonZoomInY.Click += new System.EventHandler(this.buttonZoomInY_Click);
            // 
            // buttonZoomOutY
            // 
            this.buttonZoomOutY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomOutY.Location = new System.Drawing.Point(411, 27);
            this.buttonZoomOutY.Name = "buttonZoomOutY";
            this.buttonZoomOutY.Size = new System.Drawing.Size(33, 23);
            this.buttonZoomOutY.TabIndex = 2;
            this.buttonZoomOutY.Text = "Y-";
            this.buttonZoomOutY.UseVisualStyleBackColor = true;
            this.buttonZoomOutY.Click += new System.EventHandler(this.buttonZoomOutY_Click);
            // 
            // buttonZoomXIn
            // 
            this.buttonZoomXIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomXIn.Location = new System.Drawing.Point(370, 4);
            this.buttonZoomXIn.Name = "buttonZoomXIn";
            this.buttonZoomXIn.Size = new System.Drawing.Size(33, 23);
            this.buttonZoomXIn.TabIndex = 1;
            this.buttonZoomXIn.Text = "X+";
            this.buttonZoomXIn.UseVisualStyleBackColor = true;
            this.buttonZoomXIn.Click += new System.EventHandler(this.buttonZoomXIn_Click);
            // 
            // buttonZoomXOut
            // 
            this.buttonZoomXOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomXOut.Location = new System.Drawing.Point(411, 4);
            this.buttonZoomXOut.Name = "buttonZoomXOut";
            this.buttonZoomXOut.Size = new System.Drawing.Size(33, 23);
            this.buttonZoomXOut.TabIndex = 2;
            this.buttonZoomXOut.Text = "X-";
            this.buttonZoomXOut.UseVisualStyleBackColor = true;
            this.buttonZoomXOut.Click += new System.EventHandler(this.buttonZoomXOut_Click);
            // 
            // buttonDouwnY
            // 
            this.buttonDouwnY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDouwnY.Location = new System.Drawing.Point(501, 27);
            this.buttonDouwnY.Name = "buttonDouwnY";
            this.buttonDouwnY.Size = new System.Drawing.Size(33, 23);
            this.buttonDouwnY.TabIndex = 1;
            this.buttonDouwnY.Text = "↓";
            this.buttonDouwnY.UseVisualStyleBackColor = true;
            this.buttonDouwnY.Click += new System.EventHandler(this.buttonDouwnY_Click);
            // 
            // button_upX
            // 
            this.button_upX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_upX.Location = new System.Drawing.Point(537, 27);
            this.button_upX.Name = "button_upX";
            this.button_upX.Size = new System.Drawing.Size(36, 23);
            this.button_upX.TabIndex = 1;
            this.button_upX.Text = "→";
            this.button_upX.UseVisualStyleBackColor = true;
            this.button_upX.Click += new System.EventHandler(this.button_upX_Click);
            // 
            // buttonUpY
            // 
            this.buttonUpY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpY.Location = new System.Drawing.Point(501, 4);
            this.buttonUpY.Name = "buttonUpY";
            this.buttonUpY.Size = new System.Drawing.Size(33, 23);
            this.buttonUpY.TabIndex = 2;
            this.buttonUpY.Text = "↑";
            this.buttonUpY.UseVisualStyleBackColor = true;
            this.buttonUpY.Click += new System.EventHandler(this.buttonUpY_Click);
            // 
            // button_downX
            // 
            this.button_downX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_downX.Location = new System.Drawing.Point(466, 27);
            this.button_downX.Name = "button_downX";
            this.button_downX.Size = new System.Drawing.Size(33, 23);
            this.button_downX.TabIndex = 2;
            this.button_downX.Text = "←";
            this.button_downX.UseVisualStyleBackColor = true;
            this.button_downX.Click += new System.EventHandler(this.button_downX_Click);
            // 
            // button_resetY
            // 
            this.button_resetY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_resetY.Location = new System.Drawing.Point(307, 27);
            this.button_resetY.Name = "button_resetY";
            this.button_resetY.Size = new System.Drawing.Size(55, 23);
            this.button_resetY.TabIndex = 3;
            this.button_resetY.Text = "重置 Y";
            this.button_resetY.UseVisualStyleBackColor = true;
            this.button_resetY.Click += new System.EventHandler(this.button_resetY_Click);
            // 
            // button_resetX
            // 
            this.button_resetX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_resetX.Location = new System.Drawing.Point(306, 4);
            this.button_resetX.Name = "button_resetX";
            this.button_resetX.Size = new System.Drawing.Size(55, 23);
            this.button_resetX.TabIndex = 4;
            this.button_resetX.Text = "重置 X";
            this.button_resetX.UseVisualStyleBackColor = true;
            this.button_resetX.Click += new System.EventHandler(this.button_resetX_Click);
            // 
            // chart1
            // 
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.contextMenuStrip1;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(578, 377);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chart1_KeyDown);
            this.chart1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chart1_KeyPress);
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseUp);
            this.chart1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.chart1_PreviewKeyDown);
            // 
            // button_setting
            // 
            this.button_setting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_setting.Location = new System.Drawing.Point(91, 4);
            this.button_setting.Name = "button_setting";
            this.button_setting.Size = new System.Drawing.Size(75, 23);
            this.button_setting.TabIndex = 5;
            this.button_setting.Text = "详细设置";
            this.button_setting.UseVisualStyleBackColor = true;
            this.button_setting.Click += new System.EventHandler(this.button_setting_Click);
            // 
            // button_openNewTableWihFormat
            // 
            this.button_openNewTableWihFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_openNewTableWihFormat.Location = new System.Drawing.Point(172, 4);
            this.button_openNewTableWihFormat.Name = "button_openNewTableWihFormat";
            this.button_openNewTableWihFormat.Size = new System.Drawing.Size(128, 23);
            this.button_openNewTableWihFormat.TabIndex = 6;
            this.button_openNewTableWihFormat.Text = "保留格式打开新文件";
            this.button_openNewTableWihFormat.UseVisualStyleBackColor = true;
            this.button_openNewTableWihFormat.Click += new System.EventHandler(this.button_openNewTableWihFormat_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PNG 图片|*.png|JPG 图片|*.jpg";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "提示：滚动鼠标+Shift/Alt/Control键，可实现放缩";
            // 
            // button_simpleSet
            // 
            this.button_simpleSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_simpleSet.Location = new System.Drawing.Point(10, 4);
            this.button_simpleSet.Name = "button_simpleSet";
            this.button_simpleSet.Size = new System.Drawing.Size(75, 23);
            this.button_simpleSet.TabIndex = 5;
            this.button_simpleSet.Text = "简要设置";
            this.button_simpleSet.UseVisualStyleBackColor = true;
            this.button_simpleSet.Click += new System.EventHandler(this.button_simpleSet_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button_setting);
            this.panel1.Controls.Add(this.button_resetX);
            this.panel1.Controls.Add(this.buttonZoomXOut);
            this.panel1.Controls.Add(this.buttonZoomInY);
            this.panel1.Controls.Add(this.buttonUpY);
            this.panel1.Controls.Add(this.button_openNewTableWihFormat);
            this.panel1.Controls.Add(this.button_downX);
            this.panel1.Controls.Add(this.buttonZoomXIn);
            this.panel1.Controls.Add(this.buttonZoomOutY);
            this.panel1.Controls.Add(this.button_simpleSet);
            this.panel1.Controls.Add(this.button_resetY);
            this.panel1.Controls.Add(this.button_upX);
            this.panel1.Controls.Add(this.buttonDouwnY);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 377);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(578, 58);
            this.panel1.TabIndex = 1;
            // 
            // CommonChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 435);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CommonChartForm";
            this.ShowIcon = false;
            this.Text = "通用绘图窗口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommonChartForm_FormClosing);
            this.Load += new System.EventHandler(this.CommonChartForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Chart chart1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制绘图ToolStripMenuItem;
        private System.Windows.Forms.Button buttonZoomInY;
        private System.Windows.Forms.Button buttonZoomOutY;
        private System.Windows.Forms.Button buttonZoomXIn;
        private System.Windows.Forms.Button buttonZoomXOut;
        private System.Windows.Forms.Button buttonDouwnY;
        private System.Windows.Forms.Button button_upX;
        private System.Windows.Forms.Button buttonUpY;
        private System.Windows.Forms.Button button_downX;
        private System.Windows.Forms.Button button_resetY;
        private System.Windows.Forms.Button button_resetX;
        private System.Windows.Forms.Button button_setting;
        private System.Windows.Forms.Button button_openNewTableWihFormat;
        private System.Windows.Forms.ToolStripMenuItem 移动图例LToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 绘图另存为SToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button_simpleSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 新窗口打开绘图NToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 隐藏打开按钮面板VToolStripMenuItem;
    }
}