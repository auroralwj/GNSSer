namespace Gnsser.Winform
{
    partial class MultiFileStreamerForm
    {

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_top = new System.Windows.Forms.SplitContainer();
            this.tabControl_input = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.fileOpenControl_inputPathes = new Geo.Winform.Controls.FileOpenControl();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button_detailSetting = new System.Windows.Forms.Button();
            this.parallelConfigControl1 = new Geo.Winform.Controls.ParallelConfigControl();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.tabPage_InputExtend1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabPage_InputExtend2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1_enableShowInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_showData = new System.Windows.Forms.CheckBox();
            this.label_notice = new System.Windows.Forms.Label();
            this.panel_buttons = new System.Windows.Forms.Panel();
            this.panel_buttonExtends = new System.Windows.Forms.Panel();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_solve = new System.Windows.Forms.Button();
            this.tabControl_output = new System.Windows.Forms.TabControl();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.RichTextBoxControl_processInfo = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage_OutputExtendText1 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_outputExtend1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage_OutputExtendText2 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_outputExtend2 = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage_OutputExtendTable1 = new System.Windows.Forms.TabPage();
            this.objectTableControl_param = new Geo.Winform.ObjectTableControl();
            this.tabPage_OutputExtendTable2 = new System.Windows.Forms.TabPage();
            this.objectTableControl_rms = new Geo.Winform.ObjectTableControl();
            this.tabPage_OutputExtendTable3 = new System.Windows.Forms.TabPage();
            this.objectTableControl_extesion = new Geo.Winform.ObjectTableControl();
            this.tabPage_OutputExtendTable4 = new System.Windows.Forms.TabPage();
            this.objectTableControl_extesion2 = new Geo.Winform.ObjectTableControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_top)).BeginInit();
            this.splitContainer_top.Panel1.SuspendLayout();
            this.splitContainer_top.Panel2.SuspendLayout();
            this.splitContainer_top.SuspendLayout();
            this.tabControl_input.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabPage_InputExtend1.SuspendLayout();
            this.tabPage_InputExtend2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.tabControl_output.SuspendLayout();
            this.tabPage0.SuspendLayout();
            this.tabPage_OutputExtendText1.SuspendLayout();
            this.tabPage_OutputExtendText2.SuspendLayout();
            this.tabPage_OutputExtendTable1.SuspendLayout();
            this.tabPage_OutputExtendTable2.SuspendLayout();
            this.tabPage_OutputExtendTable3.SuspendLayout();
            this.tabPage_OutputExtendTable4.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_main.Name = "splitContainer_main";
            this.splitContainer_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.splitContainer_top);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.tabControl_output);
            this.splitContainer_main.Size = new System.Drawing.Size(792, 474);
            this.splitContainer_main.SplitterDistance = 227;
            this.splitContainer_main.TabIndex = 25;
            // 
            // splitContainer_top
            // 
            this.splitContainer_top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_top.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer_top.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_top.Name = "splitContainer_top";
            this.splitContainer_top.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_top.Panel1
            // 
            this.splitContainer_top.Panel1.Controls.Add(this.tabControl_input);
            // 
            // splitContainer_top.Panel2
            // 
            this.splitContainer_top.Panel2.Controls.Add(this.progressBarComponent1);
            this.splitContainer_top.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer_top.Panel2.Controls.Add(this.panel_buttons);
            this.splitContainer_top.Size = new System.Drawing.Size(792, 227);
            this.splitContainer_top.SplitterDistance = 151;
            this.splitContainer_top.TabIndex = 44;
            // 
            // tabControl_input
            // 
            this.tabControl_input.Controls.Add(this.tabPage1);
            this.tabControl_input.Controls.Add(this.tabPage3);
            this.tabControl_input.Controls.Add(this.tabPage_InputExtend1);
            this.tabControl_input.Controls.Add(this.tabPage_InputExtend2);
            this.tabControl_input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_input.Location = new System.Drawing.Point(0, 0);
            this.tabControl_input.Name = "tabControl_input";
            this.tabControl_input.SelectedIndex = 0;
            this.tabControl_input.Size = new System.Drawing.Size(792, 151);
            this.tabControl_input.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControl_inputPathes);
            this.tabPage1.Controls.Add(this.directorySelectionControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(784, 125);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入输出";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_inputPathes
            // 
            this.fileOpenControl_inputPathes.AllowDrop = true;
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenControl_inputPathes.FilePath = "";
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Filter = "Rinex观测文件(*.??O;*.rnx)|*.??O;*.rnx|RINEX压缩文件(*.??D;*.??O.Z;*.??D.Z;*.crx.gz)|*.??" +
    "D;*.??O.Z;*.??D.Z;*.crx;*.crx.gz|所有文件|*.*";
            this.fileOpenControl_inputPathes.FirstPath = "";
            this.fileOpenControl_inputPathes.IsMultiSelect = true;
            this.fileOpenControl_inputPathes.LabelName = "输入文件：";
            this.fileOpenControl_inputPathes.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl_inputPathes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fileOpenControl_inputPathes.Name = "fileOpenControl_inputPathes";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(778, 97);
            this.fileOpenControl_inputPathes.TabIndex = 3;
            this.fileOpenControl_inputPathes.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = " 输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(3, 100);
            this.directorySelectionControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(778, 22);
            this.directorySelectionControl1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage3.Size = new System.Drawing.Size(784, 125);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "计算设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.parallelConfigControl1);
            this.panel4.Controls.Add(this.multiGnssSystemSelectControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(778, 119);
            this.panel4.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button_detailSetting);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(537, 24);
            this.panel5.TabIndex = 58;
            // 
            // button_detailSetting
            // 
            this.button_detailSetting.Location = new System.Drawing.Point(3, 0);
            this.button_detailSetting.Name = "button_detailSetting";
            this.button_detailSetting.Size = new System.Drawing.Size(75, 23);
            this.button_detailSetting.TabIndex = 11;
            this.button_detailSetting.Text = "详细设置";
            this.button_detailSetting.UseVisualStyleBackColor = true;
            this.button_detailSetting.Click += new System.EventHandler(this.button_detailSetting_Click);
            // 
            // parallelConfigControl1
            // 
            this.parallelConfigControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.parallelConfigControl1.EnableParallel = true;
            this.parallelConfigControl1.Location = new System.Drawing.Point(537, 0);
            this.parallelConfigControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.parallelConfigControl1.Name = "parallelConfigControl1";
            this.parallelConfigControl1.Size = new System.Drawing.Size(91, 119);
            this.parallelConfigControl1.TabIndex = 57;
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(628, 0);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(150, 119);
            this.multiGnssSystemSelectControl1.TabIndex = 56;
            // 
            // tabPage_InputExtend1
            // 
            this.tabPage_InputExtend1.Controls.Add(this.panel3);
            this.tabPage_InputExtend1.Location = new System.Drawing.Point(4, 22);
            this.tabPage_InputExtend1.Name = "tabPage_InputExtend1";
            this.tabPage_InputExtend1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_InputExtend1.Size = new System.Drawing.Size(784, 94);
            this.tabPage_InputExtend1.TabIndex = 2;
            this.tabPage_InputExtend1.Text = "通用设置";
            this.tabPage_InputExtend1.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(778, 88);
            this.panel3.TabIndex = 0;
            // 
            // tabPage_InputExtend2
            // 
            this.tabPage_InputExtend2.Controls.Add(this.panel2);
            this.tabPage_InputExtend2.Location = new System.Drawing.Point(4, 22);
            this.tabPage_InputExtend2.Name = "tabPage_InputExtend2";
            this.tabPage_InputExtend2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_InputExtend2.Size = new System.Drawing.Size(784, 94);
            this.tabPage_InputExtend2.TabIndex = 3;
            this.tabPage_InputExtend2.Text = "输出设置";
            this.tabPage_InputExtend2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(778, 88);
            this.panel2.TabIndex = 0;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(296, 38);
            this.progressBarComponent1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(186, 34);
            this.progressBarComponent1.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 72);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "显示控制";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 52);
            this.panel1.TabIndex = 9;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox1_enableShowInfo);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_showData);
            this.flowLayoutPanel1.Controls.Add(this.label_notice);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(290, 52);
            this.flowLayoutPanel1.TabIndex = 44;
            // 
            // checkBox1_enableShowInfo
            // 
            this.checkBox1_enableShowInfo.AutoSize = true;
            this.checkBox1_enableShowInfo.Checked = true;
            this.checkBox1_enableShowInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1_enableShowInfo.Location = new System.Drawing.Point(3, 3);
            this.checkBox1_enableShowInfo.Name = "checkBox1_enableShowInfo";
            this.checkBox1_enableShowInfo.Size = new System.Drawing.Size(72, 16);
            this.checkBox1_enableShowInfo.TabIndex = 1;
            this.checkBox1_enableShowInfo.Text = "显示信息";
            this.checkBox1_enableShowInfo.UseVisualStyleBackColor = true;
            this.checkBox1_enableShowInfo.CheckedChanged += new System.EventHandler(this.checkBox1_enableShowInfo_CheckedChanged);
            // 
            // checkBox_showData
            // 
            this.checkBox_showData.AutoSize = true;
            this.checkBox_showData.Checked = true;
            this.checkBox_showData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showData.Location = new System.Drawing.Point(81, 3);
            this.checkBox_showData.Name = "checkBox_showData";
            this.checkBox_showData.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showData.TabIndex = 3;
            this.checkBox_showData.Text = "显示数据";
            this.checkBox_showData.UseVisualStyleBackColor = true;
            this.checkBox_showData.CheckedChanged += new System.EventHandler(this.checkBox_showData_CheckedChanged);
            // 
            // label_notice
            // 
            this.label_notice.AutoSize = true;
            this.label_notice.Location = new System.Drawing.Point(159, 0);
            this.label_notice.Name = "label_notice";
            this.label_notice.Size = new System.Drawing.Size(29, 12);
            this.label_notice.TabIndex = 0;
            this.label_notice.Text = "通知";
            // 
            // panel_buttons
            // 
            this.panel_buttons.Controls.Add(this.panel_buttonExtends);
            this.panel_buttons.Controls.Add(this.button_cancel);
            this.panel_buttons.Controls.Add(this.button_solve);
            this.panel_buttons.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_buttons.Location = new System.Drawing.Point(482, 0);
            this.panel_buttons.Name = "panel_buttons";
            this.panel_buttons.Size = new System.Drawing.Size(310, 72);
            this.panel_buttons.TabIndex = 43;
            // 
            // panel_buttonExtends
            // 
            this.panel_buttonExtends.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_buttonExtends.Location = new System.Drawing.Point(0, 43);
            this.panel_buttonExtends.Name = "panel_buttonExtends";
            this.panel_buttonExtends.Size = new System.Drawing.Size(310, 29);
            this.panel_buttonExtends.TabIndex = 42;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(226, 1);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(81, 27);
            this.button_cancel.TabIndex = 7;
            this.button_cancel.Text = "结束";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_solve
            // 
            this.button_solve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solve.BackColor = System.Drawing.Color.Chartreuse;
            this.button_solve.Location = new System.Drawing.Point(127, -1);
            this.button_solve.Name = "button_solve";
            this.button_solve.Size = new System.Drawing.Size(94, 28);
            this.button_solve.TabIndex = 4;
            this.button_solve.Text = "开始";
            this.button_solve.UseVisualStyleBackColor = false;
            this.button_solve.Click += new System.EventHandler(this.button_solve_Click);
            // 
            // tabControl_output
            // 
            this.tabControl_output.Controls.Add(this.tabPage0);
            this.tabControl_output.Controls.Add(this.tabPage_OutputExtendText1);
            this.tabControl_output.Controls.Add(this.tabPage_OutputExtendText2);
            this.tabControl_output.Controls.Add(this.tabPage_OutputExtendTable1);
            this.tabControl_output.Controls.Add(this.tabPage_OutputExtendTable2);
            this.tabControl_output.Controls.Add(this.tabPage_OutputExtendTable3);
            this.tabControl_output.Controls.Add(this.tabPage_OutputExtendTable4);
            this.tabControl_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_output.Location = new System.Drawing.Point(0, 0);
            this.tabControl_output.Name = "tabControl_output";
            this.tabControl_output.SelectedIndex = 0;
            this.tabControl_output.Size = new System.Drawing.Size(792, 243);
            this.tabControl_output.TabIndex = 24;
            // 
            // tabPage0
            // 
            this.tabPage0.Controls.Add(this.RichTextBoxControl_processInfo);
            this.tabPage0.Location = new System.Drawing.Point(4, 22);
            this.tabPage0.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage0.Size = new System.Drawing.Size(784, 217);
            this.tabPage0.TabIndex = 4;
            this.tabPage0.Text = "处理信息";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxControl_processInfo
            // 
            this.RichTextBoxControl_processInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxControl_processInfo.Location = new System.Drawing.Point(2, 2);
            this.RichTextBoxControl_processInfo.MaxAppendLineCount = 10000;
            this.RichTextBoxControl_processInfo.Name = "RichTextBoxControl_processInfo";
            this.RichTextBoxControl_processInfo.Size = new System.Drawing.Size(780, 213);
            this.RichTextBoxControl_processInfo.TabIndex = 0;
            this.RichTextBoxControl_processInfo.Text = "";
            // 
            // tabPage_OutputExtendText1
            // 
            this.tabPage_OutputExtendText1.Controls.Add(this.richTextBoxControl_outputExtend1);
            this.tabPage_OutputExtendText1.Location = new System.Drawing.Point(4, 22);
            this.tabPage_OutputExtendText1.Name = "tabPage_OutputExtendText1";
            this.tabPage_OutputExtendText1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_OutputExtendText1.Size = new System.Drawing.Size(784, 262);
            this.tabPage_OutputExtendText1.TabIndex = 7;
            this.tabPage_OutputExtendText1.Text = "平差矩阵";
            this.tabPage_OutputExtendText1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_outputExtend1
            // 
            this.richTextBoxControl_outputExtend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_outputExtend1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_outputExtend1.MaxAppendLineCount = 10000;
            this.richTextBoxControl_outputExtend1.Name = "richTextBoxControl_outputExtend1";
            this.richTextBoxControl_outputExtend1.Size = new System.Drawing.Size(778, 256);
            this.richTextBoxControl_outputExtend1.TabIndex = 2;
            this.richTextBoxControl_outputExtend1.Text = "";
            // 
            // tabPage_OutputExtendText2
            // 
            this.tabPage_OutputExtendText2.Controls.Add(this.richTextBoxControl_outputExtend2);
            this.tabPage_OutputExtendText2.Location = new System.Drawing.Point(4, 22);
            this.tabPage_OutputExtendText2.Name = "tabPage_OutputExtendText2";
            this.tabPage_OutputExtendText2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_OutputExtendText2.Size = new System.Drawing.Size(784, 262);
            this.tabPage_OutputExtendText2.TabIndex = 8;
            this.tabPage_OutputExtendText2.Text = "汇总";
            this.tabPage_OutputExtendText2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_outputExtend2
            // 
            this.richTextBoxControl_outputExtend2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_outputExtend2.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_outputExtend2.MaxAppendLineCount = 10000;
            this.richTextBoxControl_outputExtend2.Name = "richTextBoxControl_outputExtend2";
            this.richTextBoxControl_outputExtend2.Size = new System.Drawing.Size(778, 256);
            this.richTextBoxControl_outputExtend2.TabIndex = 3;
            this.richTextBoxControl_outputExtend2.Text = "";
            // 
            // tabPage_OutputExtendTable1
            // 
            this.tabPage_OutputExtendTable1.Controls.Add(this.objectTableControl_param);
            this.tabPage_OutputExtendTable1.Location = new System.Drawing.Point(4, 22);
            this.tabPage_OutputExtendTable1.Name = "tabPage_OutputExtendTable1";
            this.tabPage_OutputExtendTable1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_OutputExtendTable1.Size = new System.Drawing.Size(784, 262);
            this.tabPage_OutputExtendTable1.TabIndex = 9;
            this.tabPage_OutputExtendTable1.Text = "参数表格";
            this.tabPage_OutputExtendTable1.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_param
            // 
            this.objectTableControl_param.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_param.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_param.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControl_param.Name = "objectTableControl_param";
            this.objectTableControl_param.Size = new System.Drawing.Size(778, 256);
            this.objectTableControl_param.TabIndex = 0;
            this.objectTableControl_param.TableObjectStorage = null;
            // 
            // tabPage_OutputExtendTable2
            // 
            this.tabPage_OutputExtendTable2.Controls.Add(this.objectTableControl_rms);
            this.tabPage_OutputExtendTable2.Location = new System.Drawing.Point(4, 22);
            this.tabPage_OutputExtendTable2.Name = "tabPage_OutputExtendTable2";
            this.tabPage_OutputExtendTable2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_OutputExtendTable2.Size = new System.Drawing.Size(784, 262);
            this.tabPage_OutputExtendTable2.TabIndex = 10;
            this.tabPage_OutputExtendTable2.Text = "RMS表格";
            this.tabPage_OutputExtendTable2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_rms
            // 
            this.objectTableControl_rms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_rms.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_rms.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControl_rms.Name = "objectTableControl_rms";
            this.objectTableControl_rms.Size = new System.Drawing.Size(778, 256);
            this.objectTableControl_rms.TabIndex = 1;
            this.objectTableControl_rms.TableObjectStorage = null;
            // 
            // tabPage_OutputExtendTable3
            // 
            this.tabPage_OutputExtendTable3.Controls.Add(this.objectTableControl_extesion);
            this.tabPage_OutputExtendTable3.Location = new System.Drawing.Point(4, 22);
            this.tabPage_OutputExtendTable3.Name = "tabPage_OutputExtendTable3";
            this.tabPage_OutputExtendTable3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_OutputExtendTable3.Size = new System.Drawing.Size(784, 262);
            this.tabPage_OutputExtendTable3.TabIndex = 11;
            this.tabPage_OutputExtendTable3.Text = "扩展";
            this.tabPage_OutputExtendTable3.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_extesion
            // 
            this.objectTableControl_extesion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_extesion.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_extesion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControl_extesion.Name = "objectTableControl_extesion";
            this.objectTableControl_extesion.Size = new System.Drawing.Size(778, 256);
            this.objectTableControl_extesion.TabIndex = 2;
            this.objectTableControl_extesion.TableObjectStorage = null;
            // 
            // tabPage_OutputExtendTable4
            // 
            this.tabPage_OutputExtendTable4.Controls.Add(this.objectTableControl_extesion2);
            this.tabPage_OutputExtendTable4.Location = new System.Drawing.Point(4, 22);
            this.tabPage_OutputExtendTable4.Name = "tabPage_OutputExtendTable4";
            this.tabPage_OutputExtendTable4.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_OutputExtendTable4.Size = new System.Drawing.Size(784, 262);
            this.tabPage_OutputExtendTable4.TabIndex = 12;
            this.tabPage_OutputExtendTable4.Text = "扩展2";
            this.tabPage_OutputExtendTable4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_extesion2
            // 
            this.objectTableControl_extesion2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_extesion2.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_extesion2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControl_extesion2.Name = "objectTableControl_extesion2";
            this.objectTableControl_extesion2.Size = new System.Drawing.Size(778, 256);
            this.objectTableControl_extesion2.TabIndex = 3;
            this.objectTableControl_extesion2.TableObjectStorage = null;
            // 
            // MultiFileStreamerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 474);
            this.Controls.Add(this.splitContainer_main);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MultiFileStreamerForm";
            this.Text = "通用多文件数据流执行器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogListeningForm_FormClosing);
            this.Load += new System.EventHandler(this.PointPositionForm_Load);
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.splitContainer_top.Panel1.ResumeLayout(false);
            this.splitContainer_top.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_top)).EndInit();
            this.splitContainer_top.ResumeLayout(false);
            this.tabControl_input.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabPage_InputExtend1.ResumeLayout(false);
            this.tabPage_InputExtend2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel_buttons.ResumeLayout(false);
            this.tabControl_output.ResumeLayout(false);
            this.tabPage0.ResumeLayout(false);
            this.tabPage_OutputExtendText1.ResumeLayout(false);
            this.tabPage_OutputExtendText2.ResumeLayout(false);
            this.tabPage_OutputExtendTable1.ResumeLayout(false);
            this.tabPage_OutputExtendTable2.ResumeLayout(false);
            this.tabPage_OutputExtendTable3.ResumeLayout(false);
            this.tabPage_OutputExtendTable4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.ComponentModel.BackgroundWorker backgroundWorker1;
        protected System.Windows.Forms.TabPage tabPage0;
        protected System.Windows.Forms.Label label_notice;
        protected System.Windows.Forms.CheckBox checkBox1_enableShowInfo;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button button_detailSetting;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.Windows.Forms.Button button_solve;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TabControl tabControl_input;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl_output;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage_OutputExtendText1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_outputExtend1;
        private Geo.Winform.Controls.RichTextBoxControl RichTextBoxControl_processInfo;
        protected System.Windows.Forms.Panel panel3;
        protected System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.Panel panel4;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage_InputExtend1;
        private System.Windows.Forms.TabPage tabPage_InputExtend2;
        protected System.Windows.Forms.Panel panel_buttonExtends;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.TabPage tabPage_OutputExtendText2;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_outputExtend2;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer_top;
        private System.Windows.Forms.TabPage tabPage_OutputExtendTable1;
        private System.Windows.Forms.CheckBox checkBox_showData;
        protected Geo.Winform.Controls.FileOpenControl fileOpenControl_inputPathes;
        protected System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage_OutputExtendTable2;
        private Geo.Winform.ObjectTableControl objectTableControl_param;
        private Geo.Winform.ObjectTableControl objectTableControl_rms;
        protected System.Windows.Forms.Panel panel_buttons;
        private Geo.Winform.Controls.ParallelConfigControl parallelConfigControl1;
        private System.Windows.Forms.TabPage tabPage_OutputExtendTable3;
        private Geo.Winform.ObjectTableControl objectTableControl_extesion;
        private System.Windows.Forms.TabPage tabPage_OutputExtendTable4;
        private Geo.Winform.ObjectTableControl objectTableControl_extesion2;
        protected System.Windows.Forms.Panel panel5;
    }
}

