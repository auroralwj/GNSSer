namespace Gnsser.Winform
{
    partial class SatPolarForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timeLoopControl1 = new Geo.Winform.Controls.TimeLoopControl();
            this.namedFloatControl1AngleCut = new Geo.Winform.Controls.NamedFloatControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedStringControl_prn = new Geo.Winform.Controls.NamedStringControl();
            this.button_coordSet = new System.Windows.Forms.Button();
            this.namedStringControl_coord = new Geo.Winform.Controls.NamedStringControl();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // timeLoopControl1
            // 
            this.timeLoopControl1.Location = new System.Drawing.Point(6, 124);
            this.timeLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timeLoopControl1.Name = "timeLoopControl1";
            this.timeLoopControl1.Size = new System.Drawing.Size(578, 30);
            this.timeLoopControl1.TabIndex = 71;
            // 
            // namedFloatControl1AngleCut
            // 
            this.namedFloatControl1AngleCut.Location = new System.Drawing.Point(6, 96);
            this.namedFloatControl1AngleCut.Name = "namedFloatControl1AngleCut";
            this.namedFloatControl1AngleCut.Size = new System.Drawing.Size(146, 23);
            this.namedFloatControl1AngleCut.TabIndex = 70;
            this.namedFloatControl1AngleCut.Title = "高度截止角(度)：";
            this.namedFloatControl1AngleCut.Value = 0D;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(775, 540);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 72;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.namedStringControl_prn);
            this.groupBox1.Controls.Add(this.button_coordSet);
            this.groupBox1.Controls.Add(this.namedStringControl_coord);
            this.groupBox1.Controls.Add(this.button_cancel);
            this.groupBox1.Controls.Add(this.button_run);
            this.groupBox1.Controls.Add(this.namedFloatControl1AngleCut);
            this.groupBox1.Controls.Add(this.timeLoopControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(775, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // namedStringControl_prn
            // 
            this.namedStringControl_prn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_prn.Location = new System.Drawing.Point(12, 20);
            this.namedStringControl_prn.Name = "namedStringControl_prn";
            this.namedStringControl_prn.Size = new System.Drawing.Size(656, 23);
            this.namedStringControl_prn.TabIndex = 76;
            this.namedStringControl_prn.Title = "卫星编号：";
            // 
            // button_coordSet
            // 
            this.button_coordSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_coordSet.Location = new System.Drawing.Point(604, 56);
            this.button_coordSet.Name = "button_coordSet";
            this.button_coordSet.Size = new System.Drawing.Size(75, 23);
            this.button_coordSet.TabIndex = 75;
            this.button_coordSet.Text = "设置坐标";
            this.button_coordSet.UseVisualStyleBackColor = true;
            this.button_coordSet.Click += new System.EventHandler(this.button_coordSet_Click);
            // 
            // namedStringControl_coord
            // 
            this.namedStringControl_coord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_coord.Location = new System.Drawing.Point(7, 56);
            this.namedStringControl_coord.Name = "namedStringControl_coord";
            this.namedStringControl_coord.Size = new System.Drawing.Size(591, 23);
            this.namedStringControl_coord.TabIndex = 74;
            this.namedStringControl_coord.Title = "测站坐标(XYZ)：";
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(670, 121);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 41);
            this.button_cancel.TabIndex = 72;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // button_run
            // 
            this.button_run.Location = new System.Drawing.Point(589, 121);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 41);
            this.button_run.TabIndex = 72;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 368);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.objectTableControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 342);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "结果表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(761, 336);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 342);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "绘图";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(761, 336);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // SatPolarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 540);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SatPolarForm";
            this.Text = "卫星站星位置计算器";
            this.Load += new System.EventHandler(this.SatPolarForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.TimeLoopControl timeLoopControl1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1AngleCut;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_run;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button_coordSet;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_coord;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_prn;
    }
}