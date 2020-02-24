namespace Gnsser.Winform
{
    partial class PathSettingForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_localEphemerisDir = new System.Windows.Forms.Button();
            this.button_setDataPath = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.button_setOutDir = new System.Windows.Forms.Button();
            this.textBox_localEphemerisDir = new System.Windows.Forms.TextBox();
            this.textBox_dataBaseDir = new System.Windows.Forms.TextBox();
            this.textBox_outDir = new System.Windows.Forms.TextBox();
            this.button_setComputePath = new System.Windows.Forms.Button();
            this.button_setTaskPath = new System.Windows.Forms.Button();
            this.button_setsitepath = new System.Windows.Forms.Button();
            this.textBox_compuNode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_task = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_siteName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1SampleSinexFile = new System.Windows.Forms.Button();
            this.button2SampleSP3File = new System.Windows.Forms.Button();
            this.button3SampleNFile = new System.Windows.Forms.Button();
            this.button4SampleOFile = new System.Windows.Forms.Button();
            this.textBox1SampleSinexFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2SampleSP3File = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3SampleNFile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox4SampleOFile = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_setDcbDir = new System.Windows.Forms.Button();
            this.textBox_dcbDir = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button_satExclude = new System.Windows.Forms.Button();
            this.textBox_satState = new System.Windows.Forms.TextBox();
            this.button_satState = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button_ocean = new System.Windows.Forms.Button();
            this.textBox_antennaFile = new System.Windows.Forms.TextBox();
            this.button_setAntennaFile = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_satExclude = new System.Windows.Forms.TextBox();
            this.textBox_ocean = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.openFileDialog_ocean = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_ant = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_satState = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_excludeSat = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_localEphemerisDir);
            this.groupBox1.Controls.Add(this.button_setDataPath);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.button_setOutDir);
            this.groupBox1.Controls.Add(this.textBox_localEphemerisDir);
            this.groupBox1.Controls.Add(this.textBox_dataBaseDir);
            this.groupBox1.Controls.Add(this.textBox_outDir);
            this.groupBox1.Controls.Add(this.button_setComputePath);
            this.groupBox1.Controls.Add(this.button_setTaskPath);
            this.groupBox1.Controls.Add(this.button_setsitepath);
            this.groupBox1.Controls.Add(this.textBox_compuNode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_task);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_siteName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 362);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基础信息";
            // 
            // button_localEphemerisDir
            // 
            this.button_localEphemerisDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_localEphemerisDir.Location = new System.Drawing.Point(324, 192);
            this.button_localEphemerisDir.Margin = new System.Windows.Forms.Padding(2);
            this.button_localEphemerisDir.Name = "button_localEphemerisDir";
            this.button_localEphemerisDir.Size = new System.Drawing.Size(40, 22);
            this.button_localEphemerisDir.TabIndex = 13;
            this.button_localEphemerisDir.Text = "...";
            this.button_localEphemerisDir.UseVisualStyleBackColor = true;
            this.button_localEphemerisDir.Click += new System.EventHandler(this.button_localEphemerisDir_Click);
            // 
            // button_setDataPath
            // 
            this.button_setDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setDataPath.Location = new System.Drawing.Point(324, 162);
            this.button_setDataPath.Margin = new System.Windows.Forms.Padding(2);
            this.button_setDataPath.Name = "button_setDataPath";
            this.button_setDataPath.Size = new System.Drawing.Size(40, 22);
            this.button_setDataPath.TabIndex = 11;
            this.button_setDataPath.Text = "...";
            this.button_setDataPath.UseVisualStyleBackColor = true;
            this.button_setDataPath.Click += new System.EventHandler(this.button_setDataPath_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(4, 194);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 12);
            this.label16.TabIndex = 22;
            this.label16.Text = "本地星历目录：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 165);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 22;
            this.label15.Text = "数据根目录：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 139);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 22;
            this.label14.Text = "输出根目录：";
            // 
            // button_setOutDir
            // 
            this.button_setOutDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setOutDir.Location = new System.Drawing.Point(325, 134);
            this.button_setOutDir.Name = "button_setOutDir";
            this.button_setOutDir.Size = new System.Drawing.Size(40, 23);
            this.button_setOutDir.TabIndex = 9;
            this.button_setOutDir.Text = "...";
            this.button_setOutDir.UseVisualStyleBackColor = true;
            this.button_setOutDir.Click += new System.EventHandler(this.button_setOutDir_Click);
            // 
            // textBox_localEphemerisDir
            // 
            this.textBox_localEphemerisDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_localEphemerisDir.Location = new System.Drawing.Point(95, 192);
            this.textBox_localEphemerisDir.Name = "textBox_localEphemerisDir";
            this.textBox_localEphemerisDir.Size = new System.Drawing.Size(225, 21);
            this.textBox_localEphemerisDir.TabIndex = 12;
            // 
            // textBox_dataBaseDir
            // 
            this.textBox_dataBaseDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dataBaseDir.Location = new System.Drawing.Point(95, 162);
            this.textBox_dataBaseDir.Name = "textBox_dataBaseDir";
            this.textBox_dataBaseDir.Size = new System.Drawing.Size(225, 21);
            this.textBox_dataBaseDir.TabIndex = 10;
            // 
            // textBox_outDir
            // 
            this.textBox_outDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_outDir.Location = new System.Drawing.Point(95, 137);
            this.textBox_outDir.Name = "textBox_outDir";
            this.textBox_outDir.Size = new System.Drawing.Size(225, 21);
            this.textBox_outDir.TabIndex = 8;
            this.textBox_outDir.Text = "C:\\GnsserOutput";
            // 
            // button_setComputePath
            // 
            this.button_setComputePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setComputePath.Location = new System.Drawing.Point(323, 107);
            this.button_setComputePath.Name = "button_setComputePath";
            this.button_setComputePath.Size = new System.Drawing.Size(41, 23);
            this.button_setComputePath.TabIndex = 7;
            this.button_setComputePath.Text = "...";
            this.button_setComputePath.UseVisualStyleBackColor = true;
            this.button_setComputePath.Click += new System.EventHandler(this.button_setComputePath_Click);
            // 
            // button_setTaskPath
            // 
            this.button_setTaskPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setTaskPath.Location = new System.Drawing.Point(323, 80);
            this.button_setTaskPath.Name = "button_setTaskPath";
            this.button_setTaskPath.Size = new System.Drawing.Size(41, 23);
            this.button_setTaskPath.TabIndex = 5;
            this.button_setTaskPath.Text = "...";
            this.button_setTaskPath.UseVisualStyleBackColor = true;
            this.button_setTaskPath.Click += new System.EventHandler(this.button_setTaskPath_Click);
            // 
            // button_setsitepath
            // 
            this.button_setsitepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setsitepath.Location = new System.Drawing.Point(323, 23);
            this.button_setsitepath.Name = "button_setsitepath";
            this.button_setsitepath.Size = new System.Drawing.Size(41, 23);
            this.button_setsitepath.TabIndex = 1;
            this.button_setsitepath.Text = "...";
            this.button_setsitepath.UseVisualStyleBackColor = true;
            this.button_setsitepath.Click += new System.EventHandler(this.button_setsitepath_Click);
            // 
            // textBox_compuNode
            // 
            this.textBox_compuNode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_compuNode.Location = new System.Drawing.Point(96, 104);
            this.textBox_compuNode.Name = "textBox_compuNode";
            this.textBox_compuNode.Size = new System.Drawing.Size(225, 21);
            this.textBox_compuNode.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "计算节点：";
            // 
            // textBox_task
            // 
            this.textBox_task.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_task.Location = new System.Drawing.Point(96, 77);
            this.textBox_task.Name = "textBox_task";
            this.textBox_task.Size = new System.Drawing.Size(225, 21);
            this.textBox_task.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "计算任务：";
            // 
            // textBox_siteName
            // 
            this.textBox_siteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_siteName.Location = new System.Drawing.Point(96, 23);
            this.textBox_siteName.Name = "textBox_siteName";
            this.textBox_siteName.Size = new System.Drawing.Size(225, 21);
            this.textBox_siteName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "测站文件：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "XML,TXT|*.xml;*.txt|Rinex,Sinex|*.*O;*.*n;*.Sp3;*.rnx|所有文件|*.*";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button1SampleSinexFile);
            this.groupBox2.Controls.Add(this.button2SampleSP3File);
            this.groupBox2.Controls.Add(this.button3SampleNFile);
            this.groupBox2.Controls.Add(this.button4SampleOFile);
            this.groupBox2.Controls.Add(this.textBox1SampleSinexFile);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox2SampleSP3File);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox3SampleNFile);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBox4SampleOFile);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(432, 139);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "范例数据";
            // 
            // button1SampleSinexFile
            // 
            this.button1SampleSinexFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1SampleSinexFile.Location = new System.Drawing.Point(384, 104);
            this.button1SampleSinexFile.Name = "button1SampleSinexFile";
            this.button1SampleSinexFile.Size = new System.Drawing.Size(40, 23);
            this.button1SampleSinexFile.TabIndex = 7;
            this.button1SampleSinexFile.Text = "...";
            this.button1SampleSinexFile.UseVisualStyleBackColor = true;
            this.button1SampleSinexFile.Click += new System.EventHandler(this.button1SampleSinexFile_Click);
            // 
            // button2SampleSP3File
            // 
            this.button2SampleSP3File.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2SampleSP3File.Location = new System.Drawing.Point(384, 77);
            this.button2SampleSP3File.Name = "button2SampleSP3File";
            this.button2SampleSP3File.Size = new System.Drawing.Size(40, 23);
            this.button2SampleSP3File.TabIndex = 5;
            this.button2SampleSP3File.Text = "...";
            this.button2SampleSP3File.UseVisualStyleBackColor = true;
            this.button2SampleSP3File.Click += new System.EventHandler(this.button2SampleSP3File_Click);
            // 
            // button3SampleNFile
            // 
            this.button3SampleNFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3SampleNFile.Location = new System.Drawing.Point(384, 50);
            this.button3SampleNFile.Name = "button3SampleNFile";
            this.button3SampleNFile.Size = new System.Drawing.Size(40, 23);
            this.button3SampleNFile.TabIndex = 3;
            this.button3SampleNFile.Text = "...";
            this.button3SampleNFile.UseVisualStyleBackColor = true;
            this.button3SampleNFile.Click += new System.EventHandler(this.button3SampleNFile_Click);
            // 
            // button4SampleOFile
            // 
            this.button4SampleOFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4SampleOFile.Location = new System.Drawing.Point(384, 20);
            this.button4SampleOFile.Name = "button4SampleOFile";
            this.button4SampleOFile.Size = new System.Drawing.Size(40, 23);
            this.button4SampleOFile.TabIndex = 1;
            this.button4SampleOFile.Text = "...";
            this.button4SampleOFile.UseVisualStyleBackColor = true;
            this.button4SampleOFile.Click += new System.EventHandler(this.button4SampleOFile_Click);
            // 
            // textBox1SampleSinexFile
            // 
            this.textBox1SampleSinexFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1SampleSinexFile.Location = new System.Drawing.Point(96, 104);
            this.textBox1SampleSinexFile.Name = "textBox1SampleSinexFile";
            this.textBox1SampleSinexFile.Size = new System.Drawing.Size(282, 21);
            this.textBox1SampleSinexFile.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Sinex文件：";
            // 
            // textBox2SampleSP3File
            // 
            this.textBox2SampleSP3File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2SampleSP3File.Location = new System.Drawing.Point(96, 77);
            this.textBox2SampleSP3File.Name = "textBox2SampleSP3File";
            this.textBox2SampleSP3File.Size = new System.Drawing.Size(282, 21);
            this.textBox2SampleSP3File.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "SP3文件：";
            // 
            // textBox3SampleNFile
            // 
            this.textBox3SampleNFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3SampleNFile.Location = new System.Drawing.Point(96, 50);
            this.textBox3SampleNFile.Name = "textBox3SampleNFile";
            this.textBox3SampleNFile.Size = new System.Drawing.Size(282, 21);
            this.textBox3SampleNFile.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "Rinex N文件：";
            // 
            // textBox4SampleOFile
            // 
            this.textBox4SampleOFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4SampleOFile.Location = new System.Drawing.Point(96, 23);
            this.textBox4SampleOFile.Name = "textBox4SampleOFile";
            this.textBox4SampleOFile.Size = new System.Drawing.Size(282, 21);
            this.textBox4SampleOFile.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "Rinex O文件：";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button_setDcbDir);
            this.groupBox3.Controls.Add(this.textBox_dcbDir);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.button_satExclude);
            this.groupBox3.Controls.Add(this.textBox_satState);
            this.groupBox3.Controls.Add(this.button_satState);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.button_ocean);
            this.groupBox3.Controls.Add(this.textBox_antennaFile);
            this.groupBox3.Controls.Add(this.button_setAntennaFile);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBox_satExclude);
            this.groupBox3.Controls.Add(this.textBox_ocean);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(8, 147);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(428, 159);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "通用数据源路径";
            // 
            // button_setDcbDir
            // 
            this.button_setDcbDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setDcbDir.Location = new System.Drawing.Point(379, 122);
            this.button_setDcbDir.Name = "button_setDcbDir";
            this.button_setDcbDir.Size = new System.Drawing.Size(41, 23);
            this.button_setDcbDir.TabIndex = 9;
            this.button_setDcbDir.Text = "...";
            this.button_setDcbDir.UseVisualStyleBackColor = true;
            this.button_setDcbDir.Click += new System.EventHandler(this.button_setDcbDir_Click);
            // 
            // textBox_dcbDir
            // 
            this.textBox_dcbDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dcbDir.Location = new System.Drawing.Point(92, 122);
            this.textBox_dcbDir.Name = "textBox_dcbDir";
            this.textBox_dcbDir.Size = new System.Drawing.Size(282, 21);
            this.textBox_dcbDir.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 125);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 12);
            this.label13.TabIndex = 3;
            this.label13.Text = "卫星DCB目录：";
            // 
            // button_satExclude
            // 
            this.button_satExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_satExclude.Location = new System.Drawing.Point(379, 95);
            this.button_satExclude.Name = "button_satExclude";
            this.button_satExclude.Size = new System.Drawing.Size(41, 23);
            this.button_satExclude.TabIndex = 7;
            this.button_satExclude.Text = "...";
            this.button_satExclude.UseVisualStyleBackColor = true;
            this.button_satExclude.Click += new System.EventHandler(this.button_satExclude_Click);
            // 
            // textBox_satState
            // 
            this.textBox_satState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_satState.Location = new System.Drawing.Point(92, 68);
            this.textBox_satState.Name = "textBox_satState";
            this.textBox_satState.Size = new System.Drawing.Size(282, 21);
            this.textBox_satState.TabIndex = 4;
            // 
            // button_satState
            // 
            this.button_satState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_satState.Location = new System.Drawing.Point(379, 69);
            this.button_satState.Name = "button_satState";
            this.button_satState.Size = new System.Drawing.Size(41, 23);
            this.button_satState.TabIndex = 5;
            this.button_satState.Text = "...";
            this.button_satState.UseVisualStyleBackColor = true;
            this.button_satState.Click += new System.EventHandler(this.button_satState_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "天线文件：";
            // 
            // button_ocean
            // 
            this.button_ocean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ocean.Location = new System.Drawing.Point(379, 42);
            this.button_ocean.Name = "button_ocean";
            this.button_ocean.Size = new System.Drawing.Size(41, 23);
            this.button_ocean.TabIndex = 3;
            this.button_ocean.Text = "...";
            this.button_ocean.UseVisualStyleBackColor = true;
            this.button_ocean.Click += new System.EventHandler(this.button_ocean_Click);
            // 
            // textBox_antennaFile
            // 
            this.textBox_antennaFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_antennaFile.Location = new System.Drawing.Point(92, 14);
            this.textBox_antennaFile.Name = "textBox_antennaFile";
            this.textBox_antennaFile.Size = new System.Drawing.Size(282, 21);
            this.textBox_antennaFile.TabIndex = 0;
            // 
            // button_setAntennaFile
            // 
            this.button_setAntennaFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setAntennaFile.Location = new System.Drawing.Point(379, 12);
            this.button_setAntennaFile.Name = "button_setAntennaFile";
            this.button_setAntennaFile.Size = new System.Drawing.Size(41, 23);
            this.button_setAntennaFile.TabIndex = 1;
            this.button_setAntennaFile.Text = "...";
            this.button_setAntennaFile.UseVisualStyleBackColor = true;
            this.button_setAntennaFile.Click += new System.EventHandler(this.button_setAntennaFile_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "海洋潮汐文件：";
            // 
            // textBox_satExclude
            // 
            this.textBox_satExclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_satExclude.Location = new System.Drawing.Point(92, 95);
            this.textBox_satExclude.Name = "textBox_satExclude";
            this.textBox_satExclude.Size = new System.Drawing.Size(282, 21);
            this.textBox_satExclude.TabIndex = 6;
            // 
            // textBox_ocean
            // 
            this.textBox_ocean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ocean.Location = new System.Drawing.Point(92, 41);
            this.textBox_ocean.Name = "textBox_ocean";
            this.textBox_ocean.Size = new System.Drawing.Size(282, 21);
            this.textBox_ocean.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "卫星排除文件：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "卫星状态文件：";
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存ToolStripMenuItem,
            this.重置ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // 重置ToolStripMenuItem
            // 
            this.重置ToolStripMenuItem.Name = "重置ToolStripMenuItem";
            this.重置ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.重置ToolStripMenuItem.Text = "重置";
            this.重置ToolStripMenuItem.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(818, 362);
            this.splitContainer1.SplitterDistance = 372;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 6;
            // 
            // PathSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 362);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PathSettingForm";
            this.Text = "文件地址设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_siteName;
        private System.Windows.Forms.TextBox textBox_compuNode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_task;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_setComputePath;
        private System.Windows.Forms.Button button_setTaskPath;
        private System.Windows.Forms.Button button_setsitepath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1SampleSinexFile;
        private System.Windows.Forms.Button button2SampleSP3File;
        private System.Windows.Forms.Button button3SampleNFile;
        private System.Windows.Forms.Button button4SampleOFile;
        private System.Windows.Forms.TextBox textBox1SampleSinexFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2SampleSP3File;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox3SampleNFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox4SampleOFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_satExclude;
        private System.Windows.Forms.TextBox textBox_satState;
        private System.Windows.Forms.Button button_satState;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_ocean;
        private System.Windows.Forms.TextBox textBox_antennaFile;
        private System.Windows.Forms.Button button_setAntennaFile;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_satExclude;
        private System.Windows.Forms.TextBox textBox_ocean;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ocean;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ant;
        private System.Windows.Forms.OpenFileDialog openFileDialog_satState;
        private System.Windows.Forms.OpenFileDialog openFileDialog_excludeSat;
        private System.Windows.Forms.Button button_setDcbDir;
        private System.Windows.Forms.TextBox textBox_dcbDir;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button_setOutDir;
        private System.Windows.Forms.TextBox textBox_outDir;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重置ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button_setDataPath;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox_dataBaseDir;
        private System.Windows.Forms.Button button_localEphemerisDir;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox_localEphemerisDir;
    }
}