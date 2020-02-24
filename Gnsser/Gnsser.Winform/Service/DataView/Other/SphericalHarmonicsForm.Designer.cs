namespace Gnsser.Winform
{
    partial class SphericalHarmonicsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.button_read = new System.Windows.Forms.Button();
            this.button_calculate = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button_draw = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonSinglePt = new System.Windows.Forms.Button();
            this.radioButtonXYZ = new System.Windows.Forms.RadioButton();
            this.radioButtonLonLat = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_isCancel = new System.Windows.Forms.Button();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.namedStringControlCoord = new Geo.Winform.Controls.NamedStringControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.geoGridLoopControl1 = new Geo.Winform.Controls.GeoGridLoopControl();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.directorySelectionControl_output = new Geo.Winform.Controls.DirectorySelectionControl();
            this.namedFloatControl_height = new Geo.Winform.Controls.NamedFloatControl();
            this.namedIntControl_order = new Geo.Winform.Controls.NamedIntControl();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.parallelConfigControl1 = new Geo.Winform.Controls.ParallelConfigControl();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.directorySelectionControl_output);
            this.groupBox1.Controls.Add(this.namedFloatControl_height);
            this.groupBox1.Controls.Add(this.namedIntControl_order);
            this.groupBox1.Controls.Add(this.buttonWrite);
            this.groupBox1.Controls.Add(this.button_read);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(711, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "平均半径：6378136.3米";
            // 
            // buttonWrite
            // 
            this.buttonWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWrite.Location = new System.Drawing.Point(629, 38);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(75, 26);
            this.buttonWrite.TabIndex = 1;
            this.buttonWrite.Text = "原始写出";
            this.buttonWrite.UseVisualStyleBackColor = true;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(630, 13);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // button_calculate
            // 
            this.button_calculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_calculate.Location = new System.Drawing.Point(447, 85);
            this.button_calculate.Name = "button_calculate";
            this.button_calculate.Size = new System.Drawing.Size(75, 25);
            this.button_calculate.TabIndex = 1;
            this.button_calculate.Text = "计算";
            this.button_calculate.UseVisualStyleBackColor = true;
            this.button_calculate.Click += new System.EventHandler(this.button_calculate_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // button_draw
            // 
            this.button_draw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_draw.Location = new System.Drawing.Point(613, 83);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(75, 25);
            this.button_draw.TabIndex = 1;
            this.button_draw.Text = "绘图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 111);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(712, 142);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonSinglePt);
            this.tabPage1.Controls.Add(this.radioButtonXYZ);
            this.tabPage1.Controls.Add(this.radioButtonLonLat);
            this.tabPage1.Controls.Add(this.namedStringControlCoord);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(612, 116);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "单点计算";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonSinglePt
            // 
            this.buttonSinglePt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSinglePt.Location = new System.Drawing.Point(517, 45);
            this.buttonSinglePt.Name = "buttonSinglePt";
            this.buttonSinglePt.Size = new System.Drawing.Size(75, 23);
            this.buttonSinglePt.TabIndex = 3;
            this.buttonSinglePt.Text = "计算";
            this.buttonSinglePt.UseVisualStyleBackColor = true;
            this.buttonSinglePt.Click += new System.EventHandler(this.buttonSinglePt_Click);
            // 
            // radioButtonXYZ
            // 
            this.radioButtonXYZ.AutoSize = true;
            this.radioButtonXYZ.Location = new System.Drawing.Point(233, 48);
            this.radioButtonXYZ.Name = "radioButtonXYZ";
            this.radioButtonXYZ.Size = new System.Drawing.Size(95, 16);
            this.radioButtonXYZ.TabIndex = 2;
            this.radioButtonXYZ.Text = "空间直角坐标";
            this.radioButtonXYZ.UseVisualStyleBackColor = true;
            // 
            // radioButtonLonLat
            // 
            this.radioButtonLonLat.AutoSize = true;
            this.radioButtonLonLat.Checked = true;
            this.radioButtonLonLat.Location = new System.Drawing.Point(60, 48);
            this.radioButtonLonLat.Name = "radioButtonLonLat";
            this.radioButtonLonLat.Size = new System.Drawing.Size(167, 16);
            this.radioButtonLonLat.TabIndex = 1;
            this.radioButtonLonLat.TabStop = true;
            this.radioButtonLonLat.Text = "经纬度(采用上面的球半径)";
            this.radioButtonLonLat.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.parallelConfigControl1);
            this.tabPage2.Controls.Add(this.button_isCancel);
            this.tabPage2.Controls.Add(this.button_calculate);
            this.tabPage2.Controls.Add(this.button_draw);
            this.tabPage2.Controls.Add(this.progressBarComponent1);
            this.tabPage2.Controls.Add(this.geoGridLoopControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(704, 116);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "区域批量计算";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_isCancel
            // 
            this.button_isCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_isCancel.Location = new System.Drawing.Point(524, 84);
            this.button_isCancel.Name = "button_isCancel";
            this.button_isCancel.Size = new System.Drawing.Size(83, 25);
            this.button_isCancel.TabIndex = 3;
            this.button_isCancel.Text = "取消";
            this.button_isCancel.UseVisualStyleBackColor = true;
            this.button_isCancel.Click += new System.EventHandler(this.button_isCancel_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // namedStringControlCoord
            // 
            this.namedStringControlCoord.Location = new System.Drawing.Point(17, 16);
            this.namedStringControlCoord.Name = "namedStringControlCoord";
            this.namedStringControlCoord.Size = new System.Drawing.Size(379, 23);
            this.namedStringControlCoord.TabIndex = 0;
            this.namedStringControlCoord.Title = "坐标：";
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(46, 72);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(394, 34);
            this.progressBarComponent1.TabIndex = 2;
            // 
            // geoGridLoopControl1
            // 
            this.geoGridLoopControl1.Location = new System.Drawing.Point(6, 9);
            this.geoGridLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.geoGridLoopControl1.Name = "geoGridLoopControl1";
            this.geoGridLoopControl1.Size = new System.Drawing.Size(540, 57);
            this.geoGridLoopControl1.TabIndex = 2;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(12, 285);
            this.richTextBoxControl1.MaxAppendLineCount = 10000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(712, 94);
            this.richTextBoxControl1.TabIndex = 1;
            this.richTextBoxControl1.Text = "";
            // 
            // directorySelectionControl_output
            // 
            this.directorySelectionControl_output.AllowDrop = true;
            this.directorySelectionControl_output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl_output.IsMultiPathes = false;
            this.directorySelectionControl_output.LabelName = "输出目录：";
            this.directorySelectionControl_output.Location = new System.Drawing.Point(-1, 65);
            this.directorySelectionControl_output.Name = "directorySelectionControl_output";
            this.directorySelectionControl_output.Path = "";
            this.directorySelectionControl_output.Pathes = new string[0];
            this.directorySelectionControl_output.Size = new System.Drawing.Size(688, 22);
            this.directorySelectionControl_output.TabIndex = 5;
            // 
            // namedFloatControl_height
            // 
            this.namedFloatControl_height.Location = new System.Drawing.Point(6, 38);
            this.namedFloatControl_height.Name = "namedFloatControl_height";
            this.namedFloatControl_height.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_height.TabIndex = 4;
            this.namedFloatControl_height.Title = "球面高：";
            this.namedFloatControl_height.Value = 10000D;
            // 
            // namedIntControl_order
            // 
            this.namedIntControl_order.Location = new System.Drawing.Point(358, 38);
            this.namedIntControl_order.Name = "namedIntControl_order";
            this.namedIntControl_order.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_order.TabIndex = 3;
            this.namedIntControl_order.Title = "阶次：";
            this.namedIntControl_order.Value = 100;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "文本文件|*.dat;*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(17, 13);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(607, 22);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // parallelConfigControl1
            // 
            this.parallelConfigControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.parallelConfigControl1.EnableParallel = true;
            this.parallelConfigControl1.Location = new System.Drawing.Point(460, 9);
            this.parallelConfigControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.parallelConfigControl1.Name = "parallelConfigControl1";
            this.parallelConfigControl1.Size = new System.Drawing.Size(241, 26);
            this.parallelConfigControl1.TabIndex = 4;
            // 
            // SphericalHarmonicsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 391);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.richTextBoxControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "SphericalHarmonicsForm";
            this.Text = "SphericalHarmonicsForm";
            this.Load += new System.EventHandler(this.SphericalHarmonicsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_read;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private Geo.Winform.Controls.GeoGridLoopControl geoGridLoopControl1;
        private System.Windows.Forms.Button button_calculate;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_order;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_height;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl_output;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.NamedStringControl namedStringControlCoord;
        private System.Windows.Forms.RadioButton radioButtonXYZ;
        private System.Windows.Forms.RadioButton radioButtonLonLat;
        private System.Windows.Forms.Button buttonSinglePt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_isCancel;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private Geo.Winform.Controls.ParallelConfigControl parallelConfigControl1;
    }
}