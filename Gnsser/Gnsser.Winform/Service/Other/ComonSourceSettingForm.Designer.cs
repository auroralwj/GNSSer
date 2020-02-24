namespace Gnsser.Winform
{
    partial class ComonSourceSettingForm
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
            this.button_localEphemerisDir = new System.Windows.Forms.Button();
            this.button_setDataPath = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.button_setOutDir = new System.Windows.Forms.Button();
            this.textBox_localEphemerisDir = new System.Windows.Forms.TextBox();
            this.textBox_dataBaseDir = new System.Windows.Forms.TextBox();
            this.textBox_outDir = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
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
            this.button_reset = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_localEphemerisDir
            // 
            this.button_localEphemerisDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_localEphemerisDir.Location = new System.Drawing.Point(588, 210);
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
            this.button_setDataPath.Location = new System.Drawing.Point(588, 180);
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
            this.label16.Location = new System.Drawing.Point(1, 211);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 12);
            this.label16.TabIndex = 22;
            this.label16.Text = "本地星历目录：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 182);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 22;
            this.label15.Text = "数据根目录：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 156);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 22;
            this.label14.Text = "输出根目录：";
            // 
            // button_setOutDir
            // 
            this.button_setOutDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setOutDir.Location = new System.Drawing.Point(588, 152);
            this.button_setOutDir.Name = "button_setOutDir";
            this.button_setOutDir.Size = new System.Drawing.Size(40, 22);
            this.button_setOutDir.TabIndex = 9;
            this.button_setOutDir.Text = "...";
            this.button_setOutDir.UseVisualStyleBackColor = true;
            this.button_setOutDir.Click += new System.EventHandler(this.button_setOutDir_Click);
            // 
            // textBox_localEphemerisDir
            // 
            this.textBox_localEphemerisDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_localEphemerisDir.Location = new System.Drawing.Point(96, 209);
            this.textBox_localEphemerisDir.Name = "textBox_localEphemerisDir";
            this.textBox_localEphemerisDir.Size = new System.Drawing.Size(485, 21);
            this.textBox_localEphemerisDir.TabIndex = 12;
            // 
            // textBox_dataBaseDir
            // 
            this.textBox_dataBaseDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dataBaseDir.Location = new System.Drawing.Point(96, 179);
            this.textBox_dataBaseDir.Name = "textBox_dataBaseDir";
            this.textBox_dataBaseDir.Size = new System.Drawing.Size(485, 21);
            this.textBox_dataBaseDir.TabIndex = 10;
            // 
            // textBox_outDir
            // 
            this.textBox_outDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_outDir.Location = new System.Drawing.Point(96, 154);
            this.textBox_outDir.Name = "textBox_outDir";
            this.textBox_outDir.Size = new System.Drawing.Size(485, 21);
            this.textBox_outDir.TabIndex = 8;
            this.textBox_outDir.Text = "C:\\GnsserOutput";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "XML,TXT|*.xml;*.txt|Rinex,Sinex|*.*O;*.*n;*.Sp3;*.rnx|所有文件|*.*";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button_localEphemerisDir);
            this.groupBox3.Controls.Add(this.button_setDcbDir);
            this.groupBox3.Controls.Add(this.button_setDataPath);
            this.groupBox3.Controls.Add(this.textBox_dcbDir);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.button_satExclude);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.textBox_satState);
            this.groupBox3.Controls.Add(this.button_setOutDir);
            this.groupBox3.Controls.Add(this.button_satState);
            this.groupBox3.Controls.Add(this.textBox_localEphemerisDir);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBox_dataBaseDir);
            this.groupBox3.Controls.Add(this.button_ocean);
            this.groupBox3.Controls.Add(this.textBox_outDir);
            this.groupBox3.Controls.Add(this.textBox_antennaFile);
            this.groupBox3.Controls.Add(this.button_setAntennaFile);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBox_satExclude);
            this.groupBox3.Controls.Add(this.textBox_ocean);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(11, 11);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(635, 246);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "通用数据源路径";
            // 
            // button_setDcbDir
            // 
            this.button_setDcbDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setDcbDir.Location = new System.Drawing.Point(588, 122);
            this.button_setDcbDir.Name = "button_setDcbDir";
            this.button_setDcbDir.Size = new System.Drawing.Size(40, 23);
            this.button_setDcbDir.TabIndex = 9;
            this.button_setDcbDir.Text = "...";
            this.button_setDcbDir.UseVisualStyleBackColor = true;
            this.button_setDcbDir.Click += new System.EventHandler(this.button_setDcbDir_Click);
            // 
            // textBox_dcbDir
            // 
            this.textBox_dcbDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dcbDir.Location = new System.Drawing.Point(96, 122);
            this.textBox_dcbDir.Name = "textBox_dcbDir";
            this.textBox_dcbDir.Size = new System.Drawing.Size(485, 21);
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
            this.button_satExclude.Location = new System.Drawing.Point(588, 95);
            this.button_satExclude.Name = "button_satExclude";
            this.button_satExclude.Size = new System.Drawing.Size(40, 23);
            this.button_satExclude.TabIndex = 7;
            this.button_satExclude.Text = "...";
            this.button_satExclude.UseVisualStyleBackColor = true;
            this.button_satExclude.Click += new System.EventHandler(this.button_satExclude_Click);
            // 
            // textBox_satState
            // 
            this.textBox_satState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_satState.Location = new System.Drawing.Point(96, 68);
            this.textBox_satState.Name = "textBox_satState";
            this.textBox_satState.Size = new System.Drawing.Size(485, 21);
            this.textBox_satState.TabIndex = 4;
            // 
            // button_satState
            // 
            this.button_satState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_satState.Location = new System.Drawing.Point(588, 69);
            this.button_satState.Name = "button_satState";
            this.button_satState.Size = new System.Drawing.Size(40, 23);
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
            this.button_ocean.Location = new System.Drawing.Point(588, 42);
            this.button_ocean.Name = "button_ocean";
            this.button_ocean.Size = new System.Drawing.Size(40, 23);
            this.button_ocean.TabIndex = 3;
            this.button_ocean.Text = "...";
            this.button_ocean.UseVisualStyleBackColor = true;
            this.button_ocean.Click += new System.EventHandler(this.button_ocean_Click);
            // 
            // textBox_antennaFile
            // 
            this.textBox_antennaFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_antennaFile.Location = new System.Drawing.Point(96, 14);
            this.textBox_antennaFile.Name = "textBox_antennaFile";
            this.textBox_antennaFile.Size = new System.Drawing.Size(485, 21);
            this.textBox_antennaFile.TabIndex = 0;
            // 
            // button_setAntennaFile
            // 
            this.button_setAntennaFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setAntennaFile.Location = new System.Drawing.Point(588, 12);
            this.button_setAntennaFile.Name = "button_setAntennaFile";
            this.button_setAntennaFile.Size = new System.Drawing.Size(40, 23);
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
            this.textBox_satExclude.Location = new System.Drawing.Point(96, 95);
            this.textBox_satExclude.Name = "textBox_satExclude";
            this.textBox_satExclude.Size = new System.Drawing.Size(485, 21);
            this.textBox_satExclude.TabIndex = 6;
            // 
            // textBox_ocean
            // 
            this.textBox_ocean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ocean.Location = new System.Drawing.Point(96, 41);
            this.textBox_ocean.Name = "textBox_ocean";
            this.textBox_ocean.Size = new System.Drawing.Size(485, 21);
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
            // button_reset
            // 
            this.button_reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_reset.Location = new System.Drawing.Point(571, 262);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 36);
            this.button_reset.TabIndex = 5;
            this.button_reset.Text = "重置";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(490, 262);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 36);
            this.button_save.TabIndex = 5;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // ComonSourceSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 310);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.groupBox3);
            this.Name = "ComonSourceSettingForm";
            this.Text = "文件地址设置";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
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
        private System.Windows.Forms.Button button_setDataPath;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox_dataBaseDir;
        private System.Windows.Forms.Button button_localEphemerisDir;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox_localEphemerisDir;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Button button_save;
    }
}