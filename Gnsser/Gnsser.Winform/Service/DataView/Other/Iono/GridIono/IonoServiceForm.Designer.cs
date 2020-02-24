namespace Gnsser.Winform
{
    partial class IonoServiceForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_exortTable = new System.Windows.Forms.Button();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_drawAllRms = new System.Windows.Forms.Button();
            this.button_drawAll = new System.Windows.Forms.Button();
            this.button_read = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_drawRms = new System.Windows.Forms.Button();
            this.button_draw = new System.Windows.Forms.Button();
            this.button_filterPrn = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.bindingSource_times = new System.Windows.Forms.BindingSource(this.components);
            this.checkBox_showRMS = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedTimeControl1 = new Geo.Winform.Controls.NamedTimeControl();
            this.button_calculate = new System.Windows.Forms.Button();
            this.namedStringControl1_geoCoord = new Geo.Winform.Controls.NamedStringControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button_drawMultiRms = new System.Windows.Forms.Button();
            this.button_drawMultiCacu = new System.Windows.Forms.Button();
            this.namedTimeControl2 = new Geo.Winform.Controls.NamedTimeControl();
            this.button_multiCalcu = new System.Windows.Forms.Button();
            this.geoGridLoopControl1 = new Geo.Winform.Controls.GeoGridLoopControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.bindingSource_sections = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource_records = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource_lats = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_times)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_records)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_lats)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button_exortTable);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarComponent1);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(648, 405);
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.TabIndex = 0;
            // 
            // button_exortTable
            // 
            this.button_exortTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exortTable.Location = new System.Drawing.Point(546, 155);
            this.button_exortTable.Name = "button_exortTable";
            this.button_exortTable.Size = new System.Drawing.Size(75, 23);
            this.button_exortTable.TabIndex = 20;
            this.button_exortTable.Text = "导出表格数据";
            this.button_exortTable.UseVisualStyleBackColor = true;
            this.button_exortTable.Click += new System.EventHandler(this.button_exortTable_Click);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(32, 144);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(468, 34);
            this.progressBarComponent1.TabIndex = 19;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(7, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(634, 134);
            this.tabControl2.TabIndex = 18;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.fileOpenControl1);
            this.tabPage3.Controls.Add(this.button_drawAllRms);
            this.tabPage3.Controls.Add(this.button_drawAll);
            this.tabPage3.Controls.Add(this.button_read);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.button_drawRms);
            this.tabPage3.Controls.Add(this.button_draw);
            this.tabPage3.Controls.Add(this.button_filterPrn);
            this.tabPage3.Controls.Add(this.comboBox1);
            this.tabPage3.Controls.Add(this.checkBox_showRMS);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(626, 108);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "输入";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "Iono文件|*.??i|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(6, 11);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(526, 22);
            this.fileOpenControl1.TabIndex = 6;
            // 
            // button_drawAllRms
            // 
            this.button_drawAllRms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawAllRms.Location = new System.Drawing.Point(535, 77);
            this.button_drawAllRms.Name = "button_drawAllRms";
            this.button_drawAllRms.Size = new System.Drawing.Size(75, 23);
            this.button_drawAllRms.TabIndex = 16;
            this.button_drawAllRms.Text = "绘全RMS";
            this.button_drawAllRms.UseVisualStyleBackColor = true;
            this.button_drawAllRms.Click += new System.EventHandler(this.button_drawAllRms_Click);
            // 
            // button_drawAll
            // 
            this.button_drawAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawAll.Location = new System.Drawing.Point(454, 78);
            this.button_drawAll.Name = "button_drawAll";
            this.button_drawAll.Size = new System.Drawing.Size(75, 23);
            this.button_drawAll.TabIndex = 17;
            this.button_drawAll.Text = "绘全时刻";
            this.button_drawAll.UseVisualStyleBackColor = true;
            this.button_drawAll.Click += new System.EventHandler(this.button_drawAll_Click);
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(535, 11);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(85, 22);
            this.button_read.TabIndex = 7;
            this.button_read.Text = "读取并显示";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "历元：";
            // 
            // button_drawRms
            // 
            this.button_drawRms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawRms.Location = new System.Drawing.Point(542, 45);
            this.button_drawRms.Name = "button_drawRms";
            this.button_drawRms.Size = new System.Drawing.Size(63, 23);
            this.button_drawRms.TabIndex = 15;
            this.button_drawRms.Text = "绘 RMS";
            this.button_drawRms.UseVisualStyleBackColor = true;
            this.button_drawRms.Click += new System.EventHandler(this.button_drawRms_Click);
            // 
            // button_draw
            // 
            this.button_draw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_draw.Location = new System.Drawing.Point(452, 45);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(75, 23);
            this.button_draw.TabIndex = 14;
            this.button_draw.Text = "绘图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // button_filterPrn
            // 
            this.button_filterPrn.Location = new System.Drawing.Point(234, 40);
            this.button_filterPrn.Name = "button_filterPrn";
            this.button_filterPrn.Size = new System.Drawing.Size(103, 23);
            this.button_filterPrn.TabIndex = 4;
            this.button_filterPrn.Text = "显示当前历元";
            this.button_filterPrn.UseVisualStyleBackColor = true;
            this.button_filterPrn.Click += new System.EventHandler(this.button_filterTime_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.bindingSource_times;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormatString = "F";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(53, 40);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(163, 20);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // bindingSource_times
            // 
            this.bindingSource_times.CurrentChanged += new System.EventHandler(this.bindingSource_times_CurrentChanged);
            // 
            // checkBox_showRMS
            // 
            this.checkBox_showRMS.AutoSize = true;
            this.checkBox_showRMS.Location = new System.Drawing.Point(349, 44);
            this.checkBox_showRMS.Name = "checkBox_showRMS";
            this.checkBox_showRMS.Size = new System.Drawing.Size(66, 16);
            this.checkBox_showRMS.TabIndex = 13;
            this.checkBox_showRMS.Text = "显示RMS";
            this.checkBox_showRMS.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(626, 108);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "单个计算";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.namedTimeControl1);
            this.groupBox1.Controls.Add(this.button_calculate);
            this.groupBox1.Controls.Add(this.namedStringControl1_geoCoord);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 83);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务";
            // 
            // namedTimeControl1
            // 
            this.namedTimeControl1.Location = new System.Drawing.Point(27, 20);
            this.namedTimeControl1.Name = "namedTimeControl1";
            this.namedTimeControl1.Size = new System.Drawing.Size(246, 23);
            this.namedTimeControl1.TabIndex = 9;
            this.namedTimeControl1.Title = "时间：";
            // 
            // button_calculate
            // 
            this.button_calculate.Location = new System.Drawing.Point(302, 18);
            this.button_calculate.Name = "button_calculate";
            this.button_calculate.Size = new System.Drawing.Size(75, 23);
            this.button_calculate.TabIndex = 11;
            this.button_calculate.Text = "计算";
            this.button_calculate.UseVisualStyleBackColor = true;
            this.button_calculate.Click += new System.EventHandler(this.button_calculate_Click);
            // 
            // namedStringControl1_geoCoord
            // 
            this.namedStringControl1_geoCoord.Location = new System.Drawing.Point(2, 49);
            this.namedStringControl1_geoCoord.Name = "namedStringControl1_geoCoord";
            this.namedStringControl1_geoCoord.Size = new System.Drawing.Size(271, 23);
            this.namedStringControl1_geoCoord.TabIndex = 10;
            this.namedStringControl1_geoCoord.Title = "地心坐标：";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.button_drawMultiRms);
            this.tabPage5.Controls.Add(this.button_drawMultiCacu);
            this.tabPage5.Controls.Add(this.namedTimeControl2);
            this.tabPage5.Controls.Add(this.button_multiCalcu);
            this.tabPage5.Controls.Add(this.geoGridLoopControl1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(626, 108);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "批量计算";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button_drawMultiRms
            // 
            this.button_drawMultiRms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawMultiRms.Location = new System.Drawing.Point(479, 67);
            this.button_drawMultiRms.Name = "button_drawMultiRms";
            this.button_drawMultiRms.Size = new System.Drawing.Size(63, 23);
            this.button_drawMultiRms.TabIndex = 17;
            this.button_drawMultiRms.Text = "绘 RMS";
            this.button_drawMultiRms.UseVisualStyleBackColor = true;
            this.button_drawMultiRms.Click += new System.EventHandler(this.button_drawMultiRms_Click);
            // 
            // button_drawMultiCacu
            // 
            this.button_drawMultiCacu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawMultiCacu.Location = new System.Drawing.Point(389, 67);
            this.button_drawMultiCacu.Name = "button_drawMultiCacu";
            this.button_drawMultiCacu.Size = new System.Drawing.Size(75, 23);
            this.button_drawMultiCacu.TabIndex = 16;
            this.button_drawMultiCacu.Text = "绘图";
            this.button_drawMultiCacu.UseVisualStyleBackColor = true;
            this.button_drawMultiCacu.Click += new System.EventHandler(this.button_drawMultiCacu_Click);
            // 
            // namedTimeControl2
            // 
            this.namedTimeControl2.Location = new System.Drawing.Point(6, 67);
            this.namedTimeControl2.Name = "namedTimeControl2";
            this.namedTimeControl2.Size = new System.Drawing.Size(246, 23);
            this.namedTimeControl2.TabIndex = 12;
            this.namedTimeControl2.Title = "时间：";
            // 
            // button_multiCalcu
            // 
            this.button_multiCalcu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_multiCalcu.Location = new System.Drawing.Point(303, 67);
            this.button_multiCalcu.Name = "button_multiCalcu";
            this.button_multiCalcu.Size = new System.Drawing.Size(75, 23);
            this.button_multiCalcu.TabIndex = 13;
            this.button_multiCalcu.Text = "计算";
            this.button_multiCalcu.UseVisualStyleBackColor = true;
            this.button_multiCalcu.Click += new System.EventHandler(this.button_multiCalcu_Click);
            // 
            // geoGridLoopControl1
            // 
            this.geoGridLoopControl1.Location = new System.Drawing.Point(2, 5);
            this.geoGridLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.geoGridLoopControl1.Name = "geoGridLoopControl1";
            this.geoGridLoopControl1.Size = new System.Drawing.Size(540, 57);
            this.geoGridLoopControl1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(648, 215);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(640, 189);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(634, 183);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(640, 189);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "详细信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(634, 183);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // bindingSource_lats
            // 
            this.bindingSource_lats.CurrentChanged += new System.EventHandler(this.bindingSource_lats_CurrentChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "电子数量，单位 1e16";
            // 
            // IonoServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 405);
            this.Controls.Add(this.splitContainer1);
            this.Name = "IonoServiceForm";
            this.Text = "电离层服务";
            this.Load += new System.EventHandler(this.IgsFcbViewerForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_times)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_records)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_lats)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource bindingSource_sections;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bindingSource_times;
        private System.Windows.Forms.Button button_filterPrn;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.BindingSource bindingSource_records;
        private System.Windows.Forms.BindingSource bindingSource_lats;
        private Geo.Winform.Controls.NamedStringControl namedStringControl1_geoCoord;
        private Geo.Winform.Controls.NamedTimeControl namedTimeControl1;
        private System.Windows.Forms.Button button_calculate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_showRMS;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.Button button_drawRms;
        private System.Windows.Forms.Button button_drawAllRms;
        private System.Windows.Forms.Button button_drawAll;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private Geo.Winform.Controls.NamedTimeControl namedTimeControl2;
        private System.Windows.Forms.Button button_multiCalcu;
        private Geo.Winform.Controls.GeoGridLoopControl geoGridLoopControl1;
        private System.Windows.Forms.Button button_drawMultiRms;
        private System.Windows.Forms.Button button_drawMultiCacu;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.Windows.Forms.Button button_exortTable;
        private System.Windows.Forms.Label label2;
    }
}