namespace Gnsser.Winform
{
    partial class BATForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button_save = new System.Windows.Forms.Button();
            this.button_plot = new System.Windows.Forms.Button();
            this.groupBox_plotobject = new System.Windows.Forms.GroupBox();
            this.radioButton_cycleslips = new System.Windows.Forms.RadioButton();
            this.radioButton_sn1sn2 = new System.Windows.Forms.RadioButton();
            this.radioButton_ele = new System.Windows.Forms.RadioButton();
            this.radioButton_ioniod = new System.Windows.Forms.RadioButton();
            this.radioButton_azi = new System.Windows.Forms.RadioButton();
            this.radioButton_mp1mp2 = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_lines = new System.Windows.Forms.TabPage();
            this.tableLayoutPane_lines_order = new System.Windows.Forms.TableLayoutPanel();
            this.button_lines_first = new System.Windows.Forms.Button();
            this.button_lines_before = new System.Windows.Forms.Button();
            this.button_lines_next = new System.Windows.Forms.Button();
            this.button_lines_all = new System.Windows.Forms.Button();
            this.button_lines_last = new System.Windows.Forms.Button();
            this.tableLayoutPanel_lines_selection = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chart_line = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage_points = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel_points_order = new System.Windows.Forms.TableLayoutPanel();
            this.button_points_first = new System.Windows.Forms.Button();
            this.button_points_before = new System.Windows.Forms.Button();
            this.button_points_next = new System.Windows.Forms.Button();
            this.button_points_all = new System.Windows.Forms.Button();
            this.button_points_last = new System.Windows.Forms.Button();
            this.chart_point = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.treeView_rinex = new System.Windows.Forms.TreeView();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_screening = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button_show = new System.Windows.Forms.Button();
            this.button_BAT = new System.Windows.Forms.Button();
            this.button_setPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox_plotobject.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_lines.SuspendLayout();
            this.tableLayoutPane_lines_order.SuspendLayout();
            this.tableLayoutPanel_lines_selection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_line)).BeginInit();
            this.tabPage_points.SuspendLayout();
            this.tableLayoutPanel_points_order.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_point)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(1569, 32);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 29);
            this.button_save.TabIndex = 26;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_plot
            // 
            this.button_plot.Location = new System.Drawing.Point(1683, 32);
            this.button_plot.Name = "button_plot";
            this.button_plot.Size = new System.Drawing.Size(75, 30);
            this.button_plot.TabIndex = 25;
            this.button_plot.Text = "开始绘图";
            this.button_plot.UseVisualStyleBackColor = true;
            this.button_plot.Click += new System.EventHandler(this.button_plot_Click);
            // 
            // groupBox_plotobject
            // 
            this.groupBox_plotobject.Controls.Add(this.radioButton_cycleslips);
            this.groupBox_plotobject.Controls.Add(this.radioButton_sn1sn2);
            this.groupBox_plotobject.Controls.Add(this.radioButton_ele);
            this.groupBox_plotobject.Controls.Add(this.radioButton_ioniod);
            this.groupBox_plotobject.Controls.Add(this.radioButton_azi);
            this.groupBox_plotobject.Controls.Add(this.radioButton_mp1mp2);
            this.groupBox_plotobject.Location = new System.Drawing.Point(1022, 21);
            this.groupBox_plotobject.Name = "groupBox_plotobject";
            this.groupBox_plotobject.Size = new System.Drawing.Size(490, 43);
            this.groupBox_plotobject.TabIndex = 24;
            this.groupBox_plotobject.TabStop = false;
            this.groupBox_plotobject.Text = "绘图对象:";
            // 
            // radioButton_cycleslips
            // 
            this.radioButton_cycleslips.AutoSize = true;
            this.radioButton_cycleslips.Location = new System.Drawing.Point(419, 18);
            this.radioButton_cycleslips.Name = "radioButton_cycleslips";
            this.radioButton_cycleslips.Size = new System.Drawing.Size(47, 16);
            this.radioButton_cycleslips.TabIndex = 16;
            this.radioButton_cycleslips.TabStop = true;
            this.radioButton_cycleslips.Text = "周跳";
            this.radioButton_cycleslips.UseVisualStyleBackColor = true;
            // 
            // radioButton_sn1sn2
            // 
            this.radioButton_sn1sn2.AutoSize = true;
            this.radioButton_sn1sn2.Location = new System.Drawing.Point(347, 18);
            this.radioButton_sn1sn2.Name = "radioButton_sn1sn2";
            this.radioButton_sn1sn2.Size = new System.Drawing.Size(59, 16);
            this.radioButton_sn1sn2.TabIndex = 15;
            this.radioButton_sn1sn2.TabStop = true;
            this.radioButton_sn1sn2.Text = "信噪比";
            this.radioButton_sn1sn2.UseVisualStyleBackColor = true;
            this.radioButton_sn1sn2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton_sn1sn2_MouseClick);
            // 
            // radioButton_ele
            // 
            this.radioButton_ele.AutoSize = true;
            this.radioButton_ele.Location = new System.Drawing.Point(55, 18);
            this.radioButton_ele.Name = "radioButton_ele";
            this.radioButton_ele.Size = new System.Drawing.Size(59, 16);
            this.radioButton_ele.TabIndex = 0;
            this.radioButton_ele.TabStop = true;
            this.radioButton_ele.Text = "高度角";
            this.radioButton_ele.UseVisualStyleBackColor = true;
            this.radioButton_ele.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton_ele_MouseClick);
            // 
            // radioButton_ioniod
            // 
            this.radioButton_ioniod.AutoSize = true;
            this.radioButton_ioniod.Location = new System.Drawing.Point(273, 18);
            this.radioButton_ioniod.Name = "radioButton_ioniod";
            this.radioButton_ioniod.Size = new System.Drawing.Size(59, 16);
            this.radioButton_ioniod.TabIndex = 14;
            this.radioButton_ioniod.TabStop = true;
            this.radioButton_ioniod.Text = "电离层";
            this.radioButton_ioniod.UseVisualStyleBackColor = true;
            this.radioButton_ioniod.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton_ioniod_MouseClick);
            // 
            // radioButton_azi
            // 
            this.radioButton_azi.AutoSize = true;
            this.radioButton_azi.Location = new System.Drawing.Point(126, 18);
            this.radioButton_azi.Name = "radioButton_azi";
            this.radioButton_azi.Size = new System.Drawing.Size(59, 16);
            this.radioButton_azi.TabIndex = 12;
            this.radioButton_azi.TabStop = true;
            this.radioButton_azi.Text = "方位角";
            this.radioButton_azi.UseVisualStyleBackColor = true;
            this.radioButton_azi.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton_azi_MouseClick);
            // 
            // radioButton_mp1mp2
            // 
            this.radioButton_mp1mp2.AutoSize = true;
            this.radioButton_mp1mp2.Location = new System.Drawing.Point(198, 19);
            this.radioButton_mp1mp2.Name = "radioButton_mp1mp2";
            this.radioButton_mp1mp2.Size = new System.Drawing.Size(59, 16);
            this.radioButton_mp1mp2.TabIndex = 13;
            this.radioButton_mp1mp2.TabStop = true;
            this.radioButton_mp1mp2.Text = "多路径";
            this.radioButton_mp1mp2.UseVisualStyleBackColor = true;
            this.radioButton_mp1mp2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton_mp1mp2_MouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_lines);
            this.tabControl1.Controls.Add(this.tabPage_points);
            this.tabControl1.Location = new System.Drawing.Point(1022, 69);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(740, 629);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage_lines
            // 
            this.tabPage_lines.Controls.Add(this.tableLayoutPane_lines_order);
            this.tabPage_lines.Controls.Add(this.tableLayoutPanel_lines_selection);
            this.tabPage_lines.Controls.Add(this.chart_line);
            this.tabPage_lines.Location = new System.Drawing.Point(4, 22);
            this.tabPage_lines.Name = "tabPage_lines";
            this.tabPage_lines.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_lines.Size = new System.Drawing.Size(732, 603);
            this.tabPage_lines.TabIndex = 0;
            this.tabPage_lines.Text = "二维曲线图";
            this.tabPage_lines.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPane_lines_order
            // 
            this.tableLayoutPane_lines_order.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPane_lines_order.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPane_lines_order.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPane_lines_order.ColumnCount = 5;
            this.tableLayoutPane_lines_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.56604F));
            this.tableLayoutPane_lines_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.43396F));
            this.tableLayoutPane_lines_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPane_lines_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.tableLayoutPane_lines_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.tableLayoutPane_lines_order.Controls.Add(this.button_lines_first, 0, 0);
            this.tableLayoutPane_lines_order.Controls.Add(this.button_lines_before, 1, 0);
            this.tableLayoutPane_lines_order.Controls.Add(this.button_lines_next, 2, 0);
            this.tableLayoutPane_lines_order.Controls.Add(this.button_lines_all, 3, 0);
            this.tableLayoutPane_lines_order.Controls.Add(this.button_lines_last, 4, 0);
            this.tableLayoutPane_lines_order.Location = new System.Drawing.Point(8, 552);
            this.tableLayoutPane_lines_order.Name = "tableLayoutPane_lines_order";
            this.tableLayoutPane_lines_order.RowCount = 1;
            this.tableLayoutPane_lines_order.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPane_lines_order.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPane_lines_order.Size = new System.Drawing.Size(717, 45);
            this.tableLayoutPane_lines_order.TabIndex = 13;
            // 
            // button_lines_first
            // 
            this.button_lines_first.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_lines_first.Location = new System.Drawing.Point(6, 6);
            this.button_lines_first.Name = "button_lines_first";
            this.button_lines_first.Size = new System.Drawing.Size(135, 33);
            this.button_lines_first.TabIndex = 0;
            this.button_lines_first.Text = "第一颗卫星";
            this.button_lines_first.UseVisualStyleBackColor = true;
            this.button_lines_first.Click += new System.EventHandler(this.button_lines_first_Click);
            // 
            // button_lines_before
            // 
            this.button_lines_before.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_lines_before.Location = new System.Drawing.Point(150, 6);
            this.button_lines_before.Name = "button_lines_before";
            this.button_lines_before.Size = new System.Drawing.Size(131, 33);
            this.button_lines_before.TabIndex = 1;
            this.button_lines_before.Text = "前一颗卫星";
            this.button_lines_before.UseVisualStyleBackColor = true;
            this.button_lines_before.Click += new System.EventHandler(this.button_lines_before_Click);
            // 
            // button_lines_next
            // 
            this.button_lines_next.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_lines_next.Location = new System.Drawing.Point(290, 6);
            this.button_lines_next.Name = "button_lines_next";
            this.button_lines_next.Size = new System.Drawing.Size(130, 33);
            this.button_lines_next.TabIndex = 2;
            this.button_lines_next.Text = "下一颗卫星";
            this.button_lines_next.UseVisualStyleBackColor = true;
            this.button_lines_next.Click += new System.EventHandler(this.button_lines_next_Click);
            // 
            // button_lines_all
            // 
            this.button_lines_all.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_lines_all.Location = new System.Drawing.Point(429, 6);
            this.button_lines_all.Name = "button_lines_all";
            this.button_lines_all.Size = new System.Drawing.Size(136, 33);
            this.button_lines_all.TabIndex = 3;
            this.button_lines_all.Text = "全部卫星";
            this.button_lines_all.UseVisualStyleBackColor = true;
            this.button_lines_all.Click += new System.EventHandler(this.button_lines_all_Click);
            // 
            // button_lines_last
            // 
            this.button_lines_last.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_lines_last.Location = new System.Drawing.Point(574, 6);
            this.button_lines_last.Name = "button_lines_last";
            this.button_lines_last.Size = new System.Drawing.Size(137, 33);
            this.button_lines_last.TabIndex = 4;
            this.button_lines_last.Text = "最后一颗卫星";
            this.button_lines_last.UseVisualStyleBackColor = true;
            this.button_lines_last.Click += new System.EventHandler(this.button_lines_last_Click);
            // 
            // tableLayoutPanel_lines_selection
            // 
            this.tableLayoutPanel_lines_selection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_lines_selection.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel_lines_selection.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel_lines_selection.ColumnCount = 2;
            this.tableLayoutPanel_lines_selection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_lines_selection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_lines_selection.Controls.Add(this.checkBox1, 0, 0);
            this.tableLayoutPanel_lines_selection.Controls.Add(this.checkBox2, 1, 0);
            this.tableLayoutPanel_lines_selection.Location = new System.Drawing.Point(8, 6);
            this.tableLayoutPanel_lines_selection.Name = "tableLayoutPanel_lines_selection";
            this.tableLayoutPanel_lines_selection.RowCount = 1;
            this.tableLayoutPanel_lines_selection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_lines_selection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_lines_selection.Size = new System.Drawing.Size(717, 43);
            this.tableLayoutPanel_lines_selection.TabIndex = 13;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(114, 6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(132, 31);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "选项1：Mp1/Sn1/Ion";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBox1_MouseClick);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(471, 6);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(132, 31);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "选项2：Mp2/Sn2/Iod";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBox2_MouseClick);
            // 
            // chart_line
            // 
            this.chart_line.BorderlineColor = System.Drawing.Color.DarkGray;
            this.chart_line.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.chart_line.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_line.Legends.Add(legend1);
            this.chart_line.Location = new System.Drawing.Point(8, 53);
            this.chart_line.Name = "chart_line";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_line.Series.Add(series1);
            this.chart_line.Size = new System.Drawing.Size(717, 493);
            this.chart_line.TabIndex = 14;
            this.chart_line.Text = "chart1";
            this.chart_line.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.chart_line_GetToolTipText);
            // 
            // tabPage_points
            // 
            this.tabPage_points.Controls.Add(this.tableLayoutPanel_points_order);
            this.tabPage_points.Controls.Add(this.chart_point);
            this.tabPage_points.Location = new System.Drawing.Point(4, 22);
            this.tabPage_points.Name = "tabPage_points";
            this.tabPage_points.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_points.Size = new System.Drawing.Size(732, 603);
            this.tabPage_points.TabIndex = 1;
            this.tabPage_points.Text = "二维散点图";
            this.tabPage_points.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_points_order
            // 
            this.tableLayoutPanel_points_order.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_points_order.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel_points_order.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel_points_order.ColumnCount = 5;
            this.tableLayoutPanel_points_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.45454F));
            this.tableLayoutPanel_points_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.54546F));
            this.tableLayoutPanel_points_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 143F));
            this.tableLayoutPanel_points_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.tableLayoutPanel_points_order.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 245F));
            this.tableLayoutPanel_points_order.Controls.Add(this.button_points_first, 0, 0);
            this.tableLayoutPanel_points_order.Controls.Add(this.button_points_before, 1, 0);
            this.tableLayoutPanel_points_order.Controls.Add(this.button_points_next, 2, 0);
            this.tableLayoutPanel_points_order.Controls.Add(this.button_points_all, 3, 0);
            this.tableLayoutPanel_points_order.Controls.Add(this.button_points_last, 4, 0);
            this.tableLayoutPanel_points_order.Location = new System.Drawing.Point(8, 552);
            this.tableLayoutPanel_points_order.Name = "tableLayoutPanel_points_order";
            this.tableLayoutPanel_points_order.RowCount = 1;
            this.tableLayoutPanel_points_order.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_points_order.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_points_order.Size = new System.Drawing.Size(717, 45);
            this.tableLayoutPanel_points_order.TabIndex = 15;
            // 
            // button_points_first
            // 
            this.button_points_first.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_points_first.Location = new System.Drawing.Point(6, 6);
            this.button_points_first.Name = "button_points_first";
            this.button_points_first.Size = new System.Drawing.Size(77, 33);
            this.button_points_first.TabIndex = 0;
            this.button_points_first.Text = "第一颗卫星";
            this.button_points_first.UseVisualStyleBackColor = true;
            this.button_points_first.Click += new System.EventHandler(this.button_points_first_Click);
            // 
            // button_points_before
            // 
            this.button_points_before.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_points_before.Location = new System.Drawing.Point(92, 6);
            this.button_points_before.Name = "button_points_before";
            this.button_points_before.Size = new System.Drawing.Size(79, 33);
            this.button_points_before.TabIndex = 1;
            this.button_points_before.Text = "前一颗卫星";
            this.button_points_before.UseVisualStyleBackColor = true;
            this.button_points_before.Click += new System.EventHandler(this.button_points_before_Click);
            // 
            // button_points_next
            // 
            this.button_points_next.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_points_next.Location = new System.Drawing.Point(180, 6);
            this.button_points_next.Name = "button_points_next";
            this.button_points_next.Size = new System.Drawing.Size(137, 33);
            this.button_points_next.TabIndex = 2;
            this.button_points_next.Text = "下一颗卫星";
            this.button_points_next.UseVisualStyleBackColor = true;
            this.button_points_next.Click += new System.EventHandler(this.button_points_next_Click);
            // 
            // button_points_all
            // 
            this.button_points_all.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_points_all.Location = new System.Drawing.Point(326, 6);
            this.button_points_all.Name = "button_points_all";
            this.button_points_all.Size = new System.Drawing.Size(136, 33);
            this.button_points_all.TabIndex = 3;
            this.button_points_all.Text = "全部卫星";
            this.button_points_all.UseVisualStyleBackColor = true;
            this.button_points_all.Click += new System.EventHandler(this.button_points_all_Click);
            // 
            // button_points_last
            // 
            this.button_points_last.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_points_last.Location = new System.Drawing.Point(471, 6);
            this.button_points_last.Name = "button_points_last";
            this.button_points_last.Size = new System.Drawing.Size(240, 33);
            this.button_points_last.TabIndex = 4;
            this.button_points_last.Text = "最后一颗卫星";
            this.button_points_last.UseVisualStyleBackColor = true;
            this.button_points_last.Click += new System.EventHandler(this.button_points_last_Click);
            // 
            // chart_point
            // 
            this.chart_point.BorderlineColor = System.Drawing.Color.DarkGray;
            this.chart_point.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea2.Name = "ChartArea1";
            this.chart_point.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart_point.Legends.Add(legend2);
            this.chart_point.Location = new System.Drawing.Point(7, 7);
            this.chart_point.Name = "chart_point";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart_point.Series.Add(series2);
            this.chart_point.Size = new System.Drawing.Size(718, 540);
            this.chart_point.TabIndex = 16;
            this.chart_point.Text = "chart1";
            this.chart_point.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.chart_point_GetToolTipText);
            // 
            // treeView_rinex
            // 
            this.treeView_rinex.CheckBoxes = true;
            this.treeView_rinex.Location = new System.Drawing.Point(29, 69);
            this.treeView_rinex.Name = "treeView_rinex";
            this.treeView_rinex.Size = new System.Drawing.Size(153, 629);
            this.treeView_rinex.TabIndex = 22;
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(837, 21);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(75, 30);
            this.button_clear.TabIndex = 21;
            this.button_clear.Text = "重置";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_screening
            // 
            this.button_screening.Location = new System.Drawing.Point(736, 21);
            this.button_screening.Name = "button_screening";
            this.button_screening.Size = new System.Drawing.Size(75, 30);
            this.button_screening.TabIndex = 20;
            this.button_screening.Text = "文件筛选";
            this.button_screening.UseVisualStyleBackColor = true;
            this.button_screening.Click += new System.EventHandler(this.button_screening_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column12,
            this.Column11,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Location = new System.Drawing.Point(194, 69);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(815, 629);
            this.dataGridView1.TabIndex = 19;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Column12
            // 
            this.Column12.HeaderText = "ID";
            this.Column12.Name = "Column12";
            this.Column12.Width = 40;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Sites";
            this.Column11.Name = "Column11";
            this.Column11.Width = 40;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "First epoch";
            this.Column1.Name = "Column1";
            this.Column1.Width = 95;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Last epoch";
            this.Column2.Name = "Column2";
            this.Column2.Width = 95;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Hrs";
            this.Column3.Name = "Column3";
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Dt";
            this.Column4.Name = "Column4";
            this.Column4.Width = 50;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "#Expt";
            this.Column5.Name = "Column5";
            this.Column5.Width = 70;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "#Have";
            this.Column6.Name = "Column6";
            this.Column6.Width = 70;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "%";
            this.Column7.Name = "Column7";
            this.Column7.Width = 50;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Mp1";
            this.Column8.Name = "Column8";
            this.Column8.Width = 70;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Mp2";
            this.Column9.Name = "Column9";
            this.Column9.Width = 70;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "O/slps";
            this.Column10.Name = "Column10";
            this.Column10.Width = 70;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(194, 21);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(308, 30);
            this.progressBar1.TabIndex = 18;
            // 
            // button_show
            // 
            this.button_show.Location = new System.Drawing.Point(934, 21);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(75, 30);
            this.button_show.TabIndex = 17;
            this.button_show.Text = "全部结果";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.button_show_Click);
            // 
            // button_BAT
            // 
            this.button_BAT.Location = new System.Drawing.Point(518, 21);
            this.button_BAT.Name = "button_BAT";
            this.button_BAT.Size = new System.Drawing.Size(75, 30);
            this.button_BAT.TabIndex = 16;
            this.button_BAT.Text = "TEQC批处理";
            this.button_BAT.UseVisualStyleBackColor = true;
            this.button_BAT.Click += new System.EventHandler(this.button_BAT_Click);
            // 
            // button_setPath
            // 
            this.button_setPath.Location = new System.Drawing.Point(107, 21);
            this.button_setPath.Name = "button_setPath";
            this.button_setPath.Size = new System.Drawing.Size(75, 30);
            this.button_setPath.TabIndex = 15;
            this.button_setPath.Text = "点击添加";
            this.button_setPath.UseVisualStyleBackColor = true;
            this.button_setPath.Click += new System.EventHandler(this.button_setPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "Rinex文件：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Rinex 观测文件|*.*o";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "请选择O文件";
            // 
            // BATForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1789, 718);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_plot);
            this.Controls.Add(this.groupBox_plotobject);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.treeView_rinex);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_screening);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_show);
            this.Controls.Add(this.button_BAT);
            this.Controls.Add(this.button_setPath);
            this.Controls.Add(this.label1);
            this.Name = "BATForm";
            this.Text = "BATForm";
            this.groupBox_plotobject.ResumeLayout(false);
            this.groupBox_plotobject.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_lines.ResumeLayout(false);
            this.tableLayoutPane_lines_order.ResumeLayout(false);
            this.tableLayoutPanel_lines_selection.ResumeLayout(false);
            this.tableLayoutPanel_lines_selection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_line)).EndInit();
            this.tabPage_points.ResumeLayout(false);
            this.tableLayoutPanel_points_order.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_point)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_plot;
        private System.Windows.Forms.GroupBox groupBox_plotobject;
        private System.Windows.Forms.RadioButton radioButton_cycleslips;
        private System.Windows.Forms.RadioButton radioButton_sn1sn2;
        private System.Windows.Forms.RadioButton radioButton_ele;
        private System.Windows.Forms.RadioButton radioButton_ioniod;
        private System.Windows.Forms.RadioButton radioButton_azi;
        private System.Windows.Forms.RadioButton radioButton_mp1mp2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_lines;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPane_lines_order;
        private System.Windows.Forms.Button button_lines_first;
        private System.Windows.Forms.Button button_lines_before;
        private System.Windows.Forms.Button button_lines_next;
        private System.Windows.Forms.Button button_lines_all;
        private System.Windows.Forms.Button button_lines_last;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_lines_selection;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_line;
        private System.Windows.Forms.TabPage tabPage_points;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_points_order;
        private System.Windows.Forms.Button button_points_first;
        private System.Windows.Forms.Button button_points_before;
        private System.Windows.Forms.Button button_points_next;
        private System.Windows.Forms.Button button_points_all;
        private System.Windows.Forms.Button button_points_last;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_point;
        private System.Windows.Forms.TreeView treeView_rinex;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_screening;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button_show;
        private System.Windows.Forms.Button button_BAT;
        private System.Windows.Forms.Button button_setPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}