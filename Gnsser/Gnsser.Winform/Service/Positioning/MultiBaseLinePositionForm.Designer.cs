namespace Gnsser.Winform
{
    partial class MultiBaseLinePositionForm
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
            this.button_addObsFiles = new System.Windows.Forms.Button();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_obsPath = new System.Windows.Forms.TextBox();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_kalman = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_setApproXyzPath = new System.Windows.Forms.Button();
            this.textBox_ApproXyzPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button_openProjDir = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.textBox_baseLinePath = new System.Windows.Forms.TextBox();
            this.textBox_taskName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_replaceObs = new System.Windows.Forms.Button();
            this.button_setRefObsPath = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.checkBox_ignoreException = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_maxStd = new System.Windows.Forms.TextBox();
            this.textBox_maxError = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_datasourceSeting = new System.Windows.Forms.Button();
            this.checkBox_setEphemerisFile = new System.Windows.Forms.CheckBox();
            this.checkBox_enableClockFile = new System.Windows.Forms.CheckBox();
            this.button_getClockPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ClockPath = new System.Windows.Forms.TextBox();
            this.button_getNavPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_navPath = new System.Windows.Forms.TextBox();
            this.checkBox_ignoreCourceError = new System.Windows.Forms.CheckBox();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.label_progress = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.textBox_resultInfo = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.label_fileCount = new System.Windows.Forms.Label();
            this.openFileDialog_singleObs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_ocean = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_ant = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_satState = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_excludeSat = new System.Windows.Forms.OpenFileDialog();
            this.checkBox_showProgress = new System.Windows.Forms.CheckBox();
            this.parallelConfigControl1 = new Geo.Winform.Controls.ParallelConfigControl();
            this.checkBox_outputAdjust = new System.Windows.Forms.CheckBox();
            this.checkBox_outputReslultFile = new System.Windows.Forms.CheckBox();
            this.openFileDialog_approXyzFile = new System.Windows.Forms.OpenFileDialog();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.groupBox1.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_addObsFiles
            // 
            this.button_addObsFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addObsFiles.Location = new System.Drawing.Point(733, 144);
            this.button_addObsFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_addObsFiles.Name = "button_addObsFiles";
            this.button_addObsFiles.Size = new System.Drawing.Size(108, 32);
            this.button_addObsFiles.TabIndex = 0;
            this.button_addObsFiles.Text = "添加";
            this.button_addObsFiles.UseVisualStyleBackColor = true;
            this.button_addObsFiles.Click += new System.EventHandler(this.button_addObsFiles_Click);
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件|*.*o|所有文件|*.*";
            this.openFileDialog_obs.Multiselect = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 114);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "观测文件：";
            // 
            // textBox_obsPath
            // 
            this.textBox_obsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_obsPath.Location = new System.Drawing.Point(95, 112);
            this.textBox_obsPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_obsPath.Multiline = true;
            this.textBox_obsPath.Name = "textBox_obsPath";
            this.textBox_obsPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_obsPath.Size = new System.Drawing.Size(631, 96);
            this.textBox_obsPath.TabIndex = 2;
            this.textBox_obsPath.TextChanged += new System.EventHandler(this.textBox_obsPath_TextChanged);
            // 
            // openFileDialog_nav
            // 
            this.openFileDialog_nav.FileName = "openFileDialog2";
            this.openFileDialog_nav.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(912, 269);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(101, 40);
            this.button_cancel.TabIndex = 19;
            this.button_cancel.Text = "取消计算";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_kalman
            // 
            this.button_kalman.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_kalman.Location = new System.Drawing.Point(804, 269);
            this.button_kalman.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_kalman.Name = "button_kalman";
            this.button_kalman.Size = new System.Drawing.Size(101, 40);
            this.button_kalman.TabIndex = 19;
            this.button_kalman.Text = "计算";
            this.button_kalman.UseVisualStyleBackColor = true;
            this.button_kalman.Click += new System.EventHandler(this.button_kalman_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_setApproXyzPath);
            this.groupBox1.Controls.Add(this.textBox_ApproXyzPath);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.multiGnssSystemSelectControl1);
            this.groupBox1.Controls.Add(this.button_openProjDir);
            this.groupBox1.Controls.Add(this.button_showOnMap);
            this.groupBox1.Controls.Add(this.textBox_baseLinePath);
            this.groupBox1.Controls.Add(this.textBox_taskName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.button_replaceObs);
            this.groupBox1.Controls.Add(this.button_setRefObsPath);
            this.groupBox1.Controls.Add(this.button_addObsFiles);
            this.groupBox1.Controls.Add(this.textBox_obsPath);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(997, 222);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据源设置";
            // 
            // button_setApproXyzPath
            // 
            this.button_setApproXyzPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setApproXyzPath.Location = new System.Drawing.Point(733, 79);
            this.button_setApproXyzPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_setApproXyzPath.Name = "button_setApproXyzPath";
            this.button_setApproXyzPath.Size = new System.Drawing.Size(108, 28);
            this.button_setApproXyzPath.TabIndex = 48;
            this.button_setApproXyzPath.Text = "...";
            this.button_setApproXyzPath.UseVisualStyleBackColor = true;
            this.button_setApproXyzPath.Click += new System.EventHandler(this.button_setApproXyzPath_Click);
            // 
            // textBox_ApproXyzPath
            // 
            this.textBox_ApproXyzPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ApproXyzPath.Location = new System.Drawing.Point(93, 79);
            this.textBox_ApproXyzPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_ApproXyzPath.Name = "textBox_ApproXyzPath";
            this.textBox_ApproXyzPath.Size = new System.Drawing.Size(633, 25);
            this.textBox_ApproXyzPath.TabIndex = 47;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 84);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 46;
            this.label7.Text = "概略坐标：";
            // 
            // button_openProjDir
            // 
            this.button_openProjDir.Location = new System.Drawing.Point(377, 16);
            this.button_openProjDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_openProjDir.Name = "button_openProjDir";
            this.button_openProjDir.Size = new System.Drawing.Size(87, 26);
            this.button_openProjDir.TabIndex = 38;
            this.button_openProjDir.Text = "打开目录";
            this.button_openProjDir.UseVisualStyleBackColor = true;
            this.button_openProjDir.Click += new System.EventHandler(this.button_openProjDir_Click);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_showOnMap.Location = new System.Drawing.Point(733, 176);
            this.button_showOnMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(108, 34);
            this.button_showOnMap.TabIndex = 44;
            this.button_showOnMap.Text = "地图查看";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // textBox_baseLinePath
            // 
            this.textBox_baseLinePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_baseLinePath.Location = new System.Drawing.Point(95, 48);
            this.textBox_baseLinePath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_baseLinePath.Name = "textBox_baseLinePath";
            this.textBox_baseLinePath.Size = new System.Drawing.Size(633, 25);
            this.textBox_baseLinePath.TabIndex = 28;
            // 
            // textBox_taskName
            // 
            this.textBox_taskName.Location = new System.Drawing.Point(95, 18);
            this.textBox_taskName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_taskName.Name = "textBox_taskName";
            this.textBox_taskName.Size = new System.Drawing.Size(276, 25);
            this.textBox_taskName.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 51);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 15);
            this.label6.TabIndex = 27;
            this.label6.Text = "基线列表：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 27;
            this.label5.Text = "工程名称：";
            // 
            // button_replaceObs
            // 
            this.button_replaceObs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_replaceObs.Location = new System.Drawing.Point(733, 111);
            this.button_replaceObs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_replaceObs.Name = "button_replaceObs";
            this.button_replaceObs.Size = new System.Drawing.Size(108, 34);
            this.button_replaceObs.TabIndex = 0;
            this.button_replaceObs.Text = "替换";
            this.button_replaceObs.UseVisualStyleBackColor = true;
            this.button_replaceObs.Click += new System.EventHandler(this.button_replaceObs_Click);
            // 
            // button_setRefObsPath
            // 
            this.button_setRefObsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setRefObsPath.Location = new System.Drawing.Point(733, 48);
            this.button_setRefObsPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_setRefObsPath.Name = "button_setRefObsPath";
            this.button_setRefObsPath.Size = new System.Drawing.Size(108, 28);
            this.button_setRefObsPath.TabIndex = 0;
            this.button_setRefObsPath.Text = "...";
            this.button_setRefObsPath.UseVisualStyleBackColor = true;
            this.button_setRefObsPath.Click += new System.EventHandler(this.button_setRefObsPath_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "SpResult";
            this.saveFileDialog1.Filter = "SINEX|*.SNX";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tabControl3
            // 
            this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage8);
            this.tabControl3.Controls.Add(this.tabPage2);
            this.tabControl3.Location = new System.Drawing.Point(3, 2);
            this.tabControl3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1013, 259);
            this.tabControl3.TabIndex = 26;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage5.Size = new System.Drawing.Size(1005, 230);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "基础设置";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.checkBox_ignoreException);
            this.tabPage8.Controls.Add(this.groupBox5);
            this.tabPage8.Location = new System.Drawing.Point(4, 25);
            this.tabPage8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage8.Size = new System.Drawing.Size(1005, 230);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "计算设置";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // checkBox_ignoreException
            // 
            this.checkBox_ignoreException.AutoSize = true;
            this.checkBox_ignoreException.Checked = true;
            this.checkBox_ignoreException.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ignoreException.Location = new System.Drawing.Point(7, 128);
            this.checkBox_ignoreException.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_ignoreException.Name = "checkBox_ignoreException";
            this.checkBox_ignoreException.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ignoreException.TabIndex = 34;
            this.checkBox_ignoreException.Text = "忽略错误";
            this.checkBox_ignoreException.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.textBox_maxStd);
            this.groupBox5.Controls.Add(this.textBox_maxError);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Location = new System.Drawing.Point(5, 16);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Size = new System.Drawing.Size(375, 106);
            this.groupBox5.TabIndex = 33;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "计算结果检核";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 15);
            this.label4.TabIndex = 31;
            this.label4.Text = "允许最大均方差：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 22);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 15);
            this.label12.TabIndex = 27;
            this.label12.Text = "相距超过";
            // 
            // textBox_maxStd
            // 
            this.textBox_maxStd.Location = new System.Drawing.Point(171, 55);
            this.textBox_maxStd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_maxStd.Name = "textBox_maxStd";
            this.textBox_maxStd.Size = new System.Drawing.Size(79, 25);
            this.textBox_maxStd.TabIndex = 28;
            this.textBox_maxStd.Text = "5000";
            // 
            // textBox_maxError
            // 
            this.textBox_maxError.Location = new System.Drawing.Point(105, 18);
            this.textBox_maxError.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_maxError.Name = "textBox_maxError";
            this.textBox_maxError.Size = new System.Drawing.Size(111, 25);
            this.textBox_maxError.TabIndex = 28;
            this.textBox_maxError.Text = "1000";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(223, 22);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(142, 15);
            this.label13.TabIndex = 29;
            this.label13.Text = "米，作为粗差剔除。";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(1005, 230);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "通用数据源";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button_datasourceSeting);
            this.groupBox2.Controls.Add(this.checkBox_setEphemerisFile);
            this.groupBox2.Controls.Add(this.checkBox_enableClockFile);
            this.groupBox2.Controls.Add(this.button_getClockPath);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_ClockPath);
            this.groupBox2.Controls.Add(this.button_getNavPath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_navPath);
            this.groupBox2.Location = new System.Drawing.Point(7, 8);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(923, 135);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "通用数据源";
            // 
            // button_datasourceSeting
            // 
            this.button_datasourceSeting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_datasourceSeting.Location = new System.Drawing.Point(643, 92);
            this.button_datasourceSeting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_datasourceSeting.Name = "button_datasourceSeting";
            this.button_datasourceSeting.Size = new System.Drawing.Size(99, 38);
            this.button_datasourceSeting.TabIndex = 63;
            this.button_datasourceSeting.Text = "公共数据源";
            this.button_datasourceSeting.UseVisualStyleBackColor = true;
            this.button_datasourceSeting.Click += new System.EventHandler(this.button_datasourceSeting_Click);
            // 
            // checkBox_setEphemerisFile
            // 
            this.checkBox_setEphemerisFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_setEphemerisFile.AutoSize = true;
            this.checkBox_setEphemerisFile.Location = new System.Drawing.Point(700, 29);
            this.checkBox_setEphemerisFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_setEphemerisFile.Name = "checkBox_setEphemerisFile";
            this.checkBox_setEphemerisFile.Size = new System.Drawing.Size(119, 19);
            this.checkBox_setEphemerisFile.TabIndex = 61;
            this.checkBox_setEphemerisFile.Text = "手动设置星历";
            this.checkBox_setEphemerisFile.UseVisualStyleBackColor = true;
            this.checkBox_setEphemerisFile.CheckedChanged += new System.EventHandler(this.checkBox_setEphemerisFile_CheckedChanged);
            // 
            // checkBox_enableClockFile
            // 
            this.checkBox_enableClockFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enableClockFile.AutoSize = true;
            this.checkBox_enableClockFile.Location = new System.Drawing.Point(701, 60);
            this.checkBox_enableClockFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_enableClockFile.Name = "checkBox_enableClockFile";
            this.checkBox_enableClockFile.Size = new System.Drawing.Size(119, 19);
            this.checkBox_enableClockFile.TabIndex = 62;
            this.checkBox_enableClockFile.Text = "启用钟差文件";
            this.checkBox_enableClockFile.UseVisualStyleBackColor = true;
            this.checkBox_enableClockFile.CheckedChanged += new System.EventHandler(this.checkBox_enableClockFile_CheckedChanged);
            // 
            // button_getClockPath
            // 
            this.button_getClockPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getClockPath.Enabled = false;
            this.button_getClockPath.Location = new System.Drawing.Point(643, 56);
            this.button_getClockPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_getClockPath.Name = "button_getClockPath";
            this.button_getClockPath.Size = new System.Drawing.Size(49, 26);
            this.button_getClockPath.TabIndex = 58;
            this.button_getClockPath.Text = "...";
            this.button_getClockPath.UseVisualStyleBackColor = true;
            this.button_getClockPath.Click += new System.EventHandler(this.button_getClockPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 61);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 59;
            this.label3.Text = "钟差数据文件：";
            // 
            // textBox_ClockPath
            // 
            this.textBox_ClockPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ClockPath.Enabled = false;
            this.textBox_ClockPath.Location = new System.Drawing.Point(123, 58);
            this.textBox_ClockPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_ClockPath.Name = "textBox_ClockPath";
            this.textBox_ClockPath.Size = new System.Drawing.Size(513, 25);
            this.textBox_ClockPath.TabIndex = 60;
            // 
            // button_getNavPath
            // 
            this.button_getNavPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getNavPath.Location = new System.Drawing.Point(643, 22);
            this.button_getNavPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_getNavPath.Name = "button_getNavPath";
            this.button_getNavPath.Size = new System.Drawing.Size(49, 28);
            this.button_getNavPath.TabIndex = 55;
            this.button_getNavPath.Text = "...";
            this.button_getNavPath.UseVisualStyleBackColor = true;
            this.button_getNavPath.Click += new System.EventHandler(this.button_getNavPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 56;
            this.label2.Text = "星历数据文件：";
            // 
            // textBox_navPath
            // 
            this.textBox_navPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_navPath.Location = new System.Drawing.Point(123, 25);
            this.textBox_navPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_navPath.Name = "textBox_navPath";
            this.textBox_navPath.Size = new System.Drawing.Size(513, 25);
            this.textBox_navPath.TabIndex = 57;
            // 
            // checkBox_ignoreCourceError
            // 
            this.checkBox_ignoreCourceError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_ignoreCourceError.AutoSize = true;
            this.checkBox_ignoreCourceError.Checked = true;
            this.checkBox_ignoreCourceError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ignoreCourceError.Location = new System.Drawing.Point(691, 318);
            this.checkBox_ignoreCourceError.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_ignoreCourceError.Name = "checkBox_ignoreCourceError";
            this.checkBox_ignoreCourceError.Size = new System.Drawing.Size(89, 19);
            this.checkBox_ignoreCourceError.TabIndex = 32;
            this.checkBox_ignoreCourceError.Text = "过滤粗差";
            this.checkBox_ignoreCourceError.UseVisualStyleBackColor = true;
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "openFileDialog2";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk|所有文件|*.*";
            // 
            // label_progress
            // 
            this.label_progress.AutoSize = true;
            this.label_progress.Location = new System.Drawing.Point(165, 285);
            this.label_progress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(67, 15);
            this.label_progress.TabIndex = 27;
            this.label_progress.Text = "进度显示";
            // 
            // tabPage0
            // 
            this.tabPage9.Controls.Add(this.textBox_resultInfo);
            this.tabPage9.Location = new System.Drawing.Point(4, 25);
            this.tabPage9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage9.Size = new System.Drawing.Size(1013, 216);
            this.tabPage9.TabIndex = 4;
            this.tabPage9.Text = "处理信息";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // textBox_resultInfo
            // 
            this.textBox_resultInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_resultInfo.Location = new System.Drawing.Point(3, 2);
            this.textBox_resultInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_resultInfo.Multiline = true;
            this.textBox_resultInfo.Name = "textBox_resultInfo";
            this.textBox_resultInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_resultInfo.Size = new System.Drawing.Size(1007, 212);
            this.textBox_resultInfo.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_result);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(1013, 216);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "解算结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox_result
            // 
            this.textBox_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_result.Location = new System.Drawing.Point(4, 4);
            this.textBox_result.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(1005, 208);
            this.textBox_result.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Location = new System.Drawing.Point(3, 338);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1021, 245);
            this.tabControl2.TabIndex = 24;
            // 
            // label_fileCount
            // 
            this.label_fileCount.AutoSize = true;
            this.label_fileCount.Location = new System.Drawing.Point(4, 285);
            this.label_fileCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_fileCount.Name = "label_fileCount";
            this.label_fileCount.Size = new System.Drawing.Size(127, 15);
            this.label_fileCount.TabIndex = 27;
            this.label_fileCount.Text = "文件数量进度显示";
            // 
            // openFileDialog_singleObs
            // 
            this.openFileDialog_singleObs.Filter = "文本文件|*.*txt|所有文件|*.*";
            // 
            // openFileDialog_ocean
            // 
            this.openFileDialog_ocean.Filter = "海洋潮汐文件|*.dat|所有文件|*.*";
            // 
            // openFileDialog_ant
            // 
            this.openFileDialog_ant.Filter = "天线文件|*.atx|所有文件|*.*";
            // 
            // openFileDialog_satState
            // 
            this.openFileDialog_satState.Filter = "所有文件|*.*";
            // 
            // openFileDialog_excludeSat
            // 
            this.openFileDialog_excludeSat.Filter = "排除卫星文件|*.dat|所有文件|*.*";
            // 
            // checkBox_showProgress
            // 
            this.checkBox_showProgress.AutoSize = true;
            this.checkBox_showProgress.Checked = true;
            this.checkBox_showProgress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showProgress.Location = new System.Drawing.Point(276, 284);
            this.checkBox_showProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_showProgress.Name = "checkBox_showProgress";
            this.checkBox_showProgress.Size = new System.Drawing.Size(89, 19);
            this.checkBox_showProgress.TabIndex = 40;
            this.checkBox_showProgress.Text = "显示进度";
            this.checkBox_showProgress.UseVisualStyleBackColor = true;
            // 
            // parallelConfigControl1
            // 
            this.parallelConfigControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.parallelConfigControl1.Location = new System.Drawing.Point(459, 272);
            this.parallelConfigControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.parallelConfigControl1.Name = "parallelConfigControl1";
            this.parallelConfigControl1.Size = new System.Drawing.Size(339, 32);
            this.parallelConfigControl1.TabIndex = 42;
            // 
            // checkBox_outputAdjust
            // 
            this.checkBox_outputAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_outputAdjust.AutoSize = true;
            this.checkBox_outputAdjust.Location = new System.Drawing.Point(554, 316);
            this.checkBox_outputAdjust.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_outputAdjust.Name = "checkBox_outputAdjust";
            this.checkBox_outputAdjust.Size = new System.Drawing.Size(119, 19);
            this.checkBox_outputAdjust.TabIndex = 46;
            this.checkBox_outputAdjust.Text = "输出平差文件";
            this.checkBox_outputAdjust.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputReslultFile
            // 
            this.checkBox_outputReslultFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_outputReslultFile.AutoSize = true;
            this.checkBox_outputReslultFile.Checked = true;
            this.checkBox_outputReslultFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_outputReslultFile.Location = new System.Drawing.Point(463, 316);
            this.checkBox_outputReslultFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_outputReslultFile.Name = "checkBox_outputReslultFile";
            this.checkBox_outputReslultFile.Size = new System.Drawing.Size(89, 19);
            this.checkBox_outputReslultFile.TabIndex = 45;
            this.checkBox_outputReslultFile.Text = "输出文件";
            this.checkBox_outputReslultFile.UseVisualStyleBackColor = true;
            this.checkBox_outputReslultFile.CheckedChanged += new System.EventHandler(this.checkBox_outputReslultFile_CheckedChanged);
            // 
            // openFileDialog_approXyzFile
            // 
            this.openFileDialog_approXyzFile.FileName = "openFileDialog_approXyzFile";
            this.openFileDialog_approXyzFile.Filter = "文本文件|*.*txt|所有文件|*.*";
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(871, 48);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(101, 118);
            this.multiGnssSystemSelectControl1.TabIndex = 45;
            // 
            // MultiBaseLinePositionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 590);
            this.Controls.Add(this.checkBox_ignoreCourceError);
            this.Controls.Add(this.checkBox_outputAdjust);
            this.Controls.Add(this.checkBox_outputReslultFile);
            this.Controls.Add(this.parallelConfigControl1);
            this.Controls.Add(this.checkBox_showProgress);
            this.Controls.Add(this.label_fileCount);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_kalman);
            this.Controls.Add(this.tabControl3);
            this.Controls.Add(this.tabControl2);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MultiBaseLinePositionForm";
            this.Text = "批量基线解算";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointPositionForm_FormClosing);
            this.Load += new System.EventHandler(this.PointPositionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_addObsFiles;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_obsPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_kalman;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.Label label_progress;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_maxStd;
        private System.Windows.Forms.TextBox textBox_maxError;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox checkBox_ignoreCourceError;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TextBox textBox_resultInfo;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.Label label_fileCount;
        private System.Windows.Forms.OpenFileDialog openFileDialog_singleObs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ocean;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ant;
        private System.Windows.Forms.OpenFileDialog openFileDialog_satState;
        private System.Windows.Forms.OpenFileDialog openFileDialog_excludeSat;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.TextBox textBox_taskName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_openProjDir;
        private System.Windows.Forms.CheckBox checkBox_showProgress;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_setEphemerisFile;
        private System.Windows.Forms.CheckBox checkBox_enableClockFile;
        private System.Windows.Forms.Button button_getClockPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ClockPath;
        private System.Windows.Forms.Button button_getNavPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_navPath;
        private System.Windows.Forms.Button button_datasourceSeting;
        private Geo.Winform.Controls.ParallelConfigControl parallelConfigControl1;
        private System.Windows.Forms.Button button_replaceObs;
        private System.Windows.Forms.TextBox textBox_baseLinePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_setRefObsPath;
        private System.Windows.Forms.CheckBox checkBox_ignoreException;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.Windows.Forms.CheckBox checkBox_outputAdjust;
        private System.Windows.Forms.CheckBox checkBox_outputReslultFile;
        private System.Windows.Forms.Button button_setApproXyzPath;
        private System.Windows.Forms.TextBox textBox_ApproXyzPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.OpenFileDialog openFileDialog_approXyzFile;
    }
}

