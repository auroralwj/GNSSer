namespace Gnsser.Winform
{
    partial class BaseLinePositionForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.textBox_estParam = new System.Windows.Forms.TextBox();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.button_cacuAll = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_kalman = new System.Windows.Forms.Button();
            this.button_drawDifferLine = new System.Windows.Forms.Button();
            this.button_drawRmslines = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBox_appriori = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.textBox_differXyz = new System.Windows.Forms.TextBox();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.textBox_resultInfo = new System.Windows.Forms.TextBox();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.textBox_obsMinusApriori = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.label_progress = new System.Windows.Forms.Label();
            this.openFileDialog_ant = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_ocean = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_satState = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_excludeSat = new System.Windows.Forms.OpenFileDialog();
            this.label_notice = new System.Windows.Forms.Label();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.positonResultRenderControl11 = new Geo.Winform.Controls.RmsedVectorRenderControl();
            this.checkBox_outputReslultFile = new System.Windows.Forms.CheckBox();
            this.button_toSinex = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_maxError = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_maxstdTimes = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_maxStd = new System.Windows.Forms.TextBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.radio_noOut = new System.Windows.Forms.RadioButton();
            this.radio_onTImeOutput = new System.Windows.Forms.RadioButton();
            this.radio_out100 = new System.Windows.Forms.RadioButton();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_startEpoch = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_caculateCount = new System.Windows.Forms.TextBox();
            this.checkBox_ignoreCourceError = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_differSatCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_differEpochCount = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_gps5km = new System.Windows.Forms.RadioButton();
            this.radioButtonGPSClose = new System.Windows.Forms.RadioButton();
            this.radioButton_gps2_0 = new System.Windows.Forms.RadioButton();
            this.radioButton_gps3_0 = new System.Windows.Forms.RadioButton();
            this.radioButton_beidou2_0 = new System.Windows.Forms.RadioButton();
            this.radioButton_beidou3 = new System.Windows.Forms.RadioButton();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_openProjDir = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_taskName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rmsedXyzControl_ref = new Geo.Winform.Controls.RmsedXyzControl();
            this.rmsedXyzControl_rov = new Geo.Winform.Controls.RmsedXyzControl();
            this.label18 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_setObsPath_rov = new System.Windows.Forms.Button();
            this.button_getObsPath_ref = new System.Windows.Forms.Button();
            this.textBox_obsFile_rov = new System.Windows.Forms.TextBox();
            this.textBox_obsPath_ref = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radioButton_doubleDiffer = new System.Windows.Forms.RadioButton();
            this.radioButton_norelevant = new System.Windows.Forms.RadioButton();
            this.radioButton_phaseSingleDiffer = new System.Windows.Forms.RadioButton();
            this.radioButton_pppResidualDiffer = new System.Windows.Forms.RadioButton();
            this.checkBox_enableClockFile = new System.Windows.Forms.CheckBox();
            this.button_getClockPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ClockPath = new System.Windows.Forms.TextBox();
            this.button_getNavPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_navPath = new System.Windows.Forms.TextBox();
            this.tabControl_top = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_datasourceSeting = new System.Windows.Forms.Button();
            this.checkBox_setEphemerisFile = new System.Windows.Forms.CheckBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.satDataTypeSelectControl2 = new Gnsser.Winform.Controls.SatApproxDataTypeControl();
            this.satDataTypeSelectControl1 = new Gnsser.Winform.Controls.SatObsDataTypeControl();
            this.checkBox_outputAdjust = new System.Windows.Forms.CheckBox();
            this.checkBox_preAnalysis = new System.Windows.Forms.CheckBox();
            this.button_analysisData = new System.Windows.Forms.Button();
            this.checkBox_ambiguityFixed = new System.Windows.Forms.CheckBox();
            this.checkBox_showProcessInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_autoMatchingFile = new System.Windows.Forms.CheckBox();
            this.checkBox_enableNet = new System.Windows.Forms.CheckBox();
            this.checkBox_debugModel = new System.Windows.Forms.CheckBox();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabControl_top.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件|*.*o|所有文件|*.*";
            // 
            // textBox_estParam
            // 
            this.textBox_estParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_estParam.Location = new System.Drawing.Point(3, 3);
            this.textBox_estParam.Multiline = true;
            this.textBox_estParam.Name = "textBox_estParam";
            this.textBox_estParam.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_estParam.Size = new System.Drawing.Size(1254, 159);
            this.textBox_estParam.TabIndex = 2;
            // 
            // openFileDialog_nav
            // 
            this.openFileDialog_nav.FileName = "openFileDialog2";
            this.openFileDialog_nav.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            // 
            // button_cacuAll
            // 
            this.button_cacuAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cacuAll.Location = new System.Drawing.Point(1102, 162);
            this.button_cacuAll.Name = "button_cacuAll";
            this.button_cacuAll.Size = new System.Drawing.Size(76, 33);
            this.button_cacuAll.TabIndex = 8;
            this.button_cacuAll.Text = "独立计算";
            this.button_cacuAll.UseVisualStyleBackColor = true;
            this.button_cacuAll.Click += new System.EventHandler(this.button_cacuIndependent_Click);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_showOnMap.Location = new System.Drawing.Point(936, 207);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(75, 25);
            this.button_showOnMap.TabIndex = 10;
            this.button_showOnMap.Text = "地图上显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(1181, 162);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(76, 32);
            this.button_cancel.TabIndex = 19;
            this.button_cancel.Text = "取消计算";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_kalman
            // 
            this.button_kalman.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_kalman.Location = new System.Drawing.Point(1023, 162);
            this.button_kalman.Name = "button_kalman";
            this.button_kalman.Size = new System.Drawing.Size(76, 32);
            this.button_kalman.TabIndex = 19;
            this.button_kalman.Text = "Kalman滤波";
            this.button_kalman.UseVisualStyleBackColor = true;
            this.button_kalman.Click += new System.EventHandler(this.button_kalman_Click);
            // 
            // button_drawDifferLine
            // 
            this.button_drawDifferLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawDifferLine.Location = new System.Drawing.Point(1111, 207);
            this.button_drawDifferLine.Name = "button_drawDifferLine";
            this.button_drawDifferLine.Size = new System.Drawing.Size(64, 23);
            this.button_drawDifferLine.TabIndex = 20;
            this.button_drawDifferLine.Text = "绘偏差图";
            this.button_drawDifferLine.UseVisualStyleBackColor = true;
            this.button_drawDifferLine.Click += new System.EventHandler(this.button_drawDifferLine_Click);
            // 
            // button_drawRmslines
            // 
            this.button_drawRmslines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawRmslines.Location = new System.Drawing.Point(1017, 207);
            this.button_drawRmslines.Name = "button_drawRmslines";
            this.button_drawRmslines.Size = new System.Drawing.Size(82, 23);
            this.button_drawRmslines.TabIndex = 20;
            this.button_drawRmslines.Text = "绘均方根图";
            this.button_drawRmslines.UseVisualStyleBackColor = true;
            this.button_drawRmslines.Click += new System.EventHandler(this.button_drawRmslines_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "SpResult";
            this.saveFileDialog1.Filter = "SINEX|*.SNX";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Controls.Add(this.tabPage11);
            this.tabControl2.Location = new System.Drawing.Point(2, 278);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1268, 191);
            this.tabControl2.TabIndex = 24;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox_estParam);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1260, 165);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "参数(偏差)估值";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBox_appriori);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1260, 165);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "先验信息";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox_appriori
            // 
            this.textBox_appriori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_appriori.Location = new System.Drawing.Point(3, 3);
            this.textBox_appriori.Multiline = true;
            this.textBox_appriori.Name = "textBox_appriori";
            this.textBox_appriori.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_appriori.Size = new System.Drawing.Size(1254, 159);
            this.textBox_appriori.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_result);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1260, 165);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "解算结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox_result
            // 
            this.textBox_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_result.Location = new System.Drawing.Point(3, 3);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(1254, 159);
            this.textBox_result.TabIndex = 2;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.textBox_differXyz);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage7.Size = new System.Drawing.Size(1260, 165);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "与参考坐标差";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // textBox_differXyz
            // 
            this.textBox_differXyz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_differXyz.Location = new System.Drawing.Point(2, 2);
            this.textBox_differXyz.Multiline = true;
            this.textBox_differXyz.Name = "textBox_differXyz";
            this.textBox_differXyz.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_differXyz.Size = new System.Drawing.Size(1256, 161);
            this.textBox_differXyz.TabIndex = 3;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.textBox_resultInfo);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage9.Size = new System.Drawing.Size(1260, 165);
            this.tabPage9.TabIndex = 4;
            this.tabPage9.Text = "处理信息";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // textBox_resultInfo
            // 
            this.textBox_resultInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_resultInfo.Location = new System.Drawing.Point(2, 2);
            this.textBox_resultInfo.Multiline = true;
            this.textBox_resultInfo.Name = "textBox_resultInfo";
            this.textBox_resultInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_resultInfo.Size = new System.Drawing.Size(1256, 161);
            this.textBox_resultInfo.TabIndex = 4;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.textBox_obsMinusApriori);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage11.Size = new System.Drawing.Size(1260, 165);
            this.tabPage11.TabIndex = 5;
            this.tabPage11.Text = "最后平差结果";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // textBox_obsMinusApriori
            // 
            this.textBox_obsMinusApriori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_obsMinusApriori.Location = new System.Drawing.Point(2, 2);
            this.textBox_obsMinusApriori.Multiline = true;
            this.textBox_obsMinusApriori.Name = "textBox_obsMinusApriori";
            this.textBox_obsMinusApriori.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_obsMinusApriori.Size = new System.Drawing.Size(1256, 161);
            this.textBox_obsMinusApriori.TabIndex = 5;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "openFileDialog2";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk|所有文件|*.*";
            // 
            // label_progress
            // 
            this.label_progress.AutoSize = true;
            this.label_progress.Location = new System.Drawing.Point(6, 188);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(53, 12);
            this.label_progress.TabIndex = 27;
            this.label_progress.Text = "进度显示";
            // 
            // openFileDialog_ant
            // 
            this.openFileDialog_ant.Filter = "天线文件|*.atx|所有文件|*.*";
            // 
            // openFileDialog_ocean
            // 
            this.openFileDialog_ocean.Filter = "海洋潮汐文件|*.dat|所有文件|*.*";
            // 
            // openFileDialog_satState
            // 
            this.openFileDialog_satState.Filter = "所有文件|*.*";
            // 
            // openFileDialog_excludeSat
            // 
            this.openFileDialog_excludeSat.Filter = "排除卫星文件|*.dat|所有文件|*.*";
            // 
            // label_notice
            // 
            this.label_notice.AutoSize = true;
            this.label_notice.Location = new System.Drawing.Point(9, 166);
            this.label_notice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_notice.Name = "label_notice";
            this.label_notice.Size = new System.Drawing.Size(29, 12);
            this.label_notice.TabIndex = 31;
            this.label_notice.Text = "提示";
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.positonResultRenderControl11);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage8.Size = new System.Drawing.Size(1254, 129);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "结果处理";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // positonResultRenderControl11
            // 
            this.positonResultRenderControl11.Location = new System.Drawing.Point(6, 5);
            this.positonResultRenderControl11.Margin = new System.Windows.Forms.Padding(2);
            this.positonResultRenderControl11.Name = "positonResultRenderControl11";
            this.positonResultRenderControl11.Size = new System.Drawing.Size(410, 71);
            this.positonResultRenderControl11.TabIndex = 42;
            // 
            // checkBox_outputReslultFile
            // 
            this.checkBox_outputReslultFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_outputReslultFile.AutoSize = true;
            this.checkBox_outputReslultFile.Checked = true;
            this.checkBox_outputReslultFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_outputReslultFile.Location = new System.Drawing.Point(856, 166);
            this.checkBox_outputReslultFile.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_outputReslultFile.Name = "checkBox_outputReslultFile";
            this.checkBox_outputReslultFile.Size = new System.Drawing.Size(72, 16);
            this.checkBox_outputReslultFile.TabIndex = 40;
            this.checkBox_outputReslultFile.Text = "输出文件";
            this.checkBox_outputReslultFile.UseVisualStyleBackColor = true;
            this.checkBox_outputReslultFile.CheckedChanged += new System.EventHandler(this.checkBox_outputReslultFile_CheckedChanged);
            // 
            // button_toSinex
            // 
            this.button_toSinex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_toSinex.Location = new System.Drawing.Point(1181, 207);
            this.button_toSinex.Name = "button_toSinex";
            this.button_toSinex.Size = new System.Drawing.Size(76, 23);
            this.button_toSinex.TabIndex = 17;
            this.button_toSinex.Text = "导出Sinex";
            this.button_toSinex.UseVisualStyleBackColor = true;
            this.button_toSinex.Click += new System.EventHandler(this.button_toSinex_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox9);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(1254, 129);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "计算控制";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.groupBox7);
            this.groupBox9.Controls.Add(this.groupBox10);
            this.groupBox9.Controls.Add(this.groupBox11);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox9.Location = new System.Drawing.Point(2, 2);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox9.Size = new System.Drawing.Size(1250, 125);
            this.groupBox9.TabIndex = 28;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "计算控制面板";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.textBox_maxError);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.textBox_maxstdTimes);
            this.groupBox7.Controls.Add(this.label13);
            this.groupBox7.Controls.Add(this.textBox_maxStd);
            this.groupBox7.Location = new System.Drawing.Point(285, 20);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox7.Size = new System.Drawing.Size(252, 90);
            this.groupBox7.TabIndex = 39;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "粗差处理";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 33;
            this.label12.Text = "相距超过";
            // 
            // textBox_maxError
            // 
            this.textBox_maxError.Location = new System.Drawing.Point(62, 16);
            this.textBox_maxError.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_maxError.Name = "textBox_maxError";
            this.textBox_maxError.Size = new System.Drawing.Size(84, 21);
            this.textBox_maxError.TabIndex = 35;
            this.textBox_maxError.Text = "1000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 74);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 12);
            this.label9.TabIndex = 37;
            this.label9.Text = "允许最大均方差倍数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 37;
            this.label4.Text = "允许最大均方差：";
            // 
            // textBox_maxstdTimes
            // 
            this.textBox_maxstdTimes.Location = new System.Drawing.Point(127, 68);
            this.textBox_maxstdTimes.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_maxstdTimes.Name = "textBox_maxstdTimes";
            this.textBox_maxstdTimes.Size = new System.Drawing.Size(60, 21);
            this.textBox_maxstdTimes.TabIndex = 34;
            this.textBox_maxstdTimes.Text = "10";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(151, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 12);
            this.label13.TabIndex = 36;
            this.label13.Text = "米，作为粗差剔除。";
            // 
            // textBox_maxStd
            // 
            this.textBox_maxStd.Location = new System.Drawing.Point(110, 42);
            this.textBox_maxStd.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_maxStd.Name = "textBox_maxStd";
            this.textBox_maxStd.Size = new System.Drawing.Size(60, 21);
            this.textBox_maxStd.TabIndex = 34;
            this.textBox_maxStd.Text = "5000";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.radio_noOut);
            this.groupBox10.Controls.Add(this.radio_onTImeOutput);
            this.groupBox10.Controls.Add(this.radio_out100);
            this.groupBox10.Location = new System.Drawing.Point(152, 20);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox10.Size = new System.Drawing.Size(128, 70);
            this.groupBox10.TabIndex = 29;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "面板输出";
            // 
            // radio_noOut
            // 
            this.radio_noOut.AutoSize = true;
            this.radio_noOut.Checked = true;
            this.radio_noOut.Location = new System.Drawing.Point(16, 14);
            this.radio_noOut.Name = "radio_noOut";
            this.radio_noOut.Size = new System.Drawing.Size(83, 16);
            this.radio_noOut.TabIndex = 22;
            this.radio_noOut.TabStop = true;
            this.radio_noOut.Text = "不实时输出";
            this.radio_noOut.UseVisualStyleBackColor = true;
            // 
            // radio_onTImeOutput
            // 
            this.radio_onTImeOutput.AutoSize = true;
            this.radio_onTImeOutput.Location = new System.Drawing.Point(16, 32);
            this.radio_onTImeOutput.Name = "radio_onTImeOutput";
            this.radio_onTImeOutput.Size = new System.Drawing.Size(95, 16);
            this.radio_onTImeOutput.TabIndex = 22;
            this.radio_onTImeOutput.Text = "实时输出全部";
            this.radio_onTImeOutput.UseVisualStyleBackColor = true;
            // 
            // radio_out100
            // 
            this.radio_out100.AutoSize = true;
            this.radio_out100.Location = new System.Drawing.Point(16, 48);
            this.radio_out100.Name = "radio_out100";
            this.radio_out100.Size = new System.Drawing.Size(101, 16);
            this.radio_out100.TabIndex = 22;
            this.radio_out100.Text = "实时输出前100";
            this.radio_out100.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label11);
            this.groupBox11.Controls.Add(this.textBox_startEpoch);
            this.groupBox11.Controls.Add(this.label8);
            this.groupBox11.Controls.Add(this.textBox_caculateCount);
            this.groupBox11.Location = new System.Drawing.Point(9, 20);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox11.Size = new System.Drawing.Size(136, 70);
            this.groupBox11.TabIndex = 30;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "计算数量";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "起始历元：";
            // 
            // textBox_startEpoch
            // 
            this.textBox_startEpoch.Location = new System.Drawing.Point(77, 15);
            this.textBox_startEpoch.Name = "textBox_startEpoch";
            this.textBox_startEpoch.Size = new System.Drawing.Size(51, 21);
            this.textBox_startEpoch.TabIndex = 18;
            this.textBox_startEpoch.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "计算历元数：";
            // 
            // textBox_caculateCount
            // 
            this.textBox_caculateCount.Location = new System.Drawing.Point(77, 39);
            this.textBox_caculateCount.Name = "textBox_caculateCount";
            this.textBox_caculateCount.Size = new System.Drawing.Size(51, 21);
            this.textBox_caculateCount.TabIndex = 18;
            this.textBox_caculateCount.Text = "10000";
            // 
            // checkBox_ignoreCourceError
            // 
            this.checkBox_ignoreCourceError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_ignoreCourceError.AutoSize = true;
            this.checkBox_ignoreCourceError.Checked = true;
            this.checkBox_ignoreCourceError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ignoreCourceError.Location = new System.Drawing.Point(856, 188);
            this.checkBox_ignoreCourceError.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_ignoreCourceError.Name = "checkBox_ignoreCourceError";
            this.checkBox_ignoreCourceError.Size = new System.Drawing.Size(72, 16);
            this.checkBox_ignoreCourceError.TabIndex = 38;
            this.checkBox_ignoreCourceError.Text = "过滤粗差";
            this.checkBox_ignoreCourceError.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.label7);
            this.groupBox12.Controls.Add(this.textBox_differSatCount);
            this.groupBox12.Controls.Add(this.label6);
            this.groupBox12.Controls.Add(this.textBox_differEpochCount);
            this.groupBox12.Location = new System.Drawing.Point(62, 202);
            this.groupBox12.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox12.Size = new System.Drawing.Size(214, 37);
            this.groupBox12.TabIndex = 40;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "差分设置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(103, 16);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "卫星数量:";
            // 
            // textBox_differSatCount
            // 
            this.textBox_differSatCount.Location = new System.Drawing.Point(165, 12);
            this.textBox_differSatCount.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_differSatCount.Name = "textBox_differSatCount";
            this.textBox_differSatCount.Size = new System.Drawing.Size(44, 21);
            this.textBox_differSatCount.TabIndex = 0;
            this.textBox_differSatCount.Text = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "历元数量:";
            // 
            // textBox_differEpochCount
            // 
            this.textBox_differEpochCount.Location = new System.Drawing.Point(68, 12);
            this.textBox_differEpochCount.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_differEpochCount.Name = "textBox_differEpochCount";
            this.textBox_differEpochCount.Size = new System.Drawing.Size(32, 21);
            this.textBox_differEpochCount.TabIndex = 0;
            this.textBox_differEpochCount.Text = "4";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox3);
            this.tabPage5.Controls.Add(this.multiGnssSystemSelectControl1);
            this.tabPage5.Controls.Add(this.groupBox4);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1254, 129);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "基础设置";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.radioButton_gps5km);
            this.groupBox3.Controls.Add(this.radioButtonGPSClose);
            this.groupBox3.Controls.Add(this.radioButton_gps2_0);
            this.groupBox3.Controls.Add(this.radioButton_gps3_0);
            this.groupBox3.Controls.Add(this.radioButton_beidou2_0);
            this.groupBox3.Controls.Add(this.radioButton_beidou3);
            this.groupBox3.Location = new System.Drawing.Point(1000, 6);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(245, 44);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "地址预设";
            // 
            // radioButton_gps5km
            // 
            this.radioButton_gps5km.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton_gps5km.AutoSize = true;
            this.radioButton_gps5km.Location = new System.Drawing.Point(16, 27);
            this.radioButton_gps5km.Name = "radioButton_gps5km";
            this.radioButton_gps5km.Size = new System.Drawing.Size(83, 16);
            this.radioButton_gps5km.TabIndex = 27;
            this.radioButton_gps5km.Text = "GPS2.0_5Km";
            this.radioButton_gps5km.UseVisualStyleBackColor = true;
            this.radioButton_gps5km.CheckedChanged += new System.EventHandler(this.radioButton_setRinexPathes_CheckedChanged);
            // 
            // radioButtonGPSClose
            // 
            this.radioButtonGPSClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonGPSClose.AutoSize = true;
            this.radioButtonGPSClose.Location = new System.Drawing.Point(16, 12);
            this.radioButtonGPSClose.Name = "radioButtonGPSClose";
            this.radioButtonGPSClose.Size = new System.Drawing.Size(83, 16);
            this.radioButtonGPSClose.TabIndex = 27;
            this.radioButtonGPSClose.Text = "GPS2.0_10M";
            this.radioButtonGPSClose.UseVisualStyleBackColor = true;
            this.radioButtonGPSClose.CheckedChanged += new System.EventHandler(this.radioButton_setRinexPathes_CheckedChanged);
            // 
            // radioButton_gps2_0
            // 
            this.radioButton_gps2_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton_gps2_0.AutoSize = true;
            this.radioButton_gps2_0.Location = new System.Drawing.Point(109, 12);
            this.radioButton_gps2_0.Name = "radioButton_gps2_0";
            this.radioButton_gps2_0.Size = new System.Drawing.Size(59, 16);
            this.radioButton_gps2_0.TabIndex = 27;
            this.radioButton_gps2_0.Text = "GPS2.0";
            this.radioButton_gps2_0.UseVisualStyleBackColor = true;
            this.radioButton_gps2_0.CheckedChanged += new System.EventHandler(this.radioButton_setRinexPathes_CheckedChanged);
            // 
            // radioButton_gps3_0
            // 
            this.radioButton_gps3_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton_gps3_0.AutoSize = true;
            this.radioButton_gps3_0.Location = new System.Drawing.Point(109, 27);
            this.radioButton_gps3_0.Name = "radioButton_gps3_0";
            this.radioButton_gps3_0.Size = new System.Drawing.Size(59, 16);
            this.radioButton_gps3_0.TabIndex = 27;
            this.radioButton_gps3_0.Text = "GPS3.0";
            this.radioButton_gps3_0.UseVisualStyleBackColor = true;
            this.radioButton_gps3_0.CheckedChanged += new System.EventHandler(this.radioButton_setRinexPathes_CheckedChanged);
            // 
            // radioButton_beidou2_0
            // 
            this.radioButton_beidou2_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton_beidou2_0.AutoSize = true;
            this.radioButton_beidou2_0.Location = new System.Drawing.Point(171, 12);
            this.radioButton_beidou2_0.Name = "radioButton_beidou2_0";
            this.radioButton_beidou2_0.Size = new System.Drawing.Size(65, 16);
            this.radioButton_beidou2_0.TabIndex = 27;
            this.radioButton_beidou2_0.Text = "北斗2.0";
            this.radioButton_beidou2_0.UseVisualStyleBackColor = true;
            this.radioButton_beidou2_0.CheckedChanged += new System.EventHandler(this.radioButton_setRinexPathes_CheckedChanged);
            // 
            // radioButton_beidou3
            // 
            this.radioButton_beidou3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton_beidou3.AutoSize = true;
            this.radioButton_beidou3.Checked = true;
            this.radioButton_beidou3.Location = new System.Drawing.Point(171, 27);
            this.radioButton_beidou3.Name = "radioButton_beidou3";
            this.radioButton_beidou3.Size = new System.Drawing.Size(65, 16);
            this.radioButton_beidou3.TabIndex = 28;
            this.radioButton_beidou3.TabStop = true;
            this.radioButton_beidou3.Text = "北斗3.0";
            this.radioButton_beidou3.UseVisualStyleBackColor = true;
            this.radioButton_beidou3.CheckedChanged += new System.EventHandler(this.radioButton_setRinexPathes_CheckedChanged);
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(689, 6);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(292, 43);
            this.multiGnssSystemSelectControl1.TabIndex = 44;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button_openProjDir);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.textBox_taskName);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(672, 44);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "工程设置";
            // 
            // button_openProjDir
            // 
            this.button_openProjDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_openProjDir.Location = new System.Drawing.Point(603, 14);
            this.button_openProjDir.Margin = new System.Windows.Forms.Padding(2);
            this.button_openProjDir.Name = "button_openProjDir";
            this.button_openProjDir.Size = new System.Drawing.Size(62, 22);
            this.button_openProjDir.TabIndex = 41;
            this.button_openProjDir.Text = "打开目录";
            this.button_openProjDir.UseVisualStyleBackColor = true;
            this.button_openProjDir.Click += new System.EventHandler(this.button_openProjDir_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 38;
            this.label5.Text = "工程名称：";
            // 
            // textBox_taskName
            // 
            this.textBox_taskName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_taskName.Location = new System.Drawing.Point(74, 16);
            this.textBox_taskName.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_taskName.Name = "textBox_taskName";
            this.textBox_taskName.Size = new System.Drawing.Size(526, 21);
            this.textBox_taskName.TabIndex = 39;
            this.textBox_taskName.Text = "工程名称";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rmsedXyzControl_ref);
            this.groupBox1.Controls.Add(this.rmsedXyzControl_rov);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_setObsPath_rov);
            this.groupBox1.Controls.Add(this.button_getObsPath_ref);
            this.groupBox1.Controls.Add(this.textBox_obsFile_rov);
            this.groupBox1.Controls.Add(this.textBox_obsPath_ref);
            this.groupBox1.Location = new System.Drawing.Point(6, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1239, 78);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "观测数据";
            // 
            // rmsedXyzControl_ref
            // 
            this.rmsedXyzControl_ref.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rmsedXyzControl_ref.IsEnabled = false;
            this.rmsedXyzControl_ref.Location = new System.Drawing.Point(611, 9);
            this.rmsedXyzControl_ref.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rmsedXyzControl_ref.Name = "rmsedXyzControl_ref";
            this.rmsedXyzControl_ref.Size = new System.Drawing.Size(308, 66);
            this.rmsedXyzControl_ref.TabIndex = 32;
            // 
            // rmsedXyzControl_rov
            // 
            this.rmsedXyzControl_rov.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rmsedXyzControl_rov.IsEnabled = false;
            this.rmsedXyzControl_rov.Location = new System.Drawing.Point(925, 9);
            this.rmsedXyzControl_rov.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rmsedXyzControl_rov.Name = "rmsedXyzControl_rov";
            this.rmsedXyzControl_rov.Size = new System.Drawing.Size(315, 66);
            this.rmsedXyzControl_rov.TabIndex = 32;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 48);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "流动站数据：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "参考站数据：";
            // 
            // button_setObsPath_rov
            // 
            this.button_setObsPath_rov.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setObsPath_rov.Location = new System.Drawing.Point(554, 42);
            this.button_setObsPath_rov.Name = "button_setObsPath_rov";
            this.button_setObsPath_rov.Size = new System.Drawing.Size(52, 23);
            this.button_setObsPath_rov.TabIndex = 0;
            this.button_setObsPath_rov.Text = "...";
            this.button_setObsPath_rov.UseVisualStyleBackColor = true;
            this.button_setObsPath_rov.Click += new System.EventHandler(this.button_setObsPath_rov_Click);
            // 
            // button_getObsPath_ref
            // 
            this.button_getObsPath_ref.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getObsPath_ref.Location = new System.Drawing.Point(554, 16);
            this.button_getObsPath_ref.Name = "button_getObsPath_ref";
            this.button_getObsPath_ref.Size = new System.Drawing.Size(52, 23);
            this.button_getObsPath_ref.TabIndex = 0;
            this.button_getObsPath_ref.Text = "...";
            this.button_getObsPath_ref.UseVisualStyleBackColor = true;
            this.button_getObsPath_ref.Click += new System.EventHandler(this.button_getObsPath_Click);
            // 
            // textBox_obsFile_rov
            // 
            this.textBox_obsFile_rov.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_obsFile_rov.Location = new System.Drawing.Point(95, 44);
            this.textBox_obsFile_rov.Name = "textBox_obsFile_rov";
            this.textBox_obsFile_rov.Size = new System.Drawing.Size(454, 21);
            this.textBox_obsFile_rov.TabIndex = 2;
            // 
            // textBox_obsPath_ref
            // 
            this.textBox_obsPath_ref.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_obsPath_ref.Location = new System.Drawing.Point(95, 18);
            this.textBox_obsPath_ref.Name = "textBox_obsPath_ref";
            this.textBox_obsPath_ref.Size = new System.Drawing.Size(454, 21);
            this.textBox_obsPath_ref.TabIndex = 2;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.radioButton_doubleDiffer);
            this.groupBox8.Controls.Add(this.radioButton_norelevant);
            this.groupBox8.Controls.Add(this.radioButton_phaseSingleDiffer);
            this.groupBox8.Controls.Add(this.radioButton_pppResidualDiffer);
            this.groupBox8.Location = new System.Drawing.Point(58, 160);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox8.Size = new System.Drawing.Size(554, 35);
            this.groupBox8.TabIndex = 43;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "差分类型";
            // 
            // radioButton_doubleDiffer
            // 
            this.radioButton_doubleDiffer.AutoSize = true;
            this.radioButton_doubleDiffer.Location = new System.Drawing.Point(358, 15);
            this.radioButton_doubleDiffer.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_doubleDiffer.Name = "radioButton_doubleDiffer";
            this.radioButton_doubleDiffer.Size = new System.Drawing.Size(95, 16);
            this.radioButton_doubleDiffer.TabIndex = 0;
            this.radioButton_doubleDiffer.Text = "载波相位双差";
            this.radioButton_doubleDiffer.UseVisualStyleBackColor = true;
            // 
            // radioButton_norelevant
            // 
            this.radioButton_norelevant.AutoSize = true;
            this.radioButton_norelevant.Location = new System.Drawing.Point(247, 15);
            this.radioButton_norelevant.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_norelevant.Name = "radioButton_norelevant";
            this.radioButton_norelevant.Size = new System.Drawing.Size(107, 16);
            this.radioButton_norelevant.TabIndex = 0;
            this.radioButton_norelevant.Text = "去相关相位单差";
            this.radioButton_norelevant.UseVisualStyleBackColor = true;
            // 
            // radioButton_phaseSingleDiffer
            // 
            this.radioButton_phaseSingleDiffer.AutoSize = true;
            this.radioButton_phaseSingleDiffer.Location = new System.Drawing.Point(144, 15);
            this.radioButton_phaseSingleDiffer.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_phaseSingleDiffer.Name = "radioButton_phaseSingleDiffer";
            this.radioButton_phaseSingleDiffer.Size = new System.Drawing.Size(95, 16);
            this.radioButton_phaseSingleDiffer.TabIndex = 0;
            this.radioButton_phaseSingleDiffer.Text = "载波相位单差";
            this.radioButton_phaseSingleDiffer.UseVisualStyleBackColor = true;
            // 
            // radioButton_pppResidualDiffer
            // 
            this.radioButton_pppResidualDiffer.AutoSize = true;
            this.radioButton_pppResidualDiffer.Checked = true;
            this.radioButton_pppResidualDiffer.Location = new System.Drawing.Point(11, 18);
            this.radioButton_pppResidualDiffer.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_pppResidualDiffer.Name = "radioButton_pppResidualDiffer";
            this.radioButton_pppResidualDiffer.Size = new System.Drawing.Size(119, 16);
            this.radioButton_pppResidualDiffer.TabIndex = 0;
            this.radioButton_pppResidualDiffer.TabStop = true;
            this.radioButton_pppResidualDiffer.Text = "无电离层组合双差";
            this.radioButton_pppResidualDiffer.UseVisualStyleBackColor = true;
            // 
            // checkBox_enableClockFile
            // 
            this.checkBox_enableClockFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enableClockFile.AutoSize = true;
            this.checkBox_enableClockFile.Location = new System.Drawing.Point(476, 43);
            this.checkBox_enableClockFile.Name = "checkBox_enableClockFile";
            this.checkBox_enableClockFile.Size = new System.Drawing.Size(72, 16);
            this.checkBox_enableClockFile.TabIndex = 22;
            this.checkBox_enableClockFile.Text = "启用钟差";
            this.checkBox_enableClockFile.UseVisualStyleBackColor = true;
            this.checkBox_enableClockFile.CheckedChanged += new System.EventHandler(this.checkBox_enableClockFile_CheckedChanged);
            // 
            // button_getClockPath
            // 
            this.button_getClockPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getClockPath.Enabled = false;
            this.button_getClockPath.Location = new System.Drawing.Point(424, 35);
            this.button_getClockPath.Name = "button_getClockPath";
            this.button_getClockPath.Size = new System.Drawing.Size(52, 23);
            this.button_getClockPath.TabIndex = 19;
            this.button_getClockPath.Text = "...";
            this.button_getClockPath.UseVisualStyleBackColor = true;
            this.button_getClockPath.Click += new System.EventHandler(this.button_getClockPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "钟差数据文件：";
            // 
            // textBox_ClockPath
            // 
            this.textBox_ClockPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ClockPath.Enabled = false;
            this.textBox_ClockPath.Location = new System.Drawing.Point(101, 35);
            this.textBox_ClockPath.Name = "textBox_ClockPath";
            this.textBox_ClockPath.Size = new System.Drawing.Size(318, 21);
            this.textBox_ClockPath.TabIndex = 21;
            // 
            // button_getNavPath
            // 
            this.button_getNavPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getNavPath.Location = new System.Drawing.Point(424, 11);
            this.button_getNavPath.Name = "button_getNavPath";
            this.button_getNavPath.Size = new System.Drawing.Size(52, 23);
            this.button_getNavPath.TabIndex = 0;
            this.button_getNavPath.Text = "...";
            this.button_getNavPath.UseVisualStyleBackColor = true;
            this.button_getNavPath.Click += new System.EventHandler(this.button_getNavPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "星历数据文件：";
            // 
            // textBox_navPath
            // 
            this.textBox_navPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_navPath.Location = new System.Drawing.Point(101, 13);
            this.textBox_navPath.Name = "textBox_navPath";
            this.textBox_navPath.Size = new System.Drawing.Size(318, 21);
            this.textBox_navPath.TabIndex = 2;
            // 
            // tabControl_top
            // 
            this.tabControl_top.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_top.Controls.Add(this.tabPage5);
            this.tabControl_top.Controls.Add(this.tabPage2);
            this.tabControl_top.Controls.Add(this.tabPage8);
            this.tabControl_top.Controls.Add(this.tabPage6);
            this.tabControl_top.Controls.Add(this.tabPage10);
            this.tabControl_top.Location = new System.Drawing.Point(2, 2);
            this.tabControl_top.Name = "tabControl_top";
            this.tabControl_top.SelectedIndex = 0;
            this.tabControl_top.Size = new System.Drawing.Size(1262, 155);
            this.tabControl_top.TabIndex = 26;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.groupBox2);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage6.Size = new System.Drawing.Size(1254, 129);
            this.tabPage6.TabIndex = 4;
            this.tabPage6.Text = "公共数据源";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button_datasourceSeting);
            this.groupBox2.Controls.Add(this.checkBox_setEphemerisFile);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_navPath);
            this.groupBox2.Controls.Add(this.checkBox_enableClockFile);
            this.groupBox2.Controls.Add(this.button_getNavPath);
            this.groupBox2.Controls.Add(this.textBox_ClockPath);
            this.groupBox2.Controls.Add(this.button_getClockPath);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(596, 106);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "公共数据";
            // 
            // button_datasourceSeting
            // 
            this.button_datasourceSeting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_datasourceSeting.Location = new System.Drawing.Point(393, 69);
            this.button_datasourceSeting.Margin = new System.Windows.Forms.Padding(2);
            this.button_datasourceSeting.Name = "button_datasourceSeting";
            this.button_datasourceSeting.Size = new System.Drawing.Size(82, 25);
            this.button_datasourceSeting.TabIndex = 31;
            this.button_datasourceSeting.Text = "公共数据源";
            this.button_datasourceSeting.UseVisualStyleBackColor = true;
            this.button_datasourceSeting.Click += new System.EventHandler(this.button_datasourceSeting_Click);
            // 
            // checkBox_setEphemerisFile
            // 
            this.checkBox_setEphemerisFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_setEphemerisFile.AutoSize = true;
            this.checkBox_setEphemerisFile.Location = new System.Drawing.Point(476, 18);
            this.checkBox_setEphemerisFile.Name = "checkBox_setEphemerisFile";
            this.checkBox_setEphemerisFile.Size = new System.Drawing.Size(72, 16);
            this.checkBox_setEphemerisFile.TabIndex = 45;
            this.checkBox_setEphemerisFile.Text = "手动星历";
            this.checkBox_setEphemerisFile.UseVisualStyleBackColor = true;
            this.checkBox_setEphemerisFile.CheckedChanged += new System.EventHandler(this.checkBox_setEphemerisFile_CheckedChanged);
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.satDataTypeSelectControl2);
            this.tabPage10.Controls.Add(this.satDataTypeSelectControl1);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage10.Size = new System.Drawing.Size(1254, 129);
            this.tabPage10.TabIndex = 5;
            this.tabPage10.Text = "数值类型";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // satDataTypeSelectControl2
            // 
            this.satDataTypeSelectControl2.CurrentdType = Gnsser.SatApproxDataType.IonoFreeApproxPseudoRange;
            this.satDataTypeSelectControl2.Location = new System.Drawing.Point(6, 76);
            this.satDataTypeSelectControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.satDataTypeSelectControl2.Name = "satDataTypeSelectControl2";
            this.satDataTypeSelectControl2.Size = new System.Drawing.Size(282, 51);
            this.satDataTypeSelectControl2.TabIndex = 44;
            // 
            // satDataTypeSelectControl1
            // 
            this.satDataTypeSelectControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.satDataTypeSelectControl1.CurrentdType = Gnsser.SatObsDataType.AlignedIonoFreePhaseRange;
            this.satDataTypeSelectControl1.Location = new System.Drawing.Point(5, 5);
            this.satDataTypeSelectControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.satDataTypeSelectControl1.Name = "satDataTypeSelectControl1";
            this.satDataTypeSelectControl1.Size = new System.Drawing.Size(866, 66);
            this.satDataTypeSelectControl1.TabIndex = 41;
            // 
            // checkBox_outputAdjust
            // 
            this.checkBox_outputAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_outputAdjust.AutoSize = true;
            this.checkBox_outputAdjust.Location = new System.Drawing.Point(926, 166);
            this.checkBox_outputAdjust.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_outputAdjust.Name = "checkBox_outputAdjust";
            this.checkBox_outputAdjust.Size = new System.Drawing.Size(96, 16);
            this.checkBox_outputAdjust.TabIndex = 44;
            this.checkBox_outputAdjust.Text = "输出平差文件";
            this.checkBox_outputAdjust.UseVisualStyleBackColor = true;
            // 
            // checkBox_preAnalysis
            // 
            this.checkBox_preAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_preAnalysis.AutoSize = true;
            this.checkBox_preAnalysis.Location = new System.Drawing.Point(726, 214);
            this.checkBox_preAnalysis.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_preAnalysis.Name = "checkBox_preAnalysis";
            this.checkBox_preAnalysis.Size = new System.Drawing.Size(120, 16);
            this.checkBox_preAnalysis.TabIndex = 38;
            this.checkBox_preAnalysis.Text = "数据分析后再计算";
            this.checkBox_preAnalysis.UseVisualStyleBackColor = true;
            // 
            // button_analysisData
            // 
            this.button_analysisData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_analysisData.Location = new System.Drawing.Point(855, 207);
            this.button_analysisData.Name = "button_analysisData";
            this.button_analysisData.Size = new System.Drawing.Size(75, 25);
            this.button_analysisData.TabIndex = 10;
            this.button_analysisData.Text = "数据分析";
            this.button_analysisData.UseVisualStyleBackColor = true;
            this.button_analysisData.Click += new System.EventHandler(this.button_analysisData_Click);
            // 
            // checkBox_ambiguityFixed
            // 
            this.checkBox_ambiguityFixed.AutoSize = true;
            this.checkBox_ambiguityFixed.Location = new System.Drawing.Point(292, 213);
            this.checkBox_ambiguityFixed.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_ambiguityFixed.Name = "checkBox_ambiguityFixed";
            this.checkBox_ambiguityFixed.Size = new System.Drawing.Size(120, 16);
            this.checkBox_ambiguityFixed.TabIndex = 45;
            this.checkBox_ambiguityFixed.Text = "固定模糊度(双差)";
            this.checkBox_ambiguityFixed.UseVisualStyleBackColor = true;
            // 
            // checkBox_showProcessInfo
            // 
            this.checkBox_showProcessInfo.AutoSize = true;
            this.checkBox_showProcessInfo.Checked = true;
            this.checkBox_showProcessInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showProcessInfo.Location = new System.Drawing.Point(451, 211);
            this.checkBox_showProcessInfo.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_showProcessInfo.Name = "checkBox_showProcessInfo";
            this.checkBox_showProcessInfo.Size = new System.Drawing.Size(96, 16);
            this.checkBox_showProcessInfo.TabIndex = 46;
            this.checkBox_showProcessInfo.Text = "显示处理信息";
            this.checkBox_showProcessInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_autoMatchingFile
            // 
            this.checkBox_autoMatchingFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_autoMatchingFile.AutoSize = true;
            this.checkBox_autoMatchingFile.Checked = true;
            this.checkBox_autoMatchingFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autoMatchingFile.Location = new System.Drawing.Point(305, 257);
            this.checkBox_autoMatchingFile.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_autoMatchingFile.Name = "checkBox_autoMatchingFile";
            this.checkBox_autoMatchingFile.Size = new System.Drawing.Size(384, 16);
            this.checkBox_autoMatchingFile.TabIndex = 47;
            this.checkBox_autoMatchingFile.Text = "启用自动匹配数据源（可免部分数据源设置，如星历、钟差等文件）";
            this.checkBox_autoMatchingFile.UseVisualStyleBackColor = true;
            this.checkBox_autoMatchingFile.CheckedChanged += new System.EventHandler(this.checkBox_autoMatchingFile_CheckedChanged);
            // 
            // checkBox_enableNet
            // 
            this.checkBox_enableNet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enableNet.AutoSize = true;
            this.checkBox_enableNet.Checked = true;
            this.checkBox_enableNet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enableNet.Location = new System.Drawing.Point(713, 257);
            this.checkBox_enableNet.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_enableNet.Name = "checkBox_enableNet";
            this.checkBox_enableNet.Size = new System.Drawing.Size(96, 16);
            this.checkBox_enableNet.TabIndex = 48;
            this.checkBox_enableNet.Text = "允许访问网络";
            this.checkBox_enableNet.UseVisualStyleBackColor = true;
            this.checkBox_enableNet.CheckedChanged += new System.EventHandler(this.checkBox_enableNet_CheckedChanged);
            // 
            // checkBox_debugModel
            // 
            this.checkBox_debugModel.AutoSize = true;
            this.checkBox_debugModel.Location = new System.Drawing.Point(637, 176);
            this.checkBox_debugModel.Name = "checkBox_debugModel";
            this.checkBox_debugModel.Size = new System.Drawing.Size(72, 16);
            this.checkBox_debugModel.TabIndex = 49;
            this.checkBox_debugModel.Text = "调试模式";
            this.checkBox_debugModel.UseVisualStyleBackColor = true;
            this.checkBox_debugModel.CheckedChanged += new System.EventHandler(this.checkBox_debugModel_CheckedChanged);
            // 
            // BaseLinePositionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 474);
            this.Controls.Add(this.checkBox_debugModel);
            this.Controls.Add(this.checkBox_autoMatchingFile);
            this.Controls.Add(this.checkBox_enableNet);
            this.Controls.Add(this.checkBox_showProcessInfo);
            this.Controls.Add(this.checkBox_ambiguityFixed);
            this.Controls.Add(this.checkBox_outputAdjust);
            this.Controls.Add(this.checkBox_preAnalysis);
            this.Controls.Add(this.checkBox_ignoreCourceError);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.checkBox_outputReslultFile);
            this.Controls.Add(this.label_notice);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.button_toSinex);
            this.Controls.Add(this.button_drawDifferLine);
            this.Controls.Add(this.button_drawRmslines);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_cacuAll);
            this.Controls.Add(this.button_kalman);
            this.Controls.Add(this.button_analysisData);
            this.Controls.Add(this.button_showOnMap);
            this.Controls.Add(this.tabControl_top);
            this.Controls.Add(this.tabControl2);
            this.Name = "BaseLinePositionForm";
            this.Text = "基线解算";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointPositionForm_FormClosing);
            this.Load += new System.EventHandler(this.PointPositionForm_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabControl_top.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.TextBox textBox_estParam;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.Button button_cacuAll;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_kalman;
        private System.Windows.Forms.Button button_drawRmslines;
        private System.Windows.Forms.Button button_drawDifferLine;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox_appriori;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.Label label_progress;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TextBox textBox_differXyz;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TextBox textBox_resultInfo;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.TextBox textBox_obsMinusApriori;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ant;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ocean;
        private System.Windows.Forms.OpenFileDialog openFileDialog_satState;
        private System.Windows.Forms.OpenFileDialog openFileDialog_excludeSat;
        private System.Windows.Forms.Label label_notice;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Button button_toSinex;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox checkBox_ignoreCourceError;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_maxError;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_maxStd;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.RadioButton radio_noOut;
        private System.Windows.Forms.RadioButton radio_onTImeOutput;
        private System.Windows.Forms.RadioButton radio_out100;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_startEpoch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_caculateCount;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_gps2_0;
        private System.Windows.Forms.RadioButton radioButton_gps3_0;
        private System.Windows.Forms.RadioButton radioButton_beidou2_0;
        private System.Windows.Forms.RadioButton radioButton_beidou3;
        private System.Windows.Forms.CheckBox checkBox_enableClockFile;
        private System.Windows.Forms.Button button_getClockPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ClockPath;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_setObsPath_rov;
        private System.Windows.Forms.Button button_getObsPath_ref;
        private System.Windows.Forms.Button button_getNavPath;
        private System.Windows.Forms.TextBox textBox_obsFile_rov;
        private System.Windows.Forms.TextBox textBox_obsPath_ref;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_navPath;
        private System.Windows.Forms.TabControl tabControl_top;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_setEphemerisFile;
        private System.Windows.Forms.Button button_datasourceSeting;
        private System.Windows.Forms.Button button_openProjDir;
        private System.Windows.Forms.CheckBox checkBox_outputReslultFile;
        private System.Windows.Forms.TextBox textBox_taskName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton radioButton_pppResidualDiffer;
        private Controls.SatObsDataTypeControl satDataTypeSelectControl1;
        private System.Windows.Forms.RadioButton radioButton_phaseSingleDiffer;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_differSatCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_differEpochCount;
        private Geo.Winform.Controls.RmsedVectorRenderControl positonResultRenderControl11;
        private System.Windows.Forms.RadioButton radioButton_doubleDiffer;
        private System.Windows.Forms.TabPage tabPage10;
        private Controls.SatApproxDataTypeControl satDataTypeSelectControl2;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.Windows.Forms.CheckBox checkBox_outputAdjust;
        private System.Windows.Forms.RadioButton radioButton_norelevant;
        private System.Windows.Forms.RadioButton radioButtonGPSClose;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_maxstdTimes;
        private System.Windows.Forms.RadioButton radioButton_gps5km;
        private Geo.Winform.Controls.RmsedXyzControl rmsedXyzControl_rov;
        private Geo.Winform.Controls.RmsedXyzControl rmsedXyzControl_ref;
        private System.Windows.Forms.CheckBox checkBox_preAnalysis;
        private System.Windows.Forms.Button button_analysisData;
        private System.Windows.Forms.CheckBox checkBox_ambiguityFixed;
        private System.Windows.Forms.CheckBox checkBox_showProcessInfo;
        private System.Windows.Forms.CheckBox checkBox_autoMatchingFile;
        private System.Windows.Forms.CheckBox checkBox_enableNet;
        private System.Windows.Forms.CheckBox checkBox_debugModel;
    }
}

