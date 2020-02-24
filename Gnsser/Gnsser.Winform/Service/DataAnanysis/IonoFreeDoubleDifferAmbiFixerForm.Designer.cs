namespace Gnsser.Winform
{
    partial class IonoFreeDoubleDifferAmbiFixerForm
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxIsPhaseInMetterOrCycle = new System.Windows.Forms.CheckBox();
            this.checkBox_phaseSmoothRange = new System.Windows.Forms.CheckBox();
            this.textBox_angleCut = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.bindingSource_prns = new System.Windows.Forms.BindingSource(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.fileOpenContol_obsFiles = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_floatAmbiFiles = new Geo.Winform.Controls.FileOpenControl();
            this.namedIntControl_emptyRowCount = new Geo.Winform.Controls.NamedIntControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.objectTableControlA = new Geo.Winform.ObjectTableControl();
            this.objectTableControlB = new Geo.Winform.ObjectTableControl();
            this.objectTableControlC = new Geo.Winform.ObjectTableControl();
            this.objectTableControlD = new Geo.Winform.ObjectTableControl();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.namedFloatControl1_maxDifferOfIntFloat = new Geo.Winform.Controls.NamedFloatControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prns)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(753, 517);
            this.splitContainer1.SplitterDistance = 234;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(753, 234);
            this.splitContainer2.SplitterDistance = 186;
            this.splitContainer2.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(753, 186);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenContol_obsFiles);
            this.tabPage1.Controls.Add(this.fileOpenControl_floatAmbiFiles);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(745, 157);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文件输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.namedFloatControl1_maxDifferOfIntFloat);
            this.tabPage2.Controls.Add(this.checkBoxIsPhaseInMetterOrCycle);
            this.tabPage2.Controls.Add(this.checkBox_phaseSmoothRange);
            this.tabPage2.Controls.Add(this.textBox_angleCut);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.namedIntControl_emptyRowCount);
            this.tabPage2.Controls.Add(this.multiGnssSystemSelectControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(745, 157);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsPhaseInMetterOrCycle
            // 
            this.checkBoxIsPhaseInMetterOrCycle.AutoSize = true;
            this.checkBoxIsPhaseInMetterOrCycle.Checked = true;
            this.checkBoxIsPhaseInMetterOrCycle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIsPhaseInMetterOrCycle.Location = new System.Drawing.Point(17, 79);
            this.checkBoxIsPhaseInMetterOrCycle.Name = "checkBoxIsPhaseInMetterOrCycle";
            this.checkBoxIsPhaseInMetterOrCycle.Size = new System.Drawing.Size(239, 19);
            this.checkBoxIsPhaseInMetterOrCycle.TabIndex = 73;
            this.checkBoxIsPhaseInMetterOrCycle.Text = "输入模糊度单位是距离，否则周";
            this.checkBoxIsPhaseInMetterOrCycle.UseVisualStyleBackColor = true;
            // 
            // checkBox_phaseSmoothRange
            // 
            this.checkBox_phaseSmoothRange.AutoSize = true;
            this.checkBox_phaseSmoothRange.Location = new System.Drawing.Point(319, 8);
            this.checkBox_phaseSmoothRange.Name = "checkBox_phaseSmoothRange";
            this.checkBox_phaseSmoothRange.Size = new System.Drawing.Size(119, 19);
            this.checkBox_phaseSmoothRange.TabIndex = 73;
            this.checkBox_phaseSmoothRange.Text = "载波平滑伪距";
            this.checkBox_phaseSmoothRange.UseVisualStyleBackColor = true;
            // 
            // textBox_angleCut
            // 
            this.textBox_angleCut.Location = new System.Drawing.Point(116, 41);
            this.textBox_angleCut.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_angleCut.Name = "textBox_angleCut";
            this.textBox_angleCut.Size = new System.Drawing.Size(63, 25);
            this.textBox_angleCut.TabIndex = 69;
            this.textBox_angleCut.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 68;
            this.label1.Text = "高度截止角：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_run);
            this.panel1.Controls.Add(this.progressBarComponent1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(753, 44);
            this.panel1.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Location = new System.Drawing.Point(619, 6);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(108, 38);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(753, 279);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.objectTableControlA);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(745, 250);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "第一个平滑MW值";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControlB);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(745, 250);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "第一个模糊度浮点解";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.objectTableControlC);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(745, 250);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "第一个文件模糊度浮点数";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.objectTableControlD);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(745, 250);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "第一个文件模糊度固定值";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // fileOpenContol_obsFiles
            // 
            this.fileOpenContol_obsFiles.AllowDrop = true;
            this.fileOpenContol_obsFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenContol_obsFiles.FilePath = "";
            this.fileOpenContol_obsFiles.FilePathes = new string[0];
            this.fileOpenContol_obsFiles.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenContol_obsFiles.FirstPath = "";
            this.fileOpenContol_obsFiles.IsMultiSelect = true;
            this.fileOpenContol_obsFiles.LabelName = "观测文件：";
            this.fileOpenContol_obsFiles.Location = new System.Drawing.Point(7, 4);
            this.fileOpenContol_obsFiles.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenContol_obsFiles.Name = "fileOpenContol_obsFiles";
            this.fileOpenContol_obsFiles.Size = new System.Drawing.Size(729, 69);
            this.fileOpenContol_obsFiles.TabIndex = 0;
            this.fileOpenContol_obsFiles.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            this.fileOpenContol_obsFiles.Load += new System.EventHandler(this.fileOpenContol_obsFiles_Load);
            // 
            // fileOpenControl_floatAmbiFiles
            // 
            this.fileOpenControl_floatAmbiFiles.AllowDrop = true;
            this.fileOpenControl_floatAmbiFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_floatAmbiFiles.FilePath = "";
            this.fileOpenControl_floatAmbiFiles.FilePathes = new string[0];
            this.fileOpenControl_floatAmbiFiles.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_floatAmbiFiles.FirstPath = "";
            this.fileOpenControl_floatAmbiFiles.IsMultiSelect = true;
            this.fileOpenControl_floatAmbiFiles.LabelName = "模糊度浮点解：";
            this.fileOpenControl_floatAmbiFiles.Location = new System.Drawing.Point(9, 74);
            this.fileOpenControl_floatAmbiFiles.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl_floatAmbiFiles.Name = "fileOpenControl_floatAmbiFiles";
            this.fileOpenControl_floatAmbiFiles.Size = new System.Drawing.Size(729, 69);
            this.fileOpenControl_floatAmbiFiles.TabIndex = 0;
            this.fileOpenControl_floatAmbiFiles.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // namedIntControl_emptyRowCount
            // 
            this.namedIntControl_emptyRowCount.Location = new System.Drawing.Point(5, 8);
            this.namedIntControl_emptyRowCount.Margin = new System.Windows.Forms.Padding(5);
            this.namedIntControl_emptyRowCount.Name = "namedIntControl_emptyRowCount";
            this.namedIntControl_emptyRowCount.Size = new System.Drawing.Size(220, 29);
            this.namedIntControl_emptyRowCount.TabIndex = 70;
            this.namedIntControl_emptyRowCount.Title = "清除滤波前面数据：";
            this.namedIntControl_emptyRowCount.Value = 0;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(21, 3);
            this.progressBarComponent1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(579, 35);
            this.progressBarComponent1.TabIndex = 2;
            // 
            // objectTableControlA
            // 
            this.objectTableControlA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlA.Location = new System.Drawing.Point(3, 3);
            this.objectTableControlA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControlA.Name = "objectTableControlA";
            this.objectTableControlA.Size = new System.Drawing.Size(739, 244);
            this.objectTableControlA.TabIndex = 0;
            this.objectTableControlA.TableObjectStorage = null;
            // 
            // objectTableControlB
            // 
            this.objectTableControlB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlB.Location = new System.Drawing.Point(3, 3);
            this.objectTableControlB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControlB.Name = "objectTableControlB";
            this.objectTableControlB.Size = new System.Drawing.Size(739, 244);
            this.objectTableControlB.TabIndex = 1;
            this.objectTableControlB.TableObjectStorage = null;
            // 
            // objectTableControlC
            // 
            this.objectTableControlC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlC.Location = new System.Drawing.Point(3, 3);
            this.objectTableControlC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControlC.Name = "objectTableControlC";
            this.objectTableControlC.Size = new System.Drawing.Size(739, 244);
            this.objectTableControlC.TabIndex = 1;
            this.objectTableControlC.TableObjectStorage = null;
            // 
            // objectTableControlD
            // 
            this.objectTableControlD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlD.Location = new System.Drawing.Point(3, 3);
            this.objectTableControlD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControlD.Name = "objectTableControlD";
            this.objectTableControlD.Size = new System.Drawing.Size(739, 244);
            this.objectTableControlD.TabIndex = 2;
            this.objectTableControlD.TableObjectStorage = null;
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(548, 3);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(194, 151);
            this.multiGnssSystemSelectControl1.TabIndex = 72;
            // 
            // namedFloatControl1_maxDifferOfIntFloat
            // 
            this.namedFloatControl1_maxDifferOfIntFloat.Location = new System.Drawing.Point(219, 41);
            this.namedFloatControl1_maxDifferOfIntFloat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedFloatControl1_maxDifferOfIntFloat.Name = "namedFloatControl1_maxDifferOfIntFloat";
            this.namedFloatControl1_maxDifferOfIntFloat.Size = new System.Drawing.Size(290, 29);
            this.namedFloatControl1_maxDifferOfIntFloat.TabIndex = 74;
            this.namedFloatControl1_maxDifferOfIntFloat.Title = "固定值与浮点数最大偏差(周)：";
            this.namedFloatControl1_maxDifferOfIntFloat.Value = 0.2D;
            // 
            // IonoFreeDoubleDifferAmbiFixerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 517);
            this.Controls.Add(this.splitContainer1);
            this.Name = "IonoFreeDoubleDifferAmbiFixerForm";
            this.Text = "无电离层双差模糊度固定";
            this.Load += new System.EventHandler(this.IonoFreeDoubleDifferAmbiFixerForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.Controls.FileOpenControl fileOpenContol_obsFiles;
        private System.Windows.Forms.Button button_run;
        private Geo.Winform.ObjectTableControl objectTableControlA;
        private System.Windows.Forms.BindingSource bindingSource_prns;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_floatAmbiFiles;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_emptyRowCount;
        private System.Windows.Forms.TextBox textBox_angleCut;
        private System.Windows.Forms.Label label1;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox_phaseSmoothRange;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Geo.Winform.ObjectTableControl objectTableControlB;
        private System.Windows.Forms.TabPage tabPage5;
        private Geo.Winform.ObjectTableControl objectTableControlC;
        private System.Windows.Forms.CheckBox checkBoxIsPhaseInMetterOrCycle;
        private System.Windows.Forms.TabPage tabPage6;
        private Geo.Winform.ObjectTableControl objectTableControlD;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1_maxDifferOfIntFloat;
    }
}