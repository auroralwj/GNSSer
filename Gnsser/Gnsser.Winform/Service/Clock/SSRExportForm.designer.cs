namespace Gnsser.Winform
{
    partial class SSRExportForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.button_getPath = new System.Windows.Forms.Button();
            this.textBox_SSRsp3Pathes = new System.Windows.Forms.TextBox();
            this.openFileDialog_result = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.calculate = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker_from = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_to = new System.Windows.Forms.DateTimePicker();
            this.comboBox_name = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button_filter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_inter = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClockRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.button_to_excel = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_NavPathes = new System.Windows.Forms.TextBox();
            this.button_getNavPath = new System.Windows.Forms.Button();
            this.openFileDialog_Nav = new System.Windows.Forms.OpenFileDialog();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.baseSatSelectingControl1 = new Gnsser.Winform.Controls.BaseSatSelectingControl();
            this.BNC_Selected = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "SSR文件：";
            // 
            // button_getPath
            // 
            this.button_getPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getPath.Location = new System.Drawing.Point(659, 13);
            this.button_getPath.Name = "button_getPath";
            this.button_getPath.Size = new System.Drawing.Size(50, 47);
            this.button_getPath.TabIndex = 22;
            this.button_getPath.Text = "...";
            this.button_getPath.UseVisualStyleBackColor = true;
            this.button_getPath.Click += new System.EventHandler(this.button_getPath_Click);
            // 
            // textBox_SSRsp3Pathes
            // 
            this.textBox_SSRsp3Pathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SSRsp3Pathes.Location = new System.Drawing.Point(136, 13);
            this.textBox_SSRsp3Pathes.Multiline = true;
            this.textBox_SSRsp3Pathes.Name = "textBox_SSRsp3Pathes";
            this.textBox_SSRsp3Pathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_SSRsp3Pathes.Size = new System.Drawing.Size(517, 45);
            this.textBox_SSRsp3Pathes.TabIndex = 24;
            // 
            // openFileDialog_result
            // 
            this.openFileDialog_result.Filter = "钟差估计结果文件|*.xls|所有文件|*.*";
            this.openFileDialog_result.Multiselect = true;
            this.openFileDialog_result.Title = "请选择文件";
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "openFileDialog2";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk|所有文件|*.*";
            // 
            // calculate
            // 
            this.calculate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.calculate.Location = new System.Drawing.Point(789, 229);
            this.calculate.Name = "calculate";
            this.calculate.Size = new System.Drawing.Size(75, 23);
            this.calculate.TabIndex = 30;
            this.calculate.Text = "计算";
            this.calculate.UseVisualStyleBackColor = true;
            this.calculate.Click += new System.EventHandler(this.calculate_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(581, 291);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(126, 16);
            this.checkBox1.TabIndex = 31;
            this.checkBox1.Text = "若无数据，以0填充";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(303, 292);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 32;
            this.label4.Text = "分钟";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 33;
            this.label2.Text = "采样间隔：";
            // 
            // textBox_interval
            // 
            this.textBox_interval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_interval.Location = new System.Drawing.Point(259, 289);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(31, 21);
            this.textBox_interval.TabIndex = 34;
            this.textBox_interval.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "钟差文件：";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(657, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 47);
            this.button1.TabIndex = 35;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(136, 15);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(505, 45);
            this.textBox1.TabIndex = 37;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.dateTimePicker_from);
            this.groupBox2.Controls.Add(this.dateTimePicker_to);
            this.groupBox2.Controls.Add(this.comboBox_name);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.button_filter);
            this.groupBox2.Location = new System.Drawing.Point(60, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(723, 51);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(413, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "到";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(158, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 26;
            this.label6.Text = "历元：从";
            // 
            // dateTimePicker_from
            // 
            this.dateTimePicker_from.CustomFormat = "yyyy-MM-dd HH:mm:ss ddd";
            this.dateTimePicker_from.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_from.Location = new System.Drawing.Point(217, 18);
            this.dateTimePicker_from.Name = "dateTimePicker_from";
            this.dateTimePicker_from.Size = new System.Drawing.Size(190, 21);
            this.dateTimePicker_from.TabIndex = 27;
            this.dateTimePicker_from.Value = new System.DateTime(2008, 3, 24, 0, 0, 0, 0);
            // 
            // dateTimePicker_to
            // 
            this.dateTimePicker_to.CustomFormat = "yyyy-MM-dd HH:mm:ss ddd";
            this.dateTimePicker_to.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_to.Location = new System.Drawing.Point(438, 18);
            this.dateTimePicker_to.Name = "dateTimePicker_to";
            this.dateTimePicker_to.Size = new System.Drawing.Size(180, 21);
            this.dateTimePicker_to.TabIndex = 28;
            this.dateTimePicker_to.Value = new System.DateTime(2008, 3, 24, 0, 0, 0, 0);
            // 
            // comboBox_name
            // 
            this.comboBox_name.FormattingEnabled = true;
            this.comboBox_name.Location = new System.Drawing.Point(70, 19);
            this.comboBox_name.Name = "comboBox_name";
            this.comboBox_name.Size = new System.Drawing.Size(79, 20);
            this.comboBox_name.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "载体名称：";
            // 
            // button_filter
            // 
            this.button_filter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_filter.Location = new System.Drawing.Point(633, 17);
            this.button_filter.Name = "button_filter";
            this.button_filter.Size = new System.Drawing.Size(75, 23);
            this.button_filter.TabIndex = 0;
            this.button_filter.Text = "过滤/筛选";
            this.button_filter.UseVisualStyleBackColor = true;
            this.button_filter.Click += new System.EventHandler(this.button_filter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_SSRsp3Pathes);
            this.groupBox1.Controls.Add(this.button_getPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(57, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(726, 64);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(57, 204);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(726, 66);
            this.groupBox3.TabIndex = 40;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // button_inter
            // 
            this.button_inter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_inter.Location = new System.Drawing.Point(789, 154);
            this.button_inter.Name = "button_inter";
            this.button_inter.Size = new System.Drawing.Size(75, 23);
            this.button_inter.TabIndex = 41;
            this.button_inter.Text = "插值";
            this.button_inter.UseVisualStyleBackColor = true;
            this.button_inter.Click += new System.EventHandler(this.button_inter_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.ClockRate,
            this.Column6});
            this.dataGridView1.Location = new System.Drawing.Point(66, 328);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(798, 312);
            this.dataGridView1.TabIndex = 42;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ClockType";
            this.Column1.HeaderText = "类型";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Name";
            this.Column2.HeaderText = "载体名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Time";
            this.Column3.HeaderText = "历元";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ClockBias";
            this.Column4.HeaderText = "钟差";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ClockDrift";
            this.Column5.HeaderText = "钟漂";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // ClockRate
            // 
            this.ClockRate.DataPropertyName = "DriftRate";
            this.ClockRate.HeaderText = "钟漂速度";
            this.ClockRate.Name = "ClockRate";
            this.ClockRate.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "StateCode";
            this.Column6.HeaderText = "状态";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // button_to_excel
            // 
            this.button_to_excel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_to_excel.Location = new System.Drawing.Point(789, 287);
            this.button_to_excel.Name = "button_to_excel";
            this.button_to_excel.Size = new System.Drawing.Size(75, 23);
            this.button_to_excel.TabIndex = 43;
            this.button_to_excel.Text = "Excel导出";
            this.button_to_excel.UseVisualStyleBackColor = true;
            this.button_to_excel.Click += new System.EventHandler(this.button_to_excel_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.textBox_NavPathes);
            this.groupBox4.Controls.Add(this.button_getNavPath);
            this.groupBox4.Location = new System.Drawing.Point(57, 77);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(726, 63);
            this.groupBox4.TabIndex = 44;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 46;
            this.label8.Text = "广播星历文件：";
            // 
            // textBox_NavPathes
            // 
            this.textBox_NavPathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_NavPathes.Location = new System.Drawing.Point(136, 12);
            this.textBox_NavPathes.Multiline = true;
            this.textBox_NavPathes.Name = "textBox_NavPathes";
            this.textBox_NavPathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_NavPathes.Size = new System.Drawing.Size(514, 45);
            this.textBox_NavPathes.TabIndex = 47;
            // 
            // button_getNavPath
            // 
            this.button_getNavPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getNavPath.Location = new System.Drawing.Point(658, 10);
            this.button_getNavPath.Name = "button_getNavPath";
            this.button_getNavPath.Size = new System.Drawing.Size(50, 47);
            this.button_getNavPath.TabIndex = 45;
            this.button_getNavPath.Text = "...";
            this.button_getNavPath.UseVisualStyleBackColor = true;
            this.button_getNavPath.Click += new System.EventHandler(this.button_getNavPath_Click);
            // 
            // openFileDialog_Nav
            // 
            this.openFileDialog_Nav.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            this.openFileDialog_Nav.Multiselect = true;
            this.openFileDialog_Nav.Title = "请选择文件";
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(122, 235);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(632, 22);
            this.directorySelectionControl1.TabIndex = 25;
            // 
            // baseSatSelectingControl1
            // 
            this.baseSatSelectingControl1.EnableBaseSat = true;
            this.baseSatSelectingControl1.Location = new System.Drawing.Point(487, 281);
            this.baseSatSelectingControl1.Name = "baseSatSelectingControl1";
            this.baseSatSelectingControl1.Size = new System.Drawing.Size(270, 87);
            this.baseSatSelectingControl1.TabIndex = 29;
            // 
            // BNC_Selected
            // 
            this.BNC_Selected.AutoSize = true;
            this.BNC_Selected.Location = new System.Drawing.Point(789, 52);
            this.BNC_Selected.Name = "BNC_Selected";
            this.BNC_Selected.Size = new System.Drawing.Size(65, 16);
            this.BNC_Selected.TabIndex = 45;
            this.BNC_Selected.TabStop = true;
            this.BNC_Selected.Text = "BNC数据";
            this.BNC_Selected.UseVisualStyleBackColor = true;
            // 
            // SSRExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 669);
            this.Controls.Add(this.BNC_Selected);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button_to_excel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button_inter);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_interval);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.calculate);
            this.Name = "SSRExportForm";
            this.Text = "实时卫星钟差质量分析";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_getPath;
        private System.Windows.Forms.TextBox textBox_SSRsp3Pathes;
        private System.Windows.Forms.OpenFileDialog openFileDialog_result;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private Controls.BaseSatSelectingControl baseSatSelectingControl1;
        private System.Windows.Forms.Button calculate;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_interval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker_from;
        private System.Windows.Forms.DateTimePicker dateTimePicker_to;
        private System.Windows.Forms.ComboBox comboBox_name;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_filter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_inter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClockRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button button_to_excel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_NavPathes;
        private System.Windows.Forms.Button button_getNavPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog_Nav;
        private System.Windows.Forms.RadioButton BNC_Selected;
    }
}