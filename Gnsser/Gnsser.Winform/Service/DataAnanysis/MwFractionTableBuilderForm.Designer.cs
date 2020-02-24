namespace Gnsser.Winform
{
    partial class MwFractionTableBuilderForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_angleCut = new System.Windows.Forms.TextBox();
            this.checkBox1IsOutputFractionOfSmoothedMw = new System.Windows.Forms.CheckBox();
            this.namedIntControl_emptyRowCount = new Geo.Winform.Controls.NamedIntControl();
            this.checkBox_anasysOrTable = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSmoothRange = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.baseSatSelectingControl1 = new Gnsser.Winform.Controls.BaseSatSelectingControl();
            this.button_buidWideInts = new System.Windows.Forms.Button();
            this.fileOpenControl_periodDetails = new Geo.Winform.Controls.FileOpenControl();
            this.namedFloatControl_maxRms = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_intervalOFProduct = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox_outputRaw = new System.Windows.Forms.CheckBox();
            this.checkBox_outputSmooth = new System.Windows.Forms.CheckBox();
            this.namedIntControl_aveMinCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_aveMaxBeakCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedFloatControl_aveMaxDiffer = new Geo.Winform.Controls.NamedFloatControl();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage0.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage0.Size = new System.Drawing.Size(855, 273);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Size = new System.Drawing.Size(849, 119);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.namedFloatControl_intervalOFProduct);
            this.panel2.Controls.Add(this.checkBox_outputSmooth);
            this.panel2.Controls.Add(this.checkBox_outputRaw);
            this.panel2.Size = new System.Drawing.Size(849, 119);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.Add(this.namedFloatControl_maxRms);
            this.panel4.Controls.Add(this.checkBox_IsSmoothRange);
            this.panel4.Controls.Add(this.textBox_angleCut);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Size = new System.Drawing.Size(849, 119);
            this.panel4.Controls.SetChildIndex(this.panel5, 0);
            this.panel4.Controls.SetChildIndex(this.label1, 0);
            this.panel4.Controls.SetChildIndex(this.textBox_angleCut, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_IsSmoothRange, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl_maxRms, 0);
            this.panel4.Controls.SetChildIndex(this.groupBox2, 0);
            // 
            // fileOpenControl_inputPathes
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.LabelName = "O文件：";
            this.fileOpenControl_inputPathes.Location = new System.Drawing.Point(4, 4);
            this.fileOpenControl_inputPathes.Margin = new System.Windows.Forms.Padding(5);
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(847, 93);
            this.fileOpenControl_inputPathes.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // tabPage1
            // 
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(855, 125);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(553, 0);
            this.panel_buttons.Margin = new System.Windows.Forms.Padding(4);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.checkBox_anasysOrTable);
            this.panel5.Size = new System.Drawing.Size(608, 24);
            this.panel5.Controls.SetChildIndex(this.checkBox_anasysOrTable, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 62;
            this.label1.Text = "高度截止角：";
            // 
            // textBox_angleCut
            // 
            this.textBox_angleCut.Location = new System.Drawing.Point(92, 59);
            this.textBox_angleCut.Name = "textBox_angleCut";
            this.textBox_angleCut.Size = new System.Drawing.Size(48, 21);
            this.textBox_angleCut.TabIndex = 63;
            this.textBox_angleCut.Text = "20";
            // 
            // checkBox1IsOutputFractionOfSmoothedMw
            // 
            this.checkBox1IsOutputFractionOfSmoothedMw.AutoSize = true;
            this.checkBox1IsOutputFractionOfSmoothedMw.Location = new System.Drawing.Point(6, 42);
            this.checkBox1IsOutputFractionOfSmoothedMw.Name = "checkBox1IsOutputFractionOfSmoothedMw";
            this.checkBox1IsOutputFractionOfSmoothedMw.Size = new System.Drawing.Size(156, 16);
            this.checkBox1IsOutputFractionOfSmoothedMw.TabIndex = 64;
            this.checkBox1IsOutputFractionOfSmoothedMw.Text = "输出平滑MW后的小数部分";
            this.checkBox1IsOutputFractionOfSmoothedMw.UseVisualStyleBackColor = true;
            // 
            // namedIntControl_emptyRowCount
            // 
            this.namedIntControl_emptyRowCount.Location = new System.Drawing.Point(6, 19);
            this.namedIntControl_emptyRowCount.Margin = new System.Windows.Forms.Padding(4);
            this.namedIntControl_emptyRowCount.Name = "namedIntControl_emptyRowCount";
            this.namedIntControl_emptyRowCount.Size = new System.Drawing.Size(165, 23);
            this.namedIntControl_emptyRowCount.TabIndex = 67;
            this.namedIntControl_emptyRowCount.Title = "清除滤波前面数据：";
            this.namedIntControl_emptyRowCount.Value = 20;
            // 
            // checkBox_anasysOrTable
            // 
            this.checkBox_anasysOrTable.AutoSize = true;
            this.checkBox_anasysOrTable.ForeColor = System.Drawing.Color.Red;
            this.checkBox_anasysOrTable.Location = new System.Drawing.Point(103, 5);
            this.checkBox_anasysOrTable.Name = "checkBox_anasysOrTable";
            this.checkBox_anasysOrTable.Size = new System.Drawing.Size(126, 16);
            this.checkBox_anasysOrTable.TabIndex = 64;
            this.checkBox_anasysOrTable.Text = "分析器,否则表格法";
            this.checkBox_anasysOrTable.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSmoothRange
            // 
            this.checkBox_IsSmoothRange.AutoSize = true;
            this.checkBox_IsSmoothRange.Location = new System.Drawing.Point(20, 34);
            this.checkBox_IsSmoothRange.Name = "checkBox_IsSmoothRange";
            this.checkBox_IsSmoothRange.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsSmoothRange.TabIndex = 64;
            this.checkBox_IsSmoothRange.Text = "是否载波平滑伪距";
            this.checkBox_IsSmoothRange.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.namedFloatControl_aveMaxDiffer);
            this.groupBox2.Controls.Add(this.checkBox1IsOutputFractionOfSmoothedMw);
            this.groupBox2.Controls.Add(this.namedIntControl_aveMaxBeakCount);
            this.groupBox2.Controls.Add(this.namedIntControl_aveMinCount);
            this.groupBox2.Controls.Add(this.namedIntControl_emptyRowCount);
            this.groupBox2.Location = new System.Drawing.Point(146, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(457, 75);
            this.groupBox2.TabIndex = 68;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "表格法专用配置";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.baseSatSelectingControl1);
            this.groupBox3.Controls.Add(this.button_buidWideInts);
            this.groupBox3.Controls.Add(this.fileOpenControl_periodDetails);
            this.groupBox3.Location = new System.Drawing.Point(-4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(786, 116);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "生成星间单差宽巷模糊度差分整数产品";
            // 
            // baseSatSelectingControl1
            // 
            this.baseSatSelectingControl1.EnableBaseSat = false;
            this.baseSatSelectingControl1.Location = new System.Drawing.Point(4, 52);
            this.baseSatSelectingControl1.Margin = new System.Windows.Forms.Padding(4);
            this.baseSatSelectingControl1.Name = "baseSatSelectingControl1";
            this.baseSatSelectingControl1.Size = new System.Drawing.Size(602, 43);
            this.baseSatSelectingControl1.TabIndex = 19;
            // 
            // button_buidWideInts
            // 
            this.button_buidWideInts.Location = new System.Drawing.Point(612, 62);
            this.button_buidWideInts.Name = "button_buidWideInts";
            this.button_buidWideInts.Size = new System.Drawing.Size(158, 33);
            this.button_buidWideInts.TabIndex = 1;
            this.button_buidWideInts.Text = "生成宽巷差分整数";
            this.button_buidWideInts.UseVisualStyleBackColor = true;
            this.button_buidWideInts.Click += new System.EventHandler(this.button_buidWideInts_Click);
            // 
            // fileOpenControl_periodDetails
            // 
            this.fileOpenControl_periodDetails.AllowDrop = true;
            this.fileOpenControl_periodDetails.FilePath = "";
            this.fileOpenControl_periodDetails.FilePathes = new string[0];
            this.fileOpenControl_periodDetails.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_periodDetails.FirstPath = "";
            this.fileOpenControl_periodDetails.IsMultiSelect = false;
            this.fileOpenControl_periodDetails.LabelName = "文件：";
            this.fileOpenControl_periodDetails.Location = new System.Drawing.Point(19, 20);
            this.fileOpenControl_periodDetails.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl_periodDetails.Name = "fileOpenControl_periodDetails";
            this.fileOpenControl_periodDetails.Size = new System.Drawing.Size(736, 22);
            this.fileOpenControl_periodDetails.TabIndex = 0;
            // 
            // namedFloatControl_maxRms
            // 
            this.namedFloatControl_maxRms.Location = new System.Drawing.Point(2, 86);
            this.namedFloatControl_maxRms.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_maxRms.Name = "namedFloatControl_maxRms";
            this.namedFloatControl_maxRms.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_maxRms.TabIndex = 69;
            this.namedFloatControl_maxRms.Title = "最大允许RMS：";
            this.namedFloatControl_maxRms.Value = 0.6D;
            // 
            // namedFloatControl_intervalOFProduct
            // 
            this.namedFloatControl_intervalOFProduct.Location = new System.Drawing.Point(157, 17);
            this.namedFloatControl_intervalOFProduct.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_intervalOFProduct.Name = "namedFloatControl_intervalOFProduct";
            this.namedFloatControl_intervalOFProduct.Size = new System.Drawing.Size(162, 23);
            this.namedFloatControl_intervalOFProduct.TabIndex = 69;
            this.namedFloatControl_intervalOFProduct.Title = "产品采样间隔(分)：";
            this.namedFloatControl_intervalOFProduct.Value = 20D;
            // 
            // checkBox_outputRaw
            // 
            this.checkBox_outputRaw.AutoSize = true;
            this.checkBox_outputRaw.Checked = true;
            this.checkBox_outputRaw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_outputRaw.Location = new System.Drawing.Point(23, 17);
            this.checkBox_outputRaw.Name = "checkBox_outputRaw";
            this.checkBox_outputRaw.Size = new System.Drawing.Size(84, 16);
            this.checkBox_outputRaw.TabIndex = 0;
            this.checkBox_outputRaw.Text = "输出原始MW";
            this.checkBox_outputRaw.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputSmooth
            // 
            this.checkBox_outputSmooth.AutoSize = true;
            this.checkBox_outputSmooth.Location = new System.Drawing.Point(23, 51);
            this.checkBox_outputSmooth.Name = "checkBox_outputSmooth";
            this.checkBox_outputSmooth.Size = new System.Drawing.Size(84, 16);
            this.checkBox_outputSmooth.TabIndex = 0;
            this.checkBox_outputSmooth.Text = "输出平滑MW";
            this.checkBox_outputSmooth.UseVisualStyleBackColor = true;
            // 
            // namedIntControl_aveMinCount
            // 
            this.namedIntControl_aveMinCount.Location = new System.Drawing.Point(200, 10);
            this.namedIntControl_aveMinCount.Margin = new System.Windows.Forms.Padding(4);
            this.namedIntControl_aveMinCount.Name = "namedIntControl_aveMinCount";
            this.namedIntControl_aveMinCount.Size = new System.Drawing.Size(181, 23);
            this.namedIntControl_aveMinCount.TabIndex = 67;
            this.namedIntControl_aveMinCount.Title = "时段平均最小数目：";
            this.namedIntControl_aveMinCount.Value = 8;
            // 
            // namedIntControl_aveMaxBeakCount
            // 
            this.namedIntControl_aveMaxBeakCount.Location = new System.Drawing.Point(200, 34);
            this.namedIntControl_aveMaxBeakCount.Margin = new System.Windows.Forms.Padding(4);
            this.namedIntControl_aveMaxBeakCount.Name = "namedIntControl_aveMaxBeakCount";
            this.namedIntControl_aveMaxBeakCount.Size = new System.Drawing.Size(181, 23);
            this.namedIntControl_aveMaxBeakCount.TabIndex = 67;
            this.namedIntControl_aveMaxBeakCount.Title = "时段平均最大间隔数：";
            this.namedIntControl_aveMaxBeakCount.Value = 1;
            // 
            // namedFloatControl_aveMaxDiffer
            // 
            this.namedFloatControl_aveMaxDiffer.Location = new System.Drawing.Point(228, 53);
            this.namedFloatControl_aveMaxDiffer.Name = "namedFloatControl_aveMaxDiffer";
            this.namedFloatControl_aveMaxDiffer.Size = new System.Drawing.Size(172, 23);
            this.namedFloatControl_aveMaxDiffer.TabIndex = 68;
            this.namedFloatControl_aveMaxDiffer.Title = "时段平均最大偏差：";
            this.namedFloatControl_aveMaxDiffer.Value = 2D;
            this.namedFloatControl_aveMaxDiffer.Load += new System.EventHandler(this.namedFloatControl1_Load);
            // 
            // MwFractionTableBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 530);
            this.IsShowProgressBar = true;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MwFractionTableBuilderForm";
            this.OutputDirectory = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp";
            this.Text = "MW提取器/宽巷产品生成";
            this.Load += new System.EventHandler(this.MwTableBuilderForm_Load);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_angleCut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1IsOutputFractionOfSmoothedMw;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_emptyRowCount;
        private System.Windows.Forms.CheckBox checkBox_anasysOrTable;
        private System.Windows.Forms.CheckBox checkBox_IsSmoothRange;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_buidWideInts;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_periodDetails;
        private Controls.BaseSatSelectingControl baseSatSelectingControl1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxRms;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_intervalOFProduct;
        private System.Windows.Forms.CheckBox checkBox_outputSmooth;
        private System.Windows.Forms.CheckBox checkBox_outputRaw;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_aveMaxDiffer;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_aveMaxBeakCount;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_aveMinCount;
    }
}