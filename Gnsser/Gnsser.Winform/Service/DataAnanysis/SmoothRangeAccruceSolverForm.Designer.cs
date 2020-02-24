namespace Gnsser.Winform
{
    partial class SmoothRangeAccruceSolverForm
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
            this.button_solve = new System.Windows.Forms.Button();
            this.button_multiSolveTimes = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox_IsShowCA = new System.Windows.Forms.CheckBox();
            this.button_multiDelta = new System.Windows.Forms.Button();
            this.namedIntControl_epochCount = new Geo.Winform.Controls.NamedIntControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_deltaOfC = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_deltaOfP = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_step = new Geo.Winform.Controls.NamedFloatControl();
            this.enumRadioControl_frequenceTypes = new Geo.Winform.EnumRadioControl();
            this.enumRadioControl_satelliteType = new Geo.Winform.EnumRadioControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button_solveWantedEpochCount = new System.Windows.Forms.Button();
            this.namedFloatControl_wantedRms = new Geo.Winform.Controls.NamedFloatControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_solve
            // 
            this.button_solve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solve.Location = new System.Drawing.Point(699, 40);
            this.button_solve.Name = "button_solve";
            this.button_solve.Size = new System.Drawing.Size(75, 28);
            this.button_solve.TabIndex = 1;
            this.button_solve.Text = "计算";
            this.button_solve.UseVisualStyleBackColor = true;
            this.button_solve.Click += new System.EventHandler(this.button_solve_Click);
            // 
            // button_multiSolveTimes
            // 
            this.button_multiSolveTimes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_multiSolveTimes.Location = new System.Drawing.Point(637, 30);
            this.button_multiSolveTimes.Name = "button_multiSolveTimes";
            this.button_multiSolveTimes.Size = new System.Drawing.Size(132, 38);
            this.button_multiSolveTimes.TabIndex = 1;
            this.button_multiSolveTimes.Text = "批量计算加速倍数";
            this.button_multiSolveTimes.UseVisualStyleBackColor = true;
            this.button_multiSolveTimes.Click += new System.EventHandler(this.button_multiSolve_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(3, 154);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(797, 100);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button_solve);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(789, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "单个计算";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox_IsShowCA);
            this.tabPage2.Controls.Add(this.button_multiDelta);
            this.tabPage2.Controls.Add(this.button_multiSolveTimes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(789, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "批量计算";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsShowCA
            // 
            this.checkBox_IsShowCA.AutoSize = true;
            this.checkBox_IsShowCA.Location = new System.Drawing.Point(48, 19);
            this.checkBox_IsShowCA.Name = "checkBox_IsShowCA";
            this.checkBox_IsShowCA.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsShowCA.TabIndex = 2;
            this.checkBox_IsShowCA.Text = "显示CA码";
            this.checkBox_IsShowCA.UseVisualStyleBackColor = true;
            // 
            // button_multiDelta
            // 
            this.button_multiDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_multiDelta.Location = new System.Drawing.Point(467, 30);
            this.button_multiDelta.Name = "button_multiDelta";
            this.button_multiDelta.Size = new System.Drawing.Size(132, 38);
            this.button_multiDelta.TabIndex = 1;
            this.button_multiDelta.Text = "批量计算均方差";
            this.button_multiDelta.UseVisualStyleBackColor = true;
            this.button_multiDelta.Click += new System.EventHandler(this.button_multiDelta_Click);
            // 
            // namedIntControl_epochCount
            // 
            this.namedIntControl_epochCount.Location = new System.Drawing.Point(11, 93);
            this.namedIntControl_epochCount.Name = "namedIntControl_epochCount";
            this.namedIntControl_epochCount.Size = new System.Drawing.Size(183, 23);
            this.namedIntControl_epochCount.TabIndex = 2;
            this.namedIntControl_epochCount.Title = "历元数：";
            this.namedIntControl_epochCount.Value = 100;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.namedFloatControl_deltaOfC);
            this.groupBox1.Controls.Add(this.namedFloatControl_deltaOfP);
            this.groupBox1.Controls.Add(this.namedFloatControl_step);
            this.groupBox1.Controls.Add(this.namedIntControl_epochCount);
            this.groupBox1.Controls.Add(this.enumRadioControl_frequenceTypes);
            this.groupBox1.Controls.Add(this.enumRadioControl_satelliteType);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(793, 146);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "公共设置";
            // 
            // namedFloatControl_deltaOfC
            // 
            this.namedFloatControl_deltaOfC.Location = new System.Drawing.Point(201, 123);
            this.namedFloatControl_deltaOfC.Name = "namedFloatControl_deltaOfC";
            this.namedFloatControl_deltaOfC.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_deltaOfC.TabIndex = 3;
            this.namedFloatControl_deltaOfC.Title = "CA码精度：";
            this.namedFloatControl_deltaOfC.Value = 3D;
            // 
            // namedFloatControl_deltaOfP
            // 
            this.namedFloatControl_deltaOfP.Location = new System.Drawing.Point(11, 123);
            this.namedFloatControl_deltaOfP.Name = "namedFloatControl_deltaOfP";
            this.namedFloatControl_deltaOfP.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_deltaOfP.TabIndex = 3;
            this.namedFloatControl_deltaOfP.Title = "P码精度：";
            this.namedFloatControl_deltaOfP.Value = 0.3D;
            // 
            // namedFloatControl_step
            // 
            this.namedFloatControl_step.Location = new System.Drawing.Point(215, 93);
            this.namedFloatControl_step.Name = "namedFloatControl_step";
            this.namedFloatControl_step.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_step.TabIndex = 3;
            this.namedFloatControl_step.Title = "步长：";
            this.namedFloatControl_step.Value = 0.01D;
            // 
            // enumRadioControl_frequenceTypes
            // 
            this.enumRadioControl_frequenceTypes.Location = new System.Drawing.Point(347, 20);
            this.enumRadioControl_frequenceTypes.Name = "enumRadioControl_frequenceTypes";
            this.enumRadioControl_frequenceTypes.Size = new System.Drawing.Size(311, 67);
            this.enumRadioControl_frequenceTypes.TabIndex = 0;
            this.enumRadioControl_frequenceTypes.Title = "频率类型";
            this.enumRadioControl_frequenceTypes.EnumItemSelected += new System.Action<string, bool>(this.enumRadioControl_frequenceTypes_EnumItemSelected);
            // 
            // enumRadioControl_satelliteType
            // 
            this.enumRadioControl_satelliteType.Location = new System.Drawing.Point(9, 20);
            this.enumRadioControl_satelliteType.Name = "enumRadioControl_satelliteType";
            this.enumRadioControl_satelliteType.Size = new System.Drawing.Size(332, 67);
            this.enumRadioControl_satelliteType.TabIndex = 0;
            this.enumRadioControl_satelliteType.Title = "卫星类型";
            this.enumRadioControl_satelliteType.EnumItemSelected += new System.Action<string, bool>(this.enumRadioControl_satelliteType_EnumItemSelected);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(7, 260);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(789, 184);
            this.tabControl2.TabIndex = 4;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(781, 158);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "文本结果";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(775, 152);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(781, 158);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "表格结果";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(775, 152);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.namedFloatControl_wantedRms);
            this.tabPage5.Controls.Add(this.button_solveWantedEpochCount);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(789, 74);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "精度计算";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button_solveWantedEpochCount
            // 
            this.button_solveWantedEpochCount.Location = new System.Drawing.Point(448, 23);
            this.button_solveWantedEpochCount.Name = "button_solveWantedEpochCount";
            this.button_solveWantedEpochCount.Size = new System.Drawing.Size(123, 45);
            this.button_solveWantedEpochCount.TabIndex = 0;
            this.button_solveWantedEpochCount.Text = "计算需求历元数";
            this.button_solveWantedEpochCount.UseVisualStyleBackColor = true;
            this.button_solveWantedEpochCount.Click += new System.EventHandler(this.button_solveWantedEpochCount_Click);
            // 
            // namedFloatControl_wantedRms
            // 
            this.namedFloatControl_wantedRms.Location = new System.Drawing.Point(50, 23);
            this.namedFloatControl_wantedRms.Name = "namedFloatControl_wantedRms";
            this.namedFloatControl_wantedRms.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_wantedRms.TabIndex = 3;
            this.namedFloatControl_wantedRms.Title = "需求精度(m)：";
            this.namedFloatControl_wantedRms.Value = 0.1D;
            // 
            // SmoothRangeAccruceSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 446);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Name = "SmoothRangeAccruceSolverForm";
            this.Text = "平滑伪距精度计算器";
            this.Load += new System.EventHandler(this.SmoothRangeAccruceSolverForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_solve;
        private System.Windows.Forms.Button button_multiSolveTimes;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.EnumRadioControl enumRadioControl_frequenceTypes;
        private Geo.Winform.EnumRadioControl enumRadioControl_satelliteType;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_epochCount;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_step;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_deltaOfC;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_deltaOfP;
        private System.Windows.Forms.Button button_multiDelta;
        private System.Windows.Forms.CheckBox checkBox_IsShowCA;
        private System.Windows.Forms.TabPage tabPage5;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_wantedRms;
        private System.Windows.Forms.Button button_solveWantedEpochCount;
    }
}